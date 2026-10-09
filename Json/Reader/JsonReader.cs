// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Soe.Collections.Embedded;
using Soe.Parsing;

namespace Soe.Json
{
    /// <summary>
    /// A stream reader specialized to process text into JSON tokens
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    partial class JsonReader<Source> : TrackedTokenReader<JsonToken, Source>
        where Source : struct, IStreamSource
    {
        enum JsonProcessingFlags : byte
        {
            BeforeJson = 0,

            Object,
            Array,

            BeforeElement,
            AfterElement,

            BeforeProperty,
            AfterProperty,

            AfterJson
        }

        private BuildState<JsonProcessingFlags> state;
        private readonly StringBuilder buffer;

        TextRange match;
        /// <summary>
        /// The text range of current token
        /// </summary>
        public TextRange Match
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return match; }
        }

        private EmbeddedList<string, PoolArray<string>> errors;
        /// <summary>
        /// Gets a collection of error messages appeared while parsing JSON
        /// </summary>
        public ReadOnlySpan<string> Errors
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return errors.AsSpan(); }
        }

        JsonFragment fragment;
        /// <summary>
        /// Returns the current parser fragment
        /// </summary>
        public JsonFragment Fragment
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return fragment; }
        }

        /// <inheritdoc/>
        protected override bool AutoAdvanceCaret
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return true; }
        }

        /// <summary>
        /// Creates a new JSON reader from the given stream
        /// </summary>
        /// <param name="source">The streaming source used to read JSON data from</param>
        public JsonReader(Source source)
            : base(source)
        {
            this.buffer = StringBuilderPool.Shared.Rent();
            this.state = new BuildState<JsonProcessingFlags>(JsonProcessingFlags.AfterJson);
            this.state.Set(JsonProcessingFlags.BeforeJson);
            this.errors = new EmbeddedList<string, PoolArray<string>>();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Dispose()
        {
            errors.Dispose();
            StringBuilderPool.Shared.Return(buffer);
            state.Dispose();
        }

        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>True if a token could be read properly, false otherwise</returns>
        public new bool Read()
        {
            JsonToken lastToken = JsonToken.Invalid;

        Next:
            switch ((JsonProcessingFlags)state)
            {
                #region BeforeJson
                case JsonProcessingFlags.BeforeJson:
                    {
                        JsonToken token = base.Read();
                        switch (token)
                        {
                            #region Object
                            case JsonToken.BeginObject:
                                {
                                    state.Set(JsonProcessingFlags.BeforeProperty);
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Array
                            case JsonToken.BeginArray:
                                {
                                    state.Set(JsonProcessingFlags.BeforeElement);
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidJsonRoot, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }
                    }
                #endregion

                #region BeforeElement
                case JsonProcessingFlags.BeforeElement:
                    {
                        JsonToken token = base.Read();
                        switch (token)
                        {
                            #region Object
                            case JsonToken.BeginObject:
                                {
                                    state.Change(JsonProcessingFlags.AfterElement);
                                    state.Add(JsonProcessingFlags.BeforeProperty);
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Array
                            case JsonToken.BeginArray:
                                {
                                    state.Change(JsonProcessingFlags.AfterElement);
                                    state.Add(JsonProcessingFlags.BeforeElement);
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Null
                            case JsonToken.Null:
                                {
                                    state.Change(JsonProcessingFlags.AfterElement);
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Boolean
                            case JsonToken.True:
                            case JsonToken.False:
                                {
                                    state.Change(JsonProcessingFlags.AfterElement);
                                    fragment.Clear(JsonToken.Boolean);
                                    fragment.RawValue = token.ToString().ToLowerInvariant();
                                }
                                return true;
                            #endregion

                            #region Numeric
                            case JsonToken.Numeric:
                            #endregion

                            #region String
                            case JsonToken.String:
                                {
                                    state.Change(JsonProcessingFlags.AfterElement);
                                    fragment.Clear(token);
                                    fragment.RawValue = buffer.ToString();
                                }
                                return true;
                            #endregion

                            #region EndArray
                            case JsonToken.EndArray:
                                {
                                    if (lastToken == JsonToken.Comma)
                                    {
                                        errors.Add(string.Format(ErrorCodes.UnexpectedItemSeparator, match.Start.Line, match.Start.Column));
                                    }
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidArrayElement, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }
                    }
                #endregion

                #region AfterElement
                case JsonProcessingFlags.AfterElement:
                    {
                        JsonToken token = base.Read();
                        switch (token)
                        {
                            #region Comma
                            case JsonToken.Comma:
                                {
                                    state.Change(JsonProcessingFlags.BeforeElement);
                                    lastToken = token;
                                }
                                goto Next;
                            #endregion

                            #region EndArray
                            case JsonToken.EndArray:
                                {
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidArrayElement, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }
                    }
                #endregion

                #region BeforeProperty
                case JsonProcessingFlags.BeforeProperty:
                    {
                        string name;

                        JsonToken token = base.Read();
                        switch (token)
                        {
                            #region String
                            case JsonToken.String:
                                {
                                    name = buffer.ToString();
                                }
                                break;
                            #endregion

                            #region EndObject
                            case JsonToken.EndObject:
                                {
                                    if (lastToken == JsonToken.Comma)
                                    {
                                        errors.Add(string.Format(ErrorCodes.UnexpectedItemSeparator, match.Start.Line, match.Start.Column));
                                    }
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidObjectProperty, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }

                        token = base.Read();
                        switch (token)
                        {
                            #region Colon
                            case JsonToken.Colon:
                                break;
                            #endregion

                            #region EndObject
                            case JsonToken.EndObject:
                                {
                                    errors.Add(string.Format(ErrorCodes.IncompleteProperty, match.Start.Line, match.Start.Column));
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidObjectProperty, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }

                        token = base.Read();
                        switch (token)
                        {
                            #region Object
                            case JsonToken.BeginObject:
                                {
                                    state.Change(JsonProcessingFlags.AfterProperty);
                                    state.Add(JsonProcessingFlags.BeforeProperty);
                                    fragment.Clear(token);
                                    fragment.Name = name;
                                }
                                return true;
                            #endregion

                            #region Array
                            case JsonToken.BeginArray:
                                {
                                    state.Change(JsonProcessingFlags.AfterProperty);
                                    state.Add(JsonProcessingFlags.BeforeElement);
                                    fragment.Clear(token);
                                    fragment.Name = name;
                                }
                                return true;
                            #endregion

                            #region Null
                            case JsonToken.Null:
                                {
                                    state.Change(JsonProcessingFlags.AfterProperty);
                                    fragment.Clear(token);
                                    fragment.Name = name;
                                }
                                return true;
                            #endregion

                            #region Boolean
                            case JsonToken.True:
                            case JsonToken.False:
                                {
                                    state.Change(JsonProcessingFlags.AfterProperty);
                                    fragment.Clear(JsonToken.Boolean);
                                    fragment.RawValue = token.ToString().ToLowerInvariant();
                                    fragment.Name = name;
                                }
                                return true;
                            #endregion

                            #region Numeric
                            case JsonToken.Numeric:
                            #endregion

                            #region String
                            case JsonToken.String:
                                {
                                    state.Change(JsonProcessingFlags.AfterProperty);
                                    fragment.Clear(token);
                                    fragment.RawValue = buffer.ToString();
                                    fragment.Name = name;
                                }
                                return true;
                            #endregion

                            #region EndObject
                            case JsonToken.EndObject:
                                {
                                    errors.Add(string.Format(ErrorCodes.IncompleteProperty, match.Start.Line, match.Start.Column));
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidObjectProperty, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }
                    }
                #endregion

                #region AfterProperty
                case JsonProcessingFlags.AfterProperty:
                    {
                        JsonToken token = base.Read();
                        switch (token)
                        {
                            #region Comma
                            case JsonToken.Comma:
                                {
                                    state.Change(JsonProcessingFlags.BeforeProperty);
                                    lastToken = token;
                                }
                                goto Next;
                            #endregion

                            #region EndObject
                            case JsonToken.EndObject:
                                {
                                    state.Remove();
                                    fragment.Clear(token);
                                }
                                return true;
                            #endregion

                            #region Error
                            default:
                                {
                                    errors.Add(string.Format(ErrorCodes.InvalidObjectProperty, match.Start.Line, match.Start.Column, token));
                                    fragment.Clear();
                                    state.Remove();
                                }
                                return false;
                            #endregion
                        }
                    }
                #endregion

                #region AfterJson
                case JsonProcessingFlags.AfterJson:
                    {
                        fragment.Clear();
                    }
                    return false;
                #endregion
            }
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Concat
            (
                "Token: ", fragment.Type, 
                (fragment.Name != string.Empty) ? string.Concat(", Name: ", fragment.Name) : string.Empty,
                (fragment.RawValue != string.Empty) ? string.Concat(", Value: ", fragment.RawValue) : string.Empty
            );
        }
    }
}
