using UnityEngine;

public partial class UIGuildMemberListPanel : UIBasePanel
{
	protected UILabel[] mLabels;
    protected Vector2 mClipOffset;
    protected Vector3 mScrollViewLocalPosition;
	public override void Init()
	{
		base.Init();

        mbtn_accusechief.onClick = OnAccuseChief;
        mbtn_cancelguild.onClick = OnDisposeGuild;
        EventDelegate.Add(mHideOffline.onChange, RefreshMemberList);
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.AddEvent(CEvent.OnImpeachBubbleVisible, OnImpeachBubbleVisible);

        CSGuildInfo.Instance.Tab = UnionTab.UnionMemberInfo;
        ScriptBinder.InvokeRepeating(0.0f, 1.0f, CheckAccuseChief);
    }

    protected void CheckAccuseChief()
    {
        var ImpeachMsg = CSGuildInfo.Instance.mImpeachMsg;
        bool isInImpeach = null != ImpeachMsg && ImpeachMsg.timeS > 0;
        mbtn_accusechief.CustomActive(isInImpeach || CSGuildInfo.Instance.CheckCanAccuseChief());
    }

    protected void OnGuildTabDataChanged(uint id,object argv)
    {
        RefreshBaseInfo();
        RefreshMemberList();
    }

    protected void OnImpeachBubbleVisible(uint id, object argv)
    {
        RefreshBaseInfo();
    }

    public override void Show()
    {
        base.Show();

        mbtn_accusechief.CustomActive(CSGuildInfo.Instance.CheckCanAccuseChief());

        Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
        //查询弹劾信息 刷新弹劾按钮状态
        Net.CSImpeachmentMessage(3, 0);

        mClipOffset = mScrollView.GetComponent<UIPanel>().clipOffset;
        mScrollViewLocalPosition = mScrollView.transform.localPosition;
        RefreshBaseInfo();
        RefreshMemberList();
    }

    public void RefreshBaseInfo()
    {
        mbtn_accusechief.CustomActive(CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President);
        var guildInfo = CSGuildInfo.Instance.GetGuildInfo();
        var ImpeachMsg = CSGuildInfo.Instance.mImpeachMsg;
        if (null != mlb_accusechief)
            mlb_accusechief.text = CSString.Format(ImpeachMsg == null || ImpeachMsg.timeS == 0, 860, 861);
        if(null != mlb_cancelguild && null != guildInfo && null != guildInfo.brief)
            mlb_cancelguild.text = string.IsNullOrEmpty(guildInfo.brief.name) ? CSString.Format(862) : CSString.Format(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President, 863, 864);
    }

    EndLessList<UIGuildMemberItemBinder, GuildMemberItemData> mMemberList;
    public void RefreshMemberList()
    {
        if(null == mMemberList)
        {
            mMemberList = new EndLessList<UIGuildMemberItemBinder, GuildMemberItemData>(SortType.Vertical, mContainer, mPoolHandleManager,16,ScriptBinder);
        }
        int onlineCount = 0;
        //CSGuildInfo.Instance.AddRandomMember();
        var tabInfo = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionMemberInfo);
        if (tabInfo == null || null == tabInfo.unionInfo)
        {
            mMemberList.Clear();
        }
        else
        {
            mMemberList.Clear();
            var guildInfo = tabInfo.unionInfo;
            for (int i = 0; i < guildInfo.members.Count; i++)
            {
                if (!mHideOffline.value || guildInfo.members[i].isOnline)
                {
                    var itemData = mMemberList.Append();
                    itemData.member = guildInfo.members[i];
                    itemData.index = mMemberList.Count - 1;
                }
                if (guildInfo.members[i].isOnline)
                    ++onlineCount;
            }
        }

        if(null != mOnlineCount)
        {
            mOnlineCount.text = CSString.Format(793, $"{onlineCount}/{mMemberList.Count}");
        }

        mScrollView.GetComponent<UIPanel>().clipOffset = mClipOffset;
        mScrollView.transform.localPosition = mScrollViewLocalPosition;
        mMemberList.Sort(UnionMemberComparer);
        var elements = mMemberList.Elements();
        for(int i = 0; i < elements.Count; ++i)
        {
            elements[i].index = i;
        }
        mMemberList.Bind();

