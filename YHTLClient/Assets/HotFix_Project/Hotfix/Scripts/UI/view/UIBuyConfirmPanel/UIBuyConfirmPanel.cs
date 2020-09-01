using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuyConfirmPanel : UIBasePanel
{
    #region forms
    UISprite _sp_bg;
    UISprite sp_bg { get { return _sp_bg ?? (_sp_bg = Get<UISprite>("window/bg")); } }

    private GameObject _btn_close;
    private GameObject btn_close { get { return _btn_close ?? (_btn_close = Get("events/btn_close").gameObject); } }

    private GameObject _btn_buy;
    private GameObject btn_buy { get { return _btn_buy ?? (_btn_buy = Get("events/btn_buy").gameObject); } }

    private GameObject _btn_buyAndUse;
    private GameObject btn_buyAndUse { get { return _btn_buyAndUse ?? (_btn_buyAndUse = Get("events/btn_buyAndUse").gameObject); } }

    private GameObject _btn_add;
    private GameObject btn_add { get { return _btn_add ?? (_btn_add = Get("view/UIItemBarPrefab/btn_add").gameObject); } }

    private GameObject _btn_minus;
    private GameObject btn_minus { get { return _btn_minus ?? (_btn_minus = Get("view/UIItemBarPrefab/btn_minus").gameObject); } }

    private UIInput _input_itemCount;
    private UIInput input_itemCount { get { return _input_itemCount ?? (_input_itemCount = Get<UIInput>("view/UIItemBarPrefab/lb_value")); } }

    private UILabel _lb_itemName;
    private UILabel lb_itemName { get { return _lb_itemName ?? (_lb_itemName = Get<UILabel>("view/lb_itemname")); } }

    private UILabel _lb_itemCount;
    private UILabel lb_itemCount { get { return _lb_itemCount ?? (_lb_itemCount = Get<UILabel>("view/UIItemBarPrefab/lb_value")); } }

    private UILabel _lb_cost;
    private UILabel lb_cost { get { return _lb_cost ?? (_lb_cost = Get<UILabel>("view/UIItemBarTotal/lb_value")); } }

    private UISprite _sp_cost;
    private UISprite sp_cost { get { return _sp_cost ?? (_sp_cost = Get<UISprite>("view/UIItemBarTotal/sp_icon")); } }

    private Transform _trans_itembase;
    private Transform trans_itembase { get { return _trans_itembase ?? (_trans_itembase = Get("view/ItemBase")); } }

    private GameObject _obj_limit;
    private GameObject obj_limit { get { return _obj_limit ?? (_obj_limit = Get<GameObject>("view/limit")); } }

    private UILabel _lb_limit;
    private UILabel lb_limit { get { return _lb_limit ?? (_lb_limit = Get<UILabel>("view/limit/lb_limit")); } }


    GameObject _obj_giftItem;
    GameObject obj_giftItem { get { return _obj_giftItem ?? (_obj_giftItem = Get<GameObject>("view/ItemBase/GiftItem")); } }
    private UISprite _sp_giftIcon;
    private UISprite sp_giftIcon { get { return _sp_giftIcon ?? (_sp_giftIcon = Get<UISprite>("view/ItemBase/GiftItem/icon")); } }


    #endregion

    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    private enum BuyConfirmType { Shop, GiftBag,}


    /// <summary>
    /// 当前物品
    /// </summary>
    private TABLE.ITEM ItemCfg;
    /// <summary>
    /// 当前商店信息
    /// </summary>
    private TABLE.SHOP ShopCfg;
    /// <summary>
    /// 在商店中的单价
    /// </summary>
    private int CostValue;

    /// <summary>
    /// 当前数量
    /// </summary>
    private int CurCount;
    /// <summary>
    /// 当前价格
    /// </summary>
    private int CurCost;

    /// <summary>
    /// 数量上限
    /// </summary>
    private int CountLimit;

    /// <summary>
    /// 消耗货币id
    /// </summary>
    int costMoneyId;


    /// <summary>
    /// 关闭窗口回调
    /// </summary>
    private System.Action CloseCallback;


    private int BuyTimesLimit;
    private int BuyTimes;
    bool autoUse;


    UIItemBase itemBase;


    BuyConfirmType shopType;


    GiftBagData giftBagData;


    readonly int NoLimitMaxCount = 999;


    public override void Init()
    {
        base.Init();
        AddCollider();

        UIEventListener.Get(btn_close).onClick = CloseBtnClick;
        UIEventListener.Get(btn_buy).onClick = BuyBtnClick;
        UIEventListener.Get(btn_buyAndUse).onClick = BuyBtnClick;
        UIEventListener.Get(btn_add, true).onClick = CountAddOrSubBtnClick;
        UIEventListener.Get(btn_minus, false).onClick = CountAddOrSubBtnClick;
        input_itemCount.onChange.Add(new EventDelegate(CountInputChanged));

        UIEventListener.Get(sp_cost.gameObject).onClick = MoneySpClick;
    }


    /// <summary>
    /// 打开窗口时调用
    /// </summary>
    /// <param name="itemId">购买的物品id</param>
    /// <param name="closeCallback">关闭窗口回调</param>
    public void OpenPanel(int itemId)
    {
        if (!ItemTableManager.Instance.TryGetValue((int)itemId, out ItemCfg)) 
            return;
        TABLE.SHOP shopCfg = null;
        var arr = ShopTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            if ((arr[k].Value as TABLE.SHOP).itemId == itemId)
            {
                shopCfg = arr[k].Value as TABLE.SHOP;
                break;
            }
        }
        if (shopCfg == null) return;

        InitOnOpen(shopCfg);
    }

    public void OpenPanel(int itemId, TABLE.SHOP shopCfg, System.Action closeCallback)
    {
        if (!ItemTableManager.Instance.TryGetValue((int)itemId, out ItemCfg)) return;

        InitOnOpen(shopCfg, closeCallback);
    }

    public void OpenPanel(int itemId, TABLE.SHOP shopCfg, int buyTimesLimit = 0, int buyTimes = 0, bool _autoUse = false)
    {
        if (!ItemTableManager.Instance.TryGetValue((int)itemId, out ItemCfg)) return;
        BuyTimesLimit = buyTimesLimit;
        BuyTimes = buyTimes;
        autoUse = _autoUse;
        InitOnOpen(shopCfg);
    }

    public void OpenPanel(TABLE.ITEM itemCfg, TABLE.SHOP shopCfg, int buyTimesLimit = 0, int buyTimes = 0, bool _autoUse = false)
    {
        if (itemCfg == null || shopCfg == null) return;
        ItemCfg = itemCfg;
        BuyTimesLimit = buyTimesLimit;
        BuyTimes = buyTimes;
        autoUse = _autoUse;
        InitOnOpen(shopCfg);
    }


    void InitOnOpen(TABLE.SHOP shopCfg, System.Action closeCallback = null)
    {
        if (shopCfg == null || ItemCfg == null) return;
        
        ShopCfg = shopCfg;
        CostValue = ShopCfg.value;
        CloseCallback = closeCallback;
        CurCount = 1;
        if (ShopCfg.Monthlycard > 0)//月卡特殊处理
        {
            CountLimit = 1;
        }
        else
        {
            CountLimit = BuyTimesLimit <= 0 ? NoLimitMaxCount : BuyTimesLimit - BuyTimes < 1 ? 1 : BuyTimesLimit - BuyTimes;
        }

        if (itemBase == null)
            itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, trans_itembase);
        if (itemBase != null)
        {
            itemBase.Refresh(ItemCfg, ItemClick);
            itemBase.SetCount(shopCfg.num, CSColor.white);
        }


        TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg((int)shopCfg.payType);
        sp_cost.spriteName = $"tubiao{costCfg.icon}";
        costMoneyId = costCfg.id;
        obj_giftItem.SetActive(false);
        shopType = BuyConfirmType.Shop;

        lb_itemName.text = ItemCfg.name;
        lb_itemName.color = UtilityCsColor.Instance.GetColor((int)ItemCfg.quality);

        RefreshUI();
    }


    void RefreshUI()
    {
        int offset = 0;
        obj_limit.SetActive(false);
        if (BuyTimesLimit > 0)
        {
            lb_limit.text = string.Format("{0}/{1}", BuyTimes, BuyTimesLimit);
            lb_limit.color = BuyTimes >= BuyTimesLimit ? CSColor.red : CSColor.green;
            offset = 25;
            obj_limit.SetActive(true);
        }

        sp_bg.height = 339 + offset;
        btn_buy.transform.localPosition = new Vector3(0, -170 - offset);
        btn_buyAndUse.transform.localPosition = new Vector3(0, -170 - offset);

        btn_buy.SetActive(!autoUse);
        btn_buyAndUse.SetActive(autoUse);

        RefreshCountAndCostUI();
    }


    public void OpenPanelByGiftBag(GiftBagData data)
    {
        if (data == null || data.config == null) return;
        giftBagData = data;
        BuyTimesLimit = data.config.limitTime;
        BuyTimes = data.buyTimes;
        CountLimit = BuyTimesLimit <= 0 ? NoLimitMaxCount : BuyTimesLimit - BuyTimes < 1 ? 1 : BuyTimesLimit - BuyTimes;

        InitOnOpenByGiftBag();
    }


    void InitOnOpenByGiftBag()
    {
        if (giftBagData == null || giftBagData.config == null) return;
        CostValue = giftBagData.config.para;
        CurCount = 1;

        TABLE.GIFTBAG cfg = giftBagData.config;

        int moneyId = CSGiftBagInfo.Instance.GetMoneyId(cfg.gainType);
        sp_cost.spriteName = $"tubiao{moneyId}";
        costMoneyId = moneyId;
        obj_giftItem.SetActive(true);
        sp_giftIcon.spriteName = cfg.showPic > 0 ? cfg.showPic.ToString() : ItemTableManager.Instance.GetItemIcon(cfg.rewards);

        lb_itemName.text = cfg.name;
        lb_itemName.color = /*UtilityCsColor.Instance.GetColor((int)ItemCfg.quality);*/CSColor.white;

        shopType = BuyConfirmType.GiftBag;

        RefreshUI();

        RefreshCountAndCostUI();
    }



    void RefreshCountAndCostUI()
    {
        CurCost = CostValue * CurCount;

        lb_itemCount.text = CurCount.ToString();
        lb_cost.text = CurCost.ToString();
        if (shopType == BuyConfirmType.Shop && ShopCfg != null)
        {
            lb_cost.color = ShopCfg.payType.GetItemCount() < CurCost ? CSColor.red : CSColor.beige;
        }
        else if (shopType == BuyConfirmType.GiftBag && giftBagData != null && giftBagData.config != null)
        {
            lb_cost.color = CSGiftBagInfo.Instance.GetMoneyId(giftBagData.config.gainType).GetItemCount() < CurCost ? CSColor.red : CSColor.beige;
        }
    }


    void ClosePanel()
    {
        UIManager.Instance.ClosePanel<UIBuyConfirmPanel>();
        if (CloseCallback != null) CloseCallback();
    }

    protected override void OnDestroy()
    {
        if (itemBase != null)
        {
            UIItemManager.Instance.RecycleSingleItem(itemBase);
        }
        
        base.OnDestroy();
    }


    #region btnClick
    void CloseBtnClick(GameObject _go)
    {
        ClosePanel();
    }

    void CountAddOrSubBtnClick(GameObject _go)
    {
        bool isAdd = (bool)UIEventListener.Get(_go).parameter;
        if (isAdd)
        {
            if (BuyTimesLimit > 0 && CurCount >= CountLimit)
            {
                UtilityTips.ShowRedTips(1699);
            }
            CurCount = CurCount >= CountLimit ? CountLimit : CurCount + 1;
        }
        else
        {
            CurCount = CurCount <= 1 ? 1 : CurCount - 1;
        }

        RefreshCountAndCostUI();
    }


    void BuyBtnClick(GameObject _go)
    {
        if (BuyTimesLimit > 0 && BuyTimesLimit - BuyTimes < 1)
        {
            UtilityTips.ShowRedTips("购买次数达到上限");
            ClosePanel();
            return;
        }

        switch (shopType)
        {
            case BuyConfirmType.Shop:
                if (ShopCfg == null) return;
                if (ShopCfg.payType.GetItemCount() < CurCost)//货币不足
                {
                    //if (ShopCfg.payType == (int)MoneyType.yuanbao)
                    //{
                    //    YuanBaoNotEnough();
                    //    return;
                    //}
                    TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg((int)ShopCfg.payType);
                    Utility.ShowGetWay(costCfg.id);
                }
                else
                {
                    if (ShopCfg.Monthlycard > 0)
                    {
                        CSMonthCardInfo.Instance.TryToBuyCard(ShopCfg.Monthlycard);
                    }
                    else Net.CSShopBuyItemMessage(ShopCfg.id, CurCount, false, autoUse);
                }
                break;
            case BuyConfirmType.GiftBag:
                if (giftBagData == null || giftBagData.config == null) return;
                int moneyId = CSGiftBagInfo.Instance.GetMoneyId(giftBagData.config.gainType);
                if (moneyId.GetItemCount() < CurCost)
                {
                    if (moneyId == (int)MoneyType.yuanbao)
                    {
                        YuanBaoNotEnough();
                        return;
                    }
                    TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg(moneyId);
                    Utility.ShowGetWay(costCfg.id);
                }
                else Net.CSDailyPurchaseBuyMessage(giftBagData.id, CurCount);
                break;
        }        

        ClosePanel();
    }


    void CountInputChanged()
    {
        if (string.IsNullOrEmpty(input_itemCount.value)) return;
        int count = 0;
        if (!int.TryParse(input_itemCount.value, out count) || count < 0)
        {
            count = 0;
        }

        if (count > CountLimit) count = CountLimit;

        input_itemCount.value = count.ToString();
        CurCount = count;

        RefreshCountAndCostUI();
    }

    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    void MoneySpClick(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, costMoneyId);
    }

    #endregion


    //元宝不足处理
    void YuanBaoNotEnough()
    {
        if (!CSVipInfo.Instance.IsFirstRecharge())
        {
            UtilityTips.ShowPromptWordTips(5, () =>
            {
                UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
                ClosePanel();
            });
            return;
        }

        UtilityTips.ShowPromptWordTips(6, () =>
        {
            UtilityPanel.JumpToPanel(12305);
            ClosePanel();
        });
    }

}
