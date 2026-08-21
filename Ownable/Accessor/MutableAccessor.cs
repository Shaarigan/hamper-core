// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref partial struct MutableAccessor<T>
        where T : class, IOwnable
    {
        private readonly T instance;
        private readonly ref OwnershipHandle handle;
        private bool isDisposed;

        public T Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal MutableAccessor(T instance, ref OwnershipHandle handle)
        {
            this.instance = instance;
            this.handle = ref handle;
            this.isDisposed = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(MutableAccessor<T> accessor)
        {
            return accessor.isDisposed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LockHandle Lock()
        {
            if (!isDisposed)
            {
                if (handle.IsOwningThread)
                {
                    SpinWait wait = new SpinWait();
                    while (!handle.TryGetExclusiveAccess())
                    {
                        wait.SpinOnce();
                    }
                    return new LockHandle(ref handle);
                }
                else throw new ThreadOwnershipViolationException();
            }
            else throw new ObjectDisposedException(nameof(MutableAccessor<T>));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!isDisposed)
            {
                if (handle.IsOwningThread)
                {
                    handle.ReturnOwnership();
                    isDisposed = true;
                }
                else throw new ThreadOwnershipViolationException();
            }
        }
    }
}