using System.Collections.Generic;
using Google.Protobuf.Collections;
using Unity.Collections;
using UnityEngine;

public class UIWelfareGiftBagBinder : UIBinder
{
    private UIEventListener bg;
    private UISprite sp_flag;
    private UILabel lb_name;
    private GameObject sp_time;
    private UILabel lb_time;
    private UIGridContainer grid_bag;
    private UILabel lb_count;
    private UILabel lb_count_name;

    private UILabel btn_money;

    // private UIEventListener btn_buy;
    private GameObject sp_new;
    private GameObject sp_soldout;
    private GameObject redpoint;
    RepeatedField<int> viewedIds;

    private DiscountGiftBagGroupData discountGiftBagGroupData;

    // public bool isActive = true;
    public long sec = 0;
    Dictionary<int, int> dicBoxItem = new Dictionary<int, int>();

    TABLE.RECHARGE recharge;
    PoolHandleManager mPoolHandle = new PoolHandleManager();

    public System.Action<GameObject, Vector2> OnDrag;
    public System.Action<GameObject> OnDragEnd;

    public override void Init(UIEventListener handle)
    {
        bg = Get<UIEventListener>("bg");
        sp_flag = Get<UISprite>("sp_flag");
        lb_name = Get<UILabel>("lb_name");
        sp_time = Get<GameObject>("sp_time");
        lb_time = Get<UILabel>("lb_time", sp_time.transform);
        grid_bag = Get<UIGridContainer>("grid_bag");
        lb_count = Get<UILabel>("lb_count");
        lb_count_name = Get<UILabel>("Label", lb_count.transform);
        btn_money = Get<UILabel>("btn_money");
        // btn_buy = Get<UIEventListener>("Sprite", btn_money.transform);
        sp_new = Get<GameObject>("sp_new");
        sp_soldout = Get<GameObject>("sp_soldout");
        redpoint = Get<GameObject>("redpoint");
        CSEffectPlayMgr.Instance.ShowUITexture(bg.gameObject, "gift_bag_scroll");
        // UIEventListener.Get(btn_buy.gameObject).onClick = OnClickBtnBuy;
        // UIEventListener.Get(btn_buy.gameObject).onDrag = OnDragBg;
        // UIEventListener.Get(btn_buy.gameObject).onDragEnd = OnDragBgEnd;
        UIEventListener.Get(bg.gameObject).onDrag = OnDragBg;
        UIEventListener.Get(bg.gameObject).onDragEnd = OnDragBgEnd;
        UIEventListener.Get(bg.gameObject).onClick = OnClickBtnBuy;
    }

    public override void Bind(object data)
    {
        if (data == null) return;

        // if (!isActive)
        // {
        //     widget.alpha = 0;
        //     return;
        // }

        discountGiftBagGroupData = (DiscountGiftBagGroupData) data;
        if (discountGiftBagGroupData.GiftBagData.Giftbag.gainType == 3)
        {
            RechargeTableManager.Instance.TryGetValue(discountGiftBagGroupData.GiftBagData.Giftbag.para, out recharge);
            //if (RechargeTableManager.Instance.TryGetValue(discountGiftBagGroupData.GiftBagData.Giftbag.para,
            //    out recharge))
            //    rechargeData.Init(recharge, RechargeType.DirectPurchase);
        }

        if (discountGiftBagGroupData.EndTime > 0)
            sec = (discountGiftBagGroupData.EndTime - CSServerTime.Instance.TotalMillisecond) / 1000;

        RefreshUI();
    }

    private List<UIItemBase> listItemBase;
    private ILBetterList<ILBetterList<int>> listBoxItem;
    private ILBetterList<int> listTmp;

