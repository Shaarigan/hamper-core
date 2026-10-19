// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    static partial class ModuleHost
    {
        [StructLayout(LayoutKind.Explicit)]
        internal readonly struct ModuleId
        {
            [FieldOffset(0)]
            public readonly UInt64 Value;
            [FieldOffset(0)]
            public readonly UInt32 HashCode;
            [FieldOffset(4)]
            public readonly Int32 UniqueId;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal ModuleId(UInt32 hashCode, Int32 uniqueId)
            {
                this.Value = 0;
                this.HashCode = hashCode;
                this.UniqueId = uniqueId;
            }
        }
        
        public readonly struct RuntimeModule
        {
            internal readonly ModuleId Id;
            public readonly Assembly Assembly;
            public readonly ModuleContext Context;

            /// <summary>
            /// Gets the name of this module
            /// </summary>
            public string ModuleName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Context.ModuleName; }
            }
            
            /// <summary>
            /// Gets if this element has a memory handle assigned to
            /// </summary>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Id.Value != 0; }
            }
            
            /// <summary>
            /// Gets if this element is currently occupied
            /// </summary>
            public bool IsEmpty
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Id.Value == 0; }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal RuntimeModule(ModuleId id, Assembly assembly, RuntimeModuleAttribute module)
            {
                this.Id = id;
                this.Assembly = assembly;
                this.Context = new ModuleContext(module);
            }
        }
        
        public readonly struct ModuleContext
        {
            private readonly RuntimeModuleAttribute module;

            /// <summary>
            /// Gets the name of this module
            /// </summary>
            public string ModuleName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return module.ModuleName; }
            }
            
            /// <summary>
            /// Gets if this module can be executed similar to a standalone program
            /// </summary>
            public bool IsMainModule
            {
                get { return module.IsMainModule; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal ModuleContext(RuntimeModuleAttribute module)
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