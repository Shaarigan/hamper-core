// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    sealed partial class StorageProvider
    {
        /// <summary>
        /// Handles iteration over a collection of file system entries
        /// </summary>
        /// <param name="offset">The length of the full qualified target</param>
        /// <param name="entries">A collection of entries found at target</param>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref struct ReadOnlyResultIterator(int offset, IEnumerator<string> entries) : IReadOnlyIterator<ReadOnlySpan<char>>
        {
            /// <summary>
            /// Gets the element in the collection at the current position of this iterator
            /// </summary>
            public ReadOnlySpan<char> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    int length = offset;
                    if (entries.Current[0] == ResourceIdentifier.Character.PathSeparator || entries.Current[0] == ResourceIdentifier.Character.PathSeparatorAlternative)
                    {
                        length++;
                    }
                    return entries.Current.AsSpan(length);
                }
            }
            
            /// <summary>
            /// Advances the iterator to the next element of the collection
            /// </summary>
            /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
            public bool MoveNext()
            {
                return entries.MoveNext();
            }

            /// <summary>
            /// Sets the iterator to its initial position
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                entries.Reset();
            }
        }
    }
}