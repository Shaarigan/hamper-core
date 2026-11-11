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
    interface IDirectoryProvider
    {
        void Clear(ResourceIdentifier identifier, bool recursive);
        
        void Create(ResourceIdentifier target);
        
        bool Exists(ResourceIdentifier target);
        
        
        
        void Remove(ResourceIdentifier target);
    }
}