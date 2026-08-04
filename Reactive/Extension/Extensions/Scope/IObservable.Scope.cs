// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    static partial class IObservableExtension
    {
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            var result =  new Observable<T, DispatchingStrategy, SubscriptionStrategy>();
            disposable = observable.Subscribe(result);

            return result;
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope(observable, strategy, out _);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope<T, DispatchingStrategy, SubscriptionStrategy>(observable, default, out disposable);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope<T, DispatchingStrategy, SubscriptionStrategy>(observable, default, out _);
        }
    }
}