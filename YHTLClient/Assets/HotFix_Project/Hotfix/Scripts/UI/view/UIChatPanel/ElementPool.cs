using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 元素对象池
/// </summary>
/// <typeparam name="T"></typeparam>
public class ElementPool<T> where T : class, new()
{
    Stack<T> mPool;
    System.Action<T> onCreate;
    System.Action<T> onGet;
    System.Action<T> onPut;
    System.Action<T> onDestroy;

    public ElementPool(int capacity = 8,
        System.Action<T> onCreate = null,
        System.Action<T> onGet = null,
        System.Action<T> onPut = null,
        System.Action<T> onDestroy = null)
    {
        mPool = new Stack<T>(capacity);
        this.onCreate = onCreate;
        this.onGet = onGet;
        this.onPut = onPut;
        this.onDestroy = onDestroy;
    }

    public T Get()
    {
        T handle = null;
        if (mPool.Count > 0)
        {
            handle = mPool.Pop();
            if (null != onGet)
                onGet(handle);
        }
        else
        {
            handle = new T();
            if (null != onCreate)
                onCreate(handle);
            if (null != onGet)
                onGet(handle);
        }
        return handle;
    }

    public void Put(T element)
    {
        if (null != onPut)
            onPut(element);
        mPool.Push(element);
    }

    public void Destroy()
    {
        if (null == mPool)
            return;

        if (null != onDestroy)
        {
            while (mPool.Count > 0)
                onDestroy(mPool.Pop());
        }
        mPool.Clear();
        mPool = null;
        this.onCreate = null;
        this.onGet = null;
        this.onPut = null;
        this.onDestroy = null;
    }
}