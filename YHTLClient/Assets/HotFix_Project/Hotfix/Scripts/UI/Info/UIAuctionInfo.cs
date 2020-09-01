using auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIAuctionInfo : CSInfo<UIAuctionInfo>
{
    SelfAuctionItems sellMes;
    AllAuctionItems buyMes;
    List<AuctionItemInfo> notShowList = new List<AuctionItemInfo>();
    List<AuctionItemInfo> ShowList = new List<AuctionItemInfo>();
    List<long> attenList = new List<long>();
    PoolHandleManager pool = new PoolHandleManager();
    public void Init()
    {
        Net.ReqGetAuctionShelfMessage();
    }

    #region sell
    public void GetSellMes(SelfAuctionItems _sellMes)
    {
        sellMes = _sellMes;
    }
    public void GetSelfSellListAdd(AddToShelfResponse mes)
    {
        if (sellMes == null)
        {
            return;
        }
        sellMes.items.Add(mes.item);
    }
    public void GetSelfSellListARemove(ItemIdMsg mes)
    {
        if (sellMes == null)
        {
            return;
        }
        for (int i = 0; i < sellMes.items.Count; i++)
        {
            if (sellMes.items[i].lid == mes.lid)
            {
                sellMes.items.Remove(sellMes.items[i]);
                return;
            }
        }
    }
    public void GetUnlockShelve(int _num)
    {
        sellMes.shelve = _num;
    }

    public SelfAuctionItems ReturnSellMes()
    {
        return sellMes;
    }
    public void GetSellBagMes(List<AuctionItemData> _list)
    {
        //背包数据
        _list.Clear();
        var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM _cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (/*CSMainPlayerInfo.Instance.Level >= _cfg.level &&*/ _cfg.saleType != 3)
            {
                if (iter.Current.Value.bind == 0)
                {
                    AuctionItemData aucData = pool.GetCustomClass<AuctionItemData>();
                    aucData.type = 1;
                    aucData.bagInfo = iter.Current.Value;
                    _list.Add(aucData);
                }
            }
        }
        //怀旧装备数据
        ILBetterList<NostalgiaBagClass> nosData = CSNostalgiaEquipInfo.Instance.GetSortList(NostalgiaSelectType.BAG, true);
        for (int i = 0; i < nosData.Count; i++)
        {
            AuctionItemData aucData = pool.GetCustomClass<AuctionItemData>();
            aucData.type = 3;
            aucData.bagInfo = nosData[i].bagiteminfo;
            _list.Add(aucData);
        }
        //幻灵背包数据
        var hdIter = CSHandBookManager.Instance.GetHandbookData().GetEnumerator();
        while (hdIter.MoveNext())
        {
            var value = hdIter.Current.Value;
            if (!value.Bind)
            {
                if (value.LinkedItem.saleType != 3 && !value.Setuped && value.HandBook.Quality == 1 && value.HandBook.Level == 1)
                {
                    AuctionItemData aucData = pool.GetCustomClass<AuctionItemData>();
                    aucData.type = 2;
                    aucData.tujianId = value.Guid;
                    aucData.handbookId = value.HandBookId;
                    aucData.tujianCfgId = value.LinkedItem.id;
                    _list.Add(aucData);
                }
            }
        }
    }
    #endregion

    #region buy

    public void GetBuyMes(AllAuctionItems _buyMes)
    {
        buyMes = _buyMes;
        notShowList.Clear();
        ShowList.Clear();
        for (int i = 0; i < buyMes.items.Count; i++)
        {
            if (buyMes.items[i].showTime > 0)
            {
                ShowList.Add(buyMes.items[i]);
            }
            else
            {
                notShowList.Add(buyMes.items[i]);
            }
        }
        //Debug.Log("当前商品数量  " + buyMes.items.Count);
    }
    public void GetBuyResult(ItemIdMsg _buyMes)
    {
        if (buyMes == null)
        {
            return;
        }
        for (int i = 0; i < buyMes.items.Count; i++)
        {
            if (_buyMes.lid == buyMes.items[i].lid)
            {
                if (_buyMes.count != 0)
                {
                    buyMes.items[i].item.count = _buyMes.count;
                }
                else
                {
                    buyMes.items.Remove(buyMes.items[i]);
                    break;
                }
            }
        }
        notShowList.Clear();
        ShowList.Clear();
        for (int i = 0; i < buyMes.items.Count; i++)
        {
            if (buyMes.items[i].showTime > 0)
            {
                ShowList.Add(buyMes.items[i]);
            }
            else
            {
                notShowList.Add(buyMes.items[i]);
            }
        }
        FNDebug.Log("购买返回当前商品数量  " + buyMes.items.Count + "  " + ShowList.Count + "   " + notShowList.Count);
    }
    public void GetAttentionIdList(auction.ItemIdList _msg)
    {
        attenList.Clear();
        for (int i = 0; i < _msg.lid.Count; i++)
        {
            attenList.Add(_msg.lid[i]);
        }
    }
    public List<long> GetAttentionList()
    {
        return attenList;
    }

    //   0关注1全部2普通本元3珍贵本元4普通卧龙5珍贵卧龙6其他

    public CSBetterLisHot<AuctionItemInfo> GetGoodslistByLimit(AuctionLimit _limit)
    {
        CSBetterLisHot<AuctionItemInfo> tempResult = new CSBetterLisHot<AuctionItemInfo>();
        if (_limit.firstTabs == 0)//全部
        {
            for (int i = 0; i < notShowList.Count; i++)
            {
                int itemCfgId = notShowList[i].itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(notShowList[i].tujianItem.handBookId) : notShowList[i].item.configId;
                TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(itemCfgId);
                if (IsSearchStrEquals(_limit.searchStr, cfg))
                {
                    tempResult.Add(notShowList[i]);
                }
            }
        }
        else if (_limit.firstTabs == 1)//普通装备
        {
            for (int i = 0; i < notShowList.Count; i++)
            {
                int itemCfgId = notShowList[i].itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(notShowList[i].tujianItem.handBookId) : notShowList[i].item.configId;
                TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(itemCfgId);
                if (cfg != null && cfg.showType == 1)
                {
                    if (IsCareerEquals(_limit.career, cfg) && IsLevelEquals(_limit.level, cfg) && IsSearchStrEquals(_limit.searchStr, cfg))
                    {
                        if (_limit.secondTabs == 0)
                        {
                            tempResult.Add(notShowList[i]);
                        }
                        else
                        {
                            if (cfg.subType == _limit.secondTabs)
                            {
                                tempResult.Add(notShowList[i]);
                            }
                        }
                    }
                }
            }
        }
        else if (_limit.firstTabs == 2)//卧龙装备
        {
            for (int i = 0; i < notShowList.Count; i++)
            {
                int itemCfgId = notShowList[i].itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(notShowList[i].tujianItem.handBookId) : notShowList[i].item.configId;
                TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(itemCfgId);
                if (cfg != null && cfg.showType == 2)
                {
                    if (IsCareerEquals(_limit.career, cfg) && IsLvClassEquals(_limit.lvClass, cfg) && IsSearchStrEquals(_limit.searchStr, cfg))
                    {
                        if (_limit.secondTabs == 0)
                        {
                            tempResult.Add(notShowList[i]);
                        }
                        else
                        {
                            if (cfg.subType == _limit.secondTabs + 100)
                            {
                                tempResult.Add(notShowList[i]);
                            }
                        }
                    }
                }
            }
        }
        else if (_limit.firstTabs == 3)//怀旧装备
        {
            for (int i = 0; i < notShowList.Count; i++)
            {
                int itemCfgId = notShowList[i].itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(notShowList[i].tujianItem.handBookId) : notShowList[i].item.configId;
                TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(itemCfgId);
                if (cfg != null && cfg.showType == 8)
                {
                    if (IsCareerEquals(_limit.career, cfg) && IsLvClassEquals(_limit.lvClass, cfg) && IsSearchStrEquals(_limit.searchStr, cfg))
                    {
                        if (_limit.secondTabs == 0)
                        {
                            tempResult.Add(notShowList[i]);
                        }
                        else
                        {
                            //怀旧装备subtype  120头盔   121戒指   122项链   123手镯
                            if (cfg.subType == _limit.secondTabs + 119)
                            {
                                tempResult.Add(notShowList[i]);
                            }
                        }
                    }
                }
            }
        }
        else if (_limit.firstTabs == 4)//其他
        {
            if (_limit.secondTabs == 0)//全部
            {
                for (int i = 0; i < notShowList.Count; i++)
                {
                    int itemCfgId = notShowList[i].itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(notShowList[i].tujianItem.handBookId) : notShowList[i].item.configId;
                    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(itemCfgId);
                    if (cfg != null && (cfg.showType == 3 || cfg.showType == 4))
                    {
                        if (IsSearchStrEquals(_limit.searchStr, cfg))
                        {
                            tempResult.Add(notShowList[i]);
                        }
                    }
                    if (notShowList[i].itemType == 2)
                    {
                        tempResult.Add(notShowList[i]);
                    }
                }
            }
            else if (_limit.secondTabs == 1)//图鉴卡
            {
                for (int i = 0; i < notShowList.Count; i++)
                {
                    if (notShowList[i].itemType == 2)
                    {
                        tempResult.Add(notShowList[i]);
                    }
                }
            }
        }
        tempResult.Sort((a, b) =>
        {
            if (_limit.priceUp)
            {
                return a.price - b.price;
            }
            else
            {
                return b.price - a.price;
            }
        });
        return tempResult;
    }
    //关注（已经去掉）
    public CSBetterLisHot<AuctionItemInfo> GetAttentionGoodslistByLimit(AuctionLimit _limit)
    {
        FNDebug.Log(_limit.firstTabs + "  *  " + _limit.secondTabs + "  *  " + _limit.priceUp + "  *  " + _limit.career + "  *  " + _limit.lvClass);
        CSBetterLisHot<AuctionItemInfo> result = new CSBetterLisHot<AuctionItemInfo>();

        //if (_limit.firstTabs == 0)
        //{
        //    for (int i = 0; i < ShowList.Count; i++)
        //    {
        //        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //        for (int j = 0; j < attenList.Count; j++)
        //        {
        //            if (ShowList[i].lid == attenList[j])
        //            {
        //                if (IsSearchStrEquals(_limit.searchStr, cfg))
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //            }
        //        }
        //    }
        //}
        //else if (_limit.firstTabs == 1)//全部
        //{
        //    for (int i = 0; i < ShowList.Count; i++)
        //    {
        //        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //        if (IsSearchStrEquals(_limit.searchStr, cfg))
        //        {
        //            result.Add(ShowList[i]);
        //        }
        //    }
        //}
        //else if (_limit.firstTabs == 2)//珍贵本元
        //{
        //    for (int i = 0; i < ShowList.Count; i++)
        //    {
        //        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //        if (cfg != null && cfg.showType == 5)
        //        {
        //            if (IsCareerEquals(_limit.career, cfg) && IsLvClassEquals(_limit.lvClass, cfg) && IsSearchStrEquals(_limit.searchStr, cfg))
        //            {
        //                if (_limit.secondTabs == 0)
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //                else
        //                {
        //                    if (cfg.subType == _limit.secondTabs)
        //                    {
        //                        result.Add(ShowList[i]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //else if (_limit.firstTabs == 3)//珍贵卧龙
        //{
        //    for (int i = 0; i < ShowList.Count; i++)
        //    {
        //        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //        if (cfg != null && cfg.showType == 6)
        //        {
        //            if (IsCareerEquals(_limit.career, cfg) && IsLvClassEquals(_limit.lvClass, cfg) && IsSearchStrEquals(_limit.searchStr, cfg))
        //            {
        //                if (_limit.secondTabs == 0)
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //                else
        //                {
        //                    if (cfg.subType == _limit.secondTabs + 100)
        //                    {
        //                        result.Add(ShowList[i]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //else if (_limit.firstTabs == 4)
        //{
        //    if (_limit.secondTabs == 0)
        //    {
        //        for (int i = 0; i < ShowList.Count; i++)
        //        {
        //            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //            if (cfg != null && (cfg.showType == 3 || cfg.showType == 4))
        //            {
        //                if (IsSearchStrEquals(_limit.searchStr, cfg))
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //            }
        //        }
        //    }
        //    else if (_limit.secondTabs == 1)
        //    {
        //        for (int i = 0; i < ShowList.Count; i++)
        //        {
        //            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);

        //            if (cfg != null && cfg.showType == 3)
        //            {
        //                if (IsSearchStrEquals(_limit.searchStr, cfg))
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //            }
        //        }
        //    }
        //    else if (_limit.secondTabs == 2)
        //    {
        //        for (int i = 0; i < ShowList.Count; i++)
        //        {
        //            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(ShowList[i].item.configId);
        //            if (cfg != null && cfg.showType == 4 && IsCareerEquals(_limit.career, cfg))
        //            {
        //                if (IsSearchStrEquals(_limit.searchStr, cfg))
        //                {
        //                    result.Add(ShowList[i]);
        //                }
        //            }
        //        }
        //    }
        //}

        //result.Sort((a, b) =>
        //{
        //    if (_limit.priceUp)
        //    {
        //        return a.price - b.price;
        //    }
        //    else
        //    {
        //        return b.price - a.price;
        //    }
        //});
        return result;
    }
    bool IsCareerEquals(int _limit, TABLE.ITEM _cfg)
    {
        if (_limit == -1 || _limit == 0) { return true; }
        if (_cfg.career == 0) { return true; }
        if (_cfg.career == _limit) { return true; }
        return false;
    }
    bool IsLvClassEquals(int[] _limit, TABLE.ITEM _cfg)
    {
        if (_limit == null || _limit.Length == 0) { return true; }
        if (_limit.Length == 1)
        {
            if (_cfg.wolongLv > _limit[0]) { return true; }
        }
        if (_limit.Length == 2)
        {
            if (_cfg.wolongLv >= _limit[0] && _cfg.wolongLv <= _limit[1])
            {
                return true;
            }
        }
        return false;
    }
    bool IsLevelEquals(int[] _limit, TABLE.ITEM _cfg)
    {
        if (_limit == null || _limit.Length == 0) { return true; }
        if (_limit.Length == 1)
        {
            if (_cfg.level > _limit[0]) { return true; }
        }
        if (_limit.Length == 2)
        {
            if (_cfg.level >= _limit[0] && _cfg.level <= _limit[1])
            {
                return true;
            }
        }
        return false;
    }
    bool IsSearchStrEquals(string _limit, TABLE.ITEM _cfg)
    {
        if (_limit == string.Empty) { return true; }
        if (_cfg.name.Contains(_limit)) { return true; }
        return false;
    }

    #endregion


    public override void Dispose()
    {
        pool.RecycleAll();
    }

    public bool AuctionSellRedpoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_Auction))
        {
            return false;
        }
        if (sellMes == null) { return false; }
        for (int i = 0; i < sellMes.items.Count; i++)
        {
            if (sellMes.items[i].showTime == 0)
            {
                if (259200000 < (CSServerTime.Instance.TotalMillisecond - sellMes.items[i].addTime))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool HasCanSellItems()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_Auction))
        {
            return false;
        }
        var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
        while (iter.MoveNext())
        {
            TABLE.ITEM _cfg = ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId);
            if (_cfg.saleType != 3 && iter.Current.Value.bind == 0)
            {
                if (CSBagInfo.Instance.IsWoLongEquip(_cfg))
                {
                    if (_cfg.quality < 5 && !CSBagInfo.Instance.IsCareerSexEqual(_cfg))
                    {
                        return true;
                    }
                }
                else if (CSBagInfo.Instance.IsNormalEquip(_cfg))
                {
                    if (!CSBagInfo.Instance.IsCareerSexEqual(_cfg))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
public class AuctionLimit
{
    public int firstTabs = -1;
    public int secondTabs = -1;
    public bool priceUp = true;
    public int career = -1;
    public int[] lvClass = null;
    public int[] level = null;
    public string searchStr = "";
    public AuctionLimit()
    {

    }
    public void Uninit()
    {
        firstTabs = -1;
        secondTabs = -1;
        priceUp = true;
        career = -1;
        lvClass = null;
        level = null;
        searchStr = "";
    }
}

public class AuctionItemData : IDispose
{
    /// <summary>
    /// 1.背包   2.幻灵卡  3.怀旧装备
    /// </summary>
    public int type;
    public bag.BagItemInfo bagInfo;
    public long tujianId = 0;
    public int tujianCfgId = 0;
    public int handbookId = 0;
    public AuctionItemData()
    {

    }
    public AuctionItemData(int _type, bag.BagItemInfo _baginfo = null, int _tujianId = 0)
    {
        type = _type;
        bagInfo = _baginfo;
        tujianId = _tujianId;
    }
    public void Dispose()
    {

    }
}
