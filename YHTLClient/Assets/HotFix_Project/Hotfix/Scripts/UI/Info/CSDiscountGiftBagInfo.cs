using System;
using System.Collections.Generic;
using dailypurchase;
using Google.Protobuf.Collections;
using TABLE;

/// <summary>
/// 优惠礼包类型
/// </summary>
public enum DiscountGiftBagTab
{
    OpenService = 1, //开服礼包
    SpecialOffer = 2, //特价礼包
    Discount = 3, //优惠礼包
}

public class CSDiscountGiftBagInfo : CSInfo<CSDiscountGiftBagInfo>
{
    public CSDiscountGiftBagInfo()
    {
        Init();
    }

    public override void Dispose()
    {
    }

    void Init()
    {
        var arr = GiftBagTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var value = arr[i].Value as GIFTBAG;
            if (value.type == 0 && value.subType > 0 && value.subType < 4)
            {
                DirectPurchaseData directPurchaseData = new DirectPurchaseData();
                directPurchaseData.Giftbag = value;
                allDiscountGiftBagDatas.Add(value.id, directPurchaseData);
            }
        }
    }

    /// <summary>
    /// 所有优惠礼包
    /// </summary>
    Dictionary<int, DirectPurchaseData> allDiscountGiftBagDatas = new Dictionary<int, DirectPurchaseData>();

    /// <summary>
    /// 开服礼包列表
    /// </summary>
    ILBetterList<DiscountGiftBagGroupData> listOpenServiceBags = new ILBetterList<DiscountGiftBagGroupData>();

    public ILBetterList<DiscountGiftBagGroupData> ListOpenServiceBags => listOpenServiceBags;

    /// <summary>
    /// 特价礼包列表
    /// </summary>
    ILBetterList<DiscountGiftBagGroupData> listSpecialOfferBags = new ILBetterList<DiscountGiftBagGroupData>();

    public ILBetterList<DiscountGiftBagGroupData> ListSpecialOfferBags => listSpecialOfferBags;

    /// <summary>
    /// 优惠礼包列表
    /// </summary>
    ILBetterList<DiscountGiftBagGroupData> listDiscountBags = new ILBetterList<DiscountGiftBagGroupData>();

    public ILBetterList<DiscountGiftBagGroupData> ListDiscountBags => listDiscountBags;


    /// <summary>
    /// 优惠礼包子页签红点信息
    /// </summary>
    RepeatedField<int> listPositionRedpoints = new RepeatedField<int>();

    public RepeatedField<int> ListPositionRedpoints => listPositionRedpoints;


    /// <summary>
    /// 是否存在指定id还未买完礼包
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsHasSpecifyGiftBag(int id)
    {
        for (int i = 0; i < listOpenServiceBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.id == id && directPurchaseData.SoldOut == 0)
                    return true;
            }
        }

        for (int i = 0; i < listSpecialOfferBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listSpecialOfferBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.id == id && directPurchaseData.SoldOut == 0)
                    return true;
            }
        }

        for (int i = 0; i < listDiscountBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listDiscountBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.id == id && directPurchaseData.SoldOut == 0)
                    return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 是否拥有开服礼包中的羽翼礼包 并且没有买完
    /// </summary>
    /// <returns></returns>
    public bool IsHasWingGiftBag()
    {
        if (listOpenServiceBags.Count > 0)
        {
            for (int i = 0; i < listOpenServiceBags.Count; i++)
            {
                DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
                for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
                {
                    DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                    if (directPurchaseData.Giftbag.id == 1093 /*羽翼礼包写死*/ && directPurchaseData.SoldOut == 0)
                        return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// 是否有必买的还没买完的礼包(还要未购买过)
    /// </summary>
    /// <returns></returns>
    public bool HasMustBuy()
    {
        for (int i = 0; i < listOpenServiceBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.tag1 == 1 && directPurchaseData.BuyTimes <= 0)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }

        for (int i = 0; i < listSpecialOfferBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listSpecialOfferBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.tag1 == 1 && directPurchaseData.BuyTimes <= 0)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }

        for (int i = 0; i < listDiscountBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listDiscountBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.Giftbag.tag1 == 1 && directPurchaseData.BuyTimes <= 0)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否有未查看的新礼包(还要未购买过)
    /// </summary>
    /// <returns></returns>
    public bool HasNew()
    {
        for (int i = 0; i < listOpenServiceBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.IsNew)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }


        for (int i = 0; i < listSpecialOfferBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listSpecialOfferBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.IsNew)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }

        for (int i = 0; i < listDiscountBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listDiscountBags[i];
            bool isActiveRed = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.IsNew)
                    isActiveRed = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isActiveRed = false;
                    break;
                }
            }

            if (isActiveRed)
                return true;
        }

        return false;
    }


    /// <summary>
    /// 是否有开服礼包页签红点
    /// </summary>
    /// <returns></returns>
    public bool IsHasRedPointForPosition1()
    {
        if (listPositionRedpoints.Contains(1) || listOpenServiceBags.Count <= 0) return false;

        for (int i = 0; i < listOpenServiceBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
            bool isRedPoint = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if ((directPurchaseData.IsNew || directPurchaseData.Giftbag.tag1 == 1) &&
                    directPurchaseData.BuyTimes <= 0)
                    isRedPoint = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isRedPoint = false;
                    break;
                }
            }

            if (isRedPoint)
                return true;
        }

        return false;
    }


    /// <summary>
    /// 是否有特价礼包页签红点
    /// </summary>
    /// <returns></returns>
    public bool IsHasRedPointForPosition2()
    {
        if (listPositionRedpoints.Contains(2) || listSpecialOfferBags.Count <= 0) return false;
        for (int i = 0; i < listSpecialOfferBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listSpecialOfferBags[i];
            bool isRedPoint = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if ((directPurchaseData.IsNew || directPurchaseData.Giftbag.tag1 == 1) &&
                    directPurchaseData.BuyTimes <= 0)
                    isRedPoint = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isRedPoint = false;
                    break;
                }
            }

            if (isRedPoint)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否有优惠礼包红点
    /// </summary>
    /// <returns></returns>
    public bool IsHasRedPointForPosition3()
    {
        if (listPositionRedpoints.Contains(3) || listDiscountBags.Count <= 0) return false;
        for (int i = 0; i < listDiscountBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listDiscountBags[i];
            bool isRedPoint = false;
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if ((directPurchaseData.IsNew || directPurchaseData.Giftbag.tag1 == 1) &&
                    directPurchaseData.BuyTimes <= 0)
                    isRedPoint = true;

                if (directPurchaseData.BuyTimes > 0)
                {
                    isRedPoint = false;
                    break;
                }
            }

            if (isRedPoint)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 优惠礼包系统有无红点
    /// </summary>
    /// <returns></returns>
    public bool IsHasRedPoint()
    {
        return IsHasRedPointForPosition1() || IsHasRedPointForPosition2() || IsHasRedPointForPosition3();
    }

    #region 网络响应函数

    /// <summary>
    /// 处理直购礼包消息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGiftData(dailypurchase.DailyPurchaseInfoResponse msg)
    {
        if (msg == null) return;
        listOpenServiceBags.Clear();
        listSpecialOfferBags.Clear();
        listDiscountBags.Clear();
        for (int i = 0, max = msg.giftBuyInfos.Count; i < max; i++)
        {
            GiftBuyInfo giftBuyInfo = msg.giftBuyInfos[i];
            if (allDiscountGiftBagDatas.TryGetValue(giftBuyInfo.giftId, out DirectPurchaseData purchaseData))
            {
                purchaseData.BuyTimes = giftBuyInfo.buyTimes;
                purchaseData.IsNew = giftBuyInfo.isNew;
                purchaseData.EndTime = giftBuyInfo.endTime;
                ILBetterList<DiscountGiftBagGroupData> list;
                switch (purchaseData.Giftbag.subType)
                {
                    case 1:
                        list = listOpenServiceBags;
                        break;
                    case 2:
                        list = listSpecialOfferBags;
                        break;
                    case 3:
                        list = listDiscountBags;
                        break;
                    default:
                        continue;
                }

                if (list != null)
                {
                    int groupId = purchaseData.Giftbag.group;
                    if (groupId > 0) //礼包组
                    {
                        bool isExist = false;
                        for (int j = 0; j < list.Count; j++)
                        {
                            DiscountGiftBagGroupData discountGiftBagGroupData = list[j];
                            if (discountGiftBagGroupData.GroupId == groupId)
                            {
                                if (!discountGiftBagGroupData.ListGiftBags.Contains(purchaseData) &&
                                    discountGiftBagGroupData.ListGiftBags.Count < 3)
                                    discountGiftBagGroupData.ListGiftBags.Add(purchaseData);
                                isExist = true;
                            }
                        }

                        if (!isExist)
                        {
                            //初次赋值展示礼包数据
                            DiscountGiftBagGroupData discountGiftBagData = new DiscountGiftBagGroupData();
                            discountGiftBagData.Id = purchaseData.Giftbag.id;
                            discountGiftBagData.GroupId = purchaseData.Giftbag.group;
                            discountGiftBagData.GiftBagData = purchaseData;
                            discountGiftBagData.BuyTimes = purchaseData.BuyTimes;
                            discountGiftBagData.IsNew = purchaseData.IsNew;
                            discountGiftBagData.EndTime = purchaseData.EndTime;
                            if (!discountGiftBagData.ListGiftBags.Contains(purchaseData) &&
                                discountGiftBagData.ListGiftBags.Count < 3)
                                discountGiftBagData.ListGiftBags.Add(purchaseData);
                            list.Add(discountGiftBagData);
                        }
                    }
                    else //单个礼包一组(groupId==0)
                    {
                        DiscountGiftBagGroupData discountGiftBagData = new DiscountGiftBagGroupData();
                        discountGiftBagData.Id = purchaseData.Giftbag.id;
                        discountGiftBagData.GiftBagData = purchaseData;
                        discountGiftBagData.BuyTimes = purchaseData.BuyTimes;
                        discountGiftBagData.IsNew = purchaseData.IsNew;
                        discountGiftBagData.EndTime = purchaseData.EndTime;
                        if (!discountGiftBagData.ListGiftBags.Contains(purchaseData) &&
                            discountGiftBagData.ListGiftBags.Count < 3)
                            discountGiftBagData.ListGiftBags.Add(purchaseData);
                        list.Add(discountGiftBagData);
                    }
                }
            }
        }

        SortListDiscountGiftBags(listOpenServiceBags);
        SortListDiscountGiftBags(listSpecialOfferBags);
        SortListDiscountGiftBags(listDiscountBags);
    }

    /// <summary>
    /// 礼包排序
    /// </summary>
    /// <param name="list"></param>
    void SortListDiscountGiftBags(ILBetterList<DiscountGiftBagGroupData> list)
    {
        if (list == null || list.Count <= 0) return;
        for (int i = 0; i < list.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = list[i];
            // if (discountGiftBagGroupData.GroupId > 0)
            // {
            discountGiftBagGroupData.ListGiftBags.Sort(SortDirectPurchaseDatas);
            discountGiftBagGroupData.Id = discountGiftBagGroupData.ListGiftBags[0].Giftbag.id;
            discountGiftBagGroupData.BuyTimes = discountGiftBagGroupData.ListGiftBags[0].BuyTimes;
            discountGiftBagGroupData.IsNew = discountGiftBagGroupData.ListGiftBags[0].IsNew;
            discountGiftBagGroupData.GiftBagData = discountGiftBagGroupData.ListGiftBags[0];
            discountGiftBagGroupData.EndTime = discountGiftBagGroupData.ListGiftBags[0].EndTime;
            // }
        }

        list.Sort(SortDiscountGiftBagGroupData);
    }

    /// <summary>
    /// 同一个页签下的大礼包排序
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int SortDiscountGiftBagGroupData(DiscountGiftBagGroupData a, DiscountGiftBagGroupData b)
    {
        bool isNewA = false;
        bool isNewB = false;
        for (int i = 0; i < a.ListGiftBags.Count; i++)
        {
            DirectPurchaseData directPurchaseData = a.ListGiftBags[i];
            if (directPurchaseData.IsNew)
            {
                isNewA = true;
                break;
            }
        }

        for (int i = 0; i < b.ListGiftBags.Count; i++)
        {
            DirectPurchaseData directPurchaseData = b.ListGiftBags[i];
            if (directPurchaseData.IsNew)
            {
                isNewB = true;
                break;
            }
        }

        if (isNewA != isNewB)
        {
            return isNewA ? -1 : 1;
        }
        else
        {
            if (a.GiftBagData.SoldOut != b.GiftBagData.SoldOut)
                return a.GiftBagData.SoldOut.CompareTo(b.GiftBagData.SoldOut);
            else
                return a.Id.CompareTo(b.Id);
        }
    }

    /// <summary>
    /// 同一个大礼包里的小礼包组排序
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int SortDirectPurchaseDatas(DirectPurchaseData a, DirectPurchaseData b)
    {
        if (a.SoldOut != b.SoldOut)
            return a.SoldOut.CompareTo(b.SoldOut);
        else
            return a.Giftbag.id.CompareTo(b.Giftbag.id);
    }

    /// <summary>
    /// 处理元宝购买开服礼包响应
    /// </summary>
    public void HandleBuylistOpenServiceBags(DailyPurchaseBuyResponse msg)
    {
        if (msg == null) return;
        for (int i = 0; i < listOpenServiceBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listOpenServiceBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (msg.giftBuyInfo.giftId == directPurchaseData.Giftbag.id)
                {
                    directPurchaseData.BuyTimes = msg.giftBuyInfo.buyTimes;
                    directPurchaseData.IsNew = msg.giftBuyInfo.isNew;
                    directPurchaseData.EndTime = msg.giftBuyInfo.endTime;
                    break;
                }
            }
        }

        SortListDiscountGiftBags(listOpenServiceBags);
    }

    /// <summary>
    /// 处理元宝购买特价礼包响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleBuylistSpecialOfferBags(DailyPurchaseBuyResponse msg)
    {
        if (msg == null) return;
        for (int i = 0; i < listSpecialOfferBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listSpecialOfferBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (msg.giftBuyInfo.giftId == directPurchaseData.Giftbag.id)
                {
                    directPurchaseData.BuyTimes = msg.giftBuyInfo.buyTimes;
                    directPurchaseData.IsNew = msg.giftBuyInfo.isNew;
                    directPurchaseData.EndTime = msg.giftBuyInfo.endTime;
                    break;
                }
            }
        }

        SortListDiscountGiftBags(listSpecialOfferBags);
    }

    /// <summary>
    /// 处理元宝购买优惠礼包响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleBuylistDiscountBags(DailyPurchaseBuyResponse msg)
    {
        if (msg == null) return;
        for (int i = 0; i < listDiscountBags.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = listDiscountBags[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (msg.giftBuyInfo.giftId == directPurchaseData.Giftbag.id)
                {
                    directPurchaseData.BuyTimes = msg.giftBuyInfo.buyTimes;
                    directPurchaseData.IsNew = msg.giftBuyInfo.isNew;
                    directPurchaseData.EndTime = msg.giftBuyInfo.endTime;
                    break;
                }
            }
        }

        SortListDiscountGiftBags(listDiscountBags);
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
            if (allDiscountGiftBagDatas.TryGetValue(giftId, out DirectPurchaseData item))
            {
                switch (item.Giftbag.gainType)
                {
                    case 1:
                        AddGiftBag(listOpenServiceBags, item);
                        break;
                    case 2:
                        AddGiftBag(listSpecialOfferBags, item);
                        break;
                    case 3:
                        AddGiftBag(listDiscountBags, item);
                        break;
                    default:
                        continue;
                }
            }
        }

        SortListDiscountGiftBags(listOpenServiceBags);
        SortListDiscountGiftBags(listSpecialOfferBags);
        SortListDiscountGiftBags(listDiscountBags);
    }

    void AddGiftBag(ILBetterList<DiscountGiftBagGroupData> list, DirectPurchaseData directPurchase)
    {
        if (list == null || list.Count <= 0) return;
        bool isExist = false;
        for (int i = 0; i < list.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = list[i];
            if (discountGiftBagGroupData.GroupId == directPurchase.Giftbag.group)
            {
                isExist = true;
                if (directPurchase.Giftbag.group > 0 &&
                    !discountGiftBagGroupData.ListGiftBags.Contains(directPurchase) &&
                    discountGiftBagGroupData.ListGiftBags.Count < 3)
                {
                    discountGiftBagGroupData.ListGiftBags.Add(directPurchase);
                    return;
                }

                if (directPurchase.Giftbag.group == 0)
                    return;
            }
        }

        if (!isExist)
        {
            DiscountGiftBagGroupData discountGiftBagData = new DiscountGiftBagGroupData();
            discountGiftBagData.Id = directPurchase.Giftbag.id;
            discountGiftBagData.GiftBagData = directPurchase;
            discountGiftBagData.BuyTimes = directPurchase.BuyTimes;
            discountGiftBagData.IsNew = directPurchase.IsNew;
            discountGiftBagData.EndTime = directPurchase.EndTime;
            if (!discountGiftBagData.ListGiftBags.Contains(directPurchase) &&
                discountGiftBagData.ListGiftBags.Count < 3)
                discountGiftBagData.ListGiftBags.Add(directPurchase);

            list.Add(discountGiftBagData);
        }

        SortListDiscountGiftBags(listOpenServiceBags);
        SortListDiscountGiftBags(listSpecialOfferBags);
        SortListDiscountGiftBags(listDiscountBags);
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
            if (allDiscountGiftBagDatas.TryGetValue(giftId, out DirectPurchaseData item))
            {
                switch (item.Giftbag.gainType)
                {
                    case 1:
                        RemoveGiftBag(listOpenServiceBags, giftId);
                        break;
                    case 2:
                        RemoveGiftBag(listSpecialOfferBags, giftId);
                        break;
                    case 3:
                        RemoveGiftBag(listDiscountBags, giftId);
                        break;
                    default:
                        continue;
                }
            }
        }

        SortListDiscountGiftBags(listOpenServiceBags);
        SortListDiscountGiftBags(listSpecialOfferBags);
        SortListDiscountGiftBags(listDiscountBags);
    }

    void RemoveGiftBag(ILBetterList<DiscountGiftBagGroupData> list, int giftId)
    {
        if (list == null || list.Count <= 0) return;
        for (int i = 0; i < list.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = list[i];
            if ((discountGiftBagGroupData.GroupId == 0 ||
                 (discountGiftBagGroupData.GroupId > 0 && discountGiftBagGroupData.ListGiftBags.Count == 1) &&
                 giftId == discountGiftBagGroupData.Id))
            {
                list.RemoveAt(i);
                return;
            }

            if (discountGiftBagGroupData.GroupId > 0 && discountGiftBagGroupData.ListGiftBags.Count > 1)
            {
                for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
                {
                    DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                    if (directPurchaseData.Giftbag.id == giftId)
                        discountGiftBagGroupData.ListGiftBags.RemoveAt(j);
                }
            }
        }
    }

    /// <summary>
    /// 处理查看礼包返回响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleLookGift(dailypurchase.DailyPurchaseInfoResponse msg)
    {
        if (msg == null) return;
        int subType = 0;
        for (int i = 0, max = msg.giftBuyInfos.Count; i < max; i++)
        {
            GiftBuyInfo giftBuyInfo = msg.giftBuyInfos[i];
            if (allDiscountGiftBagDatas.TryGetValue(giftBuyInfo.giftId, out DirectPurchaseData purchaseData))
            {
                subType = purchaseData.Giftbag.subType;
                purchaseData.BuyTimes = giftBuyInfo.buyTimes;
                purchaseData.IsNew = giftBuyInfo.isNew;
                purchaseData.EndTime = giftBuyInfo.endTime;
            }
        }

        switch (subType)
        {
            case 1:
                SortListDiscountGiftBags(listOpenServiceBags);
                break;
            case 2:
                SortListDiscountGiftBags(listSpecialOfferBags);
                break;
            case 3:
                SortListDiscountGiftBags(listDiscountBags);
                break;
        }
    }

    /// <summary>
    /// 处理查看子页签红点返回
    /// </summary>
    /// <param name="msg"></param>
    public void HandleLookPosition(dailypurchase.LookPositionInfo msg)
    {
        if (msg == null) return;
        listPositionRedpoints = msg.position;
    }

    #endregion
}


/// <summary>
/// 单个优惠礼包(组)数据
/// </summary>
public class DiscountGiftBagGroupData
{
    /// <summary>
    /// 当前组Id
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// 当前展示礼包Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 当前展示已购买次数
    /// </summary>
    public int BuyTimes { get; set; }

    /// <summary>
    /// 是否为新礼包
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public long EndTime { get; set; }

    /// <summary>
    /// 当前展示礼包数据
    /// </summary>
    private DirectPurchaseData giftBagData = new DirectPurchaseData();

    public DirectPurchaseData GiftBagData
    {
        get => giftBagData;
        set => giftBagData = value;
    }

    /// <summary>
    /// 当前组礼包数据列表
    /// </summary>
    private ILBetterList<DirectPurchaseData> listGiftBags = new ILBetterList<DirectPurchaseData>();

    public ILBetterList<DirectPurchaseData> ListGiftBags
    {
        get => listGiftBags;
        set => listGiftBags = value;
    }
}