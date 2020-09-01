using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//此类适合与T不需要NEW出来，来自与其他数据区的类
public static class SortHelper
{
    public const int IntLess = 1;
    public const int IntGreat = 2;
    public const int LongLess = 3;
    public const int LongGreat = 4;

    public class SortHandle
    {
        public object handle;
        public int[] intValue = new int[16];
        public long[] longValue = new long[4];
        public static int Compare(SortHandle l, SortHandle r, List<FixedCompare> comparers)
        {
            int cnt = comparers.Count;
            int intIdx = 0;
            int longIdx = 0;
            for(int i = 0; i < cnt; ++i)
            {
                if(comparers[i].CompareType == IntLess)
                {
                    if (l.intValue[intIdx] != r.intValue[intIdx])
                        return r.intValue[intIdx] - l.intValue[intIdx];
                    ++intIdx;
                }
                else if (comparers[i].CompareType == IntGreat)
                {
                    if (l.intValue[intIdx] != r.intValue[intIdx])
                        return l.intValue[intIdx] - r.intValue[intIdx];
                    ++intIdx;
                }
                else if (comparers[i].CompareType == LongLess)
                {
                    if (l.longValue[longIdx] != r.longValue[longIdx])
                        return l.longValue[longIdx] < r.longValue[longIdx] ? 1 : -1;
                    ++longIdx;
                }
                else if (comparers[i].CompareType == LongGreat)
                {
                    if (l.longValue[longIdx] != r.longValue[longIdx])
                        return l.longValue[longIdx] < r.longValue[longIdx] ? -1 : 1;
                    ++longIdx;
                }
            }
            return 0;
        }
    }

    public class FixedCompare
    {
        public int CompareType;
        public int idx;
    }

    static Stack<List<FixedCompare>> mCachedCompareList = new Stack<List<FixedCompare>>(128);
    public static List<FixedCompare> GetComparersList(int capacity)
    {
        if (mCachedCompareList.Count > 0)
            return mCachedCompareList.Pop();
        return new List<FixedCompare>(capacity < 32 ? 32 : capacity);
    }

    static Stack<FixedCompare> mCachedCompares = new Stack<FixedCompare>(128);
    static FixedCompare Get()
    {
        if (mCachedCompares.Count > 0)
            return mCachedCompares.Pop();
        return new FixedCompare();
    }

    public static void AddCompare(this List<FixedCompare> fixedComparers,int compareType,int idx)
    {
        var fixedCompare = Get();
        fixedCompare.CompareType = compareType;
        fixedCompare.idx = idx;
        fixedComparers.Add(fixedCompare);
    }

    public static void OnRecycle(this List<FixedCompare> fixedComparers)
    {
        for(int i = 0,max = fixedComparers.Count;i < max; ++i)
        {
            mCachedCompares.Push(fixedComparers[i]);
        }
        fixedComparers.Clear();
        mCachedCompareList.Push(fixedComparers);
    }

    static Stack<List<SortHandle>> mHandleCachedHandleList = new Stack<List<SortHandle>>(32);
    static Stack<SortHandle> mCachedHandles = new Stack<SortHandle>(64);
    public static List<SortHandle> GetSortHandle(int count)
    {
        List<SortHandle> handleList = null;
        if (mHandleCachedHandleList.Count > 0)
            handleList = mHandleCachedHandleList.Pop();
        else
            handleList = new List<SortHandle>(count < 32 ? 32 : count);
        for(int i = 0; i < count;++i)
        {
            var handle = mCachedHandles.Count > 0 ? mCachedHandles.Pop() : new SortHandle();
            handleList.Add(handle);
        }
        return handleList;
    }

    public static void OnRecycle(this List<SortHandle> handleList)
    {
        for (int i = 0,count = handleList.Count; i < count; ++i)
        {
            handleList[i].handle = null;
            mCachedHandles.Push(handleList[i]);
        }
        handleList.Clear();
        mHandleCachedHandleList.Push(handleList);
    }
    /// <summary>
    /// 快速排序
    /// </summary>
    /// <param name="Key">待排序数组</param>
    /// <param name="left">数组最左端索引</param>
    /// <param name="right">数组最右端索引</param>
    private static void Quick(List<SortHandle> Key, int left, int right, List<FixedCompare> comparers)
    {
        SortHandle current = Key[left];
        int i = left;
        int j = right;

        if (left < right)
        {
            while (i < j) // 当i=j时，表示i之前的数均比current小，之后的数均比current大，即i是current在正确排序序列中的正确位置
            {
                while (SortHandle.Compare(current,Key[j], comparers) < 0 && i < j)
                {
                    --j;
                }

                while (SortHandle.Compare(current, Key[i], comparers) >= 0 && i < j)
                {
                    ++i;
                }

                if (i < j)
                {
                    SortHandle temp = Key[i];
                    Key[i] = Key[j];
                    Key[j] = temp;
                }
            }
            Key[left] = Key[i];
            Key[i] = current; // 将current放置到正确的位置上
            if (left < j - 1)
                Quick(Key, left, j - 1, comparers);
            if (j + 1 < right)
                Quick(Key, j + 1, right, comparers);
        }
    }

    public static void Sort(List<SortHandle> mBuffer,List<FixedCompare> comparers)
    {
        if(mBuffer.Count > 1)
            Quick(mBuffer, 0, mBuffer.Count - 1, comparers);
    }
}