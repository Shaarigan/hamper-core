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
        where Policy : struct, IAccessPolicy
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
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
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
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
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
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(i1, i2, i3));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;
        private readonly T4 i4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3, T4 i4)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.i4 = i4;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(i1, i2, i3, i4));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;
        private readonly T4 i4;
        private readonly T5 i5;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.i4 = i4;
            this.i5 = i5;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(i1, i2, i3, i4, i5));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;
        private readonly T4 i4;
        private readonly T5 i5;
        private readonly T6 i6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.i4 = i4;
            this.i5 = i5;
            this.i6 = i6;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(i1, i2, i3, i4, i5, i6));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
        where Policy7 : struct, IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;
        private readonly T4 i4;
        private readonly T5 i5;
        private readonly T6 i6;
        private readonly T7 i7;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.i4 = i4;
            this.i5 = i5;
            this.i6 = i6;
            this.i7 = i7;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(i1, i2, i3, i4, i5, i6, i7));
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
        where Policy7 : struct, IAccessPolicy
        where Policy8 : struct, IAccessPolicy
    {
        private readonly T1 i1;
        private readonly T2 i2;
        private readonly T3 i3;
        private readonly T4 i4;
        private readonly T5 i5;
        private readonly T6 i6;
        private readonly T7 i7;
        private readonly T8 i8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaitable(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.i4 = i4;
            this.i5 = i5;
            this.i6 = i6;
            this.i7 = i7;
            this.i8 = i8;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(i1, i2, i3, i4, i5, i6, i7, i8));
        }
    }
}