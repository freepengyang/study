using activity;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TABLE;
using UnityEngine;

/// <summary>
/// 活动状态，枚举顺序即显示顺序
/// </summary>
public enum ActivityState
{
    /// <summary> 已开启,可参与</summary>
    CanJoin,
    /// <summary> 即将开启,可参与(仅限今日，非今日开启的不显示)</summary>
    OpenSoon,
    /// <summary> 不符合条件(等级不足等)</summary>
    MismatchCondition,
    /// <summary> 已结束未参与</summary>
    Missed,
    /// <summary>已参与</summary>
    Completed,
    /// <summary>非今日开启</summary>
    NotToday,
}




public class CSActivityInfo : CSInfo<CSActivityInfo>
{

    /// <summary>
    /// 所有活动总表
    /// </summary>
    private Map<int, ActivityDataDisplay> allDisplayDic;



    /// <summary>
    /// 常规活动。不限时，全天开放。对应配表中的type为1
    /// </summary>
    Map<int, ActivityDataDisplay> commonDic;


    /// <summary>
    /// 普通限时活动，每天开放。对应配表中的type为2
    /// </summary>
    Map<int, ActivityDataDisplay> timeLimitDic;


    /// <summary>
    /// 活跃度活动。对应配表中的type为3
    /// </summary>
    Map<int, ActivityDataDisplay> activeDic;


    /// <summary>
    /// 特殊活动。时间不定。配表中type>3
    /// </summary>
    Map<int, ActivityDataDisplay> specialDic;


    /// <summary>
    /// 周历专用(type > 100)，仅在周历中展示。现在只有101类型，为合并的行会战和行会首领活动展示
    /// </summary>
    Map<int, ActivityDataDisplay> weeklyDic;


    /// <summary>
    /// 需要气泡提示的限时活动。目前只有对应type为2
    /// </summary>
    Dictionary<int, ActivityDataDisplay> timeLimitBubbles; 



    private int active;
    //当前活跃度
    public int Active { get => active; set => active = value; }

    private RepeatedField<int> rewards;
    public RepeatedField<int> Rewards { get => rewards; set => rewards = value; }


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    #region UIString
    string isFinishText;
    public string IsFinishText { get { return isFinishText; } }
    string notOpenText;
    public string NotOpenText { get { return notOpenText; } }
    #endregion

    /// <summary>
    /// 沙巴克活动信息
    /// </summary>
    ActivityDataDisplay sabacDisplay;


    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        allDisplayDic?.Clear();
        allDisplayDic = null;
        commonDic?.Clear();
        commonDic = null;
        timeLimitDic?.Clear();
        timeLimitDic = null;
        activeDic?.Clear();
        activeDic = null;
        specialDic?.Clear();
        specialDic = null;

