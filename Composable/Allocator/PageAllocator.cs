// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    /// <summary>
    /// Manages memory pages used to store entities
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    partial class PageAllocator
    {
        public const int PageSize = 4096;
        public const int BlockSize = PageSize >> 6;
        public const int BlockShift = 3;
        public const int BlockMask = (BlockSize >> BlockShift) - 1;
        public const int MaxPageCount = UInt16.MaxValue;
        
        private readonly MemoryMappedFile pages;
        private int firstFreeIndex;
        private ChunkList chunks;
        
        /// <summary>
        /// Initializes the allocator
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PageAllocator()
        {
            pages = MemoryMappedFile.CreateNew(null, PageSize * MaxPageCount, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.DelayAllocatePages, HandleInheritability.None);
            firstFreeIndex = 0;
            chunks = default;
        }
        
        /// <summary>
        /// Accesses the memory page at the given index
        /// </summary>
        /// <param name="handle">A handle pointing to a block in a memory page</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <param name="entity">The entity to write into memory</param>
        /// <exception cref="InsufficientMemoryException">The memory page was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the page</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Access(in MemoryHandle handle, int index, in EntityId entity)
        {
            index *= sizeof(UInt64);
            Span<Chunk> list = chunks.AsSpan();
            if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= PageSize)
            {
                if(list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                {
                    accessor.Write(handle.BlockIndex + index, entity);
                    
                    #if DEBUG
                    if(accessor.ReadUInt64(handle.BlockIndex + index) != entity)
                        throw new Exception();
                    #endif
                }
                else throw new InsufficientMemoryException();
            }
            else throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// Accesses the memory page at the given index
        /// </summary>
        /// <param name="handle">A handle pointing to a block in a memory page</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <returns>The entity stored a the given index</returns>
        /// <exception cref="InsufficientMemoryException">The memory page was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the page</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityId Access(in MemoryHandle handle, int index)
        {
            index *= sizeof(UInt64);
            Span<Chunk> list = chunks.AsSpan();
            if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= PageSize)
            {
                if(list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                {
                    return accessor.ReadUInt64(handle.BlockIndex + index);
                }
                else throw new InsufficientMemoryException();
            }
            else throw new IndexOutOfRangeException();
        }
        
        /// <summary>
        /// Acquires a new block of the provided size
        /// </summary>
        /// <param name="size">The minimum size of the block</param>
        /// <returns>A handle pointing to the block acquired</returns>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <remarks>Blocks are always allocated as multiples of <see cref="BlockSize"/></remarks>
        public MemoryHandle Allocate(int size)
        {
            // Map the size to power of two value
            if (size < BlockSize)
            {
                size = BlockSize;
            }
            size = size.NextPowerOfTwo();
            int blockCount = size >> 6;
            Span<Chunk> list;
            
        TryInsert:
            {
                list = chunks.AsSpan();
                for (int i = firstFreeIndex; i < list.Length; i++)
                {
                    ref Chunk chunk = ref list[i];
                    if (chunk.IsEmpty)
                    {
                        // Allocate a new page
                        chunk.Handler = pages.CreateViewAccessor(i * PageSize, PageSize);
                    }
                    // Search for the next free block in this page
                    for (int bit = GetFirstFreeIndex(chunk.FreeList); bit < 64; bit++)
                    {
                        int count = BitOperations.TrailingZeroCount(chunk.FreeList >> bit) - bit;
                        if (count >= blockCount)
                        {
                            chunk.FreeList |= ((1ul << blockCount) - 1) << bit;
                            if (firstFreeIndex == i && chunk.FreeList == UInt64.MaxValue)
                            {
                                // Increase the hint for the allocator where to look for free blocks
                                firstFreeIndex++;
                            }
                            return new MemoryHandle((UInt16)size, (UInt32)i, (UInt16)(bit * BlockSize));
                        }
                    }
                }
            }
            if (list.Length * 2 <= MaxPageCount)
            {
                // There are more pages reserved, grow and retry
                chunks.Resize(list.Length * 2);
                goto TryInsert;
            }
            else throw new OutOfMemoryException();
        }
        
        /// <summary>
        /// Returns the handle to a block in a memory page
        /// </summary>
        /// <param name="handle">A handle pointing to a block in a memory page</param>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the page</exception>
        public void Free(in MemoryHandle handle)
        {
            Span<Chunk> list = chunks.AsSpan();
            if (handle.PageIndex < list.Length && (handle.BlockIndex * BlockSize) + handle.BlockSize <= PageSize)
            {
                ref Chunk chunk = ref list[handle.PageIndex];
                chunk.FreeList &= ~(((1ul << (handle.BlockSize >> 6)) - 1) << handle.BlockIndex);

                // ReSharper disable MergeIntoPattern
                
                if (chunk.FreeList == 0 && chunk.Handler is MemoryMappedViewAccessor accessor)
                {
                    // The page is unused, discard it
                    chunk.Handler = null;
                    accessor.Dispose();
                }
                else if (handle.PageIndex < firstFreeIndex)
                {
                    // Set the hint to where to look for free chunks to this page if possible
                    firstFreeIndex = handle.PageIndex;
                }
                
                // ReSharper restore MergeIntoPattern
            }
            else throw new IndexOutOfRangeException();
        }
        
        /// <summary>
        /// Initializes the memory page from the given index with default values
        /// </summary>
        /// <param name="handle">A handle pointing to a block in a memory page</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <param name="entity">The default values to write into memory</param>
        /// <exception cref="InsufficientMemoryException">The memory page was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the page</exception>
        public void InitializeBlock(in MemoryHandle handle, int index, in EntityId entity)
        {
            int count = handle.BlockSize >> BlockShift;
            EntityId[] ids = ArrayPool<EntityId>.Shared.Rent(count);
            try
            {
                ids.AsSpan().Slice(0, count)
                    .Fill(entity);

                Span<Chunk> list = chunks.AsSpan();
                if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= PageSize)
                {
                    if (list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                    {
                        accessor.WriteArray(handle.BlockIndex + index, ids, 0, count);
                        
                        #if DEBUG
                        for (int i = handle.BlockIndex + index; i < count; i++)
                        {
                            if (accessor.ReadUInt64(i * sizeof(UInt64)) != entity)
                                throw new Exception();
                        }
                        #endif
                    }
                    else throw new InsufficientMemoryException();
                }
                else throw new IndexOutOfRangeException();
            }
            finally
            {
                ArrayPool<EntityId>.Shared.Return(ids);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetFirstFreeIndex(UInt64 freeList)
        {
            freeList = ~freeList;
            if (freeList == 0)
            {
                return -1;
            }
            else return BitOperations.TrailingZeroCount(freeList);
        }
    }
}