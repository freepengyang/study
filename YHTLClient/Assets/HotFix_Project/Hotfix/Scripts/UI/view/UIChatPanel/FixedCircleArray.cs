using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于界面类内存只增不减
public class FixedCircleArray<T> where T : class,new()
{
    protected T[] mBuffer;
    public int Count { get; private set; }
    public int Head { get; private set; }
    public int Tail { get; private set; }
    public int Capacity { get; private set; }
    public bool IsFull { get { return Count == Capacity; } }

    public FixedCircleArray(int c)
    {
        Capacity = Mathf.Max(c, 2);
        Head = 0;
        Tail = 0;
        Count = 0;
        mBuffer = new T[Capacity];
    }

    public T this[int i]
    {
        get 
        {
            i += Head;
            if(i >= Capacity)
            {
                i -= Capacity;
            }
            return mBuffer[i]; 
        }
        private set
        {
            i += Head;
            if (i >= Capacity)
            {
                i -= Capacity;
            }
            mBuffer[i] = value;
        }
    }

    public void SetElementAt(int i,T element)
    {
        this[i] = element;
    }

    public void RemoveElementFromTail(int count)
    {
        if(count > Count)
        {
            count = Count;
        }
        if(count > 0)
        {
            for(int i = Count - count;i < Count; ++i)
            {
                --Tail;
                if (Tail < 0)
                {
                    Tail = Count - 1;
                }
                System.Array.Clear(mBuffer, Tail, 1);
                //mBuffer[Tail] = default(T);
            }
            Count -= count;
        }
    }

    //O(1)
    public T Append(T element = null)
    {
        T handle = null;
        
        if(null == element)
        {
            handle = mBuffer[Tail];
            if (null == handle)
            {
                handle = new T();
                mBuffer[Tail] = handle;
            }
        }
        else
        {
            handle = element;
            mBuffer[Tail] = handle;
        }

        if(++Tail == Capacity)
        {
            Tail = 0;
        }

        if(Count == Capacity)
        {
            Head = Tail;
        }
        else
        {
            ++Count;
        }

        return handle;
    }

    //O(1)
    public void Clear()
    {
        Count = 0;
        Head = 0;
        Tail = 0;
    }

    public void Dispose()
    {
        System.Array.Clear(mBuffer, 0, mBuffer.Length);
        mBuffer = null;
        Head = 0;
        Tail = 0;
        Count = 0;
    }
}