using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class UIGuildCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public enum ChildPanelType
    {
        CPT_INFO = 1,
        CPT_LIST = 2,
        CPT_STOREHOUSE = 3,
        CPT_WEAL = 4,
        CPT_PRACTICE = 5,
        CPT_ACTIVITY = 6,
        CPT_COUNT = 7,
    }

    public override void Init()
    {
        base.Init();
		mbtn_close.onClick = this.Close;

        RegChildPanel<UIGuildInfoPanel>((int)ChildPanelType.CPT_INFO, mGuildInfoPanel.gameObject, mTogInfo);
        RegChildPanel<UIGuildListGroupPanel>((int)ChildPanelType.CPT_LIST, mGuildListGroupPanel.gameObject, mTogList);
        RegChildPanel<UIGuildBagPanel>((int)ChildPanelType.CPT_STOREHOUSE, mGuildBagPanel.gameObject, mTogHouse);
        RegChildPanel<UIGuildPracticePanel>((int)ChildPanelType.CPT_PRACTICE, mGuildPracticePanel.gameObject, mTogPractice,null,()=> { return CSGuildInfo.Instance.IsGuildPractiseOpened(true); });
        RegChildPanel<UIGuildWealPanel>((int)ChildPanelType.CPT_WEAL, mGuildWealPanel.gameObject, mTogRedPkg);
        RegChildPanel<UIGuildActivityPanel>((int)ChildPanelType.CPT_ACTIVITY, mUIGuildActivityPanel, mtg_Activity);

        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnMainPlayerGuildIdChanged);

        InitTabs();

        RegisterRed(mPractiseRedPoint, RedPointType.GuildPractice);
        RegisterRedList(mMemberRedPoint, RedPointType.GuildApplyList, RedPointType.GuildList);

        UIEventListener.Get(mtg_Activity.gameObject).onClick = (go) =>
        {
            OpenChildPanel((int)ChildPanelType.CPT_ACTIVITY, true)?.OpenChildPanel(1);
        };
    }

    void InitTabs()
    {
        bool hasGuild = Utility.HasGuild();
        InitTabName(hasGuild ? 892 : 891);

        for(int i = 1; i <= (int)ChildPanelType.CPT_COUNT; ++i)
        {
            int tabKey = i;
            if(!mChildToggles.ContainsKey(tabKey))
            {
                continue;
            }

            mChildToggles[i].transform.parent.CustomActive(hasGuild ||  i == (int)ChildPanelType.CPT_LIST);
        }

        mToggleGroup.Reposition();
    }

    public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    {
        if (type == (int)ChildPanelType.CPT_PRACTICE && !CSGuildInfo.Instance.IsGuildPractiseOpened(false))
        {
            if (Utility.HasGuild())
                return base.OpenChildPanel((int)ChildPanelType.CPT_INFO);
            return base.OpenChildPanel((int)ChildPanelType.CPT_LIST)?.OpenChildPanel((int)UIGuildListGroupPanel.ChildPanelType.CPT_GUILD_LIST);
        }

        if (type == (int)ChildPanelType.CPT_LIST)
        {
            var panel = base.OpenChildPanel(type, fromToggle);
            panel.OpenChildPanel((int)UIGuildListGroupPanel.ChildPanelType.CPT_GUILD_LIST);
            return panel;
        }

        if (type == (int)ChildPanelType.CPT_ACTIVITY)
        {
            if (!Utility.HasGuild())
            {
                base.OpenChildPanel((int)ChildPanelType.CPT_LIST)?.OpenChildPanel((int)UIGuildListGroupPanel.ChildPanelType.CPT_GUILD_LIST);
                return null;
            }

            //var panel = base.OpenChildPanel(type, fromToggle);
            ////panel.OpenChildPanel(1);
            //return panel;
        }

        return base.OpenChildPanel(type, fromToggle);
    }


    void InitTabName(int tipId)
    {
        var tabName = CSString.Format(tipId);
        if (null != mListTabName)
            mListTabName.text = $"[818181]{tabName}";
        if (null != mListTabCheckName)
            mListTabCheckName.text = $"[F3E7CD]{tabName}";
    }

    protected void OnGuildTabDataChanged(uint id,object argv)
    {
        if(argv is UnionTab tab)
        {

        }
    }

    protected void OnMainPlayerGuildIdChanged(uint id,object argv)
    {
        InitTabs();

        if (!Utility.HasGuild())
        {
            OpenChildPanel((int)ChildPanelType.CPT_LIST)?.OpenChildPanel((int)UIGuildListGroupPanel.ChildPanelType.CPT_GUILD_LIST);
        }
        else
        {
            OpenChildPanel((int)ChildPanelType.CPT_INFO);
        }
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged, OnMainPlayerGuildIdChanged);
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        base.OnDestroy();
    }
}