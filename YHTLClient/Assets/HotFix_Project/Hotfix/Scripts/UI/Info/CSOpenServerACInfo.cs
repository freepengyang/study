using activity;
using Google.Protobuf.Collections;
using rank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;

public class CSOpenServerACInfo : CSInfo<CSOpenServerACInfo>
{
    List<int> openIds = new List<int>();
    Dictionary<int, SpecialActivityData> rewards = new Dictionary<int, SpecialActivityData>();

    TABLE.SPECIALACTIVEREWARD specialactivereward;

    public Dictionary<int, SpecialActivityData> Rewards
    {
        get { return rewards; }
    }
    Dictionary<int, ActivityOpenInfo> acStates = new Dictionary<int, ActivityOpenInfo>();
    public override void Dispose()
    {

    }


    #region

    BossFirstKillDatasResponse bossData;
    ResFengYinData fengyinData;

    #endregion

    public void ServerACInfoInit()
    {
        //未开放监听功能开放，开放后监听 等级变动  跨天变动    
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc))
        {
            mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, GetFuncOpen);
        }
        else
        {
            mClientEvent.AddEvent(CEvent.ResDayPassedMessage, GetPassedDay);
            RefreshActivitiesOpenState();
        }
    }

    public void GetServerACtivityOpenState(activity.SCSpecialActivityOpenInfo _msg)
    {
        acStates.Clear();
        for (int i = 0; i < _msg.infos.Count; i++)
        {
            acStates.Add(_msg.infos[i].id, _msg.infos[i]);
        }
        RefreshActivitiesOpenState();
    }
    void GetPassedDay(uint id, object data)
    {
        RefreshSendNet();
    }

    void GetFuncOpen(uint id, object data)
    {
        int funcId = (int)data;
        if (funcId == (int)FunctionType.funcP_OpenServerAc)
        {
            mClientEvent.UnReg(CEvent.FunctionOpenStateChange, GetFuncOpen);
            mClientEvent.AddEvent(CEvent.ResDayPassedMessage, GetPassedDay);
            RefreshActivitiesOpenState();
        }
    }

    void RefreshActivitiesOpenState()
    {
        openIds = SpecialActivityTableManager.Instance.GetOpenAcIdList();
        int closeNum = 0;
        var iter = acStates.GetEnumerator();
        while (iter.MoveNext())
        {
            var act = iter.Current.Value;
            //UnityEngine.Debug.Log($"活动ID      {iter.Current.Value.id}      {iter.Current.Value.open}");
            if (act.open == 1)
            {
                Net.CSSpecialActiveDataMessage(act.id);
            }
            else
            {
                closeNum++;
            }
            if (act.id == 10121) mClientEvent.SendEvent(CEvent.ActivityLinkChanged, act.id * 100);
        }
        if (closeNum == acStates.Count)
        {
            UICheckManager.Instance.FunctionOpenState(FunctionType.funcP_OpenServerAc, false);
        }
    }

    //跨天向服务器发送请求
    private void RefreshSendNet()
    {
        Net.CSDayChargeInfoMessage(); //请求每日充值信息
    }

    public bool GetOpenAcState(int _id)
    {
        if (acStates == null)
        {
            return false;
        }
        if (!acStates.ContainsKey(_id))
        {
            return false;
        }
        int openLevel = SpecialActivityTableManager.Instance.GetSpecialActivityOpenLevel(_id);
        if (openLevel > CSMainPlayerInfo.Instance.Level) return false;

        return acStates[_id].open == 1 ? true : false;
    }

    public void ResSpecialActivityData(SpecialActivityData _mes)
    {
        //Debug.Log("_mes.activityId" + _mes.activityId);
        if (!Rewards.ContainsKey(_mes.activityId))
        {
            Rewards.Add(_mes.activityId, _mes);
        }
        else
        {
            Rewards[_mes.activityId] = _mes;
        }
    }

    #region boss首杀

    public void SetBossFirstKillMes(BossFirstKillDatasResponse _bossData)
    {
        bossData = _bossData;
    }

    public BossFirstKillDatasResponse GetBossData()
    {
        return bossData;
    }

    public SpecialActivityData GetBossFirstKillRewardData()
    {
        if (Rewards.ContainsKey(10104))
        {
            return Rewards[10104];
        }

        return null;
    }

    public bool BossFisrtRedPointChenk()
    {
        if (bossData != null)
        {
            for (int i = 0; i < bossData.bossFirstKillData.Count; i++)
            {
                if (bossData.bossFirstKillData[i].service == 1 || bossData.bossFirstKillData[i].personal == 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region 封印比拼 10103

    public void SetFengYinMes(ResFengYinData _fengyinData)
    {
        fengyinData = _fengyinData;
        //Debug.Log("封印比拼 10103   "+ _fengyinData.datas.Count);
        //for (int i = 0; i < fengyinData.datas.Count; i++)
        //{
        //    Debug.Log("封印比拼   " + fengyinData.datas[i]);
        //}
    }

    public ResFengYinData GetFengYinData()
    {
        return fengyinData;
    }

    public bool SealCompetitionRedPointChenk()
    {
        if (Rewards.ContainsKey(10103))
        {
            //for (int i = 0; i < rewards[10103].finishGoals.Count; i++)
            //{
            //    Debug.Log("finishGoals     " + rewards[10103].finishGoals[i]);
            //}
            //for (int i = 0; i < rewards[10103].rewardGoals.Count; i++)
            //{
            //    Debug.Log("rewardGoals      " + rewards[10103].rewardGoals[i]);
            //}
            if (Rewards[10103].finishGoals.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 装备收集

    List<activity.CollectActivityData> equipCollects = new List<CollectActivityData>();

    public List<activity.CollectActivityData> EquipCollects
    {
        get { return equipCollects; }
    }

    Map<CollectActivityData, bool> mapReceive = new Map<CollectActivityData, bool>();

    /// <summary>
    /// 获取装备收集列表
    /// </summary>
    /// <param name="msg"></param>
    public void SCCollectActivityData(activity.CollectActivityDatas msg)
    {
        if (msg == null) return;

        if (msg.item.Count > 0)
        {
            CSBetterLisHot<activity.CollectActivityData> listhot = new CSBetterLisHot<CollectActivityData>();
            listhot.Clear();
            for (int i = 0; i < msg.item.Count; i++)
            {
                listhot.Add(msg.item[i]);
            }

            listhot.Sort((a, b) =>
            {
                if (!a.reward && !b.reward) //都未领取
                {
                    List<int> listIntA =
                        UtilityMainMath.SplitStringToIntList(
                            SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(a.goalId));
                    int maxCountA = listIntA[0];
                    List<int> listIntB =
                        UtilityMainMath.SplitStringToIntList(
                            SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(b.goalId));
                    int maxCountB = listIntB[0];


                    if (a.count >= maxCountA && b.count < maxCountB) //a满足领取，b不满足
                    {
                        return -1;
                    }
                    else if (a.count < maxCountA && b.count >= maxCountB)
                    {
                        return 1;
                    }
                    else //都满足或者都不满足
                    {
                        return a.goalId < b.goalId ? -1 : 1;
                    }
                }
                else if (!a.reward && b.reward) //a未领取 b领取
                {
                    return -1;
                }
                else if (a.reward && !b.reward)
                {
                    return 1;
                }
                else //两个都已领取
                {
                    return a.goalId < b.goalId ? -1 : 1;
                }
            });


            List<int> listInt;
            CollectActivityData collectActivityData;
            equipCollects.Clear();
            for (int i = 0; i < listhot.Count; i++)
            {
                collectActivityData = listhot[i];
                equipCollects.Add(collectActivityData);
                bool isReceive = false;
                if (!collectActivityData.reward)
                {
                    listInt =
                        UtilityMainMath.SplitStringToIntList(
                            SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(collectActivityData
                                .goalId));
                    int maxCount = listInt[0];
                    if (collectActivityData.count >= maxCount)
                    {
                        isReceive = true;
                    }
                }

                mapReceive.Add(collectActivityData, isReceive);
            }
        }
    }

    /// <summary>
    /// 是否有可收集装备
    /// </summary>
    /// <returns></returns>
    public bool HasCollectEquip()
    {
        //因为equipCollects已排序,可领取的会放在最前面.所以判断第一个就行了
        if (equipCollects.Count > 0)
        {
            if (mapReceive.ContainsKey(equipCollects[0]))
            {
                return mapReceive[equipCollects[0]];
            }
        }

        return false;
    }

    #endregion

    #region 装备评分

    //rank.RankInfo rankinfo;

    //Map<int, RankInfo> ranks = new Map<int, RankInfo>();

    //public void RankInfoChange(RankInfo rankInfo) {

    //    if (!ranks.ContainsKey(rankInfo.type))
    //    {
    //        ranks.Add(rankInfo.type, rankInfo);
    //    }
    //    else
    //    {
    //        ranks[rankInfo.type] = rankInfo;
    //    }
    //    mClientEvent.SendEvent(CEvent.EquipRankChange);
    //}

    //public RankInfo GetRankByType(int type) {
    //    if (ranks.ContainsKey(type))
    //    {
    //        return ranks[type];
    //    }
    //    else {
    //        return null;
    //    }
    //}


    /// <summary>
    /// 获得装备评分个人任务第一个可以领取的奖励位置,如果没有,则返回0
    /// </summary>
    /// <returns></returns>
    public int GetFirstPos()
    {
        var idlist = rewards[10101].finishGoals;
        int goalId = 0;
        for (int i = 0; i < idlist.Count; i++)
        {
            if (SpecialActiveRewardTableManager.Instance.TryGetValue(idlist[i], out specialactivereward))
            {
                if (specialactivereward.rewardType == 2)
                {

                    if (goalId == 0)
                    {
                        goalId = specialactivereward.goalId;
                    }
                    else
                    {
                        goalId = goalId > specialactivereward.goalId ? specialactivereward.goalId : goalId;
                    }
                }
            }
        }

        return goalId != 0 ? goalId - 1 : 0;

    }

    public bool ServerActivityRankRedPointChenk()
    {
        //Debug.Log("rewards[10101].finishGoals.Coun" + rewards[10101].finishGoals.Count);
        if (Rewards.ContainsKey(10101))
        {
            if (Rewards[10101].finishGoals.Count > 0 && Rewards[10101].rewardGoals.Count >= 0)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    public bool IsGetReward(GiftType type)
    {
        int id = (int)type;
        SpecialActivityData specialActivityData;
        if (Rewards.TryGetValue(id, out specialActivityData))
        {
            return specialActivityData.rewardGoals.Contains(GetRewardid(id));
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// 判断开服礼包是否可以领取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsCanRewardGift(int id)
    {
        //bool isOpen = UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc);
        SPECIALACTIVITY reward;
        if (SpecialActivityTableManager.Instance.TryGetValue(id , out reward))
        {
            var openday = CSMainPlayerInfo.Instance.ServerOpenDay;
            if (openday > reward.eventLast + reward.starttime || openday < reward.starttime )
                return false;
            if (CSMainPlayerInfo.Instance.Level < reward.openLevel)
                return false;
        }
        
        
        if (Rewards.ContainsKey(id))
        {
            
            bool isReceive = Rewards[id].rewardGoals.Contains(GetRewardid(id));
            return !isReceive;
        }
        else
        {
            return false;
        }


    }



    public int GetRewardid(int acid)
    {
        var arr = SpecialActiveRewardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            if (item.activityId == acid)
            {
                return item.id;
            }
        }
        return 0;
    }

}

public enum GiftType
{
    EQUIP = 10106,
    EXP = 10107,
    WING = 10108,
    SHENFU = 10109,
    SKILL = 10110,
    WOLONG = 10111,
}