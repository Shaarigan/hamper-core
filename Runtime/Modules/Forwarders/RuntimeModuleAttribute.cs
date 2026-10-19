// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a runtime module within an assembly
    /// </summary>
    /// <remarks>This is a forwarder type. The host returns an internal implementation of this attribute
    /// when requested by the CLR</remarks>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    #if COMPILER_FLAG_RUNTIME_MODULE
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    class RuntimeModuleAttribute<T> : Attribute
    #else
    class RuntimeModuleAttribute<T> : RuntimeModuleAttribute
    #endif
        where T : IRuntimeModule
    {
        private delegate void RunDelegate(string[] args);
        private readonly RunDelegate? runInternal;
        private readonly string moduleName;
        
        /// <summary>
        /// Gets the name of this module
        /// </summary>
        #if COMPILER_FLAG_RUNTIME_MODULE
        public string ModuleName
        #else
        public override string ModuleName
        #endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return moduleName; }
        }
        
        /// <summary>
        /// Gets if this module can be executed similar to a standalone program
        /// </summary>
        #if COMPILER_FLAG_RUNTIME_MODULE
        public bool IsMainModule
        #else
        public override bool IsMainModule
        #endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (runInternal != null); }
        }

        /// <summary>
        /// Creates a new instance of this attribute
        /// </summary>
        /// <param name="moduleName">The name of this module</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RuntimeModuleAttribute(string moduleName)
        {
            if(typeof(IMainModule).IsAssignableFrom(typeof(T)))
            {
                runInternal = typeof(T).GetMethod(nameof(IMainModule.Run), BindingFlags.Static | BindingFlags.Public)?
                    .CreateDelegate<RunDelegate>() ?? null;
            }
            this.moduleName = moduleName;
        }
        
        /// <summary>
        /// Executes this modules load routine
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public void Load()
        #else
        public override void Load()
        #endif
        {
            T.Load();
        }

        /// <summary>
        /// Executes this module's entry point
        /// </summary>
        /// <param name="args">Additional data passed to the module in order to modify its behavior</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public void Run(string[] args)
        #else
        public override void Run(string[] args)
        #endif
        {
            if (IsMainModule)
            {
                runInternal!(args);
            }
        }
        
        /// <summary>
        /// Executes this modules release routine
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public void Unload()
        #else
        public override void Unload()
        #endif
        {
            T.Unload();
        }
    }
}