using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CSSkillEffectData
{
    public int ID { get; set; }
    public int ModelID { get; set; }
    public float DelayTime { get; set; }
    public float Scale { get; set; }
    public int Layer { get; set; }
    public int Fps { get; set; }
    public int DepthType { get; set; }
    //value >= 8 is no rotation 
    public int RotationDirction { get; set; }
    //true: eight direction; false:single direction
    public bool IsMultiDirection { get; set; }
    public int FrameCount { get; set; }

    public int Num { get; set; }

    public string Point { get; set; }

    public float BeAttackDelayTime { get; set; }

    public int footEffectType;
    public float cameraShakeDelay;
    public float cameraShakeTime;
    public float cameraShakeAmplitude;

    public int targetCoordX;
    public int targetCoordY;
    public float modelHeight;
}

public class CSSkillEffect
{
    public event Action<CSResource> OnFinished = null;
    public GameObject entity;
    public Transform cacheTrans;
    public CSSkillAnimation animation; 
    public CSCell attackPosition ;
    public CSCell startPosition;
    //public CSSkillEffectData Info = new CSSkillEffectData();
    public CSSkillEffectData Info { get; set; }
    public AvatarUnit mAvatar { get; set; }
    protected AvatarUnit mAttackTarget { get; set; }
    protected CSResource mRes;
    protected Vector3 mHalfTargetHeight;
    protected TABLE.SKILLDIRECTION mSkillDir { get; set; }
    protected Dictionary<uint, TABLE.SKILLDIRECTION> mSkillDirectionDic { get; set; }

    private Vector3 lastAttackTarget = Vector3.zero;

    public bool mIsSelfSkillEffect = false;
    public bool mIsPlaying = false;     // false 未开始或停止中
    public bool mIsLoadingRes = false;
    public int curDirection = 8;
    public long mAttackTargetID = 0;
    public float speed = 4;
    public float mDepth = 0;
    private float mPlayTime = 0;
    private float mAnimationBeginPlayTime = 0;
    
    protected virtual ESkillEffectType mType
    {
        get
        {
            return ESkillEffectType.None;
        }
    }

    public virtual bool IsPlaying
    {
        get
        {
            return mIsPlaying;
        }
        set
        {
            if(mIsPlaying != value)
            {
                mIsPlaying = value;
                if (!mIsPlaying)
                {
                    IsLoadingRes = false;
                }
            }
        }
    }
    public bool IsLoadingRes
    {
        get { return mIsLoadingRes; }
        set
        {
            if (mIsLoadingRes == value) return;
            mIsLoadingRes = value;
        }
    }
    public Transform cacheWorldTrans
    {
        get
        {
            return CSPreLoadingBase.CahceWorldTrans;
        }
    }

    protected Vector3 TargetPosition
    {
        get
        {
            Vector3 targetPos = Vector3.zero;
            ///只有非死亡状态才去拿实时的位置，不然AttackTarget有可能已经被CSScene删除放入缓存池了
            if (AttackTarget != null && mAttackTargetID != AttackTarget.UnitID)
            {
                targetPos = lastAttackTarget;
            }
            else
            {
                if (AttackTarget != null)
                {
                    targetPos = AttackTarget.GetRealPosition2() + mHalfTargetHeight;
                }
                else if (attackPosition != null)
                {
                    targetPos = attackPosition.WorldPosition3 + mHalfTargetHeight;
                }
                lastAttackTarget = targetPos;
            }
            if (mType == ESkillEffectType.End || mType == ESkillEffectType.EndFoot)
            {
                lastAttackTarget.z = mDepth;
                targetPos.z = mDepth;
            }
            return targetPos;
        }
    }

    public AvatarUnit AttackTarget
    {
        get
        {
            return mAttackTarget;
        }
        set
        {
            mAttackTarget = value;

            if (mAttackTarget != null)
            {
                mAttackTargetID = mAttackTarget.UnitID;
            }
            else
            {
                mAttackTargetID = 0;
            }
        }
    }

