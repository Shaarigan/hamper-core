// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            where Policy : IAccessPolicy
        {
            TaskNode node = new TaskNode<T>(instance);
            if (!Dependency<T>.Append<Policy>(instance, node))
            {
                node.SignalNode();
            }
            return node;
        }
        
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2>(T1 i1, T2 i2)
            where T1 : class
            where T2 : class
            where Policy1 : IAccessPolicy
            where Policy2 : IAccessPolicy
        {
            DependencyArray2 array = default;
            array[0] = new DependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new DependencyProxy(Dependency<T2>.Append<Policy2>, i2);

            if (RuntimeHelpers.GetHashCode(i1) > RuntimeHelpers.GetHashCode(i2))
            {
                (array[0], array[1]) =  (array[1], array[0]);
            }

            TaskNode node = new TaskNode<T1, T2>(i1, i2);
            for (int i = 0; i < 2; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            return node;
        }
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(T1 i1, T2 i2, T3 i3)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : IAccessPolicy
            where Policy2 : IAccessPolicy
            where Policy3 : IAccessPolicy
        {
            DependencyArray4 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 3);
            dependencies.Sort(Compare);
            
            TaskNode node = new TaskNode<T1, T2>(i1, i2);
            for (int i = 0; i < 3; i++)
            {
                array[i].Append(array[i].Instance, node);
            }
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
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
        
        static void Return(IAccessHandle handle)
        {
            
        }
    }
}