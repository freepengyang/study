using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UIWeeklyCalendarPanel : UIBasePanel
{
    private UIGridContainer _mGrid_Col;
    private UIGridContainer mGrid_Col { get { return _mGrid_Col ?? (_mGrid_Col = Get<UIGridContainer>("center/view/Scroll View/Grid")); } }

    private Transform _obj_mask;
    private Transform obj_mask { get { return _obj_mask ?? (_obj_mask = Get("center/view/Days/mask")); } }


    /// <summary>
    /// key为开始和结束时间段, value的key为1到7，即周一到周日
    /// </summary>
    private Map<int, Map<int, TABLE.ACTIVE>> activitiesDic = new Map<int, Map<int, TABLE.ACTIVE>>();

    ILBetterList<int> sortedTimeKeys = new ILBetterList<int>();


    public override void Init()
    {
        base.Init();

        GetAllActivities();
    }


    public override void Show()
    {
        base.Show();

        RefreshUI();
    }

    

    void RefreshUI()
    {
        if (activitiesDic == null || sortedTimeKeys == null || activitiesDic.Count != sortedTimeKeys.Count) return;

        int day = CSServerTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)CSServerTime.Now.DayOfWeek;

        mPoolHandleManager.RecycleAll();
        mGrid_Col.MaxCount = activitiesDic.Count;
        int col = 0;
        
        for (int t = 0; t < sortedTimeKeys.Count; t++)
        {
            int key = sortedTimeKeys[t];
            Map<int, TABLE.ACTIVE> map;
            if (!activitiesDic.TryGetValue(key, out map)) continue;

            UIGridContainer grid_row = mGrid_Col.controlList[col].GetComponent<UIGridContainer>();
            grid_row.MaxCount = 8;
            for (int i = 0; i < grid_row.MaxCount; i++)
            {
                UIWeeklyCalendarItem item = mPoolHandleManager.GetCustomClass<UIWeeklyCalendarItem>();
                item.UIPrefab = grid_row.controlList[i];
                if (i == 0) item.InitItem(key);
                else
                {
                    if (map != null && map.ContainsKey(i) && map[i] != null) item.InitItem(map[i], i == day);
                    else item.InitItem(i == day);
                }
            }
            col++;
        }

        obj_mask.localPosition = new Vector2(115 * day, 0);
    }

    

    void GetAllActivities()
    {
        if (activitiesDic == null) activitiesDic = new Map<int, Map<int, TABLE.ACTIVE>>();
        else activitiesDic.Clear();

        if (sortedTimeKeys == null) sortedTimeKeys = new ILBetterList<int>();
        else sortedTimeKeys.Clear();

        var dic = CSActivityInfo.Instance.GetTimeLimitDic(false);
        for (dic.Begin(); dic.Next();)
        {
            var item = dic.Value;
            int weekInfo = item.openDayOfThisWeek;
            if (weekInfo == 0) continue;
            CheckActivities(item);
        }

        dic = CSActivityInfo.Instance.GetWeeklyDic(false);
        for (dic.Begin(); dic.Next();)
        {
            var item = dic.Value;
            int weekInfo = item.openDayOfThisWeek;
            if (weekInfo == 0) continue;
            CheckActivities(item);
        }

        sortedTimeKeys.Sort((a, b) =>
        {
            return a - b;
        });
    }


    void CheckActivities(ActivityDataDisplay item)
    {
        if (activitiesDic == null || sortedTimeKeys == null) return;

        int weekInfo = item.openDayOfThisWeek;
        if (weekInfo == 0) return;
        int startTime = item.StartTimeHour * 100 + item.StartTimeMinute;
        int endTime = item.EndTimeHour * 100 + item.EndTimeMinute;
        int key = startTime * 10000 + endTime;
        if (!activitiesDic.ContainsKey(key))
        {
            activitiesDic.Add(key, new Map<int, TABLE.ACTIVE>());
        }
        if (!sortedTimeKeys.Contains(key))
        {
            sortedTimeKeys.Add(key);
        }
        //策划暂定每个限时活动均为每天都开启(嗯，现在改了)，不同活动开始和结束时间也不会完全相同
        var map = activitiesDic[key];
        for (int i = 1; i < 8; i++)
        {
            if (!map.ContainsKey(i) || map[i] == null)
            {
                if ((weekInfo & (1 << i - 1)) != 0) map[i] = item.config;
                else map[i] = null;
            }            
        }
    }
}


public class UIWeeklyCalendarItem : UIBase, IDispose
{
    private UILabel _lb_name;
    private UILabel lb_name { get { return _lb_name ?? (_lb_name = Get<UILabel>("lb_name")); } }

    private GameObject _obj_mask;
    private GameObject obj_mask { get { return _obj_mask ?? (_obj_mask = Get<GameObject>("mask")); } }


    /// <summary>
    /// 显示时间段的
    /// </summary>
    /// <param name="timeInt"></param>
    public void InitItem(int timeInt)
    {
        int startTime = Mathf.FloorToInt(timeInt / 10000);
        int endTime = timeInt % 10000;
        int startH = Mathf.FloorToInt(startTime / 100);
        int startM = startTime % 100;
        int endH = Mathf.FloorToInt(endTime / 100);
        int endM = endTime % 100;

        string startStr = string.Format("{0}:{1}", startH < 10 ? $"0{startH}" : startH.ToString(), startM < 10 ? $"0{startM}" : startM.ToString());
        string endStr = string.Format("{0}:{1}", endH < 10 ? $"0{endH}" : endH.ToString(), endM < 10 ? $"0{endM}" : endM.ToString());

        lb_name.text = $"{startStr}-{endStr}".BBCode(ColorType.WeakText);
        obj_mask.SetActive(false);
    }


    /// <summary>
    /// 显示活动的
    /// </summary>
    /// <param name="activeCfg"></param>
    /// <param name="hightLight"></param>
    public void InitItem(TABLE.ACTIVE activeCfg, bool hightLight = false)
    {
        if (activeCfg == null)
        {
            lb_name.text = "";
            return;
        }
        lb_name.text = activeCfg.name.BBCode(ColorType.SecondaryText);
        obj_mask.SetActive(hightLight);
        UIEventListener.Get(UIPrefab, activeCfg).onClick = ItemOnClick;
    }


    /// <summary>
    /// 空的
    /// </summary>
    /// <param name="hightLight"></param>
    public void InitItem(bool hightLight)
    {
        lb_name.text = "";
        obj_mask.SetActive(hightLight);
    }
    

    void ItemOnClick(GameObject _go)
    {
        TABLE.ACTIVE active = (TABLE.ACTIVE)UIEventListener.Get(_go).parameter;
        UIManager.Instance.CreatePanel<UIActivityHalllTipsPanel>((f) =>
        {
            (f as UIActivityHalllTipsPanel).InitPanel(active);
        });
    }


    public override void Dispose()
    {
        _lb_name = null;
        _obj_mask = null;
        base.Dispose();
    }
}