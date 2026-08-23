// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct MutableScope : IMutablePolicy
    {
        public bool TryAcquire(ref OwnershipHandle handle)
        {
            return handle.TryTakeOwnership();
        }

        public bool Return(ref OwnershipHandle handle)
        {
            throw new System.NotImplementedException();
        }
    }
}