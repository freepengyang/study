using bag;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using activity;
using TABLE;
using UnityEngine;


public enum EquipChangeType
{
    Wear,
    UnWear,
}

public enum ItemChangeType
{
    Add,
    NumAdd,
    NumReduce,
    Remove,
    AttrChange,
}

//背包数据的管理类，初始化从PlayerInfo拿  后续增删改走Bag.proto
public partial class CSBagInfo : CSInfo<CSBagInfo>
{
    public CSBagInfo()
    {
        InitActions();
        EquipConfigInit();
    }
    public override void Dispose()
    {
        GetPageInfos.Clear();
        GetPageInfos = null;
        EqiupWearPosList.Clear();
        EqiupWearPosList = null;
        if (Timer.Instance.IsInvoking(sortSchedule))
            Timer.Instance.CancelInvoke(sortSchedule);

    }

    PoolHandleManager Pool = new PoolHandleManager();
    private bag.BagInfo mBagData;
    private Dictionary<long, bag.BagItemInfo> mItemData = new Dictionary<long, BagItemInfo>();
    private Dictionary<int, bag.BagItemInfo> mEquipData = new Dictionary<int, BagItemInfo>();
    public Dictionary<int, int> equipPosToIndex = new Dictionary<int, int>();
    Dictionary<int, long> moneyData = new Dictionary<int, long>();
    List<long> newBoxList = new List<long>();
    Dictionary<int, TABLE.ITEM> mItemCfgData = new Dictionary<int, TABLE.ITEM>();
    #region 背包排序相关
    private long sec = 10;
    private Schedule sortSchedule;
    private bool canSort = true;

    public long Sec
    {
        get => sec;
    }

    public Schedule SortSchedule
    {
        get => sortSchedule;
        set => sortSchedule = value;
    }

    public bool CanSort
    {
        get => canSort;
        set => canSort = value;
    }

    private UIBagPanel uiBagPanel;
    public void SortCountDown(Schedule schedule)
    {
        uiBagPanel = UIManager.Instance.GetPanel<UIBagPanel>();
        if (null != uiBagPanel)
            uiBagPanel.CountDown();

        if (sec > 0)
        {
            if (sec == 10 && canSort)
                canSort = false;
            sec--;
        }
        else
        {
            sec = 10;
            canSort = true;
            uiBagPanel = null;
            if (Timer.Instance.IsInvoking(sortSchedule))
                Timer.Instance.CancelInvoke(sortSchedule);
        }
    }

    #endregion

    public void InitBagData(bag.BagInfo _data)
    {
        mItemData.Clear();
        mEquipData.Clear();
        mBagData = _data;
        var itemList = mBagData.bagItems;
        for (int i = 0, max = itemList.Count; i < max; i++)
        {
            mItemData.Add(itemList[i].id, itemList[i]);
            TABLE.ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(itemList[i].configId, out cfg))
            {
                if (!mItemCfgData.ContainsKey(itemList[i].configId))
                {
                    mItemCfgData.Add(itemList[i].configId, cfg);
                }
            }
        }
        for (int i = 0, max = mBagData.equips.Count; i < max; i++)
        {
            mEquipData.Add(mBagData.equips[i].position, mBagData.equips[i].equip);
        }

