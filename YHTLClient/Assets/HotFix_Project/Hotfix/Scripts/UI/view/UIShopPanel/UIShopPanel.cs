using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class UIShopPanel : UIBasePanel
{

    private enum ShopSubType { Hot = 1, Enhance, Common, Limitation }//现在改为 常用 绑元 两种


    FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>> hotPages;
    FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>> enhancePages;


    int maxShopPage;
    int curPage;
    ShopSubType curSubType;


    float oldScrollValue;


    /// <summary>
    /// 翻页需要的距离
    /// </summary>
    float FlipPageNeededValue { get { return 0.05f; } }


    public override void Init()
	{
		base.Init();

        //mClientEvent.Reg((uint)CEvent.MoneyChange, OnMoneyChange);
        mClientEvent.Reg((uint)CEvent.ShopBuyTimesChange, OnBuyTimesChange);
        mClientEvent.Reg((uint)CEvent.ShopInfoChange, OnBuyTimesChange);

        UIEventListener.Get(mtg_hot.gameObject, ShopSubType.Hot).onClick = SubtypePageCilck;
        UIEventListener.Get(mtg_enhance.gameObject, ShopSubType.Enhance).onClick = SubtypePageCilck;
        //底部货币获取
        UIEventListener.Get(mbtn_moneyadd1, MoneyType.yuanbao).onClick = AddMoneyClick;
        UIEventListener.Get(mbtn_moneyadd2, MoneyType.bindGold).onClick = AddMoneyClick;
        UIEventListener.Get(mbtn_moneyadd3, MoneyType.gold).onClick = AddMoneyClick;

        hotPages = new FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>>(16,Get, Put);
        enhancePages = new FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>>(16,Get, Put);

        mscroll.onDragFinished += OnDragFinished;

        InitLists();

        //RefreshMoneyUI(true);

        RefreshPages(ShopSubType.Hot, true);

    }

    public FastArrayElementKeepHandle<CSShopCommodityData> Get()
    {
        var element = mPoolHandleManager.GetSystemClass<FastArrayElementKeepHandle<CSShopCommodityData>>();
        element.Clear();
        return element;
    }

    public void Put(FastArrayElementKeepHandle<CSShopCommodityData> element)
    {
        if(null != element)
        {
            element.Clear();
            mPoolHandleManager.Recycle(element);
        }
    }

    public override void Show()
	{
		base.Show();

        //RefreshMoneyUI();
        
    }
	
	protected override void OnDestroy()
	{
        mgrid_Page.UnBind<UIShopPage>();


		base.OnDestroy();
	}


    public override void SelectChildPanel(int type = 1)
    {
        if (type == (int)ShopSubType.Enhance)
        {
            RefreshPages(ShopSubType.Enhance, true);
        }
        else
        {
            RefreshPages(ShopSubType.Hot, true);
        }
    }

    void InitLists()
    {
        AddShopElementsToArray((int)ShopSubType.Hot, hotPages);
        AddShopElementsToArray((int)ShopSubType.Enhance, enhancePages);
    }


    void AddShopElementsToArray(int dicKey, FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>> array)
    {
        var dic = CSShopInfo.Instance.AllShopInfo;
        if (dic == null || array == null) return;
        if (dic.ContainsKey(dicKey))
        {
            var list = dic[dicKey];
            if (list == null || list.Count < 1) return;

            int pageIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                FastArrayElementKeepHandle<CSShopCommodityData> pageList = null;
                if (array.Count <= pageIndex)
                {
                    pageList = array.Append();
                }
                else pageList = array[pageIndex];
                if (pageList == null) continue;
                if (pageList.Count >= 12)
                {
                    pageIndex++;
                    i--;
                    continue;
                }
                pageList.Append(list[i]);
            }
        }
    }



    void RefreshMoneyUI(bool isInit = false)
    {
        if (isInit)
        {
            TABLE.ITEM ingotCfg = ItemTableManager.Instance.GetItemCfg((int)MoneyType.yuanbao);
            TABLE.ITEM bindGoldCfg = ItemTableManager.Instance.GetItemCfg((int)MoneyType.bindGold);
            TABLE.ITEM goldCfg = ItemTableManager.Instance.GetItemCfg((int)MoneyType.gold);
            if (ingotCfg != null) msp_moneyicon1.spriteName = $"tubiao{ingotCfg.icon}";
            if (bindGoldCfg != null) msp_moneyicon2.spriteName = $"tubiao{bindGoldCfg.icon}";
            if (goldCfg != null) msp_moneyicon3.spriteName = $"tubiao{goldCfg.icon}";
        }

        mlb_moneyvalue1.text = ((int)MoneyType.yuanbao).GetItemCount().ToString();
        mlb_moneyvalue2.text = ((int)MoneyType.bindGold).GetItemCount().ToString();
        mlb_moneyvalue3.text = ((int)MoneyType.gold).GetItemCount().ToString();
    }



    void RefreshPages(ShopSubType subType, bool needReset = false)
    {
        curSubType = subType;
        maxShopPage = 0;
        switch (subType)
        {
            case ShopSubType.Hot:
                mgrid_Page.Bind<FastArrayElementKeepHandle<CSShopCommodityData>, UIShopPage>(hotPages, mPoolHandleManager);
                maxShopPage = hotPages.Count;
                break;
            case ShopSubType.Enhance:
                mgrid_Page.Bind<FastArrayElementKeepHandle<CSShopCommodityData>, UIShopPage>(enhancePages, mPoolHandleManager);
                maxShopPage = enhancePages.Count;
                break;
        }

        if (maxShopPage < 1) return;

        if (needReset)
        {
            curPage = 1;
            mscroll.ScrollImmidate(0);
            oldScrollValue = /*mscroll.horizontalScrollBar.value*/0;
        }
        else
        {

        }

        RefreshPageDots(curPage);
    }


    void RefreshPageDots(int _curPage)
    {
        if (maxShopPage < 2)
        {
            mgrid_pageDot.MaxCount = 0;
            return;
        }

        curPage = _curPage;

        mgrid_pageDot.MaxCount = maxShopPage;
        for (int i = 0; i < mgrid_pageDot.MaxCount; i++)
        {
            var selected = mgrid_pageDot.controlList[i].transform.GetChild(1);
            selected.gameObject.SetActive(i + 1 == curPage);
        }
    }


    void OnDragFinished()
    {
        float value = mscrollBar.value;
        if (maxShopPage < 1 || Mathf.Abs(value - oldScrollValue) < FlipPageNeededValue)
        {
            ScrollDoReset();
            return;
        }
        bool toLeft = value < oldScrollValue;
        if (toLeft && curPage > 1)
        {
            curPage--;
            ScrollDoCenter();
        }
        else if (!toLeft && curPage < maxShopPage)
        {
            curPage++;
            ScrollDoCenter();
        }
        else
        {
            ScrollDoReset();
            return;
        }

        RefreshPageDots(curPage);
    }


    void ScrollDoCenter()
    {
        float newValue = (float)(curPage - 1) / (float)(maxShopPage - 1);
        oldScrollValue = newValue;
        TweenProgressBar.Begin(mscrollBar, 0.2f, mscrollBar.value, newValue);
    }


    void ScrollDoReset()
    {
        TweenProgressBar.Begin(mscrollBar, 0.2f, mscrollBar.value, oldScrollValue);
    }

    

    #region CEvents
    void OnMoneyChange(uint id, object data)
    {
        RefreshMoneyUI();
    }

    void OnBuyTimesChange(uint id, object data)
    {
        RefreshPages(curSubType);
    }

    #endregion

    void AddMoneyClick(GameObject _go)
    {
        MoneyType moneyType = (MoneyType)UIEventListener.Get(_go).parameter;
        Utility.ShowGetWay((int)moneyType);
    }

    void SubtypePageCilck(GameObject go)
    {
        ShopSubType subtype = (ShopSubType)UIEventListener.Get(go).parameter;
        RefreshPages(subtype, true);
    }

}



