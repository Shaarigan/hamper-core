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
    static partial class ValueTupleExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2> BorrowAsync<T1, Policy1, T2, Policy2>(this ValueTuple<T1, T2> instances)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T1, Policy1, T2, Policy2>(instances.Item1, instances.Item2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2> BorrowAsync<T1, Policy1, T2, Policy2>(this ValueTuple<T1, T2> instances, Policy1 policy1, Policy2 policy2)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            return BorrowAsync<T1, Policy1, T2, Policy2>(instances);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(this ValueTuple<T1, T2, T3> instances)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3>(instances.Item1, instances.Item2, instances.Item3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(this ValueTuple<T1, T2, T3> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(instances);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(this ValueTuple<T1, T2, T3, T4> instances)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(instances.Item1, instances.Item2, instances.Item3,  instances.Item4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(this ValueTuple<T1, T2, T3, T4> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3, Policy4 policy4)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
        {
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(instances);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(this ValueTuple<T1, T2, T3, T4, T5> instances)
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
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(instances.Item1, instances.Item2, instances.Item3,  instances.Item4, instances.Item5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(this ValueTuple<T1, T2, T3, T4, T5> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3, Policy4 policy4, Policy5 policy5)
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
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(instances);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(this ValueTuple<T1, T2, T3, T4, T5, T6> instances)
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
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(instances.Item1, instances.Item2, instances.Item3,  instances.Item4, instances.Item5, instances.Item6);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(this ValueTuple<T1, T2, T3, T4, T5, T6> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3, Policy4 policy4, Policy5 policy5, Policy6 policy6)
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
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(instances);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> instances)
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
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(instances.Item1, instances.Item2, instances.Item3,  instances.Item4, instances.Item5, instances.Item6, instances.Item7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3, Policy4 policy4, Policy5 policy5, Policy6 policy6, Policy7 policy7)
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
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(instances);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> instances)
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
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(instances.Item1, instances.Item2, instances.Item3,  instances.Item4, instances.Item5, instances.Item6, instances.Item7, instances.Item8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3, Policy4 policy4, Policy5 policy5, Policy6 policy6, Policy7 policy7, Policy8 policy8)
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
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(instances);
        }
    }
}