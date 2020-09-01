using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ILRuntime.Runtime;
using System.Runtime.InteropServices;

public interface IPool
{
    object Get();

    void Recycle(object data);

    // 释放缓存池
    void Dispose();

    int GetCount();
}

public interface ISystemPool
{

}

public interface ICustomPool
{

}

public class CSPoolSystemClassData<T> : IPool, ISystemPool where T : class, new()
{
    public const int POOL_SIZE_LIMIT = 100;

    private Queue<T> protoStack;

    public CSPoolSystemClassData()
    {
        protoStack = new Queue<T>();
    }

    public object Get()
    {
        if (protoStack.Count > 0)
        {
            return protoStack.Dequeue();
        }

        return new T();
    }

    public void Recycle(object data)
    {
        if (data != null && protoStack.Count < POOL_SIZE_LIMIT)
        {
            protoStack.Enqueue(data as T);
        }
    }

    public void Dispose()
    {
        protoStack.Clear();
    }

    public int GetCount()
    {
        if (protoStack != null)
            return protoStack.Count;
        return 0;
    }
}

public class CSPoolCustomClassData<T> : IPool, ICustomPool where T : class, IDispose, new()
{
    public const int POOL_SIZE_LIMIT = 128;

    private Stack<T> protoStack;

    public CSPoolCustomClassData()
    {
        protoStack = new Stack<T>(POOL_SIZE_LIMIT);
    }

    public object Get()
    {
        if (protoStack.Count > 0)
        {
            return protoStack.Pop();
        }

        return new T();
    }

    public void Recycle(object data)
    {
        if(null != data)
        {
            if (data is T pooledData)
            {
                if (protoStack.Count < POOL_SIZE_LIMIT)
                {
                    pooledData.Dispose();
                    protoStack.Push(pooledData);
                }
                else
                {
                    FNDebug.LogError($"recycle pool is full size = {POOL_SIZE_LIMIT}");
                }
            }
            else
            {
                FNDebug.LogError($"recycle pool error type error = {data.GetType()} T is {typeof(T).FullName}");
            }
        }
    }

    public void Dispose()
    {
        protoStack.Clear();
    }

    public int GetCount()
    {
        if (protoStack != null)
            return protoStack.Count;
        return 0;
    }
}

public class CSPoolManager
{
    public static Dictionary<Type, IPool> poolMap = new Dictionary<Type, IPool>(512);

    /// <summary>
    /// 重置缓存池
    /// 此处仅在返回登录时和场景切换调用，其他地方请勿调用
    /// </summary>
    public static void Dispose()
    {
        if (poolMap.Count <= 0) return;
        for (var it = poolMap.GetEnumerator(); it.MoveNext();)
        {
            it.Current.Value.Dispose();
        }
    }
}

public class PoolHandleManager
{
    private List<object> mPoolPairs;
    public Dictionary<Type, IPool> mDispatcher;
    int mPoolTag;
#if MEMORY_TRACE
    int mGetCount;
    int mPutCount;
    UIBase mBase;
    public Dictionary<Type, int> mPooledDicCount = new Dictionary<Type, int>(32);
    public void TryReportPoolBalance()
    {
        if(null != mBase)
        {
            var it = mPooledDicCount.GetEnumerator();
            while(it.MoveNext())
            {
                if(it.Current.Value != 0)
                    FNDebug.Log($"<color=#384fdd>pool item {it.Current.Key.FullName} lose balance count:{it.Current.Value} panel:{mBase.GetType().FullName}</color>");
            }
        }
    }
#endif

    public PoolHandleManager(int capacity = 32, UIBase uIBase = null)
    {
        mPoolPairs = new List<object>(capacity);
        mDispatcher = CSPoolManager.poolMap;
#if MEMORY_TRACE
        mBase = uIBase;
#endif
    }

