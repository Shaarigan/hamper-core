// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2> tuple)
        {
            return tuple.ref1 && tuple.ref2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2,
            out ReferenceTuple<T1, Policy1, T2, Policy2> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2>(ref1, ref2);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3>(ref1, ref2, ref3);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(ref1, ref2, ref3, ref4);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
        where T5 : class, IOwnable<T5>
        where Policy5 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }
        
        private ScopedReference<T5, Policy5> ref5;
        /// <summary>
        /// Gets the fifth reference in this tuple
        /// </summary>
        public ScopedReference<T5, Policy5> Ref5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref5; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4, ScopedReference<T5, Policy5> ref5)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
            this.ref5 = ref5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4 && tuple.ref5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
            ref5.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!t5.TryBorrow(out ref5))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(ref1, ref2, ref3, ref4, ref5);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
        where T5 : class, IOwnable<T5>
        where Policy5 : struct, IAccessPolicy
        where T6 : class, IOwnable<T6>
        where Policy6 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }
        
        private ScopedReference<T5, Policy5> ref5;
        /// <summary>
        /// Gets the fifth reference in this tuple
        /// </summary>
        public ScopedReference<T5, Policy5> Ref5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref5; }
        }
        
        private ScopedReference<T6, Policy6> ref6;
        /// <summary>
        /// Gets the sixth reference in this tuple
        /// </summary>
        public ScopedReference<T6, Policy6> Ref6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref6; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4, ScopedReference<T5, Policy5> ref5,
            ScopedReference<T6, Policy6> ref6)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
            this.ref5 = ref5;
            this.ref6 = ref6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4 && tuple.ref5 && tuple.ref6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
            ref5.Dispose();
            ref6.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;
            ScopedReference<T6, Policy6> ref6 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!t5.TryBorrow(out ref5))
            {
                success = false;
                goto Finalize;
            }
            if (!t6.TryBorrow(out ref6))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(ref1, ref2, ref3, ref4, ref5, ref6);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
        where T5 : class, IOwnable<T5>
        where Policy5 : struct, IAccessPolicy
        where T6 : class, IOwnable<T6>
        where Policy6 : struct, IAccessPolicy
        where T7 : class, IOwnable<T7>
        where Policy7 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }
        
        private ScopedReference<T5, Policy5> ref5;
        /// <summary>
        /// Gets the fifth reference in this tuple
        /// </summary>
        public ScopedReference<T5, Policy5> Ref5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref5; }
        }
        
        private ScopedReference<T6, Policy6> ref6;
        /// <summary>
        /// Gets the sixth reference in this tuple
        /// </summary>
        public ScopedReference<T6, Policy6> Ref6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref6; }
        }
        
        private ScopedReference<T7, Policy7> ref7;
        /// <summary>
        /// Gets the seventh reference in this tuple
        /// </summary>
        public ScopedReference<T7, Policy7> Ref7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref7; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4, ScopedReference<T5, Policy5> ref5,
            ScopedReference<T6, Policy6> ref6, ScopedReference<T7, Policy7> ref7)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
            this.ref5 = ref5;
            this.ref6 = ref6;
            this.ref7 = ref7;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4 && tuple.ref5 && tuple.ref6 && tuple.ref7;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
            ref5.Dispose();
            ref6.Dispose();
            ref7.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;
            ScopedReference<T6, Policy6> ref6 = default;
            ScopedReference<T7, Policy7> ref7 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!t5.TryBorrow(out ref5))
            {
                success = false;
                goto Finalize;
            }
            if (!t6.TryBorrow(out ref6))
            {
                success = false;
                goto Finalize;
            }
            if (!t7.TryBorrow(out ref7))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7,
                    Policy7>(ref1, ref2, ref3, ref4, ref5, ref6, ref7);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
        where T5 : class, IOwnable<T5>
        where Policy5 : struct, IAccessPolicy
        where T6 : class, IOwnable<T6>
        where Policy6 : struct, IAccessPolicy
        where T7 : class, IOwnable<T7>
        where Policy7 : struct, IAccessPolicy
        where T8 : class, IOwnable<T8>
        where Policy8 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }
        
        private ScopedReference<T5, Policy5> ref5;
        /// <summary>
        /// Gets the fifth reference in this tuple
        /// </summary>
        public ScopedReference<T5, Policy5> Ref5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref5; }
        }
        
        private ScopedReference<T6, Policy6> ref6;
        /// <summary>
        /// Gets the sixth reference in this tuple
        /// </summary>
        public ScopedReference<T6, Policy6> Ref6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref6; }
        }
        
        private ScopedReference<T7, Policy7> ref7;
        /// <summary>
        /// Gets the seventh reference in this tuple
        /// </summary>
        public ScopedReference<T7, Policy7> Ref7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref7; }
        }
        
        private ScopedReference<T8, Policy8> ref8;
        /// <summary>
        /// Gets the eight reference in this tuple
        /// </summary>
        public ScopedReference<T8, Policy8> Ref8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref8; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4, ScopedReference<T5, Policy5> ref5,
            ScopedReference<T6, Policy6> ref6, ScopedReference<T7, Policy7> ref7, ScopedReference<T8, Policy8> ref8)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
            this.ref5 = ref5;
            this.ref6 = ref6;
            this.ref7 = ref7;
            this.ref8 = ref8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4 && tuple.ref5 && tuple.ref6 && tuple.ref7 &&
                   tuple.ref8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
            ref5.Dispose();
            ref6.Dispose();
            ref7.Dispose();
            ref8.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7
                , T8, Policy8> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;
            ScopedReference<T6, Policy6> ref6 = default;
            ScopedReference<T7, Policy7> ref7 = default;
            ScopedReference<T8, Policy8> ref8 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!t5.TryBorrow(out ref5))
            {
                success = false;
                goto Finalize;
            }
            if (!t6.TryBorrow(out ref6))
            {
                success = false;
                goto Finalize;
            }
            if (!t7.TryBorrow(out ref7))
            {
                success = false;
                goto Finalize;
            }
            if (!t8.TryBorrow(out ref8))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7,
                    Policy7, T8, Policy8>(ref1, ref2, ref3, ref4, ref5, ref6, ref7, ref8);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
    
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    ref struct ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8, T9, Policy9>
        where T1 : class, IOwnable<T1>
        where Policy1 : struct, IAccessPolicy
        where T2 : class, IOwnable<T2>
        where Policy2 : struct, IAccessPolicy
        where T3 : class, IOwnable<T3>
        where Policy3 : struct, IAccessPolicy
        where T4 : class, IOwnable<T4>
        where Policy4 : struct, IAccessPolicy
        where T5 : class, IOwnable<T5>
        where Policy5 : struct, IAccessPolicy
        where T6 : class, IOwnable<T6>
        where Policy6 : struct, IAccessPolicy
        where T7 : class, IOwnable<T7>
        where Policy7 : struct, IAccessPolicy
        where T8 : class, IOwnable<T8>
        where Policy8 : struct, IAccessPolicy
        where T9 : class, IOwnable<T9>
        where Policy9 : struct, IAccessPolicy
    {
        private ScopedReference<T1, Policy1> ref1;
        /// <summary>
        /// Gets the first reference in this tuple
        /// </summary>
        public ScopedReference<T1, Policy1> Ref1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref1; }
        }
        
        private ScopedReference<T2, Policy2> ref2;
        /// <summary>
        /// Gets the second reference in this tuple
        /// </summary>
        public ScopedReference<T2, Policy2> Ref2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref2; }
        }
        
        private ScopedReference<T3, Policy3> ref3;
        /// <summary>
        /// Gets the third reference in this tuple
        /// </summary>
        public ScopedReference<T3, Policy3> Ref3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref3; }
        }
        
        private ScopedReference<T4, Policy4> ref4;
        /// <summary>
        /// Gets the fourth reference in this tuple
        /// </summary>
        public ScopedReference<T4, Policy4> Ref4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref4; }
        }
        
        private ScopedReference<T5, Policy5> ref5;
        /// <summary>
        /// Gets the fifth reference in this tuple
        /// </summary>
        public ScopedReference<T5, Policy5> Ref5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref5; }
        }
        
        private ScopedReference<T6, Policy6> ref6;
        /// <summary>
        /// Gets the sixth reference in this tuple
        /// </summary>
        public ScopedReference<T6, Policy6> Ref6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref6; }
        }
        
        private ScopedReference<T7, Policy7> ref7;
        /// <summary>
        /// Gets the seventh reference in this tuple
        /// </summary>
        public ScopedReference<T7, Policy7> Ref7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref7; }
        }
        
        private ScopedReference<T8, Policy8> ref8;
        /// <summary>
        /// Gets the eight reference in this tuple
        /// </summary>
        public ScopedReference<T8, Policy8> Ref8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref8; }
        }
        
        private ScopedReference<T9, Policy9> ref9;
        /// <summary>
        /// Gets the ninth reference in this tuple
        /// </summary>
        public ScopedReference<T9, Policy9> Ref9
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref9; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReferenceTuple(ScopedReference<T1, Policy1> ref1, ScopedReference<T2, Policy2> ref2,
            ScopedReference<T3, Policy3> ref3, ScopedReference<T4, Policy4> ref4, ScopedReference<T5, Policy5> ref5,
            ScopedReference<T6, Policy6> ref6, ScopedReference<T7, Policy7> ref7, ScopedReference<T8, Policy8> ref8,
            ScopedReference<T9, Policy9> ref9)
        {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
            this.ref4 = ref4;
            this.ref5 = ref5;
            this.ref6 = ref6;
            this.ref7 = ref7;
            this.ref8 = ref8;
            this.ref9 = ref9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8, T9, Policy9> tuple)
        {
            return tuple.ref1 && tuple.ref2 && tuple.ref3 && tuple.ref4 && tuple.ref5 && tuple.ref6 && tuple.ref7 &&
                   tuple.ref8 && tuple.ref9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ref1.Dispose();
            ref2.Dispose();
            ref3.Dispose();
            ref4.Dispose();
            ref5.Dispose();
            ref6.Dispose();
            ref7.Dispose();
            ref8.Dispose();
            ref9.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAcquire(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            out ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7
                , T8, Policy8, T9, Policy9> tuple)
        {
            bool success = true;
            ScopedReference<T1, Policy1> ref1 = default;
            ScopedReference<T2, Policy2> ref2 = default;
            ScopedReference<T3, Policy3> ref3 = default;
            ScopedReference<T4, Policy4> ref4 = default;
            ScopedReference<T5, Policy5> ref5 = default;
            ScopedReference<T6, Policy6> ref6 = default;
            ScopedReference<T7, Policy7> ref7 = default;
            ScopedReference<T8, Policy8> ref8 = default;
            ScopedReference<T9, Policy9> ref9 = default;

            if (!t1.TryBorrow(out ref1))
            {
                success = false;
                goto Finalize;
            }
            if (!t2.TryBorrow(out ref2))
            {
                success = false;
                goto Finalize;
            }
            if (!t3.TryBorrow(out ref3))
            {
                success = false;
                goto Finalize;
            }
            if (!t4.TryBorrow(out ref4))
            {
                success = false;
                goto Finalize;
            }
            if (!t5.TryBorrow(out ref5))
            {
                success = false;
                goto Finalize;
            }
            if (!t6.TryBorrow(out ref6))
            {
                success = false;
                goto Finalize;
            }
            if (!t7.TryBorrow(out ref7))
            {
                success = false;
                goto Finalize;
            }
            if (!t8.TryBorrow(out ref8))
            {
                success = false;
                goto Finalize;
            }
            if (!t9.TryBorrow(out ref9))
            {
                success = false;
            }
            
        Finalize:
            tuple = new ReferenceTuple<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7,
                    Policy7, T8, Policy8, T9, Policy9>(ref1, ref2, ref3, ref4, ref5, ref6, ref7, ref8, ref9);

            if (!success)
            {
                tuple.Dispose();
                return false;
            }
            else return true;
        }
    }
}