// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Represents a storage provider over the default file system
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    sealed partial class StorageProvider : IStorageProvider<StorageProvider.DirectoryProvider, StorageProvider.FileProvider, StorageProvider.ReadOnlyResultIterator>
    {
        private static readonly StorageProvider instance;
        /// <summary>
        /// Gets the default instance of this storage provider
        /// </summary>
        public static StorageProvider Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        private readonly DirectoryProvider directory;
        /// <inheritdoc/>
        public DirectoryProvider Directory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return directory; }
        }

        /// <inheritdoc/>
        public FileProvider File
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { throw new NotImplementedException(); }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static StorageProvider()
        {
            instance = new StorageProvider();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StorageProvider()
        {
            this.directory = default;
        }
    }
}