// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Collections.HashSet
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    static class HashSet
    {
        /// <summary>
        /// The default amount of elements stored before the container resizes in order to ensure
        /// an optimal hash calculation
        /// </summary>
        public const float DefaultLoadFactor = 0.86f;
    }
    
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    struct HashSet<T, Container>
        where Container : struct, IHashContainer<T>
    {
        private readonly IEqualityComparer<T> comparer;
        private readonly float loadFactor;
        private int moduloMask;
        
        private Container[]? items;
        /// <summary>
        /// Gets the items managed by this container
        /// </summary>
        public Container[]? Items
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return items; }
        }
        
        /// <summary>
        /// Gets the maximum number of elements that can be stored
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return items?.Length ?? 0; }
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
        
        private int version;
        /// <summary>
        /// Gets the current container version
        /// </summary>
        /// <remarks>Version is increased whenever the container layout changes</remarks>
        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashSet(IEqualityComparer<T> comparer, float loadFactor = HashSet.DefaultLoadFactor)
        {
            this.comparer = comparer;
            this.loadFactor = loadFactor;
            this.moduloMask = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (Capacity > 0)
            {
                Array.Clear(items!);
            }
            count = 0;
        }
        
        public ref Container Emplace(in T key, int hash, int index, int distance, int currentVersion)
        {
            if (version == currentVersion || !Find(key, hash, out index, out distance, out Ref<Container> result))
            {
                if (items == null || count >= items.Length * loadFactor)
                {
                    Grow();
                    index = (hash & moduloMask);
                }

                version++;
                return ref items![Emplace(index, distance)];
            }
            else return ref result.Value;
        }
        int Emplace(int index, int distance)
        {
            if (!items![index].IsValid)
            {
                // Insert as slot is already empty
                
                count++;
                return index;
            }
            else
            {
                Container element = default;
                {
                    int i = distance;
                    int tmp = GetDistance(index);
                    int result; 
                    
                    // Check if distance is smaller than the current distance
                    if(distance > tmp)
                    {
                        // Swap elements so the new element is closer to index
                        Swap(ref items[index], ref element);
                        result = index;
                        distance = tmp;
                    }
                    else result = -1;
                    
                    index = (index + 1) & moduloMask;
                    distance++;

                    // Move the current element to the next free slot
                    for (; i < items.Length; i++, index = (index + 1) & moduloMask)
                    {
                        // Free slot found to put the element into
                        if (!items[index].IsValid)
                        {
                            count++;
                            if (element.IsValid)
                            {
                                Swap(ref items[index], ref element);
                                
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
                                if (!element.IsValid)
                                {
                                    result = index;
                                }
                                Swap(ref items[index], ref element);
                                distance = tmp;
                            }
                            distance++;
                        }
                    }
                }
                throw new OverflowException();
            }
        }

        public bool Find(in T key, int hash, out int index, out int distance, out Ref<Container> result)
        {
            if (count > 0)
            {
                index = (hash & moduloMask);
                distance = 0;

                for (int length = items?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (items![index].IsValid)
                    {
                        if (hash == items[index].Hash && comparer.Equals(key, items[index].Key))
                        {
                            result = new Ref<Container>(ref items[index]);
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
            
            result = Ref<Container>.CreateEmpty(); 
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetDistance(int index)
        {
            int tmp = (items![index].Hash & moduloMask);
            if (tmp > index)
            {
                return (index + (items.Length - tmp));
            }
            else return (index - tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Grow()
        {
            Reserve(items == null ? 4 : items.Length + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(ref Container lhs, ref Container rhs)
        {
            (lhs, rhs) = (rhs, lhs);
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

            Container[]? tmp = items;
            items = new Container[capacity];
            if (tmp != null)
            {
                // Reinsert existing elements
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i].IsValid)
                    {
                        int index = Emplace(tmp[i].Hash & moduloMask, 0);
                        items[index] = tmp[i];
                    }
                }
            }
            version++;
        }
    }
}