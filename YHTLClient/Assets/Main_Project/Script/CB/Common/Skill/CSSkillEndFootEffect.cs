using UnityEngine;
using System.Collections;

public class CSSkillEndFootEffect : CSSkillEffect
{
    private bool mIsMove = false;
    protected override ESkillEffectType mType
    {
        get
        {
            return ESkillEffectType.EndFoot;
        }
    }

    public override void Play(float time)
    {
        if (entity != null && entity.activeSelf)
        {
            entity.SetActive(false);
        }
        base.Play(time);
    }

    protected override void OnLoadCallBack()
    {
        if (animation != null)
        {
            BaseFrame b = animation.FrameAni as BaseFrame;

            b.onFinish -= OnFinish;
            b.onFinish += OnFinish;
        }
    }

    protected override void SetPos()
    {
        if (entity == null || cacheWorldTrans == null)
        {
            return;
        }
        if (AttackTarget == null && attackPosition == null)
        {
            return;
        }
        Vector3 offsetVec = GetOffsetVec();
        Vector3 localPostion;
        Vector3 vec;
        if (Info.footEffectType != 0)
        {
            localPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(mAvatar), 0);
            mHalfTargetHeight = mAvatar.CacheTransform.TransformPoint(localPostion);
            vec = mHalfTargetHeight;
        }
        else
        {
            localPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(AttackTarget), 0);
            mHalfTargetHeight = cacheWorldTrans.TransformPoint(localPostion);
            vec = TargetPosition;
        }
        vec.z = cacheTrans.position.z;
        cacheTrans.position = vec;
    }

    public override void OnStart()
    {
        if (animation != null)
        {
            BaseFrame b = animation.FrameAni as BaseFrame;
            if (animation.Go != null)
            {
                animation.Go.SetActive(true);
            }
            mIsMove = true;
        }
        base.OnStart();
    }

    public override void Update()
    {
        base.Update();
        if(Info == null || Info.BeAttackDelayTime <= CSMisc.FloatZero || (Info.footEffectType != 0))
        {
            return;
        }
        if (mIsMove && entity != null)
        {
            entity.transform.position = TargetPosition;
        }
    }

    public override void Stop()
    {
        base.Stop();
        mIsMove = false;
    }
}
