using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;


/// <summary>
/// 目前所有商品信息均由后端主动推送，包括杂项表中配置的上架id，以及根据开服时间是否上架或下架，后端都处理好了。
/// 上线和隔天时后端会推送
/// </summary>
public class CSGiftBagInfo : CSInfo<CSGiftBagInfo>
{
    
    readonly int[] moneyIds = { 0, 3 };//GiftBag表中的货币类型，1为元宝

    PoolHandleManager mPoolHandle = new PoolHandleManager();

    CSBetterLisHot<GiftBagData> allDataList;


    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        allDataList?.Clear();
        allDataList = null;

    }


    public void SC_AllInfoMessage(dailypurchase.DailyPurchaseInfoResponse msg)
    {
        if (allDataList == null) allDataList = new CSBetterLisHot<GiftBagData>();
        else
        {
            mPoolHandle.RecycleAll();
            allDataList.Clear();
        }
        for (int i = 0; i < msg.giftBuyInfos.Count; i++)
        {
            GiftBagData data = mPoolHandle.GetCustomClass<GiftBagData>();
            GIFTBAG cfg;
            
            if (GiftBagTableManager.Instance.TryGetValue(msg.giftBuyInfos[i].giftId, out cfg))
            {
                data.Init(cfg, msg.giftBuyInfos[i].buyTimes);
                allDataList.Add(data);
            }
            else mPoolHandle.Recycle(data);
        }

        mClientEvent.SendEvent(CEvent.GiftBagAllChange);
    }

    public void SC_BuyInfoResponse(dailypurchase.DailyPurchaseBuyResponse msg)
    {
        if (allDataList == null) allDataList = new CSBetterLisHot<GiftBagData>();
        GiftBagData data = allDataList.FirstOrNull(x => { return x.id == msg.giftBuyInfo.giftId; });
        if (data != null)
        {
            data.SetBuyTimes(msg.giftBuyInfo.buyTimes);
        }
        else
        {
            data = mPoolHandle.GetCustomClass<GiftBagData>();
            GIFTBAG cfg;

            if (GiftBagTableManager.Instance.TryGetValue(msg.giftBuyInfo.giftId, out cfg))
            {
                data.Init(cfg, msg.giftBuyInfo.buyTimes);
                allDataList.Add(data);
            }
            else
            {
                mPoolHandle.Recycle(data);
                data = null;
            }
        }
        if (data != null) mClientEvent.SendEvent(CEvent.GiftBagAllChange, data);
    }


    public CSBetterLisHot<GiftBagData> GetDatas()
    {
        if (allDataList == null) return null;
        return allDataList;
    }

    

    public int GetMoneyId(int _type)
    {
        if (_type < 0 || _type >= moneyIds.Length) return 0;
        return moneyIds[_type];
    }

}



public class GiftBagData : IDispose
{
    int _id;
    public int id { get { return _id; } }

    public GIFTBAG config;

    int _buyTimes;
    public int buyTimes { get { return _buyTimes; } }

    public void Dispose()
    {
        
    }

    public void Init(GIFTBAG cfg, int times)
    {
        if (cfg == null) return;
        config = cfg;
        _id = config.id;
        SetBuyTimes(times);
    }


    public void SetBuyTimes(int times)
    {
        _buyTimes = times;
    }
}
