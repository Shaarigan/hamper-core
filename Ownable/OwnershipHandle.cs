// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.InteropServices;

namespace Soe.Threading
{
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    struct OwnershipHandle
    {
        [FieldOffset(0)]
        public int OwningThreadId;
        
        [FieldOffset(4)]
        public UInt32 SynchronizationBarrierState;
    }
}