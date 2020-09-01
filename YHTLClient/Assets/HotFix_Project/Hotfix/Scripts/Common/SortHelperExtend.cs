using Google.Protobuf.Collections;
using System.Collections.Generic;

public static class SortHelperExtend
{
    public delegate void MakeSortHandle<T>(ref long sortedValue, T value);
    public static void Sort<T>(this FastArrayElementKeepHandle<T> buffer,MakeSortHandle<T> maker) where T : class
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.LongGreat, 0);

        int count = buffer.Count;
        var handles = SortHelper.GetSortHandle(count);
        SortHelper.SortHandle handle = null;
        T data = null;
        for (int i = 0, max = count; i < max; ++i)
        {
            handle = handles[i];
            data = buffer[i];
            handle.handle = data;
            maker(ref handle.longValue[0], data);
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0; i < count; ++i)
        {
            buffer[i] = handles[i].handle as T;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    public static void Sort<T>(this FastArrayElementFromPool<T> buffer, MakeSortHandle<T> maker) where T : class
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.LongGreat, 0);

        int count = buffer.Count;
        var handles = SortHelper.GetSortHandle(count);
        SortHelper.SortHandle handle = null;
        T data = null;
        for (int i = 0, max = count; i < max; ++i)
        {
            handle = handles[i];
            data = buffer[i];
            handle.handle = data;
            maker(ref handle.longValue[0], data);
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0; i < count; ++i)
        {
            buffer[i] = handles[i].handle as T;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    public static void Sort<T>(this ILBetterList<T> buffer, MakeSortHandle<T> maker) where T : class
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.LongGreat, 0);

        int count = buffer.Count;
        var handles = SortHelper.GetSortHandle(count);
        SortHelper.SortHandle handle = null;
        T data = null;
        for (int i = 0, max = count; i < max; ++i)
        {
            handle = handles[i];
            data = buffer[i];
            handle.handle = data;
            maker(ref handle.longValue[0], data);
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0; i < count; ++i)
        {
            buffer[i] = handles[i].handle as T;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    public static void Sort<T>(this RepeatedField<T> buffer, MakeSortHandle<T> maker) where T : class
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.LongGreat, 0);

        int count = buffer.Count;
        var handles = SortHelper.GetSortHandle(count);
        SortHelper.SortHandle handle = null;
        T data = null;
        for (int i = 0, max = count; i < max; ++i)
        {
            handle = handles[i];
            data = buffer[i];
            handle.handle = data;
            maker(ref handle.longValue[0], data);
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0; i < count; ++i)
        {
            buffer[i] = handles[i].handle as T;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    public static void Sort<T>(this List<T> buffer, MakeSortHandle<T> maker) where T : class
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.LongGreat, 0);

        int count = buffer.Count;
        var handles = SortHelper.GetSortHandle(count);
        SortHelper.SortHandle handle = null;
        T data = null;
        for (int i = 0, max = count; i < max; ++i)
        {
            handle = handles[i];
            data = buffer[i];
            handle.handle = data;
            maker(ref handle.longValue[0], data);
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0; i < count; ++i)
        {
            buffer[i] = handles[i].handle as T;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
}