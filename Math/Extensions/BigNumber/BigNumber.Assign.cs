// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Utility
{
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    static partial class BigNumberExtension
    {
        /// <summary>
        /// Sets the value of this integer to zero
        /// </summary>
        /// <param name="index">The index of the segment to begin assigning</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssignZero<BigNumber>(this BigNumber a, int index)
            where BigNumber : struct, IBigNumber
        {
            BigMath.AssignZero(a.AsSpan(), index, a.SegmentCount);
        }
        /// <summary>
        /// Sets the value of this integer to zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssignZero<BigNumber>(this BigNumber a)
            where BigNumber : struct, IBigNumber
        {
            BigMath.AssignZero(a.AsSpan(), a.SegmentCount);
        }
        
        /// <summary>
        /// Assigns b to a
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assign<BigNumber>(this BigNumber a, BigNumber b)
        {
            
        }
        /// <summary>
        /// Assigns b to a
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assign<BigNumber>(this BigNumber a, int aIndex, BigNumber b, int bIndex)
        {
            
        }
    }
}