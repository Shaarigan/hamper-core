// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Inline
{
    internal static class SpanHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(Span<T> span, in T item)
            where T : class?
        {
            for (int length = span.Length, i = 0; i < length; i++)
            {
                if (ReferenceEquals(span[i], item))
                    return i;
            }
            return -1;
        }
    }
}