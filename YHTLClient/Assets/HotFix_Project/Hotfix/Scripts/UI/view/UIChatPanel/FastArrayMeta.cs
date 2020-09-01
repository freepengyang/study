using System.Collections;
using System.Collections.Generic;

//用于元类型数据数组
public class FastArrayMeta<T>
{
    protected T[] mBuffer;
    public int Count { get; private set; }

    public FastArrayMeta(int capacity = 8)
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
    public void Add(T v)
    {
        if(Count == mBuffer.Length)
        {
            ExpandMemory(mBuffer.Length << 1);
        }

        mBuffer[Count++] = v;
    }

    public bool Contains(T v)
    {
        for (int i = 0; i < Count; ++i)
        {
            if (mBuffer[i].Equals(v))
                return true;
        }
        return false;
    }

    public void AddRange(FastArrayMeta<T> fastArrayMeta)
    {
        if(null != fastArrayMeta)
        {
            if (Count + fastArrayMeta.Count >= mBuffer.Length)
            {
                ExpandMemory(Count + fastArrayMeta.Count);
            }

            for (int i = 0; i < fastArrayMeta.Count; ++i)
            {
                mBuffer[Count] = fastArrayMeta[i];
                ++Count;
            }
        }
    }

    public void AddRange(T[] argvs)
    {
        if (Count + argvs.Length >= mBuffer.Length)
        {
            ExpandMemory(Count + argvs.Length);
        }

        for (int i = 0; i < argvs.Length; ++i)
        {
            mBuffer[Count] = argvs[i];
            ++Count;
        }
    }

    //O(1)
    public void Clear()
    {
        Count = 0;
    }

    public void Dispose()
    {
        Count = 0;
        mBuffer = null;
    }

    protected void ExpandMemory(int capacity)
    {
        if(capacity < (mBuffer.Length << 1))
        {
            capacity = (mBuffer.Length << 1);
        }

        T[] templates = new T[capacity];
        for (int i = 0; i < Count; ++i)
        {
            templates[i] = mBuffer[i];
            //mBuffer[i] = default(T);
        }
        System.Array.Clear(mBuffer, 0, mBuffer.Length);
        mBuffer = templates;
    }
}