// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Ownable
{
    #if EXPORT_HAMPER_CORE_OWNABLE
    public
    #else
    internal
    #endif
    interface IOwnable
    {
        bool IsOwner
        {
            get;
        }
        
        bool TryTakeOwnership();
        
        void ReturnOwnership();
    }
}