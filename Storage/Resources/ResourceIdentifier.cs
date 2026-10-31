// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Provides an object representation of a unified resource identifier and it's various properties
    /// </summary>
    /// <param name="identifier">The source of this identifier</param>
    /// <param name="meta">The metadata of the identifier</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier(string identifier, ResourceMetaData meta)
        : IEquatable<ResourceIdentifier>
    {
        private readonly string identifier = identifier;
        private readonly ResourceMetaData meta = meta;
        
        /// <summary>
        /// Gets if this unified resource identifier is absolute
        /// </summary>
        public bool IsAbsolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return !IsRelative; }
        }

        /// <summary>
        /// Gets if this unified resource identifier is relative to a certain root
        /// </summary>
        public bool IsRelative
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return meta.Flags.HasFlag(IdentifierFlags.IsRelative); }
        }

        /// <summary>
        /// Gets if this unified resource identifier points to the root node
        /// </summary>
        public bool IsRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ((meta.Name - meta.Path) == 1 && 
                          (meta.Arguments - meta.Name) == 0 && 
                          identifier[meta.Path] == PathSeparator
            ); }
        }

        /// <summary>
        /// Gets if this unified resource identifier points to a full qualified path or location
        /// </summary>
        public bool IsPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ((meta.Name - meta.Path) > 0); }
        }

        /// <summary>
        /// Gets if this unified resource identifier has an authority component set. This is true
        /// for network paths or remote locations
        /// </summary>
        public bool IsUnc
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return meta.Flags.FlagSet(IdentifierFlags.IsUnc); }
        }

        /// <summary>
        /// Gets the name of an authority handling the location this unified resource identifier points to
        /// </summary>
        public ReadOnlySpan<char> Authority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.AsSpan(IsUnc ? 2 : 0, meta.Path); }
        }

        /// <summary>
        /// Gets the full qualified path or location this unified resource identifier points to
        /// </summary>
        public ReadOnlySpan<char> Path
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.AsSpan(meta.Path, meta.Name - meta.Path); }
        }

        /// <summary>
        /// Gets the name of the resource handled by this unified resource identifier
        /// </summary>
        public ReadOnlySpan<char> Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.AsSpan(meta.Name, meta.Arguments - meta.Name); }
        }

        /// <summary>
        /// Gets the length of this unified resource identifier
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (meta.Arguments - meta.Path); }
        }

        /// <summary>
        /// Gets the character contained at certain index
        /// </summary>
        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get 
            {
                if (index >= 0 && index <= (meta.Arguments - meta.Path))
                {
                    return identifier[meta.Path + index];
                }
                else throw new IndexOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ResourceMetaData(ResourceIdentifier identifier)
        {
            return identifier.meta;
        }

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceIdentifier operator /(ResourceIdentifier left, ResourceIdentifier right)
        {
            return Combine(left, right);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceIdentifier operator /(string left, ResourceIdentifier right)
        {
            if (string.IsNullOrWhiteSpace(left))
            {
                throw new ArgumentOutOfRangeException("left");
            }
            return Combine(new ResourceIdentifier(left), right);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceIdentifier operator /(ResourceIdentifier left, string right)
        {
            if (string.IsNullOrWhiteSpace(right))
            {
                throw new ArgumentOutOfRangeException("right");
            }
            return Combine(left, new ResourceIdentifier(right));
        }*/
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ResourceIdentifier other)
        {
            return string.Equals(identifier, other.identifier, StringComparison.InvariantCulture);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is ResourceIdentifier other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return identifier.GetHashCode();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return identifier.Substring(meta.Path, meta.Arguments - meta.Path);
        }
    }
}