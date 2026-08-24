// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections;
using System.Collections.Generic;
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
    class Entities : SparseArray<Entities>, IEnumerable<EntityId>, IOwnable<Entities>
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
            if (IsOwningThread)
            {
                Ref<MemoryHandle> handle;
                EntityId entity;

                if (freeList == EntityId.Invalid)
                {
                    // Create new entity from current max entity ID
                    entity = new EntityId(maxID++, 0, SHARD, EntityFlags.None);
                    int slot = entity.Index >> PageAllocator.BlockShift;
                    if (!Find(slot, out handle))
                    {
                        // Protect structural change
                        Handle.GetExclusiveAccessUnsafe();
                        try
                        {
                            // Entity does not exist, add it to the sparse array
                            ref MemoryHandle tmp = ref Emplace(slot, Version);
                            if (!tmp.IsValid)
                            {
                                // Block is uninitialized, we must initialize it first to prevent false positives
                                tmp = allocator.Allocate(PageAllocator.BlockSize);
                                allocator.InitializeBlock(tmp, 0, EntityId.Invalid);
                                handle = new Ref<MemoryHandle>(ref tmp);
                            }
                        }
                        finally
                        {
                            Handle.ReturnExclusiveAccessUnsafe();
                        }
                    }
                }
                else
                {
                    // Use recyclable entity
                    entity = new EntityId(freeList.Index, freeList.Version + 1, freeList.Shard, EntityFlags.None);
                    int slot = entity.Index >> PageAllocator.BlockShift;
                    if (Find(slot, out handle))
                    {
                        // Swap the recyclable entity with whatever is stored at its slot in the sparse array
                        freeList = allocator.Access(handle.Value, freeList.Index & PageAllocator.BlockMask);
                    }
                    else throw new AccessViolationException();
                }

                int index = entities.Count;
                
                // Protect structural change
                Handle.GetExclusiveAccessUnsafe();
                try
                {
                    entities.Add(entity);
                }
                finally
                {
                    Handle.ReturnExclusiveAccessUnsafe();
                }

                // Write a modified version of entity to its slot in the sparse array so entity.Index -> dense index
                allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask, new EntityId(index, entity.Version, entity.Shard, entity.Flags));
                return entity;
            }
            else throw new ThreadOwnershipViolationException();
        }

        public bool Dispose(EntityId entity)
        {
            if (IsOwningThread)
            {
                if (Find(entity.Index, out Ref<MemoryHandle> handle))
                {
                    // Check if entity is alive
                    EntityId entityPtr = allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask);
                    if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                    {
                        allocator.Access(handle.Value, entity.Index & PageAllocator.BlockMask, freeList);
                        freeList = new EntityId(entity.Index, entity.Version, entity.Shard, EntityFlags.Reserved);

                        // Protect structural change
                        try
                        {
                            if (entityPtr.Index < Count - 1)
                            {
                                // Swap entity data with last entity
                                EntityId swap = entities[Count - 1];
                                if (Find(swap.Index, out handle))
                                {
                                    EntityId tmp = allocator.Access(handle.Value, swap.Index & PageAllocator.BlockMask);
                                    allocator.Access(handle.Value, swap.Index & PageAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.Shard, tmp.Flags));

                                    Handle.GetExclusiveAccessUnsafe();
                                    Swap(entityPtr.Index, tmp.Index);
                                }
                                else throw new AccessViolationException();
                            }
                            else Handle.GetExclusiveAccessUnsafe();
                            entities.RemoveAt(Count - 1);
                        }
                        finally
                        {
                            Handle.ReturnExclusiveAccessUnsafe();
                        }
                        return true;
                    }
                }
                return false;
            }
            else throw new ThreadOwnershipViolationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(int oldIndex, int newIndex)
        {
            (entities[oldIndex], entities[newIndex]) = (entities[newIndex], entities[oldIndex]);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool TryBorrow<Policy>(out ScopedReference<Entities, Policy> reference)
        {
            if (default(Policy).TryAcquire(ref Handle))
            {
                reference = new ScopedReference<Entities, Policy>(this, ref Handle);
                return true;
            }
            else
            {
                reference = default;
                return false;
            }
        }
        
        public bool TryGet(EntityId entity, out EntityId result)
        {
            bool needsDispose = IsOwningThread;
            if (needsDispose)
            {
                Handle.GetShredAccessUnsafe();
            }
            try
            {
                if (Find(entity.Index, out Ref<MemoryHandle> handle))
                {
                    // Check if entity is alive
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
            finally
            {
                if (needsDispose)
                {
                    Handle.ReturnSharedAccessUnsafe();
                }
            }
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