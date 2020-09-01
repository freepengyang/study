using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillLaunchSystem : CSInfo<CSSkillLaunchSystem>
{
    public void OnCommonAttack()
    {
        CSAvatar target = null;
        if(TryGetTarget(ref target))
        {
            //CSAvatarManager.MainPlayer.TouchEvent.Type = ETouchType.Attack;
           CSAvatarManager.MainPlayer.TouchEvent.SetSkill(null);
            CSAutoFightManager.Instance.IsCommonFight = true;
        }
    }

    public void OnAutoFight()
    {
        //CSAvatarManager.MainPlayer.TouchEvent.SetSkill(null);
        CSAutoFightManager.Instance.IsAutoFight = !CSAutoFightManager.Instance.IsAutoFight;
        CSAutoFightManager.Instance.IsCommonFight = false;
    }

    //TODO:ddn
    public void LaunchSkill(int skillId)
    {
        TABLE.SKILL tblSkill = CSSkillInfo.Instance.GetValidSkillItem(skillId, false);
        if (null == tblSkill)
        {
            return;
        }

        if (IsCancle(tblSkill.skillGroup))
        {
            return;
        }

        switch (tblSkill.launchType)
        {
            case ESkillLaunchType.InSitu:
                {
                    CSAutoFightManager.Instance.LaunchSkillId = tblSkill.id;
                }
                break;
            case ESkillLaunchType.InSituWithoutActtion:
                {
                    LaunchSkillInSituWithoutActtion(tblSkill);
                }
                break;
            default:
                {
                    LaunchNormalSkill(tblSkill);
                }
                break;
        }
    }

    private void LaunchNormalSkill(TABLE.SKILL tblSkill)
    {
        CSAvatar target = null;
        if (TryGetTarget(ref target, tblSkill))
        {
            if (tblSkill.clientTargetType == (int)ESkillTargetType.ZhanHunInteadOfEnemy)
            {
                bool isInRange = CSAvatarManager.Instance.IsInZhanHunAttackRange(tblSkill.effectArea,
                                tblSkill.clientRange, tblSkill.effectRange, target.OldCell.Coord);
                if (!isInRange)
                {
                    UtilityTips.ShowTips(1979);
                    return;
                }

                //if (!CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange,
                //    tblSkill.effectRange, CSAvatarManager.MainPlayer.OldCell.Coord, target.OldCell.Coord))
                //{
                //    FNDebug.Log("<color=#ff0000> avatar is not in attack range</color>");
                //    return;
                //}
            }
            CSAutoFightManager.Instance.LaunchSkillId = tblSkill.id;
        }
    }

    public void LaunchSkillAuto(TABLE.SKILL tblSkill)
    {
        if(tblSkill == null)
        {
            return;
        }
        LaunchSkillWithoutAction(tblSkill.id);

        //if (tblSkill.skillGroup == (int)ESkillGroup.LieHuo)
        //{
        //    LaunchSkillWithoutAction(tblSkill.id);
        //}
        //else
        //{
        //    CSAutoFightManager.Instance.LaunchSkillId = tblSkill.id;
        //}
    }

    private void LaunchSkillInSituWithoutActtion(TABLE.SKILL tblSkill)
    {
        if (tblSkill == null)
        {
            return;
        }
        if (tblSkill.skillGroup == (int)ESkillGroup.LieHuo)
        {
            if(UtilityFight.InAttackRange(1))
            {
                LaunchSkillWithoutAction(tblSkill.id);
            }
        }
        else
        {
            LaunchSpecialSkill(tblSkill);
        }
    }

    public void LaunchSkillWithoutAction(int skillId)
    {
        CSMisc.Dot2 tragetDot = CSMisc.Dot2.Zero;
        CSCell cell =CSAvatarManager.MainPlayer.NewCell;
        if (cell != null)
        {
            tragetDot = cell.Coord;
        }
        Net.ReqUseSkillMessage(skillId, 0, tragetDot.x, tragetDot.y, 0);
    }

    public void LaunchSkilNoDetectTarget()
    {
        //LaunchSpecialSkill(tblSkill);
    }


    public void LaunchSpecialSkill(TABLE.SKILL tblSkill)
    {
        CSCharacter character =CSAvatarManager.MainPlayer;
        CSSkill skill = character.SkillEngine.AddSkill(tblSkill.id);
        if (skill.IsCanRelease)
        {
            skill.Target = null;
            
            character.TouchEvent.Type = ETouchType.Attack;
            character.TouchEvent.Skill = skill;

            //character.TouchEvent.SetTarget(null);
            CSMisc.Dot2 tragetDot = GetTargetDot(tblSkill);
            skill.Launch(tblSkill.id,0, tragetDot.x, tragetDot.y,0);
        }
    }

    public void LaunchUpgradeSkill(int skillId)
    {
        TABLE.SKILL tblSkill = null;
        if (SkillTableManager.Instance.TryGetValue(skillId, out tblSkill))
        {
            if (CSMainPlayerInfo.Instance.MP >= tblSkill.mpCost)
            {
                LaunchSkill(skillId);
            }
        }
    }

    private bool IsCancle(int skillGroup)
    {
        if(UtilityFight.IsFlagSkill(skillGroup) || skillGroup == (int)ESkillGroup.YeMan)
        {
            return CSSkillFlagManager.Instance.IsCancle();
        }
        return false;
    }


    //TODO:ddn 
    private CSMisc.Dot2 GetTargetDot(TABLE.SKILL tblSkill)
    {
        CSMisc.Dot2 tragetDot = CSMisc.Dot2.Zero;
        if (tblSkill.skillGroup == (int)ESkillGroup.YeMan)
        {
            CSCharacter character =CSAvatarManager.MainPlayer;
            int dir = character.GetDirection();
            CSMisc.Dot2 dot = character.OldCell.Coord;
            tragetDot = dot + (CSMisc.dirMove[dir] * tblSkill.effectRange);
            CSCell targetCell = CSMesh.Instance.getCell(tragetDot.x, tragetDot.y);
            character.Attack(targetCell.LocalPosition2);
        }
        else if(tblSkill.skillGroup == (int)ESkillGroup.Healing)
        {
            CSCell cell =CSAvatarManager.MainPlayer.NewCell;
            if (cell != null)
            {
                tragetDot = cell.Coord;
            }
        }
        return tragetDot;
    }

    private bool TryGetTarget(ref CSAvatar target, TABLE.SKILL tblSkill = null)
    {
        CSCharacter character =CSAvatarManager.MainPlayer;
        if (character.SkillEngine.IsPublicCDing)
        {
            FNDebug.Log("character is in public cd time");
            return false;
        }

        target =CSAvatarManager.MainPlayer.TouchEvent.Target;
        if(target != null && target.AvatarType == EAvatarType.NPC)
        {
            target = null;
        }
        if (target == null)
        {
            if (tblSkill != null)
            {
                target = CSAvatarManager.Instance.GetSelectTargetByType(false,tblSkill.clientTargetType);
            }
            else
            {
                target = CSAvatarManager.Instance.GetNearestAttackTarget();
            }
        }
        if(target == null)
        {
            UtilityTips.ShowTips(527);
        }
        return (target != null);
    }

    public override void Dispose()
    {
        
    }
}
