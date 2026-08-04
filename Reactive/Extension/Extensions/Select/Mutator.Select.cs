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
    static partial class MutatorExtension
    {
        /// <summary>
        /// Projects each element of a sequence into a new form
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
        /// <typeparam name="MutatorT">A data projection unit</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TOut, SelectProjectionMutator<TIn, TOut, MutatorT>> Select<TIn, TOut, MutatorT>(this Mutator<TIn, MutatorT> mutator, Func<TIn, TOut> selector)
            where MutatorT : struct, IGatingMutator<TIn>
        {
            return new Mutator<TIn, TOut, SelectProjectionMutator<TIn, TOut, MutatorT>>(mutator.Source, new SelectProjectionMutator<TIn, TOut, MutatorT>(mutator.Instance, selector));
        }
        
        /// <summary>
        /// Projects each element of a sequence into a new form
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TTmp">The output type of the data processing unit to chain</typeparam>
        /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
        /// <typeparam name="MutatorT">A data projection unit</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TOut, SelectProjectionMutator<TIn, TTmp, TOut, MutatorT>> Select<TIn, TTmp, TOut, MutatorT>(this Mutator<TIn, TTmp, MutatorT> mutator, Func<TTmp, TOut> selector)
            where MutatorT : struct, IProjectionMutator<TIn, TTmp>
        {
            return new Mutator<TIn, TOut, SelectProjectionMutator<TIn, TTmp, TOut, MutatorT>>(mutator.Source, new SelectProjectionMutator<TIn, TTmp, TOut, MutatorT>(mutator.Instance, selector));
        }
    }
}