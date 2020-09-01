using System;
using System.Collections.Generic;
using union;
using UnityEngine;

public partial class UIGuildListPanel : UIBasePanel
{
    protected Vector2 mClipOffset;
    protected Vector3 mScrollViewLocalPosition;
    EndLessList<UIGuildListItemBinder,GuildItemData> mGuilList;

    public override void Init()
    {
        base.Init();
        mbtnCreate.onClick = OnBtnCreateClicked;

        mClientEvent.AddEvent(CEvent.OnGuildApplyUnionListChanged, OnGuildApplyUnionListChanged);
        mClientEvent.AddEvent(CEvent.OnGuildInfoChanged, OnGuildApplyUnionListChanged);
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildApplyUnionListChanged);

        EventDelegate.Add(mtg_select.onChange, OnToggleChanged);

        mGuilList = new EndLessList<UIGuildListItemBinder, GuildItemData>(SortType.Vertical, mContainer, mPoolHandleManager, 12, ScriptBinder);
        mbtn_help.onClick = OnClickHelp;
        mbtnOneKeyApply.onClick = this.OnClickOneKeyApply;
        CSGuildInfo.Instance.Tab = UnionTab.UnionsList;
        CSEffectPlayMgr.Instance.ShowUITexture(mpattern, "pattern");
    }

    protected void OnToggleChanged()
    {
        RefreshGuildList();
    }

    protected void OnClickHelp(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.FamilyListHelp);
    }

    protected void OnClickOneKeyApply(GameObject go)
    {
        if (Utility.HasGuild())
        {
            return;
        }
        if (null != mGuilList)
        {
            var elements = mGuilList.Elements();
            for(int i = 0; i < elements.Count; ++i)
            {
                var element = elements[i];
                if (null != element && null != element.brief)
                {
                    long id = element.brief.unionId;
                    string str = CSMainPlayerInfo.Instance.GuildName;
                    var applyUnionList = element.applyUnionList;
                    if (applyUnionList != null && applyUnionList.allUList.Contains(id))
                    {
                        //暂时去掉取消申请
                        //Net.CSRemoveApplyUnionMessage(id);
                    }
                    else
                    {
                        Net.CSApplyUnionMessage(id);
                    }
                }
            }
        }
    }

    protected void OnGuildApplyUnionListChanged(uint id,object argv)
    {
        Refresh();
        RefreshAddTime();
        RefreshGuildList();
    }

    public override void Show()
    {
        base.Show();

        mClipOffset = mScrollView.GetComponent<UIPanel>().clipOffset;
        mScrollViewLocalPosition = mScrollView.transform.localPosition;
        Net.CSGetUnionTabMessage((int)UnionTab.UnionsList);

        Refresh();
        RefreshAddTime();
        RefreshGuildList();
    }

    protected void Refresh()
    {
        if (null != mbtn_help)
            mbtn_help.CustomActive(Utility.HasGuild());

        bool hasGuild = !string.IsNullOrEmpty(CSMainPlayerInfo.Instance.GuildName);
        if (null != mlb_create)
        {
            var pos = CSMainPlayerInfo.Instance.GuildPos;
            mlb_create.text = hasGuild ? (CSString.Format(pos == (int)GuildPos.President, 863, 864)) : CSString.Format(862);
            mbtnCreate.CustomActive(!hasGuild);
        }
    }

    protected void RefreshAddTime()
    {
        var applyUnionList = CSGuildInfo.Instance.applyUnionList;
        if (null == applyUnionList || applyUnionList.lastUnionTime == 0 || !CSGuildInfo.Instance.IsInApplyColdTime(applyUnionList.lastUnionTime) || Utility.HasGuild())
        {
            mlb_timecount.CustomActive(false);
        }
        else
        {
            mlb_timecount.CustomActive(true);
            ScriptBinder.InvokeRepeating(0.0f, 0.01f, AddTimeCoolDown);
        }
    }

    protected void RefreshGuildList()
    {
        mGuilList.Clear();
        var applyUnionList = CSGuildInfo.Instance.applyUnionList;
        var unionList = CSGuildInfo.Instance.UnionList;
        if(null != applyUnionList && null != applyUnionList.allUList && null != unionList)
        {
            int idx = 0;
            for(int i = 0; i < unionList.Count; ++i)
            {
                var brief = unionList[i];
                TABLE.UNION tbl_union;
                if (!UnionTableManager.Instance.TryGetValue(brief.level, out tbl_union))
                {
                    continue;
                }

                bool isOpenAdd = false;
                int addNum = isOpenAdd ? 0 : 0;
                int max = tbl_union.maxPlayers + addNum;
                //是否行会人数已满
                bool canAdd = brief.size < max;
                //如果公会已满 且 过滤打开
                if (!canAdd && mtg_select.value)
                    continue;

                var guildData = mGuilList.Append();
                guildData.brief = brief;
                guildData.applyUnionList = applyUnionList;
                guildData.index = idx++;
            }
        }

        mScrollView.GetComponent<UIPanel>().clipOffset = mClipOffset;
        mScrollView.transform.localPosition = mScrollViewLocalPosition;

        mScrollView.enabled = mGuilList.Count > 6;
        mGuilList.Sort(UnionListComparer);
        var elements = mGuilList.Elements();
        mbtnOneKeyApply.CustomActive(elements.Count > 0 && !Utility.HasGuild());
        for (int i = 0; i < elements.Count; ++i)
        {
            elements[i].index = i;
        }
        mGuilList.Bind();
        mEmptyDesc.CustomActive(mGuilList.Count <= 0);
    }

    protected int UnionListComparer(GuildItemData l,GuildItemData r)
    {
        if (l.brief.level != r.brief.level)
            return r.brief.level - l.brief.level;
        if (l.brief.size != r.brief.size)
            return r.brief.size - l.brief.size;
        return l.brief.unionId < r.brief.unionId ? -1 : (l.brief.unionId == r.brief.unionId ? 0 : 1);
    }

    protected void AddTimeCoolDown()
    {
        var applyUnionList = CSGuildInfo.Instance.applyUnionList;
        if (null == applyUnionList || applyUnionList.lastUnionTime == 0 || !CSGuildInfo.Instance.IsInApplyColdTime(applyUnionList.lastUnionTime) || Utility.HasGuild())
        {
            ScriptBinder.StopInvokeRepeating();
            mlb_timecount.CustomActive(false);
            RefreshGuildList();
            return;
        }
        DateTime lastToTime = CSServerTime.StampToDateTime(applyUnionList.lastUnionTime).AddHours(2);
        TimeSpan desTime = lastToTime - CSServerTime.Now;
        mlb_timecount.text = CSString.Format(869) + ((int)desTime.TotalHours).ToString().PadLeft(2, '0') + ":" + desTime.Minutes.ToString().PadLeft(2, '0') + ":" + desTime.Seconds.ToString().PadLeft(2, '0');
    }

    private void OnBtnCreateClicked(GameObject gp)
    {
        if (string.IsNullOrEmpty(CSMainPlayerInfo.Instance.GuildName))
        {
            UIManager.Instance.CreatePanel<UICreateGuildPanel>();
        }
        else
        {
            if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President)
            {
                UtilityTips.ShowPromptWordTips(28, Net.CSDisposeUnionMessage);
            }
            else
            {
                Net.CSLeaveUnionMessage(CSMainPlayerInfo.Instance.ID);
            }
        }
    }

    protected override void OnDestroy()
    {
        if(null != mpattern)
            CSEffectPlayMgr.Instance.Recycle(mpattern);
        mpattern = null;
        mGuilList?.Destroy();
        mGuilList = null;
        UIGuildListItemBinder.Clear();
        mClientEvent.RemoveEvent(CEvent.OnGuildApplyUnionListChanged, OnGuildApplyUnionListChanged);
        mClientEvent.RemoveEvent(CEvent.OnGuildInfoChanged, OnGuildApplyUnionListChanged);
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildApplyUnionListChanged);
        base.OnDestroy();
    }
}