        timeLimitBubbles?.Clear();
        timeLimitBubbles = null;


    }


    public CSActivityInfo()
    {
        Init();
        if (string.IsNullOrEmpty(isFinishText))
        {
            isFinishText = ClientTipsTableManager.Instance.GetClientTipsContext(1625);
        }
        if (string.IsNullOrEmpty(notOpenText))
        {
            notOpenText = ClientTipsTableManager.Instance.GetClientTipsContext(1624);
        }
    }


    void Init()
    {
        mPoolHandle.RecycleAll();
        if (allDisplayDic == null) allDisplayDic = new Map<int, ActivityDataDisplay>(64);
        else allDisplayDic.Clear();
        if (commonDic == null) commonDic = new Map<int, ActivityDataDisplay>(16);
        else commonDic.Clear();
        if (timeLimitDic == null) timeLimitDic = new Map<int, ActivityDataDisplay>(16);
        else timeLimitDic.Clear();
        if (activeDic == null) activeDic = new Map<int, ActivityDataDisplay>(16);
        else activeDic.Clear();
        if (specialDic == null) specialDic = new Map<int, ActivityDataDisplay>(16);
        else specialDic.Clear();
        if (weeklyDic == null) weeklyDic = new Map<int, ActivityDataDisplay>(16);
        else weeklyDic.Clear();

        if (timeLimitBubbles == null) timeLimitBubbles = new Dictionary<int, ActivityDataDisplay>(16);
        else timeLimitBubbles.Clear();

        TABLE.ACTIVE cfg = null;
        var arr = ActiveTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            cfg = arr[i].Value as TABLE.ACTIVE;
            if (cfg == null) continue;
            
            //有开服天数配置的计算开服天数
            if (cfg.openday > 0 && CSMainPlayerInfo.Instance.ServerOpenDay < cfg.openday) continue;

            ActivityDataDisplay display = GetNewDisplay(cfg);

            int _id = cfg.id;
            int _type = cfg.type;
            if (_type == 1) commonDic.Add(_id, display);
            else if (_type == 2)
            {
                if (cfg.time == 0)
                {
                    FNDebug.LogError($"Active表格：ID{_id}的限时活动time字段为0，检查表格!!!");
                }
                else timeLimitDic.Add(_id, display);
                if (cfg.time != 0 && cfg.bulletin != 0)//bulletin为0的不提示
                {
                    timeLimitBubbles.Add(_id, display);
                }
            }
            else if (_type == 3) activeDic.Add(_id, display);
            else if (_type > 100) weeklyDic.Add(_id, display);
            else if (_type > 3)
            {
                specialDic.Add(_id, display);
                if (_type != 5) weeklyDic.Add(_id, display);//类型5的行会首领和行会战不直接显示在周历中
            }

            //沙巴克单独拿出来
            if (_type == 4) sabacDisplay = display;

            allDisplayDic.Add(_id, display);
        }
    }


    /// <summary>
    /// 玩家活动数据变更时调用
    /// </summary>
    /// <param name="msg"></param>
    public void SetDatas(activity.ActivityInfo msg)
    {
        if (allDisplayDic == null) return;

        if (msg.datas.Count < 1)//后端重置时发的是空数组
        {
            for (allDisplayDic.Begin(); allDisplayDic.Next();)
            {
                ActivityDataDisplay display = allDisplayDic.Value;
                if (display == null || display.config == null) continue;
                display.completeTimes = 0;
                //if (display.id == 12) display.completeTimes = -1;//世界boss特殊处理
                display.reward = 0;
                display.activityCount = 0;
            }
            mClientEvent.SendEvent(CEvent.DailyActiveTaskChange);
            return;
        }


        //数据变化，非重置
        for (int i = 0; i < msg.datas.Count; i++)
        {
            int id = msg.datas[i].activityId;
            ActivityDataDisplay display = null;
            if (!allDisplayDic.ContainsKey(id))
            {
                TABLE.ACTIVE cfg = null;
                if (ActiveTableManager.Instance.TryGetValue(id, out cfg))
                {
                    display = GetNewDisplay(cfg);
                    allDisplayDic.Add(id, display);
                }
            }
            else
            {
                display = allDisplayDic[id];
            }


            if (display != null)
            {
                display.completeTimes = msg.datas[i].count;
                display.reward = msg.datas[i].reward;
                display.activityCount = msg.datas[i].bonusCount;
                CheckActivityState(display);
            }
            
        }
        mClientEvent.SendEvent(CEvent.DailyActiveTaskChange);
    }


    /// <summary>
    /// 获取常规活动
    /// </summary>
    /// <param name="needCheckState">是否检查状态</param>
    /// <returns></returns>
    public Map<int, ActivityDataDisplay> GetCommonDic(bool needCheckState)
    {
        if (allDisplayDic == null || commonDic == null) Init();

        if (needCheckState)
        {
            for (commonDic.Begin(); commonDic.Next();)
            {
                ActivityDataDisplay display = commonDic.Value;
                if (display != null) CheckActivityState(display);
            }
        }

        return commonDic;
    }

    /// <summary>
    /// 获取普通限时活动
    /// </summary>
    /// <param name="needCheckState">是否检查状态</param>
    /// <returns></returns>
    public Map<int, ActivityDataDisplay> GetTimeLimitDic(bool needCheckState)
    {
        if (allDisplayDic == null || timeLimitDic == null) Init();

        if (needCheckState)
        {
            for (timeLimitDic.Begin(); timeLimitDic.Next();)
            {
                ActivityDataDisplay display = timeLimitDic.Value;
                if (display != null) CheckActivityState(display);
            }
        }

        return timeLimitDic;
    }

    /// <summary>
    /// 获取活跃度活动
    /// </summary>
    /// <param name="needCheckState">是否检查状态</param>
    /// <returns></returns>
    public Map<int, ActivityDataDisplay> GetActiveDic(bool needCheckState)
    {
        if (allDisplayDic == null || activeDic == null) Init();

        if (needCheckState)
        {
            for (activeDic.Begin(); activeDic.Next();)
            {
                ActivityDataDisplay display = activeDic.Value;
                if (display != null) CheckActivityState(display);
            }
        }

        return activeDic;
    }

    
    /// <summary>
    /// 获取特殊限时活动
    /// </summary>
    /// <param name="needCheckState"></param>
    /// <returns></returns>
    public Map<int, ActivityDataDisplay> GetSpecialDic(bool needCheckState)
    {
        if (allDisplayDic == null || specialDic == null) Init();

        for (specialDic.Begin(); specialDic.Next();)
        {
            ActivityDataDisplay display = specialDic.Value;
            if (display != null)
            {
                SetSpecialDisplayTimeInfo(display);
                if (needCheckState) CheckActivityState(display);
            }
        }

        return specialDic;
    }

    /// <summary>
    /// 获取周历专门展示的活动
    /// </summary>
    /// <param name="needCheckState"></param>
    /// <returns></returns>
    public Map<int, ActivityDataDisplay> GetWeeklyDic(bool needCheckState)
    {
        if (allDisplayDic == null || weeklyDic == null) Init();

        for (weeklyDic.Begin(); weeklyDic.Next();)
        {
            ActivityDataDisplay display = weeklyDic.Value;
            if (display != null)
            {
                SetSpecialDisplayTimeInfo(display);
                if (needCheckState) CheckActivityState(display);
            }
        }

        return weeklyDic;
    }


    /// <summary>
    /// 获取要气泡提示的限时活动
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, ActivityDataDisplay> GetTimeLimitBubbles()
    {
        if (allDisplayDic == null || timeLimitBubbles == null) Init();

        return timeLimitBubbles;
    }



    ActivityDataDisplay GetNewDisplay(TABLE.ACTIVE cfg)
    {
        if (cfg == null) return null;
        ActivityDataDisplay display = mPoolHandle.GetCustomClass<ActivityDataDisplay>();
        display.Init(cfg);
        SetDisplayStartAndEndTimeInfo(display);

        return display;
    }


    /// <summary>
    /// 检测活动状态。检测前需保证活动的时间信息已正确设定
    /// </summary>
    /// <param name="display"></param>
    public void CheckActivityState(ActivityDataDisplay display)
    {
        TABLE.ACTIVE cfg = display.config;
        if (cfg == null) return;

        //检测是否符合等级条件
        if (CSMainPlayerInfo.Instance.Level < cfg.level)
        {
            display.state = ActivityState.MismatchCondition;
            return;
        }

        DateTime curDate = CSServerTime.Now;
        if (cfg.type == 4 || cfg.type == 5)//沙巴克，行会战和行会首领
        {
            //判断今天是否开放
            //DateTime actDate = CSServerTime.StampToDateTime(display.StartTimeStamp * 1000);
            //if (curDate.Day != actDate.Day)
            //{
            //    display.state = ActivityState.NotToday;
            //    return;
            //}
            int day = (int)CSServerTime.Now.DayOfWeek;
            day = day == 0 ? 7 : day;
            if ((display.openDayOfThisWeek & (1 << day - 1)) == 0)
            {
                display.state = ActivityState.NotToday;
                return;
            }
        }

        //检测是否参与
        if (cfg.type == 3)
        {
            if (cfg.bonusCount > 0 && display.activityCount >= cfg.bonusCount /*&& display.reward == 1*/)
            {
                display.state = ActivityState.Completed;
                return;
            }
        }
        else if (cfg.type == 2 || cfg.type == 1)//现在限时活动也有已完成未完成状态，如果次数达到上限且isContinue字段为0则不可再进入
        {
            if (cfg.count > 0 && display.completeTimes >= cfg.count && cfg.isContinue == 0)
            {
                display.state = ActivityState.Completed;
                return;
            }
        }

        //判断开启时间
        if (cfg.time != 0)
        {
            int nowTime = curDate.Hour * 100 + curDate.Minute;
            int startTime = display.StartTimeHour * 100 + display.StartTimeMinute;
            int endTime = display.EndTimeHour * 100 + display.EndTimeMinute;
            if (nowTime < startTime)
            {
                display.state = ActivityState.OpenSoon;
                return;
            }
            if (nowTime >= endTime)
            {
                display.state = ActivityState.Missed;
                return;
            }

            //暂时只有世界boss这么特殊处理
            if (display.completeTimes < 0)
            {
                display.state = ActivityState.Missed;//boss被人打死且自己未参与，算作已结束
                return;
            }
        }

        display.state = ActivityState.CanJoin;
        
    }


    /// <summary>
    /// 普通的限时活动时间相关信息处理
    /// </summary>
    /// <param name="display"></param>
    void SetDisplayStartAndEndTimeInfo(ActivityDataDisplay display)
    {
        if (display.config == null || display.config.time == 0 || display.config.type > 3 || display.timeInfoIsSet) return;

        TABLE.TIMER timerCfg = null;
        if (!TimerTableManager.Instance.TryGetValue(display.config.time, out timerCfg)) return;
        var timeNow = CSServerTime.Now;

        string startCron = timerCfg.startTime;
        if (!string.IsNullOrEmpty(startCron))
        {
            UtilityMath.CronTimeStringParseToHMS(startCron, out display.StartTimeHour, out display.StartTimeMinute, out display.StartTimeSecond);
        }

        string endCron = timerCfg.endTime;
        if (!string.IsNullOrEmpty(endCron))
        {
            UtilityMath.CronTimeStringParseToHMS(endCron, out display.EndTimeHour, out display.EndTimeMinute, out display.EndTimeSecond);
        }

        display.timeInfoIsSet = true;
    }


    /// <summary>
    /// 特殊的限时活动，非每天开放。需要处理是周几开放。暂时只有沙城活动(配表中的type==4)
    /// </summary>
    /// <param name="display"></param>
    void SetSpecialDisplayTimeInfo(ActivityDataDisplay display)
    {
        if (display.config == null || display.config.time == 0 || display.timeInfoIsSet) return;

        int _type = display.config.type;
        if (_type == 4)//沙巴克
        {
            display.openDayOfThisWeek = 0b_000_0000;
            display.StartTimeStamp = 0;
            var list = CSGuildFightManager.Instance.GetFightItemDatas();
            if (list == null) return;

            long startTime = 0;
            long endTime = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].fightActivity == null) continue;
                bool isThisWeek = CSServerTime.Instance.IsWeekByMinusCurTime(list[i].fightActivity.startTime);
                if (!isThisWeek) continue;

                long st = list[i].fightActivity.startTime;
                DayOfWeek week = CSServerTime.StampToDateTime(st).DayOfWeek;
                int weekInt = week == 0 ? 7 : (int)week;
                display.openDayOfThisWeek = display.openDayOfThisWeek | (1 << weekInt - 1);

                var tempOld = Mathf.Abs(startTime - CSServerTime.Instance.TotalMillisecond);
                var tempNew = Mathf.Abs(st - CSServerTime.Instance.TotalMillisecond);
                if (tempNew < tempOld)
                {
                    startTime = list[i].fightActivity.startTime;
                    endTime = list[i].fightActivity.endTime;
                }
            }

            display.StartTimeStamp = startTime / 1000;
            display.EndTimeStamp = endTime / 1000;

            DateTime dt = CSServerTime.StampToDateTime(startTime);
            display.StartTimeHour = dt.Hour;
            display.StartTimeMinute = dt.Minute;
            display.StartTimeSecond = dt.Second;

            dt = CSServerTime.StampToDateTime(endTime);
            display.EndTimeHour = dt.Hour;
            display.EndTimeMinute = dt.Minute;
            display.EndTimeSecond = dt.Second;

            display.timeInfoIsSet = true;
        }
        else if(_type == 5 || _type == 101)//行会战和行会boss。这两个活动本周开放时间要根据沙巴克来算
        {
            if (sabacDisplay == null) return;
            if (!sabacDisplay.timeInfoIsSet)
            {
                SetSpecialDisplayTimeInfo(sabacDisplay);
            }
            display.openDayOfThisWeek = ~sabacDisplay.openDayOfThisWeek;
            TABLE.TIMER timerCfg = null;
            if (!TimerTableManager.Instance.TryGetValue(display.config.time, out timerCfg)) return;
            var timeNow = CSServerTime.Now;

            string startCron = timerCfg.startTime;
            if (!string.IsNullOrEmpty(startCron))
            {
                UtilityMath.CronTimeStringParseToHMS(startCron, out display.StartTimeHour, out display.StartTimeMinute, out display.StartTimeSecond);
            }

            string endCron = timerCfg.endTime;
            if (!string.IsNullOrEmpty(endCron))
            {
                UtilityMath.CronTimeStringParseToHMS(endCron, out display.EndTimeHour, out display.EndTimeMinute, out display.EndTimeSecond);
            }

            display.timeInfoIsSet = true;
        }
    }


    public ActivityDataDisplay GetDataById(int id)
    {
        if (allDisplayDic == null) return null;

        ActivityDataDisplay data = null;
        if(allDisplayDic.TryGetValue(id, out data))
        {
            if (data.config != null &&( data.config.type == 4 || data.config.type == 5))
            {
                SetSpecialDisplayTimeInfo(data);
            }
        }
        return data;
    }


    /// <summary>
    /// 刷新所有常规限时活动的开始和结束时间戳
    /// </summary>
    public void RefreshAllCommonTimeInfo()
    {
        if (timeLimitDic == null || timeLimitDic.Count < 1) return;
        var timeNow = CSServerTime.Now;
        for (timeLimitDic.Begin(); timeLimitDic.Next();)
        {
            ActivityDataDisplay display = timeLimitDic.Value;
            if (display == null || display.config == null || display.config.type > 3 || display.config.time == 0) continue;
            DateTime dt = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, display.StartTimeHour, display.StartTimeMinute, display.StartTimeSecond);
            display.StartTimeStamp = CSServerTime.DateTimeToStamp(dt);
            dt = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, display.EndTimeHour, display.EndTimeMinute, display.EndTimeSecond);
            display.EndTimeStamp = CSServerTime.DateTimeToStamp(dt);
        }
    }



    #region 活跃度相关

    /// <summary>
    /// 活跃度值变化
    /// </summary>
    /// <param name="resActive"></param>
    public void SetActive(activity.ResActive resActive) {
        Active = resActive.active;
        mClientEvent.SendEvent(CEvent.SCResActiveMessageRefresh);
        mClientEvent.SendEvent(CEvent.DailyBtnRedPointCheck);
    }

    public void SetReceivedActive(activity.ResActiveReward resActiveReward) {
        //Debug.Log(rewards[0]);
        rewards = resActiveReward.rewards;
        mClientEvent.SendEvent(CEvent.SCResActiveMessageRefresh);
        mClientEvent.SendEvent(CEvent.DailyBtnRedPointCheck);
    }

    public bool CheckActiveGet() {
        var arr = ActiveRewardTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.ACTIVEREWARD;
            if (Active >= int.Parse(item.num)&& !rewards.Contains(item.id))
            {
                return true;
            }
        }

        return false;

    }
    #endregion

    #region 七日试炼数据
    Map<int, CSBetterLisHot<GoalDatas>> SevenDayMap = new Map<int, CSBetterLisHot<GoalDatas>>();
    
    //七日试炼数据

    activity.SevenDayData sevenDayData = null;

    public SevenDayData SevenDayData { get => sevenDayData;
        set
        {
            sevenDayData = value;
            mClientEvent.SendEvent(CEvent.SevenDayDataChange);
        }
    }

    
    
    
    /// <summary>
    /// 获取七日试炼开启天数
    /// </summary>
    /// <returns></returns>
    public int GetSevenDayTime()
    {
        
        if (sevenDayData == null)
            return 0;
        
        return CSServerTime.Instance.GetDayByMinusCurTime(sevenDayData.openTime) + 1;
    }

    public bool IsOpenTab(int curTab) {
        return GetSevenDayTime() > curTab;
    }

    /// <summary>
    /// 判断分页下的任务是否全部完成
    /// </summary>
    public bool IsFinishByType(int group) {
        var datas = NewbieActivityTableManager.Instance.GetDatabyGroup(group);
        //sevenDayData.datas

        var iter = sevenDayData.datas.GetEnumerator();
        int num = 0;
        
        while (iter.MoveNext())
        {
            int curgroup = NewbieActivityTableManager.Instance.GetNewbieActivityGroup(iter.Current.configId);
            
            if (curgroup == group)
            {
                int count = NewbieActivityTableManager.Instance.GetNewbieActivityCount(iter.Current.configId);
                //int showType = NewbieActivityTableManager.Instance.GetNewbieActivityShowType(iter.Current.configId);
                // if (showType == 2 && iter.Current.value == 1)
                //     num++;
                // if (showType == 1 && iter.Current.value >= NewbieActivityTableManager.Instance.
                //     GetNewbieActivityRequirePara1(iter.Current.configId))
                //     num++;
                if (iter.Current.value >= count)
                    num++;
            }
        }

        if (num > 0 && num == datas.Count)
            return true;
        else 
            return false;
    }

    /// <summary>
    /// 判断是否有任务没有完成且没有领取奖励
    /// </summary>
    /// <returns></returns>
    public bool IsHaveFinishByType(int group)
    {
        if (sevenDayData == null)
        {
            return false;
        }
        var iter = sevenDayData.datas.GetEnumerator();
        //int num = 0;
        while (iter.MoveNext())
        {
            int cfgid = iter.Current.configId;
            int curgroup = NewbieActivityTableManager.Instance.GetNewbieActivityGroup(cfgid);
            if (curgroup == group)
            {
                int count = NewbieActivityTableManager.Instance.GetNewbieActivityCount(iter.Current.configId);
                
                if (iter.Current.value >= count && iter.Current.reward != 1)
                    return true;
                    
            }
        }

        return false;
    }


    public Map<int, CSBetterLisHot<GoalDatas>> GetSevenMapData() {
        if (SevenDayMap.Count<=0)
        {
            //初始化SevenMapData
            for (int i = 1; i <= 7; i++)
            {
                CSBetterLisHot<GoalDatas> Goaldatas = new CSBetterLisHot<GoalDatas>();
                var NewbieGroup = NewbieActivityTableManager.Instance.GetDatabyGroup(i);
                //Debug.Log("NewbieGroup" + NewbieGroup.Count);
                //Debug.Log("NewbieGroup" + NewbieGroup.Count);
                for (NewbieGroup.Begin(); NewbieGroup.Next();)
                {
                    GoalDatas data = new GoalDatas();
                    data.configId = NewbieGroup.Value.id;
                    data.reward = 0;
                    data.value = 0;
                    Goaldatas.Add(data);
                }
                SevenDayMap.Add(i, Goaldatas);
            }
        }
        
        var iter = SevenDayData.datas.GetEnumerator();
        while (iter.MoveNext()) {
            int group = NewbieActivityTableManager.Instance.GetNewbieActivityGroup(iter.Current.configId);
            GoalDatas data = SevenDayMap[group].FirstOrNull(x => (int)x.configId == iter.Current.configId);
            //SevenDayMap[group].
            //SevenDayMap[group] = iter.Current;
            data.reward = iter.Current.reward;
            data.value = iter.Current.value;
            //data = iter.Current;    
        }

        
        for (SevenDayMap.Begin();SevenDayMap.Next();)
        {
            SevenDayMap.Value.Sort((x, y) =>
            {
                return SevenDayMapSort(x.reward,y.reward);
            });
            //var goalDatases = SevenDayMap.Value;
            //CSBetterLisHot
        }
        
        //SevenDayMap.sort
        
        //排序
        
        return SevenDayMap;
    }

    public int SevenDayMapSort(int reward1, int reward2)
    {
        switch (reward1)
        {
            case 2 :
                return -1;
            case 0 :
                if (reward2 == 2)
                    return 1;
                else
                    return -1;
            case 1 :
                if (reward2 == 2 || reward2 == 0)
                    return 1;
                else
                    return -1;
            default:
                return -1;
        }
    }

    public bool SevenDayRedPointChenk()
    {
        if (sevenDayData == null)
        {
            return false;
        }
        
        for (int i = 1; i <= 7; i++)
        {
            if (IsHaveFinishByType(i))
                return true;
        }
        var arr = NewbieActivityScheduleTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i<max;++i)
        {
            int score = sevenDayData.score;
            if (!sevenDayData.scoreRewards.Contains(arr[i].key) &&  score >= (arr[i].Value as TABLE.NEWBIEACTIVITYSCHEDULE).requiresSore)
            {
                return true;
            }
        }
        
        
        return false;
    }    

    #endregion
}

