using UnityEngine;
using System.Collections;

public class TouchData
{
    
    private CSMisc.Dot2 mCoord;
    private CSMisc.Dot2 mSkillTargetCoord;
    private CSSkill mSkill;
    private CSAvatar mLastCanAttackTarget;
    private CSAvatar mTarget;
    private int mType;
   
    public CSAvatar Owner;
    public int Direction;// 触摸摇杆使用

    public TouchData(CSAvatar avater)
    {
        this.Owner = avater;
    }
 
    public CSAvatar Target
    {
        get
        {
            return mTarget;
        }
        set
        {
            mTarget = value;
        }
    }

    public CSMisc.Dot2 SkillTargetCoord
    {
        get { return mSkillTargetCoord; }
        set { mSkillTargetCoord = value; }
    }

    public void SetSkillTargetCoord(int x, int y)
    {
        mSkillTargetCoord.x = x;
        mSkillTargetCoord.y = y;
    }

    public CSSkill Skill
    {
        get { return mSkill; }
        set { mSkill = value; }
    }

    public TABLE.SKILL GetTableSkill()
    {
        if(mSkill != null)
        {
            return mSkill.SkillInfo;
        }
        return null;
    }

    public int Type
    {
        get { return mType; }
        set
        {
            mType = value;
        }
    }

    public CSMisc.Dot2 Coord
    {
        get
        {
            if (CSScene.IsLanuchMainPlayer && CSAvatarManager.MainPlayer.TouchEvent == this)
            {
                if (Type == ETouchType.Attack && Target != null)
                {
                    return Target.OldCell.Coord;
                }
                else
                {
                    return mCoord;
                }
            }
            return mCoord;
        }
        set
        {
            mCoord = value;
            Owner.SetTouchhCoord(value);
        }
    }

    private bool IsDifferentTarget(CSAvatar target, CSAvatar newTarget)
    {
        if (target == null || newTarget == null || (target.BaseInfo != null &&
               newTarget.BaseInfo != null && target.BaseInfo.ID != newTarget.BaseInfo.ID))
        {
            return true;
        }
        return false;
    }

    public bool SetTarget(CSAvatar newTarget)
    {
        if(Owner != null && Owner.AvatarType == EAvatarType.MainPlayer)
        {
            if(IsDifferentTarget(mTarget,newTarget))
            {
                Target = newTarget;
                if(IsDifferentTarget(mLastCanAttackTarget, newTarget))
                {
                    //TODO:ddn
                    CSAvatar temp = mLastCanAttackTarget;
                    mLastCanAttackTarget = newTarget;
                    if (mLastCanAttackTarget != null && mLastCanAttackTarget.AvatarGo != null)
                    {
                        mLastCanAttackTarget.AvatarGo.OnHit(Owner);
                        return true;
                    }
                }
            }
        }
        else
        {
            Target = newTarget;
        }
        return false;
    }

    public void SetSkill(CSSkill skill)
    {
        mSkill = skill;
    }

    public void CancelSelect()
    {
        Target = null;
        mLastCanAttackTarget = null;
    }

    public void CancleSelectTarget(long id)
    {
        if (Target == null)
        {
            return;
        }
        if(id == Target.ID)
        {
            CancelSelect();
            CSAutoFightManager.Instance.IsCommonFight = false;
        }
    }

    public int GetActionOfAfterTheRun()
    {
        int montion = 3;//CSMotion.Attack

        switch (Type)
        {
            case ETouchType.Normal:// 仅仅是跑过来
            case ETouchType.Touch:
                montion = CSMotion.Stand;
                break;
            case ETouchType.WaKuang:
                montion = CSMotion.WaKuang;
                break;
            case ETouchType.GuWu:
                montion = CSMotion.GuWu;
                break;
            case ETouchType.RunOverDoSomething:
                montion = CSMotion.RunOverDoSmoething;
                break;
            case ETouchType.Attack:
                if (Skill != null)
                {
                    montion = Skill.RealAttackMontion;
                }
                break;
        }
        return montion;
    }

    public int GetActionOfterTheRunMainPlyer()
    {
        switch(Type)
        {
            case ETouchType.Normal:
            case ETouchType.Touch:
            case ETouchType.Task:
                return CSMotion.Stand;
            case ETouchType.WaKuang:
                return CSMotion.WaKuang;
            case ETouchType.GuWu:
                return CSMotion.GuWu;
            case ETouchType.RunOverDoSomething:
                return CSMotion.RunOverDoSmoething;
            case ETouchType.Attack:
            case ETouchType.AttackTerrain:
                if (Skill != null)
                {
                    return Skill.RealAttackMontion;
                }
                break;
        }
        return CSMotion.Attack;
    }

    public void Clear()
    {
        CancelSelect();
        Type = ETouchType.Normal;
        mTarget = null;
        mLastCanAttackTarget = null;
        //TODO:ddn
        CSAutoFightManager.Instance.IsCommonFight = false;
        Coord.Clear();
    }
}
