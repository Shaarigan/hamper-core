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
        private const float LoadFactor = 0.86f;
        private static int moduloMask;
        
        private static RuntimeModule[]? modules;
        private static int count;
        /// <summary>
        /// Gets the current number of modules loaded
        /// </summary>
        public static int ModuleCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }
        
        private static int version;
        /// <summary>
        /// Gets the current container version
        /// </summary>
        /// <remarks>Version is increased whenever the container layout changes</remarks>
        public static int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ModuleHost()
        {
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            Reserve(4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<RuntimeModule> AsReadOnlySpan()
        {
            return modules;
        }
        
        static ref RuntimeModule Emplace(in ModuleId id, int index, int distance, int currentVersion)
        {
        Head:
            if (version == currentVersion || !Find(in id, out index, out distance, out Ref<RuntimeModule> module))
            {
                if (modules == null || count >= modules.Length * LoadFactor)
                {
                    Grow();
                    goto Head;
                }

                ref RuntimeModule element = ref modules![Emplace(index, distance)];
                element = new RuntimeModule(id, null!, null!);

                version++;
                return ref element;
            }
            else return ref module.Value;
        }
        static int Emplace(int index, int distance)
        {
            if (modules![index].IsEmpty)
            {
                // Insert as slot is already empty
                
                count++;
                return index;
            }
            else
            {
                RuntimeModule element = default;
                {
                    int i = distance;
                    int tmp = GetDistance(index);
                    int result; 
                    
                    // Check if distance is smaller than the current distance
                    if(distance > tmp)
                    {
                        // Swap elements so the new element is closer to index
                        Swap(ref modules[index], ref element);
                        result = index;
                        distance = tmp;
                    }
                    else result = -1;
                    
                    index = (index + 1) & moduloMask;
                    distance++;

                    // Move the current element to the next free slot
                    for (; i < modules.Length; i++, index = (index + 1) & moduloMask)
                    {
                        // Free slot found to put the element into
                        if (modules[index].IsEmpty)
                        {
                            count++;
                            if (!element.IsEmpty)
                            {
                                Swap(ref modules[index], ref element);
                                
                                #if DEBUG
                                if(result < 0)
                                {
                                    throw new IndexOutOfRangeException();
                                }
                                else
                                #endif
                                return result;
                            }
                            else return index;
                        }
                        else
                        {
                            // Check if distance is smaller than the current distance and swap
                            tmp = GetDistance(index);
                            if (distance > tmp)
                            {
                                if (element.IsEmpty)
                                {
                                    result = index;
                                }
                                Swap(ref modules[index], ref element);
                                distance = tmp;
                            }
                            distance++;
                        }
                    }
                }
                throw new OverflowException();
            }
        }
        
        static bool Find(in ModuleId id, out int index, out int distance, out Ref<RuntimeModule> RuntimeModule)
        {
            index = (int)(id.HashCode & moduloMask);
            if (count > 0)
            {
                distance = 0;

                for (int length = modules?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (modules![index].IsValid)
                    {
                        if (id.Value == modules[index].Id.Value)
                        {
                            RuntimeModule = new Ref<RuntimeModule>(ref modules[index]);
                            return true;
                        }
                        else if (distance > GetDistance(index))
                            break;
                    }
                    else break;
                }
            }
            else distance = 0;
            
            RuntimeModule = Ref<RuntimeModule>.CreateEmpty(); 
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetDistance(int index)
        {
            int tmp = (int)(modules![index].Id.HashCode & moduloMask);
            if (tmp > index)
            {
                return (index + (modules.Length - tmp));
            }
            else return (index - tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Grow()
        {
            Reserve(modules == null ? 4 : modules.Length + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (RuntimeModuleAttribute runtimeAttribute in assembly.GetCustomAttributes<RuntimeModuleAttribute>())
                {
                    ModuleId id = new ModuleId(runtimeAttribute.ModuleName.ToLowerInvariant().Fnv32(), UniqueId.Next32());
                    ref RuntimeModule module = ref Emplace(id, 0, 0, version + 1);
                    if (module.Assembly == null)
                    {
                       module = new RuntimeModule(id, assembly, runtimeAttribute);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Swap(ref RuntimeModule lhs, ref RuntimeModule rhs)
        {
            (lhs, rhs) = (rhs, lhs);
        }
        
        static void Reserve(int capacity)
        {
            capacity = capacity.NextPowerOfTwo();
            moduloMask = (capacity - 1);
            count = 0;

            RuntimeModule[]? tmp = modules;
            modules = new RuntimeModule[capacity];
            if (tmp != null)
            {
                // Reinsert existing elements
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i].IsValid)
                    {
                        int index = Emplace((int)(tmp[i].Id.HashCode & moduloMask), 0);
                        modules[index] = tmp[i];
                    }
                }
            }
            version++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="assemblyLoadEventArgs"></param>
        static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs assemblyLoadEventArgs)
        {
            // Detect if the loaded assembly is a module
            // TODO
            
            /*assemblyLoadEventArgs.LoadedAssembly.GetTypes();
            foreach (RuntimeModuleAttribute runtimeAttribute in assemblyLoadEventArgs.LoadedAssembly.GetCustomAttributes<RuntimeModuleAttribute>())
            {
                ModuleId id = new ModuleId(runtimeAttribute.ModuleName.ToLowerInvariant().Fnv32(), UniqueId.Next32());
                ref RuntimeModule module = ref Emplace(id, 0, 0, version + 1);
                if (module.Assembly == null)
                {
                    module = new RuntimeModule(id, assemblyLoadEventArgs.LoadedAssembly, runtimeAttribute);
                }
            }*/
        }
        
        /// <summary>
        /// Returns an iterator that iterates through a collection
        /// </summary>
        /// <returns>The iterator handling the underlying collection</returns>
        /// <remarks>The method must be named GetEnumerator to be recognized by foreach</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyIterator<RuntimeModule, ReadOnlyIteratorStrategy> GetEnumerator()
        {
            return new ReadOnlyIterator<RuntimeModule, ReadOnlyIteratorStrategy>(AsReadOnlySpan());
        }
    }
}