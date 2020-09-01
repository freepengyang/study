using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillEngine
{
    public List<CSSkill> Skills = null;
    private CSAvatar mAvatar;   //SkillEngine owner
    private CSSkill mLastUseSkill;

    public bool IsPublicCDing
    {
        get { return false; }
    }

    public CSSkill LastUseSkill
    {
        get
        {
            return mLastUseSkill;
        }
    }

    public CSSkillEngine(CSAvatar avatar)
    {
        mAvatar = avatar;
        Skills = new List<CSSkill>();
    }

    public void Init()
    {
        //CSPlayerInfo player = mAvatar.BaseInfo as CSPlayerInfo;
        //if(player != null)
        //{
        //    var cur = player.SkillShortcutInfos.GetEnumerator();
        //    while (cur.MoveNext())
        //    {
        //        if (cur.Current.Value > 40000000) continue;
        //        AddSkill(cur.Current.Value);
        //    }
        //}
    }


    public void LaunchSkill(CSSkill skill)
    {
        if(mAvatar == null || skill == null)
        {
            return;
        }
        skill.Launch();
    }


    public CSSkill GetSkill(int skillID)
    {
        for (int i = 0; i < Skills.Count; ++i)
        {
            CSSkill skill = Skills[i];
            if (skill.ID == skillID)
            {
                if(!skill.IsPlaying())
                {
                    return skill;
                }
            }
        }
        return null;
    }

    public CSSkill GetSkillById(int skillId)
    {
        for (int i = 0; i < Skills.Count; ++i)
        {
            CSSkill skill = Skills[i];
            if (skill.ID == skillId)
            {
                return skill;
            }
        }
        return null;
    }

    public CSSkill AddSkillById(int skillID)
    {
        CSSkill skill = GetSkillById(skillID);
        if (skill == null)
        {
            if (mAvatar.AvatarType == EAvatarType.MainPlayer)
            {
                skill = new CSSkill(skillID, mAvatar);
            }
            else
            {
                skill = CSSkillPool.Instance.PopSkill(skillID, mAvatar);
                skill.Attach(skillID, mAvatar);
            }
            Skills.Add(skill);
        }
        return skill;
    }

    public CSSkill AddSkill(int skillID)
    {
        CSSkill skill = GetSkill(skillID);
        if (skill == null)
        {
            if (mAvatar.AvatarType == EAvatarType.MainPlayer)
            {
                skill = new CSSkill(skillID, mAvatar);
            }
            else
            {
                skill = CSSkillPool.Instance.PopSkill(skillID, mAvatar);
                skill.Attach(skillID, mAvatar);
            }
            Skills.Add(skill);
        }
        return skill;
    }

    public void Update()
    {
        for(int i = 0,iMax = Skills.Count; i < iMax; ++i)
        {
            Skills[i].Update();
        }
    }

    public bool DetectSkillRelease(CSSkill skill = null)
    {
        CSCharacter chr = mAvatar as CSCharacter;
        if (chr == null)
        {
            return false;
        }
        if (chr.TouchEvent.Type != ETouchType.Attack)
        {
            return false;
        }
        if (!chr.IsHasMoveToServerPos())
        {
            return false;
        }
        skill = (skill != null) ? skill : chr.TouchEvent.Skill;
        CSMisc.Dot2 targetDot;
        if (chr.TouchEvent.Type == ETouchType.Attack)
        {
            if (skill != null && skill.Target != null && skill.Target.AvatarType == EAvatarType.MainPlayer)
            {
                return true;
            }

            if (chr.TouchEvent.Target == null || chr.TouchEvent.Target.OldCell == null)
            {
                return false;
            }
            if (chr.TouchEvent.Target.IsServerDead)
            {
                return false;
            }

            if(skill != null && skill.SkillInfo != null && skill.SkillInfo.effectArea == ESkillHurtEffectType.FrontLine)
            {
                targetDot = chr.TouchEvent.Target.ServerCell.Coord;
            }
            else
            {
                targetDot = chr.TouchEvent.Target.OldCell.Coord;
            }

            if (chr.TouchEvent.Target.AvatarType != EAvatarType.MainPlayer && chr.OldCell.Coord.Equal(targetDot))
            {
                return false;
            }
        }
        else
        {
            targetDot = chr.TouchEvent.Coord;
        }
        //if (chr.TouchEvent.Target.OldCell != null && !CSScene.Sington.MainPlayer.IsSeparate(chr.TouchEvent.Target.OldCell.Coord))
        //{
        //    chr.TouchEvent.Coord = targetDot;
        //}

        //skill = (skill != null) ? skill : chr.TouchEvent.Skill;
        TABLE.SKILL tblSkill = skill.SkillInfo;
        if (skill == null /*|| !skill.isLaunch*/ || tblSkill == null)
        {
            return false;
        }
        if (chr.OldCell != null && CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange, tblSkill.effectRange, chr.OldCell.Coord, targetDot))
        {
            return true;
        }
        return false;
    }

    public bool PlayerLaunch(CSSkill skill)
    {
        if (skill == null || mAvatar == null)
        {
            return false;
        }
        SetLastUseSkill(skill);
        if(mAvatar.IsAttackPlaying() || mAvatar.AvatarType == EAvatarType.Monster)
        {
            skill.PlaySkillEffect();
            return true;
        }
        return false;
    }


    public bool LaunchYeMan(CSSkill skill)
    {
        if (skill == null || mAvatar == null)
        {
            return false;
        }
        SetLastUseSkill(skill);
        skill.PlaySkillEffect();
        return true;
    }


    public void DoubleHit(int skillId,CSAvatar target)
    {
        CSSkill skill = AddSkill(skillId);
        if(skill != null)
        {
            skill.Target = target;
        }
        PlayerLaunch(skill);
    }

    public void SetLastUseSkill(CSSkill skill)
    {
        if(skill != mLastUseSkill)
        {
            if ((mLastUseSkill != null) && (mAvatar.AvatarType != EAvatarType.MainPlayer))
            {
                mLastUseSkill.RemoveAttach();
                CSSkillPool.Instance.PushSkill(mLastUseSkill);
                Skills.Remove(mLastUseSkill);
            }
            mLastUseSkill = skill;
        }
    }

    public void StopAll()
    {
        for (int i = 0; i < Skills.Count; ++i)
        {
            Skills[i].Stop();
        }
    }

    public void Release()
    {
        for (int i = 0; i < Skills.Count; ++i)
        {
            Skills[i].Release();
        }
        Skills.Clear();
    }

    public void Destroy()
    {
        StopAll();
        if(mAvatar.AvatarType != EAvatarType.MainPlayer)
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                CSSkill skill = Skills[i];
                if(skill != null)
                {
                    skill.RemoveAttach();
                    CSSkillPool.Instance.PushSkill(skill);
                }
            }
            Skills.Clear();
        }
    }



}
