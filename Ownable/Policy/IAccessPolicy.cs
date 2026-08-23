// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IAccessPolicy
    {
        bool TryAcquire(ref OwnershipHandle handle);
        
        bool Return(ref OwnershipHandle handle);
    }
}