using activity;
using dailypurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSArmRaceInfo : CSInfo<CSArmRaceInfo>
{
    PoolHandleManager Pool = new PoolHandleManager();

    public override void Dispose()
    {
        finifhedList.Clear();
        finifhedList = null;
        reachedlist.Clear();
        reachedlist = null;
        todolist.Clear();
        todolist = null;

        Pool.RecycleAll();
        Pool = null;

        mClientEvent.UnRegAll();
    }
    #region
    EquipCompetition armData;
    #endregion
    int funcType = 33;
    public void Init()
    {
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, GetFunctionOpenStateChange);
        if (UICheckManager.Instance.DoCheckFunction(funcType))
        {
            Net.CSEquipCompetitionDataMessage();
        }
    }
    void GetFunctionOpenStateChange(uint id, object data)
    {
        if ((int)data == funcType)
        {
            mClientEvent.UnReg(CEvent.FunctionOpenStateChange, GetFunctionOpenStateChange);
            Net.CSEquipCompetitionDataMessage();
        }
    }
    public void GetArmRaceDataBack(EquipCompetition _data)
    {
        armData = _data;
    }
    public EquipCompetition ReturnArmData()
    {
        return armData;
    }
    #region  军备礼包
    Dictionary<int, GiftBuyInfo> buyInfoDic = new Dictionary<int, GiftBuyInfo>();
    public void GetAllArmGiftInfo(DailyPurchaseInfoResponse _res)
    {
        buyInfoDic.Clear();
        for (int i = 0; i < _res.giftBuyInfos.Count; i++)
        {
            buyInfoDic.Add(_res.giftBuyInfos[i].giftId, _res.giftBuyInfos[i]);
        }
    }
    public void GetBuyArmGiftInfo(GiftBuyInfo _buyInfo)
    {
        if (buyInfoDic.ContainsKey(_buyInfo.giftId))
        {
            buyInfoDic[_buyInfo.giftId] = _buyInfo;
        }
    }
    public GiftBuyInfo GetGiftInfoById(int _id)
    {
        if (buyInfoDic.ContainsKey(_id))
        {
            return buyInfoDic[_id];
        }
        return null;
    }
    #endregion
    public int ReturnCurrentGroup()
    {
        if (armData != null) { return armData.curGroup; }
        return 0;
    }
    /// <summary>
    /// 是否已领取
    /// </summary>
    /// <param name="_gId"></param>
    /// <returns></returns>
    public bool ReturnCurrentGroup(int _gId)
    {
        if (armData != null) { return armData.groupRewards.Contains(_gId); }
        return false;
    }

    List<GoalDatas> finifhedList = new List<GoalDatas>();
    List<GoalDatas> reachedlist = new List<GoalDatas>();
    List<GoalDatas> todolist = new List<GoalDatas>();
    public void GetCurrentGroupDatas(int _gId, List<GoalDatas> _list)
    {
        _list.Clear();
        finifhedList.Clear();
        reachedlist.Clear();
        todolist.Clear();
        if (armData != null)
        {
            for (int i = 0; i < armData.datas.Count; i++)
            {
                int cfgId = armData.datas[i].configId;
                if (ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskGroup(cfgId) == _gId)
                {
                    if (armData.datas[i].reward == 1)
                    {
                        finifhedList.Add(armData.datas[i]);
                        continue;
                    }
                    if (ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskShowType(cfgId) == 1)
                    {
                        if (armData.datas[i].value == ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskCount(cfgId))
                        {
                            reachedlist.Add(armData.datas[i]);
                            continue;
                        }
                        else
                        {
                            todolist.Add(armData.datas[i]);
                            continue;
                        }
                    }
                    else
                    {
                        if (armData.datas[i].value == 1)
                        {
                            reachedlist.Add(armData.datas[i]);
                            continue;
                        }
                        else
                        {
                            todolist.Add(armData.datas[i]);
                            continue;
                        }
                    }
                }
            }
        }
        _list.AddRange(finifhedList);
        _list.AddRange(todolist);
        _list.AddRange(reachedlist);
    }
    Dictionary<int, GoalDatas> CurrentGroupDic = new Dictionary<int, GoalDatas>();
    public void GetCurrentGroupDicDatas(int _gId, Dictionary<int, GoalDatas> _CurrentGroupDic)
    {
        if (_CurrentGroupDic == null)
        {
            return;
        }
        _CurrentGroupDic.Clear();
        if (armData != null)
        {
            for (int i = 0; i < armData.datas.Count; i++)
            {
                GoalDatas tempdata = armData.datas[i];
                //Debug.Log($"{tempdata.configId}   {tempdata.value}   {tempdata.reward}");
                if (ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskGroup(tempdata.configId) == _gId)
                {
                    _CurrentGroupDic.Add(tempdata.configId, tempdata);
                }
            }
        }
    }

    List<TABLE.ARMSRACETASK> Redpointlist = new List<TABLE.ARMSRACETASK>();
    public bool GetArmRaceRedPoint()
    {
        if (armData == null)
        {
            return false;
        }
        //四个阶段奖励领完后返回false
        if (armData.groupRewards.Count == 4)
        {
            return false;
        }
        else
        {
            int num = 0;
            Redpointlist.Clear();
            ArmsRaceTaskTableManager.Instance.GetTasksByGroup(armData.curGroup, Redpointlist);
            //判断当前阶段是否有可领的奖励
            GetCurrentGroupDicDatas(armData.curGroup, CurrentGroupDic);
            var iter = CurrentGroupDic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.reward == 0)
                {
                    if (iter.Current.Value.value >= ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskCount(iter.Current.Value.configId))
                    {
                        //Debug.Log("有可领取的   " + iter.Current.Value.configId);
                        return true;
                    }
                }
                else
                {
                    num++;
                }
            }
            if (num != 0 && num == Redpointlist.Count && !armData.groupRewards.Contains(armData.curGroup))
            {
                //Debug.Log("大阶段未领取  " + armData.curGroup);
                return true;
            }
        }
        //判断当前阶段礼包领取状态 (6.12  军备竞赛礼包去掉)
        //int packId = GiftBagTableManager.Instance.GetArmIdByGroupId(armData.curGroup);
        //GiftBuyInfo T_info = GetGiftInfoById(packId);
        //if (T_info != null && T_info.buyTimes != 0 && CSDayChargeInfo.Instance.GetDayCharge() >= GiftBagTableManager.Instance.GetGiftBagPara(packId))
        //{
        //    //Debug.Log("当前阶段礼包未领取  " + armData.curGroup);
        //    return true;
        //}
        return false;
    }
}