public class ActivityDataDisplay : IDispose
{
    private int _id;
    public int id { get { return _id; } }

    private TABLE.ACTIVE _config;
    public TABLE.ACTIVE config { get { return _config; } }

    /// <summary>
    /// 完成次数（如果小于0则为提前结束，，暂时只有世界boss这么特殊处理）
    /// </summary>
    public int completeTimes;

    /// <summary>
    /// 额外加成次数
    /// </summary>
    public int extraCount;

    /// <summary>
    /// 活跃度完成次数
    /// </summary>
    public int activityCount;

    /// <summary>
    /// 领奖状态，0未领奖，1已领奖
    /// </summary>
    public int reward;
    /// <summary>
    /// 活动状态
    /// </summary>
    public ActivityState state;

    //开启时间时分秒
    public int StartTimeHour;
    public int StartTimeMinute;
    public int StartTimeSecond;
    /// <summary> 时间戳(秒)暂时只有沙巴克用 </summary>
    public long StartTimeStamp;

    //结束时间
    public int EndTimeHour;
    public int EndTimeMinute;
    public int EndTimeSecond;

    /// <summary> 时间戳(秒)沙巴克用</summary>
    public long EndTimeStamp;


    /// <summary>
    /// 主界面预告类型，0不显示，1开始预告，2结束预告
    /// </summary>
    public int PreviewType;


