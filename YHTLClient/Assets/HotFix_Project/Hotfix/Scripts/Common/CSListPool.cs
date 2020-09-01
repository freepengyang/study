using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class CSListPool
{
    static Dictionary<System.Type, List<object>> mPooledItems = new Dictionary<System.Type, List<object>>();
    public static List<T> Get<T>()
    {
        List<T> ret = null;
        var key = typeof(T);
        if (mPooledItems.ContainsKey(key))
        {
            if(mPooledItems[key].Count > 0)
            {
                ret = mPooledItems[key][0] as List<T>;
                mPooledItems[key].RemoveAt(0);
                return ret;
            }
        }
        ret = new List<T>();
        return ret;
    }

    public static void Put<T>(List<T> handle, System.Action onRecycle = null)
    {
        if(null != handle)
        {
            var key = typeof(T);
            List<object> pooledList;
            if (!mPooledItems.ContainsKey(key))
            {
                pooledList = new List<object>();
                mPooledItems.Add(key, pooledList);
            }
            else
            {
                pooledList = mPooledItems[key];
            }
            handle.Clear();
            onRecycle?.Invoke();
            pooledList.Add(handle);
        }
    }
}