// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class Entities : SparseArray
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
                entity = freeList;
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
    }
}