    public Vector3 GetOffsetVec()
    {
        Vector3 vec3 = Vector3.zero;
        mSkillDir = null;
        if (mSkillDirectionDic != null)
        {
            if (mSkillDirectionDic.ContainsKey((uint)(curDirection)))
            {
                mSkillDir = mSkillDirectionDic[(uint)(curDirection)];
            }
            else
            {
                if (mSkillDirectionDic.ContainsKey((uint)CSDirection.None))
                    mSkillDir = mSkillDirectionDic[(uint)CSDirection.None];
            }
            if (mSkillDir != null)
            {
                vec3.x = mSkillDir.x - 10000;
                vec3.y = mSkillDir.y - 10000;
            }
        }
        return vec3;
    }

    public virtual bool IsTakeEffect
    {
        get
        {
            return (Info != null && Info.ModelID > 0);
        }
    }

    public void InitTargetCoord(int targetCoordX, int targetCoordY)
    {
        if (Info != null)
        {
            Info.targetCoordX = targetCoordX;
            Info.targetCoordY = targetCoordY;
        }
    }

    public void UpdateData(AvatarUnit target,AvatarUnit attack, int targetCoordX, int targetCoordY)
    {
        AttackTarget = target;
        mAvatar = attack;
        InitTargetCoord(targetCoordX,targetCoordY);
    }

    public virtual void Init(AvatarUnit avater,AvatarUnit attackTarget, CSSkillEffectData effectData, bool isSelfSkillEffect, Transform parent)
    {
        if (this == null) return;
        mAvatar = avater;
        AttackTarget = attackTarget;
        Info = effectData;
        mIsSelfSkillEffect = isSelfSkillEffect;
        mSkillDirectionDic = null;

        if (effectData != null && effectData.ModelID > 0)
        {
            if (entity == null)
            {
                entity = new GameObject(effectData.ModelID.ToString());
            }
            cacheTrans = entity.transform;
            entity.SetActive(false);
            NGUITools.SetParent(mAvatar.CacheTransform, cacheTrans.gameObject);
            if (animation == null)
            {
                animation = entity.AddComponent<CSSkillAnimation>();
                animation.InItData(entity, mAvatar.IsDataSplit);
            }
            if (SkillDirectionTableManager.Instance.modelDic.ContainsKey((uint)Info.ModelID))
            {
                mSkillDirectionDic = SkillDirectionTableManager.Instance.modelDic[(uint)Info.ModelID];
            }
        }
    }

    public void UpdateIAvater(AvatarUnit avater, AvatarUnit target)
    {
        mAvatar = avater;
    }

    public void SetRotationDirction(int dirction)
    {
        Info.RotationDirction = curDirection;
    }

    public void RemoveAttach()
    {
        if (CSPreLoadingBase.Instance.SkillPoolAnchor != null && cacheTrans != null)
        {
            if (entity != null)
            {
                entity.SetActive(false);
            }
            cacheTrans.parent = CSPreLoadingBase.Instance.SkillPoolAnchor;
        }
    }

    protected float GetAnchorHeight(AvatarUnit avatar)
    {
        if (avatar == null) return 0;
        if(mSkillDir != null && mSkillDir.maodian == 1)
        {
            return 0;
        }
        return avatar.GetModelHeight()*0.7f;
    }

    public virtual void Play(float time)
    {
        SetDirection();
        SetFrame();
        SetPos();
        SetRotation(TargetPosition);
        SetDepth();
        mPlayTime = time;
        if (animation != null)
        {
            animation.ClearCurrentNames();
            animation.ClearAtlas();
            if (mRes != null)
            {
                mRes.onLoaded -= OnFinishedCallBack;
            }
            if (Info.ModelID == 0)
            {
                IsPlaying = (animation.FrameAni == null) ? false : animation.FrameAni.isPlaying;
                OnLoadCallBack();
            }
            else if (mAvatar.IsDataSplit)
            {
                animation.SetCurrentNames(Info.FrameCount);
                animation.Play(mPlayTime);
                mAnimationBeginPlayTime = Time.time + mPlayTime;
                IsPlaying = (animation.FrameAni == null) ? false : animation.FrameAni.isPlaying;
                OnLoadCallBack();
                LoadResource(OnFinishedCallBack);
            }
            else
            {
                LoadResource(OnFinishedCallBack);
            }
        }
        else
        {
            if (Info.ModelID == 0)
            {
                IsPlaying = false;
                OnLoadCallBack();
            }
        }
    }

