using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPlayerBehavior : BehaviourProvider
{
    private CSAvatar mAvatar;
    public CSPlayerBehavior(CSAvatar avatar)
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
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.WaKuang, WaKuangStart, WaKuangUpdate, WaKuangEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.RunOverDoSmoething, RunOverDoSmoethingStart, RunOverDoSmoethingUpdate, RunOverDoSmoethingEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Run, RunStart, RunUpdate, RunEnd));
        return true;
    }

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

    private void AttackStart(BehaviorState last)
    {
        AttackStartCommon(last);
    }

    private int AttackUpdate()
    {
        int action = AttackUpdateCommon(CSMotion.Attack);
        if (action != CSMotion.Static) return action;
        return CSMotion.Attack;
    }

    private void AttackEnd(BehaviorState next)
    {
        AttackEndCommon(next);
    }

    private void Attack2Start(BehaviorState last)
    {
        AttackStartCommon(last);
    }

    private int Attack2Update()
    {
        int action = AttackUpdateCommon(CSMotion.Attack2);
        if (action != CSMotion.Static) return action;
        return CSMotion.Attack2;
    }

    private void Attack2End(BehaviorState next)
    {
        AttackEndCommon(next);
    }

    private void BeAttackStart(BehaviorState last)
    {
    }

    private int BeAttackUpdate()
    {
        return CSMotion.BeAttack;
    }

    private void BeAttackEnd(BehaviorState next)
    {
    }
    private void DeadStart(BehaviorState last)
    {
    }

    private int DeadUpdate()
    {
        return CSMotion.Dead;
    }

    private void DeadEnd(BehaviorState next)
    {
    }

    private void WaKuangStart(BehaviorState last)
    {
    }

    private int WaKuangUpdate()
    {
        return CSMotion.WaKuang;
    }

    private void WaKuangEnd(BehaviorState next)
    {
    }

    private void RunOverDoSmoethingStart(BehaviorState last)
    {
        if (onRunOverDoSmoethingStart != null)
        {
            onRunOverDoSmoethingStart();
            onRunOverDoSmoethingStart = null;
        }
    }

    private int RunOverDoSmoethingUpdate()
    {
        return CSMotion.RunOverDoSmoething;
    }

    private void RunOverDoSmoethingEnd(BehaviorState next)
    {
        if (onRunOverDoSmoethingEnd != null)
        {
            onRunOverDoSmoethingEnd();
            onRunOverDoSmoethingEnd = null;
        }
    }

    private void RunStart(BehaviorState last)
    {
        if (mAvatar != null)
        {
            mAvatar.MoveInit();
        }
    }

    private int RunUpdate()
    {
        if (mAvatar != null)
        {
            if (!mAvatar.IsMoving)
            {
                int ret = mAvatar.TouchEvent.GetActionOfAfterTheRun();
                if (ret == CSMotion.Stand)
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

    private void AttackStartCommon(BehaviorState last)
    {
        if (mAvatar != null)
        {
            if (mAvatar.TouchEvent != null)
            {
                CSSkill skill = mAvatar.TouchEvent.Skill;
                if (skill != null && skill.SkillInfo != null)
                {
                    mAvatar.IsReplaceEquip = true;

                    CSCell cell = null;
                    if (UtilityFight.IsFlagSkill(skill.SkillInfo.skillGroup))
                    {
                        cell = CSMesh.Instance.getCell(mAvatar.TouchEvent.SkillTargetCoord.x, mAvatar.TouchEvent.SkillTargetCoord.y);
                    }
                    else if(mAvatar.TouchEvent.Target != null)
                    {
                        cell = mAvatar.TouchEvent.Target.OldCell;
                    }
                    if(cell == null)
                    {
                        return;
                    }
                    switch(skill.SkillInfo.action)
                    {
                        case 0:
                            {
                                mAvatar.Attack(cell.LocalPosition2);
                            }
                            break;
                        case 1:
                            {
                                mAvatar.Attack2(cell.LocalPosition2);
                            }
                            break;
                    }
                    mAvatar.SkillEngine.PlayerLaunch(skill);
                }
            }
        }
    }

    private int AttackUpdateCommon(int motion)
    {
        if (mAvatar != null)
        {
            CSSkill skill = mAvatar.TouchEvent.Skill;
            if (skill != null)
            {
                if (!mAvatar.IsAttackPlaying())//技能动作播放完
                {
                    mAvatar.TouchEvent.Type = ETouchType.Normal;
                    mAvatar.FSM.SetWait(CSMotion.Stand, UtilityFight.SkillStandDelay(motion), true);
                    return UpdateBehaviour(CSMotion.Static);
                }
            }
        }
        return UpdateBehaviour(CSMotion.Static);
    }

    private void AttackEndCommon(BehaviorState next)
    {
    }

    private int UpdateBehaviour(int motion)
    {
        return motion;
    }

    public override void Reset()
    {
        
    }
}
