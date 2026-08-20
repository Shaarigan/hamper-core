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
    abstract partial class Ownable
    {
        private const int DefaultThread = -1;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckIsOwningThread(ref OwnershipHandle handle)
        {
            return (Volatile.Read(ref handle.OwningThreadId) == Thread.CurrentThread.ManagedThreadId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetImmutableAccess(ref OwnershipHandle handle)
        {
            return SynchronizationBarrier.TryBeginSharedOperation(ref handle.SynchronizationBarrierState);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMutableAccess(ref OwnershipHandle handle)
        {
            if (CheckIsOwningThread(ref handle))
            {
                return TryGetMutableAccessUnsafe(ref handle);
            }
            else throw new ThreadOwnershipViolationException();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMutableAccessUnsafe(ref OwnershipHandle handle)
        {
            return SynchronizationBarrier.TryBeginExclusiveOperation(ref handle.SynchronizationBarrierState);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryTakeOwnership(ref  OwnershipHandle handle)
        {
            return (Interlocked.CompareExchange(ref handle.OwningThreadId, Thread.CurrentThread.ManagedThreadId, DefaultThread) == DefaultThread);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReturnImmutableAccess(ref OwnershipHandle handle)
        {
            SynchronizationBarrier.EndSharedOperation(ref handle.SynchronizationBarrierState);
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReturnMutableAccess(ref OwnershipHandle handle)
        {
            if (CheckIsOwningThread(ref handle))
            {
                SynchronizationBarrier.EndExclusiveOperation(ref handle.SynchronizationBarrierState);
                return true;
            }
            else return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReturnOwnership(ref OwnershipHandle handle)
        {
            if (CheckIsOwningThread(ref handle))
            {
                Volatile.Write(ref handle.OwningThreadId, DefaultThread);
                return true;
            }
            else return false;
        }
    }
}