    public void LoadResource(Action<CSResource> onFinished)
    {
        string name = string.Empty;
        this.OnFinished = onFinished;
        if (Info.ModelID == 0)
        {
            OnLoadedResource(null);
            return;
        }
        long key = 0;
        if (!Info.IsMultiDirection)
        {
            cacheTrans.localScale = Vector3.one;

            key = CSMisc.GetKey(Info.ModelID, 0, 0);
            string path = CSResourceManager.Singleton.GetKeyPath(key);

            if (!string.IsNullOrEmpty(path))
            {
                name = path;
            }
            else
            {
                CSStringBuilder.Clear();
                CSStringBuilder.Append(Info.ModelID.ToString(), "_0");
                name = CSStringBuilder.ToString();
                path = CSResource.GetPath(name, ResourceType.SkillAtlas, false);
                name = path;
            }
        }
        else
        {
            int direction = mAvatar.GetDirection();
            Vector3 realScale = new Vector3(Info.Scale, Info.Scale, Info.Scale);
            int d = direction;
            if (d == CSDirection.Left)
            {
                d = CSDirection.Right;
                realScale.x = -realScale.x;
            }
            else if (d == CSDirection.Left_Up)
            {
                d = CSDirection.Right_Up;
                realScale.x = -realScale.x;
            }
            else if (d == CSDirection.Left_Down)
            {
                d = CSDirection.Right_Down;
                realScale.x = -realScale.x;
            }

            direction = (int)d;
            cacheTrans.localScale = realScale;
            key = CSMisc.GetKey(Info.ModelID, 0, direction);
            string path = CSResourceManager.Singleton.GetKeyPath(key);
            if (!string.IsNullOrEmpty(path))
            {
                name = path;
            }
            else
            {
                CSStringBuilder.Clear();
                CSStringBuilder.Append(Info.ModelID.ToString(), "_", direction.ToString());
                name = CSStringBuilder.ToString();
                path = CSResource.GetPath(name, ResourceType.SkillAtlas, false);
                name = path;
            }
        }
        if (mRes != null)
        {
            mRes.onLoaded -= OnLoadedResource;
        }
        IsLoadingRes = true;
        IsPlaying = (animation == null || animation.FrameAni == null) ? false : animation.FrameAni.isPlaying;

        if (CSMisc.avatarLoadProriDic.ContainsKey(mAvatar.AvatarType))
        {
            CSResourceManager.Instance.AddQueue(name, ResourceType.SkillAtlas, OnLoadedResource, CSMisc.avatarLoadProriDic[(int)mAvatar.AvatarType], true, key);
        }
    }

    void OnLoadedResource(CSResource res)
    {
        IsLoadingRes = false;
        if (OnFinished != null)
        {
            OnFinished(res);
            OnFinished = null;
        }
    }

    void AtlasPoolItemDeal(UIAtlas atlas)
    {
        if (CSObjectPoolMgr.Instance != null)
        {
            CSObjectPoolItem poolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Resource(atlas.name, atlas.ResPath, null);
            CSObjectPoolMgr.Instance.RemovePoolItem(poolItem);
        }
    }

    void OnFinishedCallBack(CSResource res)
    {
        GameObject g = res.MirrorObj as GameObject;

        if (g == null) return;

        UIAtlas atlas = g.GetComponent<UIAtlas>();

        if (atlas == null) return;

        atlas.ResPath = res.Path;
        AtlasPoolItemDeal(atlas);

        //if (!Skill.isShow)
        //{
        //    Stop();
        //}
        //else
        {
            IsLoadingRes = false;
            if (animation != null)
            {
                animation.setAtlas(atlas);
                CSSkillFrame sf = animation.FrameAni as CSSkillFrame;
                if (sf != null)
                {
                    animation.Play(mPlayTime);
                    mAnimationBeginPlayTime = Time.time + mPlayTime;
                }
                EShareMatType mType = EShareMatType.Normal;
                switch ((ESkillEffectLayer)Info.Layer)
                {
                    case ESkillEffectLayer.Overly:
                        mType = EShareMatType.ColorAdd;
                        break;
                    case ESkillEffectLayer.Colourfilter:
                        mType = EShareMatType.ColorScreen;
                        break;
                }
                animation.Sprite.SetShader(CSShaderManager.GetShareMaterial(atlas, mType), Vector4.one, Vector4.one);
                IsPlaying = (animation.FrameAni == null) ? false : animation.FrameAni.isPlaying;
            }
            else
            {
                IsPlaying = false;
            }

            if (!mAvatar.IsDataSplit)
            {
                OnLoadCallBack();
            }
        }
    }

