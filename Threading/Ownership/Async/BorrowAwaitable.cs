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
    readonly struct BorrowAwaitable<T, Policy>
        where T : class
        where Policy : IAccessPolicy
    {
        private readonly T instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T instance)
        {
            this.instance = instance;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T, Policy>(instance));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2>
        where T1 : class
        where T2 : class
        where Policy1 : IAccessPolicy
        where Policy2 : IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2>(i1,  i2));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3>
        where T1 : class
        where T2 : class
        where T3 : class
        where Policy1 : IAccessPolicy
        where Policy2 : IAccessPolicy
        where Policy3 : IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(i1,  i2, i3));
        }
    }
}