    /// <summary>
    /// 从缓存池中获取系统提供的类，， 自定义类请勿调用此方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSystemClass<T>() where T : class, new()
    {
        if (mDispatcher == null) return new T();

        if(!mDispatcher.TryGetValue(typeof(T),out IPool datasPool))
        {
            datasPool = new CSPoolSystemClassData<T>();
            mDispatcher.Add(typeof(T), datasPool);
        }

        T dataobj = datasPool.Get() as T;

#if MEMORY_TRACE
        ++mGetCount;
        FNDebug.LogWarning($"Get:[{mGetCount}]:[{typeof(T).FullName}]");
        if (!mPooledDicCount.ContainsKey(typeof(T)))
            mPooledDicCount.Add(typeof(T), 1);
        else
            ++mPooledDicCount[typeof(T)];
#endif
        //mPoolPairs.Add(dataobj);
        return dataobj;
    }

    /// <summary>
    /// 从缓存池中获取自定义类，所有自定义类必须继承 IDispose， 在Dispose方法中，将类中所用数据清空
    /// 注意：Dispose 方法调用时，不要将方法外部new的对象置空，会导致再次调用时报错
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCustomClass<T>() where T : class, IDispose, new()
    {
        if (mDispatcher == null) return new T();
        if (!mDispatcher.TryGetValue(typeof(T), out IPool datasPool))
        {
            datasPool = new CSPoolCustomClassData<T>();
            mDispatcher.Add(typeof(T), datasPool);
        }

        T dataobj = datasPool.Get() as T;

        mPoolPairs.Add(dataobj);

#if MEMORY_TRACE
        ++mGetCount;
        if (!mPooledDicCount.ContainsKey(typeof(T)))
            mPooledDicCount.Add(typeof(T), 1);
        else
            ++mPooledDicCount[typeof(T)];
        //FNDebug.LogWarning($"Get:[{mGetCount}]:[{typeof(T).FullName}]");
#endif
        return dataobj;
    }

    /// <summary>
    /// 回收所有对象
    /// </summary>
    public void RecycleAll()
    {
        object message;
        Type type;
        if (mDispatcher != null)
        {
            for (int i = 0, max = mPoolPairs.Count; i < max; i++)
            {
                message = mPoolPairs[i];
                type = message.GetType();
                if(mDispatcher.TryGetValue(type,out IPool datasPool))
                {
                    datasPool.Recycle(message);
#if MEMORY_TRACE
                    ++mPutCount;
                    --mPooledDicCount[type];
                    //FNDebug.LogWarning($"Recycle:[{mPutCount}]:[{type.FullName}]");
#endif
                }
            }
        }

        mPoolPairs.Clear();
    }

    /// <summary>
    /// 回收单个对象
    /// 注：调用时，请将获取的对象传入，，如果获取的对象复制给另一对象，传入另一对象时无效
    /// </summary>
    /// <param name="message"></param>
    public void Recycle(object message)
    {
        if (message == null)
        {
            FNDebug.LogError($"try to recycle null object");
            return;
        }

        var type = message.GetType();
        if(mDispatcher == null || !mDispatcher.TryGetValue(type,out IPool datasPool) || null == datasPool)
        {
            FNDebug.LogError($"recycle item error pool not existed for {type.FullName}");
            return;
        }


#if MEMORY_TRACE
        ++mPutCount;
        --mPooledDicCount[type];
        //FNDebug.LogWarning($"Recycle:[{mPutCount}]:[{type.FullName}]");
#endif
        
        if(datasPool is ICustomPool)
        {
            for (int i = 0, max = mPoolPairs.Count; i < max; i++)
            {
                if (mPoolPairs[i] == message)
                {
                    datasPool.Recycle(mPoolPairs[i]);
                    mPoolPairs.RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            datasPool.Recycle(message);
        }
    }

    /// <summary>
    /// 不回收，，直接置空，，，
    /// </summary>
    public void OnDestroy()
    {
        if (mPoolPairs != null)
        {
            for (int i = 0; i < mPoolPairs.Count; i++)
            {
                mPoolPairs[i] = null;
            }

            mPoolPairs.Clear();
        }

        mDispatcher = null;
    }
}