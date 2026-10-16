// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
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
        private const string ReservedName = "Soe.Generated.Reference";
        
        static ModuleHost()
        {
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (RuntimeModuleAttribute runtimeAttribute in assembly.GetCustomAttributes<RuntimeModuleAttribute>())
                {
                    ModuleDescriptor descriptor = new ModuleDescriptor(runtimeAttribute);
                }
            }
        }
        
        static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            // Detect if the requested assembly is a generated reference assembly
            if (new AssemblyName(args.Name).Name == ReservedName)
            {
                /*
                 Modules are building against a generated reference assembly rather than embedding the necessary
                 packages into the module directly. The host assembly or one of it's generated capability assemblies
                 will be put into the role of the reference assembly in order to provide the necessary security level
                */
                return Assembly.GetExecutingAssembly();
            }
            else return null;
        }

        static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs assemblyLoadEventArgs)
        {
            // Detect if the loaded assembly is a module
            // TODO
            
            assemblyLoadEventArgs.LoadedAssembly.GetTypes();
            foreach (RuntimeModuleAttribute runtimeAttribute in assemblyLoadEventArgs.LoadedAssembly.GetCustomAttributes<RuntimeModuleAttribute>())
            {
                ModuleDescriptor descriptor = new ModuleDescriptor(runtimeAttribute);
            }
        }
    }
}