// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;

namespace Soe.Json
{
    /// <summary>
    /// A JSON parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    struct JsonFragment<Source>
        where Source : struct, IStreamSource
    {
        private JsonReader<Source> source;
        private int nameLength;
        private int valueLength;
        
        private JsonToken type;
        /// <summary>
        /// Returns this fragments type
        /// </summary>
        /// <remarks>
        /// Must be one of (BeginObject, EndObject, BeginArray, EndArray, Null, Boolean, Numeric, String)
        /// </remarks>
        public JsonToken Type
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return type; }
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
            type = JsonToken.Invalid;
            source = null!;
            nameLength = 0;
            valueLength = 0;
        }
        
        // ReSharper disable ParameterHidesMember
        
        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        /// <param name="token">One of (BeginObject, EndObject, BeginArray, EndArray, Null, Boolean, Numeric, String)</param>
        /// <param name="source">The root source of this fragment</param>
        /// <param name="nameLength">The length of the fragment name</param>
        /// <param name="valueLength">The length of the fragment value</param>
        public void Clear(JsonToken token, JsonReader<Source> source, int nameLength, int valueLength)
        {
            switch (token)
            {
                case JsonToken.BeginObject:
                case JsonToken.EndObject:
                case JsonToken.BeginArray:
                case JsonToken.EndArray:
                case JsonToken.Null:
                case JsonToken.Boolean:
                case JsonToken.Numeric:
                case JsonToken.String:
                    {
                        this.type = token;
                        this.source = source;
                        this.nameLength = nameLength;
                        this.valueLength = valueLength;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(token));
            }
        }
        
        // ReSharper restore ParameterHidesMember

        /// <summary>
        /// Returns the contained raw data as bool value
        /// </summary>
        public bool ToBoolean(out bool result)
        {
            if (valueLength >= 4 && valueLength <= 5)
            {
                Span<char> rawValue = stackalloc char[valueLength];
                RawValue.CopyTo(nameLength, rawValue, valueLength);
                
                if (type != JsonToken.Boolean || !bool.TryParse(rawValue, out result))
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
        public bool ToDecimal(out Decimal result)
        {
            if(valueLength > 0 && valueLength <= 32)
            {
                Span<char> rawValue = stackalloc char[valueLength];
                RawValue.CopyTo(nameLength, rawValue, valueLength);
                
                if (type != JsonToken.Numeric || !Decimal.TryParse(rawValue, out result))
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            switch(type)
            {
                case JsonToken.Null: return type.ToString();
                case JsonToken.Boolean:
                case JsonToken.Numeric:
                case JsonToken.String: return Name.ToString();
                default: return string.Empty;
            }
        }
    }
}