        mScrollView.gameObject.SetActive(false);
        mScrollView.gameObject.SetActive(true);
        mScrollView.enabled = mMemberList.Count > 6;
    }

    int UnionMemberComparer(GuildMemberItemData lmember, GuildMemberItemData rmember)
    {
        union.UnionMember l = lmember.member;
        union.UnionMember r = rmember.member;
        //在线排前面
        if (l.isOnline != r.isOnline)
            return l.isOnline ? -1 : 1;

        //职位大的排前面
        if (l.position != r.position)
            return l.position - r.position;

        //等级从大到小排
        if (l.level != r.level)
            return r.level - l.level;

        //战力从大到小排
        if (l.fighting != r.fighting)
            return r.fighting - l.fighting;

        //安GUID排序
        return l.roleId.CompareTo(r.roleId);
    }

    private void OnAccuseChief(GameObject obj)
    {
        if (CSGuildInfo.Instance.mImpeachMsg == null || CSGuildInfo.Instance.mImpeachMsg.timeS == 0)
        {
            if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.VicePresident)
            {
                Net.CSImpeachmentMessage(1, 0);
            }
            else
            {
                UtilityTips.ShowPromptWordTips(31, OnImpeachment, CSGuildInfo.Instance.ImpeachmentCostItemCount, CSGuildInfo.Instance.ImpeachmentCostItemId.ItemName());
            }
        }
        else
        {
            UIManager.Instance.CreatePanel<UIAccuseChiefPanel>();
        }
    }

    private void OnDisposeGuild(GameObject gp)
    {
        var guildInfo = CSGuildInfo.Instance.GetGuildInfo();
        if (null == guildInfo)
            return;

        if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President && guildInfo.members.Count >= 10)
        {
            UtilityTips.ShowRedTips(857);
            return;
        }
        int pid = 35;
        if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President)
        {
            pid = 34;
        }
        else
        {
            TABLE.SUNDRY tblSundry = null;
            int serverOpenDay = 0;
            int serverCombineDay = 0;
            if (SundryTableManager.Instance.TryGetValue(434, out tblSundry))
            {
                string[] values = tblSundry.effect.Split('#');

                if (values.Length > 0) int.TryParse(values[0], out serverOpenDay);
                if (values.Length > 1) int.TryParse(values[1], out serverCombineDay);
            }

            int combineServerDay = 0;//=CSMainPlayerInfo.Instance.RoleExtraValues.combineServerDay;
            int openServerDays = null == CSMainPlayerInfo.Instance.RoleExtraValues ? 0 : CSMainPlayerInfo.Instance.RoleExtraValues.openServerDays;
            pid = ((openServerDays >= serverOpenDay && combineServerDay <= 0) ||
                combineServerDay >= serverCombineDay) ? 36 : 35;
        }

        UtilityTips.ShowPromptWordTips(pid,OnSendMessageClick);
    }

    private void OnSendMessageClick()
    {
        if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President)
        {
            Net.CSDisposeUnionMessage();
        }
        else
        {
            Net.CSLeaveUnionMessage(CSMainPlayerInfo.Instance.ID);
        }
    }

    protected void OnImpeachment()
    {
        int need = CSGuildInfo.Instance.ImpeachmentCostItemCount;
        long owned = CSItemCountManager.Instance.GetItemCount(CSGuildInfo.Instance.ImpeachmentCostItemId);
        if(owned < need)
        {
            Utility.ShowGetWay(CSGuildInfo.Instance.ImpeachmentCostItemId);
            return;
        }
        Net.CSImpeachmentMessage(1, 0);
    }

    protected override void OnDestroy()
    {
        mMemberList?.Destroy();
        mMemberList = null;
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.RemoveEvent(CEvent.OnImpeachBubbleVisible, OnImpeachBubbleVisible);
        UIGuildMemberItemBinder.Clear();
        base.OnDestroy();
    }
}