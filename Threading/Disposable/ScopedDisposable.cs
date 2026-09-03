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
    static class ScopedDisposable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Acquire<T, Policy>(ref T parameter)
            where Policy : struct, IRefScopePolicy<T>
        {
            default(Policy).Acquire(ref parameter);
            return new RefScopedDisposable<T, Policy>(ref parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Acquire<T, Policy>(ref T parameter, Policy policy)
            where Policy : struct, IRefScopePolicy<T>
        {
            return Acquire<T, Policy>(ref parameter);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Create<T, Policy>(ref T parameter)
            where Policy : struct, IRefScopePolicy<T>
        {
            return new RefScopedDisposable<T, Policy>(ref parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Create<T, Policy>(ref T parameter, Policy policy)
            where Policy : struct, IRefScopePolicy<T>
        {
            return Create<T, Policy>(ref parameter);
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly ref struct ScopedDisposable<T, Policy>
        where Policy : struct, IScopePolicy<T>
    {
        private readonly T parameter;
        private readonly Policy policy;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ScopedDisposable(T parameter)
        {
            this.parameter = parameter;
            this.policy = default;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            policy.Dispose(parameter);
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly ref struct RefScopedDisposable<T, Policy>
        where Policy : struct, IRefScopePolicy<T>
    {
        private readonly ref T parameter;
        private readonly Policy policy;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefScopedDisposable(ref T parameter)
        {
            this.parameter = ref parameter;
            this.policy = default;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            policy.Dispose(ref parameter);
        }
    }
}