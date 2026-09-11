// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        /// <summary>
        /// Manages the dependency graph instances for objects of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">A reference type</typeparam>
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

            /// <summary>
            /// Appends a <see cref="TaskNode"/> instance to the access graph of the provided object
            /// </summary>
            /// <param name="instance">An object instance to access</param>
            /// <param name="node">The task instance to append</param>
            /// <typeparam name="Policy">The desired access policy</typeparam>
            /// <returns>True if the node has other tasks to wait on, false otherwise</returns>
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
                SynchronizationBarrier.BeginExclusiveOperation(ref lockVariable);
                using(ScopedDisposable.Create<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))   
                {  
                    ref TaskList taskList = ref tasks.Emplace(instance, hash, index, distance, version);
                    if(!taskList.IsValid)
                    {
                        taskList = new TaskList(hash, instance);
                    }

                    // Downgrade exclusive access to shared access
                    SynchronizationBarrier.TryShiftReleaseExclusiveOperation(ref lockVariable);
                    return taskList.Append<T, Policy>(node);
                }
            }

            /// <summary>
            /// Removes the appended <see cref="TaskNode"/> instances from the access graph of the provided object
            /// </summary>
            /// <param name="instance">An object instance accessed</param>
            /// <param name="node">The corresponding task instance to remove</param>
            /// <exception cref="ArgumentNullException">Throws if instance is a null value</exception>
            public static void Remove(object? instance, TaskNode node)
            {
                if(instance != null)
                {
                    int hash = RuntimeHelpers.GetHashCode(instance);
                    using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
                    {
                        if (tasks.Find(instance, hash, out _, out _, out Ref<TaskList> result))
                        {
                            result.Value.Remove(node);
                        }
                    }
                }
                else throw new ArgumentNullException(nameof(instance));
            }

            /// <summary>
            /// Releases the provided reference if possible
            /// </summary>
            /// <param name="instance">An object to release the dependency graph for</param>
            /// <typeparam name="T">The reference type to release</typeparam>
            /// <returns>True if the dependency graph for this instance was successfully released, false otherwise</returns>
            public static bool Release(object instance)
            {
                int hash = RuntimeHelpers.GetHashCode(instance);
                using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
                {
                    if (tasks.Find(instance, hash, out _, out _, out Ref<TaskList> result) && result.Value.Count == 0)
                    {
                        result.Value = default;
                        return true;
                    }
                    else return false;
                }
            }
        }
    }
}