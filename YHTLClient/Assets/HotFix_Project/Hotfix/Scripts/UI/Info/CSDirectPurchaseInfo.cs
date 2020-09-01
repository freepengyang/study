using System;
using System.Collections.Generic;
using dailypurchase;
using Google.Protobuf.Collections;
using TABLE;

/// <summary>
/// 直购礼包数据类
/// </summary>
public class CSDirectPurchaseInfo : CSInfo<CSDirectPurchaseInfo>
{
    public CSDirectPurchaseInfo()
    {
        Init();
    }

    public override void Dispose()
    {

    }

    void Init()
    {
        var arr = GiftBagTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var value = arr[i].Value as GIFTBAG;
            if (value.type == 3)
            {
                DirectPurchaseData directPurchaseData = new DirectPurchaseData();
                directPurchaseData.Giftbag = value;
                allDirectPurchaseDatas.Add(value.id, directPurchaseData);
                // listDirectPurchaseDatas.Add(directPurchaseData);
            }
        }

        var arrRewards = RechargeRewardTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arrRewards.Length; i < max; ++i)
        {
            var value = arrRewards[i].Value as RECHARGEREWARD;
            if (value.type == 2)
            {
                DirectPurchaseRewardsData drectPurchaseRewardsDatas = new DirectPurchaseRewardsData();
                drectPurchaseRewardsDatas.Rechargereward = value;
                listDirectPurchaseRewardsDatas.Add(drectPurchaseRewardsDatas);
            }
        }
    }
    
    /// <summary>
    /// 配置里所有的直购礼包信息
    /// </summary>
    private Dictionary<int, DirectPurchaseData> allDirectPurchaseDatas = new Dictionary<int, DirectPurchaseData>();

    /// <summary>
    /// 直购礼包数据
    /// </summary>
    private ILBetterList<DirectPurchaseData> listDirectPurchaseDatas = new ILBetterList<DirectPurchaseData>();

    public ILBetterList<DirectPurchaseData> ListDirectPurchaseDatas => listDirectPurchaseDatas;

    /// <summary>
    /// 直购回馈奖励信息列表
    /// </summary>
    private ILBetterList<DirectPurchaseRewardsData> listDirectPurchaseRewardsDatas =
        new ILBetterList<DirectPurchaseRewardsData>();

    public ILBetterList<DirectPurchaseRewardsData> ListDirectPurchaseRewardsDatas => listDirectPurchaseRewardsDatas;

    /// <summary>
    /// 累计充值人民币
    /// </summary>
    private int money = 0;

    public int Money => money;

    /// <summary>
    /// 已购礼包Id
    /// </summary>
    private RepeatedField<int> buyIds = new RepeatedField<int>();

    public RepeatedField<int> BuyIds => buyIds;


    /// <summary>
    /// 充值人民币order
    /// </summary>
    private ZhiGouOrder zhiGouOrder = new ZhiGouOrder();

    public ZhiGouOrder ZhiGouOrder => zhiGouOrder;


    /// <summary>
    /// 是否存在指定id还未买完礼包
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsHasSpecifyGiftBag(int id)
    {
        for (int i = 0, max = listDirectPurchaseDatas.Count; i < max; i++)
        {
            DirectPurchaseData directPurchaseData = listDirectPurchaseDatas[i];
            if (directPurchaseData.Giftbag.id == id && directPurchaseData.SoldOut == 0)
                return true;
        }
        return false;
    }


    /// <summary>
    /// 是否有用元宝购买的礼包元宝足够
    /// </summary>
    /// <returns></returns>
    public bool IsHasEnoughYuanBao()
    {
        for (int i = 0; i < listDirectPurchaseDatas.Count; i++)
        {
            DirectPurchaseData directPurchaseData = ListDirectPurchaseDatas[i];
            if (directPurchaseData.Giftbag.gainType == 1 && CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao) >=
                directPurchaseData.Giftbag.para && directPurchaseData.SoldOut == 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否有可领取回馈礼包
    /// </summary>
    /// <returns></returns>
    public bool IsHasEnableReceive()
    {
        for (int i = 0; i < listDirectPurchaseRewardsDatas.Count; i++)
        {
            DirectPurchaseRewardsData directPurchaseRewardsData = listDirectPurchaseRewardsDatas[i];
            if (directPurchaseRewardsData.ReceiveState == 0)
                return true;
        }

        return false;
    }

    #region 网络响应

    /// <summary>
    /// 处理直购礼包消息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGiftData(DailyPurchaseInfoResponse msg)
    {
        if (msg == null) return;
        listDirectPurchaseDatas.Clear();
        for (int i = 0; i < msg.giftBuyInfos.Count; i++)
        {
            GiftBuyInfo giftBuyInfo = msg.giftBuyInfos[i];
            if (allDirectPurchaseDatas.TryGetValue(giftBuyInfo.giftId, out DirectPurchaseData directPurchaseData))
            {
                if (!listDirectPurchaseDatas.Contains(directPurchaseData))
                {
                    directPurchaseData.BuyTimes = giftBuyInfo.buyTimes;
                    listDirectPurchaseDatas.Add(directPurchaseData);
                }
            }
        }

        listDirectPurchaseDatas.Sort(SortDirectPurchaseDatas);
    }

    int SortDirectPurchaseDatas(DirectPurchaseData a, DirectPurchaseData b)
    {
        if (a.SoldOut != b.SoldOut)
            return a.SoldOut.CompareTo(b.SoldOut);
        else
            return a.Giftbag.id.CompareTo(b.Giftbag.id);
    }

    /// <summary>
    /// 处理元宝购买响应
    /// </summary>
    public void HandleDailyPurchaseBuy(DailyPurchaseBuyResponse msg)
    {
        if (msg == null) return;
        for (int i = 0; i < listDirectPurchaseDatas.Count; i++)
        {
            DirectPurchaseData directPurchaseData = listDirectPurchaseDatas[i];
            if (directPurchaseData.Giftbag.id == msg.giftBuyInfo.giftId)
            {
                directPurchaseData.BuyTimes = msg.giftBuyInfo.buyTimes;
                break;
            }
        }
        listDirectPurchaseDatas.Sort(SortDirectPurchaseDatas);
    }

    /// <summary>
    /// 处理直购领取响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleZhiGouInfo(ZhiGouInfo msg)
    {
        if (msg == null) return;
        money = msg.money;
        buyIds = msg.buyIds;
        for (int i = 0; i < listDirectPurchaseRewardsDatas.Count; i++)
        {
            DirectPurchaseRewardsData directPurchaseRewardsData = listDirectPurchaseRewardsDatas[i];
            bool isreceived = false;
            for (int j = 0; j < buyIds.Count; j++)
            {
                int receiveId = buyIds[j];
                if (directPurchaseRewardsData.Rechargereward.id == receiveId)
                {
                    directPurchaseRewardsData.ReceiveState = 2;
                    isreceived = true;    
                }
            }

            if (!isreceived)
                directPurchaseRewardsData.ReceiveState = money >= directPurchaseRewardsData.Rechargereward.need ? 0 : 1;
        }

        listDirectPurchaseRewardsDatas.Sort(SortDirectPurchaseRewardsDatas);
    }

    int SortDirectPurchaseRewardsDatas(DirectPurchaseRewardsData a, DirectPurchaseRewardsData b)
    {
        if (a.ReceiveState != b.ReceiveState)
            return a.ReceiveState.CompareTo(b.ReceiveState);
        else
            return a.Rechargereward.id.CompareTo(b.Rechargereward.id);
    }

    /// <summary>
    /// 处理直购人民币充值订单响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleZhiGouOrder(ZhiGouOrder msg)
    {
        if (msg == null) return;
        zhiGouOrder = msg;
        //接SDK(充值不走这里了)
    }

    /// <summary>
    /// giftbag表活动信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGiftBagInfo(dailypurchase.GiftBagList msg)
    {
    }

    /// <summary>
    /// giftbag表活动开启
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGiftBagOpen(dailypurchase.GiftBagList msg)
    {
        for (int i = 0; i < msg.giftIds.Count; i++)
        {
            int giftId = msg.giftIds[i];
            if (allDirectPurchaseDatas.TryGetValue(giftId, out DirectPurchaseData item))
            {
                if (!listDirectPurchaseDatas.Contains(item))
                    listDirectPurchaseDatas.Add(item);
            }
        }

        listDirectPurchaseDatas.Sort(SortDirectPurchaseDatas);
    }

    /// <summary>
    /// giftbag表活动关闭
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGiftBagClose(dailypurchase.GiftBagList msg)
    {
        for (int i = 0; i < msg.giftIds.Count; i++)
        {
            int giftId = msg.giftIds[i];
            for (int j = 0; j < listDirectPurchaseDatas.Count; j++)
            {
                DirectPurchaseData directPurchaseData = listDirectPurchaseDatas[j];
                if (directPurchaseData.Giftbag.id == giftId)
                    listDirectPurchaseDatas.RemoveAt(j);
            }
        }
        
        listDirectPurchaseDatas.Sort(SortDirectPurchaseDatas);
    }

    #endregion
}

/// <summary>
/// 单个礼包数据
/// </summary>
public class DirectPurchaseData
{
    /// <summary>
    /// 礼包配置
    /// </summary>
    private GIFTBAG giftbag = new GIFTBAG();

    public GIFTBAG Giftbag
    {
        get => giftbag;
        set { giftbag = value; }
    }

    /// <summary>
    /// 已构次数
    /// </summary>
    public int BuyTimes { get; set; }

    /// <summary>
    /// 是否新礼包
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public long EndTime { get; set; }

    /// <summary>
    /// 是否售罄(0可购买  1已售罄)
    /// </summary>
    public int SoldOut
    {
        get { return BuyTimes >= giftbag.limitTime && giftbag.limitType > 0 ? 1 : 0; }
    }
}

/// <summary>
/// 单个直购回馈奖励数据
/// </summary>
public class DirectPurchaseRewardsData
{
    /// <summary>
    /// 奖励配置
    /// </summary>
    private RECHARGEREWARD rechargereward = new RECHARGEREWARD();

    public RECHARGEREWARD Rechargereward
    {
        get => rechargereward;
        set { rechargereward = value; }
    }

    /// <summary>
    /// 领取状态(0可领取  1未达成  2已领取)
    /// </summary>
    public int ReceiveState = 1;
}