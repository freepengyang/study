

//-------------------------------------------------------------------------
//怪物行为
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CSMonsterBehavior : BehaviourProvider
{
    private CSAvatar mAvatar;
    public CSMonsterBehavior(CSAvatar avatar)
    {
        this.mAvatar = avatar;
    }
    public override bool InitializeFSM(FSMState fsm)
    {
        if (fsm.Behaviors.Count != 0) return true;
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Stand, StandStart, StandUpdate, StandEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Attack, AttackStart, AttackUpdate, AttackEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Attack2, Attack2Start, Attack2Update, Attack2End));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.BeAttack, BeAttackStart, BeAttackUpdate, BeAttackEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Dead, DeadStart, DeadUpdate, DeadEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Run, RunStart, RunUpdate, RunEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.RunToStand, RunToStandStart, RunToStandUpdate, RunToStandEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.StandToRun, StandToRunStart, StandToRunUpdate, StandToRunEnd));
        return true;
    }

    // 待机
    private void StandStart(BehaviorState last)
    {
        if (mAvatar != null)
        {
            mAvatar.SetAction(CSMotion.Stand);
        }
    }   
    private int StandUpdate()
    {
        return CSMotion.Stand;
    }
    private void StandEnd(BehaviorState next)
    {

    }
    // 攻击
    private void AttackStart(BehaviorState last)
    {
        AttackStartCommon(last);
    }
    private int AttackUpdate()
    {
        int action = AttackUpdateCommon(CSMotion.Attack);
        return action;
    }
    private void AttackEnd(BehaviorState next)
    {
    }
    // 法攻击
    private void Attack2Start(BehaviorState last)
    {
        AttackStartCommon(last);
    }
    private int Attack2Update()
    {
        int action = AttackUpdateCommon(CSMotion.Attack);
        return action;
    }
    private void Attack2End(BehaviorState next)
    {
    }

    // 被击
    private void BeAttackStart(BehaviorState last)
    {
    }
    private int BeAttackUpdate()
    {
        if (mAvatar.BaseInfo.HP <= 0)
        {
            return CSMotion.Dead;
        }
        else
        {
            if (!mAvatar.EndOfAction())
            {
                return CSMotion.Stand;
            }
        }
        return CSMotion.BeAttack;
    }
    private void BeAttackEnd(BehaviorState next)
    {

    }
    // 死亡
    private void DeadStart(BehaviorState last)
    {
       if(mAvatar != null)
        {
            mAvatar.Dead();
        }
    }
    private int DeadUpdate()
    {
        return CSMotion.Dead;
    }
    private void DeadEnd(BehaviorState next)
    {

    }
    // 跑步
    private void RunStart(BehaviorState last)
    {
        mAvatar.MoveInit();
    }
    private int RunUpdate()
    {
        if (mAvatar.TouchEvent.Type == ETouchType.Attack)
        {
            int ret = mAvatar.TouchEvent.GetActionOfAfterTheRun();
            if ((mAvatar.AvatarType == EAvatarType.Pet || mAvatar.AvatarType == EAvatarType.Monster
                || mAvatar.AvatarType == EAvatarType.ZhanHun) && ret == CSMotion.Stand)
            {
                mAvatar.FSM.SetWait(CSMotion.Stand, 0.1f, true);
                return CSMotion.Run;
            }
            else if (ret == CSMotion.Attack || ret == CSMotion.Attack2)
            {
                mAvatar.Stop(true);
            }
            return ret;
        }
        else
        {
            if (!mAvatar.IsMoving)
            {
                int ret = mAvatar.TouchEvent.GetActionOfAfterTheRun();
                if ((mAvatar.AvatarType == EAvatarType.Pet || mAvatar.AvatarType == EAvatarType.Monster
                    || mAvatar.AvatarType == EAvatarType.ZhanHun) && ret == CSMotion.Stand)
                {
                    mAvatar.FSM.SetWait(CSMotion.Stand, 0.1f, true);
                    return CSMotion.Run;
                }
                return ret;
            }
        }

        return CSMotion.Run;
    }
    private void RunEnd(BehaviorState next)
    {

    }
    private void RunToStandStart(BehaviorState last)
    {
    }

    private int RunToStandUpdate()
    {
        if (!mAvatar.EndOfAction())
        {
            return CSMotion.Stand;
        }

        return CSMotion.RunToStand;
    }

    private void RunToStandEnd(BehaviorState next)
    {
   
    }

    private void StandToRunStart(BehaviorState last)
    {

    }

    private int StandToRunUpdate()
    {
        if (!mAvatar.EndOfAction())
        {
            return CSMotion.Stand;
        }
        return CSMotion.StandToRun;
    }

    private void StandToRunEnd(BehaviorState next)
    {
    }

    private void AttackStartCommon(BehaviorState last)
    {
        CSSkill skill = mAvatar.TouchEvent.Skill;
        if (skill != null && skill.Target != null)
        {
            bool isPlayAttackAction = true;
            if (mAvatar.AvatarType == EAvatarType.Monster)
            {
                CSMonster monster = mAvatar as CSMonster;
                if (!monster.IsPlayAttackAction())
                {
                    isPlayAttackAction = false;
                }
            }
            if (isPlayAttackAction)
            {
                mAvatar.Attack(skill.Target.NewCell.LocalPosition2);
            }
            if (mAvatar.AvatarType == EAvatarType.Pet || mAvatar.AvatarType == EAvatarType.ZhanHun)
            {
                UtilityFight.PlaySkillAudio(skill.SkillInfo);
            }
            mAvatar.SkillEngine.PlayerLaunch(skill);
        }
    }
    private int AttackUpdateCommon(int motion)
    {
        if (!mAvatar.EndOfAction())
        {
            mAvatar.TouchEvent.Type = ETouchType.Normal;
            if (mAvatar.AvatarType == EAvatarType.ZhanHun)
            {
                mAvatar.FSM.SetWait(CSMotion.Stand, UtilityFight.SkillStandDelay(motion), true);
                return motion;
            }
            return CSMotion.Stand;
        }
        return motion;
    }

    private bool IsAvatarTypeActive(CSAvatar avatar)
    {
        switch(avatar.AvatarType)
        {
            case EAvatarType.Monster:
            case EAvatarType.Pet:
            case EAvatarType.ZhanHun:
            case EAvatarType.Guard:
                {
                    return true;
                }
            default:
                return false;
        }
    }

    public override void Reset()
    {

    }
}