    protected virtual void OnLoadCallBack()
    {

    }

    public void SetDirection()
    {
        if (mAvatar != null)
        {
            curDirection = (int)mAvatar.GetDirection();
        }
    }

    public void SetFrame()
    {
        if (animation != null)
        {
            animation.setFPS((int)Info.Fps);
        }
    }

    protected virtual void SetPos()//缩放在初始化的时候处理了
    {
    }

    protected void SetRotation(Vector3 targetPos)
    {
        if (entity == null) return;
        if (Info.RotationDirction != 8)
        {
            if (animation.CahcheTrans)
            {
                Vector3 v = targetPos - animation.CahcheTrans.position;
                float zAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                cacheTrans.rotation = Quaternion.Euler(0, 0, Info.RotationDirction * 45 + zAngle);
            }
        }
    }

    /// <summary>
    /// 无论哪一段都只是在播放的时候设置一次
    /// </summary>
    public void SetDepth()
    {
        if (entity == null || Info == null)
        {
            return;
        }

        CSCell cell = null;

        switch (mType)
        {
            case ESkillEffectType.Start:
            case ESkillEffectType.Move:
                cell = mAvatar.OldCell;
                break;
            case ESkillEffectType.End:
            case ESkillEffectType.EndFoot:
                cell = (AttackTarget != null) ? AttackTarget.OldCell : attackPosition;
                break;
        }

        if (cell == null)
        {
            return;
        }
        float depth = CSMisc.GetDepth(cell, Info.DepthType);
        Vector3 pos = new Vector3(0, 0, depth);
        if(cacheWorldTrans != null)
        {
            pos = cacheWorldTrans.TransformPoint(pos);
        }
        cacheTrans.position = new Vector3(cacheTrans.position.x, cacheTrans.position.y, pos.z);
        if (mType == ESkillEffectType.End || mType == ESkillEffectType.EndFoot)
        {
            mDepth = pos.z;
        }
    }

    public virtual float GetEffectTime()
    {
        if (animation != null)
        {
            return animation.FrameAni.EstimateTakeTime;
        }
        return 0f;
    }

    public virtual float DelayTime
    {
        get
        {
             if (Info == null)
            {
                return 0;
            }
            float t = (Info.FrameCount / (float)Info.Fps)* Info.DelayTime;
            return t;
        }
    }

    public virtual void Update()
    {
        if(IsPlaying)
        {
            if(animation != null)
            {
                if (Time.time > mAnimationBeginPlayTime && mAnimationBeginPlayTime > 0)
                {
                    OnStart();
                    mAnimationBeginPlayTime = 0;
                }
            }
        }
    }

    public virtual void OnStart()
    {
    }

    public void OnFinish()
    {
        BaseFrame b = animation.FrameAni as BaseFrame;
        b.onFinish -= OnFinish;
        Stop();
        if (animation != null && animation.FrameAni != null)
        {
            IsPlaying = animation.FrameAni.isPlaying; //false 说明从未开始，或，结束。
        }
    }

    public virtual void Stop()
    {
        OnFinished = null;
        IsPlaying = false;
        IsLoadingRes = false;
        if (entity != null)
        {
            entity.SetActive(false);
        }
    }

    public virtual void Release()
    {

    }

    public virtual void Destroy()
    {
        if(entity != null)
        {
            GameObject.Destroy(entity);
        }
        entity = null;
        cacheTrans = null;
        Info = null;
        mAttackTarget = null;
        mRes = null;
    }
 

}
