// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Parsing
{
    /// <summary>
    /// A wrapper class embedding a string into a streaming context
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    class StringContext : IStreamingContext
    {
        private readonly string? identifier;
        private readonly int length;
        
        int position;
        /// <summary>
        /// Gets the position within the current string
        /// </summary>
        public int Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { position = value.Clamp(0, length); }
        }

        /// <inheritdoc/>
        public Encoding CurrentEncoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Encoding.Unicode; }
        }
        
        /// <inheritdoc/>
        public bool EndOfStream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (position == length); }
        }

        /// <summary>
        /// Initializes a new wrapper that reads from the specified string
        /// </summary>
        /// <param name="identifier">The string to which the wrapper is initialized from</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringContext(string? identifier) 
        {
            this.identifier = identifier;
            this.length = (identifier?.Length ?? 0);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek() 
        {
            if (!EndOfStream)
            {
                return identifier![position];
            }
            else return -1;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (!EndOfStream)
            {
                return identifier![position++];
            }
            else return -1;
        }
    }
}
