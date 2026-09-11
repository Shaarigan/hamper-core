// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Composable
{
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    readonly partial struct ComponentId : IEquatable<ComponentId>
    {
        [FieldOffset(0)]
        readonly UInt64 value;

        [FieldOffset(0)]
        readonly int index;

        public int Index
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return index; }
        }

        [FieldOffset(4)]
        readonly byte shard;

        public int Shard
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return shard; }
        }
        
		public ComponentId(int index, int shard)
        {
            this.index = index;
            this.shard = (byte)shard;
        }
        
		public ComponentId(UInt64 value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ComponentId(UInt64 value)
        {
            return new ComponentId(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt64(ComponentId entity)
        {
            return entity.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ComponentId other)
        {
            return value == other.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is ComponentId other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"{{Index: {index}, Shard: {shard}}}";
        }
    }
}
