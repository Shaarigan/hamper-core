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
            private static UInt32 lockVariable;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Dependency()
            {
                tasks = new HashSet<object, TaskList>(EqualityComparer<object>.Default);
                lockVariable = 0;
            }

            public static bool Append<Policy>(object instance, TaskNode node)
                where Policy : struct, IAccessPolicy
            {
                int hash = RuntimeHelpers.GetHashCode(instance);
                int index;
                int distance;
                int version;
                
                using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))  
                {  
                    if(tasks.Find(instance, hash, out index, out distance, out Ref<TaskList> result))  
                    {  
                        return result.Value.Append<T, Policy>(node);
                    }
                    version = tasks.Version;
                }
                using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))   
                {  
                    ref TaskList taskList = ref tasks.Emplace(instance, hash, index, distance, version);
                    if(!taskList.IsValid)
                    {
                        taskList = new TaskList(hash, instance);
                    }
                    return taskList.Append<T, Policy>(node);
                }
            }

            public static void Remove(object instance, TaskNode node)
            {
                int hash = RuntimeHelpers.GetHashCode(instance);
                using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))  
                {  
                    if(tasks.Find(instance, hash, out _, out _, out Ref<TaskList> result))
                    {
                        result.Value.Remove(node);
                    }
                }
            }
        }
    }
}