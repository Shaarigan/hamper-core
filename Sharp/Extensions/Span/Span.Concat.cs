// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class SpanExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char> tuple)
        {
            return tuple.Item1.ToString();
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + tuple.Item2.Length, tuple, Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + 
                          tuple.Item2.Length + 
                          tuple.Item3.Length, 
                tuple, 
                Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
                offset += tuple.Item2.Length;

                tuple.Item3.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            if (length > 0 && !tuple.Item3.IsEmpty)
            {
                length++;
            }
            length += tuple.Item3.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
                offset += t.Item2.Length;
                
                if (offset > 0 && !t.Item3.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item3.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + 
                          tuple.Item2.Length + 
                          tuple.Item3.Length + 
                          tuple.Item4.Length, 
                tuple, 
                Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
                offset += tuple.Item2.Length;

                tuple.Item3.CopyTo(destination.Slice(offset));
                offset += tuple.Item3.Length;

                tuple.Item4.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            if (length > 0 && !tuple.Item3.IsEmpty)
            {
                length++;
            }
            length += tuple.Item3.Length;
            if (length > 0 && !tuple.Item4.IsEmpty)
            {
                length++;
            }
            length += tuple.Item4.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
                offset += t.Item2.Length;
                
                if (offset > 0 && !t.Item3.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item3.CopyTo(destination.Slice(offset));
                offset += t.Item3.Length;
                
                if (offset > 0 && !t.Item4.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item4.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + 
                          tuple.Item2.Length + 
                          tuple.Item3.Length + 
                          tuple.Item4.Length + 
                          tuple.Item5.Length, 
                tuple, 
                Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
                offset += tuple.Item2.Length;

                tuple.Item3.CopyTo(destination.Slice(offset));
                offset += tuple.Item3.Length;

                tuple.Item4.CopyTo(destination.Slice(offset));
                offset += tuple.Item4.Length;

                tuple.Item5.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            if (length > 0 && !tuple.Item3.IsEmpty)
            {
                length++;
            }
            length += tuple.Item3.Length;
            if (length > 0 && !tuple.Item4.IsEmpty)
            {
                length++;
            }
            length += tuple.Item4.Length;
            if (length > 0 && !tuple.Item5.IsEmpty)
            {
                length++;
            }
            length += tuple.Item5.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
                offset += t.Item2.Length;
                
                if (offset > 0 && !t.Item3.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item3.CopyTo(destination.Slice(offset));
                offset += t.Item3.Length;
                
                if (offset > 0 && !t.Item4.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item4.CopyTo(destination.Slice(offset));
                offset += t.Item4.Length;
                
                if (offset > 0 && !t.Item5.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item5.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + 
                          tuple.Item2.Length + 
                          tuple.Item3.Length + 
                          tuple.Item4.Length + 
                          tuple.Item5.Length + 
                          tuple.Item6.Length, 
                tuple, 
                Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
                offset += tuple.Item2.Length;

                tuple.Item3.CopyTo(destination.Slice(offset));
                offset += tuple.Item3.Length;

                tuple.Item4.CopyTo(destination.Slice(offset));
                offset += tuple.Item4.Length;

                tuple.Item5.CopyTo(destination.Slice(offset));
                offset += tuple.Item5.Length;

                tuple.Item6.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            if (length > 0 && !tuple.Item3.IsEmpty)
            {
                length++;
            }
            length += tuple.Item3.Length;
            if (length > 0 && !tuple.Item4.IsEmpty)
            {
                length++;
            }
            length += tuple.Item4.Length;
            if (length > 0 && !tuple.Item5.IsEmpty)
            {
                length++;
            }
            length += tuple.Item5.Length;
            if (length > 0 && !tuple.Item6.IsEmpty)
            {
                length++;
            }
            length += tuple.Item6.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
                offset += t.Item2.Length;
                
                if (offset > 0 && !t.Item3.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item3.CopyTo(destination.Slice(offset));
                offset += t.Item3.Length;
                
                if (offset > 0 && !t.Item4.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item4.CopyTo(destination.Slice(offset));
                offset += t.Item4.Length;
                
                if (offset > 0 && !t.Item5.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item5.CopyTo(destination.Slice(offset));
                offset += t.Item5.Length;
                
                if (offset > 0 && !t.Item6.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item6.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Item1.Length + 
                          tuple.Item2.Length + 
                          tuple.Item3.Length + 
                          tuple.Item4.Length + 
                          tuple.Item5.Length + 
                          tuple.Item6.Length + 
                          tuple.Item7.Length, 
                tuple, 
                Create);

            static void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char, char> tuple)
            {
                int offset = 0;

                tuple.Item1.CopyTo(destination.Slice(offset));
                offset += tuple.Item1.Length;

                tuple.Item2.CopyTo(destination.Slice(offset));
                offset += tuple.Item2.Length;

                tuple.Item3.CopyTo(destination.Slice(offset));
                offset += tuple.Item3.Length;

                tuple.Item4.CopyTo(destination.Slice(offset));
                offset += tuple.Item4.Length;

                tuple.Item5.CopyTo(destination.Slice(offset));
                offset += tuple.Item5.Length;

                tuple.Item6.CopyTo(destination.Slice(offset));
                offset += tuple.Item6.Length;

                tuple.Item7.CopyTo(destination.Slice(offset));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char, char> tuple, char separator)
        {
            int length = tuple.Item1.Length;
            if (length > 0 && !tuple.Item2.IsEmpty)
            {
                length++;
            }
            length += tuple.Item2.Length;
            if (length > 0 && !tuple.Item3.IsEmpty)
            {
                length++;
            }
            length += tuple.Item3.Length;
            if (length > 0 && !tuple.Item4.IsEmpty)
            {
                length++;
            }
            length += tuple.Item4.Length;
            if (length > 0 && !tuple.Item5.IsEmpty)
            {
                length++;
            }
            length += tuple.Item5.Length;
            if (length > 0 && !tuple.Item6.IsEmpty)
            {
                length++;
            }
            length += tuple.Item6.Length;
            if (length > 0 && !tuple.Item7.IsEmpty)
            {
                length++;
            }
            length += tuple.Item7.Length;
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char, char> t)
            {
                int offset = 0;

                t.Item1.CopyTo(destination.Slice(offset));
                offset += t.Item1.Length;

                if (offset > 0 && !t.Item2.IsEmpty)
                {
                    destination[offset++] = separator;
                }
                
                t.Item2.CopyTo(destination.Slice(offset));
                offset += t.Item2.Length;
                
                if (offset > 0 && !t.Item3.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item3.CopyTo(destination.Slice(offset));
                offset += t.Item3.Length;
                
                if (offset > 0 && !t.Item4.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item4.CopyTo(destination.Slice(offset));
                offset += t.Item4.Length;
                
                if (offset > 0 && !t.Item5.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item5.CopyTo(destination.Slice(offset));
                offset += t.Item5.Length;
                
                if (offset > 0 && !t.Item6.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item6.CopyTo(destination.Slice(offset));
                offset += t.Item6.Length;
                
                if (offset > 0 && !t.Item7.IsEmpty)
                {
                    destination[offset++] = separator;
                }

                t.Item7.CopyTo(destination.Slice(offset));
            }
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}