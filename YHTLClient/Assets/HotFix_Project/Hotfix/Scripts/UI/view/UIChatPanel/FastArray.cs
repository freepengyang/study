using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IndexedItem
{
    int Index { get; set; }
}

//用于界面类内存只增不减
public class FastArray<T> where T : class, IndexedItem,new()
{
    protected T[] mBuffer;
    public int Count { get; private set; }

    public FastArray(int capacity = 8)
    {
        if(capacity <= 0)
        {
            capacity = 8;
        }
        Count = 0;
        mBuffer = new T[capacity];
    }

    public T this[int i]
    {
        get { return mBuffer[i]; }
    }

    //O(1)
    public T PushNewElementToTail()
    {
        if(Count == mBuffer.Length)
        {
            ExpandMemory(mBuffer.Length << 1);
        }

        T handle = mBuffer[Count];
        if(null == handle)
        {
            handle = new T();
            handle.Index = Count;
            mBuffer[Count] = handle;
        }
        ++Count;

        return handle;
    }

    //O(N)
    public void Remove(T handle)
    {
        if(null != handle)
        {
            T temp = handle;
            int cnt = mBuffer.Length - 1;
            for (int i = handle.Index; i < cnt; ++i)
            {
                mBuffer[i] = mBuffer[i + 1];
                mBuffer[i].Index = i;
            }
            Count = cnt;
            temp.Index = Count;
        }
    }

    //O(1)
    public void Clear()
    {
        Count = 0;
    }

    public void Dispose()
    {
        System.Array.Clear(mBuffer, 0, mBuffer.Length);
        mBuffer = null;
        Count = 0;
    }

    protected void ExpandMemory(int capacity)
    {
        if(capacity > mBuffer.Length)
        {
            T[] templates = new T[capacity];
            for(int i = 0; i < Count; ++i)
            {
                templates[i] = mBuffer[i];
            }
            System.Array.Clear(mBuffer, 0, mBuffer.Length);
            mBuffer = templates;
        }
    }
}