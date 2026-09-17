// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Manages a single instance of type <typeparamref name="T"/> from the given pool
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly struct PooledObjectDisposable<T> : IDisposable
        where T : class
    {
        private readonly IObjectPool<T> pool;
        private readonly T instance;

        /// <summary>
        /// The instance rented from the underlying pool
        /// </summary>
        public T Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PooledObjectDisposable(IObjectPool<T> pool, T instance)
        {
            this.pool = pool;
            this.instance = instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(PooledObjectDisposable<T> disposable)
        {
            return disposable.instance;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            pool.Return(instance);
        }
    }
}