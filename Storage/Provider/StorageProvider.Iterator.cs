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
        public readonly ref struct StorageIterator(int offset, IEnumerator<string> entries) : IStorageIterator
        {
            /// <inheritdoc/>
            public string Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return entries.Current; }
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<char> AsSpan(bool relative = false)
            {
                if(relative)
                {
                    ReadOnlySpan<char> result = Current.AsSpan(offset);
                    if (Current[0] == ResourceIdentifier.Character.PathSeparator || Current[0] == ResourceIdentifier.Character.PathSeparatorAlternative)
                    {
                        result = result.Slice(1);
                    }
                    return result;
                }
                else return Current;
            }
            
            /// <inheritdoc/>
            public bool MoveNext()
            {
                return entries.MoveNext();
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                entries.Reset();
            }
        }
    }
}