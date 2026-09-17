// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 


using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Provides a resource pool that enables reusing instances of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    /// <typeparam name="PoolPolicy">A policy managing type <typeparamref name="T"/></typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    class ObjectPool<T, PoolPolicy> : FinalizerObject, IObjectPool<T>
        where T : class
        where PoolPolicy : struct, IPoolPolicy<T>
    {
        private readonly ConcurrentStack<T> pool;
        
        #pragma warning disable CS0649
        private readonly PoolPolicy policy;
        #pragma warning restore CS0649
        
        /// <summary>
        /// Gets the amount of objects currently pooled
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return pool.Count; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObjectPool()
        {
            this.pool = new ConcurrentStack<T>();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (!Disposed)
            {
                for (T? instance; pool.TryPop(out instance);)
                {
                    // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                    policy.OnDispose(instance);
                    // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                }
            }
            else throw ThrowOnDisposed();
        }

        /// <summary>
        /// Obtains a disposable instance of type <typeparamref name="T"/> from the pool or creates one
        /// </summary>
        /// <returns>A disposable instance to use in a temporary block</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PooledObjectDisposable<T> Loan()
        {
            return new PooledObjectDisposable<T>(this, Rent());
        }
        
        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">The pool has been disposed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Rent()
        {
            if (!Disposed)
            {
                if (pool.TryPop(out T? instance))
                {
                    // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                    policy.OnRent(ref instance);
                    // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                    
                    return instance;
                }
                // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                else return policy.CreateInstance();
                // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
            }
            else throw ThrowOnDisposed();
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">The pool has been disposed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T instance)
        {
            if (!Disposed)
            {
                // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                policy.OnReturn(ref instance);
                // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                
                pool.Push(instance);
            }
            else throw ThrowOnDisposed();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Dispose(bool disposing)
        {
            if (base.Dispose(disposing))
            {
                pool.Clear();
                return true;
            }
            else return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ObjectDisposedException ThrowOnDisposed()
        {
            return new ObjectDisposedException(GetType().Name);
        }
    }
}