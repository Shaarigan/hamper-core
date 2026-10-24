// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;

namespace Soe.Runtime
{
    /// <summary>
    /// A command parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    struct CommandFragment<Source>
        where Source : struct, IStreamSource
    {
        private CommandReader<Source> source;
        private int nameLength;
        private int valueLength;
        
        private CommandToken type;
        /// <summary>
        /// Returns this fragments type
        /// </summary>
        public CommandToken Type
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return type; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set { type = value; }
        }

        /// <summary>
        /// Returns this fragments property name if any
        /// </summary>
        public CharacterSource<TextBuffer> Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source.GetCharacterSource(0, nameLength); }
        }
        
        /// <summary>
        /// Returns this fragments property value
        /// </summary>
        public CharacterSource<TextBuffer> RawValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source.GetCharacterSource(nameLength, valueLength); }
        }

        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            this.type = CommandToken.EndOfStream;
            this.source = null!;
            this.nameLength = 0;
            this.valueLength = 0;
        }
        
        // ReSharper disable ParameterHidesMember
        
        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        public void Clear(CommandToken token, CommandReader<Source> source, int nameLength, int valueLength)
        {
            this.type = token;
            this.source = source;
            this.nameLength = nameLength;
            this.valueLength = valueLength;
        }
        
        // ReSharper restore ParameterHidesMember

        /// <summary>
        /// Returns the contained raw data as bool value
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToBoolean(out bool result)
        {
            if (valueLength >= 4 && valueLength <= 5)
            {
                Span<char> rawValue = stackalloc char[valueLength];
                RawValue.CopyTo(nameLength, rawValue, valueLength);
                
                if (!bool.TryParse(rawValue, out result))
                {
                    result = false;
                    return false;
                }
                else return true;
            }
            else
            {
                result = false;
                return false;
            }
        }

        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out Int16 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToInt16(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out UInt16 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToUInt16(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out Int32 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToInt32(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out UInt32 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToUInt32(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out Int64 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToInt64(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out UInt64 result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToUInt64(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToInteger(out Decimal result)
        {
            if (ToDecimal(out result))
            {
                result = Decimal.Truncate(result);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToSingle(out float result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToSingle(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToDouble(out double result)
        {
            if (ToDecimal(out Decimal tmp))
            {
                result = Decimal.ToDouble(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToDecimal(out Decimal result)
        {
            if(valueLength > 0 && valueLength <= 32)
            {
                Span<char> rawValue = stackalloc char[valueLength];
                RawValue.CopyTo(nameLength, rawValue, valueLength);
                
                if (!Decimal.TryParse(rawValue, out result))
                {
                    result = 0;
                    return false;
                }
                else return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Returns the contained raw data as .NET string value
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return RawValue.ToString();
        }
    }
}
