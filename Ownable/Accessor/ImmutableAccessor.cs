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
    ref struct ImmutableAccessor<T>
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
        internal ImmutableAccessor(T instance, ref OwnershipHandle handle)
        {
            this.instance = instance;
            this.handle = ref handle;
            this.isDisposed = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ImmutableAccessor<T> accessor)
        {
            return accessor.isDisposed;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!isDisposed)
            {
                handle.ReturnSharedAccess();
                isDisposed = true;
            }
        }
    }
}