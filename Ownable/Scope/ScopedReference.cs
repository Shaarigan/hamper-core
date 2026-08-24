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
    ref struct ScopedReference<T, Policy>
        where T : class, IOwnable<T>
        where Policy : struct, IAccessPolicy
    {
        private readonly T instance;
        private readonly ref OwnershipHandle handle;
        private bool isActive;

        public T Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ScopedReference(T instance, ref OwnershipHandle handle)
        {
            this.instance = instance;
            this.handle = ref handle;
            this.isActive = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ScopedReference<T, Policy> accessor)
        {
            return accessor.isActive;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (isActive)
            {
                if (default(Policy).Return(ref handle))
                {
                    isActive = false;
                }
                else throw new ThreadOwnershipViolationException();
            }
        }
    }
}