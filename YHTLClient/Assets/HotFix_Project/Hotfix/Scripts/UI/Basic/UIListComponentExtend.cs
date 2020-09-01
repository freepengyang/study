using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;

public static class UIListComponentExtend
{
    public static void MakeActivedChildCount(this Transform parent,int count)
    {
        if (null == parent || parent.childCount <= 0)
            return;

        var template = parent.GetChild(0);
        if (null == template)
            return;

        int prevCount = parent.childCount;

        for (int i = parent.childCount, maxCount = count + 1; i < maxCount; ++i)
        {
            var go = Object.Instantiate(template.gameObject, parent) as GameObject;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.CustomActive(true);
        }

        for(int i = count + 1; i < prevCount; ++i)
        {
            var hideObj = parent.GetChild(i);
            hideObj.CustomActive(false);
        }

        template.CustomActive(false);
    }

    public static void MakeActivedChildCountNoTemplate(this Transform parent, int count)
    {
        if (null == parent || parent.childCount <= 0)
            return;

        var template = parent.GetChild(0);
        if (null == template)
            return;

        for (int i = parent.childCount, maxCount = count; i < maxCount; ++i)
        {
            var go = Object.Instantiate(template.gameObject, parent) as GameObject;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
        }
    }

    public static void Bind<T>(this UITable container, IList<T> values,System.Action<GameObject,T,object> onItemVisible,object p = null)
    {
        if(null != container)
        {
            int count = null == values ? 0 : values.Count;
            int prevCnt = container.transform.childCount;

            container.transform.MakeActivedChildCount(count);

            var it = values.GetEnumerator();
            for (int i = 1, maxCount = values.Count + 1; i < maxCount && it.MoveNext(); ++i)
            {
                var go = container.transform.GetChild(i).gameObject;
                onItemVisible?.Invoke(go,(T)it.Current, p);
            }

            container.Reposition();
        }
    }

    public static void Bind<T>(this UITable container, FastArrayElementKeepHandle<T> values, System.Action<GameObject, T, object> onItemVisible, object p = null) where T : class,new()
    {
        if (null != container)
        {
            int count = null == values ? 0 : values.Count;
            int prevCnt = container.transform.childCount;

            container.transform.MakeActivedChildCount(count);

            for (int i = 1, maxCount = values.Count + 1; i < maxCount; ++i)
            {
                var go = container.transform.GetChild(i).gameObject;
                onItemVisible?.Invoke(go, values[i], p);
            }

            container.Reposition();
        }
    }

    public static void Bind<T>(this UITable container, FastArrayElementFromPool<T> values, System.Action<GameObject, T, object> onItemVisible, object p = null) where T : class, new()
    {
        if (null != container)
        {
            int count = null == values ? 0 : values.Count;
            int prevCnt = container.transform.childCount;

            container.transform.MakeActivedChildCountNoTemplate(count);

            for (int i = 0, maxCount = values.Count; i < maxCount; ++i)
            {
                var go = container.transform.GetChild(i).gameObject;
                onItemVisible?.Invoke(go, values[i], p);
            }

            container.Reposition();
        }
    }

    public static void Bind<T>(this UIGrid container, IList<T> values, System.Action<GameObject, T, object> onItemVisible, object p = null)
    {
        if (null != container)
        {
            int count = null == values ? 0 : values.Count;
            int prevCnt = container.transform.childCount;

            container.transform.MakeActivedChildCountNoTemplate(count);

            for (int i = 0, maxCount = values.Count; i < maxCount; ++i)
            {
                var go = container.transform.GetChild(i).gameObject;
                onItemVisible?.Invoke(go, values[i], p);
            }

            container.Reposition();
        }
    }

    public static void Bind<T>(this UIGrid container, FastArrayElementFromPool<T> values, System.Action<GameObject, T, object> onItemVisible, object p = null) where T : class, new()
    {
        if (null != container)
        {
            int count = null == values ? 0 : values.Count;
            int prevCnt = container.transform.childCount;

            container.transform.MakeActivedChildCount(count);

            for (int i = 1, maxCount = values.Count + 1; i < maxCount; ++i)
            {
                var go = container.transform.GetChild(i).gameObject;
                onItemVisible?.Invoke(go, values[i], p);
            }

            container.Reposition();
        }
    }
}

public enum SortType
{
    Vertical = 0,
    Horizen = 1,
}
public class EndLessList<K,V> where V : class,new() where K : UIBinder,new()
{
    SortType mSortType = SortType.Vertical;
    UIWrapContent mContainer;
    FastArrayElementFromPool<V> mElements;
    PoolHandleManager mPoolHandleManager;
    FastArrayElementKeepHandle<Transform> mPools;
    ScriptBinder mFrame;
    private Dictionary<GameObject, int> itemIndexList = new Dictionary<GameObject, int>();

    
    void OnItemVisible(GameObject go, int wrapIndex, int realIndex)
    {
        if (itemIndexList.ContainsKey(go))
            itemIndexList[go] = realIndex;
        else
            itemIndexList.Add(go,realIndex);
        OnItemVisible(go,realIndex);
    }
    
