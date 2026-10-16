// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a runtime module within an assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal abstract class RuntimeModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets if this module can be executed similar to a standalone program
        /// </summary>
        public abstract bool IsMainModule
        {
            get;
        }
        
        /// <summary>
        /// Executes this modules load routine
        /// </summary>
        public abstract void Load();
        
        /// <summary>
        /// Executes this module's entry point
        /// </summary>
        /// <param name="args">Additional data passed to the module in order to modify its behavior</param>
        public abstract void Run(string[] args);
        
        /// <summary>
        /// Executes this modules release routine
        /// </summary>
        public abstract void Unload();
    }
}