using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EffectBase
{
    public class EPlayType
    {
        public const int OnceDestroy = 0;
        public const int Loop = 1;
        public const int Once = 2;
    }  

    public Transform root = null;
    protected GameObject mGoRoot = null;
    protected Transform mParent = null;
    protected CSSprite mSprite = null;
    protected MeshFilter mMeshFilter = null;
    protected MeshRenderer mMeshRenderer = null;
    protected CSEffectAnimation mEffectAnimation = null;
    protected CSObjectPoolItem mPoolItem = null;
    protected Vector3 mLocalPosition = Vector3.zero;
    protected int playType = 0;  //0: frame effect 1:particle effect
    protected Schedule mSchedule = null;
    protected float mDestroyTime = 0;
    protected int mPlayType;
    protected System.Action onLoadCallBack;
    protected System.Action onPlayFinishedCallBack;
    protected System.Action<int> onDestroyCallBack;
    public CSObjectPoolItem PoolItem
    {
        get { return mPoolItem; }
        set { mPoolItem = value; }
    }
    public bool IsActive { get; set; }

    public CSSprite Sprite
    {
        get
        {
            return mSprite;
        }
    }


    public void Init(Transform parent, string resName,Vector3 localPosition,int playType,float destroyTime = 0,
        int resType = ResourceType.Effect, System.Action onLoadCallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        IsActive = true;
        mDestroyTime = destroyTime;
        mLocalPosition = localPosition;
        mParent = parent;
        mPlayType = playType;
        this.onLoadCallBack = onLoadCallBack;
        CSResourceManager.Singleton.AddQueue(resName, resType, OnLoaded, resAssistType);
    }

    protected bool IsLoop()
    {
        return (mPlayType == EPlayType.Loop);
    }

    public bool IsDestroy()
    {
        return (mPlayType == EPlayType.OnceDestroy);
    }

    public void SetPlayFinishedCallBack(System.Action finishedCallBack)
    {
        onPlayFinishedCallBack = finishedCallBack;
    }

    public void SetDestroyCallBack(System.Action<int> destroyCallBack)
    {
        onDestroyCallBack = destroyCallBack;
    }

    public void SetAvtive(bool value)
    {
        if(mGoRoot != null && mGoRoot.activeSelf != value)
        {
            mGoRoot.SetActive(value);
        }
    }

    public void Replay()
    {
        if(mEffectAnimation != null)
        {
            mEffectAnimation.Play();
        }
    }

    public void Play(Vector3 localPosition)
    {
        if(mGoRoot != null && mEffectAnimation != null)
        {
            mLocalPosition = localPosition;
            mGoRoot.transform.localPosition = mLocalPosition;
            mEffectAnimation.Play();
        }
    }

    public void AddGo(string resName)
    {
        if (mParent == null)
        {
            return;
        }
        if (mGoRoot == null)
        {
            mGoRoot = new GameObject(resName);
            root = mGoRoot.transform;
        }
        else
        {
            mGoRoot.SetActive(true);
            IsActive = true;
            mGoRoot.name = resName;
        }
        Transform trans = mGoRoot.transform;
        trans.parent = mParent;
        trans.localScale = Vector3.one;
        trans.localPosition = mLocalPosition;
        if (mPoolItem != null)
        {
            mPoolItem.go = mGoRoot;
        }
        if (!IsLoop() && mDestroyTime > 0)
        {
            float destroyTime = mDestroyTime / 1000f;
            mSchedule = Timer.Instance.Invoke(destroyTime, DestroyEffect);
        }
    }

    public virtual void OnLoaded(CSResource res)
    {
        if (res == null || res.MirrorObj == null)
        {
            return;
        }
        if (mParent == null)
        {
            return;
        }
        GameObject go = res.MirrorObj as GameObject;
        if (go == null)
        {
            return;
        }
        UIAtlas atlas = go.GetComponent<UIAtlas>();
        if (atlas == null)
        {
            return;
        }
        AddGo(res.FileName);
        if (mMeshFilter == null)
        {
            mMeshFilter = mGoRoot.AddComponent<MeshFilter>();
        }
        if (mMeshRenderer == null)
        {
            mMeshRenderer = mGoRoot.AddComponent<MeshRenderer>();
        }

        if (mSprite == null)
        {
            mSprite = mGoRoot.AddComponent<CSSprite>();
        }
        mSprite.Atlas = atlas;
        mEffectAnimation = mGoRoot.GetComponent<CSEffectAnimation>();
        if (mEffectAnimation == null)
        {
            mEffectAnimation = mGoRoot.AddComponent<CSEffectAnimation>();
        }
        mEffectAnimation.RefreshNames();
        mEffectAnimation.setLoop(IsLoop());
        mEffectAnimation.StopFrameType = (mPlayType == EPlayType.Once) ? EActionStopFrameType.End : EActionStopFrameType.None;

        if (onLoadCallBack != null)
        {
            onLoadCallBack();
        }
    }

    protected void DestroyEffect(Schedule schedule)
    {
        if(onPlayFinishedCallBack != null)
        {
            onPlayFinishedCallBack();
        }
        if(onDestroyCallBack != null)
        {
            int key = this.GetHashCode();
            onDestroyCallBack(key);
        }
        Destroy();
    }

    public virtual void Release()
    {
        if (mSchedule != null)
        {
            Timer.Instance.CancelInvoke(mSchedule);
        }
        if (CSObjectPoolMgr.Instance != null && mPoolItem != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(mPoolItem);
        }
        mLocalPosition = Vector3.zero;
        mPoolItem = null;
        IsActive = false;
        SetAvtive(false);
        mPlayType = EPlayType.OnceDestroy;
        onLoadCallBack = null;
        onPlayFinishedCallBack = null;
        onDestroyCallBack = null;
    }

    public virtual void Destroy()
    {
        Release();
        if (mGoRoot != null)
        {
            GameObject.Destroy(mGoRoot);
        }
        mSchedule = null;
        mGoRoot = null;
        root = null;
        mParent = null;
        mSprite = null;
        mMeshFilter = null;
        mMeshRenderer = null;
        mEffectAnimation = null;
    }
}
