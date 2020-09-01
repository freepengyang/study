using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;

public class ESkillResultState
{
    public const int None = 0;
    public const int StartPlay = 1;
    public const int Playing = 2;
    public const int Unuse = 3;
};
public class CSSkillResult 
{
    public CSObjectPoolItem poolItem;
    private CSAvatar mAttacker = null;
    private CSSkill mSkill = null;
    private fight.SkillEffect mEffect = null;
    private RepeatedField<fight.EffectedTarget> mTargetEffects;
    private RepeatedField<long> mIsCriticalIds;
    private RepeatedField<long> mMissedIds;
    private RepeatedField<long> mImmuPhyAttIds;
    private const float HURT_PLAY_TIME = 1.5f;   //UIActorHurtEffect time
    private const float PLAY_DELAY_TIME = 0.15f;
    private const float PLAY_MAX_TIME = 3.0f;
    private float mStartTime = 0;
    private int mState;

    private float mDelayTime = 0;


    public void Initialize(fight.SkillEffect effect,CSAvatar attacker, CSAvatar target)
    {
        if (effect == null|| CSScene.Sington == null)
        {
            return;
        }
        mEffect = effect;
        mTargetEffects = mEffect.targets;
        mMissedIds = mEffect.missedIds;
        mIsCriticalIds = mEffect.isCriticalIds;
        mImmuPhyAttIds = mEffect.immuPhyAttIds;
        mAttacker = attacker;
        mState = ESkillResultState.StartPlay;
        AddSkill();

        mDelayTime = (mSkill != null) ? mSkill.GetStartPlayHurt() : 0.2f;

        OnSkillEffect();
        //mStartTime = (mSkill != null) ? Time.time + mSkill.GetStartPlayHurt() : (Time.time + 0.2f);
    }

    public void Update()
    {
        switch (mState)
        {
            case ESkillResultState.StartPlay:
                {
                    //if (Time.time - mStartTime >= PLAY_DELAY_TIME)
                    //if (mAttacker != null && mAttacker.IsPlayHurt)
                    if(Time.time >= mStartTime)
                    {
                        OnSkillEffect();
                        mState = ESkillResultState.Playing;
                        mStartTime = Time.time;
                    }
                }
                break;
            case ESkillResultState.Playing:
                {
                    if (Time.time - mStartTime >= HURT_PLAY_TIME)
                    {
                        mState = ESkillResultState.Unuse;
                        Remove();
                    }
                }
                break;
            default:
                break;
        }

        if(mState != ESkillResultState.Unuse && mState != ESkillResultState.None)
        {
            if (Time.time - mStartTime > PLAY_MAX_TIME)
            {
                mState = ESkillResultState.Unuse;
                Remove();
            }
        }
     
    }

    public void Remove()
    {
        mState = ESkillResultState.None;
        mStartTime = 0;
    }

    public bool IsUnuse()
    {
        return (mState == ESkillResultState.Unuse || mState == ESkillResultState.None);
    }

    private void AddSkill()
    {
        if (mAttacker == null)
        {
            return;
        }
        mSkill = mAttacker.SkillEngine.AddSkillById(mEffect.skillId);
        mAttacker.TouchEvent.Skill = mSkill;
    }

    public void OnSkillEffect()
    {
        PlayMissId();
        //PlayCrit();
        PlayNormalDamage();
        PlayImmuPhyAttIds();
        PlayBeattackAudio();
    }
    private void PlayNormalDamage()
    {
        if (mTargetEffects == null || mAttacker == null)
        {
            return;
        }
        for (int i = 0; i < mTargetEffects.Count; ++i)
        {
            fight.EffectedTarget target = mTargetEffects[i];
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(target.id);
            if (avatar == null || avatar.head == null || avatar.BaseInfo == null)
            {
                continue;
            }
            int dir = UtilityFight.GetFlyDirection(mAttacker,avatar);
            //avatar.head.PlayHurtEffect(target.deltaHP, dir, ThrowTextType.NormalDamage);

            int textType = GetTextType(target.id);
            avatar.head.PlayHurtEffect(target.deltaHP, dir,mDelayTime,textType);

            if (avatar.BaseInfo.HP > 0)
            {
                avatar.BaseInfo.HP = avatar.BaseInfo.RealHP;
            }
        }
    }

    private void PlayMissId()
    {
        if(mMissedIds == null)
        {
            return;
        }
        for (int i = 0; i < mMissedIds.Count; i++)
        {
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(mMissedIds[i]);
            if (avatar == null || avatar.head == null)
            {
                continue;
            }
            int dir = UtilityFight.GetFlyDirection(mAttacker,avatar);
            avatar.head.PlayHurtEffect(0, dir, mDelayTime, ThrowTextType.Dodge);
        }
    }

    private void PlayCrit()
    {
        if(mIsCriticalIds == null)
        {
            return;
        }

        for(int i = 0; i < mIsCriticalIds.Count; ++i)
        {
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(mIsCriticalIds[i]);
            if (avatar == null || avatar.head == null)
            {
                continue;
            }
            int dir = UtilityFight.GetFlyDirection(mAttacker,avatar);
            int deltaHp = GetDeltaHp(mIsCriticalIds[i]);
            avatar.head.PlayHurtEffect(deltaHp, dir, mDelayTime, ThrowTextType.Critical);
        }
    }

    private void PlayImmuPhyAttIds()
    {
        if (mImmuPhyAttIds == null)
        {
            return;
        }

        for (int i = 0; i < mImmuPhyAttIds.Count; ++i)
        {
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(mImmuPhyAttIds[i]);
            if (avatar == null || avatar.head == null)
            {
                continue;
            }
            int dir = UtilityFight.GetFlyDirection(mAttacker,avatar);
            avatar.head.PlayHurtEffect(0, dir, mDelayTime, ThrowTextType.Immune);
        }
    }

    private void PlayBeattackAudio()
    {
        if(mEffect != null && mSkill != null && mSkill.SkillInfo != null && mEffect.performer == CSMainPlayerInfo.Instance.ID)
        {
            CSAudioManager.Instance.Play(true, mSkill.SkillInfo.beattackAudio);
            CSAudioManager.Instance.Play(true,3009);
        }
    }

    private int GetDeltaHp(long id)
    {
        if(mTargetEffects == null)
        {
            return 0;
        }
        for (int i = 0; i < mTargetEffects.Count; ++i)
        {
            fight.EffectedTarget target = mTargetEffects[i];
            if(target.id == id)
            {
                return target.deltaHP;
            }
        }
        return 0;
    }

    private int GetTextType(long id)
    {
        if(mIsCriticalIds != null && mIsCriticalIds.Contains(id))
        {
            return ThrowTextType.Critical;
        }
        return ThrowTextType.NormalDamage;
    }

    public void Release()
    {
        if (mTargetEffects != null)
        {
            mTargetEffects.Clear();
        }
        if (mMissedIds != null)
        {
            mMissedIds.Clear();
        }
        if (mIsCriticalIds != null)
        {
            mIsCriticalIds.Clear();
        }
        if (mImmuPhyAttIds != null)
        {
            mImmuPhyAttIds.Clear();
        }
        mAttacker = null;
        Remove();
    }

    public void Destroy()
    {
        Release();
        if (CSObjectPoolMgr.Instance != null && poolItem != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(poolItem);
        }
        poolItem = null;
    }
}
