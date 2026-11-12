// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Handles iteration over a collection of file system entries
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IStorageIterator : IReadOnlyIterator<string>
    {
        /// <summary>
        /// Creates a new read-only span over the current file system entry
        /// </summary>
        /// <param name="relative">Sets if the span should contain target information relative to the source</param>
        /// <returns>The newly created span over the current file system entry</returns>
        ReadOnlySpan<char> AsSpan(bool relative);
    }
}