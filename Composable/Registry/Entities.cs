// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class Entities : SparseArray, IEnumerable<EntityId>
    {
        private const int SHARD = 0;

        private readonly PageAllocator allocator;
        private EntityId freeList;
        private int maxID;

        private EmbeddedList<EntityId> entities;

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Length * (PageAllocator.BlockSize >> PageAllocator.BlockShift); }
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return entities.Count; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entities(PageAllocator allocator)
        {
            this.allocator = allocator;
            this.freeList = EntityId.Invalid;
            this.maxID = 0;
            this.entities = default;
        }

        public EntityId Create()
        {
            Ref<MemoryHandle> handle;
            EntityId entity;

            if (freeList == EntityId.Invalid)
            {
                entity = new EntityId(maxID++, 0, SHARD, EntityFlags.None);
                int slot = entity.Index >> PageAllocator.BlockShift;
                if (!Find(slot, out handle))
                {
                    ref MemoryHandle tmp = ref Emplace(slot, Version);
                    if (!tmp.IsValid)
                    {
                        tmp = allocator.Allocate(PageAllocator.BlockSize);
                        allocator.InitializeBlock(tmp, 0, EntityId.Invalid);
                        handle = new Ref<MemoryHandle>(ref tmp);
                    }
                }
            }
            else
            {
                entity = new EntityId(freeList.Index, freeList.Version + 1, freeList.Shard, EntityFlags.None);
                int slot = entity.Index >> PageAllocator.BlockShift;
                if (Find(slot, out handle))
                {
                    freeList = allocator.Access(handle.Value, freeList.Index & PageAllocator.BlockMask);
                }
                else throw new AccessViolationException();
            }
            int index = entities.Count;
            entities.Add(entity);
            
            allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask, new EntityId(index, entity.Version, entity.Shard, entity.Flags));
            return entity;
        }

        public bool Dispose(EntityId entity)
        {
            if (Find(entity.Index, out Ref<MemoryHandle> handle))
            {
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask, freeList);
                    freeList = new EntityId(entity.Index, entity.Version, entity.Shard, EntityFlags.Reserved);
                    if (entityPtr.Index < Count - 1)
                    {
                        // Swap entity data with last entity
                        EntityId swap = entities[Count - 1];
                        if (Find(swap.Index, out handle))
                        {
                            EntityId tmp = allocator.Access(handle.Value, swap.Index & PageAllocator.BlockMask);
                            allocator.Access(handle.Value, swap.Index & PageAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.Shard, tmp.Flags));

                            Swap(entityPtr.Index, tmp.Index);
                        }
                        else throw new AccessViolationException();
                    }
                    entities.RemoveAt(Count - 1);
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(int oldIndex, int newIndex)
        {
            (entities[oldIndex], entities[newIndex]) = (entities[newIndex], entities[oldIndex]);
        }
        
        public bool TryGet(EntityId entity, out EntityId result)
        {
            if (Find(entity.Index, out Ref<MemoryHandle> handle))
            {
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    result = entities[entityPtr.Index];
                    return true;
                }
            }

            result = EntityId.Invalid;
            return false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<EntityId> GetEnumerator()
        {
            return entities.GetEnumerator();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}