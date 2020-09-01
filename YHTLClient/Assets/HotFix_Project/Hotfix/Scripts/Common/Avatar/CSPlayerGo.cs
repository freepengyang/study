using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;

public class CSPlayerGo : CSAvatarGo
{
    public override void Init(CSAvatar avatar)
    {
        base.Init(avatar);
        Transform root = avatar.CacheRootTransform;
        if (root != null)
        {
            CSModelModule module = avatar.ModelModule;
            if (module != null)
            {
                Owner.Model.InitPart(module);
            }

            avatar.InitHead();
            if (Owner is CSPlayer player)
                player.InitShieldEffect(true);
        }
    }

    public override void OnHit(CSAvatar clicker)
    {
        if (clicker == null)
        {
            return;
        }

        if (Owner == null || Owner.IsDead)
        {
            //Debug.Log("MonsterGo: owner is null or isDead");
            return;
        }

        CSModel clickerModel = clicker.Model;
        if (clickerModel != null && clickerModel.Bottom.Go != null)
        {
            Owner.Model.AttachBottom(clickerModel.Bottom);
            clickerModel.ShowSelectAndHideOtherBottom(ModelStructure.Bottom);
            Owner.head.Show();
        }

        if (Owner.BaseInfo == null)
        {
            return;
        }

        CSAvatarInfo info = Owner.BaseInfo;
        UIManager.Instance.CreatePanel<UIRoleSelectionInfoPanel>((f) =>
        {
            (f as UIRoleSelectionInfoPanel).ShowSelectData(info);
        });

        if (CSMainPlayerInfo.Instance.TeamId > 0 && CSTeamInfo.Instance.MyTeamData != null)
        {
            CSPlayerInfo playerInfo = info as CSPlayerInfo;
            ILBetterList<team.TeamMember> listTeamMember =
                CSTeamInfo.Instance.SortDicTeamShowOrder(CSTeamInfo.Instance.MyTeamData.teamInfo);
            for (int i = 0, max = listTeamMember.Count; i < max; i++)
            {
                team.TeamMember member = listTeamMember[i];
                if (member.roleId == playerInfo.ID)
                {
                    HotManager.Instance.EventHandler.SendEvent(CEvent.SelectMyTeamPlayer, i);
                    break;
                }
            }
        }
    }
}