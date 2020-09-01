using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFight
{
    static Dictionary<CSMotion, int> PetFps = new Dictionary<CSMotion, int>
    {


    };

    static Dictionary<CSMotion, int> MonsterFps = new Dictionary<CSMotion, int>
    {


    };

    static Dictionary<CSMotion, int> PlayerFps = new Dictionary<CSMotion, int>
    {


    };
    static float[] mDirThresholds;
    static float mDirOffset;
    public static void InitDirThresholds()
    {
        if (SundryTableManager.Instance.ContainsKey(200))
        {
            string[] splitStr = (SundryTableManager.Instance.array.gItem.id2offset[200].Value as TABLE.SUNDRY).effect.Split('#');
            mDirThresholds = new float[splitStr.Length];
            float value;
            for (int i = 0; i < mDirThresholds.Length; i++)
            {
                if (float.TryParse(splitStr[i], out value))
                {
                    mDirThresholds[i] = value;
                }
            }

            if (mDirThresholds.Length > 0)
            {
                mDirOffset = 90f + (360f - mDirThresholds[0]) / 2f;
            }
        }
    }

    public static int GetDirection(Vector3 offsetPos)
    {
        if (mDirThresholds == null || offsetPos == Vector3.zero || offsetPos.magnitude < 1)
        {
            return CSDirection.None;
        }

        float angle = Mathf.Atan2(offsetPos.y, offsetPos.x) * Mathf.Rad2Deg;

        angle -= mDirOffset;
        if (angle < 0)
        {
            angle += 360f;
        }

        for (int i = 0; i < mDirThresholds.Length; i++)
        {
            if (angle >= mDirThresholds[i])
            {
                return i;
            }
        }
        return CSDirection.None;
    }

    public static int GetMouseDirection(Vector3 target, Vector3 original)
    {
        return GetDirection(target - original);
    }

    public static float SkillStandDelay(int motion)
    {
        return 2.0f;
    }

    public static void PlaySkillAudio(TABLE.SKILL tblSkill, bool isMainPlayer = false)
    {
        if (CSAudioMgr.Instance != null)
        {
            if(tblSkill.audioMan > 0)
            {
                if(CSMainPlayerInfo.Instance.Sex == ESex.WoMan)
                {
                    CSAudioManager.Instance.Play(isMainPlayer, tblSkill.audio);
                }
                else
                {
                    CSAudioManager.Instance.Play(isMainPlayer, tblSkill.audioMan);
                }
            }
            else
            {
                CSAudioManager.Instance.Play(isMainPlayer, tblSkill.audio);
            }
        }
    }

    public static int GetFlyDirection(CSAvatar attacker, CSAvatar target)
    {
        int direction = EFlyDirection.Right;
        if (attacker == null || target == null || attacker.OldCell == null || target.OldCell == null)
        {
            return direction;
        }
        direction = (target.OldCell.Coord.x > attacker.OldCell.Coord.x) ? EFlyDirection.Right : EFlyDirection.Left;
        return direction;
    }


    public static bool IsDisplacementSkill(int skillGroup)
    {
        switch(skillGroup)
        {
            case ESkillGroup.YeMan:
            case ESkillGroup.KangJuHuoHuan:
            case ESkillGroup.QiBoGong:
                return true;
            default:
                return false;
        }
    }

    public static bool IsSkillWorkOnTarget(int type)
    {
        switch(type)
        {
            case ESkillHurtEffectType.Single:
            case ESkillHurtEffectType.Circle:
            case ESkillHurtEffectType.DistSingle:
                return true;
            default:
                return false;
        }
    }

    public static bool IsSkillWorkOnLieHuo(int skillGroup)
    {
        switch(skillGroup)
        {
            case ESkillGroup.CiSha:
            case ESkillGroup.BanYue:
            case ESkillGroup.WarriorAttack:
                return true;
            default:
                return false;
        }
    }

    public static bool IsFlagSkill(int skillGroup)
    {
        switch (skillGroup)
        {
            case ESkillGroup.LiuXingHuoYu:
            case ESkillGroup.BingPaoXiao:
            case ESkillGroup.HuoQiangShu:
                return true;
            default:
                return false;
        }
    }

    public static bool InAttackRange(int range)
    {
        CSAvatar target = CSAvatarManager.Instance.GetSelectTarget();
        bool isInRage = InAttackRange(target,range);
        return isInRage;
    }

    public static bool InAttackRange(CSAvatar target, int range)
    {
        if (target != null && target.OldCell != null)
        {
            CSMisc.Dot2 dot = (target.OldCell.Coord -CSAvatarManager.MainPlayer.OldCell.Coord);
            if (Mathf.Abs(dot.x) <= range && Mathf.Abs(dot.y) <= range)
            {
                return true;
            }
        }
        return false;
    }


    static int red_player_pk_value = -1;

    public static int RedPlayerPkValue
    {
        get
        {
            if (-1 == red_player_pk_value)
            {
                int sundryId = 107;
                TABLE.SUNDRY sundryItem = null;
                if (SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
                {
                    var tokens = sundryItem.effect.Split('#');
                    if (tokens.Length > 0)
                    {
                        int.TryParse(tokens[0], out red_player_pk_value);
                    }
                }
            }

            return red_player_pk_value;
        }
    }

    public static bool IsReadOrGreyName(CSPlayerInfo playerInfo)
    {
        if(playerInfo == null)
        {
            return false;
        }
        return ((playerInfo.GreyName) || (playerInfo.PkValue >= RedPlayerPkValue));
    }

    public static void LaunchUpgradeSkill(int skillId)
    {
        switch (skillId)
        {
            case ESpecialSkillID.ShenShou5:
            case ESpecialSkillID.ShenShou10:
            case ESpecialSkillID.ShenShou15:
            case ESpecialSkillID.ShenShou20:
                {
                    CSSkillLaunchSystem.Instance.LaunchUpgradeSkill(skillId);
                }
                break;
        }
    }

}
