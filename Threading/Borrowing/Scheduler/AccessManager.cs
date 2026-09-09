// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        delegate bool AppendDelegate(object instance, TaskNode node);
        readonly struct DependencyProxy
        {
            public readonly AppendDelegate Append;
            public readonly object Instance;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public DependencyProxy(AppendDelegate append, object instance)
            {
                this.Append = append;
                this.Instance = instance;
            }
        }
        readonly struct HashedDependencyProxy
        {
            public readonly AppendDelegate Append;
            public readonly object Instance;
            public readonly int HashCode;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public HashedDependencyProxy(AppendDelegate append, object instance)
            {
                this.Append = append;
                this.Instance = instance;
                this.HashCode = RuntimeHelpers.GetHashCode(instance);
            }
        }
        [InlineArray(2)]
        struct DependencyArray2
        {
            public DependencyProxy element0;
        }
        [InlineArray(4)]
        struct DependencyArray4
        {
            public HashedDependencyProxy element0;
        }
        [InlineArray(8)]
        struct DependencyArray8
        {
            public HashedDependencyProxy element0;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T, Policy>(T instance)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            TaskNode node = new TaskNode<T, Policy>(instance);
            if (!Dependency<T>.Append<Policy>(instance, node))
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2>(T1 i1, T2 i2)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            DependencyArray2 array = default;
            array[0] = new DependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new DependencyProxy(Dependency<T2>.Append<Policy2>, i2);

            if (RuntimeHelpers.GetHashCode(i1) > RuntimeHelpers.GetHashCode(i2))
            {
                (array[0], array[1]) =  (array[1], array[0]);
            }

            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2>(i1, i2);
            for (int i = 0; i < 2; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(T1 i1, T2 i2, T3 i3)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            DependencyArray4 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 3);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3>(i1, i2, i3);
            for (int i = 0; i < 3; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(T1 i1, T2 i2, T3 i3, T4 i4)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
        {
            DependencyArray4 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 4);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(i1, i2, i3, i4);
            for (int i = 0; i < 4; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5)
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
            DependencyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 5);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(i1, i2, i3, i4, i5);
            for (int i = 0; i < 5; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6)
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
            DependencyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            array[5] = new HashedDependencyProxy(Dependency<T6>.Append<Policy6>, i6);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 6);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(i1, i2, i3, i4, i5, i6);
            for (int i = 0; i < 6; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7)
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
            DependencyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            array[5] = new HashedDependencyProxy(Dependency<T6>.Append<Policy6>, i6);
            array[6] = new HashedDependencyProxy(Dependency<T7>.Append<Policy7>, i7);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 7);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(i1, i2, i3, i4, i5, i6, i7);
            for (int i = 0; i < 7; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
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
            DependencyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            array[5] = new HashedDependencyProxy(Dependency<T6>.Append<Policy6>, i6);
            array[6] = new HashedDependencyProxy(Dependency<T7>.Append<Policy7>, i7);
            array[7] = new HashedDependencyProxy(Dependency<T8>.Append<Policy8>, i8);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 8);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(i1, i2, i3, i4, i5, i6, i7, i8);
            for (int i = 0; i < 8; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int Compare(HashedDependencyProxy x, HashedDependencyProxy y)
        {
            if (x.HashCode == y.HashCode)
            {
                return 0;
            }
            else if (x.HashCode > y.HashCode)
            {
                return 1;
            }
            else return -1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Return(IAccessHandle handle)
        {
            if (handle is TaskNode task)
            {
                SmallArray<TaskNode, SmallArray16<TaskNode>> dispatchableNodes = default;
                for (int i = task.Clear(ref dispatchableNodes) - 1; i >= 0; i--)
                {
                    dispatchableNodes[i].SignalNode();
                }
            }
            else throw new ArgumentException(nameof(handle));
        }
    }
}