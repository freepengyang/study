using bag;
using Google.Protobuf.Collections;
using Skyiv.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CSAttributeInfo;

public class ItemFightScore
{
    long _id;
    public long id
    {
        get
        {
            return _id;
        }
        set
        {
            //if (_id > 0 && _id != value)
            //{
            //    UnityEngine.Debug.LogErrorFormat("[idmodified]:[id:{0} => {1}]", id, value);
            //}
            _id = value;
        }
    }
    int _fightScore;
    public int fightScore
    {
        get
        {
            return _fightScore;
        }
        set
        {
            _fightScore = value;
            //UnityEngine.Debug.LogErrorFormat("[id:{0}][score:{1}", id,value);
        }
    }

    public ItemFightScore()
    {
        id = 0;
        _fightScore = 0;
    }

    public static int Compare(ItemFightScore l, ItemFightScore r)
    {
        if (l.fightScore != r.fightScore)
            return r.fightScore - l.fightScore;
        return l.id < r.id ? -1 : (l.id == r.id ? 0 : 1);
    }

    public static int Compare(int l, int r)
    {
        return l - r;
    }
}

public enum EquipSubType
{
    EST_WEAPON = 1,//武器
    EST_CLOTH = 2,//衣服
    EST_HELMET = 3,//头盔
    EST_NECKLACE = 4,//项链
    EST_BRACELET = 5,//手镯
    EST_RING = 6,//戒指
    EST_SHOE = 7,//鞋子
    EST_SASH = 8,//腰带
    EST_MEDAL = 9,//勋章
    EST_JEWEL = 10,//宝石
    EST_COUNT,
}

public class CSItemCountManager : CSInfo<CSItemCountManager>
{
    Dictionary<long, long> mItemDics = new Dictionary<long, long>(32);
    Dictionary<int, long> mItemCfg2Count = new Dictionary<int, long>(32);
    Dictionary<long, long> mBagEquipCountDic = new Dictionary<long, long>(32);
    int m_BagNormalEquipCount;

    PoolHandleManager mPoolHandle = new PoolHandleManager();
    //物品使用提示
    List<long> mHintUsedItem = new List<long>(8);
    //fightScore set
    RB_Tree<ItemFightScore, ItemFightScore> mItemFightDic;
    Dictionary<long, ItemFightScore> mAllItemFightDic = new Dictionary<long, ItemFightScore>(32);
    //比我等级高的装备
    HashSet<long> mHighLevelItems = new HashSet<long>();
    //装备位置分数
    Dictionary<int, List<long>> mPos2EquipMinScore = new Dictionary<int, List<long>>(16);
    //同部位背包中最好装备(我要变强)
    Dictionary<int, PooledHeap<BestEquipOnSamePos>> mBestEquipData = new Dictionary<int, PooledHeap<BestEquipOnSamePos>>(20);
    Dictionary<long, BestEquipOnSamePos> mBestEquipDic = new Dictionary<long, BestEquipOnSamePos>(128);
    //同部位背包中最好装备（背包装备加号显示  判断不一致）
    Dictionary<int, bag.BagItemInfo> mbetterEquipData = new Dictionary<int, BagItemInfo>();
    int[] mPos2Cnt = new int[(int)EquipSubType.EST_COUNT - 1]
    {
        1,1,1,1,2,2,1,1,1,1,
    };
    int sundryId = 634;//更好装备提示，截止等级
    int maxHintLv = 60;
    Dictionary<long, int> mCfgId2MaxHintLv = new Dictionary<long, int>(32);
    Dictionary<long, int> mPersisdentCfgId2MaxHintLv = new Dictionary<long, int>(32);

    public int GetBagNormalEquipCount()
    {
        return m_BagNormalEquipCount;
    }

    void InitHintItems()
    {
        InitMaxLv(mCfgId2MaxHintLv, 670);
        InitMaxLv(mPersisdentCfgId2MaxHintLv, 676);
    }

    void InitMaxLv(Dictionary<long,int> dic,int sundryId)
    {
        //int sundryId = 670;
        TABLE.SUNDRY sundryItem = null;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
            return;
        var group = sundryItem.effect.Split('&');
        for (int i = 0; i < group.Length; ++i)
        {
            var tokens = group[i].Split('#');
            int lv = 0;
            if (!int.TryParse(tokens[0], out lv))
            {
                continue;
            }

            int cfgId = 0;
            for (int j = 1; j < tokens.Length; ++j)
            {
                if (!int.TryParse(tokens[j], out cfgId))
                {
                    continue;
                }

                if (dic.ContainsKey(cfgId))
                    continue;

                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
                {
                    continue;
                }

                dic.Add(cfgId, lv);
                //Debug.LogFormat("<color=#00ff00>[物品使用提示]:{0} MaxLv={1}</color>", item.name, lv);
            }
        }
    }

