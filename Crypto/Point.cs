// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Crypto
{
    /// <summary>
    /// Elliptic Curve Point
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    class Point
    {
        private readonly int size;
        /// <summary>
        /// Size of 64 bit segments per value in this curve point
        /// </summary>
        public int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return size; }
        }

        private readonly UInt64[] x;
        /// <summary>
        /// X Coord of this curve point
        /// </summary>
        public UInt64[] X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return x; }
        }

        private readonly UInt64[] y;
        /// <summary>
        /// Y Coord of this curve point
        /// </summary>
        public UInt64[] Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return y; }
        }

        /// <summary>
        /// Creates a new curve point
        /// </summary>
        /// <param name="size">Size of 64 bit segments per value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point(int size)
        {
            this.size = size;
            this.x = new UInt64[size];
            this.y = new UInt64[size];
        }
    }
}
