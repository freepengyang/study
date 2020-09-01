using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkill
{
    private CSAvatar mAvatar;
    private TABLE.SKILL mSkillInfo;
    private CSSkillEffectSystem mSkillEffectSystem = null;
    public int ID { get; set; }
    public CSAvatar Target { get; set; }

    //public SkillCD skillCD { get; set; }

    private int mRealEffectId = -1;
    private float mLastPlayLieHuoTime = 0;

    public TABLE.SKILL SkillInfo
    {
        get
        {
            return mSkillInfo;
        }
    }

    public CSSkillEffectSystem SkillEffectSystem
    {
        get
        {
            return mSkillEffectSystem;
        }
    }

    public int Range
    {
        get
        {
            if (mSkillInfo != null)
            {
                return mSkillInfo.clientRange;
            }
            return 0;
        }
    }

    public int EffectRange
    {
        get
        {
            if (mSkillInfo != null)
            {
                return mSkillInfo.effectRange;
            }
            return 0;
        }
    }

    public int SkillArea
    {
        get
        {
            if (mSkillInfo != null)
            {
                return mSkillInfo.effectArea;
            }
            return 0;
        }
    }

    public bool IsCoolOver
    {
        get { return true; }
    }

    public bool IsLaunch
    {
        get
        {
            if (!IsCanRelease)
            {
                return false;
            }
            if (Target == null || Target.IsDead)
            {
                return false;
            }
            return true;
        }
    }

    public bool IsCanRelease
    {
        get
        {
            if (mAvatar == null || mAvatar.BaseInfo == null)
            {
                return false;
            }
            if (mSkillInfo == null)
            {
                return false;
            }
            CSAvatarInfo info = mAvatar.BaseInfo;
            if (info == null)
            {
                return false;
            }
            if (info.RealHP <= 0)
            {
                return false;
            }
            if(info.MP < mSkillInfo.mpCost && CSSkillInfo.Instance.GetSkillNeedCostMp(mSkillInfo.skillGroup))
            {
                return false;
            }

            if (CSSkillInfo.Instance.IsSkillInCD(ID))
            {
                return false;
            }
            return true;

        }
    }

    public bool IsPlaying()
    {
        if(mSkillEffectSystem != null)
        {
            return mSkillEffectSystem.IsPlaying();
        }
        return false;
    }
    
    public long GetTargetID()
    {
        if(Target != null)
        {
            return Target.ID;
        }
        return 0;
    }

    /// <summary>
    /// 技能释放动作类型：1：物理攻击 2：法系攻击3：弓手攻击
    /// </summary>
    public int RealAttackMontion
    {
        get
        {
            int motion = CSMotion.Stand;
            if (SkillInfo != null)
            {
                switch (SkillInfo.action)
                {
                    case 0: motion = CSMotion.Attack; break;
                    case 1: motion = CSMotion.Attack2; break;
                    case 2: motion = CSMotion.Attack3; break;
                }
            }
            if (mAvatar.AvatarType == EAvatarType.MainPlayer)
            {
                //Debug.Log("Action Of UseSkill = " + ((CSMotion)motion).ToString());
            }
            return motion;
        }
    }

    public float GetStartPlayHurt()
    {
        if(mSkillEffectSystem != null)
        {
            return mSkillEffectSystem.StartPlayHurt();
        }
        return 0.2f;
    }

    public bool IsGongSha()
    {
        return (mRealEffectId == ESkillEffectID.GongSha);
    }

    public CSSkill(int id, CSAvatar avater)
    {
        Attach(id, avater);
    }

    public void Attach(int id, CSAvatar avater)
    {
        if (avater == null)
        {
            return;
        }
        ID = id;
        mAvatar = avater;

        if (SkillTableManager.Instance.TryGetValue(id, out mSkillInfo))
        {
            mRealEffectId = GetRealEffectId(SkillInfo.effectId);
            InitEffect();
        }
    }

    public void Update()
    {
        //if (mSkillEffectSystem != null)
        //{
        //    mSkillEffectSystem.Update();
        //}
    }

    public void Launch()
    {
        ReqUseSkill(ID, Target.ID, Target.OldCell.Coord.x, Target.OldCell.Coord.y, 0);
        PlaySkillEffect();
    }

    public void ReqUseSkill(int skillId, long targetId, int x, int y, long time)
    {
        if (mAvatar.ID != CSMainPlayerInfo.Instance.ID)
        {
            return;
        }
        Net.ReqUseSkillMessage(skillId, targetId, x, y, time);
    }

    public void Launch(int skillId, long targetId, int x, int y, long time)
    {
        ReqUseSkill(skillId, targetId, x, y, time);
        PlaySkillEffect();
    }

    private void InitEffect()
    {
        if (CSSkillEffectSystem.IsVaild(mRealEffectId))
        {
            Transform trans = (mAvatar.AvatarType == EAvatarType.MainPlayer) ?
                CSAvatarManager.MainPlayer.CacheTransform.parent : CSPreLoadingBase.Instance.SkillAnchor;

            int targetCoordX = mAvatar.TouchEvent.SkillTargetCoord.x;
            int targetCoordY = mAvatar.TouchEvent.SkillTargetCoord.y;
            if (mSkillEffectSystem != null)
            {
                mSkillEffectSystem.Attach(mAvatar, Target, mRealEffectId, targetCoordX, targetCoordY);
            }
            else
            {
                GameObject go = new GameObject(mAvatar.GetName());
                NGUITools.SetParent(trans, go);
                mSkillEffectSystem = go.AddComponent<CSSkillEffectSystem>();
                mSkillEffectSystem.Create(mAvatar, Target, trans, mRealEffectId, targetCoordX, targetCoordY);
            }
        }
    }

    public void PlaySkillEffect()
    {
        //if(mAvatar == null)
        //{
        //    return;
        //}
        if (CSConfigInfo.Instance.GetBool(ConfigOption.HideSkillEffect))
        {
            return;
        }

        UpdateEffect();
        if (mSkillEffectSystem != null && IsCanPlayEffect)
        {
            mSkillEffectSystem.Play(Target, mAvatar,mAvatar.TouchEvent.SkillTargetCoord.x, mAvatar.TouchEvent.SkillTargetCoord.y);
        }
        else
        {
            //Debug.Log("mSkillEffectSystem is null");
        }
    }

    private void UpdateEffect()
    {
        if (mAvatar.AvatarType == EAvatarType.MainPlayer || mAvatar.AvatarType == EAvatarType.Player)
        {
            if (SkillInfo == null)
            {
                return;
            }
            mRealEffectId = GetRealEffectId(SkillInfo.effectId);
            InitEffect();
        }
    }

    public bool IsCanPlayEffect
    {
        get
        {
            if (SkillInfo == null)
            {
                return false;
            }
            if (SkillInfo.skillGroup == (int)(ESkillGroup.LieHuo))
            {
                return false;
            }
            if (mRealEffectId == 0 && SkillInfo.effectId == 0)
            {
                return false;
            }
            return true;
        }
    }

    public bool IsInsteadOfSkill(int skillGroup, int specialBuff)
    {
        if (mAvatar.AvatarType == EAvatarType.MainPlayer || mAvatar.AvatarType == EAvatarType.Player)
        {
            if (mAvatar.BaseInfo != null && mAvatar.BaseInfo.BuffInfo != null && SkillInfo != null)
            {
                return ((SkillInfo.skillGroup != skillGroup) &&
                    mAvatar.BaseInfo.BuffInfo.IsHasSpecialBuff(specialBuff) &&
                    UtilityFight.IsSkillWorkOnLieHuo(SkillInfo.skillGroup));
            }
        }
        return false;
    }


    private int GetRealEffectId(int groupId)
    {
        if (IsInsteadOfSkill(ESkillGroup.LieHuo, ESpecialBuff.LieYanZhan))
        {
            return ESkillEffectID.LieYanZhan;
        }
        else if(IsInsteadOfSkill(ESkillGroup.LieHuo,ESpecialBuff.LieHuo) && (Time.time > (mLastPlayLieHuoTime + 0.5f)))
        {
            mLastPlayLieHuoTime = Time.time;
            return ESkillEffectID.LieHuo;
        }
        else if (IsInsteadOfSkill(ESkillGroup.GongSha, ESpecialBuff.GongSha))
        {
            return ESkillEffectID.GongSha;
        }
        return groupId;
    }

    public void RemoveAttach()
    {
        if(mSkillEffectSystem != null)
        {
            mSkillEffectSystem.RemoveAttach();
        }
    }

    public void Stop()
    {
       if(mSkillEffectSystem != null)
        {
            mSkillEffectSystem.Stop();
        }
    }

    public void Release()
    {
        if(mSkillEffectSystem != null)
        {
            mSkillEffectSystem.Release();
        }
        mAvatar = null;
        Target = null;
    }

}