    public void Initialize(bag.BagInfo bagInfo)
    {
        InitHintItems();
        mItemDics.Clear();
        mItemCfg2Count.Clear();
        mItemFightDic = new RB_Tree<ItemFightScore, ItemFightScore>(mPoolHandle, ItemFightScore.Compare);
        mAllItemFightDic.Clear();
        mHighLevelItems.Clear();
        mPos2EquipMinScore.Clear();
        m_BagNormalEquipCount = 0;

        maxHintLv = 60;
        TABLE.SUNDRY sundryItem = null;
        if (SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            int.TryParse(sundryItem.effect, out maxHintLv);
        }

        if (null != bagInfo)
        {
            var equipedDatas = bagInfo.equips;
            for (int i = 0; i < equipedDatas.Count; ++i)
            {
                TABLE.ITEM itemCfg = null;
                var equipData = equipedDatas[i];
                if (null == equipData || null == equipData.equip || !ItemTableManager.Instance.TryGetValue(equipData.equip.configId, out itemCfg))
                {
                    continue;
                }
                if (CSBagInfo.Instance.IsWoLongEquip(itemCfg))
                {
                    //卧龙装备等人物卧龙等级初始化以后
                    continue;
                }
                OnEquipWeared(equipedDatas[i].equip);
                //mBestEquipData.Add(itemCfg.subType, equipData.equip);
            }

            var itemList = bagInfo.bagItems;
            for (int i = 0; i < itemList.Count; i++)
            {
                TABLE.ITEM itemCfg = null;
                var bagItemInfo = itemList[i];
                if (null == bagItemInfo || !ItemTableManager.Instance.TryGetValue(bagItemInfo.configId, out itemCfg))
                {
                    continue;
                }
                if (CSBagInfo.Instance.IsWoLongEquip(itemCfg))
                {
                    //卧龙装备等人物卧龙等级初始化以后
                    continue;
                }
                OnItemAdd(itemList[i], false);
            }

            for (int i = 0; i < bagInfo.currencyInfos.Count; i++)
            {
                OnMoneyAmountChanged(bagInfo.currencyInfos[i].id, bagInfo.currencyInfos[i].value);
            }
        }

        HotManager.Instance.EventHandler.AddEvent(CEvent.MainPlayer_LevelChange, OnMainPlayerLvChagned);
        HotManager.Instance.EventHandler.AddEvent(CEvent.WoLongLevelUpgrade, OnMainPlayerLvChagned);
        mClientEvent.SendEvent(CEvent.BagInit);
    }

    public void InitWolongEquips()
    {
        var playerInfo = CSMainPlayerInfo.Instance.GetMyInfo();
        if (null == playerInfo || null == playerInfo.bag)
            return;
        var bagInfo = playerInfo.bag;
        var equipedDatas = bagInfo.equips;
        if (null == equipedDatas)
            return;

        for (int i = 0; i < equipedDatas.Count; ++i)
        {
            TABLE.ITEM itemCfg = null;
            var equipData = equipedDatas[i];
            if (null == equipData || null == equipData.equip || !ItemTableManager.Instance.TryGetValue(equipData.equip.configId, out itemCfg))
            {
                continue;
            }
            if (!CSBagInfo.Instance.IsWoLongEquip(itemCfg))
            {
                //卧龙装备等人物卧龙等级初始化以后
                continue;
            }
            OnEquipWeared(equipedDatas[i].equip);
        }

        var itemList = bagInfo.bagItems;
        for (int i = 0; i < itemList.Count; i++)
        {
            TABLE.ITEM itemCfg = null;
            var bagItemInfo = itemList[i];
            if (null == bagItemInfo || !ItemTableManager.Instance.TryGetValue(bagItemInfo.configId, out itemCfg))
            {
                continue;
            }
            if (!CSBagInfo.Instance.IsWoLongEquip(itemCfg))
            {
                //卧龙装备等人物卧龙等级初始化以后
                continue;
            }
            OnItemAdd(itemList[i], false);
        }
    }

