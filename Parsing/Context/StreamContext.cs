// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Text;

namespace Soe.Parsing
{
    /// <summary>
    /// A wrapper class embedding a stream into a streaming context
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    class StreamingContext : StreamReader, IStreamingContext
    {
        /// <summary>
        /// Initializes a new wrapper that reads from the specified stream
        /// </summary>
        public StreamingContext(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        { }
    }
}
