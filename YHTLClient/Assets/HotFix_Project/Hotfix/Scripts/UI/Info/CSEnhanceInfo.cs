using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/*
部位         格子           装备部位Subtype(type==2)           部位(卧龙)   格子           装备部位(type==5)

武器          1               1                         武器          101               1           
衣服          2               2                         衣服          102               2
头盔          3               3                         头盔          103               3
项链          4               4                         项链          104               4
护腕(左)      5               5                         护腕(左)      105               5
护腕(右)      6               5                         护腕(右)      106               5
戒指(左)      7               6                         戒指(左)      107               6
戒指(右)      8               6                         戒指(右)      108               6
靴子          9               7                         靴子          109               7
腰带          10              8                         腰带          110               8
勋章          11              9                         勋章          111               9
宝石          12              10                        宝石          112               10
 **/

public class CSEnhanceInfo : CSInfo<CSEnhanceInfo>
{
    public readonly int[] equipPosIndex = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };//装备位id
    public readonly int[] subTypeIndex = { 1, 2, 3, 4, 5, 5, 6, 6, 7, 8, 9, 10 };//格子对应的装备子类型id

    const int EnhanceItemId = 50000020;
    const int CostMoneyId = 1;

    ILBetterList<EnhanceData> enhanceSortList;
    Map<int, EnhanceData> enhanceDatas;
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    /// <summary>
    /// 按所有装备位中的最低等级计算出的当前套装等级，套装等级最低三星（三级）*****
    /// 显示时若保护等级大于该等级则优先显示保护等级
    /// </summary>
    int normalSuitStarLv;
    public int NormalSuitStarLv { get { return normalSuitStarLv; } }

    /// <summary>
    /// 保护中的套装等级，0级则无
    /// </summary>
    int protectSuitStarLv;
    public int ProtectSuitStarLv { get { return protectSuitStarLv; } }
    /// <summary>
    /// 保护套装失效的时间戳
    /// </summary>
    long protectTimeStamp;
    public long ProtectTimeStamp { get { return protectTimeStamp; } }
    

    /// <summary>
    /// 实际生效的套装等级
    /// </summary>
    int realSuitStarLv;
    public int RealSuitStarLv { get { return realSuitStarLv; } }

    /// <summary>
    /// 达到当前等级的部位数量
    /// </summary>
    int satisfyCurLvCount;
    public int SatisfyCurLvCount { get { return satisfyCurLvCount; } }

    /// <summary>
    /// 达到下一级套装等级的部位数量
    /// </summary>
    int satisfyNextLvCount;
    public int SatisfyNextLvCount { get { return satisfyNextLvCount; } }


    bool enhanceFuncOpen;
    bool EnhanceFuncOpen
    {
        get
        {
            if (!enhanceFuncOpen)
            {
                enhanceFuncOpen = UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance);
            }
            return enhanceFuncOpen;
        }
    }


    static bool HasOpenEnhancePanel = false;


    public CSEnhanceInfo()
    {
        CalculateSuitStarLv();

        HasOpenEnhancePanel = false;
    }

    public override void Dispose()
    {
        enhanceDatas?.Clear();
        enhanceDatas = null;
        enhanceSortList?.Clear();
        enhanceSortList = null;
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;


    }


    public void OpenEnhancePanel()
    {
        if (!HasOpenEnhancePanel && ShowForgeEnhanceRedPoint())//如果第一次打开强化面板时不满足强化条件，红点没触发过，则不改变此变量
        {
            HasOpenEnhancePanel = true;
        }
        mClientEvent.SendEvent(CEvent.OpenEnhancePanel);
    }


    #region SCEvent
    /// <summary>
    /// 服务器推送的所有部位强化信息
    /// </summary>
    /// <param name="msg"></param>
    public void SCEnhanceInfo(intensify.IntensifyInfoResponse msg)
    {
        if (enhanceDatas == null) enhanceDatas = new Map<int, EnhanceData>();

        for (int i = 0; i < msg.intensifyInfos.Count; i++)
        {
            int pos = msg.intensifyInfos[i].position;
            EnhanceData data = null;
            if (!enhanceDatas.TryGetValue(pos, out data))
            {
                data = mPoolHandle.GetCustomClass<EnhanceData>();
                data.SetDatas(pos, GemSlotTableManager.Instance.GetGemSlotName(pos), GemSlotTableManager.Instance.GetGemSlotPic(pos));
                enhanceDatas[pos] = data;
            }
            data.SetLv(msg.intensifyInfos[i].intensifyLv);
        }
        CalculateSuitStarLv();
    }

    /// <summary>
    /// 强化结果响应
    /// </summary>
    /// <param name="msg"></param>
    public void EnhanceResponse(intensify.IntensifyResponse msg)
    {
        int pos = msg.intensifyInfo.position;
        int newLv = msg.intensifyInfo.intensifyLv;
        if (enhanceDatas == null) enhanceDatas = new Map<int, EnhanceData>();

        EnhanceData data;
        if (!enhanceDatas.TryGetValue(pos, out data))
        {
            data = mPoolHandle.GetCustomClass<EnhanceData>();
            data.SetDatas(pos, GemSlotTableManager.Instance.GetGemSlotName(pos), GemSlotTableManager.Instance.GetGemSlotPic(pos));
            enhanceDatas[pos] = data;
        }
        bool localResult = data.Lv < newLv;
        data.SetLv(newLv);

        CalculateSuitStarLv();

        //EventData eData = CSEventObjectManager.Instance.SetValue(pos, msg.result);
        mClientEvent.SendEvent(CEvent.EnhanceResponse, localResult);
        //CSEventObjectManager.Instance.Recycle(eData);
    }

    /// <summary>
    /// 服务端推送的套装信息
    /// </summary>
    /// <param name="msg"></param>
    public void SCSuitInfo(intensify.IntensifySuitInfoResponse msg)
    {
        protectSuitStarLv = msg.intensifySuitInfo.suitId;
        if (protectSuitStarLv < 3) return;
        protectTimeStamp = msg.intensifySuitInfo.endTime / 1000;

        CalculateSuitStarLv();
        mClientEvent.SendEvent(CEvent.SuitStarLvProtectStart);
    }
    #endregion

    

    /// <summary>
    /// 计算套装星级
    /// </summary>
    void CalculateSuitStarLv()
    {
        if (enhanceDatas == null) GetSortedDataList();

        normalSuitStarLv = 15;
        for (enhanceDatas.Begin(); enhanceDatas.Next();)
        {
            int lv = enhanceDatas.Value.Lv;
            if (lv < normalSuitStarLv) normalSuitStarLv = lv;
        }
        normalSuitStarLv = normalSuitStarLv > 15 ? 15 : normalSuitStarLv;

        int oldLv = realSuitStarLv;
        realSuitStarLv = normalSuitStarLv >= protectSuitStarLv ? normalSuitStarLv : protectSuitStarLv;

        //如果实际等级达到保护等级，由于此时服务器不会发送消息，则前端自己重置保护时间
        protectTimeStamp = normalSuitStarLv >= protectSuitStarLv ? 0 : protectTimeStamp;

        CalculateStarLvWearingCount();

        if (oldLv != realSuitStarLv)
        {
            //Debug.LogError("nor:" + normalSuitStarLv + ", pro:" + protectSuitStarLv + ", real:" + realSuitStarLv + ", old:" + oldLv);
            mClientEvent.SendEvent(CEvent.SuitStarLvChange, realSuitStarLv > oldLv && protectTimeStamp < CSServerTime.Instance.TotalSeconds);
        }
        mClientEvent.SendEvent(CEvent.EnhanceBtnRedPointCheck);
    }

    /// <summary>
    /// 计算满足当前等级和下一级的装备数量
    /// </summary>
    void CalculateStarLvWearingCount()
    {
        if (enhanceDatas == null || enhanceDatas.Count < 12) GetSortedDataList();

        satisfyCurLvCount = 0;
        satisfyNextLvCount = 0;
        for (enhanceDatas.Begin(); enhanceDatas.Next();)
        {
            int lv = enhanceDatas.Value.Lv;
            if (realSuitStarLv >= 3)
            {
                if (lv >= realSuitStarLv) satisfyCurLvCount++;
                if (lv > realSuitStarLv) satisfyNextLvCount++;
            }
            else
            {
                if (lv >= 3) satisfyCurLvCount++;
                if (lv > 3) satisfyNextLvCount++;
            }
                        
        }
    }


    public EnhanceData GetDataByPos(int pos)
    {
        if (enhanceDatas == null) return null;
        EnhanceData data = null;
        enhanceDatas.TryGetValue(pos, out data);
        return data;
    }


    public ILBetterList<EnhanceData> GetSortedDataList()
    {
        if (enhanceDatas == null)
        {
            enhanceDatas = new Map<int, EnhanceData>();
            for (int i = 0; i < equipPosIndex.Length; i++)
            {
                EnhanceData data;
                if (!enhanceDatas.TryGetValue(equipPosIndex[i], out data))
                {
                    data = mPoolHandle.GetCustomClass<EnhanceData>();
                    data.SetDatas(equipPosIndex[i], GemSlotTableManager.Instance.GetGemSlotName(equipPosIndex[i]), GemSlotTableManager.Instance.GetGemSlotPic(equipPosIndex[i]));
                    enhanceDatas[equipPosIndex[i]] = data;
                }
            }
        }

        if (enhanceSortList == null)
        {
            enhanceSortList = new ILBetterList<EnhanceData>(16);
            for (var it = enhanceDatas.GetEnumerator(); it.MoveNext();)
            {
                var data = it.Current.Value;
                enhanceSortList.Add(data);
            }
        }
        enhanceSortList.Sort((a, b) => { return a.Pos - b.Pos; });

        return enhanceSortList;
    }


    public int GetEnhanceLv(int pos)
    {
        if (enhanceDatas != null && enhanceDatas.ContainsKey(pos))
        {
            return enhanceDatas[pos].Lv;
        }

        return 0;
    }


    /// <summary>
    /// 获得单部位当前强化等级的万分比属性加成
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetEnhanceAddAttribute(int pos)
    {
        if (enhanceDatas != null && enhanceDatas.ContainsKey(pos))
        {
            return QianghuaTableManager.Instance.GetQianghuaQhAttr(enhanceDatas[pos].Lv + 1);
        }
        return 0;
    }


    public bool HasProtectSuitLv()
    {
        if (protectSuitStarLv >= realSuitStarLv && protectTimeStamp > CSServerTime.Instance.TotalSeconds) return true;
        return false;
    }
    
    public bool IsPosLvLowerThanSuitLv(int pos)
    {
        if (enhanceDatas == null || !enhanceDatas.ContainsKey(pos)) return false;

        return enhanceDatas[pos].Lv < realSuitStarLv;

    }


    public bool ShowForgeEnhanceRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance))
        {
            return false;
        }
        if (HasOpenEnhancePanel) return false;

        return CanEnhance();
    }


    public bool CanEnhance()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance))
        {
            return false;
        }
        if (enhanceDatas != null)
        {
            var moneyCount = CostMoneyId.GetItemCount();
            var itemCount = EnhanceItemId.GetItemCount();
            for (var it = enhanceDatas.GetEnumerator(); it.MoveNext();)
            {
                var data = it.Current.Value;
                if (data.Lv >= 15) continue;
                if (moneyCount >= data.CostMoney && itemCount >= data.CostItem) return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 某部位是否可强化
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool SinglePosCanEnhance(int pos)
    {
        if (!EnhanceFuncOpen)
        {
            return false;
        }
        if (enhanceDatas != null)
        {
            if (!enhanceDatas.ContainsKey(pos)) return false;
            var data = enhanceDatas[pos];
            return data.Lv < 15 && CostMoneyId.GetItemCount() >= data.CostMoney && EnhanceItemId.GetItemCount() >= data.CostItem;
        }

        return false;
    }


    /// <summary>
    /// 获取可强化的第一个部位，从小到大
    /// </summary>
    /// <returns></returns>
    public int GetFirstCanEnhancePos()
    {
        if (enhanceSortList == null)
        {
            GetSortedDataList();
        }
        for (int i = 0; i < enhanceSortList.Count; i++)
        {
            var data = enhanceSortList[i];
            if (SinglePosCanEnhance(data.Pos)) return data.Pos;
        }

        return 1;
    }
}


public class EnhanceData : IDispose
{
    private int _pos;
    /// <summary>
    /// 装备位置
    /// </summary>
    public int Pos { get { return _pos; } }

    private string _name;
    /// <summary>
    /// 部位名
    /// </summary>
    public string Name { get { return _name; } }

    private int _lv;
    /// <summary>
    /// 强化等级
    /// </summary>
    public int Lv { get { return _lv; } }


    public string IconName;


    #region Configs
    //以下部分均根据等级读表
    private int _costMoney;
    /// <summary>
    /// 消耗货币数量
    /// </summary>
    public int CostMoney { get { return _costMoney; } }

    private int _costItem;
    /// <summary>
    /// 消耗的强化道具数量(非额外增加概率道具和掉星保护道具)
    /// </summary>
    public int CostItem { get { return _costItem; } }

    private int _costAddOddsItem;
    /// <summary>
    /// 增加概率道具消耗数量
    /// </summary>
    public int CostAddOddsItem { get { return _costAddOddsItem; } }

    private int _costProtectItem;
    /// <summary>
    /// 增加概率道具消耗数量
    /// </summary>
    public int CostProtectItem { get { return _costProtectItem; } }

    private int _curAttr;
    /// <summary>
    /// 当前等级强化属性(百分比数,如50即50%)
    /// </summary>
    public int CurAttr { get { return _curAttr; } }

    private int _nextAttr;
    /// <summary>
    /// 下级强化属性
    /// </summary>
    public int NextAttr { get { return _nextAttr; } }

    private int _baseOdds;
    /// <summary>
    /// 基础成功率
    /// </summary>
    public int BaseOdds { get { return _baseOdds; } }

    private int _bonusOdds;
    /// <summary>
    /// 额外成功率
    /// </summary>
    public int BonusOdds { get { return _bonusOdds; } }

    #endregion
    

    public void Dispose()
    {
        
    }

    public void SetDatas(int pos, string name, string iconName = "", int lv = 0)
    {
        _pos = pos;
        _name = name;
        IconName = iconName;

        SetLv(lv);
    }

    public void SetLv(int lv)
    {
        _lv = lv;

        //这里处理获取配表中的材料货币消耗数值
        //此处取id采用等级直接+1的方式是因为前端只取library为1中的做显示
        TABLE.QIANGHUA cfg = null;
        if (QianghuaTableManager.Instance.TryGetValue(_lv + 1, out cfg))
        {
            _costMoney = GetCostItemCount(cfg.moneyCost);
            _costItem = GetCostItemCount(cfg.itemCost);
            _costAddOddsItem = GetCostItemCount(cfg.jingpoCost);
            _costProtectItem = GetCostItemCount(cfg.dingxingCost);

            _baseOdds = cfg.qhProba / 100;
            _bonusOdds = cfg.jingpoProba / 100;

            _curAttr = cfg.qhAttr / 100;
        }

        if (_lv < 15)
        {
            _nextAttr = QianghuaTableManager.Instance.GetQianghuaQhAttr(_lv + 2) / 100;
        }
        else
        {
            _nextAttr = 0;
        }
    }

    int GetCostItemCount(string str)
    {
        int count = 0;
        if (string.IsNullOrEmpty(str)) return count;
        string[] arr = str.Split('#');
        if (arr.Length > 1)
        {
            int.TryParse(arr[1], out count);
        }

        return count;
    }
}