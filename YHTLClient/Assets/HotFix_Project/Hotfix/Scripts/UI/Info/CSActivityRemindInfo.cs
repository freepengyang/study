using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSActivityRemindInfo : CSInfo<CSActivityRemindInfo>
{
    ILBetterList<ActivityDataDisplay> activities = new ILBetterList<ActivityDataDisplay>(16);

    /// <summary>
    /// 常规的限时活动，该脚本自己管理，当前显示的气泡
    /// </summary>
    ILBetterList<ActivityDataDisplay> normalBubbleList = new ILBetterList<ActivityDataDisplay>(8);
    /// <summary>
    /// 特殊的活动，由外部传来，当前显示的气泡
    /// </summary>
    ILBetterList<ActivityDataDisplay> specialBubbleList = new ILBetterList<ActivityDataDisplay>(8);

    /// <summary>
    /// 即将开启的限时活动，也由该脚本管理
    /// </summary>
    ILBetterList<ActivityDataDisplay> openSoonBubbleList = new ILBetterList<ActivityDataDisplay>(8);


    /// <summary>
    /// 所有显示的气泡
    /// </summary>
    ILBetterList<ActivityDataDisplay> allShowBubbleList = new ILBetterList<ActivityDataDisplay>(8);

    List<int> notPopSummons = new List<int>();//本次登录不再提示的列表。跨天重置

    /// <summary> 进入副本后不提示的活动，key为活动id，value为副本id </summary>
    Dictionary<int, int> instanceActs = new Dictionary<int, int>();


    /// <summary>
    /// 当前所在副本id
    /// </summary>
    int curInstanceId;

    /// <summary>
    /// 开始前多少秒预告
    /// </summary>
    const int OpenRemindSeconds = 300;


    //public CSActivityRemindInfo()
    //{
    //    Init();
    //}


    public override void Dispose()
    {
        mClientEvent.RemoveEvent(CEvent.DailyActiveTaskChange, NeedCheckActivitis);
        mClientEvent.RemoveEvent(CEvent.MainPlayer_LevelChange, NeedCheckActivitis);
        mClientEvent.RemoveEvent(CEvent.GetEnterInstanceInfo, NeedCheckActivitis);
        mClientEvent.RemoveEvent(CEvent.LeaveInstance, NeedCheckActivitis);
        mClientEvent.RemoveEvent(CEvent.ResDayPassedMessage, DayPassed);

        normalBubbleList?.Clear();
        normalBubbleList = null;
        specialBubbleList?.Clear();
        specialBubbleList = null;
        openSoonBubbleList?.Clear();
        openSoonBubbleList = null;

        allShowBubbleList?.Clear();
        allShowBubbleList = null;

        notPopSummons?.Clear();
        notPopSummons = null;

        instanceActs?.Clear();
        instanceActs = null;

        activities?.Clear();
        activities = null;


    }



    public void Initialize()
    {
        mClientEvent.AddEvent(CEvent.DailyActiveTaskChange, NeedCheckActivitis);
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, NeedCheckActivitis);
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, NeedCheckActivitis);
        mClientEvent.AddEvent(CEvent.LeaveInstance, NeedCheckActivitis);
        mClientEvent.AddEvent(CEvent.ResDayPassedMessage, DayPassed);

        if (activities == null) activities = new ILBetterList<ActivityDataDisplay>();
        else activities.Clear();
        var dic = CSActivityInfo.Instance.GetTimeLimitBubbles();
        for (var it = dic.GetEnumerator(); it.MoveNext();)
        {
            activities.Add(it.Current.Value);
        }

        var map = CSActivityInfo.Instance.GetSpecialDic(false);
        for (var it = map.GetEnumerator(); it.MoveNext();)
        {
            activities.Add(it.Current.Value);
        }

        RegInstaceActs();

        CheckActivitis();
    }


    void RegInstaceActs()
    {
        if (activities == null) return;

        if (instanceActs == null) instanceActs = new Dictionary<int, int>();
        else instanceActs.Clear();

        for (int i = 0; i < activities.Count; i++)
        {
            ActivityDataDisplay display = activities[i];
            if (DisplayIsNull(display)) continue;
            int goal = display.config.goal;
            if (goal == 5 || goal == 6 || goal == 8)
            {
                int insId = 0;
                if (!int.TryParse(display.config.goalParam, out insId)) continue;
                instanceActs.Add(display.id, insId);
            }
        }
    }


    /// <summary>
    /// 只检测常规的限时活动
    /// </summary>
    public void CheckActivitis()
    {
        if (!CSConfigInfo.Instance.GetBool(ConfigOption.PushActivity)) return;//设置中的消息推送开关

        if (activities == null || activities.Count < 1) return;

        var nowSeconds = CSServerTime.Instance.TotalSeconds;
        int changedCount = 0;
        for (int i = 0; i < activities.Count; i++)
        {
            ActivityDataDisplay display = activities[i];
            if (DisplayIsNull(display)) continue;
            CSActivityInfo.Instance.CheckActivityState(display);
            if (display.state == ActivityState.CanJoin)
            {
                RemoveOpenSoonBubble(display);//从预告气泡中移除可不计入变动数量
                if (IsInActsInstance(display.id))
                {
                    if (RemoveNormalBubble(display))
                        changedCount++;
                    StopSummon(display);
                    continue;
                }
                display.PreviewType = 2;
                if (AddNormalBubble(display))
                    changedCount++;
                if (display.config.type  == 2)
                    PopSummon(display);
            }
            else if (display.state == ActivityState.OpenSoon && display.StartTimeFakeStamp - nowSeconds <= OpenRemindSeconds)
            {
                display.PreviewType = 1;
                if (AddOpenSoonBubble(display))
                    changedCount++;
            }
            else
            {
                display.PreviewType = 0;
                RemoveOpenSoonBubble(display);//从预告气泡中移除可不计入变动数量
                if (RemoveNormalBubble(display))
                    changedCount++;
                StopSummon(display);
            }
        }

        if (changedCount > 0)
            mClientEvent.SendEvent(CEvent.ActivityBubbleChange);
    }


    bool IsInActsInstance(int actId)
    {
        if (instanceActs == null || !instanceActs.ContainsKey(actId)) return false;
        var info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info == null) return false;
        curInstanceId = info.instanceId;
        return curInstanceId != 0 && instanceActs[actId] == curInstanceId && info.state < 3;
    }

    /// <summary>
    /// 增加常规可参与气泡，返回值为是否变化
    /// </summary>
    /// <param name="display"></param>
    bool AddNormalBubble(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || display.state != ActivityState.CanJoin) return false;
        if (normalBubbleList == null) normalBubbleList = new ILBetterList<ActivityDataDisplay>();

        if (!normalBubbleList.Contains(display))
        {
            normalBubbleList.Add(display);
            //mClientEvent.SendEvent(CEvent.ActivityBubbleAdded, display.id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 移除常规可参与气泡
    /// </summary>
    /// <param name="display"></param>
    bool RemoveNormalBubble(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || normalBubbleList == null || normalBubbleList.Count < 1) return false;

        return normalBubbleList.Remove(display);
    }

    /// <summary>
    /// 特殊可参与气泡
    /// </summary>
    /// <param name="actId"></param>
    public void AddSpecialBubble(int actId)
    {
        var display = CSActivityInfo.Instance.GetDataById(actId);
        if (DisplayIsNull(display)) return;
        if (!specialBubbleList.Contains(display))
        {
            CSActivityInfo.Instance.CheckActivityState(display);
            specialBubbleList.Add(display);
            mClientEvent.SendEvent(CEvent.ActivityBubbleAdded, display.id);
        }
    }


    public void RemoveSpecialBubble(int actId)
    {
        var display = CSActivityInfo.Instance.GetDataById(actId);
        if (DisplayIsNull(display)) return;
        if (specialBubbleList.Remove(display))
        {
            mClientEvent.SendEvent(CEvent.ActivityBubbleRemoved, display.id);
        }
    }


    public bool AddOpenSoonBubble(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || display.state != ActivityState.OpenSoon) return false;
        if (openSoonBubbleList == null) openSoonBubbleList = new ILBetterList<ActivityDataDisplay>();
        if (!openSoonBubbleList.Contains(display))
        {
            openSoonBubbleList.Add(display);
            //mClientEvent.SendEvent(CEvent.ActivityBubbleAdded, display.id);
            return true;
        }
        return false;
    }


    public bool RemoveOpenSoonBubble(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || openSoonBubbleList == null || openSoonBubbleList.Count < 1) return false;

        return openSoonBubbleList.Remove(display);
    }


    public void RemoveAllNormalBubbles()
    {
        if (normalBubbleList != null && normalBubbleList.Count > 0)
        {
            for (int i = 0; i < normalBubbleList.Count; i++)
            {
                mClientEvent.SendEvent(CEvent.ActivityBubbleRemoved, normalBubbleList[i].id);
            }
            normalBubbleList.Clear();
        }
    }


    #region Summons
    void PopSummon(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || display.state != ActivityState.CanJoin) return;
        if (notPopSummons == null) notPopSummons = new List<int>();
        if (!notPopSummons.Contains(display.id))
        {
            notPopSummons.Add(display.id);
            string desc = CSString.Format(1632, display.config.name);
            CSSummonMgr.Instance.ShowSummon(desc, (s, d) =>
            {
                if (s == (int)MsgBoxType.MBT_OK)
                {
                    JoinActivity(display);
                }
            }, SummonType.TimeLimitActivity, 8, display.id);
        }
    }


    void StopSummon(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display)) return;
        CSSummonMgr.Instance.StopSummon(SummonType.TimeLimitActivity, display.id);
    }



    void JoinActivity(ActivityDataDisplay display)
    {
        if (DisplayIsNull(display) || display.state != ActivityState.CanJoin) return;

        if (display.config.deliver != 0)
        {
            UtilityPath.FindWithDeliverId(display.config.deliver);
        }
        else if (display.config.uiModel != 0)
        {
            UtilityPanel.JumpToPanel(display.config.uiModel);
        }
    }
    #endregion


    bool DisplayIsNull(ActivityDataDisplay display)
    {
        return display == null || display.config == null;
    }



    void NeedCheckActivitis(uint id, object param)
    {
        CheckActivitis();
    }


    void DayPassed(uint id, object param)
    {
        if (notPopSummons != null) notPopSummons.Clear();
    }

    public ILBetterList<ActivityDataDisplay> GetShowBubbleList()
    {
        if (allShowBubbleList == null) allShowBubbleList = new ILBetterList<ActivityDataDisplay>();
        else allShowBubbleList.Clear();

        if (openSoonBubbleList != null) allShowBubbleList.AddRange(openSoonBubbleList);
        if (normalBubbleList != null) allShowBubbleList.AddRange(normalBubbleList);
        //if (specialBubbleList != null) allShowBubbleList.AddRange(specialBubbleList);

        return allShowBubbleList;
    }
}
