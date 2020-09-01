using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSpecialActivityMonsterSlayInfo : CSInfo<CSSpecialActivityMonsterSlayInfo>
{
    const int actId = 10121;


    ILBetterList<MonsterSlayData> allDatas;
    public ILBetterList<MonsterSlayData> AllDatas { get { return allDatas; } }


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    public CSSpecialActivityMonsterSlayInfo()
    {
        InitConfigInfo();
    }

    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        allDatas?.Clear();
        allDatas = null;

    }


    void InitConfigInfo()
    {
        if (allDatas == null) allDatas = new ILBetterList<MonsterSlayData>(32);
        else allDatas.Clear();
        mPoolHandle.RecycleAll();

        var arr = SpecialActiveRewardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            if (item.activityId != actId) continue;
            MonsterSlayData data = mPoolHandle.GetCustomClass<MonsterSlayData>();
            data.Init(item.id);
            allDatas.Add(data);
        }
        SortDatas();
    }


    public void SC_ResKillDemon(activity.ResKillDemon msg)
    {
        if (allDatas == null) allDatas = new ILBetterList<MonsterSlayData>(32);

        bool listChange = false;
        for (int i = 0; i < msg.killDemonData.Count; i++)
        {
            MonsterSlayData data = allDatas.FirstOrNull(x => { return x.id == msg.killDemonData[i].goalId; });
            if (data == null)
            {
                data = mPoolHandle.GetCustomClass<MonsterSlayData>();
                allDatas.Add(data);
                listChange = true;
            }
            data.Init(msg.killDemonData[i].goalId, msg.killDemonData[i].count, msg.killDemonData[i].reward);
        }
        if (listChange) SortDatas();

        mClientEvent.SendEvent(CEvent.MonsterSlayInfoChange);
    }


    void SortDatas()
    {
        if (allDatas != null)
        {
            allDatas.Sort(DataSorter);
        }
    }


    protected void DataSorter(ref long sortValue, MonsterSlayData r)
    {
        sortValue = r.id;
    }


    public void TryToReceiveRewards(int cfgId)
    {
        Net.ReqSpecialActivityRewardMessage(actId, cfgId);
    }


    public bool CheckCanReceiveRedPoint()
    {
        if (allDatas == null || allDatas.Count < 1) return false;
        return allDatas.Any(x => { return x.curNum >= x.goalNum && !x.rewardReceived; });
    }


    /// <summary>
    /// 是否有可以完成的条目。如果所有的条目奖励均已领取则返回空
    /// </summary>
    /// <returns></returns>
    public MonsterSlayData AnyCanComplete()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc) || 
            !CSOpenServerACInfo.Instance.GetOpenAcState(10121) || allDatas == null) return null;
        MonsterSlayData data = null;
        for (int i = 0; i < allDatas.Count; i++)
        {
            if (allDatas[i].StateInt != 2)
            {
                data = allDatas[i];
                break;
            }
        }
        return data;
    } 


    public bool IsActivityOpen()
    {
        return AnyCanComplete() != null;
    }

    
}


public class MonsterSlayData : IDispose
{
    int _id;
    public int id { get { return _id; } }

    /// <summary> 目标品质 </summary>
    public int quality;

    /// <summary> 目标等级 </summary>
    public int targetLv;

    /// <summary> 目标数 </summary>
    public int goalNum;

    /// <summary> 玩家当前击杀数 </summary>
    public int curNum;

    /// <summary> 玩家该档奖励是否领取 </summary>
    public bool rewardReceived;


    int stateInt;
    /// <summary> 0可领取，1未完成，2已领取 </summary>
    public int StateInt { get { return stateInt; } }


    public void Dispose() { }


    public void Init(int id, int count = 0, bool award = false)
    {
        if (!SpecialActiveRewardTableManager.Instance.ContainsKey(id)) return;
        _id = id;
        List<int> quaAndNum = UtilityMainMath.SplitStringToIntList(SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(id));
        if (quaAndNum.Count > 0) quality = quaAndNum[0];
        if (quaAndNum.Count > 1) targetLv = quaAndNum[1];
        if (quaAndNum.Count > 2) goalNum = quaAndNum[2];

        SetData(count, award);
    }


    public void SetData(int count, bool award)
    {
        curNum = count;
        rewardReceived = award;

        if (rewardReceived) stateInt = 2;
        else
        {
            stateInt = curNum >= goalNum ? 0 : 1;
        }
    }

    public void CopyData(MonsterSlayData data)
    {
        if (data == null) return;
        _id = data.id;
        quality = data.quality;
        targetLv = data.targetLv;
        goalNum = data.goalNum;

        SetData(data.curNum, data.rewardReceived);
    }

}