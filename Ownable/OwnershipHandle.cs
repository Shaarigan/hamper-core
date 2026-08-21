// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

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
        private const int DefaultThread = -1;
        
        [FieldOffset(0)]
        private int owningThreadId;

        [FieldOffset(4)]
        private UInt32 synchronizationBarrierState;

        public bool IsOwningThread 
        {
            get { return (Volatile.Read(ref owningThreadId) == Thread.CurrentThread.ManagedThreadId); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnershipHandle()
        {
            this.owningThreadId = DefaultThread;
            this.synchronizationBarrierState = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetShredAccess()
        {
            return SynchronizationBarrier.TryBeginSharedOperation(ref synchronizationBarrierState);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetExclusiveAccess()
        {
            return SynchronizationBarrier.TryBeginExclusiveOperation(ref synchronizationBarrierState);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryTakeOwnership(ref  OwnershipHandle handle)
        {
            return (Interlocked.CompareExchange(ref owningThreadId, Thread.CurrentThread.ManagedThreadId, DefaultThread) == DefaultThread);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnSharedAccess()
        {
            SynchronizationBarrier.EndSharedOperation(ref synchronizationBarrierState);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnExclusiveAccess()
        {
            SynchronizationBarrier.EndExclusiveOperation(ref synchronizationBarrierState);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnOwnership()
        {
            Volatile.Write(ref owningThreadId, DefaultThread);
        }
    }
}