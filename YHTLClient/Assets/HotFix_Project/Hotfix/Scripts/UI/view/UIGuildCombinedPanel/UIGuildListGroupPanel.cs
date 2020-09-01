public partial class UIGuildListGroupPanel : UIBasePanel
{
    public enum ChildPanelType
    {
        CPT_MEMBER_LIST = 1,//成员列表
        CPT_APPLY_LIST = 2,//申请列表
        CPT_GUILD_LIST = 3,//公会列表
    }

    public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    {
        bool hasGuild = Utility.HasGuild();
        int guildPos = CSMainPlayerInfo.Instance.GuildPos;
        UIBasePanel panel = null;
        if (!hasGuild)
        {
            panel = base.OpenChildPanel((int)ChildPanelType.CPT_GUILD_LIST, fromToggle);
        }
        else
        {
            if (type == (int)ChildPanelType.CPT_APPLY_LIST && guildPos > (int)GuildPos.Presbyter)
            {
                panel = base.OpenChildPanel((int)ChildPanelType.CPT_MEMBER_LIST, fromToggle);
            }
            else
            {
                panel = base.OpenChildPanel(type, fromToggle);
            }
        }

        mtg_familyList.CustomActive(true);
        mtg_memberList.CustomActive(hasGuild);
        mtg_applyList.CustomActive(hasGuild && guildPos <= (int)GuildPos.Presbyter);
        mTogGroup.Reposition();

        return panel;
    }

    public override void Init()
    {
        base.Init();

        RegChildPanel<UIGuildMemberListPanel>((int)ChildPanelType.CPT_MEMBER_LIST, mGuildMemberListPanel, mtg_memberList);
        RegChildPanel<UIGuilApplyListPanel>((int)ChildPanelType.CPT_APPLY_LIST, mGuilApplyListPanel, mtg_applyList);
        RegChildPanel<UIGuildListPanel>((int)ChildPanelType.CPT_GUILD_LIST, mGuildListPanel, mtg_familyList);

        RegisterRed(mApplyListRedPoint,RedPointType.GuildApplyList);
        RegisterRed(mFamilyListRedPoint,RedPointType.GuildList);

        mtg_memberList.Set(true);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnMainPlayerGuildIdChanged);
    }

    protected void OnMainPlayerGuildIdChanged(uint id, object argv)
    {
        bool hasGuild = Utility.HasGuild();
        int guildPos = CSMainPlayerInfo.Instance.GuildPos;
        mtg_familyList.CustomActive(true);
        mtg_memberList.CustomActive(hasGuild);
        mtg_applyList.CustomActive(hasGuild && guildPos <= (int)GuildPos.Presbyter);
        mTogGroup.Reposition();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged, OnMainPlayerGuildIdChanged);
        base.OnDestroy();
    }
}