    void OnMainPlayerLvChagned(uint id, object argv)
    {
        var unLevelFitIds = mPoolHandle.GetSystemClass<List<long>>();
        unLevelFitIds.Clear();
        var it = mHighLevelItems.GetEnumerator();
        while (it.MoveNext())
        {
            var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(it.Current);
            if (null == itemInfo)
                continue;

            TABLE.ITEM itemCfg = null;
            if (!ItemTableManager.Instance.TryGetValue(itemInfo.configId, out itemCfg))
            {
                continue;
            }

            if (!HasUseTimes(itemInfo.configId))
            {
                mHintUsedItem.Remove(id);
                continue;
            }

            if (mCfgId2MaxHintLv.ContainsKey(itemInfo.configId))
            {
                int playerLv = CSMainPlayerInfo.Instance.Level;
                if (playerLv >= mCfgId2MaxHintLv[itemInfo.configId])
                {
                    mHintUsedItem.Remove(id);
                    continue;
                }

                if (itemCfg.level <= playerLv)
                {
                    mHintUsedItem.Add(it.Current);
                }
                else
                    unLevelFitIds.Add(it.Current);
            }
            else if(mPersisdentCfgId2MaxHintLv.ContainsKey(itemInfo.configId))
            {
                int playerLv = CSMainPlayerInfo.Instance.Level;
                if (playerLv >= mPersisdentCfgId2MaxHintLv[itemInfo.configId])
                {
                    mHintUsedItem.Remove(id);
                    continue;
                }

                if (itemCfg.level <= playerLv)
                {
                    mHintUsedItem.Add(it.Current);
                }
                else
                    unLevelFitIds.Add(it.Current);
            }
            else
            {
                if (itemCfg.level <= CSMainPlayerInfo.Instance.Level && itemCfg.wolongLv <= CSWoLongInfo.Instance.GetWoLongLevel())
                {
                    AddItemFightScore(itemInfo, true, false);
                    GetNewEquip(itemInfo);
                }
                else
                    unLevelFitIds.Add(it.Current);
            }
        }
        mHighLevelItems.Clear();
        for (int i = 0; i < unLevelFitIds.Count; ++i)
        {
            mHighLevelItems.Add(unLevelFitIds[i]);
        }
        unLevelFitIds.Clear();
        mPoolHandle.Recycle(unLevelFitIds);

        //升级完成后可能会弹出
        TryHintBetterEquip();
    }

    public long GetItemCount(int cfgId)
    {
        if (cfgId == 3)
        {
            return CSYuanBaoInfo.Instance.GetAllYuanBaoCount();
        }
        else if (cfgId == 50)
        {
            return CSYuanBaoInfo.Instance.GetCanTradeYuanBaoCount();
        }
        if (mItemCfg2Count.ContainsKey(cfgId))
        {
            return mItemCfg2Count[cfgId];
        }
        return 0;
    }

    public long GetItemCount(long guid)
    {
        if (mItemDics.ContainsKey(guid))
            return mItemDics[guid];
        return 0;
    }

    public void OnMoneyChanged(object argv)
    {
        if (argv is MoneyType moneyType)
        {
            OnMoneyAmountChanged((int)moneyType, CSBagInfo.Instance.GetMoneyCount((int)moneyType));
        }
    }

