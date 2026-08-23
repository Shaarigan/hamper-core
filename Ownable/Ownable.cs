// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    abstract class Ownable : IOwnable<Ownable>
    {
        private OwnershipHandle handle;

        protected ref OwnershipHandle Handle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref handle; }
        }

        public bool IsOwningThread
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return handle.IsOwningThread; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryBorrow<Policy>(out ScopedReference<Ownable, Policy> reference)
            where Policy : struct, IAccessPolicy
        {
            if (default(Policy).TryAcquire(ref handle))
            {
                reference = new ScopedReference<Ownable, Policy>(this, ref handle);
                return true;
            }
            else
            {
                reference = default;
                return false;
            }
        }
    }
}