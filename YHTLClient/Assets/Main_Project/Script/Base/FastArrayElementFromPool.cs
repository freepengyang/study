using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyBirds.Model
{
    //此类适合与T不需要NEW出来，来自与其他数据区的类
    public class FastArrayElementFromPool<T> where T : class
    {
        public delegate int ArrayCompare(T l, T r);
        protected T[] mBuffer;
        int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value < 0)
                    value = 0;

                if (mBuffer.Length < value)
                {
                    ExpandMemory(Mathf.Max(mBuffer.Length << 1, value));
                }

                for (int i = _count; i < value; ++i)
                {
                    mBuffer[i] = onCreate?.Invoke();
                }

                for (int i = value; i < _count; ++i)
                {
                    onRecycle?.Invoke(mBuffer[i]);
                }

                _count = value;
            }
        }

        public T Append()
        {
            this.Count = _count + 1;
            return this.mBuffer[_count - 1];
        }

        public delegate T OnCreate();
        public OnCreate onCreate;

        public delegate void OnRecycle(T element);
        public OnRecycle onRecycle;

        public delegate void OnDestroy();
        public OnDestroy onDestroy;

        public FastArrayElementFromPool(int capacity = 8, OnCreate onCreate = null, OnRecycle onRecycle = null, OnDestroy onDestroy = null)
        {
            if (capacity <= 0)
            {
                capacity = 8;
            }
            _count = 0;
            mBuffer = new T[capacity];
            this.onCreate = onCreate;
            this.onRecycle = onRecycle;
            this.onDestroy = onDestroy;
        }

        public T this[int i]
        {
            get { return mBuffer[i]; }
            set
            {
                mBuffer[i] = value;
            }
        }

        public void RemoveAt(int i)
        {
            if (i >= 0 && i < _count)
            {
                onRecycle?.Invoke(mBuffer[i]);

                for (int k = i + 1; k < _count; ++k)
                {
                    mBuffer[k - 1] = mBuffer[k];
                }

                System.Array.Clear(mBuffer, --_count, 1);
            }
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
            if (i >= 0 && i < _count)
            {
                onRecycle?.Invoke(mBuffer[i]);
                mBuffer[i] = mBuffer[--_count];
                System.Array.Clear(mBuffer, _count, 1);
            }
        }

        //O(N)
        public void Clear()
        {
            for (int i = 0; i < _count; ++i)
            {
                onRecycle?.Invoke(mBuffer[i]);
            }
            System.Array.Clear(mBuffer, 0, _count);
            _count = 0;
        }

        public void Destroy()
        {
            Clear();
            var ondestroy = this.onDestroy;
            this.onDestroy = null;
            ondestroy?.Invoke();
        }

        protected void ExpandMemory(int capacity)
        {
            if (capacity > mBuffer.Length)
            {
                T[] templates = new T[capacity];
                for (int i = 0; i < _count; ++i)
                {
                    templates[i] = mBuffer[i];
                }
                System.Array.Clear(mBuffer, 0, mBuffer.Length);
                mBuffer = templates;
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
            if (_count > 1)
                Quick(mBuffer, 0, _count - 1, predicate);
        }
    }

    /// <summary>
    /// 此类用于数据类的专用池子
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastArrayPoolLocal<T> where T : class, new()
    {
        Stack<T> mPool;
        public FastArrayPoolLocal(int capacity = 8)
        {
            mPool = new Stack<T>(capacity);
        }

        public T Get()
        {
            if (mPool.Count > 0)
                return mPool.Pop();
            return new T();
        }

        public void Put(T element)
        {
            if(null != element)
                mPool.Push(element);
        }

        public void Destroy()
        {
            mPool.Clear();
            mPool = null;
        }
    }
}