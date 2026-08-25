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
    readonly struct SharedScope : IScopePolicy<OwnershipHandle>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Acquire(ref OwnershipHandle parameter)
        {
            parameter.GetShredAccessUnsafe();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose(ref OwnershipHandle parameter)
        {
            parameter.ReturnSharedAccessUnsafe();
        }
    }
}