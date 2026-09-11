// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Collections.HashSet
{
    /// <summary>
    /// Stores a set of <typeparamref name="T"/> via Robin Hood hash algorithm
    /// </summary>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    static partial class HashSet
    {
        /// <summary>
        /// A default element for this <see cref="HashSet{T,Container}"/>
        /// </summary>
        /// <param name="hash">The hash code of this element</param>
        /// <param name="key">The key of this element</param>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly struct DefaultContainer<T>(int hash, T key) : IHashContainer<T>
        {
            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            /// <inheritdoc/>
            public T Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (hash != 0 && key != null); }
            }
        }
    }
}