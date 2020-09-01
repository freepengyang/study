using fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHurtManager : Singleton<CSHurtManager>
{
    public void Play(SkillEffect effect, CSAvatar attacker, CSAvatar target)
    {
        if(attacker == null)
        {
            return;
        }
        attacker.GetSkillResult().Initialize(effect, attacker, target);
    }

    public void ShowHp(BufferDeltaHP info)
    {
        if(CSAvatarManager.Instance == null)
        {
            return;
        }
        if(CSAvatarManager.Instance == null)
        {
            return;
        }
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(info.id);
        if (avatar != null && avatar.BaseInfo != null)
        {
            CSAvatarInfo avatarInfo = avatar.BaseInfo;
            if (avatarInfo != null)
            {
                if (info.hp != null)
                {
                    avatarInfo.RealHP = info.hp.value;
                    avatarInfo.HP = info.hp.value;
                }
            }
        }
        ShowHp(avatar,info.deltaHP, info.isCritical);
    }

    public void ShowHp(CSAvatar avatar, int deltaHp, bool isCrit)
    {
        if (avatar != null && avatar.head != null && avatar.BaseInfo != null)
        {
            int type = (deltaHp > 0) ? ThrowTextType.Cure : (isCrit ? ThrowTextType.Critical : ThrowTextType.NormalDamage);
            int dir = UtilityFight.GetFlyDirection(CSAvatarManager.MainPlayer, avatar);
            if (type == ThrowTextType.Cure)
            {
                dir = (dir == EFlyDirection.Right) ? EFlyDirection.Left : EFlyDirection.Right;
            }
            avatar.head.PlayHurtEffect(deltaHp, dir,0, type);
            if (avatar.BaseInfo.HP > 0)
            {
                avatar.BaseInfo.HP = avatar.BaseInfo.RealHP;
            }
        }
    }
}
