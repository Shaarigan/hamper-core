// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly ref struct MutableAccessor
    {
        private readonly ref OwnershipHandle handle;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MutableAccessor(ref OwnershipHandle handle)
        {
            this.handle = ref handle;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!Ownable.ReturnMutableAccess(ref handle))
            {
                throw new InvalidOperationException();
            }
        }
    }
}