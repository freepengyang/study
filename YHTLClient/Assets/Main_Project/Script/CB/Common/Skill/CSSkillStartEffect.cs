using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CSSkillStartEffect : CSSkillEffect
{
    private float mBeginPlayTime = 0;
    private float mDucationTime = 0;
    private bool mIsBeginDelay = false;

    public CSSkillMoveEffectMulti moveEffect { get; set; }
    public CSSkillEndEffectMulti endEffect;
    public CSSkillEndFootEffectMulti endFootEffect;

    protected override ESkillEffectType mType { get { return ESkillEffectType.Start; } }

    public bool IsBeginDelay
    {
        get { return mIsBeginDelay; }
        set { mIsBeginDelay = value; }
    }

    public override void Play(float time)
    {
        mDucationTime = time;
        mBeginPlayTime = Time.time;
        mIsBeginDelay = (Info != null) && (Info.DelayTime != 0);
        if (!mIsBeginDelay)
        {
            base.Play(time);
        }
    }

    public override void Update()
    {
        base.Update();
        if (mIsBeginDelay)
        {
            if(Info != null)
            {
                if (Time.time - mBeginPlayTime >= Info.DelayTime)
                {
                    mIsBeginDelay = false;
                    base.Play(mDucationTime);
                }
            }
        }
    }

    protected override void OnLoadCallBack()
    {
        base.OnLoadCallBack();

        if (animation != null && entity != null)
        {
            BaseFrame b = animation.FrameAni as BaseFrame;
            b.onFinish -= OnFinish;
            b.onFinish += OnFinish;
            entity.SetActive(true);
        }
        else
        {
            OnStart();
        }
    }

    protected override void SetPos()
    {
        if (entity == null)
        {
            return;
        }
        Vector3 vec;
        Vector3 offsetVec = GetOffsetVec();
        Vector3 localPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(mAvatar), 0);
        mHalfTargetHeight = mAvatar.CacheTransform.TransformPoint(localPostion);
        vec = mHalfTargetHeight;
        vec.z = cacheTrans.position.z;
        cacheTrans.position = vec;
    }

    public override void OnStart()
    {
        if (mIsSelfSkillEffect)
        {
            CSSkillEffect skillResult = null;
            float delayTime = 0f;
            bool isPlayEndEffect = true;
            if (moveEffect != null)
            {
                skillResult = moveEffect.GetSelfSkillEffect();
                if(skillResult != null && skillResult.animation != null)
                {
                    isPlayEndEffect = false;
                    if (skillResult.Info != null && skillResult.Info.DelayTime > 0)
                    {
                        delayTime = (animation != null) ? (GetEffectTime() * skillResult.Info.DelayTime) : skillResult.DelayTime;
                    }
                    moveEffect.Play(delayTime);
                }
            }
            if(isPlayEndEffect)
            {
                if (endEffect != null)
                {
                    endEffect.Play(delayTime);
                }

                if (endFootEffect != null)
                {
                    endFootEffect.Play(delayTime);
                }
            }
        }
        PlayCameShake();
        base.OnStart();
    }

    public void PlayCameShake()
    {
        if(mAvatar != null && mAvatar.IsMasterSelf)
        {
            if (Info != null)
            {
                if (Info.cameraShakeTime > CSMisc.FloatZero && Info.cameraShakeTime > CSMisc.FloatZero)
                {
                    CSPreLoadingBase.Instance.ShakeCamera(Info.cameraShakeDelay, Info.cameraShakeTime, Info.cameraShakeAmplitude);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
    }
}
