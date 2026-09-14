// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Collections.HashSet
{
    #pragma warning disable CS0693
    #pragma warning disable CS0282
    
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    partial struct HashSet<T, Container>
    {
        public struct Enumerator : IEnumerator<Container>
        {
            private readonly Container[]? buffer;
            private readonly int count;
            private int index;
            
            /// <inheritdoc/>
            public Container Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return buffer?[index] ?? default; }
            }
            /// <inheritdoc/>
            object? IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Current; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Container[]? buffer, int count)
            {
                this.buffer = buffer;
                this.count = count;
                this.index = -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            { }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (index + 1 < count)
                {
                    index++;
                    for (; index < count; index++)
                    {
                        if (buffer?[index].IsValid ?? false)
                            return true;
                    }
                    return false;
                }
                else return false;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                index = 0;
            }
        }
    }
    
    #pragma warning restore CS0282
    #pragma warning restore CS0693
}