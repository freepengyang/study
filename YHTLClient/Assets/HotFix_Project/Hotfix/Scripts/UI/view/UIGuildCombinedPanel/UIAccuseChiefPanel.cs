using System;
using UnityEngine;

public partial class UIAccuseChiefPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();

        mbtn_Yes.onClick = OnBtnVoteYes;
        mbtn_No.onClick = OnBtnVoteNo;
        mbtn_close.onClick = this.Close;
        mbtn_bg.onClick = this.Close;
        mbtn_description.onClick = this.OnHelpTips;

        mClientEvent.AddEvent(CEvent.OnImpeachBubbleVisible, OnImpeachBubbleVisible);
        mlb_time.CustomActive(true);

        Net.CSImpeachmentMessage(3, 0);
    }

    protected void OnHelpTips(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.AccuseChiefPanel);
    }

    protected void OnImpeachBubbleVisible(uint id,object argv)
    {
        if(null == CSGuildInfo.Instance.mImpeachMsg)
        {
            this.Close();
            return;
        }
        InitMenuInfo(CSGuildInfo.Instance.mImpeachMsg);
    }

    public override void Show()
    {
        base.Show();
        InitMenuInfo(CSGuildInfo.Instance.mImpeachMsg);
    }

    private void InitMenuInfo(union.ImpeachementMsg impeachementMsg)
    {
        if (impeachementMsg == null) return;

        bool isVoted = impeachementMsg.data;
        mhvnt_vote.CustomActive(!isVoted);
        mlb_have_voted.text = isVoted ? CSString.Format(853) : string.Empty;
        mlb_have_voted.CustomActive(isVoted);
        msp_agree_graphic.value = (float)impeachementMsg.agree / (impeachementMsg.agree + impeachementMsg.refuse);
        msp_disagree_graphic.value = (float)impeachementMsg.refuse / (impeachementMsg.agree + impeachementMsg.refuse);
        mlb_agree_count.text = CSString.Format(854, impeachementMsg.agree);
        mlb_disagree_count.text = CSString.Format(855, impeachementMsg.refuse);
        mlb_desc.text = CSString.Format(856, impeachementMsg.impeachementerName);
        mLastToVote = CSServerTime.StampToDateTimeForSecond(((long)impeachementMsg.timeS) * 1000).AddHours(12);
        ScriptBinder.InvokeRepeating(0.0f, 0.01f, TimeCountDown);
    }

    DateTime mLastToVote;
    protected void TimeCountDown()
    {
        if((mLastToVote - CSServerTime.Now).TotalHours > 0)
        {
            if(null != mlb_time)
                mlb_time.text = GetTimeStr(mLastToVote - CSServerTime.Now);
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
            this.Close();
        }
    }

    private string GetTimeStr(TimeSpan desTime)
    {
        return CSString.Format(852, ((int)desTime.TotalHours).ToString().PadLeft(2, '0'), desTime.Minutes.ToString().PadLeft(2, '0'), desTime.Seconds.ToString().PadLeft(2, '0'));
    }

    private void OnBtnVoteYes(GameObject go)
    {
        Net.CSImpeachmentMessage(2, 1);
    }

    private void OnBtnVoteNo(GameObject go)
    {
        Net.CSImpeachmentMessage(2, 2);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnImpeachBubbleVisible, OnImpeachBubbleVisible);
        base.OnDestroy();
    }
}