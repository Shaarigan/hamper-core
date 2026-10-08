// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Collections.Embedded;

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
    abstract partial class TrackedTokenReader<TokenType> : TokenReader<TokenType>, TrackedTokenReader<TokenType>.ITextData 
            where TokenType : struct, IComparable, IConvertible
    {
        private EmbeddedList<int, PoolArray<int>> buffer;
        
        private TextPointer caret;
        /// <inheritdoc/>
        public TextPointer Caret
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return caret; }
        }
        
        /// <summary>
        /// Gets if the caret should be updated on successfully reading a token
        /// </summary>
        protected abstract bool AutoAdvanceCaret { get; }

        /// <summary>
        /// Returns meta information about the processed text
        /// </summary>
        protected ITextData TextData
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this; }
        }

        /// <summary>
        /// Creates a new token reader from the given stream
        /// </summary>
        /// <param name="reader">The stream reader used to process the data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TrackedTokenReader(IStreamingContext reader)
            : base(reader)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.caret = TextPointer.Initial;
        }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="encoding">The encoding data is stored in the stream</param>
        /// <param name="bufferSize">The size of the buffer used to read from stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TrackedTokenReader(Stream baseStream, Encoding encoding, int bufferSize)
            : base(baseStream, encoding, bufferSize)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.caret = TextPointer.Initial;
        }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="encoding">The encoding data is stored in the stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TrackedTokenReader(Stream baseStream, Encoding encoding)
            : base(baseStream, encoding)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.caret = TextPointer.Initial;
        }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        /// <param name="bufferSize">The size of the buffer used to read from stream</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TrackedTokenReader(Stream baseStream, int bufferSize)
            : base(baseStream, bufferSize)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.caret = TextPointer.Initial;
        }
        /// <summary>
        /// Creates a new token reader from the given base stream
        /// </summary>
        /// <param name="baseStream">The stream this reader should act on</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TrackedTokenReader(Stream baseStream)
            : base(baseStream)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.caret = TextPointer.Initial;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Dispose()
        {
            buffer.Dispose();
            base.Dispose();
        }
        
        /// <summary>
        /// Trims the internal buffer up to the current position and updates the caret
        /// </summary>
        /// <param name="shift">True if the caret should be shifted, false otherwise</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TextPointer Advance(bool shift)
        {
            if (shift)
            {
                caret = TextData.GetCurrentCaretPosition();
            }
            Clear();

            return caret;
        }
        /// <summary>
        /// Trims the internal buffer up to the current position and updates the caret
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TextPointer Advance()
        {
            return Advance(true);
        }

        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>The token type read from the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override TokenType Read()
        {
            if (TryRead(out var result))
            {
                Advance(AutoAdvanceCaret);
            }
            return result;
        }
    }
}
