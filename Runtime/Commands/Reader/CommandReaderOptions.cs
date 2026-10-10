// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// A set of configuration properties applied to an argument reader
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    enum CommandReaderOptions
    {
        Default = 0,
        
        /// <summary>
        /// Treats characters (-abc) as multiple commands a, b, c
        /// </summary>
        AllowCompound = 0x1,
        
        /// <summary>
        /// Allows verbs to appear in the command stream. Verbs are ignored otherwise
        /// </summary>
        AllowVerbValues = 0x2,
        
        /// <summary>
        /// Provides command names as lower invariant values
        /// </summary>
        IgnoreCase = 0x4,
        
        /// <summary>
        /// Ignores the solidus (/) character as invariant command delimiter
        /// </summary>
        IgnoreInvariant = 0x8,
    }
}
