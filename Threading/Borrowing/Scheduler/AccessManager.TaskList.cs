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
        struct TaskList : IHashContainer<object>
        {
            private SmallArray<TaskNode?, SmallArray4<TaskNode?>> tasks;
            private ConcurrentBuffer<TaskNode> buffer;
            
            private readonly int hash;

            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            private readonly object key;
            
            /// <inheritdoc/>
            public object Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                get { return (hash != 0 && key != null); }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskList(int hash, object key)
            {
                this.key = key;
                this.hash = hash;
            }

            public bool Append<T, Policy>(TaskNode task)
                where T : class
                where Policy : struct, IAccessPolicy
            {
                Policy policy = default;
                int index = buffer.Enqueue(ref tasks, task);
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.SharedOperation>(ref buffer))
                {
                    SpinWait wait = new SpinWait();
                    
                    int moduloMask = tasks.Length - 1;
                    for (int beforeTail = ((buffer.Tail - 1) & moduloMask), i = ((index - 1) & moduloMask); i != beforeTail; i = ((i - 1) & moduloMask))
                    {
                        while (tasks[i]?.State < TaskNodeState.Initialized)
                        {
                            wait.SpinOnce();
                        }
                        int order = tasks[i]?.GetOrder<T>() ?? int.MaxValue;
                        if (policy.IsConflict(order)) 
                        { 
                            tasks[i]!.AppendChild(task);
                            for (i = ((i - 1) & moduloMask); i != beforeTail; i = ((i - 1) & moduloMask)) 
                            {
                                int nextOrder = tasks[i]?.GetOrder<T>() ?? int.MaxValue;
                                if (order == nextOrder)
                                {
                                    tasks[i]!.AppendChild(task);
                                }
                                else break;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }

            public void Remove(TaskNode task)
            {
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.ExclusiveOperation>(ref buffer))
                {
                    int index = tasks.IndexOf(task);
                    if (index >= 0)
                    {
                        int moduloMask = tasks.Length - 1;
                        
                        (tasks[index], tasks[buffer.Tail & moduloMask]) = (tasks[buffer.Tail & moduloMask], tasks[index]);
                        buffer.TryDequeue(ref tasks, out _);
                    }
                    else throw new IndexOutOfRangeException();
                }
            }
        }
    }
}