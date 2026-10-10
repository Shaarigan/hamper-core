// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Defines valid command tokens
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    enum CommandToken : byte
    {
        EndOfStream = 0,

        ArgumentName = 1,
        ArgumentValue = 2,
        CompoundArgument = 4,
        
        Command = 6,
        VerbValue = 7,
        PlainText,
    }
}
