using UnityEngine;
using System.Collections;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

public interface IProto
{
    IMessage Get();

    void Recycle(IMessage data);

    // 释放缓存池
    void Dispose();
}

public class CSProtoData<T> : IProto where T : class, IMessage
{
    public const int POOL_SIZE_LIMIT = 100;

    private Queue<T> protoStack;
    object mutex;
    public CSProtoData()
    {
        mutex = new object();
        protoStack = new Queue<T>();
    }

    public IMessage Get()
    {
        lock (mutex)
        {
            if (protoStack.Count > 0)
            {
                return protoStack.Dequeue();
            }
            return Activator.CreateInstance<T>();
        }
    }

    public void Recycle(IMessage data)
    {
        lock (mutex)
        {
            if (data != null && protoStack.Count < POOL_SIZE_LIMIT)
            {
                protoStack.Enqueue(data as T);
            }
        }
    }

    public void Dispose()
    {
        lock (mutex)
        {
            protoStack.Clear();
        }
    }
}


public class CSProtoManager
{
    static Map<Type, IProto> poolMap = new Map<Type, IProto>();

    public static T Get<T>() where T:  class, IMessage
    {
        if (poolMap.ContainsKey(typeof(T)))
        {
            //Debug.LogError("-------------------");
            IProto data = poolMap[typeof(T)];
            return data.Get() as T;

        }else
        {
            poolMap[typeof(T)] = new CSProtoData<T>();
            return Activator.CreateInstance<T>();
        }
    }

    public static void Recycle(IMessage message)
    {
        if (message == null) return;
        IProto data = poolMap[message.GetType()];
        if (data == null) return;
        data.Recycle(message);
    }

    public static void Dispose()
    {
        if (poolMap.Count <= 0) return;
        for (poolMap.Begin();poolMap.Next();)
        {
            poolMap.Value.Dispose();
        }
    }
}

public class CSNetRepeatedFieldPool
{
    static Dictionary<Type,Stack<object>> poolMap = new Dictionary<Type,Stack<object>>();

    public static RepeatedField<T> Get<T>()
    {
        if (poolMap.ContainsKey(typeof(RepeatedField<T>)))
        {
            var data = poolMap[typeof(RepeatedField<T>)];
            return (RepeatedField<T>)data.Pop();
        }
        else
        {
            return new RepeatedField<T>();
            //return (RepeatedField<T>)typeof(HotMain).Assembly.CreateInstance(typeof(RepeatedField<T>).FullName);
        }
    }

    public static void Put<T>(RepeatedField<T> list)
    {
        var type = typeof(RepeatedField<T>);
        if(!poolMap.ContainsKey(typeof(RepeatedField<T>)))
        {
            poolMap.Add(type, new Stack<object>(8));
        }
        var pool = poolMap[type];
        pool.Push(list);
    }

    public static void Dispose()
    {
        poolMap.Clear();
    }
}