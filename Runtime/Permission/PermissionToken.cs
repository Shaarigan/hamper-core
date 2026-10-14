// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Crypto;

namespace Soe.Runtime
{
    /// <summary>
    /// Represents a permission token handled by the <see cref="PermissionManager"/>
    /// </summary>
    /// <param name="value">A cryptographic signature pair over a certain cryptographic hash function</param>
    #if EXPORT_HAMPER_CORE_RUNTIME_PERMISSION
    public
    #else
    internal
    #endif
    readonly struct PermissionToken(Point128 value)
    {
        /// <summary>
        /// Gets the cryptographic signature managed by this token
        /// </summary>
        public ReadOnlyIterator<UInt64, ReadOnlyIterator<UInt64>.DefaultStrategy, UInt64, ReadOnlyIterator<UInt64>.DefaultStrategy> Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ReadOnlyIterator<ulong, ReadOnlyIterator<ulong>.DefaultStrategy, ulong, ReadOnlyIterator<ulong>.DefaultStrategy>(value.X, value.Y); }
        }

        /// <summary>
        /// Verifies this token by the provided cryptographic key and hash value
        /// </summary>
        /// <param name="key">A cryptographic key to verify this token</param>
        /// <param name="hash">The cryptographic hash value to verify</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Verify(in Point128 key, Span<byte> hash)
        {
            return Secp128r1.Instance.Verify(in key, hash, value);
        }
        
        /// <summary>
        /// Returns an iterator that iterates through the cryptography signature managed by this token
        /// </summary>
        /// <returns>The iterator handling the underlying cryptography signature</returns>
        /// <remarks>The method must be named GetEnumerator to be recognized by foreach</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyIterator<UInt64, ReadOnlyIterator<UInt64>.DefaultStrategy, UInt64, ReadOnlyIterator<UInt64>.DefaultStrategy> GetEnumerator()
        {
            return Value;
        }
    }
}