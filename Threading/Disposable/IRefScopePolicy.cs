// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IRefScopePolicy<T>
    {
        void Acquire(ref T parameter);
        
        // ReSharper disable PureAttributeOnVoidMethod
        
        [Pure]
        void Dispose(ref T parameter);
        
        // ReSharper restore PureAttributeOnVoidMethod
    }
}