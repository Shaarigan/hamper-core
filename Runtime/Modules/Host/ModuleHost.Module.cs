// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    static partial class ModuleHost
    {
        private readonly struct ModuleDescriptor
        {
            private readonly RuntimeModuleAttribute module;

            /// <summary>
            /// Gets if this module can be executed similar to a standalone program
            /// </summary>
            public bool IsMainModule
            {
                get { return module.IsMainModule; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ModuleDescriptor(RuntimeModuleAttribute module)
            {
                this.module = module;
            }

            /// <summary>
            /// Executes this modules load routine
            /// </summary>
            public void Load()
            {
                module.Load();
            }

            /// <summary>
            /// Executes this module's entry point
            /// </summary>
            /// <param name="args">Additional data passed to the module in order to modify its behavior</param>
            public void Run(string[] args)
            {
                module.Run(args);
            }

            /// <summary>
            /// Executes this modules release routine
            /// </summary>
            public void Unload()
            {
                module.Unload();
            }
        }
    }
}