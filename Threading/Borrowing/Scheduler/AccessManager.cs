// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soe.Collections.Inline;

namespace Soe.Threading
{
    /// <summary>
    /// Allows to create typed scopes in which an object instance can be safely used according to a certain policy
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        [ThreadStatic]
        private static IAccessHandle? current;
        
        /// <summary>
        /// Schedules access to the provided instance according to the desired access policy
        /// </summary>
        /// <param name="instance">An instance to get access</param>
        /// <typeparam name="T">An instance type to get access</typeparam>
        /// <typeparam name="Policy">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T, Policy>(T instance)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T, Policy>,
                ReleaseDependencies<T>
            );
            instances[0] = instance;

            if (!Dependency<T>.Append<Policy>(instance, node))
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2>(T1 i1, T2 i2)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            ProxyArray2 array = default;
            array[0] = new DependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new DependencyProxy(Dependency<T2>.Append<Policy2>, i2);

            if (RuntimeHelpers.GetHashCode(i1) > RuntimeHelpers.GetHashCode(i2))
            {
                (array[0], array[1]) =  (array[1], array[0]);
            }

            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2>,
                ReleaseDependencies<T1, T2>
            );
            instances[0] = i1;
            instances[1] = i2;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(T1 i1, T2 i2, T3 i3)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            ProxyArray4 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 3);
            dependencies.Sort(Compare);
            
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3>,
                ReleaseDependencies<T1, T2, T3>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
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
            ProxyArray4 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 4);
            dependencies.Sort(Compare);
            
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>,
                ReleaseDependencies<T1, T2, T3, T4>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;
            instances[3] = i4;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
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
            ProxyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 5);
            dependencies.Sort(Compare);
            
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>,
                ReleaseDependencies<T1, T2, T3, T4, T5>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;
            instances[3] = i4;
            instances[4] = i5;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
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
            ProxyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            array[5] = new HashedDependencyProxy(Dependency<T6>.Append<Policy6>, i6);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 6);
            dependencies.Sort(Compare);
            
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>,
                ReleaseDependencies<T1, T2, T3, T4, T5, T6>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;
            instances[3] = i4;
            instances[4] = i5;
            instances[5] = i6;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <param name="i7">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <typeparam name="T7">An instance type to get access</typeparam>
        /// <typeparam name="Policy7">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
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
            ProxyArray8 array = default;
            array[0] = new HashedDependencyProxy(Dependency<T1>.Append<Policy1>, i1);
            array[1] = new HashedDependencyProxy(Dependency<T2>.Append<Policy2>, i2);
            array[2] = new HashedDependencyProxy(Dependency<T3>.Append<Policy3>, i3);
            array[3] = new HashedDependencyProxy(Dependency<T4>.Append<Policy4>, i4);
            array[4] = new HashedDependencyProxy(Dependency<T5>.Append<Policy5>, i5);
            array[5] = new HashedDependencyProxy(Dependency<T6>.Append<Policy6>, i6);
            array[6] = new HashedDependencyProxy(Dependency<T7>.Append<Policy7>, i7);
            
            Span<HashedDependencyProxy> dependencies = MemoryMarshal.CreateSpan(ref array.element0, 7);
            dependencies.Sort(Compare);
            
            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>,
                ReleaseDependencies<T1, T2, T3, T4, T5, T6, T7>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;
            instances[3] = i4;
            instances[4] = i5;
            instances[5] = i6;
            instances[6] = i7;

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
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <param name="i7">An instance to get access</param>
        /// <param name="i8">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <typeparam name="T7">An instance type to get access</typeparam>
        /// <typeparam name="Policy7">The access policy for this instance type</typeparam>
        /// <typeparam name="T8">An instance type to get access</typeparam>
        /// <typeparam name="Policy8">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
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
            ProxyArray8 array = default;
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

            TaskNode node = GetNodeInstance();
            Span<object?> instances = node.Initialize
            (
                GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>,
                ReleaseDependencies<T1, T2, T3, T4, T5, T6, T7, T8>
            );
            instances[0] = i1;
            instances[1] = i2;
            instances[2] = i3;
            instances[3] = i4;
            instances[4] = i5;
            instances[5] = i6;
            instances[6] = i7;
            instances[7] = i8;
            
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

        static void ReleaseDependencies<T>(Span<object?> instances, TaskNode node)
            where T : class
        {
            Dependency<T>.Remove(instances[0], node);
        }
        
        static void ReleaseDependencies<T1, T2>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
            Dependency<T3>.Remove(instances[2], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3, T4>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
            Dependency<T3>.Remove(instances[2], node);
            Dependency<T4>.Remove(instances[3], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3, T4, T5>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
            Dependency<T3>.Remove(instances[2], node);
            Dependency<T4>.Remove(instances[3], node);
            Dependency<T5>.Remove(instances[4], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3, T4, T5, T6>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
            Dependency<T3>.Remove(instances[2], node);
            Dependency<T4>.Remove(instances[3], node);
            Dependency<T5>.Remove(instances[4], node);
            Dependency<T6>.Remove(instances[5], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3, T4, T5, T6, T7>(Span<object?> instances, TaskNode node)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            Dependency<T1>.Remove(instances[0], node);
            Dependency<T2>.Remove(instances[1], node);
            Dependency<T3>.Remove(instances[2], node);
            Dependency<T4>.Remove(instances[3], node);
            Dependency<T5>.Remove(instances[4], node);
            Dependency<T6>.Remove(instances[5], node);
            Dependency<T7>.Remove(instances[6], node);
        }
        
        static void ReleaseDependencies<T1, T2, T3, T4, T5, T6, T7, T8>(Span<object?> instances, TaskNode node)
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
           where T6 : class
           where T7 : class
           where T8 : class
       {
           Dependency<T1>.Remove(instances[0], node);
           Dependency<T2>.Remove(instances[1], node);
           Dependency<T3>.Remove(instances[2], node);
           Dependency<T4>.Remove(instances[3], node);
           Dependency<T5>.Remove(instances[4], node);
           Dependency<T6>.Remove(instances[5], node);
           Dependency<T7>.Remove(instances[6], node);
           Dependency<T8>.Remove(instances[7], node);
        }
            
        static int GetOrder<T, Policy>(Type type)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            if (type == typeof(T))
            {
                return default(Policy).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2>(Type type)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3>(Type type)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(Type type)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
        {
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else if (type == typeof(T4))
            {
                return default(Policy4).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(Type type)
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
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else if (type == typeof(T4))
            {
                return default(Policy4).Order;
            }
            else if (type == typeof(T5))
            {
                return default(Policy5).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(Type type)
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
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else if (type == typeof(T4))
            {
                return default(Policy4).Order;
            }
            else if (type == typeof(T5))
            {
                return default(Policy5).Order;
            }
            else if (type == typeof(T6))
            {
                return default(Policy6).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(Type type)
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
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else if (type == typeof(T4))
            {
                return default(Policy4).Order;
            }
            else if (type == typeof(T5))
            {
                return default(Policy5).Order;
            }
            else if (type == typeof(T6))
            {
                return default(Policy6).Order;
            }
            else if (type == typeof(T7))
            {
                return default(Policy7).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        static int GetOrder<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(Type type)
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
            if (type == typeof(T1))
            {
                return default(Policy1).Order;
            }
            else if (type == typeof(T2))
            {
                return default(Policy2).Order;
            }
            else if (type == typeof(T3))
            {
                return default(Policy3).Order;
            }
            else if (type == typeof(T4))
            {
                return default(Policy4).Order;
            }
            else if (type == typeof(T5))
            {
                return default(Policy5).Order;
            }
            else if (type == typeof(T6))
            {
                return default(Policy6).Order;
            }
            else if (type == typeof(T7))
            {
                return default(Policy7).Order;
            }
            else if (type == typeof(T8))
            {
                return default(Policy8).Order;
            }
            else throw new ArgumentException(type.Name);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TaskNode GetNodeInstance()
        {
            return new TaskNode();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ReturnNodeInstance(TaskNode node)
        {}
        
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
                for (int i = task.Finish(ref dispatchableNodes) - 1; i >= 0; i--)
                {
                    dispatchableNodes[i].SignalNode();
                }
                ReturnNodeInstance(task);
            }
            else throw new ArgumentException(nameof(handle));
        }

        /// <summary>
        /// Releases the provided reference if possible
        /// </summary>
        /// <param name="instance">An object to release the dependency graph for</param>
        /// <typeparam name="T">The reference type to release</typeparam>
        /// <returns>True if the dependency graph for this instance was successfully released, false otherwise</returns>
        /// <remarks>Releasing of the object will fail if the dependency graph is not empty, which means one or more tasks
        /// are pending and the scheduler is waiting for a trigger to run</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReleaseReference<T>(T instance)
            where T : class
        {
            return Dependency<T>.Release(instance);
        }
        
        /// <summary>
        /// Throws if the calling thread is executed in an owned scope and the desired access doesn't match
        /// </summary>
        /// <param name="access">The desired access type for this operation</param>
        /// <typeparam name="T">The reference type to check</typeparam>
        /// <exception cref="ThreadOwnershipViolationException">Thrown if executed within an owned scope and the access doesn't match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnAccessViolation<T>(AccessType access)
            where T : class
        {
            if ((current?.GetAccess<T>() ?? access) != access)
                throw new ThreadOwnershipViolationException();
        }
        /// <summary>
        /// Throws if the calling thread is executed in an owned scope and the desired access is less permissive as expected
        /// </summary>
        /// <param name="access">The minimum access type for this operation</param>
        /// <typeparam name="T">The reference type to check</typeparam>
        /// <exception cref="ThreadOwnershipViolationException">Thrown if executed within an owned scope and the access is less permissive as expected</exception>
        /// <remarks>Access is an ordered ID growing from zero, where zero means most permissive access</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessAccessible<T>(AccessType access)
            where T : class
        {
            if ((current?.GetAccess<T>() ?? access) > access)
                throw new ThreadOwnershipViolationException();
        }
    }
}