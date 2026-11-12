// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.IO;
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
        public struct DirectoryProvider : IDirectoryProvider<StorageIterator>
        {
            public void Clear(ReadOnlySpan<char> target, bool recursive)
            {
                throw new NotImplementedException();
            }

            public void Create(ReadOnlySpan<char> target)
            {
                throw new NotImplementedException();
            }

            public bool Exists(ReadOnlySpan<char> target)
            {
                throw new NotImplementedException();
            }

            #region GetDirectories
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            StorageIterator GetDirectories(string target, string searchPattern, SearchOption searchOptions)
            {
                return new StorageIterator(target.Length, System.IO.Directory.EnumerateDirectories
                (
                    target,
                    searchPattern,
                    searchOptions

                ).GetEnumerator());
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char, char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char, char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpanTuple<char, char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetDirectories(ReadOnlySpan<char> target)
            {
                return GetDirectories(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            StorageIterator GetDirectories(string target)
            {
                return new StorageIterator(target.Length, System.IO.Directory.EnumerateDirectories
                (
                    target

                ).GetEnumerator());
            }
            #endregion
            
            #region GetFiles
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char, char> target, string searchPattern, SearchOption searchOptions)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath), searchPattern, searchOptions);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            StorageIterator GetFiles(string target, string searchPattern, SearchOption searchOptions)
            {
                return new StorageIterator(target.Length, System.IO.Directory.EnumerateFiles
                (
                    target,
                    searchPattern,
                    searchOptions

                ).GetEnumerator());
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char, char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char, char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char, char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpanTuple<char, char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public StorageIterator GetFiles(ReadOnlySpan<char> target)
            {
                return GetFiles(ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath));
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            StorageIterator GetFiles(string target)
            {
                return new StorageIterator(target.Length, System.IO.Directory.EnumerateFiles
                (
                    target

                ).GetEnumerator());
            }
            #endregion
            
            public void Remove(ReadOnlySpan<char> target)
            {
                throw new NotImplementedException();
            }
        }
    }
}