// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// The maximum amount of elements stored before the container resizes in order to ensure
        /// an optimal hash calculation
        /// </summary>
        public const float LoadFactor = 0.86f;
        private int moduloMask;
        
        readonly IEqualityComparer<TKey> comparer;
        
        private HashEntry[]? data;
        /// <summary>
        /// Gets the maximum number of elements that can be stored
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return data?.Length ?? 0; }
        }
        
        private int count;
        /// <summary>
        /// Gets the current number of elements stored
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public ref TValue this[in TKey key]
        {
            get
            {
                int hash = key!.GetHashCode();
                if (Find(key, hash, out _, out _, out Ref<TValue> result))
                {
                    return ref result.Value;
                }
                else throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc/>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this[key]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { this[key] = value; }
        }
        
        /// <inheritdoc/>
        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this[key]; }
        }
        
        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }
        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }
        /// <inheritdoc/>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Keys; }
        }
        /// <inheritdoc/>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Values; }
        }
                
        /// <summary>
        /// Initializes this container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <param name="comparer">An object instance that can compare <typeparamref name="TValue"/> instances</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(int capacity, EqualityComparer<TKey> comparer)
            : this(comparer)
        {
            Reserve(capacity);
        }
        /// <summary>
        /// Initializes this container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(int capacity)
         : this(capacity, EqualityComparer<TKey>.Default)
        { }
        /// <summary>
        /// Initializes an empty instance of this container
        /// </summary>
        /// <param name="comparer">An object instance that can compare <typeparamref name="TValue"/> instances</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(EqualityComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }
        /// <summary>
        /// Initializes an empty instance of this container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary()
         : this(EqualityComparer<TKey>.Default)
        { }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            int hash = key!.GetHashCode();
            if (!Find(key, hash, out _, out _, out _))
            {
                Emplace(key, hash) = value;
            }
            else throw new ArgumentException();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (Capacity > 0)
            {
                Array.Clear(data!);
            }
            count = 0;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int hash = item.Key!.GetHashCode();
            if (Find(item.Key, hash, out _, out _, out Ref<TValue> result))
            {
                return EqualityComparer<TValue>.Default.Equals(item.Value, result.Value);
            }
            else return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key)
        {
            int hash = key!.GetHashCode();
            if (Find(key, hash, out _, out _, out _))
            {
                return true;
            }
            else return false;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex >= count)
            {
                for (int i = 0, length = data!.Length; i < length; i++)
                {
                    if (data![i].IsValid)
                        array[arrayIndex++] = new KeyValuePair<TKey, TValue>(data![i].Key, data[i].Value);
                }
            }
            else throw new ArgumentException();
        }

        ref TValue Emplace(in TKey key, int hash)
        {
            if (!Find(key, hash, out int index, out int distance, out Ref<TValue> result))
            {
                if (data == null || count >= data.Length * LoadFactor)
                {
                    Grow();
                    index = (hash & moduloMask);
                }

                ref HashEntry element = ref data![Emplace(index, distance)];
                element = new HashEntry(key, hash);
                return ref element.Value;
            }
            else return ref result.Value;
        }
        int Emplace(int index, int distance)
        {
            if (data![index].IsEmpty)
            {
                // Insert as slot is already empty
                
                count++;
                return index;
            }
            else
            {
                HashEntry element = default;
                {
                    int i = distance;
                    int tmp = GetDistance(index);
                    int result; 
                    
                    // Check if distance is smaller than the current distance
                    if(distance > tmp)
                    {
                        // Swap elements so the new element is closer to index
                        Swap(ref data[index], ref element);
                        result = index;
                        distance = tmp;
                    }
                    else result = -1;
                    
                    index = (index + 1) & moduloMask;
                    distance++;

                    // Move the current element to the next free slot
                    for (; i < data.Length; i++, index = (index + 1) & moduloMask)
                    {
                        // Free slot found to put the element into
                        if (data[index].IsEmpty)
                        {
                            count++;
                            if (!element.IsEmpty)
                            {
                                Swap(ref data[index], ref element);
                                
                                #if DEBUG
                                if(result < 0)
                                {
                                    throw new IndexOutOfRangeException();
                                }
                                else
                                #endif
                                return result;
                            }
                            else return index;
                        }
                        else
                        {
                            // Check if distance is smaller than the current distance and swap
                            tmp = GetDistance(index);
                            if (distance > tmp)
                            {
                                if (element.IsEmpty)
                                {
                                    result = index;
                                }
                                Swap(ref data[index], ref element);
                                distance = tmp;
                            }
                            distance++;
                        }
                    }
                }
                throw new OverflowException();
            }
        }

        bool Find(in TKey key, int hash, out int index, out int distance, out Ref<TValue> result)
        {
            if (count > 0)
            {
                index = (hash & moduloMask);
                distance = 0;

                for (int length = data?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (data![index].IsValid)
                    {
                        if (hash == data[index].Hash && comparer.Equals(key, data[index].Key))
                        {
                            result = new Ref<TValue>(ref data[index].Value);
                            return true;
                        }
                        else if (distance > GetDistance(index))
                            break;
                    }
                    else break;
                }
            }

            index = 0;
            distance = 0;
            
            result = Ref<TValue>.CreateEmpty(); 
            return false;
        }

        int GetDistance(int index)
        {
            int tmp = (data![index].Hash & moduloMask);
            if (tmp > index)
            {
                return (index + (data.Length - tmp));
            }
            else return (index - tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Grow()
        {
            Reserve(data == null ? 4 : data.Length + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(ref HashEntry lhs, ref HashEntry rhs)
        {
            (lhs, rhs) = (rhs, lhs);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            if (count > 0)
            {
                for (int hash = key!.GetHashCode(), index = (hash & moduloMask), distance = 0, length = data?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (data![index].IsValid)
                    {
                        if (hash == data[index].Hash && comparer.Equals(key, data[index].Key))
                        {
                            data[index] = default;
                            return true;
                        }
                        else if (distance > GetDistance(index))
                            break;
                    }
                    else break;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Resizes the container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        public void Reserve(int capacity)
        {
            capacity = capacity.NextPowerOfTwo();
            moduloMask = (capacity - 1);
            count = 0;

            HashEntry[]? tmp = data;
            data = new HashEntry[capacity];
            if (tmp != null)
            {
                // Reinsert existing elements
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i].IsValid)
                    {
                        int index = Emplace(tmp[i].Hash & moduloMask, 0);
                        data[index] = tmp[i];
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            int hash = key!.GetHashCode();
            if (Find(key, hash, out _, out _, out Ref<TValue> result))
            {
                value = result.Value;
                return true;
            }
            else
            {
                value = default!;
                return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(data, count);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}