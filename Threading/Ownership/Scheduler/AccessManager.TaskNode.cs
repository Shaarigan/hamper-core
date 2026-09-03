// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;
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
        enum TaskNodeState : int
        {
            Created = 0,
            Initialized = 1,
            Running = 2,
            Completed = 3
        }
        
        public interface IAccessHandle
        { }
        abstract class TaskNode : IAccessHandle
        {
            SmallArray<TaskNode?, SmallArray2<TaskNode?>> array;
            ConcurrentBuffer<TaskNode> children;
            
            TaskCompletionSource<IAccessHandle> signal;

            private int state;

            public TaskNodeState State
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (TaskNodeState)Volatile.Read(ref state); }
            }

            private int dependencyCount;

            public int DependencyCount
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Volatile.Read(ref dependencyCount); }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected TaskNode()
            {
                this.array = default;
                this.children = default;
                this.signal = new TaskCompletionSource<IAccessHandle>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Task<IAccessHandle>(TaskNode node)
            {
                return node.signal.Task;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddParent()
            {
                Interlocked.Increment(ref dependencyCount);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AppendChild(TaskNode child)
            {
                children.Enqueue(array, child);
            }

            public virtual int Clear<Accessor>(ref Accessor dispatchableNodes)
                where Accessor : IArrayAccessor<TaskNode>
            {
                Volatile.Write(ref state, (int)TaskNodeState.Completed);
                using(ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.ExclusiveOperation>(ref children))
                {
                    Span<TaskNode> nodes = dispatchableNodes.AsSpan();
                    int nodeCount = 0;
                    
                    for (int i = children.Tail, count = children.Count; count > 0; i++, count--)
                    {
                        if (array[i] != null)
                        {
                            if (array[i]!.RemoveParent())
                            {
                                if (nodes.Length == nodeCount)
                                {
                                    dispatchableNodes.Resize(nodeCount * 2);
                                    nodes = dispatchableNodes.AsSpan();
                                }
                                nodes[nodeCount] = array[i]!;
                                nodeCount++;
                            }
                            array[i] = null;
                        }
                    }
                    children.Reset();
                    return nodeCount;
                }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool RemoveParent()
            {
                return (Interlocked.Decrement(ref dependencyCount) == 0);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Shift()
            {
                if (State < TaskNodeState.Completed)
                {
                    Interlocked.Increment(ref state);
                }
            }

            public void SignalNode()
            {
                signal.TrySetResult(this);
            }
        }

        class TaskNode<T> : TaskNode
            where T : class
        {
            private readonly T instance;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskNode(T instance)
            {
                this.instance = instance;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T>.Remove(instance, this);
                return base.Clear(ref dispatchableNodes);
            }
        }
        class TaskNode<T1, T2> : TaskNode
            where T1 : class
            where T2 : class
        {
            private readonly T1 i1;
            private readonly T2 i2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskNode(T1 i1, T2 i2)
            {
                this.i1 = i1;
                this.i2 = i2;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T1>.Remove(i2, this);
                return base.Clear(ref dispatchableNodes);
            }
        }
        class TaskNode<T1, T2, T3> : TaskNode
            where T1 : class
            where T2 : class
            where T3 : class
        {
            private readonly T1 i1;
            private readonly T2 i2;
            private readonly T3 i3;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskNode(T1 i1, T2 i2, T3 i3)
            {
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T1>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                return base.Clear(ref dispatchableNodes);
            }
        }
    }
}