        moneyData.Clear();
        for (int i = 0, max = mBagData.currencyInfos.Count; i < max; i++)
        {
            moneyData.Add(mBagData.currencyInfos[i].id, mBagData.currencyInfos[i].value);
        }
    }


    /// <summary>
    /// 根据type和subType取得道具列表
    /// </summary>
    /// <param name="type"></param>
    /// <param name="subType"></param>
    /// <returns></returns>
    public void SetItemsByTypeAndSubType(ILBetterList<BagItemInfo> listItems, int type, int subType)
    {
        if (listItems == null) return;
        listItems.Clear();
        var dic = mItemData.GetEnumerator();
        while (dic.MoveNext())
        {
            if (type == ItemTableManager.Instance.GetItemType(dic.Current.Value.configId) &&
                subType == ItemTableManager.Instance.GetItemSubType(dic.Current.Value.configId))
            {
                listItems.Add(dic.Current.Value);
            }
        }
    }

    public BagItemInfo GetBagItemByConfigId(long id)
    {
        //TABLE.ITEM _cfg;
        var dic = mItemData.GetEnumerator();
        while (dic.MoveNext())
        {
            if (dic.Current.Value.configId == id)
            {
                return dic.Current.Value;
            }
        }

        return null;
    }

    // public ILBetterList<BagItemInfo> GetBaoZhuItems()
    // {
    //     BagItemInfo equipBaoZhu = GetSelfEquipByGridPos(12);
    //     if (equipBaoZhu == null)
    //     {
    //         return GetItemsByType(2, 10);
    //     }
    //     else
    //     {
    //         ILBetterList<BagItemInfo> listItems = new ILBetterList<BagItemInfo>();
    //         listItems.Clear();
    //         listItems.Add(equipBaoZhu);
    //         ILBetterList<BagItemInfo> list = GetItemsByType(2, 10);
    //         for (int i = 0; i < list.Count; i++)
    //         {
    //             if (!listItems.Contains(list[i]))
    //             {
    //                 listItems.Add(list[i]);
    //             }
    //         }
    //
    //         return listItems;
    //     }
    // }


    //背包的道具变动
    public void ItemsChangeList(BagItemChangeList _mes)
    {

        //分开 增删改
        // if (CSGame.Sington != null)
        //     CSGame.Sington.StartCoroutine(ItemChangeCoroutine(_mes));

        for (int i = 0; i < _mes.changeList.Count; i++)
        {
            if (!mItemData.ContainsKey(_mes.changeList[i].id))
            {
                //Debug.Log(" 添加  "+ ItemTableManager.Instance.GetItemName(_mes.changeList[i].configId));
                BagItemAdd(_mes.changeList[i], _mes.logType);
            }
            else
            {
                if (_mes.changeList[i].count == 0)
                {
                    BagItemRemove(_mes.changeList[i], _mes.logType);
                }
                else
                {
                    BagItemChange(_mes.changeList[i], _mes.logType);
                }
            }
        }

        mClientEvent.SendEvent(CEvent.ItemListChange);
    }

    public void GetBagMaxCountChange(int _maxCount)
    {
        mBagData.maxCount = _maxCount;
    }

    #region 背包增删改

    public void BagItemAdd(BagItemInfo _info, int _logType = 0)
    {
        TABLE.ITEM cfg;
        if (ItemTableManager.Instance.TryGetValue(_info.configId, out cfg))
        {
            if (cfg.notPrompt == 1 || cfg.notPrompt == 3)
            {
                ShowChangeTips(_info.configId, _info.count, 622, _logType);
            }
            if (_logType != 1030 && cfg.type == (int)ItemType.Box)
            {
                if (cfg.level <= CSMainPlayerInfo.Instance.Level)
                {
                    GetNewBoxRedPoint(_info.id);
                }
            }
            mItemData.Add(_info.id, _info);
            if (!mItemCfgData.ContainsKey(_info.configId))
            {
                mItemCfgData.Add(_info.configId, cfg);
            }
        }
        ApplyItemChange(_info, ItemChangeType.Add, _logType);
    }

    public void BagItemRemove(BagItemInfo _info, int _logType = 0)
    {
        if (ItemTableManager.Instance.GetItemNotPrompt(_info.configId) == 2)
        {
            ShowChangeTips(_info.configId, mItemData[_info.id].count, 623, _logType);
        }
        CancelNewBoxRedPoint(_info.id);
        mItemData.Remove(_info.id);
        ApplyItemChange(_info, ItemChangeType.Remove, _logType);
        mItemCfgData.Remove(_info.configId);
    }

    public void BagItemChange(BagItemInfo _info, int _logType = 0)
    {
        TABLE.ITEM cfg;
        if (ItemTableManager.Instance.TryGetValue(_info.configId, out cfg))
        {
            BagItemInfo old = mItemData[_info.id];
            if (old.configId == _info.configId)
            {
                if (old.count < _info.count)
                {
                    if (cfg.notPrompt == 1 || cfg.notPrompt == 3)
                    {
                        ShowChangeTips(_info.configId, (_info.count - old.count), 622, _logType);
                    }
                    if (_logType != 1030 && cfg.type == (int)ItemType.Box)
                    {
                        if (cfg.level <= CSMainPlayerInfo.Instance.Level)
                        {
                            GetNewBoxRedPoint(_info.id);
                        }
                    }
                    mItemData[_info.id] = _info;
                    ApplyItemChange(_info, ItemChangeType.NumAdd, _logType);
                }
                else
                {
                    if (cfg.notPrompt == 2)
                    {
                        ShowChangeTips(_info.configId, (old.count - _info.count), 623, _logType);
                    }

                    mItemData[_info.id] = _info;
                    ApplyItemChange(_info, ItemChangeType.NumReduce, _logType);
                }
            }
        }


    }

    void ShowChangeTips(int id, int num, int tipsId, int logType)
    {
        if (logType == 1030)
        {
            return;
        } //仓库存取

        if (logType == 1004)
        {
            return;
        } //装备穿戴

        if (logType == 1005)
        {
            return;
        } //装备卸下

        UtilityTips.LeftDownTips(id, num, tipsId);
    }

    public TABLE.ITEM GetCfg(int _configId)
    {
        TABLE.ITEM cfg;
        if (mItemCfgData.ContainsKey(_configId))
        {
            return mItemCfgData[_configId];
        }
        ItemTableManager.Instance.TryGetValue(_configId, out cfg);
        return cfg;
    }
    #endregion

    Dictionary<long, bag.BagItemInfo> mTempItemData = new Dictionary<long, BagItemInfo>(125);
    //整理
    public void BagSrot(RepeatedField<BagItemInfo> _repeat)
    {
        mTempItemData.Clear();
        for (int i = 0, max = _repeat.Count; i < max; i++)
        {
            mTempItemData.Add(_repeat[i].id, _repeat[i]);
        }

        //mItemData.Clear();
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            var curValue = iter.Current;
            if (!mTempItemData.ContainsKey(curValue.Key))
            {
                mItemData.Remove(curValue.Key);
            }
            else
            {
                if (curValue.Value.count > mTempItemData[curValue.Key].count)
                {
                    curValue.Value.count = mTempItemData[curValue.Key].count;
                    ApplyItemChange(curValue.Value, ItemChangeType.NumAdd, 1030);
                }
                else
                {
                    curValue.Value.count = mTempItemData[curValue.Key].count;
                    ApplyItemChange(curValue.Value, ItemChangeType.NumReduce, 1030);
                }
                curValue.Value.bagIndex = mTempItemData[curValue.Key].bagIndex;
            }
        }
        //for (int i = 0; i < _repeat.Count; i++)
        //{
        //    mItemData.Add(_repeat[i].id, _repeat[i]);
        //}

        mClientEvent.SendEvent(CEvent.BagSort);
    }

    //货币变动
    public void MoneyChange(WealthAmountChangeResponse msg)
    {
        //Debug.Log(costCfg.currencyInfo.id + "   " + costCfg.logType);
        if (moneyData.ContainsKey(msg.currencyInfo.id))
        {
            ShowMoneyTip(msg.currencyInfo.id, moneyData[msg.currencyInfo.id], msg.currencyInfo.value, msg.logType);
            moneyData[msg.currencyInfo.id] = msg.currencyInfo.value;
            ApplyMoneyChange((MoneyType)msg.currencyInfo.id);
        }
        else
        {
            moneyData.Add(msg.currencyInfo.id, msg.currencyInfo.value);
            ShowMoneyTip(msg.currencyInfo.id, 0, msg.currencyInfo.value, msg.logType);
            ApplyMoneyChange((MoneyType)msg.currencyInfo.id);
        }
    }

    protected void ApplyItemChange(bag.BagItemInfo _info, ItemChangeType itemChangeType, int _logType = 0)
    {

        EventData data = CSEventObjectManager.Instance.SetValue(_info, itemChangeType);
        CSItemCountManager.Instance.OnItemChanged(data, _logType);
        mClientEvent.SendEvent(CEvent.ItemChange, data);
        CSEventObjectManager.Instance.Recycle(data);
    }

    protected void ApplyMoneyChange(MoneyType moneyType)
    {
        CSItemCountManager.Instance.OnMoneyChanged(moneyType);
        mClientEvent.SendEvent(CEvent.MoneyChange, moneyType);
    }

    int tipId = 0;
    long moneyNum = 0;

    enum itemprompType
    {
        none = 0,
        gotOnly = 1,
        loseOnly = 2,
        alwaysShow = 3,
    }

    void ShowMoneyTip(int id, long oldNum, long newNum, int _logtype)
    {
        tipId = 0;
        moneyNum = 0;
        tipId = (oldNum > newNum) ? 623 : 622;
        moneyNum = newNum - oldNum;
        if (id == 9)
        {
            if (moneyNum > 0)
            {
                mClientEvent.SendEvent(CEvent.ZhenQiAdd);
            }
            if (_logtype == 10204)
            {
                return;
            }
        }
        itemprompType type = (itemprompType)ItemTableManager.Instance.GetItemNotPrompt(id);
        if (type == itemprompType.alwaysShow && moneyNum != 0 || type == itemprompType.gotOnly && moneyNum > 0
                                                              || type == itemprompType.loseOnly && moneyNum < 0)
        {
            UtilityTips.LeftDownTips(id, Math.Abs(moneyNum), tipId);
        }
    }
    public long GetMoneyCount(int _type)
    {
        if (moneyData.ContainsKey(_type))
        {
            return moneyData[_type];
        }
        else
        {
            return 0;
        }
    }
    //返回背包格子数据
    public Dictionary<long, BagItemInfo> GetBagItemData()
    {
        return mItemData;
    }

    //返回背包最大数量
    public int GetMaxCount()
    {
        if (mBagData == null) { return 0; }
        return mBagData.maxCount;
    }
    //返回
    public int GetCurMaxCount()
    {
        return mItemData.Count;
    }

    public bool IsBagFilled()
    {
        if (mBagData == null)
        {
            return true;
        }
        return mBagData.maxCount <= mItemData.Count;
    }

    /// <summary>
    /// 判断背包内有多少空位
    /// </summary>
    /// <returns></returns>
    public int IsNullBagNum()
    {
        if (mBagData == null || mItemData == null)
        {
            return 0;
        }
        return mBagData.maxCount - mItemData.Count;
    }


    //返回道具信息（用id拿）
    public BagItemInfo GetBagItemInfoById(long _id)
    {
        bag.BagItemInfo info = null;
        if (mItemData.ContainsKey(_id))
            info = mItemData[_id];
        return info;
    }



    //返回装备数据
    public Dictionary<int, BagItemInfo> GetEquipItemData()
    {
        return mEquipData;
    }

    //返回身上普通装备
    public void GetNormalEquip(Dictionary<int, bag.BagItemInfo> equips)
    {
        equips.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (1 <= iter.Current.Key || iter.Current.Key <= 12)
            {
                equips.Add(iter.Current.Key, iter.Current.Value);
            }
        }
    }
    public int GetNormalEquipCount()
    {
        int i = 0;
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (1 <= iter.Current.Key || iter.Current.Key <= 12)
            {
                i++;
            }
        }
        return i;
    }

    //返回身上卧龙装备
    public void GetWoLongEquip(Dictionary<int, bag.BagItemInfo> equips)
    {
        equips.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (101 <= iter.Current.Key || iter.Current.Key <= 112)
            {
                equips.Add(iter.Current.Key, iter.Current.Value);
            }
        }
    }
    public int GetWoLongEquipCount()
    {
        int count = 0;
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (101 <= iter.Current.Key || iter.Current.Key <= 112)
            {
                count++;
            }
        }
        return count;
    }
    Dictionary<int, bag.BagItemInfo> recastEquips = new Dictionary<int, bag.BagItemInfo>();

    //返回身上可以重铸的装备
    ILBetterList<bag.EquipInfo> selfRecastlist = new ILBetterList<EquipInfo>(12);
    public Dictionary<int, bag.BagItemInfo> GetEquipRecastItemData()
    {
        recastEquips.Clear();
        selfRecastlist.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                if (iter.Current.Value.quality != 5 && cfg.levClass != 0)
                {
                    EquipInfo info = new EquipInfo();
                    info.position = iter.Current.Key;
                    info.equip = iter.Current.Value;
                    selfRecastlist.Add(info);
                }
            }
        }
        selfRecastlist.Sort((a, b) => { return b.equip.quality - a.equip.quality; });
        for (int i = 0; i < selfRecastlist.Count; i++)
        {
            recastEquips.Add(selfRecastlist[i].position, selfRecastlist[i].equip);
        }
        return recastEquips;
    }

    //返回身上可以洗练的装备
    public Dictionary<int, BagItemInfo> GetEquipRefineItemData()
    {
        recastEquips.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                recastEquips.Add(iter.Current.Key, iter.Current.Value);
            }
        }
        return recastEquips;
    }

    ILBetterList<BagItemInfo> itemInfos = new ILBetterList<BagItemInfo>(); //对象缓存只new一次

    /// <summary>
    /// 根据位置获取背包内的同类宝石
    /// </summary>
    /// <param name="pos">位置</param>
    /// <returns></returns>
    public ILBetterList<BagItemInfo> GetGemInfoByPos(int pos)
    {
        //geminfos.Clear();
        itemInfos.Clear();
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);

            if (cfg.type == 9 && cfg.subType == pos)
            {
                //if (iter.Current.Value.quality != 1)
                // {
                //Debug.Log("");
                itemInfos.Add(iter.Current.Value);
                // }
            }
        }

        return itemInfos;
    }

    /// <summary>
    /// 根据位置获取背包内最高等级的宝石
    /// </summary>
    public BagItemInfo GethighGemInfoByPos(int pos)
    {
        BagItemInfo bagitemInfo = null;
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (cfg.type == 9 && cfg.subType == pos)
            {
                if (bagitemInfo != null)
                {
                    if (GemTableManager.Instance.GetGemLv(bagitemInfo.configId) <
                        GemTableManager.Instance.GetGemLv(iter.Current.Value.configId))
                        bagitemInfo = iter.Current.Value;
                }
                else
                    bagitemInfo = iter.Current.Value;
            }
        }
        return bagitemInfo;
    }

    //按页数返回道具信息
    List<BagItemInfo> GetPageInfos = new List<BagItemInfo>();
    public void GetBagItemInfosByPageNum(int _page, BagShowType _type, Dictionary<long, bag.BagItemInfo> _dic)
    {
        GetPageInfos.Clear();
        List<BagItemInfo> temp_infos = GetPageInfos;
        _dic.Clear();
        int startNum = 25 * (_page - 1) + 1;
        int endNum = startNum + 24;
        if (_type == BagShowType.All)
        {
            var iter = mItemData.GetEnumerator();
            while (iter.MoveNext())
            {
                if (startNum <= iter.Current.Value.bagIndex && iter.Current.Value.bagIndex <= endNum)
                {
                    _dic.Add(iter.Current.Key, iter.Current.Value);
                }
            }
        }
        else if (_type == BagShowType.Equip)
        {
            var iter = mItemData.GetEnumerator();
            while (iter.MoveNext())
            {
                TABLE.ITEM cfg = GetCfg(iter.Current.Value.configId);
                if (cfg != null && cfg.type == (int)ItemType.Equip)
                {
                    temp_infos.Add(iter.Current.Value);
                }
            }
            for (int i = 0; i < temp_infos.Count; i++)
            {
                if (startNum <= (i + 1) && _dic.Count < 25)
                {
                    if (CSItemRecycleInfo.Instance.recycleMode == CSItemRecycleInfo.RecycleMode.RM_NORMAL)
                    {
                        TABLE.ITEM itemCfg = null;
                        if (ItemTableManager.Instance.TryGetValue(temp_infos[i].configId, out itemCfg))
                        {
                            if (CSItemRecycleInfo.Instance.CanAsNormalRecycle(itemCfg))
                            {
                                _dic.Add(temp_infos[i].bagIndex, temp_infos[i]);
                            }
                        }
                    }
                    else if (CSItemRecycleInfo.Instance.recycleMode == CSItemRecycleInfo.RecycleMode.RM_NEIGONG)
                    {
                        TABLE.ITEM itemCfg = null;
                        if (ItemTableManager.Instance.TryGetValue(temp_infos[i].configId, out itemCfg))
                        {
                            if (CSItemRecycleInfo.Instance.CanAsNeigongRecycle(itemCfg))
                            {
                                _dic.Add(temp_infos[i].bagIndex, temp_infos[i]);
                            }
                        }
                    }
                    else
                    {
                        _dic.Add(temp_infos[i].bagIndex, temp_infos[i]);
                    }
                }
            }
        }
        else if (_type == BagShowType.Medicine)
        {
            var iter = mItemData.GetEnumerator();
            while (iter.MoveNext())
            {
                TABLE.ITEM cfg = GetCfg(iter.Current.Value.configId);
                if (cfg != null && cfg.type == (int)ItemType.Medicine)
                {
                    temp_infos.Add(iter.Current.Value);
                }
            }
            for (int i = 0; i < temp_infos.Count; i++)
            {
                if (startNum <= (i + 1) && _dic.Count < 25)
                {
                    _dic.Add(temp_infos[i].bagIndex, temp_infos[i]);
                }
            }
        }
        else
        {
            var iter = mItemData.GetEnumerator();
            while (iter.MoveNext())
            {
                TABLE.ITEM cfg = GetCfg(iter.Current.Value.configId);
                if (cfg != null && cfg.type != (int)ItemType.Medicine && cfg.type != (int)ItemType.Equip)
                {
                    temp_infos.Add(iter.Current.Value);
                }
            }
            for (int i = 0; i < temp_infos.Count; i++)
            {
                if (startNum <= (i + 1) && _dic.Count < 25)
                {
                    _dic.Add(temp_infos[i].bagIndex, temp_infos[i]);
                }
            }
        }
    }

    //收到穿装备
    public void ResEquipWear(EquipItemMsg _msg)
    {
        //Debug.Log("装备穿戴  " + _msg.position);
        bag.BagItemInfo info;
        if (mEquipData.ContainsKey(_msg.position))
        {
            info = mEquipData[_msg.position];
            CSItemCountManager.Instance.RemoveEquipedList(info);
            mEquipData[_msg.position] = _msg.equip;
        }
        else
        {
            mEquipData.Add(_msg.position, _msg.equip);
        }
        TABLE.ITEM cfg;
        if (ItemTableManager.Instance.TryGetValue(_msg.equip.configId, out cfg))
        {
            if (!mItemCfgData.ContainsKey(_msg.equip.configId))
            {
                mItemCfgData.Add(_msg.equip.configId, cfg);
            }
        }
        _msg.equip.bagIndex = _msg.bagIndex;
        BagItemRemove(_msg.equip, 1004);
        CSItemCountManager.Instance.OnEquipWeared(_msg.equip);
        mClientEvent.SendEvent(CEvent.WearEquip, _msg);
    }

    //收到脱装备消息
    public void ResEquipUnWear(UnEquipItemResponse _msg)
    {
        //Debug.Log("装备脱下  " + _msg.position);
        BagItemInfo info = mEquipData[_msg.position];
        mEquipData.Remove(_msg.position);
        info.bagIndex = _msg.bagIndex;
        CSItemCountManager.Instance.OnEquipUnWeared(info);
        BagItemAdd(info, 1005);
        mItemCfgData.Remove(info.configId);
        mClientEvent.SendEvent(CEvent.UnWeatEquip, _msg);
    }

    //按位置获取装备数据
    public void GetEquipInfoByPos(int _pos, List<bag.EquipInfo> list)
    {
        list.Clear();
        if (mEquipData == null)
        {
            return;
        }

        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (ItemTableManager.Instance.GetItemSubType(iter.Current.Value.configId) == _pos)
            {
                EquipInfo info = new EquipInfo();
                info.position = iter.Current.Key;
                info.equip = iter.Current.Value;
                list.Add(info);
            }
        }
    }

    //返回身上装备数据，参数是格子位置
    public bag.BagItemInfo GetSelfEquipByGridPos(int _pos)
    {
        if (mEquipData.ContainsKey(_pos))
        {
            return mEquipData[_pos];
        }

        return null;
    }

    //洗练结果返回
    public void GetEquipRefineRes(bag.EquipInfo _msg)
    {
        if (_msg.position < 0)
        {
            mEquipData[Math.Abs(_msg.position)] = _msg.equip;
        }
        else
        {
            mItemData[_msg.equip.id] = _msg.equip;
        }
        CSItemCountManager.Instance.OnEquipAttrChange(_msg.equip);
    }

    public bool HasEquiped(int pos)
    {
        return mEquipData.ContainsKey(pos);
    }

    public bag.BagItemInfo GetEquipedData(int pos)
    {
        if (!mEquipData.ContainsKey(pos))
            return null;
        return mEquipData[pos];
    }

    //重铸结果返回
    public void GetEuqipRecastRes(bag.EquipInfo _msg)
    {
        //Debug.Log("重铸返回   " + _msg.position + "   " + _msg.equip.configId + "   " + _msg.equip.quality);
        // position<0 是身上装备  >0是背包道具
        if (_msg.position < 0)
        {
            mEquipData[Math.Abs(_msg.position)] = _msg.equip;
        }
        else
        {
            mItemData[_msg.equip.id] = _msg.equip;
        }
        CSItemCountManager.Instance.OnEquipAttrChange(_msg.equip);
        TABLE.ITEM cfg;
        if (ItemTableManager.Instance.TryGetValue(_msg.equip.configId, out cfg))
        {
            if (!mItemCfgData.ContainsKey(_msg.equip.configId))
            {
                mItemCfgData.Add(_msg.equip.configId, cfg);
            }
        }
    }
    //卧龙龙技洗练返回
    public void GetWoLongLongJiBack(wolong.WoLongXiLianResponse _mes)
    {
        if (_mes.result.position < 0)
        {
            mEquipData[Math.Abs(_mes.result.position)] = _mes.result.equip;
        }
        else
        {
            mItemData[_mes.result.equip.id] = _mes.result.equip;
        }
    }
    //卧龙龙技选中洗练返回
    public void GetWoLongLongJiConfirmBack(wolong.WoLongXiLianSelectResponse _mes)
    {
        if (_mes.result.position < 0)
        {
            mEquipData[Math.Abs(_mes.result.position)] = _mes.result.equip;
        }
        else
        {
            mItemData[_mes.result.equip.id] = _mes.result.equip;
        }
        CSItemCountManager.Instance.OnEquipAttrChange(_mes.result.equip);
    }
    //按部位返回身上的装备
    List<bag.EquipInfo> EqiupWearPosList = new List<EquipInfo>();
    public int GetEquipWearPos(TABLE.ITEM _itemCfg)
    {
        EqiupWearPosList.Clear();
        int pos = 0;
        if (IsNormalEquip(_itemCfg))
        {
            if (_itemCfg.subType == 5) //5   6
            {
                List<bag.EquipInfo> list = EqiupWearPosList;
                GetEquipInfoByPos(_itemCfg.subType, list);
                if (list.Count == 0)
                {
                    pos = 5;
                }
                else if (list.Count == 1)
                {
                    pos = (list[0].position == 5) ? 6 : 5;
                }
                else if (list.Count == 2)
                {
                    int fightScoreL = CSItemCountManager.Instance.GetFightScore(list[0].equip.id);
                    int fightScoreR = CSItemCountManager.Instance.GetFightScore(list[1].equip.id);
                    if (fightScoreL < fightScoreR)
                    {
                        pos = (list[0].position == 5) ? 5 : 6;
                    }
                    else
                    {
                        pos = (list[1].position == 5) ? 5 : 6;
                    }
                }
            }
            else if (_itemCfg.subType == 6) // 7  8
            {
                List<bag.EquipInfo> list = EqiupWearPosList;
                GetEquipInfoByPos((int)_itemCfg.subType, list);
                if (list.Count == 0)
                {
                    pos = 7;
                }
                else if (list.Count == 1)
                {
                    pos = (list[0].position == 8) ? 7 : 8;
                }
                else if (list.Count == 2)
                {
                    int fightScoreL = CSItemCountManager.Instance.GetFightScore(list[0].equip.id);
                    int fightScoreR = CSItemCountManager.Instance.GetFightScore(list[1].equip.id);
                    if (fightScoreL < fightScoreR)
                    {
                        pos = (list[0].position == 7) ? 7 : 8;
                    }
                    else
                    {
                        pos = (list[1].position == 7) ? 7 : 8;
                    }
                }
            }
            else
            {
                pos = equipPosToIndex[(int)_itemCfg.subType];
            }
        }

        if (IsWoLongEquip(_itemCfg))
        {
            if (_itemCfg.subType == 105) //5   6
            {
                List<bag.EquipInfo> list = EqiupWearPosList;
                GetEquipInfoByPos(_itemCfg.subType, list);
                if (list.Count == 0)
                {
                    pos = 105;
                }
                else if (list.Count == 1)
                {
                    pos = (list[0].position == 105) ? 106 : 105;
                }
                else if (list.Count == 2)
                {
                    int fightScoreL = CSItemCountManager.Instance.GetFightScore(list[0].equip.id);
                    int fightScoreR = CSItemCountManager.Instance.GetFightScore(list[1].equip.id);
                    if (fightScoreL < fightScoreR)
                    {
                        pos = (list[0].position == 105) ? 105 : 106;
                    }
                    else
                    {
                        pos = (list[1].position == 105) ? 105 : 106;
                    }
                }
            }
            else if (_itemCfg.subType == 106) // 7  8
            {
                List<bag.EquipInfo> list = EqiupWearPosList;
                GetEquipInfoByPos(_itemCfg.subType, list);
                if (list.Count == 0)
                {
                    pos = 107;
                }
                else if (list.Count == 1)
                {
                    pos = (list[0].position == 108) ? 107 : 108;
                }
                else if (list.Count == 2)
                {
                    int fightScoreL = CSItemCountManager.Instance.GetFightScore(list[0].equip.id);
                    int fightScoreR = CSItemCountManager.Instance.GetFightScore(list[1].equip.id);
                    if (fightScoreL < fightScoreR)
                    {
                        pos = (list[0].position == 107) ? 107 : 108;
                    }
                    else
                    {
                        pos = (list[1].position == 107) ? 107 : 108;
                    }
                }
            }
            else
            {
                pos = equipPosToIndex[(int)_itemCfg.subType];
            }
        }
        return pos;
    }

    //返回背包里可以重铸的装备
    public void GetAllRecastBagEquip(Dictionary<int, bag.BagItemInfo> tempInfoDic)
    {
        CSBetterLisHot<BagItemInfo> list = Pool.GetSystemClass<CSBetterLisHot<BagItemInfo>>();
        list.Clear();
        tempInfoDic.Clear();
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                if (iter.Current.Value.quality != 5 && cfg.levClass != 0)
                {
                    list.Add(iter.Current.Value);
                }
            }
        }
        list.Sort((a, b) => { return b.quality - a.quality; });
        for (int i = 0; i < list.Count; i++)
        {
            tempInfoDic.Add(list[i].bagIndex, list[i]);
        }
        Pool.Recycle(list);
    }

    //返回背包里可以洗练的装备
    public void GetAllRefineBagEquip(Dictionary<int, bag.BagItemInfo> tempInfoDic)
    {
        CSBetterLisHot<BagItemInfo> list = Pool.GetSystemClass<CSBetterLisHot<BagItemInfo>>();
        list.Clear();
        tempInfoDic.Clear();
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                list.Add(iter.Current.Value);
            }
        }
        list.Sort((a, b) => { return b.quality - a.quality; });
        for (int i = 0; i < list.Count; i++)
        {
            tempInfoDic.Add(list[i].bagIndex, list[i]);
        }
        Pool.Recycle(list);
    }

    //拿到item数量，索引是cfgId
    public long GetItemCount(int _cfgId)
    {
        return CSItemCountManager.Instance.GetItemCount(_cfgId);
    }

    /// <summary>
    /// 拿所有类型item的数量（货币加道具加装备）
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns></returns>
    public long GetAllItemCount(int configId)
    {
        return CSItemCountManager.Instance.GetItemCount(configId);
    }

    /// <summary>
    /// 根据configId获取身上装备的背包中所有的该物品
    /// </summary>
    /// <param name="_cfgId"></param>
    /// <returns></returns>
    public long GetItemAndEquipItemCount(int _cfgId)
    {
        long num = GetItemCount(_cfgId);
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.configId == _cfgId)
            {
                num = num + iter.Current.Value.count;
            }
        }

        return num;
    }

    //返回是否比身上同部位的装备好(背包中显示提升剪头用)
    public bool GetEquipCompareResult(TABLE.ITEM _cfg, BagItemInfo _info)
    {
        int equipedMinScore = CSItemCountManager.Instance.GetEquipedMinFightScore(_cfg.id);
        if (equipedMinScore <= 0)
        {
            return true;
        }
        int currentScore = CSItemCountManager.Instance.GetFightScore(_info.id);
        //Debug.Log($"当前身上最小分数{equipedMinScore}     当前装备{_cfg.name}{_info.quality}     评分{currentScore}");
        return currentScore > equipedMinScore;
    }

    //返回背包中同部位的最好装备
    ILBetterList<BagItemInfo> BestEquipList = new ILBetterList<BagItemInfo>();
    public BagItemInfo GetBestEquipByPos(int _pos)
    {
        BagItemInfo info;
        BestEquipList.Clear();
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM _cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (_cfg.type == (int)ItemType.Equip)
            {
                if (IsEquipCareerSexEqual(_cfg) && _cfg.subType == _pos)
                {
                    if (IsWoLongEquip(_cfg))
                    {
                        if (CSWoLongInfo.Instance.GetWoLongLevel() >= _cfg.wolongLv)
                        {
                            BestEquipList.Add(iter.Current.Value);
                        }
                    }
                    else if (IsNormalEquip(_cfg))
                    {
                        if (CSMainPlayerInfo.Instance.Level >= _cfg.level)
                        {
                            if (IsEquipCareerSexEqual(_cfg) && _cfg.subType == _pos)
                            {
                                BestEquipList.Add(iter.Current.Value);
                            }
                        }
                    }
                }
            }
        }

        if (BestEquipList.Count == 1)
        {
            info = BestEquipList[0];
            return info;
        }
        else if (BestEquipList.Count > 1)
        {
            BestEquipList.Sort((a, b) =>
            {
                int powerA = CSItemCountManager.Instance.GetFightScore(a.id);
                int powerB = CSItemCountManager.Instance.GetFightScore(b.id);
                if (powerA != powerB)
                {
                    return (powerA >= powerB) ? -1 : 1;
                }
                return powerA - powerB;
            });
            info = BestEquipList[0];
            return info;
        }
        else
        {
            return null;
        }
    }

    public bool IsHasSamePosEquipInBag(int _pos)
    {
        //var iter = mItemData.GetEnumerator();
        //while (iter.MoveNext())
        //{
        //    TABLE.ITEM _cfg = GetCfg(iter.Current.Value.configId);
        //    if (IsWoLongEquip(_cfg))
        //    {
        //        if (CSWoLongInfo.Instance.GetWoLongLevel() >= _cfg.wolongLv)
        //        {
        //            if (IsEquipCareerSexEqual(_cfg) && _cfg.subType == _pos)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    if (IsNormalEquip(_cfg))
        //    {
        //        if (CSMainPlayerInfo.Instance.Level >= _cfg.level)
        //        {
        //            if (IsEquipCareerSexEqual(_cfg) && _cfg.subType == _pos)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //}
        //int[] Pos = null;
        //if (_pos < 100)
        //{
        //    Pos = UIRoleEquipPanel.normalPosIndex;
        //}
        //else if (_pos > 100)
        //{
        //    Pos = UIRoleEquipPanel.wolongPosIndex;
        //}
        return CSItemCountManager.Instance.GetHaveBetterEquipByPos(_pos);
    }

    //返回身上的卧龙套装情况
    public int GetSelfWoLongNum(int _levClass)
    {
        int num = 0;
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg) && cfg.levClass >= _levClass)
            {
                num++;
            }
        }

        return num;
    }

    //返回身上的卧龙装备id
    public List<int> GetSelfWoLongIdList(List<int> selfWoLongIdList)
    {
        selfWoLongIdList.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsWoLongEquip(cfg))
            {
                selfWoLongIdList.Add(iter.Current.Value.configId);
            }
        }

        return selfWoLongIdList;
    }

    public bool IsEquipCareerSexEqual(TABLE.ITEM _cfg)
    {
        if (_cfg.type == 2)
        {
            if (_cfg.sex == CSMainPlayerInfo.Instance.Sex || _cfg.sex == 2)
            {
                if (_cfg.career == CSMainPlayerInfo.Instance.Career || _cfg.career == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public bool IsCareerSexEqual(TABLE.ITEM _cfg)
    {
        if (_cfg != null)
        {
            if (_cfg.sex == CSMainPlayerInfo.Instance.Sex || _cfg.sex == 2)
            {
                if (_cfg.career == CSMainPlayerInfo.Instance.Career || _cfg.career == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void EquipConfigInit()
    {
        equipPosToIndex.Add(1, 1);
        equipPosToIndex.Add(2, 2);
        equipPosToIndex.Add(3, 3);
        equipPosToIndex.Add(4, 4);
        equipPosToIndex.Add(7, 9);
        equipPosToIndex.Add(8, 10);
        equipPosToIndex.Add(9, 11);
        equipPosToIndex.Add(10, 12);
        equipPosToIndex.Add(101, 101);
        equipPosToIndex.Add(102, 102);
        equipPosToIndex.Add(103, 103);
        equipPosToIndex.Add(104, 104);
        equipPosToIndex.Add(107, 109);
        equipPosToIndex.Add(108, 110);
        equipPosToIndex.Add(109, 111);
        equipPosToIndex.Add(110, 112);
    }

    #region 卧龙装备普通装备判断

    public bool IsNormalEquip(TABLE.ITEM _cfg)
    {
        if (_cfg == null) { return false; }
        if (_cfg.type == 2 && 1 <= _cfg.subType && _cfg.subType <= 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsNotValuableNormalEquip(TABLE.ITEM _cfg)
    {

        if (IsNormalEquip(_cfg) && _cfg.saleType == 2)
        {
            return true;
        }

        return false;
    }

    public bool IsValuableNormalEquip(TABLE.ITEM _cfg)
    {
        if (IsNormalEquip(_cfg) && _cfg.saleType == 1)
        {
            return true;
        }

        return false;
    }

    public bool IsWoLongEquip(TABLE.ITEM _cfg)
    {
        if (_cfg == null) { return false; }
        if (_cfg.type == 2 && 101 <= _cfg.subType && _cfg.subType <= 110)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsNotValuableWoLongEquip(TABLE.ITEM _cfg)
    {
        if (IsWoLongEquip(_cfg) && _cfg.saleType == 2)
        {
            return true;
        }

        return false;
    }

    public bool IsValuableWoLongEquip(TABLE.ITEM _cfg)
    {
        if (IsWoLongEquip(_cfg) && _cfg.saleType == 1)
        {
            return true;
        }

        return false;
    }
    string[] wolongIconNames;
    public string GetItemBaseWoLongIconName(int _levClass)
    {
        if (wolongIconNames == null)
        {
            wolongIconNames = SundryTableManager.Instance.GetSundryEffect(1081).Split('#');
        }
        if (_levClass < wolongIconNames.Length)
        {
            return wolongIconNames[_levClass];
        }
        return null;
    }
    #endregion

    #region 道具使用

    /// <summary>
    ///  用CfgID取一个BagItemInfo 每次使用需要重新取值 不能保证上一次的返回值一直存在
    /// </summary>
    /// <param name="_cfgId"></param>
    /// <returns></returns>
    public BagItemInfo GetItemInfoByCfgId(int _cfgId)
    {
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.configId == _cfgId)
            {
                return iter.Current.Value;
            }
        }

        return null;
    }

    public void UseItem(long _id, int count = 1, bool auto = false)
    {
        if (mItemData.ContainsKey(_id))
        {
            UseItem(mItemData[_id], count, auto);
        }
    }

    public void UseItem(BagItemInfo _info, int _count = 1, bool auto = false)
    {
        if (!mItemData.ContainsKey(_info.id))
        {
            return;
        }

        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(_info.configId);
        if (cfg == null)
        {
            return;
        }
        if (cfg.level > CSMainPlayerInfo.Instance.Level)
        {
            UtilityTips.ShowRedTips(1956);
            return;
        }

        if (ItemCDManager.Instance.InGroupCD(cfg.group))
        {
            UtilityTips.ShowRedTips(1524, cfg.name);
            return;
        }

        //1.特殊ID逻辑 2.特殊类型操作   3.打开界面  4.发消息使用

        if (specialItemUse.ContainsKey(cfg.id))
        {
            specialItemUse[cfg.id]?.Invoke(_info);
            return;
        }

        if (specialTypeItemUse.ContainsKey(cfg.type * 1000 + cfg.subType))
        {
            specialTypeItemUse[cfg.type * 1000 + cfg.subType]?.Invoke(_info, _count);
            return;
        }

        if (cfg.funcopen != 0)
        {
            UtilityPanel.JumpToPanel(cfg.funcopen);
            UIManager.Instance.ClosePanel<UIBagPanel>();
            return;
        }
        //ItemCDManager.Instance.EnterGroupCD(cfg.group, cfg.itemcd * 0.001f);
        Net.ReqUseItemMessage(_info.bagIndex, _count, auto, 0, _info.id);
    }

    #endregion

    /// <summary>
    /// 当前是否穿戴武器
    /// </summary>
    /// <returns></returns>
    public bool GetWeatherWeaponArmed()
    {
        return mEquipData.ContainsKey(1);
    }

    /// <summary>
    /// 拿到背包中可吞噬装备
    /// </summary>
    /// <returns></returns>
    public void GetdevourItems(List<BagItemInfo> bagItemInfos)
    {
        // bagItemInfos.Clear();
        // var iter = mItemData.GetEnumerator();
        // while (iter.MoveNext())
        // {
        //     TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
        //     if (cfg.devouredExp != 0)
        //     {
        //         bagItemInfos.Add(iter.Current.Value);
        //     }
        // }
    }

    public void GetSelfWoLongEquipData(Dictionary<int, bag.BagItemInfo> dic, bool _SelecNoLongLi = false)
    {
        dic.Clear();
        CSBetterLisHot<EquipInfo> list = Pool.GetSystemClass<CSBetterLisHot<EquipInfo>>();
        list.Clear();
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsWoLongEquip(cfg) && cfg.levClass > 0)
            {
                if (_SelecNoLongLi)
                {
                    if (iter.Current.Value.baseAffixs.Count != 0 && iter.Current.Value.intensifyAffixs.Count != 0)
                    {
                        EquipInfo info = new EquipInfo();
                        info.position = iter.Current.Key;
                        info.equip = iter.Current.Value;
                        list.Add(info);
                    }
                }
                else
                {
                    EquipInfo info = new EquipInfo();
                    info.position = iter.Current.Key;
                    info.equip = iter.Current.Value;
                    list.Add(info);
                }
            }
        }
        list.Sort((a, b) => { return b.equip.quality - a.equip.quality; });
        for (int i = 0; i < list.Count; i++)
        {
            dic.Add(list[i].position, list[i].equip);
        }
        Pool.Recycle(list);
    }

    public void GetBagWoLongEquipData(Dictionary<int, bag.BagItemInfo> dic, bool _SelecNoLongLi = false)
    {
        dic.Clear();
        CSBetterLisHot<BagItemInfo> list = Pool.GetSystemClass<CSBetterLisHot<BagItemInfo>>();
        list.Clear();
        if (mItemData == null)
        {
            return;
        }

        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsWoLongEquip(cfg) && cfg.levClass > 0)
            {
                if (_SelecNoLongLi)
                {
                    if (iter.Current.Value.baseAffixs.Count != 0 && iter.Current.Value.intensifyAffixs.Count != 0)
                    {
                        list.Add(iter.Current.Value);
                    }
                }
                else
                {
                    list.Add(iter.Current.Value);
                }
            }
        }

        list.Sort((a, b) => { return (b.quality - a.quality); });
        for (int i = 0; i < list.Count; i++)
        {
            dic.Add(list[i].bagIndex, list[i]);
        }
        list.Clear();
        Pool.Recycle(list);
    }

    public void RecycleObject(object data)
    {
        if (data != null)
        {
            Pool.Recycle(data);
        }
    }

    /// <summary>
    /// 根据BagItemInfo判断该装备是否装备在身上
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool IsEquip(BagItemInfo info)
    {
        if (mEquipData != null && mEquipData.ContainsValue(info))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 根据配置Id判断该装备是否装备在身上
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public bool IsEquip(int configId)
    {
        if (mEquipData != null)
        {
            var iter = mEquipData.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.configId == configId)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 判断是否是卧龙丹
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsWoLongDan(TABLE.ITEM item)
    {
        if (item == null) return false;
        if (item.type == 3 && item.subType == 3)
        {
            return true;
        }

        return false;
    }

    ILBetterList<int> equiplvList = new ILBetterList<int>(12);

    /// <summary>
    /// 获得该装备是否比身上高
    /// </summary>
    /// <param name="_cfg"></param>
    /// <returns></returns>
    public bool GetEquipLevelByPos(ITEM _cfg)
    {
        bool isNormalEquip = IsNormalEquip(_cfg);

        var playinfo = CSMainPlayerInfo.Instance;
        var wolonglevel = CSWoLongInfo.Instance.ReturnWoLongInfo()?.wolongLevel;
        bool islevel = isNormalEquip ? _cfg.level <= playinfo.Level : _cfg.wolongLv <= wolonglevel;

        if (!islevel || (_cfg.sex != 2 && _cfg.sex != playinfo.Sex) || (_cfg.career != 0 && _cfg.career != playinfo.Career))
        {
            return false;
        }

        equiplvList.Clear();
        int lv = 0;

        var list = GetPosByItem(_cfg);

        //如果没有装备直接return true;
        if (list.Count == 0)
            return true;

        for (int i = 0; i < list.Count; i++)
        {
            ITEM cfg = GetCfg(list[i].configId);
            if (IsNormalEquip(cfg))
            {
                lv = (cfg.level > lv) ? cfg.level : lv;
                return _cfg.level - lv > 0;
            }
            if (IsWoLongEquip(cfg))
            {
                lv = (cfg.levClass > lv) ? cfg.levClass : lv;
                return _cfg.levClass - lv > 0;
            }

        }

        return false;
    }

    ILBetterList<BagItemInfo> equipList = new ILBetterList<BagItemInfo>(); //用于返回位置列表

    /// <summary>
    /// 获得该装备对应位置上的装备列表
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public ILBetterList<BagItemInfo> GetPosByItem(ITEM item)
    {
        equipList.Clear();

        if (IsWoLongEquip(item))
        {
            var list = UIRoleEquipPanel.wolongPosIndex;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == item.subType)
                {
                    BagItemInfo bagItemInfo;
                    int pos = UIRoleEquipPanel.wolongEquipIndex[i];
                    if (mEquipData.TryGetValue(pos, out bagItemInfo))
                        equipList.Add(bagItemInfo);
                }


            }

        }
        else
        {
            var list = UIRoleEquipPanel.normalPosIndex;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == item.subType)
                {
                    BagItemInfo bagItemInfo;
                    int pos = UIRoleEquipPanel.normalEquipIndex[i];
                    if (mEquipData.TryGetValue(pos, out bagItemInfo))
                        equipList.Add(bagItemInfo);

                }

                //equipList.Add(UIRoleEquipPanel.normalEquipIndex[i]);

            }
        }

        return equipList;
    }



    #region 重铸洗练红点
    public bool GetEquipRecastRedpointState()
    {
        //判断功能是否开放
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu))
        {
            return false;
        }

        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                if (iter.Current.Value.quality != 5 && cfg.levClass > 0)
                {
                    if (IsEquipRecastCostEnough(iter.Current.Value, cfg))
                    {
                        return true;
                    }
                }
            }
        }

        //var iter1 = mItemData.GetEnumerator();
        //while (iter1.MoveNext())
        //{
        //    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter1.Current.Value.configId);
        //    if (IsNormalEquip(cfg))
        //    {
        //        if (iter1.Current.Value.quality != 5)
        //        {
        //            if (IsEquipRecastCostEnough(iter.Current.Value, cfg))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //}

        return false;
    }

    public bool IsEquipRecastCostEnough(BagItemInfo _info, TABLE.ITEM _cfg, bool isJudgeTopCost = true)
    {
        if (_info.quality == 5)
        {
            return false;
        }
        //紫装判断一键重铸消耗  紫装以下判断普通洗练消耗
        if (_info.quality == 4 && isJudgeTopCost)
        {
            TABLE.CHONGZHUTOPCOST costData;
            ChongZhuTopCostTableManager.Instance.TryGetValue(_cfg.levClass * 10000 + _info.quality, out costData);
            if (costData == null)
            {
                return false;
            }
            else
            {
                long money = CSItemCountManager.Instance.GetItemCount(costData.payType);
                long goods = CSItemCountManager.Instance.GetItemCount(costData.costItemID);
                long goods2 = CSItemCountManager.Instance.GetItemCount(costData.costItemID2);
                if ((money >= costData.price) && (goods >= costData.num) && (goods2 >= costData.num2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            TABLE.CHONGZHUCOST costData;
            ChongZhuCostTableManager.Instance.TryGetValue(_cfg.levClass + 1, out costData);
            if (costData == null)
            {
                return false;
            }
            else
            {
                long money = CSItemCountManager.Instance.GetItemCount(costData.payType);
                long goods = CSItemCountManager.Instance.GetItemCount(costData.costItemID);
                if ((money >= costData.price) && (goods >= costData.num))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public bool GetEquipRefineRedpointState()
    {
        //判断功能是否开放
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
        {
            return false;
        }

        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                if (IsEquipRefineCostEnough(iter.Current.Value, cfg))
                {
                    return true;
                }
            }
        }
        //var iter1 = mItemData.GetEnumerator();
        //while (iter1.MoveNext())
        //{
        //    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter1.Current.Value.configId);
        //    if (IsNormalEquip(cfg))
        //    {
        //        if (IsEquipRefineCostEnough(iter1.Current.Value, cfg))
        //        {
        //            return true;
        //        }
        //    }
        //}
        return false;
    }

    public bool IsEquipRefineCostEnough(BagItemInfo _info, TABLE.ITEM _cfg)
    {
        //橙色装备不显示红点
        if (_info.quality == 5)
        {
            return false;
        }
        //如果全部是橙色属性，不显示红点
        bool isAllAttrTopLv = true;
        int qua = 0;
        for (int i = 0; i < _info.randAttrValues.Count; i++)
        {
            RandAttr attr = _info.randAttrValues[i];
            if (attr.type == 1)
            {
                //value2！=0 是有大小值的属性  =0是单属性
                if (attr.value2 != 0)
                {
                    qua = (attr.configId1 > attr.configId2) ? attr.configId1 : attr.configId2;
                }
                else
                {
                    qua = attr.configId1;
                }
                if (qua < 5)
                {
                    isAllAttrTopLv = false;
                    continue;
                }
            }
        }
        if (isAllAttrTopLv)
        {
            return false;
        }
        //如果有免费次数显示红点
        if (_info.freeXiLianCount > 0)
        {
            return true;
        }
        TABLE.XILIANCOST costData = XiLianCostTableManager.Instance.GetCfg(_cfg.level);
        if (costData == null)
        {
            return false;
        }
        else
        {
            long money = CSItemCountManager.Instance.GetItemCount((int)costData.payType);
            long goods = GetItemCount((int)costData.costItemID);
            if ((money >= costData.price[0]) && (goods >= costData.num[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region 无装备时装备格子点击
    /// <summary>
    /// 剔除当前获取普通装备按钮
    /// </summary>
    /// <param name="list"></param>
    public void ScreenButtonForNormalEquipObtain(ILBetterList<NormalEquipObtain> list)
    {
        if (list == null) return;
        //未开启首充或者已完成领取首充
        if (/*!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_rechargefirst) ||*/
            CSVipInfo.Instance.IsFinishRechargeFirst())
        {
            if (list.Contains(NormalEquipObtain.FirstCharge))
            {
                list.Remove(NormalEquipObtain.FirstCharge);
            }
        }


        //开服礼包未开启或者已关闭
        /*if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc))
        {
            if (list.Contains(NormalEquipObtain.OpenServicePackage))
            {
                list.Remove(NormalEquipObtain.OpenServicePackage);
            }
        }
        else
        {*/
        SpecialActivityData specialActivityData;
        if (CSOpenServerACInfo.Instance.Rewards.TryGetValue(10106, out specialActivityData))
        {
            if (CSServerTime.Instance.TotalMillisecond >= specialActivityData.endTime)
            {
                if (list.Contains(NormalEquipObtain.OpenServicePackage))
                {
                    list.Remove(NormalEquipObtain.OpenServicePackage);
                }
            }
        }

        // else
        // {
        //     if (list.Contains(NormalEquipObtain.OpenServicePackage))
        //     {
        //         list.Remove(NormalEquipObtain.OpenServicePackage);
        //     }
        // }
        /*}*/



        //未开启野外boss
        // int levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
        int openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(30);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(NormalEquipObtain.FieldBoss))
            {
                list.Remove(NormalEquipObtain.FieldBoss);
            }
        }


        //未开启个人boss
        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(11);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(NormalEquipObtain.PersonalBoss))
            {
                list.Remove(NormalEquipObtain.PersonalBoss);
            }
        }


        //未开启地牢围攻
        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(34);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(34);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(NormalEquipObtain.DungeonSiege))
            {
                list.Remove(NormalEquipObtain.DungeonSiege);
            }
        }

    }

    /// <summary>
    /// 是否有获取普通装备途径
    /// </summary>
    /// <returns></returns>
    public bool IsHasNormalEquipObtain()
    {
        if (/*UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_rechargefirst) &&*/
            !CSVipInfo.Instance.IsFinishRechargeFirst())
            return true;


        /*if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc))
        {*/
        SpecialActivityData specialActivityData;
        if (CSOpenServerACInfo.Instance.Rewards.TryGetValue(10106, out specialActivityData))
        {
            if (CSServerTime.Instance.TotalMillisecond < specialActivityData.endTime)
                return true;
        }
        /*}*/

        // int levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
        int openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(30);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(11);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(34);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(34);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
        CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        return false;
    }

    /// <summary>
    /// 剔除当前获取卧龙装备按钮
    /// </summary>
    /// <param name="list"></param>
    public void ScreenButtonForWolongEquipObtain(ILBetterList<WolongEquipObtain> list)
    {
        if (list == null) return;
        //未开启野外boss
        // int levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
        int openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(30);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(WolongEquipObtain.FieldBoss))
            {
                list.Remove(WolongEquipObtain.FieldBoss);
            }
        }

        //未开启个人boss
        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(11);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(WolongEquipObtain.PersonalBoss))
            {
                list.Remove(WolongEquipObtain.PersonalBoss);
            }
        }

        //未开启世界boss
        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(14);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(14);
        if (/*CSMainPlayerInfo.Instance.Level < levellimt ||*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(WolongEquipObtain.WorldBoss))
            {
                list.Remove(WolongEquipObtain.WorldBoss);
            }
        }

        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(29);
        //未开启寻宝
        if (/*!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_seekTreasure)*/
            CSMainPlayerInfo.Instance.ServerOpenDay < openDays)
        {
            if (list.Contains(WolongEquipObtain.SeekTreasure))
            {
                list.Remove(WolongEquipObtain.SeekTreasure);
            }
        }

    }

    /// <summary>
    /// 是否有获取卧龙装备途径
    /// </summary>
    /// <returns></returns>
    public bool IsHasWolongEquipObtain()
    {
        // int levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
        int openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(30);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(11);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        // levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(14);
        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(14);
        if (/*CSMainPlayerInfo.Instance.Level >= levellimt &&*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        openDays = FuncOpenTableManager.Instance.GetFuncOpenOpenday(29);
        if (/*UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_seekTreasure)*/
            CSMainPlayerInfo.Instance.ServerOpenDay >= openDays)
            return true;

        return false;
    }
    #endregion

    #region 背包红点
    public void GetNewBoxRedPoint(long _id)
    {
        if (!newBoxList.Contains(_id))
        {
            newBoxList.Add(_id);
        }
    }
    public void CancelNewBoxRedPoint(long _id)
    {
        if (newBoxList.Contains(_id))
        {
            newBoxList.Remove(_id);
        }
    }
    public bool GetBoxRedStateById(long _id)
    {
        return newBoxList.Contains(_id);
    }
    public bool GetBagBoxRedPointState()
    {
        return newBoxList.Count > 0 ? true : false;
    }
    public bool GetBagEquipRedPointState()
    {
        CSItemCountManager ins = CSItemCountManager.Instance;
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsNormalEquip(cfg))
            {
                if (ins.GetFightScore(iter.Current.Value.id) > ins.GetEquipedMinFightScore(cfg.id))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool GetBagRedPointState()
    {
        return GetBagBoxRedPointState();
    }
    #endregion

    #region  祝福油红点
    int zhufuRedCount = 0;
    public bool ZhuFuRedPointState()
    {
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1077), out zhufuRedCount);
        if (CSItemCountManager.Instance.GetItemCount(50000028) > zhufuRedCount)
        {
            if (mEquipData.ContainsKey(1))
            {
                if (mEquipData[1].weaponLuckLv < 7)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    #region  卧龙洗练相关
    /// <summary>
    /// 计算身上装备的基础词缀数量，key  --》 skillGroupId   value --》 count   参数！=0 则计算时去除此ID装备的计算
    /// </summary>
    /// <param name="_id"></param>
    public int GetWoLongLongLiAffixCount(int _id, long _itemID = 0)
    {
        WoLongRandomAttrTableManager wolongIns = WoLongRandomAttrTableManager.Instance;
        int count = 0;
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.id != _itemID)
            {
                for (int i = 0; i < iter.Current.Value.baseAffixs.Count; i++)
                {
                    if (wolongIns.GetWoLongRandomAttrParameter(iter.Current.Value.baseAffixs[i].id) == _id)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
    Dictionary<int, Dictionary<int, WoLongAffixEffect>> wolongAffixCount = new Dictionary<int, Dictionary<int, WoLongAffixEffect>>();

    public void CalculateWLAffixCount(long _id = 0)
    {
        wolongAffixCount.Clear();
        WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
        for (var it = mEquipData.GetEnumerator(); it.MoveNext();)
        {
            var value = it.Current.Value;
            if (_id != 0 && value.id == _id)
            {
                continue;
            }
            for (int i = 0; i < value.baseAffixs.Count; i++)
            {
                int skillId = ins.GetWoLongRandomAttrParameter(value.baseAffixs[i].id);
                if (!wolongAffixCount.ContainsKey(skillId))
                {
                    wolongAffixCount.Add(skillId, new Dictionary<int, WoLongAffixEffect>());
                }
            }
            for (int i = 0; i < value.intensifyAffixs.Count; i++)
            {
                TABLE.WOLONGRANDOMATTR cfg = ins.GetCfgById(value.intensifyAffixs[i].id);
                TABLE.ZHANCHONGCIZHUIEFFECT cizhui = ZhanChongCiZhuiEffectTableManager.Instance.GetDataByType(cfg.parameter);
                Dictionary<int, WoLongAffixEffect> temp_dic = wolongAffixCount[cfg.bindParam];
                if (!temp_dic.ContainsKey(cizhui.type))
                {
                    temp_dic.Add(cizhui.type, new WoLongAffixEffect());
                }
                WoLongAffixEffect eff = temp_dic[cizhui.type];
                eff.id = cizhui.id;
                eff.per = cizhui.per;
                eff.plus = cizhui.plus;
                eff.value = eff.value + value.intensifyAffixs[i].effectValue;
            }
        }
    }
    public Dictionary<int, WoLongAffixEffect> GetWoLongIntenAffixState(int _skillId, long _id = 0)
    {
        CalculateWLAffixCount(_id);
        if (wolongAffixCount.ContainsKey(_skillId))
        {
            return wolongAffixCount[_skillId];
        }
        return null;
    }
    public bool GetLongJiRefineRedState()
    {
        //判断功能是否开放
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_LongJi))
        {
            return false;
        }
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsWoLongEquip(cfg))
            {
                if (IsLongJiCostEnough(iter.Current.Value, cfg))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsLongJiCostEnough(BagItemInfo _info, TABLE.ITEM _cfg)
    {
        TABLE.ZHANCHONGXILIANCOSTNEW costData = ZhanChongXiLianCostNewTableManager.Instance.GetCfg(_cfg.levClass);
        if (costData == null)
        {
            return false;
        }
        else
        {
            long money = CSItemCountManager.Instance.GetItemCount(costData.hunjicost[0]);
            long goods = CSItemCountManager.Instance.GetItemCount(costData.hunjicost1[0]);
            if ((money >= costData.hunjicost[1]) && (goods >= costData.hunjicost1[1]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool GetLongLiRefineRedState()
    {
        //判断功能是否开放
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_LongLi))
        {
            return false;
        }
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (IsWoLongEquip(cfg))
            {
                if (IsLongLiCostEnough(iter.Current.Value, cfg))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsLongLiCostEnough(BagItemInfo _info, TABLE.ITEM _cfg)
    {
        TABLE.ZHANCHONGXILIANCOSTNEW costData = ZhanChongXiLianCostNewTableManager.Instance.GetCfg(_cfg.levClass);
        if (costData == null)
        {
            return false;
        }
        else
        {
            long money = CSItemCountManager.Instance.GetItemCount(costData.hunlicost[0]);
            long goods = CSItemCountManager.Instance.GetItemCount(costData.hunlicost1[0]);
            if ((money >= costData.hunlicost[1]) && (goods >= costData.hunlicost1[1]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsHasBaseAffix()
    {
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.baseAffixs.Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsHasLongJi()
    {
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.longJis.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void GetAllLongJiInfo(Dictionary<int, UILongJiTipsPanel.LongJiEff> _list)
    {
        int career = CSMainPlayerInfo.Instance.Career;
        var iter = mEquipData.GetEnumerator();
        while (iter.MoveNext())
        {
            RepeatedField<WolongRandomEffect> temp_List = iter.Current.Value.longJis;
            for (int i = 0; i < temp_List.Count; i++)
            {
                TABLE.WOLONGRANDOMATTR wolongAttr;
                if (WoLongRandomAttrTableManager.Instance.TryGetValue(temp_List[i].id, out wolongAttr))
                {
                    //Debug.Log($"{wolongAttr.career}  {wolongAttr.parameter}  {wolongAttr.name}");
                    if (wolongAttr.career == career)
                    {
                        if (!_list.ContainsKey(wolongAttr.parameter))
                        {
                            _list.Add(wolongAttr.parameter, new UILongJiTipsPanel.LongJiEff(temp_List[i]));
                        }
                        else
                        {
                            _list[wolongAttr.parameter].AddValue(temp_List[i].effectValue);
                        }
                    }
                }
            }
        }
    }

    public void GetAllLongLiInfo(Dictionary<int, LongLiBaseAff> baseAffixCount, Dictionary<int, Dictionary<int, LongLiEff>> wolongAffixCount)
    {
        WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
        for (var it = mEquipData.GetEnumerator(); it.MoveNext();)
        {
            var value = it.Current.Value;
            for (int i = 0; i < value.baseAffixs.Count; i++)
            {
                int skillId = ins.GetWoLongRandomAttrParameter(value.baseAffixs[i].id);
                //基础词缀数量计算
                if (baseAffixCount.ContainsKey(skillId))
                {
                    baseAffixCount[skillId].value++;
                }
                else
                {
                    LongLiBaseAff t_base = new LongLiBaseAff();
                    t_base.id = skillId;
                    t_base.value = 1;
                    baseAffixCount.Add(skillId, t_base);
                }
                //强化词缀用基础词缀ID存
                if (!wolongAffixCount.ContainsKey(skillId))
                {
                    wolongAffixCount.Add(skillId, new Dictionary<int, LongLiEff>());
                }
            }
            for (int i = 0; i < value.intensifyAffixs.Count; i++)
            {
                TABLE.WOLONGRANDOMATTR cfg = ins.GetCfgById(value.intensifyAffixs[i].id);
                TABLE.ZHANCHONGCIZHUIEFFECT cizhui = ZhanChongCiZhuiEffectTableManager.Instance.GetDataByType(cfg.parameter);
                Dictionary<int, LongLiEff> temp_dic = wolongAffixCount[cfg.bindParam];
                if (!temp_dic.ContainsKey(cizhui.type))
                {
                    temp_dic.Add(cizhui.type, new LongLiEff());
                }
                LongLiEff eff = temp_dic[cizhui.type];
                eff.id = cizhui.id;
                eff.per = cizhui.per;
                eff.plus = cizhui.plus;
                eff.value = eff.value + value.intensifyAffixs[i].effectValue;
            }
        }
    }

    #endregion

    Dictionary<int, ResItemUsedDaily> itemUseCountDic = new Dictionary<int, ResItemUsedDaily>(5);
    #region 道具使用次数
    public void GetAllItemUseCountMes(ResItemUsedDailyTotal _mes)
    {
        itemUseCountDic.Clear();
        for (int i = 0; i < _mes.useItemDailyInfo.Count; i++)
        {
            itemUseCountDic.Add(_mes.useItemDailyInfo[i].group, _mes.useItemDailyInfo[i]);
        }
        FNDebug.Log($"[物品使用次数变更]");
        mClientEvent.SendEvent(CEvent.OnItemUsedTimesChanged);
    }
    public void GetItemUseCountChangeMes(ResItemUsedDaily _mes)
    {
        if (itemUseCountDic.ContainsKey(_mes.group))
        {
            itemUseCountDic[_mes.group] = _mes;
        }
        else
        {
            itemUseCountDic.Add(_mes.group, _mes);
        }
        FNDebug.Log($"[物品使用次数变更]");
        mClientEvent.SendEvent(CEvent.OnItemUsedTimesChanged);
    }
    public ResItemUsedDaily RetuanSingleItemUseCountMes(int _group)
    {
        if (itemUseCountDic.ContainsKey(_group))
        {
            return itemUseCountDic[_group];
        }
        return null;
    }
    #endregion
}