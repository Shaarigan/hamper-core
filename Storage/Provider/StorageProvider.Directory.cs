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
        public struct DirectoryProvider : IDirectoryProvider<ReadOnlyResultIterator>
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyResultIterator GetDirectories(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions)
            {
                if (target.Length > 0 && (target[0] == ResourceIdentifier.Character.PathSeparator ||
                                          target[0] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(1);
                }
                if (target.Length > 0 && (target[target.Length - 1] == ResourceIdentifier.Character.PathSeparator ||
                                          target[target.Length - 1] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(0, target.Length - 1);
                }
                return new ReadOnlyResultIterator(target.Length, System.IO.Directory.EnumerateDirectories
                (
                    ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar),
                    searchPattern,
                    searchOptions

                ).GetEnumerator());
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyResultIterator GetDirectories(ReadOnlySpan<char> target)
            {
                if (target.Length > 0 && (target[0] == ResourceIdentifier.Character.PathSeparator ||
                                          target[0] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(1);
                }
                if (target.Length > 0 && (target[target.Length - 1] == ResourceIdentifier.Character.PathSeparator ||
                                          target[target.Length - 1] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(0, target.Length - 1);
                }
                return new ReadOnlyResultIterator(target.Length, System.IO.Directory.EnumerateDirectories
                (
                    ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar)

                ).GetEnumerator());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyResultIterator GetFiles(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions)
            {
                if (target.Length > 0 && (target[0] == ResourceIdentifier.Character.PathSeparator ||
                                          target[0] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(1);
                }
                if (target.Length > 0 && (target[target.Length - 1] == ResourceIdentifier.Character.PathSeparator ||
                                          target[target.Length - 1] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(0, target.Length - 1);
                }
                return new ReadOnlyResultIterator(target.Length, System.IO.Directory.EnumerateFiles
                (
                    ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar),
                    searchPattern,
                    searchOptions

                ).GetEnumerator());
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyResultIterator GetFiles(ReadOnlySpan<char> target)
            {
                if (target.Length > 0 && (target[0] == ResourceIdentifier.Character.PathSeparator ||
                                          target[0] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(1);
                }
                if (target.Length > 0 && (target[target.Length - 1] == ResourceIdentifier.Character.PathSeparator ||
                                          target[target.Length - 1] == ResourceIdentifier.Character.PathSeparatorAlternative))
                {
                    target = target.Slice(0, target.Length - 1);
                }
                return new ReadOnlyResultIterator(target.Length, System.IO.Directory.EnumerateFiles
                (
                    ResourceIdentifier.Normalize(target, NormalizationFlags.SystemSeparatorChar)

                ).GetEnumerator());
            }

            public void Remove(ReadOnlySpan<char> target)
            {
                throw new NotImplementedException();
            }
        }
    }
}