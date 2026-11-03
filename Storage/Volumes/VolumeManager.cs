// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.IO;
using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;
using Soe.Collections.HashSet;

namespace Soe.Storage
{

    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    static partial class VolumeManager
    {
        private static HashSet<string, HeapArray<VolumeInfo>, VolumeInfo> volumes;

        static VolumeManager()
        {
            volumes = new HashSet<string, HeapArray<VolumeInfo>, VolumeInfo>(StringComparer.InvariantCultureIgnoreCase);
        }
        
        public static void Initialize()
        {
            volumes.Clear();
            if (OperatingSystem.IsLinux())
            {
                InitializeLinux();
            }
            else if (OperatingSystem.IsWindows())
            {
                InitializeWindows();
            }
        }

        static void InitializeLinux()
        {
            const string mountFile = "/proc/self/mounts";
            if (File.Exists(mountFile))
            {
                foreach (string line in File.ReadLines(mountFile))
                {
                    int mount = FindWhitespace(line, 0) + 1;
                    int fs = FindWhitespace(line, mount) + 1;

                    if (fs >= 0 && (fs - mount) - 1 > 0 && !line.AsSpan().StartsWith(line.AsSpan(fs, Math.Max(FindWhitespace(line, fs) - fs, 0))))
                    {
                        ReadOnlySpan<char> mountPoint = line.AsSpan(mount, (fs - mount) - 1);

                        if ((mountPoint.Length == 1 || !line.AsSpan().StartsWith(mountPoint)) && Path.IsPathFullyQualified(mountPoint))
                        {
                            ReadOnlySpan<char> name = (mountPoint.Length == 1 ? "root" : mountPoint.Slice(mountPoint.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                            ReadOnlySpan<char> volumeName = (line.StartsWith("/dev/") ? line.AsSpan(5, mount - 6) : ReadOnlySpan<char>.Empty);

                            if (!volumeName.IsEmpty)
                            {
                                Register(name.ToString(), new ResourceIdentifier(mountPoint.ToString(), ResourceIdentifier.Parse(mountPoint)), volumeName.ToString());
                            }
                        }
                    }
                }
            }
        }
        static void InitializeWindows()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                try
                {
                    string identifier = drive.RootDirectory.FullName.TrimEnd(':', '\\', '/');
                    if (!string.IsNullOrWhiteSpace(identifier))
                    {
                        Register(identifier, new ResourceIdentifier(drive.RootDirectory.FullName, ResourceIdentifier.Parse(drive.RootDirectory.FullName)), null);
                    }
                }
                catch (IOException)
                { }
                catch (UnauthorizedAccessException)
                { }
            }
        }

        static int FindWhitespace(string str, int index)
        {
            for (int length = str.Length; index < length; index++)
            {
                if (Char.IsWhiteSpace(str[index]))
                {
                    return index;
                }
            }
            return 0;
        }

        static string Register(string identifier, ResourceIdentifier location, string? volumeName)
        {
            if(ValidateString(identifier))
            {
                int hash = identifier.GetHashCode();
                if(volumes.Find(identifier, hash, out int index, out int distance, out _))
                {
                    identifier = (!string.IsNullOrWhiteSpace(volumeName) ? string.Concat(volumeName, ">", identifier) : identifier);
                    for (int suffix = 0; ;suffix++)
                    {
                        string key = (suffix == 0 ? identifier : string.Concat(identifier, "~", suffix));
                        hash = identifier.GetHashCode();
                        if(!volumes.Find(identifier, hash, out index, out distance, out _))
                        {
                            identifier = key;
                            break;
                        }
                    }
                }
                volumes.Emplace(identifier, hash, index, distance, volumes.Version) = new VolumeInfo(identifier, location);
                return identifier;
            }
            else throw new ArgumentOutOfRangeException(nameof(identifier));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Register(string identifier, ResourceIdentifier location)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            return Register(identifier, location, null);
        }
        
        static bool ValidateString(string str)
        {
            for (int index = 0, length = str.Length; index < length; index++)
            {
                if (!Char.IsAsciiLetterOrDigit(str[index]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}