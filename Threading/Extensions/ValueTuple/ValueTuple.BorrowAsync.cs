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
            where T1 : class, IOwnable<T1>
            where T2 : class, IOwnable<T2>
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T1, Policy1, T2, Policy2>(instances.Item1, instances.Item2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2> BorrowAsync<T1, Policy1, T2, Policy2>(
            this ValueTuple<T1, T2> instances, Policy1 policy1, Policy2 policy2)
            where T1 : class, IOwnable<T1>
            where T2 : class, IOwnable<T2>
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            return BorrowAsync<T1, Policy1, T2, Policy2>(instances);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(this ValueTuple<T1, T2, T3> instances)
            where T1 : class, IOwnable<T1>
            where T2 : class, IOwnable<T2>
            where T3 : class, IOwnable<T3>
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3>(instances.Item1, instances.Item2, instances.Item3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3> BorrowAsync<T1, Policy1, T2, Policy2, T3,
            Policy3>(this ValueTuple<T1, T2, T3> instances, Policy1 policy1, Policy2 policy2, Policy3 policy3)
            where T1 : class, IOwnable<T1>
            where T2 : class, IOwnable<T2>
            where T3 : class, IOwnable<T3>
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            return BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(instances);
        }
    }
}