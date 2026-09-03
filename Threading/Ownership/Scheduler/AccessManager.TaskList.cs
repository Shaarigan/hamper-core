// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        struct TaskList : IHashContainer<object>
        {
            private readonly int hash;

            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            private readonly object key;
            
            /// <inheritdoc/>
            public object Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                get { return (hash != 0 && key != null); }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskList(int hash, object key)
            {
                this.key = key;
                this.hash = hash;
            }
        }
    }
}