using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSWildAdventureInfo : CSInfo<CSWildAdventureInfo>
{

    int _time;
    /// <summary> 当前已收益时间，单位秒 </summary>
    public int time { get { return _time; } }

    int _timeLimit;
    /// <summary> 收益时间上限，单位秒 </summary>
    public int timeLimit { get { return _timeLimit; } }

    long _hasExp;
    public long hasExp { get { return _hasExp; } }

    long _hasMoney;
    public long hasMoney { get { return _hasMoney; } }


    CSBetterLisHot<bag.BagItemInfo> rewardsList;

    const int RewardsMaxCount = 100;

    /// <summary> 当前boss奖励列表，只做展示用 </summary>
    public CSBetterLisHot<TABLE.ITEM> bossRewards;

    public override void Dispose()
    {
        rewardsList?.Clear();
        rewardsList = null;


    }

    

    public void SC_AdventureInfo(wildadventure.WildAdventureInfo msg)
    {
        _hasExp = msg.huangExp;
        _hasMoney = msg.huangUpSilver;
        //后端发来的时间是已收益时间，单位ms
        _time = Mathf.FloorToInt(msg.time / 1000);
        _timeLimit = Mathf.FloorToInt(msg.timeLimit / 1000);
        
        RewardsListChange(msg.itemList);

        mClientEvent.SendEvent(CEvent.WildAdventureInfoChange);
    }


    public void SC_BossRewardInfo(wildadventure.BossItemNotify msg)
    {        
        FNDebug.LogError("@@@@@@野外探险boss出现");
        if (bossRewards == null) bossRewards = new CSBetterLisHot<TABLE.ITEM>();
        else bossRewards.Clear();
        for (int i = 0; i < msg.itemList.Count; i++)
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(msg.itemList[i].configId);
            if (cfg != null) bossRewards.Add(cfg);
        }

        mClientEvent.SendEvent(CEvent.WildAdventureBossInfoChange);
    }


    public void TryToTakeOutItem(bag.BagItemInfo bagItem)
    {
        Net.CSTakeOutItemMessage(bagItem.bagIndex);
    }

    public void TryToTakeOutItem(int bagIndex)
    {
        Net.CSTakeOutItemMessage(bagIndex);
    }


    public CSBetterLisHot<bag.BagItemInfo> GetRewardsList()
    {
        if (rewardsList == null) rewardsList = new CSBetterLisHot<bag.BagItemInfo>();
        return rewardsList;
    }


    public bool CanShowRedPoint()
    {
        return (time >= _timeLimit && _timeLimit > 0) || (rewardsList != null && rewardsList.Count >= RewardsMaxCount);
    }



    void RewardsListChange(RepeatedField<bag.BagItemInfo> itemList)
    {
        if (rewardsList == null) rewardsList = new CSBetterLisHot<bag.BagItemInfo>();
        else rewardsList.Clear();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].count > 0) rewardsList.Add(itemList[i]);
        }
        rewardsList.Sort((a, b) => { return ItemTableManager.Instance.GetItemQuality(b.configId).CompareTo(ItemTableManager.Instance.GetItemQuality(a.configId)); });
    }
}
