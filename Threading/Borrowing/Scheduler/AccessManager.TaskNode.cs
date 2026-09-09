// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        
        abstract class TaskNode : IAccessHandle
        {
            SmallArray<TaskNode?, SmallArray4<TaskNode?>> array;
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
                children.Enqueue(ref array, child);
                child.AddParent();
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

            public abstract int GetOrder<T>()
                where T : class;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool RemoveParent()
            {
                return (Interlocked.Decrement(ref dependencyCount) == 0);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetInitialized()
            {
                if (State < TaskNodeState.Initialized)
                {
                    Volatile.Write(ref state, (int)TaskNodeState.Initialized);
                }
            }

            public void SignalNode()
            {
                if (signal.TrySetResult(this))
                {
                    Volatile.Write(ref state, (int)TaskNodeState.Running);
                }
                else Debugger.Break();
            }
        }

        class TaskNode<T, Policy> : TaskNode
            where T : class
            where Policy : struct, IAccessPolicy
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<TOther>()
            {
                if (typeof(TOther) == typeof(T))
                {
                    return default(Policy).Order;
                }
                else throw new ArgumentException(nameof(TOther));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2> : TaskNode
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
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
                Dependency<T2>.Remove(i2, this);
                return base.Clear(ref dispatchableNodes);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3> : TaskNode
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
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4> : TaskNode
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
            public TaskNode(T1 i1, T2 i2, T3 i3,  T4 i4)
            {
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
                this.i4 = i4;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                Dependency<T4>.Remove(i4, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else if (typeof(T) == typeof(T4))
                {
                    return default(Policy4).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5> : TaskNode
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
            public TaskNode(T1 i1, T2 i2, T3 i3,  T4 i4, T5 i5)
            {
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
                this.i4 = i4;
                this.i5 = i5;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                Dependency<T4>.Remove(i4, this);
                Dependency<T5>.Remove(i5, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else if (typeof(T) == typeof(T4))
                {
                    return default(Policy4).Order;
                }
                else if (typeof(T) == typeof(T5))
                {
                    return default(Policy5).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6> : TaskNode
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
            public TaskNode(T1 i1, T2 i2, T3 i3,  T4 i4, T5 i5, T6 i6)
            {
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
                this.i4 = i4;
                this.i5 = i5;
                this.i6 = i6;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                Dependency<T4>.Remove(i4, this);
                Dependency<T5>.Remove(i5, this);
                Dependency<T6>.Remove(i6, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else if (typeof(T) == typeof(T4))
                {
                    return default(Policy4).Order;
                }
                else if (typeof(T) == typeof(T5))
                {
                    return default(Policy5).Order;
                }
                else if (typeof(T) == typeof(T6))
                {
                    return default(Policy6).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7> : TaskNode
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
            public TaskNode(T1 i1, T2 i2, T3 i3,  T4 i4, T5 i5, T6 i6, T7 i7)
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
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                Dependency<T4>.Remove(i4, this);
                Dependency<T5>.Remove(i5, this);
                Dependency<T6>.Remove(i6, this);
                Dependency<T7>.Remove(i7, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else if (typeof(T) == typeof(T4))
                {
                    return default(Policy4).Order;
                }
                else if (typeof(T) == typeof(T5))
                {
                    return default(Policy5).Order;
                }
                else if (typeof(T) == typeof(T6))
                {
                    return default(Policy6).Order;
                }
                else if (typeof(T) == typeof(T7))
                {
                    return default(Policy7).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
        
        class TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8> : TaskNode
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
            public TaskNode(T1 i1, T2 i2, T3 i3,  T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
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
            public override int Clear<Accessor>(ref Accessor dispatchableNodes)
            {
                Dependency<T1>.Remove(i1, this);
                Dependency<T2>.Remove(i2, this);
                Dependency<T3>.Remove(i3, this);
                Dependency<T4>.Remove(i4, this);
                Dependency<T5>.Remove(i5, this);
                Dependency<T6>.Remove(i6, this);
                Dependency<T7>.Remove(i7, this);
                Dependency<T8>.Remove(i8, this);
                return base.Clear(ref dispatchableNodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetOrder<T>()
            {
                if (typeof(T) == typeof(T1))
                {
                    return default(Policy1).Order;
                }
                else if (typeof(T) == typeof(T2))
                {
                    return default(Policy2).Order;
                }
                else if (typeof(T) == typeof(T3))
                {
                    return default(Policy3).Order;
                }
                else if (typeof(T) == typeof(T4))
                {
                    return default(Policy4).Order;
                }
                else if (typeof(T) == typeof(T5))
                {
                    return default(Policy5).Order;
                }
                else if (typeof(T) == typeof(T6))
                {
                    return default(Policy6).Order;
                }
                else if (typeof(T) == typeof(T7))
                {
                    return default(Policy7).Order;
                }
                else if (typeof(T) == typeof(T8))
                {
                    return default(Policy8).Order;
                }
                else throw new ArgumentException(nameof(T));
            }
        }
    }
}