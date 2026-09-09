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
        public readonly struct ManagedAccessPolicy : IScopePolicy<IAccessHandle>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose(in IAccessHandle instance)
            {
                Return(instance);
            }
        }
    }
}