    void OnItemVisible(GameObject go, int realIndex)
    {
        if (go == null) return;
        var binder = go.GetOrAddBinder<K>(mPoolHandleManager);
        realIndex = realIndex < 0 ? -realIndex : realIndex;
        if (realIndex >= 0 && realIndex < mElements.Count)
        {
            go.CustomActive(true);
            binder.Bind(mElements[realIndex]);
        }
        else
            go.CustomActive(false);
    }

    public FastArrayElementFromPool<V> Elements()
    {
        return mElements;
    }

    public void Sort(FastArrayElementFromPool<V>.ArrayCompare comparer)
    {
        mElements.Sort(comparer);
    }

    public void Bind()
    {
        mContainer.enabled = false;
        int count = null == mElements ? 0 : mElements.Count;
        if (mSortType == SortType.Vertical)
        {
            mContainer.minIndex = -count + 1;
            mContainer.maxIndex = 0;
        }
        else
        {
            mContainer.minIndex = 0;
            mContainer.maxIndex = count - 1;
        }
        mContainer.cullContent = false;
        //mContainer.SortBasedOnScrollMovement();
        mContainer.enabled = true;

        var dic = itemIndexList.GetEnumerator();
        while (dic.MoveNext())
        {
            OnItemVisible(dic.Current.Key, dic.Current.Value);
        }
    }

    public void Clear()
    {
        mElements.Clear();
    }

    public int Count
    {
        get
        {
            return mElements.Count;
        }
        set
        {
            mElements.Count = value;
        }
    }

    public V Append()
    {
        return mElements.Append();
    }

    protected void DetachChild()
    {
        if (mContainer.transform.childCount > 0)
        {
            for (int i = 0; i < mContainer.transform.childCount; ++i)
            {
                var child = mContainer.transform.GetChild(i);
                mPools.Append(child);
            }
            for (int i = 0; i < mPools.Count; ++i)
            {
                mPools[i].transform.SetParent(mFrame.transform);
                mPools[i].transform.CustomActive(false);
            }
        }
    }

    protected void AttachChild(int v)
    {
        int maxV = Mathf.Min(v, mPools.Count);
        for(int i = 0; i < maxV; ++i)
        {
            var child = mPools[mPools.Count - 1];
            mPools.SwapErase(mPools.Count - 1);
            child.transform.SetParent(mContainer.transform);
            child.CustomActive(true);
        }
    }

    public void Destroy()
    {
        DetachChild();
        for (int i = 0; i < mPools.Count; ++i)
        {
            var eventListener = mPools[i].transform.GetComponent<UIEventListener>();
            if(null != eventListener && eventListener.parameter is UIBinder binder)
            {
                binder.Destroy();
            }
        }
        mPools?.Clear();
        mPools = null;
        mElements?.Clear();
        mElements = null;
        mContainer = null;
        mFrame = null;
        mPoolHandleManager = null;
    }

    public EndLessList(SortType eSort,UIWrapContent container,PoolHandleManager poolHandleManager,int maxCount,ScriptBinder scriptBinder)
    {
        mFrame = scriptBinder;
        mPoolHandleManager = poolHandleManager;
        mSortType = eSort;
        mContainer = container;
        mContainer.transform.MakeActivedChildCountNoTemplate(maxCount);
        mPools = new FastArrayElementKeepHandle<Transform>(maxCount);
        mPools.Clear();
        //DetachChild();
        mElements = poolHandleManager.CreateGeneratePool<V>();
        if (null != container)
        {
            mContainer.onInitializeItem = OnItemVisible;
        }
    }
}




public class EndLessKeepHandleList<K, V> where V : class, new() where K : UIBinder, new()
{
    SortType mSortType = SortType.Vertical;
    UIWrapContent mContainer;
    FastArrayElementKeepHandle<V> mElements;
    PoolHandleManager mPoolHandleManager;
    FastArrayElementKeepHandle<Transform> mPools;
    ScriptBinder mFrame;
    private Dictionary<GameObject, int> itemIndexList = new Dictionary<GameObject, int>();

    void OnItemVisible(GameObject go, int wrapIndex, int realIndex)
    {
        if (itemIndexList.ContainsKey(go))
            itemIndexList[go] = realIndex;
        else
            itemIndexList.Add(go,realIndex);
        OnItemVisible(go,realIndex);
    }


