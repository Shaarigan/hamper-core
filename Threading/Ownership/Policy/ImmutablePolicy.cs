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
    readonly struct ImmutablePolicy : IAccessPolicy
    {
        /// <inheritdoc/>
        public int Order
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (int)AccessPermission.Immutable; }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsConflict(int order)
        {
            return (order < (int)AccessPermission.Immutable);
        }
    }
}