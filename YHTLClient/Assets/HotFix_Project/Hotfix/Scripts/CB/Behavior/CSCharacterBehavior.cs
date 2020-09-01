

//-------------------------------------------------------------------------
//角色行为
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;

public class CSCharacterBehavior : BehaviourProvider
{
    private CSAvatar mAvatar;
    public CSCharacterBehavior(CSAvatar avatar)
    {
        mAvatar = avatar;
    }

    public override bool InitializeFSM(FSMState fsm)
    {
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Stand, StandStart, StandUpdate, StandEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Attack, AttackStart, AttackUpdate, AttackEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Attack2, Attack2Start, Attack2Update, Attack2End));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.BeAttack, BeAttackStart, BeAttackUpdate, BeAttackEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Dead, DeadStart, DeadUpdate, DeadEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.WaKuang, WaKuangStart, WaKuangUpdate, WaKuangEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.GuWu, GuWuStart, GuWuUpdate, GuWuEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.RunOverDoSmoething, RunOverDoSmoethingStart, RunOverDoSmoethingUpdate, RunOverDoSmoethingEnd));
        fsm.InitialAddBehavior(new BehaviorState(CSMotion.Run, RunStart, RunUpdate, RunEnd));
        return true;
    }

    // 待机
    private void StandStart(BehaviorState last)
    {
        //CSCharacter chr = mAvatar as CSCharacter;
        //TODO:ddn
        //if (chr == null || chr.Info.IsMining || chr.MoveState == EMoveState.Controlled) return;

        if (mAvatar == null)
        {
            FNDebug.Log("======>StandStart: mAvatar is null");
            return;
        }
        if (mAvatar.IsBeControl)
        {
            return;
        }
        mAvatar.SetAction(CSMotion.Stand);
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
        if (action != CSMotion.Static) return action;

        return CSMotion.Attack;
    }

    private void AttackEnd(BehaviorState next)
    {
        AttackEndCommon(next);
    }

    // 法攻
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

    private void Attack2End( BehaviorState next)
    {
        AttackEndCommon(next);
    }

    private void AttackStartCommon(BehaviorState last)
    { 
        if (mAvatar.TouchEvent != null)
        {
            CSSkill skill = mAvatar.TouchEvent.Skill;
            if (skill != null)
            {
                //TODO:ddn
                //mAvatar.SkillEngine.ResetData(skill.ID);
                TABLE.SKILL tblSkill = skill.SkillInfo;
                if (skill.IsLaunch && (tblSkill != null))
                {
                    bool isStop = false;
                    CSCell targetCell = null;
                    CSAvatar target = null;
                    if (skill.Target != null && skill.Target.AvatarType == EAvatarType.MainPlayer)
                    {
                        isStop = true;
                        targetCell = mAvatar.OldCell;
                        if (UtilityFight.IsFlagSkill(tblSkill.skillGroup))
                        {
                            targetCell = CSSkillFlagManager.Instance.GetTargetCell(targetCell);
                        }
                        if(tblSkill.clientTargetType == ESkillTargetType.ZhanHunInteadOfEnemy)
                        {
                            bool isInRange = CSAvatarManager.Instance.IsInZhanHunAttackRange(tblSkill.effectArea,
                                tblSkill.clientRange,tblSkill.effectRange, target.OldCell.Coord);
                            if(!isInRange)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        target = mAvatar.TouchEvent.Target;
                        if (target != null)
                        {
                            targetCell = (tblSkill.effectArea == ESkillHurtEffectType.FrontLine) ? target.ServerCell:target.OldCell;
                            if(tblSkill.effectArea == ESkillHurtEffectType.FrontLine)
                            {
                                isStop = CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange, tblSkill.effectRange, mAvatar.OldCell.Coord, target.ServerCell.Coord);
                            }
                            else
                            {
                                isStop = CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange, tblSkill.effectRange, mAvatar.OldCell.Coord, targetCell.Coord);
                            }
                        }
                    }
                    if(targetCell == null)
                    {
                        return;
                    }

                    if (isStop)
                    {
                        mAvatar.DetectForceSendMoveRequest();//有些单个不发移动消息，导致主角的位置和服务器那边主角的位置不一致，引起技能打空
                        mAvatar.Stop();
                        mAvatar.IsReplaceEquip = true;
                        switch (skill.SkillInfo.action)
                        {
                            case 0:
                                mAvatar.Attack(targetCell.LocalPosition2);
                                break;
                            case 1:
                                mAvatar.Attack2(targetCell.LocalPosition2);
                                break;
                        }
                        mAvatar.SkillEngine.LaunchSkill(skill);

                        if(skill.IsGongSha())
                        {
                            SkillTableManager.Instance.TryGetValue(ESpecialSkillID.GongSha, out tblSkill);
                        }
                        UtilityFight.PlaySkillAudio(tblSkill, true);
                        //if (skill.Target != null && skill.Target.IsCanBeAttack) chr.LastAttackID = skill.Target.BaseInfo.ID;
                    }
                    else
                    {
                        mAvatar.TowardsTargetAttack(targetCell.Coord);
                    }
                }
            }
        }
    }

    private int AttackUpdateCommon(int motion)
    {
        if (mAvatar != null)
        {
            CSSkill skill = mAvatar.TouchEvent.Skill;

            //TODO:ddn 
            if (skill != null && !mAvatar.EndOfAction())
            {
                mAvatar.TouchEvent.Type = ETouchType.Normal;

                if (mAvatar.TouchMove != EControlState.Idle)
                {
                    mAvatar.FSM.SetWait(CSMotion.Stand, 0, true);
                }
                else
                {
                    mAvatar.FSM.SetWait(CSMotion.Stand, UtilityFight.SkillStandDelay(motion), true);
                }
            }
            return UpdateBehaviour(CSMotion.Static);
            #region begin

            //TODO:ddn
            //CSCharacter chr = mAvatar as CSCharacter;
            //CSSkill skill = chr.TouchEvent.Skill;

            //if (skill != null)
            //{
            //    if (skill[ESpecialSkillGroupType.ChongFeng] ||
            //        skill[ESpecialSkillGroupType.QiangHuaChongFeng] ||
            //        skill[ESpecialSkillGroupType.XuanFengZhan])
            //    {
            //        return UpdateBehaviour(CSMotion.Static);
            //    }
            //    if (!chr.EndOfAction())
            //    {
            //        if (chr.SkillEffect != null)
            //        {
            //            CSScene.Sington.ResSkillEffectMessage(chr.SkillEffect);
            //            chr.SkillEffect = null;
            //        }

            //        if (chr.AutoCombatChange)
            //        {
            //            chr.AutoCombatChange = false;
            //            chr.TouchEvent.Type = TouchType.Normal;
            //            chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid), true);
            //            return UpdateBehaviour(CSMotion.Static);
            //        }
            //        if (skill.Target == null || skill.Target.isServerDead)
            //        {
            //            chr.TouchEvent.Type = TouchType.Normal;
            //            chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid), true);
            //            return UpdateBehaviour(CSMotion.Static);
            //        }

            //        if (skill.SkillInfo.targetType == (uint)SkillTargetType.SelfAroundBit)
            //        {
            //            ISFAvater pet = CSScene.Sington.getPet(chr.BaseInfo.ID,false);
            //            if (pet != null)
            //            {
            //                chr.TouchEvent.Type = TouchType.Normal;
            //                chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid), true);
            //                chr.TouchEvent.SetTarget(null);
            //                return UpdateBehaviour(CSMotion.Static);
            //            }
            //        }
            //        if (skill.SkillInfo.targetType == (uint)SkillTargetType.Self 
            //            || skill.SkillInfo.targetType == (uint)SkillTargetType.Friend)
            //        {
            //            chr.TouchEvent.Type = TouchType.Normal;
            //            chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid), true);
            //            chr.TouchEvent.SetTarget(null);
            //            return UpdateBehaviour(CSMotion.Static);
            //        }

            //        float f2 = skill.CoolTime *0.001f - (Time.time - skill.TemCoolTime);
            //        skill.isCoolOver = f2 <= 0;//有一帧的误差

            //        if(!CSAutoFightMgr.Instance.IsBegin)
            //        {
            //            chr.TouchEvent.Type = TouchType.Normal;
            //            chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid),true);
            //            return UpdateBehaviour(CSMotion.Static);
            //        }
            //        else
            //        {
            //            if (skill.isLaunch)
            //            {
            //                chr.TouchEvent.Type = TouchType.Normal;
            //                chr.FSM.SetWait(CSMotion.Stand, CSMisc.SkillStandDelay(skill.SkillInfo.sid), true);
            //                return UpdateBehaviour(CSMotion.Static);
            //            }
            //        }
            //    }
            //}
            #endregion
        }
        return (CSMotion.Static);  
    }

    private int UpdateBehaviour(int motion)
    {
        return motion;
    }

    private void AttackEndCommon(BehaviorState next)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Reset);
        if(mAvatar != null)
        {
            CSCharacter character = mAvatar as CSCharacter;
            if (character != null)
            {
                character.IsDetectContineMove = true;
            }
        }
    }

    // 被击
    public void BeAttackStart(BehaviorState last)
    {
    }

    private int BeAttackUpdate()
    {
        return CSMotion.BeAttack;
    }
    private void BeAttackEnd(BehaviorState next)
    {
    }
    // 死亡
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

    // 挖矿
    private void WaKuangStart(BehaviorState last)
    {
        //TODO:ddn
        //if(!CSScene.IsLanuchMainPlayer)return;
        //CSCharacter chr = avater as CSCharacter;
        //if (chr == null) return;
        //CSCell cell = CSScene.Sington.Mesh.getCell(chr.TouchEvent.Coord.x,chr.TouchEvent.Coord.y);
        //if (cell != null)
        //{
        //    CSMisc.Dot2 d = chr.OldCell.Coord - cell.Coord;
        //    d = d.Abs();
        //    if (d.x <= 1 && d.y <= 1)
        //    {
        //        chr.AttackLoop(cell.LocalPosition2);
        //        //CSMine.StartCurMine();
        //        //Net.ReqStartMiningMessage();
        //        CSPathFinderManager.Instance.ClearFindPathWordAndFeixie();
        //        Net.ReqStartWaKuangMessage();
        //        chr.Info.IsMining = true;

        //        //chr.Info.IsPrisonMining = true;
        //        UIManager.Instance.CreatePanel<UIProcessBarTip>(true, null, true,(f) => {
        //            (f as UIProcessBarTip).ShowRepeating(5, "citan_wz23", ()=> { Net.ReqWakuangMessage(); });
        //        });
        //        chr.ReplaceEquip();
        //    }
        //    else
        //    {
        //        Utility.ShowRedTips(105004);
        //    }

        //    if (CSAutoFightMgr.Instance != null) CSAutoFightMgr.Instance.IsBegin = false;
        //}
    }
    private int WaKuangUpdate()
    {
        return CSMotion.WaKuang;
    }
    private void WaKuangEnd(BehaviorState next)
    {
        //CSMine.StopCurMine();

        //TODO:ddn
        //UIManager.Instance.ClosePanel<UIProcessBarTip>();
        //if (Utility.IsInPrison())
        //    Net.ReqStopWaKuangMessage();
        //CSCharactar chr = avater as CSCharactar;
        //if (chr != null)
        //{
        //    chr.Info.IsMining = false;
        //    //chr.Info.IsPrisonMining = false;
        //    chr.ReplaceEquip();
        //}
            
    }

    // 鼓舞
    private void GuWuStart(BehaviorState last)
    {
        //if (!CSScene.IsLanuchMainPlayer) return;
        //CSCharacter chr = avater as CSCharacter;
        //if (chr == null) return;
        //CSCell cell = CSScene.Sington.Mesh.getCell(chr.TouchEvent.Coord.x, chr.TouchEvent.Coord.y);
        //if (cell != null)
        //{
        //    chr.Attack(cell.LocalPosition2);
        //}
    }
    private int GuWuUpdate()
    {
        //CSAvatar avater = a as CSAvatar;
        //if (!avater.EndOfAction()) return CSMotion.Stand;
        return CSMotion.GuWu;
    }
    private void GuWuEnd(BehaviorState next)
    {
        
    }

    // 跑过去做一些事情
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

    // 跑步
    private void RunStart(BehaviorState last)
    {
        CSCharacter character = mAvatar as CSCharacter;
        if (character != null)
        {
            if(character.IsHasMoveToServerPos())
            {
                bool isStop = character.SkillEngine.DetectSkillRelease();
                if(isStop)
                {
                    character.Stop();
                }
                else
                {
                    character.MoveInit();
                }
            }
        }
    }
    private int RunUpdate()
    {
        CSCharacter chr = mAvatar as CSCharacter;

        if (chr != null)
        {
            if (chr.IsHasMoveToServerPos())
            {
                if (!chr.IsMoving)
                {
                    int action = chr.TouchEvent.GetActionOfterTheRunMainPlyer();
                    return UpdateBehaviour(action);
                }
            }
        }

        return CSMotion.Run;
    }
    private void RunEnd(BehaviorState next)
    {

    }

    // 重置
    public override void Reset()
    {

    }
} 