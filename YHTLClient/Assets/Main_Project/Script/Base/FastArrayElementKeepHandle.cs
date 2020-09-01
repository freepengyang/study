using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyBirds.Model
{
    //此类适合与T不需要NEW出来，来自与其他数据区的类
    public class FastArrayElementKeepHandle<T> where T : class
    {
        public delegate int ArrayCompare(T l, T r);
        public delegate bool MatchCompare(T v);

        protected T[] mBuffer;
        public int Count
        {
            get; private set;
        }

        public FastArrayElementKeepHandle()
        {
            Count = 0;
            mBuffer = new T[16];
        }

        public FastArrayElementKeepHandle(int capacity = 8)
        {
            if (capacity <= 0)
            {
                capacity = 8;
            }
            Count = 0;
            mBuffer = new T[capacity];
        }

        public T this[int i]
        {
            get { return mBuffer[i]; }
            set
            {
                mBuffer[i] = value;
            }
        }

        //O(1)
        public void Clear()
        {
            System.Array.Clear(mBuffer, 0, Count);
            Count = 0;
        }

        //O(1)
        public void Swap(int i, int j)
        {
            T temp = mBuffer[i];
            mBuffer[i] = mBuffer[j];
            mBuffer[j] = temp;
        }

        //O(1)
        public void SwapErase(int i)
        {
            if (i >= 0 && i < Count)
            {
                if (i == Count - 1)
                {
                    mBuffer[i] = default(T);
                    --Count;
                }
                else
                {
                    --Count;
                    mBuffer[i] = mBuffer[Count];
                    mBuffer[Count] = default(T);
                }
            }
        }

        public void RemoveAt(int i)
        {
            if (i >= 0 && i < Count)
            {
                for (int k = i + 1; k < Count; ++k)
                {
                    mBuffer[k - 1] = mBuffer[k];
                }

                mBuffer[--Count] = default(T);
            }
        }

        public void Append(T element)
        {
            if (mBuffer.Length == Count)
            {
                ExpandMemory(mBuffer.Length << 1);
            }
            mBuffer[Count] = element;
            ++Count;
        }

        public void AddRange(FastArrayElementKeepHandle<T> fastArray)
        {
            if (null != fastArray)
            {
                if (Count + fastArray.Count >= mBuffer.Length)
                {
                    ExpandMemory(Count + fastArray.Count);
                }

                for (int i = 0; i < fastArray.Count; ++i)
                {
                    mBuffer[Count] = fastArray[i];
                    ++Count;
                }
            }
        }

        public int FindIndex(MatchCompare matchCompare)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (matchCompare(mBuffer[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddRange(RepeatedField<T> fastArray)
        {
            if (null != fastArray)
            {
                if (Count + fastArray.Count >= mBuffer.Length)
                {
                    ExpandMemory(Count + fastArray.Count);
                }

                for (int i = 0; i < fastArray.Count; ++i)
                {
                    mBuffer[Count] = fastArray[i];
                    ++Count;
                }
            }
        }

        public void GetRange(int pos, int length, FastArrayElementKeepHandle<T> other)
        {
            if (null != other)
            {
                other.Clear();
                int end = Mathf.Min(pos + length, Count);
                for (int i = pos; i >= 0 && i < end; ++i)
                {
                    other.Append(mBuffer[i]);
                }
            }
        }

        protected void ExpandMemory(int capacity)
        {
            if (capacity > mBuffer.Length)
            {
                System.Array.Resize(ref mBuffer, capacity);
            }
        }
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="Key">待排序数组</param>
        /// <param name="left">数组最左端索引</param>
        /// <param name="right">数组最右端索引</param>
        private void Quick(T[] Key, int left, int right, ArrayCompare predicate)
        {
            T current = Key[left];
            int i = left;
            int j = right;

            if (left < right)
            {
                while (i < j) // 当i=j时，表示i之前的数均比current小，之后的数均比current大，即i是current在正确排序序列中的正确位置
                {
                    while (predicate(current, Key[j]) < 0 && i < j)
                    {
                        --j;
                    }

                    while (predicate(current, Key[i]) >= 0 && i < j)
                    {
                        ++i;
                    }

                    if (i < j)
                    {
                        T temp = Key[i];
                        Key[i] = Key[j];
                        Key[j] = temp;
                    }
                }
                Key[left] = Key[i];
                Key[i] = current; // 将current放置到正确的位置上
                if (left < j - 1)
                    Quick(Key, left, j - 1, predicate);
                if (j + 1 < right)
                    Quick(Key, j + 1, right, predicate);
            }
        }

        public void Sort(ArrayCompare predicate)
        {
            if (Count > 1)
                Quick(mBuffer, 0, Count - 1, predicate);
        }
    }
}