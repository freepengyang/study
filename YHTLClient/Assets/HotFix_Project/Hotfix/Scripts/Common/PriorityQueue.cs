﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 此代码摘自网络 并进行微调 - by 沈少军
/// https://www.cnblogs.com/skyivben/archive/2009/04/18/1438731.html
/// </summary>
namespace Skyiv.Util
{
    class PriorityQueue<T>
    {
        public delegate int Comparer(T l, T r);
        Comparer comparer;
        T[] heap;

        public int Count { get; private set; }
        public PriorityQueue(int capacity, Comparer comparer)
        {
            this.comparer = comparer;
            this.heap = new T[capacity];
        }

        public void Push(T v)
        {
            if (Count >= heap.Length) Array.Resize(ref heap, Count << 1);
            heap[Count] = v;
            SiftUp(Count++);
        }

        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0) SiftDown(0);
            return v;
        }

        public T Top()
        {
            if (Count > 0) return heap[0];
            throw new InvalidOperationException("优先队列为空");
        }

        void SiftUp(int n)
        {
            var v = heap[n];
            for (var n2 = n >> 1; n > 0 && comparer(v, heap[n2]) > 0; n = n2, n2 >>= 1) heap[n] = heap[n2];
            heap[n] = v;
        }

        void SiftDown(int n)
        {
            var v = heap[n];
            for (var n2 = n << 1; n2 < Count; n = n2, n2 <<= 1)
            {
                if (n2 + 1 < Count && comparer(heap[n2 + 1], heap[n2]) > 0) n2++;
                if (comparer(v, heap[n2]) >= 0) break;
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }
    }
}