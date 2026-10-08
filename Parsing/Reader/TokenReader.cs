// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Collections.Embedded;
using Soe.Unicode;

namespace Soe.Parsing
{
    /// <summary>
    /// A stream reader specialized to process text into tokens of certain grammar
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    abstract partial class TokenReader<TokenType> : TokenReader<TokenType>.IStreamBuffer, IReadOnlyIterable<int, ReadOnlyIterator<int>.DefaultStrategy>, IReadOnlySequence<int>, IDisposable 
        where TokenType : struct, IComparable, IConvertible
    {
        /// <summary>
        /// The default size of the internal <see cref="IStreamBuffer"/> in bytes
        /// </summary>
        public const int DefaultBufferSize = 4096;

        private readonly IStreamingContext reader;
        private EmbeddedList<int, PoolArray<int>> buffer;

        private int position;
        /// <summary>
        /// Gets the position within the current character buffer
        /// </summary>
        public int Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected set { StreamBuffer.Position = value; }
        }

        /// <summary>
        /// Gets the current character encoding used to parse tokens
        /// </summary>
        public Encoding Encoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return reader.CurrentEncoding; }
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the
        /// end of the stream and there are no tokens left to process
        /// </summary>
        public bool EndOfStream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return reader.EndOfStream && (position == buffer.Count); }
        }

        /// <summary>
        /// The character considered as currently pointed to
        /// </summary>
        protected Char32 Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (position == buffer.Count)
                {
                    return Char32.Invalid;
                }
                else return buffer[position];
            }
        }

        /// <summary>
        /// Returns the underlying Unicode buffer
        /// </summary>
        protected IStreamBuffer StreamBuffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this; }
        }

        /// <summary>
        /// Creates a new token reader from the given stream
        /// </summary>
        /// <param name="reader">The stream reader used to process the data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(IStreamingContext reader)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.reader = reader;
        }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="encoding">The encoding data is stored in the stream</param>
        /// <param name="bufferSize">The size of the buffer used to read from stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(Stream baseStream, Encoding encoding, int bufferSize)
            : this(new StreamingContext(baseStream, encoding, false, bufferSize, true))
        { }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="encoding">The encoding data is stored in the stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(Stream baseStream, Encoding encoding)
            : this(baseStream, encoding, DefaultBufferSize)
        { }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="bufferSize">The size of the buffer used to read from stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(Stream baseStream, int bufferSize)
            : this(new StreamingContext(baseStream, Encoding.UTF8, true, bufferSize, true))
        { }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(Stream baseStream)
            : this(baseStream, DefaultBufferSize)
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<int> AsReadOnlySpan()
        {
            return buffer.AsSpan();
        }
        
        /// <summary>
        /// Trims the internal buffer up to the current position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Clear()
        {
            buffer.Clear();
            position = 0;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Dispose()
        {
            buffer.Dispose();
            reader.Dispose();
        }
        
        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>The token type read from the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual TokenType Read()
        {
            if (TryRead(out TokenType result))
            {
                Clear();
            }
            return result;
        }

        /// <summary>
        /// Reads the next token from the input stream if possible
        /// </summary>
        /// <param name="token">The token type read from the stream</param>
        /// <returns>True if a token was successfully read, false otherwise</returns>
        protected abstract bool TryRead(out TokenType token);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyIterator<int, ReadOnlyIterator<int>.DefaultStrategy> GetEnumerator()
        {
            return new ReadOnlyIterator<int, ReadOnlyIterator<int>.DefaultStrategy>(AsReadOnlySpan());
        }
    }
}
