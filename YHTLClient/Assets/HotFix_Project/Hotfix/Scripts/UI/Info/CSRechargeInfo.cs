using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSRechargeInfo : CSInfo<CSRechargeInfo>
{

    List<CSRechargeData> curRechargeList = new List<CSRechargeData>();
    public List<CSRechargeData> CurRechargeList { get { return curRechargeList; } }

    Dictionary<int, int> buyTimesDic = new Dictionary<int, int>();


    Dictionary<int, int> curRewardBox = new Dictionary<int, int>();


    PoolHandleManager mPoolHandle = new PoolHandleManager();

    readonly int mAndroidNum = 8;
    readonly int mIOSNum = 8;

    readonly string monthRechargeRedPointPrefKey = "monthRechargeLastMonth";

    public bool monthRechargePanelOpened;


    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        curRechargeList?.Clear();
        curRechargeList = null;
        buyTimesDic?.Clear();
        buyTimesDic = null;
        curRewardBox?.Clear();
        curRewardBox = null;

    }

    
    /// <summary>
    /// 全部数据，目前是初始化时请求，每月1号重置时后端也会主动推
    /// </summary>
    /// <param name="msg"></param>
    public void SC_RechargeInfo(vip.ResMonthRechargeInfo msg)
    {
        if (buyTimesDic == null) buyTimesDic = new Dictionary<int, int>();
        var info = msg.monthRechagreInfo;
        for (int i = 0; i < info.Count; i++)
        {
            int id = info[i].id;
            buyTimesDic[id] = info[i].rewardFlag;
        }

        mClientEvent.SendEvent(CEvent.RechargeInfoUpdate);
        mClientEvent.SendEvent(CEvent.MonthRechargeRedPointCheck);
    }


    /// <summary>
    /// 充值响应。单条数据。用于奖励弹框
    /// </summary>
    public void SC_RechargeRes(vip.MonthRechargeInfo msg)
    {
        if (buyTimesDic == null) buyTimesDic = new Dictionary<int, int>();
        int oldTimes = 0;
        buyTimesDic.TryGetValue(msg.id, out oldTimes);

        buyTimesDic[msg.id] = msg.rewardFlag;

        if (curRewardBox == null) curRewardBox = new Dictionary<int, int>();
        else curRewardBox.Clear();
        TABLE.RECHARGE cfg = null;
        RechargeTableManager.Instance.TryGetValue(msg.id, out cfg);
        if (cfg != null)
        {
            if (cfg.bonusBox > 0 && oldTimes < 1)
            {
                BoxTableManager.Instance.GetBoxAwardById(cfg.bonusBox, curRewardBox);
            }
            int goldKey = (int)MoneyType.yuanbao;
            if (curRewardBox.ContainsKey(goldKey))
            {
                curRewardBox[goldKey] += (int)cfg.gold;
            }
            else curRewardBox.Add(goldKey, (int)cfg.gold);

            Utility.OpenGiftPrompt(curRewardBox);
        }

        mClientEvent.SendEvent(CEvent.RechargeInfoUpdate);
    }


    public void Init()
    {
        if (Platform.IsEditor)
        {
            GetDataByChannel(0);
            if (curRechargeList.Count > mAndroidNum)
            {
                curRechargeList.RemoveRange(mAndroidNum, curRechargeList.Count - mAndroidNum);
            }
        }
        else if (Platform.IsAndroid)
        {
            GetDataByChannel(0);
            if (curRechargeList.Count > mAndroidNum)
            {
                curRechargeList.RemoveRange(mAndroidNum, curRechargeList.Count - mAndroidNum);
            }
        }
        else if (Platform.IsIOS)
        {
            GetDataByChannel(1);
            if (curRechargeList.Count > mIOSNum)
            {
                curRechargeList.RemoveRange(mIOSNum, curRechargeList.Count - mIOSNum);
            }
        }

        curRechargeList.Sort((a, b) =>
        {
            if (a.Config != null && b.Config != null)
            {
                return a.Config.order - b.Config.order;
            }
            else return a.Id - b.Id;
        });

        Net.CSMonthRechargeInfoMessage();
    }


    void GetDataByChannel(int channel)
    {
        if (curRechargeList == null) curRechargeList = new List<CSRechargeData>();
        else curRechargeList.Clear();
        mPoolHandle.RecycleAll();

        var arr = RechargeTableManager.Instance.array.gItem.handles;
        if (arr == null) return;

        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.RECHARGE tbData = arr[k].Value as TABLE.RECHARGE;
            if (tbData == null || tbData.channelControl != channel) continue;
            bool isAdd = true;
            if (!string.IsNullOrEmpty(tbData.vip))
            {
                if (CSScene.IsLanuchMainPlayer)
                {
                    int vipLimit;
                    int intIsShow;
                    bool isShow;
                    string[] strs = tbData.vip.Split('#');

                    if (strs.Length >= 2 && int.TryParse(strs[0], out vipLimit) &&
                        int.TryParse(strs[1], out intIsShow))
                    {
                        isShow = intIsShow == 1;
                        if (CSMainPlayerInfo.Instance.VipLevel >= vipLimit && !isShow) isAdd = false;
                    }
                }
            }


            if (isAdd)
            {
                CSRechargeData data = mPoolHandle.GetCustomClass<CSRechargeData>();
                data.Init(tbData);
                curRechargeList.Add(data);
            }
        }
    }


    public void TryToRecharge(CSRechargeData data)
    {
        if (data == null || data.Config == null) return;

        TABLE.RECHARGE RechargeCfg = data.Config;
        TryToRecharge(RechargeCfg);
    }


    public void TryToRecharge(TABLE.RECHARGE RechargeCfg)
    {
        if (RechargeCfg == null) return;
        if (Platform.IsEditor)
        {
            int channel = RechargeCfg.channelControl;
            if (channel == 0 || channel == 1)
            {
                Net.CSMonthRechargeMessage(RechargeCfg.id);
            }
            else if (channel == 2 || channel == 3 || channel == 4 || channel == 5)
            {
                Net.GMCommand($"@recharge {RechargeCfg.id}");
            }
        }
        else if (Platform.IsAndroid)
        {
            /*if (QuDaoConstant.isEditorMode())
                return;*/

            if (CSConstant.mOnlyServerId > 0)
            {
                if (QuDaoConstant.GetPlatformData() != null &&
                    QuDaoConstant.GetPlatformData().payCode == PayCode.Special)
                    CSGame.Sington.StartCoroutine(CSRechargeMgr.Instance.queryPaySign(RechargeCfg)); //zhifu签名
                else
                    SDKUtility.Pay(CSRechargeMgr.Instance.payParams(RechargeCfg.money, RechargeCfg.id));
            }
            else
            {
                if (FNDebug.developerConsoleVisible) FNDebug.LogError("服务器ID有问题");
            }
        }
        else if (Platform.IsIOS)
        {
            FNDebug.LogError("@@@@mPlatformType:" + Platform.mPlatformType + ", IsIOS");
            if (CSConstant.mOnlyServerId > 0)
            {
                SDKUtility.Pay(CSRechargeMgr.Instance.IOSPayParams(RechargeCfg.money, RechargeCfg.id));
            }
        }
    }


    public int GetRechargeTimes(int id)
    {
        int times = 0;
        if (buyTimesDic == null) return times;
        buyTimesDic.TryGetValue(id, out times);
        return times;
    }


    public void MonthRechargePanelOpen()
    {
        if (!monthRechargePanelOpened)
        {
            monthRechargePanelOpened = true;
            var id = CSMainPlayerInfo.Instance.ID;
            PlayerPrefs.SetInt($"{id}{monthRechargeRedPointPrefKey}", CSServerTime.Now.Month);
            mClientEvent.SendEvent(CEvent.MonthRechargeRedPointCheck);
        }
    }

    public bool MonthRechargeRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_BiQiShop))
        {
            return false;
        }
        if (monthRechargePanelOpened) return false;
        var id = CSMainPlayerInfo.Instance.ID;
        int lastMonth = PlayerPrefs.GetInt($"{id}{monthRechargeRedPointPrefKey}");
        return lastMonth != CSServerTime.Now.Month;
    }
}



public class CSRechargeData : IDispose
{
    int _id;
    public int Id { get { return _id; } }

    TABLE.RECHARGE config;
    public TABLE.RECHARGE Config { get { return config; } }


    /// <summary>
    /// 用来缓存礼包内的物品。key为item.id， value为数量
    /// </summary>
    public Dictionary<int, int> rewardList;

    public void Dispose()
    {
        config = null;
        rewardList?.Clear();
        rewardList = null;
    }

    public void Init(TABLE.RECHARGE cfg)
    {
        if (cfg == null) return;
        config = cfg;
        _id = cfg.id;

        if (cfg.bonusBox > 0)
        {
            if (rewardList == null) rewardList = new Dictionary<int, int>();
            else rewardList.Clear();

            BoxTableManager.Instance.GetBoxAwardById(cfg.bonusBox, rewardList);
        }
    }
}