// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;

namespace Soe.Runtime
{
    /// <summary>
    /// A stream reader specialized to process text into arguments
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    partial class CommandReader<Source> : TokenReader<CommandToken, Source>
        where Source : struct, IStreamSource
    {
        enum CommandProcessingFlags : byte
        {
            BeforeCommand = 0,
            BeforeValue,
        }

        private CommandToken preserved;
        private BuildState<CommandProcessingFlags> state;
        private readonly StringBuilder buffer;

        private readonly CommandReaderOptions options;
        /// <summary>
        /// Gets the configuration of the argument reader
        /// </summary>
        public CommandReaderOptions Options
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return options; }
        }
        
        private CommandFragment fragment;
        /// <summary>
        /// Returns the current parser fragment
        /// </summary>
        public CommandFragment Fragment
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return fragment; }
        }

        /// <summary>
        /// Creates a new command reader from the given stream
        /// </summary>
        /// <param name="source">The stream reader used to process the data</param>
        /// <param name="options">Configures the behavior of the argument reader</param>
        public CommandReader(Source source, CommandReaderOptions options = CommandReaderOptions.AllowVerbValues)
            : base(source)
        {
            this.options = options;
            this.state = new BuildState<CommandProcessingFlags>();
            this.buffer = StringBuilderPool.Shared.Rent();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Initialize(Source source)
        {
            base.Initialize(source);
            this.preserved = CommandToken.EndOfStream;
            this.buffer.Clear();
            this.state.Clear();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Dispose()
        {
            StringBuilderPool.Shared.Return(buffer);
        }

        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>True if a token could be read properly, false otherwise</returns>
        public new bool Read()
        {

        Next:
            switch ((CommandProcessingFlags)state)
            {
                #region BeforeCommand
                case CommandProcessingFlags.BeforeCommand:
                {
                    CommandToken token = ReadNext();
                    switch (token)
                    {
                        #region Argument
                        case CommandToken.ArgumentName:
                            {
                                fragment.Clear(CommandToken.Command);
                                if (options.HasFlag(CommandReaderOptions.IgnoreCase))
                                {
                                    fragment.Name = buffer.ToString()
                                        .ToLowerInvariant();
                                }
                                else fragment.Name = buffer.ToString();
                                state.Set(CommandProcessingFlags.BeforeValue);
                            }
                            goto Next;
                        #endregion
                        
                        #region Argument
                        case CommandToken.CompoundArgument:
                            {
                                bool hasCommandsLeft;
                                
                                fragment.Clear(CommandToken.Command);
                                if(buffer.Length > 1)
                                {
                                    preserved = CommandToken.CompoundArgument;
                                    hasCommandsLeft = true;
                                }
                                else hasCommandsLeft = false;
                                if (options.HasFlag(CommandReaderOptions.IgnoreCase))
                                {
                                    fragment.Name = buffer[0].ToString()
                                        .ToLowerInvariant();
                                }
                                else fragment.Name = buffer[0].ToString();
                                buffer.Remove(0, 1);

                                if (!hasCommandsLeft)
                                {
                                    state.Set(CommandProcessingFlags.BeforeValue);
                                }
                                else return true;
                            }
                            goto Next;
                        #endregion
                        
                        #region VerbValue
                        case CommandToken.ArgumentValue: if(options.FlagSet(CommandReaderOptions.AllowVerbValues))
                            {
                                fragment.Clear(CommandToken.VerbValue);
                                fragment.RawValue = buffer.ToString();
                                return true;
                            }
                            else goto Next;
                        #endregion
                        
                        #region PlainText
                        case CommandToken.PlainText:
                            {
                                fragment.Clear(CommandToken.PlainText);
                                fragment.RawValue = buffer.ToString();
                            }
                            return true;
                        #endregion

                        #region EOF
                        case CommandToken.EndOfStream: return false;
                        #endregion
                        
                        #region Invalid
                        default: goto Next;
                        #endregion
                    }
                }
                #endregion
                
                #region BeforeValue
                case CommandProcessingFlags.BeforeValue:
                {
                    CommandToken token = ReadNext();
                    switch (token)
                    {
                        #region Value
                        case CommandToken.ArgumentValue:
                            {
                                fragment.RawValue = buffer.ToString();
                                state.Reset();
                            }
                            return true;
                        #endregion
                        
                        #region Invalid
                        default:
                            {
                                preserved = token;
                                state.Reset();
                            }
                            return true;
                        #endregion
                    }
                }
                #endregion
            }
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        CommandToken ReadNext()
        {
            if (preserved != CommandToken.EndOfStream)
            {
                CommandToken token = preserved;
                preserved = CommandToken.EndOfStream;
                
                return token;
            }
            else return base.Read();
        }
    }
}