    /// <summary>
    /// 主界面活动开启倒计时用
    /// </summary>
    public long StartTimeFakeStamp
    {
        get
        {
            DateTime dateTime = CSServerTime.Instance.ServerNows;
            DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, StartTimeHour, StartTimeMinute, StartTimeSecond);

            return CSServerTime.DateTimeToStamp(dateTime2);
        }
    }



    /// <summary>
    /// 主界面活动结束倒计时用
    /// </summary>
    public long FinishTimeFakeStamp
    {
        get
        {
            DateTime dateTime = CSServerTime.Instance.ServerNows;
            DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, EndTimeHour, EndTimeMinute, EndTimeSecond);

            return CSServerTime.DateTimeToStamp(dateTime2);
        }
    }



    /// <summary>
    /// 本周周几开放
    /// </summary>
    public int openDayOfThisWeek = 0b_111_1111;


    /// <summary>
    /// 时间信息已设置
    /// </summary>
    public bool timeInfoIsSet;


    public void Dispose()
    {
        _config = null;
    }


    public void Init(TABLE.ACTIVE cfg, activity.ActivityData data = null)
    {
        if (cfg == null) return;
        _id = cfg.id;
        _config = cfg;

        if (data != null)
        {
            completeTimes = data.count;
            reward = data.reward;
            activityCount = data.bonusCount;
        }
        else
        {
            completeTimes = 0;
            reward = 0;
            activityCount = 0;
        }

        state = ActivityState.NotToday;

        InitTimeInfo();
    }


    public void InitTimeInfo()
    {
        StartTimeHour = 0;
        StartTimeMinute = 0;
        StartTimeSecond = 0;
        StartTimeStamp = 0;

        EndTimeHour = 0;
        EndTimeMinute = 0;
        EndTimeSecond = 0;
        EndTimeStamp = 0;

        openDayOfThisWeek = 0b_111_1111;

        timeInfoIsSet = false;
    }

}