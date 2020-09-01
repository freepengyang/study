using System.Collections;
using System.Collections.Generic;
using rank;
using UnityEngine;

public class CSSealGradeInfo : CSInfo<CSSealGradeInfo>
{
    public CSSealGradeInfo()
    {
    }

    public override void Dispose()
    {
    }


    SealGradeData mySealData = null;

    /// <summary>
    /// 封印期间我的封印数据
    /// </summary>
    public SealGradeData MySealData
    {
        get { return mySealData; }
    }

    int mySealLevel = 1; //封印等级（即使不在封印期，也需要保存最近的一个封印等级数据）

    /// <summary>
    /// 最近的世界封印等级
    /// </summary>
    public int MySealLevel
    {
        get { return mySealLevel; }
    }

    public bool IsSealTime
    {
        get { return (mySealData != null); }
    }

    /// <summary>
    /// 最近一次请求排行榜时间(单位:秒)
    /// </summary>
    private long reqTime = 0;

    public long ReqTime
    {
        get => reqTime;
        set
        {
            if (value > 0 && reqTime != value)
                reqTime = value;
        }
    }

    #region 接收网络消息处理数据函数

    /// <summary>
    /// 封印开启处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetFengYinOpenMessage(fengyin.FengYinOpen msg)
    {
        if (msg == null) return;
        if (mySealData == null)
        {
            TABLE.LEVELSEAL tableLevelSeal;
            if (LevelSealTableManager.Instance.TryGetValue(msg.level, out tableLevelSeal))
            {
                mySealData = new SealGradeData();
                mySealData.level = msg.level;
                mySealData.endTime = msg.endTime;
                mySealData.roleName = msg.roleName;
                mySealData.shortenCount = msg.shortenCount;
                mySealData.speedupTime =
                    msg.shortenCount < tableLevelSeal.xinFaNumber ? 0 : tableLevelSeal.shortenTime;
                mySealData.sealLeveled = msg.fengYinLevel;
                mySealLevel = msg.level;
            }
        }
    }

    /// <summary>
    /// 封印时间缩短处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetFengYinTimeShortenMessage(fengyin.FengYinOpen msg)
    {
        if (msg == null) return;
        if (mySealData != null)
        {
            TABLE.LEVELSEAL tableLevelSeal;
            if (LevelSealTableManager.Instance.TryGetValue(msg.level, out tableLevelSeal))
            {
                // mySealData = new SealGradeData();
                mySealData.level = msg.level;
                mySealData.endTime = msg.endTime;
                mySealData.roleName = msg.roleName;
                mySealData.shortenCount = msg.shortenCount;
                mySealData.speedupTime =
                    msg.shortenCount < tableLevelSeal.xinFaNumber ? 0 : tableLevelSeal.shortenTime;
                mySealData.sealLeveled = msg.fengYinLevel;
            }
        }
    }

    /// <summary>
    /// 封印结束处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetFengYinCloseMessage()
    {
        if (mySealData != null)
        {
            mySealData = null;
        }
    }

    #endregion


    int WorldLevel = 0;

    public void SetNowWorldLevel(int _lv)
    {
        WorldLevel = _lv;
    }

    public int GetNowWorldLevel()
    {
        return WorldLevel;
    }
}

/// <summary>
/// 封印期间相关数据类
/// </summary>
public class SealGradeData
{
    /// <summary>
    /// 封印等级
    /// </summary>
    public int level = 0;

    /// <summary>
    /// 结束时间
    /// </summary>
    public long endTime = 0;

    /// <summary>
    /// 触发封印人的名字
    /// </summary>
    public string roleName = "";

    /// <summary>
    /// 加速人数
    /// </summary>
    public int shortenCount = 0;

    /// <summary>
    /// 已加速天数
    /// </summary>
    public int speedupTime = 0;

    /// <summary>
    /// 已封印过的等级
    /// </summary>
    public int sealLeveled = 0;
}