// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
    struct JsonFragment
    {
        JsonToken type;
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set { type = value; }
        }

        string name;
        /// <summary>
        /// Returns this fragments property name if any
        /// </summary>
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return name; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set { name = value; }
        }

        string rawValue;
        /// <summary>
        /// Returns this fragments property value
        /// </summary>
        public string RawValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return rawValue; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set { rawValue = value; }
        }

        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            this.type = JsonToken.Invalid;
            this.name = string.Empty;
            this.rawValue = string.Empty;
        }
        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        /// <param name="token">One of (BeginObject, EndObject, BeginArray, EndArray, Null, Boolean, Numeric, String)</param>
        public void Clear(JsonToken token)
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
                        this.name = string.Empty;
                        this.rawValue = string.Empty;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        /// <summary>
        /// Returns the contained raw data as bool value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToBoolean()
        {
            if (type == JsonToken.Boolean)
            {
                return Convert.ToBoolean(rawValue);
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 16 bit integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int16 ToInt16()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToInt16(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 16 bit unsigned integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt16 ToUInt16()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToUInt16(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 32 bit integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 ToInt32()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToInt32(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 32 bit unsigned integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt32 ToUInt32()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToUInt32(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 32 bit integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 ToInt64()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToInt64(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }
        
        /// <summary>
        /// Returns the contained raw data as 64 bit unsigned integer value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64 ToUInt64()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToUInt64(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 32 bit floating point value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Single ToSingle()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToSingle(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as 64 bit floating point value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double ToDouble()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.ToDouble(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as .NET decimal value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Decimal ToInteger()
        {
            if (type == JsonToken.Numeric)
            {
                return Decimal.Truncate(Convert.ToDecimal(rawValue));
            }
            else throw new InvalidCastException();
        }

        /// <summary>
        /// Returns the contained raw data as .NET decimal value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Decimal ToDecimal()
        {
            if (type == JsonToken.Numeric)
            {
                return Convert.ToDecimal(rawValue);
            }
            else throw new InvalidCastException();
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
                case JsonToken.String: return rawValue.ToString();
                default: return string.Empty;
            }
        }
    }
}
