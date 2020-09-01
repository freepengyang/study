using System.Collections;
using System.Collections.Generic;
using unionbattle;
using UnityEngine;

public partial class UIGuildBossPanel : UIBasePanel
{
    UnionActivityInfo activityInfo;

    long timerCount;//倒计时秒
    string timerHintStr;
    string timerColorStr;

    TABLE.TIMER timerCfg;


    List<UIItemBase> items = new List<UIItemBase>();


    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "guild_boss");
        mClientEvent.AddEvent(CEvent.GuildActivityStateChange, ActivityInfoChange);
        mbtn_go.onClick = JoinBtnClick;

        mlb_activityEntrance.text = ClientTipsTableManager.Instance.GetClientTipsContext(1960);
        timerCfg = CSGuildActivityInfo.Instance.GuildBossTimer;
        if (timerCfg != null)
        {
            mlb_time.text = $"[cbb694]{ClientTipsTableManager.Instance.GetClientTipsContext(1959)}{timerCfg.desc}";
        }
        mlb_openTimer.text = "";

        var dic = CSGuildActivityInfo.Instance.GetBossRewardsDic();
        if (dic != null)
        {
            Utility.GetItemByBoxid(dic, mGrid, ref items, itemSize.Size60);
        }

        mGrid.CustomActive(false);
    }

    public override void Show()
    {
        base.Show();

        mGrid.CustomActive(true);

        ActivityInfoChange(0, null);
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        UIItemManager.Instance.RecycleItemsFormMediator(items);

        base.OnDestroy();
    }


    void ActivityInfoChange(uint id, object data)
    {
        mlb_guild.text = ClientTipsTableManager.Instance.GetClientTipsContext(1965);
        var info = CSGuildActivityInfo.Instance.LastCombatRank;
        if (info != null)
        {
            string guildInstanceLevel = mlb_guild.text;
            int rank = info.myRank;
            switch (rank)
            {
                case 0:
                    guildInstanceLevel = ClientTipsTableManager.Instance.GetClientTipsContext(1968);
                    break;
                case 1:
                    guildInstanceLevel = ClientTipsTableManager.Instance.GetClientTipsContext(1967);
                    break;
                case 2:
                    guildInstanceLevel = ClientTipsTableManager.Instance.GetClientTipsContext(1966);
                    break;
            }

            mlb_guild.text = guildInstanceLevel;
        }

        //倒计时
        activityInfo = CSGuildActivityInfo.Instance.GetActivityInfo((int)GuildActivityType.BOSS);
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

        CSGuildActivityInfo.Instance.TryToJoinBoss();
        UIManager.Instance.ClosePanel<UIGuildCombinedPanel>();
    }
}
