// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Ownable
{
    #if EXPORT_HAMPER_CORE_OWNABLE
    public
    #else
    internal
    #endif
    abstract class Ownable : IOwnable
    {
        private const int DefaultThread = -1;
        
        private int owningThread;

        
        public bool IsOwner
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (Volatile.Read(ref owningThread) == Thread.CurrentThread.ManagedThreadId); }
        }

        protected Ownable()
        {
            this.owningThread = DefaultThread;
        }
        

        public bool TryTakeOwnership()
        {
            return false;
        }

        public void ReturnOwnership()
        {
            
        }
    }
}