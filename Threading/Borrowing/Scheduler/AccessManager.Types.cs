// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
        delegate void ReleaseDependenciesDelegate(Span<object?> instances, TaskNode node);
        delegate int GetOrderDelegate(Type type);
        
        /// <summary>
        /// Keeps track of a typed append operation and the corresponding object instance
        /// </summary>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly struct DependencyProxy(AppendDelegate append, object instance)
        {
            public readonly AppendDelegate Append = append;
            public readonly object Instance = instance;
        }
        /// <summary>
        /// Keeps track of a typed append operation and the corresponding object instance
        /// </summary>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly struct HashedDependencyProxy(AppendDelegate append, object instance)
        {
            public readonly AppendDelegate Append = append;
            public readonly object Instance = instance;
            public readonly int HashCode = RuntimeHelpers.GetHashCode(instance);
        }
        
        /// <summary>
        /// A fixed array of proxy instances used to sort access in order to prevent the AB problem
        /// </summary>
        [InlineArray(2)]
        struct ProxyArray2
        {
            public DependencyProxy element0;
        }
        /// <summary>
        /// A fixed array of proxy instances used to sort access in order to prevent the AB problem
        /// </summary>
        [InlineArray(4)]
        struct ProxyArray4
        {
            public HashedDependencyProxy element0;
        }
        /// <summary>
        /// A fixed array of proxy instances used to sort access in order to prevent the AB problem
        /// </summary>
        [InlineArray(8)]
        struct ProxyArray8
        {
            public HashedDependencyProxy element0;
        }
    }
}