using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class UtilityLinq
{
    #region Where
    
    /// <summary>
    /// 从集合中筛选出满足条件的对象，并存入指定的list中，不可传入空列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <param name="resluts">用来存结果的list</param>
    /// <param name="needClear">存储前是否清空list</param>
    public static void WhereToList<T>(this IEnumerable<T> list, Func<T, bool> conditionFunc, CSBetterLisHot<T> results, bool needClear = true)
    {
        if (results == null || list == null) return;
        var enumerator = list.GetEnumerator();
        if (needClear) results.Clear();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (conditionFunc.Invoke(item))
            {
                results.Add(item);
            }
        }
    }

    /// <summary>
    /// 从集合中筛选出满足条件的对象，并存入指定的list中，不可传入空列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <param name="resluts">用来存结果的list</param>
    /// <param name="needClear">存储前是否清空list</param>
    public static void WhereToList<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc, CSBetterLisHot<T> results, bool needClear = true)
    {
        if (results == null || list == null) return;
        if (needClear) results.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                results.Add(list[i]);
            }
        }
    }

    public static void WhereToList<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc, List<T> results, bool needClear = true)
    {
        if (results == null || list == null) return;
        if (needClear) results.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                results.Add(list[i]);
            }
        }
    }


    /// <summary>
    /// 查询集合中满足条件的元素数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static int WhereCount<T>(this IEnumerable<T> list, Func<T, bool> conditionFunc)
    {
        int count = 0;
        if (list == null) return count;
        var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (conditionFunc.Invoke(item)) count++;
        }

        return count;
    }


    /// <summary>
    /// 查询集合中满足条件的元素数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static int WhereCount<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc)
    {
        int count = 0;
        if (list == null) return count;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i])) count++;
        }

        return count;
    }
    #endregion


    /// <summary>
    /// 从集合中筛选满足条件的元素，返回第一个满足条件的元素或者null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static T FirstOrNull<T>(this IEnumerable<T> list, Func<T, bool> conditionFunc) where T : class
    {
        if (list == null) return null;
        var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (conditionFunc.Invoke(item))
            {
                return item;
            }

        }
        return null;
    }

    /// <summary>
    /// 从集合中筛选满足条件的元素，返回第一个满足条件的元素或者null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static T FirstOrNull<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc) where T : class
    {
        if (list == null) return null;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                return list[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 从集合中筛选满足条件的元素，返回第一个满足条件的元素或者null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static T FirstOrNull<T>(this ILBetterList<T> list, Func<T, bool> conditionFunc) where T : class
    {
        if (list == null) return null;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                return list[i];
            }
        }
        return null;
    }



    /// <summary>
    /// 查询集合中是否有满足条件的元素(不论数量)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static bool Any<T>(this IEnumerable<T> list, Func<T, bool> conditionFunc)
    {
        if (list == null) return false;
        var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (conditionFunc.Invoke(item)) return true;
        }
        return false;
    }

    /// <summary>
    /// 查询集合中是否有满足条件的元素(不论数量)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static bool Any<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc)
    {
        if (list == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i])) return true;
        }
        return false;
    }

    /// <summary>
    /// 查询集合中是否有满足条件的元素(不论数量)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc">条件委托</param>
    /// <returns></returns>
    public static bool Any<T>(this ILBetterList<T> list, Func<T, bool> conditionFunc)
    {
        if (list == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i])) return true;
        }
        return false;
    }


    /// <summary>
    /// 查询列表中第一个满足条件的元素的key。未查找到会返回0。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc"></param>
    /// <returns></returns>
    public static int FirstKey<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc)
    {
        if (list == null) return 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                return i;
            }
        }

        return 0;
    }


    /// <summary>
    /// 从列表中删除第一个符合条件的元素。返回执行结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="conditionFunc"></param>
    /// <returns></returns>
    public static bool RemoveFirst<T>(this CSBetterLisHot<T> list, Func<T, bool> conditionFunc)
    {
        if (list == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (conditionFunc.Invoke(list[i]))
            {
                list.Remove(list[i]);
                return true;
            }
        }
        return false;
    }


    //public static T 

}
