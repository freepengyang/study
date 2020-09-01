using UnityEngine;
using System.Collections;

public enum EPoolType
{
    Normal,
    Resource,//场景模型Atlas，一直在缓存池里面，知道refCount=0并且超过最大未使用时间时，使用ResourcesManager删除资源
}

public class CSObjectPoolBase : MonoBehaviour
{
    public bool isRefresh = false;

    protected CSObjectPoolMgr mMgr;

    public bool isForeverCanChange = true;

    public bool mIsForever;
    public bool isForever
    {
        get { return mIsForever; }
        set
        {
            if (!isForeverCanChange) return;
            mIsForever = value;
        }
    }

    public string resName;

    public string poolName;
    /// <summary>
    /// 这个参数表示该缓存池，外部有
    /// </summary>
    public int refCount = 0;

    /// <summary>
    /// 同时最大存活个数
    /// </summary>
    public int poolNum = 10;

    /// <summary>
    /// 外部将GameObject返还给缓存池，如果超过了maxLiveCount，超过部分删除的时间间隔
    /// </summary>
    public float releaseInterval = 1;

    protected float mLastRealseTime = 0;

    /// <summary>
    /// 缓存池在多久没用的情况下，自动进行根据releaseInterval删除
    /// </summary>
    public float releaseTime
    {
        get
        {
#if UNITY_EDITOR||UNITY_ANDROID
            //return 300 * SFOut.IGame.GetPoolRealseApseat;
            return 300 * CSConstant.mPoolRealseApseat;
#elif UNITY_IOS
            return 10;
#endif
            return 10;
        }
    }

    protected float mLastNotUseTime = 0;

    public CSBetterList<CSObjectPoolItem> mList = new CSBetterList<CSObjectPoolItem>();
    public int ListCount
    {
        get
        {
            return mList.Count;
        }
    }

    public virtual void Init(CSObjectPoolMgr mgr)
    {
        mMgr = mgr;
    }

    public void MarkForeverCanChange(bool b)
    {
        isForeverCanChange = b;
    }

    public virtual CSObjectPoolItem GetGOFromPool()
    {
        return null;
    }

    public virtual void AddPoolItem(CSObjectPoolItem item)
    {

    }

    public virtual void RemovePoolItem(CSObjectPoolItem item, bool isDestroyResImmi = false)
    {

    }

    protected virtual void DestroyPoolItem(CSObjectPoolItem item)
    {

    }

    public virtual void CSUpdate()
    {
    }

    public virtual void CSOnDestroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
        if (mList != null)
        {
            mList.Release();
        }
    }

    public virtual void CSOnDestroy(bool isDestroyGameObject)
    {
        if (isDestroyGameObject && gameObject != null)
        {
            Destroy(gameObject);
        }
        if (mList != null)
        {
            mList.Release();
        }
    }
}