public class UIShopItem : UIBinder
{
    protected Transform _trans_itemBase;

    protected UILabel _lb_name;

    protected GameObject _obj_flag;

    protected UISprite _sp_flag;

    protected Transform _trans_money;

    protected UILabel _lb_money;

    protected UISprite _sp_money;

    protected GameObject _obj_limit;

    protected UILabel _lb_limit;

    protected UILabel _lb_special;


    protected UIItemBase itemBase;
    protected CSShopCommodityData mData;

    protected int BuyTimesLimit;
    protected int BuyTimes;


    protected readonly string[] flagIcon = { "title5", "title3", "title2", "title4", "title1" };


    public override void Init(UIEventListener handle)
    {
        _trans_itemBase = Get("ItemBase");
        _lb_name = Get<UILabel>("lb_name");
        _obj_flag = Get<GameObject>("flag");
        _sp_flag = Get<UISprite>("flag");
        _trans_money = Get("lb_money");
        _lb_money = Get<UILabel>("lb_money");
        _sp_money = Get<UISprite>("lb_money/obj_money");
        _obj_limit = Get<GameObject>("lb_limit");
        _lb_limit = Get<UILabel>("lb_limit");
        _lb_special = Get<UILabel>("lb_special");

    }

    public override void Bind(object data)
    {
        mData = null;
        mData = data as CSShopCommodityData;
        if (mData == null || mData.config == null || mData.itemCfg == null) return;
        TABLE.SHOP shopCfg = mData.config;

        if (itemBase == null)
        {
            itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, _trans_itemBase);
        }
        itemBase.Refresh(mData.itemCfg, ItemClick);
        itemBase.SetCount(shopCfg.num, CSColor.white);

