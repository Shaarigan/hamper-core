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
        where TIterator : IStorageIterator, allows ref struct
    {
        void Clear(ReadOnlySpan<char> target, bool recursive);
        
        void Create(ReadOnlySpan<char> target);
        
        bool Exists(ReadOnlySpan<char> target);
        
        #region GetDirectories
        TIterator GetDirectories(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char, char> target);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char, char> target);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char, char> target);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char, char> target);
        TIterator GetDirectories(ReadOnlySpanTuple<char, char> target);
        TIterator GetDirectories(ReadOnlySpan<char> target);
        #endregion
        
        #region GetFiles
        TIterator GetFiles(ReadOnlySpan<char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char, char> target, string searchPattern, SearchOption searchOptions);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char, char> target);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char, char> target);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char, char> target);
        TIterator GetFiles(ReadOnlySpanTuple<char, char, char> target);
        TIterator GetFiles(ReadOnlySpanTuple<char, char> target);
        TIterator GetFiles(ReadOnlySpan<char> target);
        #endregion
        
        void Remove(ReadOnlySpan<char> target);
    }
}