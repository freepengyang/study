using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class UIGuildInfoPanel : UIBasePanel
{
	string mDefaultAnnoncement;
    enum AnnounceStatus
    {
        Edit,
        Common
    }
    AnnounceStatus annouceStatus = AnnounceStatus.Common;

	public override void Init()
	{
		base.Init();
		mbtn_donate.onClick = OnClickDonate;
		mbtn_help.onClick = OnClickHelp;
		mbtn_rename.onClick = OnClickRename;
		mbtn_submit.onClick = OnSubmitAnnounce;
        mbtn_devide.onClick = OnClickDivide;
		UIEventListener.Get(mannouncement.gameObject).onSelect = OnFamilyNoticeChange;
		mDefaultAnnoncement = CSString.Format(791);

        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.AddEvent(CEvent.OnGuildBulletChanged, OnGuildBulletChanged);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnItemCounterChanged);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnItemCounterChanged);

        CSEffectPlayMgr.Instance.ShowUITexture(mGuildIcon, "guild_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mtexBg, "guild_flag");

        SetNoticeState(AnnounceStatus.Common);
        SetNoticeUIChange();

        CSGuildInfo.Instance.Tab = UnionTab.MainInfo;
    }

    public override void Show()
    {
        base.Show();

        BindCoroutine(1001, RefreshLogs());
        //RefreshLogs();
        mbtn_devide.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President);
        Net.CSGetUnionTabMessage((int)UnionTab.MainInfo);
        Net.CSGetUnionTabMessage((int)UnionTab.UnionLogMessages);
    }

    private void OnFamilyNoticeChange(GameObject go, bool state)
    {
        if (!state)
        {
            if (!string.IsNullOrEmpty(mannouncement.value) && mannouncement.value != mDefaultAnnoncement)
            {
                if (annouceStatus == AnnounceStatus.Edit)
                {
                    int leftCount = GetLeftAnnouncementModifyCount();
                    int tipId = leftCount <= 0 ? 61 : 30;
                    UtilityTips.ShowPromptWordTips(tipId, OnCancelSubmit, OnSubmit,0, leftCount);
                }
            }
            else
            {
                mannouncement.selectAllTextOnFocus = false;
                SetNoticeState(AnnounceStatus.Common);
                mannouncement.isSelected = false;
                SetDragScrollVisible(true);
                SetNoticeUIChange();
                mannouncement.value = mDefaultAnnoncement;
            }
        }
    }

    private void OnSubmit()
    {
        SetNoticeState(AnnounceStatus.Common);
        SetNoticeUIChange();
        SetDragScrollVisible(true);

        if (GetLeftAnnouncementModifyCount() > 0)
        {
            Net.CSUnionBulletinMessage(mannouncement.value);
        }
    }

    private void OnCancelSubmit()
    {
        mannouncement.selectAllTextOnFocus = true;
        mannouncement.isSelected = false;
        mannouncement.value = mDefaultAnnoncement;
        SetNoticeState(AnnounceStatus.Common);
        SetNoticeUIChange();
        SetDragScrollVisible(true);
    }

	protected void OnGuildTabDataChanged(uint id,object argv)
	{
		if(argv is UnionTab tab)
		{
			var union = CSGuildInfo.Instance.GetTabInfo(tab);
            if(null != union)
            {
                if (tab == UnionTab.MainInfo)
                {
                    if(null != union.unionInfo)
                        OnGuildMainInfoChanged(union.unionInfo);
                }
                else if (tab == UnionTab.UnionLogMessages)
                {
                    BindCoroutine(1001, RefreshLogs());
                }
            }
		}
	}

    protected IEnumerator RefreshLogs()
    {
        var logList = mPoolHandleManager.GetSystemClass<List<union.UnionLogInfo>>();
        logList.Clear();
        var union = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionLogMessages);
        if (null != union && null != union.logs)
        {
            logList.AddRange(union.logs);
            logList.Sort(sort);
        }
        mGrid.MaxCount = logList.Count;

        MatchCollection matchs = null;
        string pattern = "\\[item:(\\d+)\\]";
        TABLE.ITEM item;

        for (int i = 0; i < mGrid.MaxCount; i++)
        {
            var info = logList[i];
            var time = (double)info.time;
            var dt = CSServerTime.StampToDateTime(time);
            var lb = mGrid.controlList[i].transform.Find("Label").GetComponent<UILabel>();
            var labTime = mGrid.controlList[i].transform.Find("labTime").GetComponent<UILabel>();
            mGrid.controlList[i].transform.Find("bg").GetComponent<UISprite>().spriteName = (i % 2 == 0) ? "list_subbg1" : "list_subbg2";

            matchs = Regex.Matches(info.message, pattern);

            int itemid = 0;
            string date = dt.Year + "-" + dt.Month + "-" + dt.Day;
            if (matchs.Count > 0)
            {
                int star = 6;
                int lenght = matchs[0].Value.Length - 7;
                int.TryParse(matchs[0].Value.Substring(star, lenght), out itemid);

                if (ItemTableManager.Instance.TryGetValue(itemid, out item))
                {
                    lb.text = info.message.Replace(matchs[0].Value, item.name).BBCode(ColorType.SubTitleColor);
                }
                labTime.text = date.ToString();
            }
            else
            {
                lb.text = info.message.BBCode(ColorType.SubTitleColor);
                labTime.text = date.ToString();
            }
        }

        int cnt = logList.Count;
        logList.Clear();
        mPoolHandleManager.Recycle(logList);
        yield return null;

        if (cnt > 6)
        {
            mScrollView.ScrollImmidate(1);
        }
        else
        {
            mScrollView.ScrollImmidate(0);
        }
    }

    protected void OnGuildMainInfoChanged(union.UnionInfo info)
    {
        if (null != mlb_president)
            mlb_president.text = info.brief.leaderName;

        if (null != mlb_level)
            mlb_level.text = CSString.Format(1711, info.brief.level);

        if (null != mlb_cur_money)
        {
            mlb_cur_money.text = $"{info.wealth}".BBCode(info.wealth > 0 ? ColorType.MainText : ColorType.Red);
        }

        if (null != mlb_not_enough)
        {
            mlb_not_enough.text = info.wealth >= 0 ? string.Empty : CSString.Format(790);
        }

        if (null != mlb_name)
        {
            mlb_name.text = info.brief.name;
        }

        if (null != mlb_unionName)
        {
            mlb_unionName.text = info.brief.name;
        }

        if (null != mlb_yuanbao)
            mlb_yuanbao.text = $"{CSGuildInfo.Instance.OwnedYuanbao}";

        TABLE.UNION unionItem;
        if (UnionTableManager.Instance.TryGetValue(info.brief.level, out unionItem))
        {
            if (null != mlb_member_count)
            {
                mlb_member_count.text = $"{info.brief.size}/{unionItem.maxPlayers}";
            }
        }

        if (null != mannouncement)
        {
            if (string.IsNullOrEmpty(info.bulletin))
            {
                mannouncement.value = mDefaultAnnoncement;
            }
            else
            {
                mannouncement.value = info.bulletin;
                mDefaultAnnoncement = info.bulletin;
            }
        }

        SetNoticeUIChange();

        mbtn_devide.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President);
        mbtn_submit.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President);
        mbtn_rename.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President);

        RefreshProgress();
    }

    void RefreshProgress()
    {
        var guildInfo = CSGuildInfo.Instance.GetGuildInfo();
        TABLE.UNION tbl = null;
        if (null != guildInfo && null != guildInfo.brief && UnionTableManager.Instance.TryGetValue(guildInfo.brief.level, out tbl))
        {
            float cur = (float)guildInfo.wealth;
            float tar = (float)tbl.upgrade;
            float scale = cur / tar;
            if (null != mSlider)
                mSlider.value = scale;
            if (null != mlb_progress)
                mlb_progress.text = $"{guildInfo.wealth}/{tbl.upgrade}";
        }
    }

    protected void OnGuildBulletChanged(uint id,object argv)
    {
        if(argv is union.UnionBulletinAck ack && ack.isSucccess)
        {
            mDefaultAnnoncement = mannouncement.value;
        }
    }

    protected void OnItemCounterChanged(uint id ,object argv)
    {
        RefreshProgress();
    }

    private void OnSubmitAnnounce(GameObject go)
    {
        if (GetLeftAnnouncementModifyCount() <= 0)
        {
            UtilityTips.ShowRedTips(792);
            return;
        }

        if (annouceStatus == AnnounceStatus.Common)
        {
            mannouncement.selectAllTextOnFocus = true;
            SetNoticeState(AnnounceStatus.Edit);
            SetNoticeUIChange();
			mannouncement.isSelected = true;
            SetDragScrollVisible(false);
        }
    }

    private void OnClickDivide(GameObject go)
    {
        if(CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President)
        {
            //只有会长才可以分配
            UtilityTips.ShowRedTips(1048);
            return;
        }

        UIManager.Instance.CreatePanel<UIGuildDevidePanel>();
    }

    private void SetDragScrollVisible(bool isShow)
    {
        mDragScrollView.CustomActive(isShow);
    }

    private void SetNoticeState(AnnounceStatus statu)
    {
        annouceStatus = statu;
    }

    /// <summary>
    /// 获取剩余的公告修改次数
    /// </summary>
    /// <returns></returns>
    private int GetLeftAnnouncementModifyCount()
    {
        TABLE.SUNDRY tblSundry;
        if (SundryTableManager.Instance.TryGetValue(283, out tblSundry))
        {
            int totalCount = 0;
            int.TryParse(tblSundry.effect, out totalCount);

            var guildInfo = CSGuildInfo.Instance.GetGuildInfo();
            if (guildInfo != null)
            {
                return Mathf.Max(totalCount - guildInfo.bulletinCount);
            }
        }

        return 0;
    }

    private void SetNoticeUIChange()
    {
		if(null != mcolider)
		{
			mcolider.enabled = annouceStatus == AnnounceStatus.Common ? false : CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President;
		}
    }

    protected void OnClickDonate(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIGuildDonatePanel>();
    }

    protected void OnClickHelp(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GuildInfo);
    }

    protected void OnClickRename(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIRenameGuildPanel>();
    }

    private int sort(union.UnionLogInfo l, union.UnionLogInfo r)
    {
        return l.time.CompareTo(r.time);
    }

    protected override void OnDestroy()
	{
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.RemoveEvent(CEvent.OnGuildBulletChanged, OnGuildBulletChanged);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemCounterChanged);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnItemCounterChanged);
        base.OnDestroy();
	}
}