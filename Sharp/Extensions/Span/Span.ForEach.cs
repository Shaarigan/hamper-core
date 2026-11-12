// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class SpanExtension
    {
        /// <summary>
        /// Represents a callback invoked for each item in a <see cref="SpanTuple{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items contained in the span tuple</typeparam>
        /// <typeparam name="TParam">The type of the additional parameter passed to the callback</typeparam>
        /// <param name="item">The current item</param>
        /// <param name="index">The zero-based index of the current item within the tuple</param>
        /// <param name="parameter">An additional parameter passed to the callback</param>
        public delegate void SpanDelegate<T, in TParam>(Span<T> item, int index, TParam parameter)
            where TParam : allows ref struct;
        /// <summary>
        /// Represents a callback invoked for each item in a <see cref="SpanTuple{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items contained in the span tuple</typeparam>
        /// <typeparam name="TParam">The type of the additional parameter passed to the callback</typeparam>
        /// <param name="item">The current item</param>
        /// <param name="index">The zero-based index of the current item within the tuple</param>
        /// <param name="parameter">An additional parameter passed to the callback</param>
        public delegate void ReadOnlySpanDelegate<T, in TParam>(ReadOnlySpan<T> item, int index, TParam parameter)
            where TParam : allows ref struct;
        
        // ReSharper disable InvalidXmlDocComment

        /// <summary>
        /// Invokes the specified callback for the single item contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple item</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for the item</param>
        /// <param name="parameter">An additional parameter passed to the callback</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for the single item contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple item</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for the item</param>
        /// <param name="parameter">An additional parameter passed to the callback</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the two items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the two items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the three items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the three items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the four items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T, T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the four items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T, T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the five items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T, T, T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the five items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T, T, T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the six items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T, T, T, T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
            callback(tuple.Item6, 5, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the six items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T, T, T, T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
            callback(tuple.Item6, 5, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the seven items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this SpanTuple<T, T, T, T, T, T, T> tuple, SpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
            callback(tuple.Item6, 5, parameter);
            callback(tuple.Item7, 6, parameter);
        }
        /// <summary>
        /// Invokes the specified callback for each of the seven items contained in the tuple.
        /// </summary>
        /// <typeparam name="T">The type of the tuple items</typeparam>
        /// <typeparam name="TParam">The type of the additional callback parameter</typeparam>
        /// <param name="callback">The callback invoked for each item</param>
        /// <param name="parameter">An additional parameter passed to each callback invocation</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T, TParam>(this ReadOnlySpanTuple<T, T, T, T, T, T, T> tuple, ReadOnlySpanDelegate<T, TParam> callback, TParam parameter)
        {
            callback(tuple.Item1, 0, parameter);
            callback(tuple.Item2, 1, parameter);
            callback(tuple.Item3, 2, parameter);
            callback(tuple.Item4, 3, parameter);
            callback(tuple.Item5, 4, parameter);
            callback(tuple.Item6, 5, parameter);
            callback(tuple.Item7, 6, parameter);
        }

        // ReSharper restore InvalidXmlDocComment
    }
}