    void OnItemVisible(GameObject go, int realIndex)
    {
        if (go == null) return;
        var binder = go.GetOrAddBinder<K>(mPoolHandleManager);
        realIndex = realIndex < 0 ? -realIndex : realIndex;
		//播放动画过程中，点击关闭，获取不到组件会报错，加判空放回
		if (mElements == null) return;
        if (realIndex >= 0 && realIndex < mElements.Count)
        {
            go.CustomActive(true);
            binder.Bind(mElements[realIndex]);
        }
        else
            go.CustomActive(false);
    }

    //public FastArrayElementFromPool<V> Elements()
    //{
    //    return mElements;
    //}

    //public void Sort(FastArrayElementFromPool<V>.ArrayCompare comparer)
    //{
    //    mElements.Sort(comparer);
    //}

    public void Bind()
    {
        mContainer.enabled = false;
        int count = null == mElements ? 0 : mElements.Count;
        if (mSortType == SortType.Vertical)
        {
            mContainer.minIndex = -count + 1;
            mContainer.maxIndex = 0;
        }
        else
        {
            mContainer.minIndex = 0;
            mContainer.maxIndex = count - 1;
        }
        mContainer.cullContent = false;
        //mContainer.SortBasedOnScrollMovement();
        mContainer.enabled = true;
        var dic = itemIndexList.GetEnumerator();
        while (dic.MoveNext())
        {
            OnItemVisible(dic.Current.Key, dic.Current.Value);
        }
    }

    /// <summary>
    /// 初始显示不为第一个的情况 
    /// </summary>
    /// <param name="curIndex">初始显示的索引</param>
    /// <param name="SceenNum">屏幕列表显示的个数</param>
    public void Bind(int curIndex , int SceenNum)
    {
        int index = curIndex > mElements.Count - SceenNum ? mElements.Count - SceenNum : curIndex;
        
        mContainer.enabled = false;
        int count = null == mElements ? 0 : mElements.Count;
        if (mSortType == SortType.Vertical)
        {
            mContainer.minIndex = -count + 1 + index;
            mContainer.maxIndex = 0 + index;
        }
        else
        {
            mContainer.minIndex = 0 - index;
            mContainer.maxIndex = count - 1 - index;
        }
        mContainer.cullContent = false;
        //mContainer.SortBasedOnScrollMovement();
        mContainer.enabled = true;
        var dic = itemIndexList.GetEnumerator();
        while (dic.MoveNext())
        {
            OnItemVisible(dic.Current.Key, dic.Current.Value);
        }
        
    }
    
    
    public void Clear()
    {
        mElements.Clear();
    }

    public int Count
    {
        get
        {
            return mElements.Count;
        }
        //set
        //{
        //    mElements.Count = value;
        //}
    }

    public void Append(V element)
    {
        mElements.Append(element);
    }

    protected void DetachChild()
    {
        if (mContainer.transform.childCount > 0)
        {
            for (int i = 0; i < mContainer.transform.childCount; ++i)
            {
                var child = mContainer.transform.GetChild(i);
                mPools.Append(child);
            }
            for (int i = 0; i < mPools.Count; ++i)
            {
                mPools[i].transform.SetParent(mFrame.transform);
                mPools[i].transform.CustomActive(false);
            }
        }
    }

    protected void AttachChild(int v)
    {
        int maxV = Mathf.Min(v, mPools.Count);
        for (int i = 0; i < maxV; ++i)
        {
            var child = mPools[mPools.Count - 1];
            mPools.SwapErase(mPools.Count - 1);
            child.transform.SetParent(mContainer.transform);
            child.CustomActive(true);
        }
    }

    public void Destroy()
    {
        DetachChild();
        for (int i = 0; i < mPools.Count; ++i)
        {
            var eventListener = mPools[i].transform.GetComponent<UIEventListener>();
            if (null != eventListener && eventListener.parameter is UIBinder binder)
            {
                binder.OnDestroy();
            }
        }
        mPools?.Clear();
        mPools = null;
        mElements?.Clear();
        mElements = null;
        mContainer = null;
        mFrame = null;
        mPoolHandleManager = null;
    }

    public EndLessKeepHandleList(SortType eSort, UIWrapContent container, PoolHandleManager poolHandleManager, int maxCount, ScriptBinder scriptBinder)
    {
        mFrame = scriptBinder;
        mPoolHandleManager = poolHandleManager;
        mSortType = eSort;
        mContainer = container;
        mContainer.transform.MakeActivedChildCountNoTemplate(maxCount);
        mPools = new FastArrayElementKeepHandle<Transform>(maxCount);
        mPools.Clear();
        //DetachChild();
        mElements = poolHandleManager.GetSystemClass<FastArrayElementKeepHandle<V>>();
        if (null != container)
        {
            mContainer.onInitializeItem = OnItemVisible;
        }
    }
}