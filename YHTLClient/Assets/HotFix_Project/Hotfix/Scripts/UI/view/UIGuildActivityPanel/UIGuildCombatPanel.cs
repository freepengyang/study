using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unionbattle;

public partial class UIGuildCombatPanel : UIBasePanel
{
    Dictionary<int, Dictionary<int, int>> allRewards;


    UnionActivityInfo activityInfo;

    long timerCount;//倒计时秒
    string timerHintStr;
    string timerColorStr;

    TABLE.TIMER timerCfg;

    string firstGuildName;
    string secondGuildName;
    string thirdGuildName;

    string firstStr;
    string secondStr;
    string thirdStr;
    string otherStr;
    string noneStr;

    Dictionary<int, UILabel> guildNameLabels = new Dictionary<int, UILabel>();
    Dictionary<int, UIGridContainer> guildGrids = new Dictionary<int, UIGridContainer>();


    List<UIItemBase> rewardA = new List<UIItemBase>();
    List<UIItemBase> rewardB = new List<UIItemBase>();
    List<UIItemBase> rewardC = new List<UIItemBase>();
    List<UIItemBase> rewardD = new List<UIItemBase>();

    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mtx_bg, "guild_fight");
        mClientEvent.AddEvent(CEvent.GuildActivityStateChange, ActivityInfoChange);

        mbtn_go.onClick = JoinBtnClick;


        mlb_activityEntrance.text = ClientTipsTableManager.Instance.GetClientTipsContext(1927);
        timerCfg = CSGuildActivityInfo.Instance.GuildCombatTimer;
        if (timerCfg != null)
        {
            mlb_time.text = $"[cbb694]{ClientTipsTableManager.Instance.GetClientTipsContext(1959)}{timerCfg.desc}";
        }
        mlb_openTimer.text = "";


        guildNameLabels.Clear();
        mGrid_rewards.MaxCount = 4;
        for (int i = 0; i < mGrid_rewards.MaxCount; i++)
        {
            var go = mGrid_rewards.controlList[i].transform;
            UILabel name = go.Find("lb_name").GetComponent<UILabel>();
            UIGridContainer grid = go.Find("Grid").GetComponent<UIGridContainer>();
            int rank = i < mGrid_rewards.MaxCount - 1 ? i + 1 : 0;
            guildNameLabels.Add(rank, name);
            guildGrids.Add(rank, grid);
        }   

        firstStr = ClientTipsTableManager.Instance.GetClientTipsContext(1923);
        secondStr = ClientTipsTableManager.Instance.GetClientTipsContext(1924);
        thirdStr = ClientTipsTableManager.Instance.GetClientTipsContext(1925);
        otherStr = ClientTipsTableManager.Instance.GetClientTipsContext(1926);
        noneStr = ClientTipsTableManager.Instance.GetClientTipsContext(1163);

        guildNameLabels[0].text = $"{UtilityColor.SubTitle}{otherStr}";


        allRewards = CSGuildActivityInfo.Instance.GetCombatRewardsDic();
        if (allRewards.ContainsKey(0))
        {
            Utility.GetItemByBoxid(allRewards[0], guildGrids[0], ref rewardA, itemSize.Size50);
        }
        if (allRewards.ContainsKey(1))
        {
            Utility.GetItemByBoxid(allRewards[1], guildGrids[1], ref rewardB, itemSize.Size50);
        }
        if (allRewards.ContainsKey(2))
        {
            Utility.GetItemByBoxid(allRewards[2], guildGrids[2], ref rewardC, itemSize.Size50);
        }
        if (allRewards.ContainsKey(3))
        {
            Utility.GetItemByBoxid(allRewards[3], guildGrids[3], ref rewardD, itemSize.Size50);
        }

        mscroll.SetDynamicArrowVertical(msp_scroll);

        for (var it = guildGrids.GetEnumerator(); it.MoveNext();)
        {
            it.Current.Value.CustomActive(false);
        }
    }

    public override void Show()
    {
        base.Show();

        for (var it = guildGrids.GetEnumerator(); it.MoveNext();)
        {
            it.Current.Value.CustomActive(true);
        }

        ActivityInfoChange(0, null);
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtx_bg);
        UIItemManager.Instance.RecycleItemsFormMediator(rewardA);
        UIItemManager.Instance.RecycleItemsFormMediator(rewardB);
        UIItemManager.Instance.RecycleItemsFormMediator(rewardC);
        UIItemManager.Instance.RecycleItemsFormMediator(rewardD);

        activityInfo = null;
        guildNameLabels?.Clear();
        guildNameLabels = null;
        guildGrids?.Clear();
        guildGrids = null;

        base.OnDestroy();
    }


    void ActivityInfoChange(uint id, object data)
    {
        //公会名字
        firstGuildName = noneStr;
        secondGuildName = noneStr;
        thirdGuildName = noneStr;
        var info = CSGuildActivityInfo.Instance.LastCombatRank;
        if (info != null)
        {
            var ranks = info.ranks;
            if (ranks.Count > 0) firstGuildName = ranks[0].name;
            if (ranks.Count > 1) secondGuildName = ranks[1].name;
            if (ranks.Count > 2) thirdGuildName = ranks[2].name;
        }
        guildNameLabels[1].text = $"{UtilityColor.SubTitle}{firstStr}{UtilityColor.MainText}{firstGuildName}";
        guildNameLabels[2].text = $"{UtilityColor.SubTitle}{secondStr}{UtilityColor.MainText}{secondGuildName}";
        guildNameLabels[3].text = $"{UtilityColor.SubTitle}{thirdStr}{UtilityColor.MainText}{thirdGuildName}";


        //倒计时
        activityInfo = CSGuildActivityInfo.Instance.GetActivityInfo((int)GuildActivityType.COMBAT);
        if (activityInfo == null)
        {
            mlb_openTimer.text = "";
            ScriptBinder.StopInvokeRepeating();
            return;
        }

        if (activityInfo.state == 0)
        {
            timerColorStr = UtilityColor.Red;
            timerHintStr = CSGuildActivityInfo.Instance.openTimeLeftStr;
            timerCount = activityInfo.startTime / 1000 - CSServerTime.Instance.TotalSeconds;
        }
        else
        {
            timerColorStr = UtilityColor.Green;
            timerHintStr = CSGuildActivityInfo.Instance.closeTimeLeftStr;
            if (timerCfg == null)
            {
                timerCount = -1;
            }
            else
            {
                var endTime = UtilityMath.CronTimeStringParseToTamp(timerCfg.endTime);
                timerCount = endTime - CSServerTime.Instance.TotalSeconds;
            }
        }
        ScriptBinder.InvokeRepeating(0, 1, ActivityTimerCount);

    }


    /// <summary>
    /// 活动倒计时。未开启时则是开启时间倒计时，开启时则是结束时间倒计时
    /// </summary>
    void ActivityTimerCount()
    {
        if (timerCount >= 0)
        {
            mlb_openTimer.text = $"{timerColorStr}{timerHintStr}{CSServerTime.Instance.FormatLongToTimeStr(timerCount, 2)}";
            timerCount--;
        }
        else
        {
            mlb_openTimer.text = "";
            ScriptBinder.StopInvokeRepeating();
        }
    }


    void JoinBtnClick(GameObject go)
    {
        if (!Utility.HasGuild())
        {
            UtilityTips.ShowRedTips(1949);
            return;
        }
        if (activityInfo == null || activityInfo.state == 0)
        {
            UtilityTips.ShowRedTips(1930);
            return;
        }

        CSGuildActivityInfo.Instance.TryToJoinCombat();
        UIManager.Instance.ClosePanel<UIGuildCombinedPanel>();
    }
}
