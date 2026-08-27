// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Collections.HashSet
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    interface IHashContainer<out T>
    {
        int Hash
        {
            get;
        }

        T Key
        {
            get;
        }

        bool IsValid
        {
            get;
        }
    }
}