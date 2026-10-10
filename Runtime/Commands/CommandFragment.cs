// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    /// <summary>
    /// A command parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    struct CommandFragment
    {
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

        private string name;
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

        private string rawValue;
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
            this.type = CommandToken.EndOfStream;
            this.name = string.Empty;
            this.rawValue = string.Empty;
        }
        /// <summary>
        /// Resets this fragment to default
        /// </summary>
        public void Clear(CommandToken token)
        {
            this.type = token;
            this.name = string.Empty;
            this.rawValue = string.Empty;
        }

        /// <summary>
        /// Returns the contained raw data as bool value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToBoolean(out bool result)
        {
            if (!bool.TryParse(rawValue, out result))
            {
                result = false;
                return false;
            }
            else return true;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToDecimal(out Decimal result)
        {
            if (!Decimal.TryParse(rawValue, out result))
            {
                result = 0;
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Returns the contained raw data as .NET string value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return rawValue;
        }
    }
}
