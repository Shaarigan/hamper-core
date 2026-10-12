// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Utility
{
    [InlineArray(2)]
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    struct UInt128 : IBigNumber
    {
        private const int Capacity = 2;
        
        private UInt64 element0;

        /// <inheritdoc/>
        public int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Capacity; }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<UInt64> AsSpan()
        {
            return MemoryMarshal.CreateSpan(ref element0, Capacity);
        }
    }
}