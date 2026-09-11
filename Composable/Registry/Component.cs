// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Collections;
using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;
using Soe.Threading;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class Component<T> : SparseMap, IEnumerable<T>
        where T : struct
    {
        private readonly Shard shard;

        private EmbeddedList<EntityId> entities;
        private EmbeddedList<T> components;
        
        public new int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Requires at least immutable access when scheduled
                AccessManager.ThrowOnLessAccessible<Component<T>>(AccessType.Immutable);
                
                return components.Count;
            }
        }

        public ReadOnlySpan<EntityId> Entities
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Requires at least immutable access when scheduled
                AccessManager.ThrowOnLessAccessible<Component<T>>(AccessType.Immutable);
                
                return entities.AsReadOnlySpan();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Component(Shard shard)
        {
            this.shard = shard;
            this.entities = default;
            this.components = default;
        }

        public ref T Add(EntityId entity)
        {
            if(entity.Shard == shard.Id)
            {
                // Requires mutable access when scheduled
                AccessManager.ThrowOnAccessViolation<Component<T>>(AccessType.Mutable);
                IMemoryAllocator allocator = shard;
                
                int slot = entity.Index >> MemoryAllocator.BlockShift;
                if (!Find(slot, out int index, out int distance, out Ref<MemoryHandle> handle))
                {
                    // Entity slot is new to this container, add it
                    ref MemoryHandle tmp = ref Emplace(slot, index, distance, Version);
                    if (!tmp.IsValid)
                    {
                        // Block is uninitialized, initialize it to prevent false positives
                        tmp = allocator.Allocate(MemoryAllocator.BlockSize);
                        allocator.InitializeBlock(tmp, 0, EntityId.Invalid);
                        
                        handle = new Ref<MemoryHandle>(ref tmp);
                    }
                }

                // Look the entity up in the sparse map
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    // Entity exists and is alive
                    index = entityPtr.Index;
                }
                else
                {
                    // Entity is new to this component, add it
                    index = components.Count;
                    components.Add(default);
                    entities.Add(entity);

                    // Write a modified version of entity to its slot in the sparse map so entity.Index -> dense index
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(index, entity.Version, entity.Shard, entity.Flags));
                }
                return ref components[index];
            }
            else throw new ArgumentOutOfRangeException(nameof(entity.Shard));
        }

        public bool Remove(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(AccessType.Mutable);
            
            if (Find(entity.Index, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    // Mark component as removed by adding the reserved flag
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, entityPtr.Version, entityPtr.Shard, EntityFlags.Reserved));
                    if (entityPtr.Index < Count - 1)
                    {
                        // Swap entity data with last entity
                        EntityId swap = entities[Count - 1];
                        if (Find(swap.Index, out _, out _, out handle))
                        {
                            EntityId tmp = allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask);
                            allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.Shard, tmp.Flags));

                            Swap(entityPtr.Index, tmp.Index);
                        }
                        else throw new AccessViolationException();
                    }

                    int index = Count - 1;
                    components.RemoveAt(index);
                    entities.RemoveAt(index);

                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(int oldIndex, int newIndex)
        {
            (components[oldIndex], components[newIndex]) = (components[newIndex], components[oldIndex]);
            (entities[oldIndex], entities[newIndex]) = (entities[newIndex], entities[oldIndex]);
        }
        
        public bool TryGet(EntityId entity, out Ref<T> result)
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(AccessType.Immutable);
            
            if (Find(entity.Index, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if the entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    result = new Ref<T>(ref components[entityPtr.Index]);
                    return true;
                }
            }

            result = Ref<T>.CreateEmpty();
            return false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator()
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(AccessType.Immutable);
            
            return components.GetEnumerator();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}