        _lb_name.text = mData.itemCfg.name;
        _lb_name.color = UtilityCsColor.Instance.GetColor((int)mData.itemCfg.quality);

        BuyTimesLimit = mData.buyTimesLimit;        

        bool isLimit = BuyTimesLimit > 0;

        _trans_money.localPosition = isLimit ? new Vector2(-262, 140) : new Vector2(-262, 120);
        string orgCost = shopCfg.showMoney > 0 ? shopCfg.showMoney.ToString() : "";
        string curCost = shopCfg.value.ToString();
        _lb_money.text = string.Format("[s]{0}[/s] {1}", orgCost, curCost);
        TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg((int)shopCfg.payType);
        _sp_money.spriteName = $"tubiao{costCfg.icon}";

        _obj_limit.SetActive(isLimit);
        if (isLimit)
        {
            BuyTimes = CSShopInfo.Instance.GetBuyTimes(shopCfg.id);
            string color = BuyTimes < BuyTimesLimit ? UtilityColor.MainText : UtilityColor.Red;
            _lb_limit.text = string.Format("{0}{1}/{2}", color, BuyTimes, BuyTimesLimit);
        }

        SetIconFlag();

        _lb_special.gameObject.SetActive(shopCfg.Monthlycard > 0);
        if (shopCfg.Monthlycard > 0)
        {
            UIEventListener.Get(_lb_special.gameObject).onClick = MonthCardDetailsClick;
        }

        if (Handle != null)
        {
            Handle.onClick = QuickBuyBtnClick;
        }
    }
    
    public void SetIconFlag()
    {
        if (_obj_flag == null || _sp_flag == null) return;
        _obj_flag.SetActive(false);
        if (string.IsNullOrEmpty(mData.config.Recommend)) return;
        
        int flag = 0;
        if (!int.TryParse(mData.config.Recommend, out flag)) return;

        if (flag < 1 || flag >= flagIcon.Length) return;
        
        _obj_flag.SetActive(true);
        _sp_flag.spriteName = flagIcon[flag - 1];
    }

    public void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    public virtual void QuickBuyBtnClick(GameObject _go)
    {
        if (itemBase.itemCfg == null || mData == null || mData.config == null) return;

        if (BuyTimesLimit > 0 && BuyTimes >= BuyTimesLimit)
        {
            UtilityTips.ShowRedTips(1699);
            return;
        }

        UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
        {
            (f as UIBuyConfirmPanel).OpenPanel(itemBase.itemCfg, mData.config, BuyTimesLimit, BuyTimes);
        });

    }

    public void MonthCardDetailsClick(GameObject _go)
    {
        if (mData.config.Monthlycard > 0)
        {
            UIManager.Instance.ClosePanel<UIShopCombinePanel>();
            UtilityPanel.JumpToPanel(12600);
        }
    }

    public override void OnDestroy()
    {
        if (itemBase != null) UIItemManager.Instance.RecycleSingleItem(itemBase);
        itemBase = null;
        mData = null;

        _trans_itemBase = null;
        _lb_name = null;
        _obj_flag = null;
        _sp_flag = null;
        _trans_money = null;
        _lb_money = null;
        _sp_money = null;
        _obj_limit = null;
        _lb_limit = null;
        _lb_special = null;
    }
}


public class UIShopPage : UIBinder
{

    UIGridContainer grid;

    public override void Init(UIEventListener handle)
    {
        grid = handle.GetComponent<UIGridContainer>();
    }
    public override void Bind(object data)
    {
        FastArrayElementKeepHandle<CSShopCommodityData> list = data as FastArrayElementKeepHandle<CSShopCommodityData>;
        if (list == null || list.Count < 1 || list.Count > 12)
        {
            FNDebug.LogError("@@@@商品页列表错误");
            return;
        }

        if (grid != null)
        {
            grid.Bind<CSShopCommodityData, UIShopItem>(list, PoolHandle);
        }
    }

    public override void OnDestroy()
    {
        if (grid != null)
        {
            grid.UnBind<UIShopItem>();
        }
        grid = null;
    }
}