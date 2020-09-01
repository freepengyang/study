using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if(mBuffer.Length < value)
            {
                ExpandMemory(Mathf.Max(mBuffer.Length << 1,value));
            }

            for(int i = _count; i < value; ++i)
            {
                mBuffer[i] = onCreate?.Invoke();
            }

            for(int i = value; i < _count; ++i)
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
        if(capacity <= 0)
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

            for(int k = i + 1;k < _count;++k)
            {
                mBuffer[k - 1] = mBuffer[k];
            }

            System.Array.Clear(mBuffer, --_count, 1);
        }
    }

    //O(1)
    public void Swap(int i,int j)
    {
        T temp = mBuffer[i];
        mBuffer[i] = mBuffer[j];
        mBuffer[j] = temp;
    }

    //O(1)
    public void SwapErase(int i)
    {
        if(i >= 0 && i < _count)
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
        System.Array.Clear(mBuffer,0,_count);
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
        if(capacity > mBuffer.Length)
        {
            T[] templates = new T[capacity];
            for(int i = 0; i < _count; ++i)
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
        mPool.Push(element);
    }

    public void Destroy()
    {
        mPool.Clear();
        mPool = null;
    }
}

public class FastArrayPool<T> where T : class,new()
{
    PoolHandleManager mPool;
    public FastArrayPool(PoolHandleManager pool)
    {
        mPool = pool;
    }

    public T Get()
    {
        return mPool.GetSystemClass<T>();
    }

    public void Put(T element)
    {
        mPool.Recycle(element);
    }
}

public class ItemPool
{
    PoolHandleManager mPool;
    Transform mParent;
    PropItemType mPropItemType;
    itemSize mSize;
    public ItemPool(PoolHandleManager pool,PropItemType propItemType, Transform parent, itemSize size = itemSize.Default)
    {
        mPool = pool;
        mPropItemType = propItemType;
        mParent = parent;
        mSize = size;
    }

    public UIItemBase Get()
    {
        return UIItemManager.Instance.GetItem(mPropItemType, mParent,mSize);
    }

    public void Put(UIItemBase element)
    {
        UIItemManager.Instance.RecycleSingleItem(element);
    }
}

public class ClonedObjectPool
{
    Stack<Transform> mPooledObjects;
    Transform mTemplate;
    Transform mParent;
    bool mNeedDestroyTemplate;
    System.Action<Transform> onDestroy;
    public ClonedObjectPool(Transform template, Transform parent,int capacity = 8, System.Action<Transform> onDestroy = null,bool needDestroyTemplate = false)
    {
        this.onDestroy = onDestroy;
        mNeedDestroyTemplate = needDestroyTemplate;
        mTemplate = template;
        mTemplate.CustomActive(false);
        mParent = parent;
        mPooledObjects = new Stack<Transform>(capacity);
    }
    public Transform Get()
    {
        if (mPooledObjects.Count > 0)
            return mPooledObjects.Pop();
        var handle = Object.Instantiate(mTemplate, mParent) as Transform;
        handle.transform.localPosition = Vector3.zero;
        handle.transform.localRotation = Quaternion.identity;
        handle.transform.localScale = Vector3.one;
        handle.CustomActive(true);
        return handle;
    }
    public void Put(Transform transform)
    {
        if(null != transform)
        {
            transform.CustomActive(false);
            mPooledObjects.Push(transform);
        }
    }
    public void OnDestroy()
    {
        mParent = null;
        if(null != onDestroy && mNeedDestroyTemplate)
        {
            onDestroy(mTemplate);
        }
        mTemplate = null;
        if(null != onDestroy)
        {
            while(mPooledObjects.Count > 0)
            {
                onDestroy(mPooledObjects.Pop());
            }
        }
        mPooledObjects?.Clear();
        mPooledObjects = null;
    }
}

public static class FastArrayElementFromPoolExtend
{
    public static FastArrayElementFromPool<T> CreateGeneratePool<T>(this PoolHandleManager poolHandle, int capacity = 8) where T : class,new()
    {
        //此对象由回调持有
        var pool = new FastArrayPool<T>(poolHandle);
        return new FastArrayElementFromPool<T>(capacity, pool.Get, pool.Put);
    }

    public static FastArrayElementFromPool<UIItemBase> CreateItemPool(this PoolHandleManager poolHandle, PropItemType propItemType, Transform parent,int capacity = 8, itemSize size = itemSize.Default)
    {
        var pool = new ItemPool(poolHandle, propItemType, parent, size);
        return new FastArrayElementFromPool<UIItemBase>(capacity,pool.Get,pool.Put);
    }

    public static FastArrayElementFromPool<Transform> CreateClonePool(this Transform template,Transform parent = null,int capacity = 8, System.Action<Transform> onDestroy = null,bool needDestroyTemplate = false)
    {
        if (null == parent)
            parent = template.parent;
        var pool = new ClonedObjectPool(template, parent, capacity, onDestroy,needDestroyTemplate);
        return new FastArrayElementFromPool<Transform>(capacity, pool.Get, pool.Put,pool.OnDestroy);
    }
}