// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class ValueTupleExtension
    {

        public static bool TryBorrow<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7,
            Policy7, T8, Policy8>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> instances, Policy1 policy1,
            Policy2 policy2, Policy3 policy3, Policy4 policy4, Policy5 policy5, Policy6 policy6, Policy7 policy7,
            Policy8 policy8)
            where T1 : class, IOwnable<T1>
            where Policy1 : struct, IAccessPolicy
            where T2 : class, IOwnable<T2>
            where Policy2 : struct, IAccessPolicy
            where T3 : class, IOwnable<T3>
            where Policy3 : struct, IAccessPolicy
            where T4 : class, IOwnable<T4>
            where Policy4 : struct, IAccessPolicy
            where T5 : class, IOwnable<T5>
            where Policy5 : struct, IAccessPolicy
            where T6 : class, IOwnable<T6>
            where Policy6 : struct, IAccessPolicy
            where T7 : class, IOwnable<T7>
            where Policy7 : struct, IAccessPolicy
            where T8 : class, IOwnable<T8>
            where Policy8 : struct, IAccessPolicy
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;
            ScopedReference<T6, Policy6> ref6 = default;
            ScopedReference<T7, Policy7> ref7 = default;
            ScopedReference<T8, Policy8> ref8 = default;

            if (!instances.Item1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item5.TryBorrow(out ref5))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item6.TryBorrow(out ref6))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item7.TryBorrow(out ref7))
            {
                success = false;
                goto Finalize;
            }
            if (!instances.Item8.TryBorrow(out ref8))
            {
                success = false;
            }

        Finalize:
            if (success)
            {

            }
            else
            {
                ref8.Dispose();
                ref7.Dispose();
                ref6.Dispose();
                ref5.Dispose();
                ref4.Dispose();
                ref3.Dispose();
                ref2.Dispose();
                ref1.Dispose();
            }
            return success;
        }
    }
}