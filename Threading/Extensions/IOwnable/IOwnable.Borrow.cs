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
    static partial class IOwnableExtension
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScopedReference<T, Policy> Borrow<T, Policy>(this T instance, Policy policy)
            where T : class, IOwnable<T>
            where Policy : struct, IAccessPolicy
        {
            SpinWait wait = new SpinWait();
            for (; ;)
            {
                if (instance.TryBorrow(out ScopedReference<T, Policy> reference))
                {
                    return reference;
                }
                else wait.SpinOnce();
            }
        }
    }
}