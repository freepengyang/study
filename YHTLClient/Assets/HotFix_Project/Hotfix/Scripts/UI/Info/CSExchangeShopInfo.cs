using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSExchangeShopInfo : CSInfo<CSExchangeShopInfo>
{
    /// <summary> 日常兑换券(周一到周六对应的券) </summary>
    public Dictionary<int, TABLE.ITEM> dailyTicketList;

    /// <summary> 开服兑换券 </summary>
    public Dictionary<int, TABLE.ITEM> serverOpenTicketList;


    /// <summary> 万能券::注1：策划表示万能券换其他券的比例定死为1比1。 </summary>
    public TABLE.ITEM universalTicket;

    /// <summary> 如当前还在开服前几日，则为类型1的所有数据，key为开服第几天。如已进入日常时间，则为类型2的所有数据，key为周几 </summary>
    Dictionary<int, ILBetterList<CSExchangeItemData>> allDatasDic;

    /// <summary> 购买次数。key为配表id </summary>
    Dictionary<int, int> buyTimesDic;

    PoolHandleManager mPoolHandle = new PoolHandleManager();

    /// <summary> 开服兑换最大天数(读杂项表)，当前开服天数超过此值则开始显示日常兑换 </summary>
    public int serverOpenExchangeDays;

    /// <summary> 当前显示的数据类型。对应BiQiShop表的主类型，1为开服类，2为日常类 </summary>
    int curShowDataType;
    /// <summary> 当前显示的数据类型。对应BiQiShop表的主类型，1为开服类，2为日常类 </summary>
    public int CurShowDataType { get { return curShowDataType; } }


    string serverOpenDayStr;
    string[] dayOfWeekStr;


    public long serverOpenDay;
    public int dayOfWeek;


    public bool exchangeShopOpend;
    public bool lastRedPointCheck;


    public override void Dispose()
    {
        mClientEvent.RemoveEvent(CEvent.ResDayPassedMessage, DayPassedEvent);
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        dailyTicketList?.Clear();
        dailyTicketList = null;
        allDatasDic?.Clear();
        allDatasDic = null;
        buyTimesDic?.Clear();
        buyTimesDic = null;
        universalTicket = null;
    }



    public void Init()
    {
        string daysStr = SundryTableManager.Instance.GetSundryEffect(737);
        int.TryParse(daysStr, out serverOpenExchangeDays);

        serverOpenDay = CSMainPlayerInfo.Instance.ServerOpenDay;
        dayOfWeek = (int)CSServerTime.Now.DayOfWeek;

        InitTicketConfig();

        mClientEvent.AddEvent(CEvent.ResDayPassedMessage, DayPassedEvent);

        InitAllData();
        
        Net.CSDuiHuanShopInfoMessage();
    }


    /// <summary>
    /// 初始化兑换券配置信息
    /// </summary>
    void InitTicketConfig()
    {
        string universalIdStr = SundryTableManager.Instance.GetSundryEffect(735);
        var item = GetItemCfgByIdStr(universalIdStr);
        if (item != null)
            universalTicket = item;

        if (dailyTicketList == null) dailyTicketList = new Dictionary<int, TABLE.ITEM>();
        else dailyTicketList.Clear();
        string dailyIdStr = SundryTableManager.Instance.GetSundryEffect(734);
        var idArray = dailyIdStr.Split('#');
        if (idArray.Length >= 6)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                item = GetItemCfgByIdStr(idArray[i]);
                if (item == null) continue;
                dailyTicketList[i + 1] = item;
            }
        }


        if (serverOpenTicketList == null) serverOpenTicketList = new Dictionary<int, TABLE.ITEM>();
        else serverOpenTicketList.Clear();

        dailyIdStr = SundryTableManager.Instance.GetSundryEffect(1002);
        idArray = dailyIdStr.Split('#');
        if (idArray.Length >= 6)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                item = GetItemCfgByIdStr(idArray[i]);
                if (item == null) continue;
                serverOpenTicketList[i + 1] = item;
            }
        }
    }



    void InitAllData()
    {
        serverOpenDay = CSMainPlayerInfo.Instance.ServerOpenDay;
        dayOfWeek = (int)CSServerTime.Now.DayOfWeek;

        int oldType = curShowDataType;
        bool isServerOpenLoop = serverOpenDay <= serverOpenExchangeDays;
        int newType = isServerOpenLoop ? 1 : 2;
        if (oldType == newType) return;//最后一次保存的数据是否需要变化


        if (dailyTicketList == null || universalTicket == null) return;//需要先初始化兑换券数据


        var arr = BiQiShopTableManager.Instance.array.gItem.handles;
        if (arr == null) return;

        if (allDatasDic == null) allDatasDic = new Dictionary<int, ILBetterList<CSExchangeItemData>>(8);
        else
        {
            mPoolHandle.RecycleAll();
            for (var it = allDatasDic.GetEnumerator(); it.MoveNext();)
            {
                var list = it.Current.Value;
                list?.Clear();
            }
        }

        for(int i = 0,max = arr.Length; i < max;++i)
        {
            TABLE.BIQISHOP cfg = arr[i].Value as TABLE.BIQISHOP;
            if (cfg == null || cfg.type != newType) continue;
            CSExchangeItemData data = mPoolHandle.GetCustomClass<CSExchangeItemData>();
            data.Init(cfg);
            //兑换券
            if (cfg.type == 1)
            {
                if (serverOpenTicketList.ContainsKey(cfg.subType))
                    data.SetTicket(serverOpenTicketList[cfg.subType]);
            }
            else if (cfg.type == 2)
            {
                if (dailyTicketList.ContainsKey(cfg.subType))
                    data.SetTicket(dailyTicketList[cfg.subType]);
            }
            ILBetterList<CSExchangeItemData> list = null;
            if (!allDatasDic.TryGetValue(cfg.subType, out list))
            {
                list = new ILBetterList<CSExchangeItemData>(16);
                allDatasDic.Add(cfg.subType, list);
            }
            list.Add(data);
        }

        for (var it = allDatasDic.GetEnumerator(); it.MoveNext();)
        {
            var list = it.Current.Value;
            if (list == null || list.Count < 1) continue;
            list.Sort((a, b) =>
            {
                int orderA = a.config != null && a.config.order > 0 ? a.config.order : a.Id;
                int orderB = b.config != null && b.config.order > 0 ? b.config.order : b.Id;
                return orderA - orderB;
            });
        }

        curShowDataType = newType;


        mClientEvent.SendEvent(CEvent.ExchangeShopDataChange);
    }



    TABLE.ITEM GetItemCfgByIdStr(string idStr)
    {
        int id = 0;
        TABLE.ITEM item = null;
        if (int.TryParse(idStr, out id))
        {
            ItemTableManager.Instance.TryGetValue(id, out item);
        }

        return item;
    }


    public void SC_AllInfo(shop.ResDuiHuanShopInfo msg)
    {
        if (buyTimesDic == null) buyTimesDic = new Dictionary<int, int>();
        else buyTimesDic.Clear();

        var info = msg.duiHuanShopInfo;

        for (int i = 0; i < info.Count; i++)
        {
            if (!buyTimesDic.ContainsKey(info[i].id))
                buyTimesDic.Add(info[i].id, info[i].dailyDuiHuanNum);
            else
                buyTimesDic[info[i].id] = info[i].dailyDuiHuanNum;
        }

        mClientEvent.SendEvent(CEvent.ExchangeShopRefresh);
    }


    public void SC_SingleInfo(shop.DuiHuanShopInfo msg)
    {
        if (buyTimesDic == null) buyTimesDic = new Dictionary<int, int>();
        buyTimesDic[msg.id] = msg.dailyDuiHuanNum;

        mClientEvent.SendEvent(CEvent.ExchangeShopSingleChange);
    }


    public ILBetterList<CSExchangeItemData> GetOneDayDatasList(int subType)
    {
        if (allDatasDic == null) return null;
        ILBetterList<CSExchangeItemData> list = null;
        allDatasDic.TryGetValue(subType, out list);
        return list;
    }


    /// <summary>
    /// 查询某个商品购买次数
    /// </summary>
    /// <param name="cfgId"></param>
    /// <returns></returns>
    public int GetBuyTimes(int cfgId)
    {
        int buyTimes = 0;
        if (buyTimesDic.Count < 1 || !buyTimesDic.ContainsKey(cfgId)) return buyTimes;
        buyTimes = buyTimesDic[cfgId];

        return buyTimes;
    }


    public string GetServerOpenDayStr()
    {
        if (string.IsNullOrEmpty(serverOpenDayStr))
        {
            serverOpenDayStr = ClientTipsTableManager.Instance.GetClientTipsContext(1789);
        }

        return serverOpenDayStr;
    }


    public string GetDayOfWeekStr(int day)
    {
        if (dayOfWeekStr == null || dayOfWeekStr.Length < 1)
        {
            string str = SundryTableManager.Instance.GetSundryEffect(574);//0下标为周日
            dayOfWeekStr = str.Split('#');
        }

        int _day = day == 7 ? 0 : day;
        if (dayOfWeekStr.Length > _day) return dayOfWeekStr[_day];

        return "";
    }



    void DayPassedEvent(uint id, object data)
    {
        serverOpenDay = CSMainPlayerInfo.Instance.ServerOpenDay;
        dayOfWeek = (int)CSServerTime.Now.DayOfWeek;
        InitAllData();
    }


    public void OpenExchangeShop()
    {
        //if (!exchangeShopOpend/* && !lastRedPointCheck*/)
        //{
        //    exchangeShopOpend = true;
        //    mClientEvent.SendEvent(CEvent.ExchangeShopRedPointCheck);
        //}
        if (!lastRedPointCheck) return;
        exchangeShopOpend = true;
        mClientEvent.SendEvent(CEvent.ExchangeShopRedPointCheck);
    }


    public bool CheckRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_BiQiShop))
        {
            return false;
        }
        if (exchangeShopOpend && lastRedPointCheck)
        {
            return false;
        }

        if (allDatasDic == null || allDatasDic.Count < 1)
        {
            return false;
        }

        int index = 0;
        if (curShowDataType == 1)
        {
            index = (int)CSMainPlayerInfo.Instance.ServerOpenDay;
        }
        else
        {
            index = (int)CSServerTime.Now.DayOfWeek;
        }

        if (index != 0 && allDatasDic.ContainsKey(index))
        {
            var list = allDatasDic[index];
            for (int i = 0; i < list.Count; i++)
            {
                var data = list[i];
                if (data == null || data.config == null) continue;
                TABLE.BIQISHOP cfg = data.config;
                TABLE.ITEM ticket = data.ticket;
                bool moneyEnough = ((int)MoneyType.yuanbao).GetItemCount() >= cfg.value;
                bool ticketEnough = ticket.id.GetItemCount() >= cfg.value2;
                if (moneyEnough && ticketEnough)
                {
                    lastRedPointCheck = true;
                    return true;
                }
            }
        }
        else
        {
            //周日情况判断所有
            for (var it = allDatasDic.GetEnumerator(); it.MoveNext();)
            {
                var list = it.Current.Value;
                if (list == null) continue;
                for (int i = 0; i < list.Count; i++)
                {
                    var data = list[i];
                    if (data == null || data.config == null) continue;
                    TABLE.BIQISHOP cfg = data.config;
                    TABLE.ITEM ticket = data.ticket;
                    bool moneyEnough = ((int)MoneyType.yuanbao).GetItemCount() >= cfg.value;
                    bool ticketEnough = ticket.id.GetItemCount() >= cfg.value2;
                    if (moneyEnough && ticketEnough)
                    {
                        lastRedPointCheck = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

}


public class CSExchangeItemData : IDispose
{
    int _id;
    public int Id { get { return _id; } }

    public TABLE.BIQISHOP config;

    Dictionary<int, int> rewardDic;

    public Dictionary<int, int> RewardDic { get { return rewardDic; } }


    public TABLE.ITEM ticket;


    public void Dispose()
    {
        config = null;
        rewardDic?.Clear();
        rewardDic = null;
        ticket = null;
    }

    public void Init(TABLE.BIQISHOP cfg)
    {
        if (cfg == null) return;
        if (_id == cfg.id) return;

        config = cfg;
        _id = cfg.id;

        if (rewardDic == null) rewardDic = new Dictionary<int, int>();
        else rewardDic.Clear();

        BoxTableManager.Instance.GetBoxAwardById(config.boxId, rewardDic);
    }


    public void SetTicket(TABLE.ITEM _ticket)
    {
        if (_ticket != null) ticket = _ticket;
    }

}

