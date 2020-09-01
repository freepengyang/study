using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDreamLandInfo : CSInfo<CSDreamLandInfo>
{
    public CSDreamLandInfo()
    {
    }

    public override void Dispose()
    {
    }

    /// <summary>
    /// 幻境数据
    /// </summary>
    DreamLandData myDreamLandData = null;

    public DreamLandData MyDreamLandData
    {
        get { return myDreamLandData; }
    }

    /// <summary>
    /// 扣幻境时间定时器
    /// </summary>
    Schedule scheduleUseMyTime;

    /// <summary>
    /// 请求同步环境事件定时器
    /// </summary>
    Schedule scheduleReqMyTime;

    /// <summary>
    /// 是否打开强制离开副本倒计时界面
    /// </summary>
    bool isOpenCountDownLeavePanel = false;

    /// <summary>
    /// 计算幻境时间
    /// </summary>
    public void CalculateDreamLandTime()
    {
        if (myDreamLandData == null) return;
        scheduleUseMyTime = Timer.Instance.InvokeRepeating(0f, 1f, OnScheduleUseMyTime);
        scheduleReqMyTime = Timer.Instance.InvokeRepeating(0f, 10f, OnScheduleReqMyTime);
    }

    void OnScheduleUseMyTime(Schedule schedule)
    {
        long sec = myDreamLandData.myTime / 1000;
        long sec2 = myDreamLandData.endTime / 1000 - CSServerTime.Instance.TotalSeconds;
        if ((sec <= 60 || sec2 <= 60)
            && !isOpenCountDownLeavePanel)
        {
            // Utility.ShowCountDownLeavePanel(sec <= sec2 ? (int) sec : (int) sec2);
            UtilityTips.ShowExitInstanceCountDown(sec <= sec2 ? sec : sec2);
            isOpenCountDownLeavePanel = true;
        }

        myDreamLandData.myTime -= 1000;
    }

    void OnScheduleReqMyTime(Schedule schedule)
    {
        Net.CSHuanJingTimeMessage();
    }

    // /// <summary>
    // /// 是否有足够时间(5分钟)
    // /// </summary>
    // /// <returns></returns>
    // public bool HasEnoughTime()
    // {
    //     if (myDreamLandData == null) return false;
    //     return myDreamLandData.myTime >= 300000;
    // }

    /// <summary>
    /// 是否能够进入幻境
    /// </summary>
    /// <returns></returns>
    public bool IsEnterDreamLand()
    {
        if (myDreamLandData == null) return false;
        long openedTime = (CSServerTime.Instance.TotalMillisecond - myDreamLandData.startTime) / 3600000; //幻境已开放小时数
        int levelCondition = 0; //我的等级必须满足大于等于封印等级-XX级差的条件
        int timeCondition = 0; //我的幻境时间必须满足大于等于XX分钟的条件
        List<List<int>> listCondition =
            UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(256));
        for (int i = 0; i < listCondition.Count; i++)
        {
            if (i == listCondition.Count - 1)
            {
                if (openedTime >= listCondition[i][0])
                {
                    levelCondition = myDreamLandData.sealLevel - listCondition[i][1];
                    timeCondition = listCondition[i][2];
                }
            }
            else
            {
                if (openedTime >= listCondition[i][0] && openedTime < listCondition[i + 1][0])
                {
                    levelCondition = myDreamLandData.sealLevel - listCondition[i][1];
                    timeCondition = listCondition[i][2];
                }
            }
        }

        if (CSMainPlayerInfo.Instance.Level >= levelCondition && myDreamLandData.myTime / 60000 >= timeCondition &&
            myDreamLandData.myTime > 0)
        {
            return true;
        }

        return false;
    }

    #region 接收网络消息处理数据函数

    /// <summary>
    /// 处理获取幻境信息响应
    /// </summary>
    /// <param name="msg"></param>
    public void GetDreamLandInfo(fengyin.HuanJingOpen msg)
    {
        if (msg == null) return;
        if (myDreamLandData == null)
            myDreamLandData = new DreamLandData();
        myDreamLandData.startTime = msg.startTime;
        myDreamLandData.endTime = msg.endTime;
        // myDreamLandData.myTime = msg.time;
        myDreamLandData.sealLevel = msg.worldLeving;
        myDreamLandData.sealLeveled = msg.fengYinLevel;

        List<int> listMaps =
            UtilityMainMath.SplitStringToIntList(LevelSealTableManager.Instance.GetLevelSealMapid(msg.worldLeving));
        myDreamLandData.listMaps.Clear();
        for (int i = 0, max = listMaps.Count; i < max; i++)
        {
            int mapId = listMaps[i]; 
            if (mapId > 0)
                myDreamLandData.listMaps.Add(mapId);
        }
    }

    /// <summary>
    /// 更新幻境时间变化
    /// </summary>
    /// <param name=""></param>
    public void ChangeDreamLandTime(fengyin.HuanJingChange msg)
    {
        if (msg == null) return;
        if (myDreamLandData != null)
            myDreamLandData.myTime = msg.time;
    }

    /// <summary>
    /// 处理幻境关闭响应
    /// </summary>
    public void HandleDreamLand()
    {
        if (myDreamLandData != null)
        {
            myDreamLandData = null;
        }
    }

    /// <summary>
    /// 进入幻境
    /// </summary>
    public void EnterDreamLand(instance.InstanceInfo msg)
    {
        if (msg != null && myDreamLandData != null && myDreamLandData.listMaps != null &&
            myDreamLandData.listMaps.Count > 0 && msg.instanceId == myDreamLandData.listMaps[0])
        {
            CalculateDreamLandTime();
        }
    }

    /// <summary>
    /// 退出幻境
    /// </summary>
    public void ExitDreamLand()
    {
        if (Timer.Instance.IsInvoking(scheduleUseMyTime))
            Timer.Instance.CancelInvoke(scheduleUseMyTime);
        if (Timer.Instance.IsInvoking(scheduleReqMyTime))
            Timer.Instance.CancelInvoke(scheduleReqMyTime);
        isOpenCountDownLeavePanel = false;
    }

    #endregion
}

/// <summary>
/// 幻境开放期幻境相关数据
/// </summary>
public class DreamLandData
{
    /// <summary>
    /// 幻境开启时间
    /// </summary>
    public long startTime = 0;

    /// <summary>
    /// 幻境关闭时间(是可变的，封印期+配置的固定时间)
    /// </summary>
    public long endTime = 0;

    /// <summary>
    /// 我的可参与幻境时间
    /// </summary>
    public long myTime = 0;
    //-------------------以上单位均为毫秒

    /// <summary>
    /// 当前封印等级
    /// </summary>
    public int sealLevel = 0;

    /// <summary>
    /// 幻境地图列表
    /// </summary>
    public List<int> listMaps = new List<int>();


    /// <summary>
    /// 当前封印过的等级
    /// </summary>
    public int sealLeveled = 0;
}