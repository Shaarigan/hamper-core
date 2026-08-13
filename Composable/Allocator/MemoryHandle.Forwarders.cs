// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    readonly partial struct MemoryHandle
    {
        /*/// <summary>
        /// Gets the entity at the given index
        /// </summary>
        /// <param name="index">The index of the entity in block range</param>
        /// <returns>The entity found at the given index</returns>
        /// <exception cref="IndexOutOfRangeException">The index was not in range of the block</exception>
        public EntityId Get(int index)
        {
            int entityIndex = index * sizeof(UInt64);
            if (entityIndex >= 0 && entityIndex < blockSize)
            {
                return PageAllocator.Access(this, entityIndex);
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Writes the entity to a memory region at the given index
        /// </summary>
        /// <param name="index">The index of the entity in block range</param>
        /// <param name="entity">The entity to write</param>
        /// <exception cref="IndexOutOfRangeException">The index was not in range of the block</exception>
        public void Set(int index, EntityId entity)
        {
            int entityIndex = index * sizeof(UInt64);
            if (entityIndex >= 0 && entityIndex < blockSize)
            {
                PageAllocator.Access(this, entityIndex, entity);
            }
            else throw new IndexOutOfRangeException();
        }*/
    }
}
