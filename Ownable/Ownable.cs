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
    abstract partial class Ownable : IOwnable
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
            get { return Ownable.CheckIsOwningThread(ref handle); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Ownable()
        {
            this.handle.OwningThreadId = DefaultThread;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryTakeOwnership()
        {
            return Ownable.TryTakeOwnership(ref handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReturnOwnership()
        {
            return Ownable.ReturnOwnership(ref handle);
        }
    }
}