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
    readonly struct EmptyRefScope<T> : IRefScopePolicy<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Acquire(ref T parameter)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose(ref T parameter)
        { }
    }
}