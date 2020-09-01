using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSShopInfo : CSInfo<CSShopInfo>
{
    private Dictionary<int, int> buyTimesDic = new Dictionary<int, int>();


    Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>> allShopInfo; //普通商城信息
    public Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>> AllShopInfo { get { return allShopInfo; } }
    FastArrayElementFromPool<CSShopCommodityData> notReacheLvCache;

    private Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>> rechargeShopInfo;//充值商城信息

    
    ILBetterList<int> moneyList = new ILBetterList<int>(); //限购moeny列表
    
    public Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>> RechargeShopInfo
    {
        get => rechargeShopInfo;
        set => rechargeShopInfo = value;
    }

    //FastArrayElementFromPool<CSShopCommodityData> notReacheLvCacheByRecharge;
    
    PoolHandleManager mPoolHandle = new PoolHandleManager();

    private int _dailyRmb = 0;

    public int dailyRmb
    {
        get => _dailyRmb;
        set
        {
            _dailyRmb = value; 
            HotManager.Instance.EventHandler.SendEvent(CEvent.DailyRmbChange);
        }
        
    }

    
    /// <summary>
    /// 充值商城价格列表
    /// </summary>
    /// <returns></returns>
    public int GetShopMoney(int index)
    {
        if (moneyList.Count == 0)
        {
            var info = SundryTableManager.Instance.GetSundryEffect(736).Split('#');
            for (int i = 0; i < info.Length; i++)
            {
                int id;
                int.TryParse(info[i],out id);
                moneyList.Add(id);
            }
        }
        
        
        if (moneyList.Count>=index)
        {
            return moneyList[index];
        }
        else
        {
            return moneyList[0];
        }
    }


    //private ILBetterList<CSShopCommodityData> mDatas = new ILBetterList<CSShopCommodityData>();
    /// <summary>
    /// 返回商城中物品id相同的列表
    /// </summary>
    public CSShopCommodityData GetShopData(int id)
    {
        //mDatas.Clear();

        int index = 0;
        
        
        
        for (var iter =  RechargeShopInfo.GetEnumerator();iter.MoveNext();)
        {
            if (dailyRmb < GetShopMoney(index))
            {
                break;
            }
            
            var v = iter.Current.Value;
            for (int i = 0; i < v.Count; i++)
            {
                int BuyTimes = v[i].BuyTimes;
                int BuyTimesLimit = v[i].buyTimesLimit;
                bool isbuy = (BuyTimesLimit > 0 && BuyTimes < BuyTimesLimit) || BuyTimesLimit == 0;
                if (v[i].itemCfg.id == id && isbuy)
                {
                    return v[i];
                }
            }

            index++;
        }
        
        return null;
    }


    public override void Dispose()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_LevelChange, OnPlayerLevelChange);
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;

        buyTimesDic?.Clear();
        buyTimesDic = null;

        allShopInfo?.Clear();
        allShopInfo = null;

        notReacheLvCache?.Clear();
        notReacheLvCache = null;
    }

    public CSShopInfo()
    {
        Init();

        if (notReacheLvCache != null && notReacheLvCache.Count > 0)
            HotManager.Instance.EventHandler.AddEvent(CEvent.MainPlayer_LevelChange, OnPlayerLevelChange);
    }

    void Init()
    {
        if (allShopInfo == null) allShopInfo = new Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>>();
        else
        {
            for (var it = allShopInfo.GetEnumerator(); it.MoveNext();)
            {
                FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
                if(list != null) list.Clear();
            }
        }

        if (RechargeShopInfo == null) RechargeShopInfo = new Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>>();
        else
        {
            for (var it = RechargeShopInfo.GetEnumerator(); it.MoveNext();)
            {
                FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
                if(list != null) list.Clear();
            }
        }
        
        var arr = ShopTableManager.Instance.array.gItem.handles;
        if (arr == null) return;
        // var dic = ShopTableManager.Instance.Dic.array.Dic;
        // if (dic == null) return;

        int roleLv = CSMainPlayerInfo.Instance.Level;

        CSShopCommodityData data = null;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.SHOP cfg = arr[k].Value as TABLE.SHOP;
            if (cfg == null) continue;
            if (cfg.npcId != 4 && cfg.npcId != 1)
                continue;
            
            if (cfg.showLevel != 0 && cfg.showLevel > roleLv)//未满足等级的加入临时数组
            {
                if (notReacheLvCache == null) notReacheLvCache = mPoolHandle.CreateGeneratePool<CSShopCommodityData>();
                data = notReacheLvCache.Append();
                if (data != null)
                {
                    data.Init(cfg);
                }
                continue;
            }
            
            if (cfg.npcId == 1)
            {
                
                if (cfg.maxLevel != 0 && cfg.maxLevel < roleLv) continue;

                int subType = cfg.subType;

                if (cfg.subType < 1 || cfg.subType > 4) continue;

                FastArrayElementFromPool<CSShopCommodityData> list = null;
                if (!allShopInfo.ContainsKey(subType))
                {
                    list = mPoolHandle.CreateGeneratePool<CSShopCommodityData>();
                    allShopInfo.Add(subType, list);
                }
                else list = allShopInfo[subType];

                data = list.Append();
                if (data != null)
                {
                    data.Init(cfg);
                }
            }
            
            //判断如果是充值商城数据,直接加入
            if (cfg.npcId == 4)
            {
                
                if (cfg.maxLevel != 0 && cfg.maxLevel < roleLv) continue;

                int subType = cfg.subType;

                if (cfg.subType < 1 || cfg.subType > 7) continue;

                FastArrayElementFromPool<CSShopCommodityData> list = null;
                if (!RechargeShopInfo.ContainsKey(subType))
                {
                    list = mPoolHandle.CreateGeneratePool<CSShopCommodityData>();
                    RechargeShopInfo.Add(subType, list);
                }
                else list = RechargeShopInfo[subType];

                data = list.Append();
                if (data != null)
                {
                    data.Init(cfg);
                }
            }

        }

        for (var it = allShopInfo.GetEnumerator(); it.MoveNext();)
        {
            FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
            if (list == null || list.Count < 1)
            {
                continue;
            }
            SortShopList(list);
        }

        //排序充值商城数据
        for (var it = RechargeShopInfo.GetEnumerator(); it.MoveNext();)
        {
            FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
            if (list == null || list.Count < 1)
            {
                continue;
            }
            SortShopList(list);
        }
        
    }


    void SortShopList(FastArrayElementFromPool<CSShopCommodityData> list)
    {
        if (list == null || list.Count < 2) return;
        list.Sort((a, b) =>
        {
            if (a.config == null || b.config == null) return -1;
            string orderA = a.config.order;
            string orderB = b.config.order;
            if (!string.IsNullOrEmpty(orderA) && !string.IsNullOrEmpty(orderB))
            {
                return int.Parse(orderB) - int.Parse(orderA);
            }
            else if (string.IsNullOrEmpty(orderA) && string.IsNullOrEmpty(orderB))
            {
                return a.config.id - b.config.id;  //id按从小到大排序
            }
            else
            {
                return !string.IsNullOrEmpty(orderA) ? -1 : 1;
            }
        });
    }


    void OnPlayerLevelChange(uint id, object data)
    {
        if (notReacheLvCache == null || notReacheLvCache.Count < 1) return;
        if (allShopInfo == null) return;

        int lv = CSMainPlayerInfo.Instance.Level;
        bool hasChange = false;
        
        
        for (int i = 0; i < notReacheLvCache.Count; i++)
        {
            var item = notReacheLvCache[i];
            if (item.config != null && item.config.showLevel <= lv)
            {
                int subtype = item.config.subType;

                Dictionary<int, FastArrayElementFromPool<CSShopCommodityData>> tempinfo;
                
                if (item.npcId == 1)
                {
                    tempinfo = allShopInfo;
                }
                else
                {
                    tempinfo = rechargeShopInfo;
                }

                if (!tempinfo.ContainsKey(subtype) || tempinfo[subtype] == null) continue;
                var itemNew = tempinfo[subtype].Append();
                if (itemNew != null)
                {
                    itemNew.Init(item.config);
                }
                hasChange = true;
                notReacheLvCache.RemoveAt(i);
                i--;
            }
        }

        if (!hasChange) return;

        for (var it = allShopInfo.GetEnumerator(); it.MoveNext();)
        {
            FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
            if (list == null || list.Count < 1)
            {
                continue;
            }
            SortShopList(list);
        }
        
        for (var it = rechargeShopInfo.GetEnumerator(); it.MoveNext();)
        {
            FastArrayElementFromPool<CSShopCommodityData> list = it.Current.Value;
            if (list == null || list.Count < 1)
            {
                continue;
            }
            SortShopList(list);
        }
        
    }

    

    /// <summary>
    /// 登录和隔天刷新都是走该方法。重置次数时后端发的数据为空
    /// </summary>
    /// <param name="msg"></param>
    public void GetShopBuyInfo(shop.ShopBuyInfoResponse msg)
    {
        if (buyTimesDic == null) buyTimesDic = new Dictionary<int, int>();
        else buyTimesDic.Clear();
        for (int i = 0; i < msg.shopBuyInfos.Count; i++)
        {
            var temp = msg.shopBuyInfos[i].shopItemBuyInfos;
            for (int j = 0; j < temp.Count; j++)
            {
                if (!buyTimesDic.ContainsKey(temp[j].shopId))
                    buyTimesDic.Add(temp[j].shopId, temp[j].buyTimes);
                else
                    buyTimesDic[temp[j].shopId] = temp[j].buyTimes;
            }
        }

        mClientEvent.SendEvent(CEvent.ShopInfoChange);
    }


    /// <summary>
    /// 查询单个商品信息的购买次数
    /// </summary>
    /// <param name="shopId"></param>
    /// <returns></returns>
    public int GetBuyTimes(int shopId)
    {
        int buyTimes = 0;
        if (buyTimesDic.Count < 1 || !buyTimesDic.ContainsKey(shopId)) return buyTimes;
        buyTimes = buyTimesDic[shopId];

        return buyTimes;
    }

    

    /// <summary>
    /// 某商品购买次数增加(购买商品后消息回调)
    /// </summary>
    /// <returns></returns>
    public void AddBuyTimes(int shopId, int addTimes)
    {
        if (!buyTimesDic.ContainsKey(shopId))
        {
            buyTimesDic.Add(shopId, 0);
        }
        buyTimesDic[shopId] += addTimes;
        EventData data = CSEventObjectManager.Instance.SetValue(shopId, buyTimesDic[shopId]);
        mClientEvent.SendEvent(CEvent.ShopBuyTimesChange, data);
        CSEventObjectManager.Instance.Recycle(data);
    }

    private int UnlockNum;

    public bool RechargeRed = true;
    
    public bool RechargeShopRed()
    {
        
        int num = 0;
        GetShopMoney(0);
        for (int i = 0; i < moneyList.Count; i++)
        {
            if (dailyRmb > moneyList[i])
                num++;
            else
                break;
        }

        if (UnlockNum != num)
        {
            UnlockNum = num;
            RechargeRed =  true;
        }
        
        return RechargeRed;
    }


}


public class CSShopCommodityData : IDispose
{
    public CSShopCommodityData()
    {
        
    }
    
    public TABLE.SHOP config;

    public TABLE.ITEM itemCfg;

    public int buyTimesLimit;
    public int BuyTimes
    {
        get
        {
            if (config == null || buyTimesLimit == 0) return 0;
            return CSShopInfo.Instance.GetBuyTimes(config.id);
        }
    }


    public int npcId = 1;  //数据类型 1 代表普通商城 4 代表充值商城
    

    public void Dispose()
    {
        config = null;
        itemCfg = null;
    }

    public void Init(TABLE.SHOP _shop)
    {
        if (_shop == null) return;
        int itemId = _shop.itemId2 == 0 ? _shop.itemId : _shop.itemId2;
        TABLE.ITEM item = ItemTableManager.Instance.GetItemCfg(itemId);
        if (item == null)
        {
            FNDebug.LogError($"Shop表格错误:Item表中无id为{itemId}的物品。ShopID{_shop.id}");
            return;
        }
        
        config = _shop;
        itemCfg = item;
        npcId = config.npcId;
        if (!string.IsNullOrEmpty(config.frequency))
        {
            int.TryParse(config.frequency, out buyTimesLimit);
        }

    }

    
}