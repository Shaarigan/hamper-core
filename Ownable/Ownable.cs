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
    abstract class Ownable<T> : IOwnable<T>
        where T : Ownable<T>
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
        protected Ownable()
        {
            this.handle = new OwnershipHandle();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract bool TryBorrow<Policy>(out ScopedReference<T, Policy> reference)
            where Policy : struct, IAccessPolicy;
    }
}