    public void PushEquipedList(bag.BagItemInfo itemInfo)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(itemInfo.configId, out item) || item.type != 2)
        {
            return;
        }

        int subType = item.subType;

        List<long> equipedList = null;
        if (!mPos2EquipMinScore.ContainsKey(subType))
        {
            equipedList = new List<long>(4);
            mPos2EquipMinScore.Add(subType, equipedList);
        }
        else
        {
            equipedList = mPos2EquipMinScore[subType];
        }

        if (!equipedList.Contains(itemInfo.id))
        {
            equipedList.Add(itemInfo.id);
        }
    }

    public bool RemoveEquipedList(bag.BagItemInfo itemInfo)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(itemInfo.configId, out item) || item.type != 2)
        {
            return false;
        }

        int subType = item.subType;
        if (mPos2EquipMinScore.ContainsKey(subType))
        {
            return mPos2EquipMinScore[subType].Remove(itemInfo.id);
        }

        return false;
    }
    /// <summary>
    /// 获得同部位的最小积分
    /// </summary>
    /// <param name="cfgId"></param>
    /// <returns></returns>
    public int GetEquipedMinFightScore(int cfgId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
        {
            return 0;
        }
        return GetEquipedMinFightScoreBySubtype(item.subType);
    }
    public int GetEquipedMinFightScoreBySubtype(int subType)
    {
        if (!mPos2EquipMinScore.ContainsKey(subType))
        {
            return 0;
        }

        int idx = subType % 100 - 1;
        if (idx < 0 || idx >= mPos2Cnt.Length)
            return 0;

        var guids = mPos2EquipMinScore[subType];
        if (guids.Count < mPos2Cnt[idx])
        {
            return 0;
        }

        int v = int.MaxValue;
        for (int i = 0, max = guids.Count; i < max; ++i)
        {
            int compareValue = GetFightScore(guids[i]);
            if (v > compareValue)
            {
                v = compareValue;
            }
        }
        if (guids.Count <= 0)
            v = 0;
        return v;
    }
    public void OnEquipWeared(bag.BagItemInfo itemInfo)
    {
        AddItemFightScore(itemInfo, false, true);
        PushEquipedList(itemInfo);
    }

    public void OnEquipUnWeared(bag.BagItemInfo itemInfo)
    {
        RemoveFightScore(itemInfo.id);
        RemoveEquipedList(itemInfo);
    }
    public void OnEquipAttrChange(bag.BagItemInfo itemInfo)
    {
        RemoveFightScore(itemInfo.id);
        bool equiped = RemoveEquipedList(itemInfo);
        AddItemFightScore(itemInfo, false, true);
        if(equiped)
            PushEquipedList(itemInfo);

        if(GetEquipRemove(itemInfo))
        {
            GetNewEquip(itemInfo);
        }
    }

    public void OnItemChanged(object argv, int _logType)
    {
        if (argv is EventData eventData)
        {
            if (eventData.arg1 is bag.BagItemInfo itemInfo && eventData.arg2 is ItemChangeType changeType)
            {
                if (changeType == ItemChangeType.Add)
                {
                    //1004 装备替换时 脱下来的装备
                    //1005 手动脱下来的装备
                    OnItemAdd(itemInfo, _logType != 1004 && _logType != 1005);
                }
                else if (changeType == ItemChangeType.Remove)
                {
                    OnItemRemoved(itemInfo);
                }
                else if (changeType == ItemChangeType.NumAdd)
                {
                    OnItemChanged(itemInfo);
                    TryAddHintUseItem(itemInfo);
                }
                else if (changeType == ItemChangeType.NumReduce)
                {
                    OnItemChanged(itemInfo);
                    TryAddHintUseItem(itemInfo);
                }
                //BrocastCounterChangedMessage();
            }
        }
    }

    public void RemoveFightScore(long id)
    {
        if (mHighLevelItems.Remove(id))
        {
            if (mAllItemFightDic.Remove(id))
            {
                //Debug.LogFormat("<color=#00ff00>[移除Error][id:{0}]</color>", id);
            }
            return;
        }

        if (!mAllItemFightDic.ContainsKey(id))
        {
            return;
        }

        var recycleItem = mAllItemFightDic[id];
        //Debug.LogFormat("<color=#00ff00>[移除][id:{0}][score:{1}]</color>", recycleItem.id, recycleItem.fightScore);
        mItemFightDic.Remove(recycleItem);
        mAllItemFightDic.Remove(id);
        mPoolHandle.Recycle(recycleItem);
    }

    void TryAddHintUseItem(bag.BagItemInfo _info)
    {
        if (!ItemTableManager.Instance.TryGetValue(_info.configId, out TABLE.ITEM item))
        {
            return;
        }

        int maxLv = 0;
        if (mCfgId2MaxHintLv.ContainsKey(_info.configId))
        {
            maxLv = mCfgId2MaxHintLv[_info.configId];
        }
        else if (mPersisdentCfgId2MaxHintLv.ContainsKey(_info.configId))
        {
            maxLv = mPersisdentCfgId2MaxHintLv[_info.configId];
        }
        else
            return;

        if (maxLv <= CSMainPlayerInfo.Instance.Level)
            return;

        if (item.level > CSMainPlayerInfo.Instance.Level)
        {
            //压入字典 等待升级时候弹出
            mHighLevelItems.Add(_info.id);
            return;
        }

        if (!mHintUsedItem.Contains(_info.id))
        {
            mHintUsedItem.Add(_info.id);
        }
        else
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnHintUsedItemCountChanged);
        }

        TryHintBetterEquip();
    }

    void AddItemFightScore(bag.BagItemInfo _info, bool newAdd, bool equiped)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(_info.configId, out item) || item.type != 2)
        {
            return;
        }

        //职业或者性别不符合
        if (!CSBagInfo.Instance.IsEquipCareerSexEqual(item))
        {
            return;
        }

        if (CSBagInfo.Instance.IsWoLongEquip(item))
        {
            if (item.wolongLv > CSWoLongInfo.Instance.GetWoLongLevel())
            {
                //压入字典 等待升级时候弹出
                mHighLevelItems.Add(_info.id);
                return;
            }
        }
        else
        {
            if (item.level > CSMainPlayerInfo.Instance.Level)
            {
                //压入字典 等待升级时候弹出
                mHighLevelItems.Add(_info.id);
                return;
            }
        }

        //压入战斗力列表
        if (mAllItemFightDic.ContainsKey(_info.id))
        {
            var oldItem = mAllItemFightDic[_info.id];
            bool alreadyExist = mItemFightDic.Remove(oldItem);
            oldItem.id = _info.id;
            oldItem.fightScore = CalculateFightScore(_info, item);
            //Debug.LogFormat("<color=#00ff00>[修改战斗力积分][id:{0}][score:{1}]</color>", _info.id, oldItem.fightScore);
            if (alreadyExist && NeedBetterEquipHint)
            {
                mItemFightDic.Add(oldItem, oldItem);
            }
            //Debug.LogFormat("<color=#00ff00>[修改][name:{0}][score:{1}]</color>", item.QualityName(), oldItem.fightScore);
            return;
        }

        var fightScore = mPoolHandle.GetSystemClass<ItemFightScore>();
        fightScore.id = _info.id;
        fightScore.fightScore = CalculateFightScore(_info, item);
        //Debug.LogFormat("<color=#00ff00>[新增][id:{0}][name:{1}][score:{2}]</color>", _info.id, item.QualityName(), fightScore.fightScore);
        if (newAdd && NeedBetterEquipHint)
            mItemFightDic.Add(fightScore, fightScore);
        mAllItemFightDic.Add(_info.id, fightScore);
    }

    int CalculateFightScore(bag.BagItemInfo _info, TABLE.ITEM itemCfg)
    {
        return UtilityFightPower.GetFightPower(_info, itemCfg);
    }

    public int GetFightScore(long id)
    {
        if (mAllItemFightDic.ContainsKey(id))
            return mAllItemFightDic[id].fightScore;
        return 0;
    }

    void OnItemAdd(bag.BagItemInfo _info, bool newAdd)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(_info.configId, out item))
        {
            return;
        }

        if (CSBagInfo.Instance.IsNormalEquip(item))
            ++m_BagNormalEquipCount;

        if (!mCfgId2MaxHintLv.ContainsKey(_info.configId) && !mPersisdentCfgId2MaxHintLv.ContainsKey(_info.configId))
        {
            AddItemFightScore(_info, newAdd, false);
        }
        else
        {
            if (newAdd)
            {
                TryAddHintUseItem(_info);
            }
        }

        if (null != _info && !mItemDics.ContainsKey(_info.id))
        {
            mItemDics.Add(_info.id, _info.count);
            AddItemCount(_info.configId, _info.count);
        }

        if (item.type == 2)
        {
            GetNewEquip(_info);

            //有新加入的装备时可能会弹出
            TryHintBetterEquip();
        }
    }

    public void RemoveUseItemFromQueue(long id)
    {
        mHintUsedItem.Remove(id);
        //点击物品使用时,可能会弹出下一件装备
        TryHintBetterEquip();
    }

    public void MarkBetterEquipOld(bag.BagItemInfo _info)
    {
        if (!mAllItemFightDic.ContainsKey(_info.id))
        {
            return;
        }
        RemoveFightScore(_info.id);
        AddItemFightScore(_info, false, false);

        //点击穿戴时，可能会弹出下一件装备
        TryHintBetterEquip();
    }

    //当装备被其他途径销毁时
    public void TryCallNextBetterEquip()
    {
        TryHintBetterEquip();
    }

    //3912 【优化需求单】新获得到的提示规则优化
    //2 如果在玩家等级达到60级（sundry表配）之后
    public bool NeedBetterEquipHint
    {
        get
        {
            bool ret = CSMainPlayerInfo.Instance.Level < maxHintLv;
            if (!ret)
            {
                //Debug.LogFormat("[更好装备]:人物等级[{0}] >= 最大提示等级[{1}] 不需要提示", CSMainPlayerInfo.Instance.Level, maxHintLv);
            }
            return ret;
        }
    }

    BagItemInfo GetPopedEquipItem()
    {
        BagItemInfo bagItemInfo = null;
        ItemFightScore min = null;
        while (!mItemFightDic.Empty())
        {
            min = mItemFightDic.Min.key;
            var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(min.id);
            if (null == itemInfo)
            {
                //mPoolHandle.Recycle(min);注意这里不需要移除此内存有mAllItemFightDic字典持有
                mItemFightDic.Remove(min);
                //Debug.LogFormat("<color=#ff0000>装备已经不存在:[{0}] 直接被弹出</color>", min.id);
                continue;
            }
            int minScore = GetEquipedMinFightScore(itemInfo.configId);
            //如果比身上的装备差，直接弹出
            if (min.fightScore <= minScore)
            {
                //Debug.LogFormat("<color=#ff0000>同部位身上最差装备评分:[{0}] >= 当前装备评分:[{1}]直接被弹出</color>", minScore, min.fightScore);
                //mPoolHandle.Recycle(min);注意这里不需要移除此内存有mAllItemFightDic字典持有
                mItemFightDic.Remove(min);
                continue;
            }

            //Debug.LogFormat("<color=#00ff00>同部位身上最差装备评分:[{0}] < 当前装备评分:[{1}] 弹出提示窗口</color>", minScore, min.fightScore);
            bagItemInfo = itemInfo;
            break;
        }
        //如果不需要装备提示 清除数据
        if (!NeedBetterEquipHint)
        {
            bagItemInfo = null;
        }
        return bagItemInfo;
    }

    BagItemInfo GetPopedUsedItem()
    {
        BagItemInfo usedItem = null;
        int playerLv = CSMainPlayerInfo.Instance.Level;
        while (mHintUsedItem.Count > 0)
        {
            long id = mHintUsedItem[0];
            var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(id);
            if (null == itemInfo)
            {
                mHintUsedItem.Remove(id);
                continue;
            }

            if(!HasUseTimes(itemInfo.configId))
            {
                mHintUsedItem.Remove(id);
                continue;
            }

            if(mCfgId2MaxHintLv.ContainsKey(itemInfo.configId))
            {
                if (playerLv >= mCfgId2MaxHintLv[itemInfo.configId])
                {
                    mHintUsedItem.Remove(id);
                    continue;
                }
            }
            else if (mPersisdentCfgId2MaxHintLv.ContainsKey(itemInfo.configId))
            {
                if (playerLv >= mPersisdentCfgId2MaxHintLv[itemInfo.configId])
                {
                    mHintUsedItem.Remove(id);
                    continue;
                }
            }
            else
            {
                mHintUsedItem.Remove(id);
                continue;
            }

            usedItem = itemInfo;
            break;
        }
        return usedItem;
    }

    void TryHintBetterEquip()
    {
        //获取更好装备
        BagItemInfo bagItemInfo = GetPopedEquipItem();
        //获取需要提示使用的物品
        BagItemInfo usedItem = GetPopedUsedItem();

        int state = 0;
        BagItemInfo popedItem = null;
        if(null != bagItemInfo)
        {
            popedItem = bagItemInfo;
            state = 1;
        }
        else if(null != usedItem)
        {
            popedItem = usedItem;
            state = 2;
        }

        if (null == popedItem)
        {
            UIManager.Instance.ClosePanel<UIGetItemPanel>();
            return;
        }

        var panel = UIManager.Instance.GetPanel<UIGetItemPanel>();
        if (null == panel)
        {
            //弹出更好装备提示
            UIManager.Instance.CreatePanel<UIGetItemPanel>(f =>
            {
                (f as UIGetItemPanel).Show(popedItem,!mPersisdentCfgId2MaxHintLv.ContainsKey(popedItem.configId), state);
            });
        }
    }

    void OnItemChanged(bag.BagItemInfo _info)
    {
        if (null != _info && mItemDics.ContainsKey(_info.id))
        {
            long oldCount = mItemDics[_info.id];
            if (oldCount != _info.count)
            {
                RemoveItemCount(_info.configId, oldCount);
                AddItemCount(_info.configId, _info.count);
                mItemDics[_info.id] = _info.count;
            }
            else
            {
                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(_info.configId, out item))
                {
                    return;
                }
                //属性变化也可能会走这里
                if (item.type != 2)
                {
                    return;
                }
                RemoveFightScore(_info.id);
                AddItemFightScore(_info, true, false);
            }
        }
    }

    void OnItemRemoved(bag.BagItemInfo _info)
    {
        if (null != _info && mItemDics.ContainsKey(_info.id))
        {
            if(ItemTableManager.Instance.TryGetValue(_info.configId, out TABLE.ITEM itemCfg) && CSBagInfo.Instance.IsNormalEquip(itemCfg))
                --m_BagNormalEquipCount;
            RemoveItemCount(_info.configId, mItemDics[_info.id]);
            mItemDics.Remove(_info.id);
            RemoveFightScore(_info.id);
            GetEquipRemove(_info);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnBetterEquipRemoved, _info);
        }
    }

    void AddItemCount(int cfgId, int count)
    {
        if (mItemCfg2Count.ContainsKey(cfgId))
        {
            mItemCfg2Count[cfgId] += count;
        }
        else
        {
            mItemCfg2Count.Add(cfgId, count);
        }
    }

    void RemoveItemCount(int cfgId, long count)
    {
        if (mItemCfg2Count.ContainsKey(cfgId))
        {
            mItemCfg2Count[cfgId] -= count;
            if (mItemCfg2Count[cfgId] < 0)
            {
                FNDebug.LogErrorFormat("count error value = {0}", mItemCfg2Count[cfgId]);
            }
        }
    }

    void OnMoneyAmountChanged(int cfgId, long count)
    {
        if (mItemCfg2Count.ContainsKey(cfgId))
        {
            mItemCfg2Count[cfgId] = count;
        }
        else
        {
            mItemCfg2Count.Add(cfgId, count);
        }

        //BrocastCounterChangedMessage();
    }

    //void BrocastCounterChangedMessage()
    //{
    //    HotManager.Instance.EventHandler.SendEvent(CEvent.ItemCounterChanged);
    //}
    #region 背包中每个位置最好的装备（我要变强特殊判断 评分 品质 levClass）
    public class BestEquipOnSamePos : HeapEntry
    {
        public int heapIndex { get; set; }
        public BagItemInfo bagItemInfo;
        public TABLE.ITEM cfg;
        public int fightScore;
        public static int Comparer(BestEquipOnSamePos l, BestEquipOnSamePos r)
        {
            if (l.fightScore != r.fightScore)
                return r.fightScore - l.fightScore;

            if (l.cfg.quality > r.cfg.quality && l.cfg.levClass > r.cfg.levClass)
                return -1;

            if (l.cfg.levClass != r.cfg.levClass)
                return r.cfg.levClass - l.cfg.levClass;

            if (l.cfg.quality != r.cfg.quality)
                return r.cfg.quality - l.cfg.quality;

            return l.bagItemInfo.id < r.bagItemInfo.id ? -1 : 1;
        }
    }
    public void GetNewEquip(BagItemInfo _info)
    {
        TABLE.ITEM cfg = CSBagInfo.Instance.GetCfg(_info.configId);
        if (cfg == null)
        {
            return;
        }
        if (cfg.type == (int)ItemType.Equip)
        {
            //职业或者性别不符合
            if (!CSBagInfo.Instance.IsEquipCareerSexEqual(cfg))
            {
                return;
            }

            if (CSBagInfo.Instance.IsWoLongEquip(cfg))
            {
                if (cfg.wolongLv > CSWoLongInfo.Instance.GetWoLongLevel())
                {
                    //压入字典 等待升级时候弹出
                    //mHighLevelItems.Add(_info.id);
                    return;
                }
            }
            else
            {
                if (cfg.level > CSMainPlayerInfo.Instance.Level)
                {
                    //压入字典 等待升级时候弹出
                    //mHighLevelItems.Add(_info.id);
                    return;
                }
            }

            PooledHeap<BestEquipOnSamePos> pooledHeap = null;
            if (!mBestEquipData.TryGetValue(cfg.subType, out pooledHeap))
            {
                pooledHeap = new PooledHeap<BestEquipOnSamePos>(32, BestEquipOnSamePos.Comparer);
                mBestEquipData.Add(cfg.subType, pooledHeap);
            }

            var bestItem = pooledHeap.Get();
            bestItem.fightScore = GetFightScore(_info.id);
            bestItem.bagItemInfo = _info;
            bestItem.cfg = cfg;
            pooledHeap.Push(bestItem);
            //UnityEngine.Debug.Log($"Add  {bestItem.cfg.name}  {bestItem.cfg.quality}  {pooledHeap.Count}");
            if (mBestEquipDic.ContainsKey(_info.id))
            {
                mBestEquipDic[_info.id] = bestItem;
            }
            else
            {
                mBestEquipDic.Add(_info.id, bestItem);
            }
        }
    }
    public bool GetEquipRemove(BagItemInfo _info)
    {
        TABLE.ITEM cfg = CSBagInfo.Instance.GetCfg(_info.configId);
        if (cfg == null)
        {
            return false;
        }
        if (cfg.type == (int)ItemType.Equip)
        {
            BestEquipOnSamePos bestItem = null;
            if (mBestEquipDic.TryGetValue(_info.id, out bestItem))
            {
                //UnityEngine.Debug.Log($"Remove {bestItem.cfg.name}  {bestItem.cfg.quality}  {mBestEquipData[cfg.subType].Count}");
                mBestEquipDic.Remove(_info.id);
                mBestEquipData[cfg.subType].Remove(bestItem);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    ///  有可穿戴的装备（装备界面判断+使用） _pos 是item表subType  
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public bool GetHaveBetterEquipByPos(int _pos)
    {
        if (!mBestEquipData.TryGetValue(_pos, out PooledHeap<BestEquipOnSamePos> pooledHeap))
        {
            return false;
        }
        //UnityEngine.Debug.Log(_pos + "      pooledHeap.count    " + pooledHeap.Count);
        if (pooledHeap.Empty())
            return false;
        return true;
    }

    BestEquipOnSamePos mCompareValue = new BestEquipOnSamePos();
    int[] allEquipPos = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112 };//格子位置
    int[] allEquipSubtypePos = { 1, 2, 3, 4, 5, 5, 6, 6, 7, 8, 9, 10, 101, 102, 103, 104, 105, 105, 106, 106, 107, 108, 109, 110 };//装备可以装配位置
    public bool GetHaveBestEquipByPos()
    {
        if (UIManager.Instance.GetPanel<UIGetItemPanel>() != null)
        {
            return false;
        }
        if (CSMainPlayerInfo.Instance.Level <= maxHintLv)
        {
            return false;
        }
        for (int i = 0; i < allEquipPos.Length; i++)
        {
            BagItemInfo selfPosData = CSBagInfo.Instance.GetSelfEquipByGridPos(allEquipPos[i]);
            PooledHeap<BestEquipOnSamePos> pooledHeap = null;
            if (selfPosData == null)
            {
                if (mBestEquipData.TryGetValue(allEquipSubtypePos[i], out pooledHeap))
                {
                    if (!pooledHeap.Empty())
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (mBestEquipData.TryGetValue(allEquipSubtypePos[i], out pooledHeap))
                {
                    if (!pooledHeap.Empty())
                    {
                        TABLE.ITEM cfgSelf = CSBagInfo.Instance.GetCfg(selfPosData.configId);
                        TABLE.ITEM cfgBest = pooledHeap.Top().cfg;
                        int scoreBest = GetFightScore(pooledHeap.Top().bagItemInfo.id);
                        int scoreSelf = GetEquipedMinFightScoreBySubtype(cfgSelf.subType);
                        if (scoreBest > scoreSelf && cfgBest.quality >= cfgSelf.quality && cfgBest.levClass >= cfgSelf.levClass)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void RepalceBestEquip()
    {
        for (int i = 0; i < allEquipPos.Length; i++)
        {
            BagItemInfo selfPosData = CSBagInfo.Instance.GetSelfEquipByGridPos(allEquipPos[i]);
            PooledHeap<BestEquipOnSamePos> pooledHeap = null;
            if (selfPosData == null)
            {
                if (mBestEquipData.TryGetValue(allEquipSubtypePos[i], out pooledHeap))
                {
                    if (!pooledHeap.Empty())
                    {
                        BagItemInfo bestEquip = pooledHeap.Top().bagItemInfo;
                        //Debug.Log($"{bestEquip.bagIndex}     {i}   {pooledHeap.Top().cfg.name}");
                        Net.ReqEquipItemMessage(bestEquip.bagIndex, allEquipPos[i], 0, bestEquip);
                        return;
                    }
                }
            }
            else
            {
                if (mBestEquipData.TryGetValue(allEquipSubtypePos[i], out pooledHeap))
                {
                    if (!pooledHeap.Empty())
                    {
                        TABLE.ITEM cfgSelf = CSBagInfo.Instance.GetCfg(selfPosData.configId);
                        TABLE.ITEM cfgBest = pooledHeap.Top().cfg;
                        int scoreSelf = GetEquipedMinFightScoreBySubtype(cfgSelf.subType);
                        int scoreBest = GetFightScore(pooledHeap.Top().bagItemInfo.id);
                        if (scoreBest > scoreSelf && cfgBest.quality >= cfgSelf.quality && cfgBest.levClass >= cfgSelf.levClass)
                        {
                            //Debug.Log($"{pooledHeap.Top().bagItemInfo.bagIndex}     {i}   {pooledHeap.Top().cfg.name}");
                            Net.ReqEquipItemMessage(pooledHeap.Top().bagItemInfo.bagIndex, allEquipPos[i], 0, pooledHeap.Top().bagItemInfo);
                            return;
                        }
                    }
                }
            }
        }
    }
    #endregion

    public bool HasUseTimes(TABLE.ITEM cfg)
    {
        if (null == cfg)
            return false;

        var dailyInfo = CSBagInfo.Instance.RetuanSingleItemUseCountMes(cfg.group);
        if(null != dailyInfo)
        {
            if (dailyInfo.dailyMaxUseCount <= dailyInfo.dailyUsedCount)
                return false;
        }

        return true;
    }

    public bool HasUseTimes(int cfgId)
    {
        if(!ItemTableManager.Instance.TryGetValue(cfgId,out TABLE.ITEM itemCfg))
        {
            return false;
        }
        return HasUseTimes(itemCfg);
    }



    public override void Dispose()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_LevelChange, OnMainPlayerLvChagned);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.WoLongLevelUpgrade, OnMainPlayerLvChagned);
        mPersisdentCfgId2MaxHintLv?.Clear();
        mPersisdentCfgId2MaxHintLv = null;
        mCfgId2MaxHintLv?.Clear();
        mCfgId2MaxHintLv = null;
        mItemCfg2Count?.Clear();
        mItemCfg2Count = null;
        mItemDics?.Clear();
        mItemDics = null;
        mItemCfg2Count?.Clear();
        mItemCfg2Count = null;
        mPoolHandle = null;
        mItemFightDic?.Clear();
        mItemFightDic = null;
        mAllItemFightDic?.Clear();
        mAllItemFightDic = null;
        mHighLevelItems?.Clear();
        mHighLevelItems = null;
    }
}