    void RefreshUI()
    {
        //标题
        lb_name.text = discountGiftBagGroupData.GroupId <= 0
            ? discountGiftBagGroupData.GiftBagData.Giftbag.name
            : discountGiftBagGroupData.GiftBagData.Giftbag.groupName;
        //角标
        int tag1 = discountGiftBagGroupData.GiftBagData.Giftbag.tag1;
        sp_flag.gameObject.SetActive(tag1 > 0);
        if (tag1 > 0)
            sp_flag.spriteName = $"title{tag1}";
        //时间
        sp_time.SetActive(sec > 0);
        //boxItem
        if (listItemBase == null)
            listItemBase = new List<UIItemBase>();
        BoxTableManager.Instance.GetBoxAwardById(discountGiftBagGroupData.GiftBagData.Giftbag.rewards, dicBoxItem);
        grid_bag.MaxCount = Mathf.Min(4, dicBoxItem.Count);
        int index = 0;
        GameObject gp;
        for (var it = dicBoxItem.GetEnumerator(); it.MoveNext();)
        {
            if (index < 4)
            {
                gp = grid_bag.controlList[index];
                if (listItemBase.Count <= index)
                    listItemBase.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform,
                        itemSize.Size60));
                UIItemBase itemBase = listItemBase[index];
                itemBase.ListenDrag();
                itemBase.Refresh(it.Current.Key);
                itemBase.SetCount(it.Current.Value, false, false);
                index++;
            }
        }

        //限购信息
        int limitType = discountGiftBagGroupData.GiftBagData.Giftbag.limitType;
        switch (limitType)
        {
            case 0:
                lb_count.gameObject.SetActive(false);
                sp_soldout.SetActive(false);
                break;
            case 1:
            case 2:
                int buytimes = discountGiftBagGroupData.BuyTimes;
                int limitTime = discountGiftBagGroupData.GiftBagData.Giftbag.limitTime;
                lb_count.text =
                    $"{buytimes}/{limitTime}".BBCode(buytimes >= limitTime
                        ? ColorType.ToolTipUnDone
                        : ColorType.ToolTipDone);

                sp_soldout.SetActive(buytimes >= limitTime);

                if (limitType == 1)
                    lb_count_name.text = CSString.Format(1812);
                else if (limitType == 2)
                    lb_count_name.text = CSString.Format(1811);
                
                lb_count.gameObject.SetActive(discountGiftBagGroupData.GroupId <= 0);
                break;
        }
        

        //按钮
        if (discountGiftBagGroupData.GroupId > 0)
        {
            btn_money.text = CSString.Format(1813);
        }
        else
        {
            switch (discountGiftBagGroupData.GiftBagData.Giftbag.gainType)
            {
                case 1: //元宝
                    btn_money.text = CSString.Format(1843, discountGiftBagGroupData.GiftBagData.Giftbag.para);
                    // redpoint.SetActive(CSBagInfo.Instance.GetMoneyByType(MoneyType.yuanbao) >=
                    //                    directPurchaseData.Giftbag.para);
                    break;
                case 3: //人民币
                    btn_money.text = CSString.Format(1844,
                        RechargeTableManager.Instance.GetRechargeMoney(
                            discountGiftBagGroupData.GiftBagData.Giftbag.para));
                    // redpoint.SetActive(false);
                    break;
            }
        }

        //新礼包图标
        bool isActiveNew = false;
        for (int i = 0; i < discountGiftBagGroupData.ListGiftBags.Count; i++)
        {
            DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[i];
            if (directPurchaseData.IsNew)
            {
                isActiveNew = true;
                break;
            }
        }

        sp_new.SetActive(isActiveNew);

        //红点显示
        bool isActiveRedPoint = false;
        if (!isActiveNew)
        {
            for (int i = 0; i < discountGiftBagGroupData.ListGiftBags.Count; i++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[i];
                if (!directPurchaseData.IsNew && directPurchaseData.Giftbag.tag1 == 1 &&
                    directPurchaseData.BuyTimes <= 0)
                    isActiveRedPoint = true;

                if (directPurchaseData.IsNew || directPurchaseData.BuyTimes > 0)
                {
                    isActiveRedPoint = false;
                    break;
                }
            }
        }

        redpoint.SetActive(isActiveRedPoint);
    }

    public void SetTime()
    {
        lb_time.text = CSServerTime.Instance.FormatLongToTimeStr(sec); //XX:XX:XX
    }

    // void OnScheDule(Schedule _schedule)
    // {
    //     if (sec < 0)
    //     {
    //         if (Timer.Instance.IsInvoking(schedule))
    //             Timer.Instance.CancelInvoke(schedule);
    //     }
    //     else
    //     {
    //         lb_name.text = CSServerTime.Instance.FormatLongToTimeStr(sec); //XX:XX:XX
    //         sec--;
    //     }
    // }

    void OnClickBtnBuy(GameObject go)
    {
        if (discountGiftBagGroupData.GroupId > 0)
        {
            UIManager.Instance.CreatePanel<UIGiftBagPreviewPanel>(
                P => (P as UIGiftBagPreviewPanel).OpenGiftBagPreviewPanel(discountGiftBagGroupData));
        }
        else
        {
            switch (discountGiftBagGroupData.GiftBagData.Giftbag.gainType)
            {
                case 1: //元宝
                    if (CSItemCountManager.Instance.GetItemCount((int) MoneyType.yuanbao) >=
                        discountGiftBagGroupData.GiftBagData.Giftbag.para)
                    {
                        Net.CSDailyPurchaseBuyMessage(discountGiftBagGroupData.GiftBagData.Giftbag.id, 1);
                        if (viewedIds == null)
                            viewedIds = new RepeatedField<int>();
                        viewedIds.Clear();
                        viewedIds.Add(discountGiftBagGroupData.GiftBagData.Giftbag.id);
                        Net.CSLookGiftMessage(viewedIds);
                    }
                    else
                    {
                        // if (CSVipInfo.Instance.IsFirstRecharge()) //充值
                        // {
                        //     UtilityTips.ShowPromptWordTips(6, () => { UtilityPanel.JumpToPanel(12305); });
                        // }
                        // else //活动
                        // {
                        //     UtilityTips.ShowPromptWordTips(5,
                        //         () => { UIManager.Instance.CreatePanel<UIRechargeFirstPanel>(); });
                        // }
                        Utility.ShowGetWay(3);
                    }

                    break;
                case 3: //人民币
                    if (discountGiftBagGroupData.GiftBagData.SoldOut == 0)
                    {
                        CSRechargeInfo.Instance.TryToRecharge(recharge);
                        if (viewedIds == null)
                            viewedIds = new RepeatedField<int>();
                        viewedIds.Clear();
                        viewedIds.Add(discountGiftBagGroupData.GiftBagData.Giftbag.id);
                        Net.CSLookGiftMessage(viewedIds);
                    }
                    else
                    {
                        UtilityTips.ShowRedTips(1842);
                    }

                    break;
            }
        }
    }


    void OnDragBg(GameObject go, Vector2 vec)
    {
        OnDrag?.Invoke(go, vec);
    }

    void OnDragBgEnd(GameObject go)
    {
        OnDragEnd?.Invoke(go);
    }

    public override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(bg.gameObject);
        bg = null;
        sp_flag = null;
        lb_name = null;
        sp_time = null;
        lb_time = null;
        grid_bag = null;
        lb_count = null;
        lb_count_name = null;
        btn_money = null;
        sp_new = null;
        sp_soldout = null;
        discountGiftBagGroupData = null;
        dicBoxItem = null;
        recharge = null;
        mPoolHandle = null;
        OnDrag = null;
        OnDragEnd = null;
        viewedIds = null;
        // widget = null;
        // if (Timer.Instance.IsInvoking(schedule))
        //     Timer.Instance.CancelInvoke(schedule);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBase);
    }
}