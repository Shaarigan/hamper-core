// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;
using Soe.Collections.Inline;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        static class Dependency<T>
            where T : class
        {
            private static HashSet<object, TaskList> tasks;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Dependency()
            {
                tasks = default;
            }

            public static bool Append<Policy>(object instance, TaskNode node)
                where Policy : IAccessPolicy
            {
                return false;
            }

            public static void Remove(object instance, TaskNode node)
            {
                
            }
        }
    }
}