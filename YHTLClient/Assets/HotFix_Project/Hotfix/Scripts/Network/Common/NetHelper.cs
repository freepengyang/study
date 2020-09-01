using Google.Protobuf.Collections;
using System.Collections.Generic;

public static class NetHelper
{
    public static RepeatedField<T> ToGoogleList<T>()
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this T element,params T[] elements)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        ret.Add(element);
        ret.AddRange(elements);
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this HashSet<T> element)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        ret.Add(element);
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this T element)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        ret.Add(element);
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this T[] array)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        ret.AddRange(array);
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this List<T> array)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        ret.AddRange(array);
        return ret;
    }

    public static RepeatedField<T> BetterLisToGoogleList<T>(this CSBetterLisHot<T> array)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        for(int i = 0; i < array.Count; ++i)
        {
            ret.Add(array[i]);
        }
        return ret;
    }

    public static RepeatedField<T> ToGoogleList<T>(this FastArrayMeta<T> array)
    {
        RepeatedField<T> ret = CSNetRepeatedFieldPool.Get<T>();
        ret.Clear();
        for (int i = 0; i < array.Count; ++i)
        {
            ret.Add(array[i]);
        }
        return ret;
    }
}