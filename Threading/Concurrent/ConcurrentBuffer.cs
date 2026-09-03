// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    partial struct ConcurrentBuffer<T>
        where T : class
    {
        private UInt32 lockVariable;
        private UInt32 head;
        private UInt32 tail;

        public int Tail
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (int)Volatile.Read(ref tail);
            }
        }
        
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                UInt32 current = Volatile.Read(ref tail);
                return (int)(Volatile.Read(ref head) - current);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConcurrentBuffer()
        {
            this.lockVariable = 0;
            this.head = 0;
            this.tail = 0;
        }
        
        public int Enqueue<Accessor>(in Accessor array, T value)
            where Accessor : IArrayAccessor<T?>
        {
        Head:
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                if (TryEnqueue(array, value, out int index))
                {
                    return index;
                }
            }
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))
            {
                int capacityBits = array.Length - 1;
                if (head - tail < capacityBits)
                {
                    // Remove completed in the meantime, insert and return
                    Volatile.Write(ref array[(int)(head % capacityBits)], value);
                    head++;

                    return (int)(head % capacityBits);
                }
                else
                {
                    Grow(array);
                    goto Head;
                }
            }
        }

        public void Grow<Accessor>(in Accessor array)
            where Accessor : IArrayAccessor<T?>
        {
            int capacityBits = array.Length - 1;
            
            head = (UInt32)(head % capacityBits);
            tail = (UInt32)(tail % capacityBits);
            int oldCount = Count;
                    
            array.Resize((array.Length + 1).NextPowerOfTwo());
            if (tail > head)
            {
                int count = (int)(head + 1);
                int mergeIndex = (int)(tail + oldCount - count);
                        
                Span<T?> span = array.AsSpan();
                span.Slice(0, count)
                    .CopyTo(span.Slice(mergeIndex));

                head = (UInt32)(mergeIndex + count - 1);
            }
        }

        public bool TryEnqueue<Accessor>(in Accessor array, T value, out int index)
            where Accessor : IArrayAccessor<T?>
        {
            int capacityBits = array.Length - 1;
            for (;;)
            {
                UInt32 current = Volatile.Read(ref head);
                if (current - Volatile.Read(ref tail) >= capacityBits)
                {
                    // List is full
                    index = -1;
                    return false;
                }
                else if (Interlocked.CompareExchange(ref head, current + 1, current) == current)
                {
                    Volatile.Write(ref array[(int)(current & capacityBits)], value);
                    index = (int)(current & capacityBits);
                    return true;
                }
            }
        }

        public bool TryDequeue<Accessor>(in Accessor array, out T? value)
            where Accessor : IArrayAccessor<T?>
        {
            if (Count > 0)
            {
                int capacityBits = array.Length - 1;
                value = array[(int)(tail & capacityBits)];

                Interlocked.Increment(ref tail);
                return (value != null);
            }
            else
            {
                value = null;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            head = 0;
            tail = 0;
        }
    }
}