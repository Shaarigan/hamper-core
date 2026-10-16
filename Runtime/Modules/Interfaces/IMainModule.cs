// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a module that can be executed similar to a standalone program
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    interface IMainModule
    {
        /// <summary>
        /// The entry point of this module
        /// </summary>
        static abstract void Run(string[] args);
    }
}