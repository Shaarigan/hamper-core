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
    ref partial struct MutableAccessor<T>
    {
        public ref struct LockHandle
        {
            private readonly ref OwnershipHandle handle;
            private bool isDisposed;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public LockHandle(ref OwnershipHandle handle)
            {
                this.handle = handle;
                this.isDisposed = false;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                if (!isDisposed)
                {
                    if (handle.IsOwningThread)
                    {
                        handle.ReturnExclusiveAccess();
                        isDisposed = true;
                    }
                    else throw new ThreadOwnershipViolationException();
                }
            }
        }
    }
}