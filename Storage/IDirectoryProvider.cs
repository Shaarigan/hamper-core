// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Represents a provider instance operating on directory data of the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IDirectoryProvider<out TIterator>
        where TIterator : IReadOnlyIterator<ReadOnlySpan<char>>, allows ref struct
    {
        void Clear(ReadOnlySpan<char> target, bool recursive);
        
        void Create(ReadOnlySpan<char> target);
        
        bool Exists(ReadOnlySpan<char> target);
        
        TIterator GetDirectories(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpan<char> target);
        
        TIterator GetFiles(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpan<char> target);
        
        void Remove(ReadOnlySpan<char> target);
    }
}