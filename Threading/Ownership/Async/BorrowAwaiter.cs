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
    readonly struct BorrowAwaiter : INotifyCompletion
    {
        private readonly Task<AccessManager.IAccessHandle> task;

        public bool IsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return task.IsCompleted; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter(Task<AccessManager.IAccessHandle> task)
        {
            this.task = task;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            task.GetAwaiter().OnCompleted(continuation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ScopedDisposable<AccessManager.IAccessHandle, AccessManager.ManagedAccessPolicy> GetResult()
        {
            return new ScopedDisposable<AccessManager.IAccessHandle, AccessManager.ManagedAccessPolicy>(task.Result);
        }
    }
}