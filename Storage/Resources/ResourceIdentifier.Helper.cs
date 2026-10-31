// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier
    {
        
        public static ResourceMetaData Parse(ReadOnlySpan<char> path)
        {
            ResourceMetaData result = default;
            for (int i = 0, state = 0; i < path.Length; i++)
            {
                switch (state)
                {
                    case 0: switch (path[i])
                        {
                            #region Remote path
                            case PathSeparator:
                            case PathSeparatorAlternative: if (path.Length > i + 1 && path[i + 1] == path[i])
                                {
                                    
                                }
                            #endregion
                                
                            #region Local path
                                else
                                {
                                    
                                }
                                break;
                             #endregion
                            
                            #region Relative path
                            case PathShiftDelimiter:
                                {
                                    
                                }
                                break;
                            #endregion
                        }
                        break;
                }
            }
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceMetaData Parse(string path)
        {
            return ResourceIdentifier.Parse(path.AsSpan());
        }
    }
}