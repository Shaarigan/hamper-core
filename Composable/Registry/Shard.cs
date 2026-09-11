// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;
using Soe.Threading;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    partial class Shard : IMemoryAllocator
    {
        static int NextShardId = 0;
        
        private readonly IMemoryAllocator allocator;
        private HashSet<Type, ComponentContainer> components;
        
        private readonly Entities entities;

        public Entities Entities
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return entities; }
        }
        
        private readonly int id;

        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return id; }
        }
        
        public Shard(IMemoryAllocator allocator)
        {
            this.id = Interlocked.Increment(ref NextShardId) - 1;
            
            this.allocator = allocator;
            this.components = default;
            this.entities = new Entities(this);
        }

        
        
        #region IMemoryAllocator implementation
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.Access(in MemoryHandle handle, int index, in EntityId entity)
        {
            allocator.Access(in handle, index, in entity);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EntityId IMemoryAllocator.Access(in MemoryHandle handle, int index)
        {
            return allocator.Access(in handle, index);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        MemoryHandle IMemoryAllocator.Allocate(int size)
        {
            return allocator.Allocate(size);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.Free(in MemoryHandle handle)
        {
            allocator.Free(in handle);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.InitializeBlock(in MemoryHandle handle, int index, in EntityId entity)
        {
            allocator.InitializeBlock(in handle, index, in entity);
        }
        #endregion
    }
}