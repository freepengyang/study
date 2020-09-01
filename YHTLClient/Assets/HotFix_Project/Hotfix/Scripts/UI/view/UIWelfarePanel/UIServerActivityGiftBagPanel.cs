using dailypurchase;
using System;
using System.Collections.Generic;
using UnityEngine;


public partial class UIServerActivityGiftBagPanel : UIBasePanel
{
    #region const
    /// <summary>
    /// 商城每页最多能显示的物品数量
    /// </summary>
    const int MaxItemCountPerPage = 6;

    /// <summary> 杂项表中配置的上架id礼包。优先读此配置，再检测开服时间是否上架下架 </summary>
    //const int GiftBagSundryId = 350;//后端已处理好，暂时不用
    #endregion

    CSBetterLisHot<GiftBagData> allDatas;

    List<GiftBagData> refreshDatas;//刷新时绑定的单页数据列表

    /// <summary>
    /// 当前商品页
    /// </summary>
    private int CurShopPage = 1;
    /// <summary>
    /// 总商品页
    /// </summary>
    private int MaxShopPage = 1;
    
    private int GridAPage = 1;
    private int GridBPage = 1;
    /// <summary>
    /// 当前在page中间位置的grid，0为grid1，1为grid2
    /// </summary>
    //private int CurCenterGrid = 0;

    int lastRealIndex = 0;

    


    public override void Init()
	{
		base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mbanner8, "banner8");

        mClientEvent.Reg((uint)CEvent.GiftBagAllChange, GiftBagAllChange);

        GetAllDatas();
    }
	
	public override void Show()
	{
		base.Show();


        RefreshPanel();

    }
	
	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(mbanner8);

        mGrid_A.UnBind<UILimitGiftBagItem>();
        mGrid_B.UnBind<UILimitGiftBagItem>();

        base.OnDestroy();
	}

    void WrapUpdate(GameObject go, int wrapIndex, int realIndex)//warpIndex 0对应gridA,1对应gridB;;realIndex起始为0
    {
        int page = lastRealIndex > realIndex ? CurShopPage - 1 : CurShopPage + 1;
        page = page <= 0 ? MaxShopPage : page > MaxShopPage ? 1 : page;
        lastRealIndex = realIndex;
        if (wrapIndex == 0) GridAPage = page;
        else GridBPage = page;
        RefreshOnePagesItems(wrapIndex, page);
    }

    void OnCenter(GameObject go)
    {
        if (go.GetHashCode() == mGrid_A.gameObject.GetHashCode())
        {
            //CurCenterGrid = 0;
            CurShopPage = GridAPage;
        }
        else if (go.GetHashCode() == mGrid_B.gameObject.GetHashCode())
        {
            //CurCenterGrid = 1;
            CurShopPage = GridBPage;
        }

        RefreshPageDotUI();
    }


    /// <summary>
    /// 刷新某一页grid的物品
    /// </summary>
    /// <param name="wrapIndex">0对应grid1,1对应grid2</param>
    /// <param name="page">刷新对应的商店页数，起始页为1</param>
    void RefreshOnePagesItems(int wrapIndex, int page)
    {
        if (allDatas == null || allDatas.Count < 1) return;
        int passCount = (page - 1) * MaxItemCountPerPage;//小于当前页的物品个数
        int leftCount = allDatas.Count - passCount;//大于等于当前页物品个数 
        int count = leftCount <= MaxItemCountPerPage ? leftCount : MaxItemCountPerPage;//要刷新的个数

        if (count < 1) return;

        if (refreshDatas == null) refreshDatas = new List<GiftBagData>();
        else refreshDatas.Clear();
        for (int i = 0; i < count; i++)
        {
            refreshDatas.Add(allDatas[i + passCount]);
        }

        if (wrapIndex == 0)
        {
            mGrid_A.Bind<GiftBagData, UILimitGiftBagItem>(refreshDatas, mPoolHandleManager);
        }
        else
        {
            mGrid_B.Bind<GiftBagData, UILimitGiftBagItem>(refreshDatas, mPoolHandleManager);
        }

    }

    void RefreshPageDotUI()
    {
        if (MaxShopPage < 2)
        {
            mgrid_pageDot.MaxCount = 0;
            return;
        }
        mgrid_pageDot.MaxCount = MaxShopPage;
        for (int i = 0; i < mgrid_pageDot.MaxCount; i++)
        {
            var selected = mgrid_pageDot.controlList[i].transform.GetChild(1);
            selected.gameObject.SetActive(i + 1 == CurShopPage);
        }
    }


    void GetAllDatas()
    {
        CSBetterLisHot<GiftBagData> list = CSGiftBagInfo.Instance.GetDatas();
        if (list == null) return;
        if (allDatas == null) allDatas = new CSBetterLisHot<GiftBagData>();
        else allDatas.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            allDatas.Add(list[i]);
        }
        MaxShopPage = Mathf.CeilToInt(allDatas.Count / (float)MaxItemCountPerPage);
        mScrollView.enabled = MaxShopPage > 1;
    }
    

    void RefreshPanel()
    {
        if (allDatas == null || allDatas.Count < 1) return;

        mWrap.onInitializeItem = WrapUpdate;
        mCenter.onCenter = OnCenter;

        GridAPage = 1;
        if (MaxShopPage < 2) GridBPage = 1;
        else GridBPage = GridAPage + 1;

        RefreshOnePagesItems(0, GridAPage);
        RefreshOnePagesItems(1, GridBPage);
        lastRealIndex = 0;

        RefreshPageDotUI();
    }


    void GiftBagAllChange(uint id, object data)
    {
        GetAllDatas();

        RefreshPanel();

        RefreshPageDotUI();
    }
}


public class UILimitGiftBagItem : UIBinder
{
    UISprite sp_flag;
    UILabel lb_discount;
    UISprite sp_money;
    UISprite sp_icon;

    UILabel lb_name;
    UILabel lb_buyCount;
    UILabel lb_buyLimitType;
    UILabel lb_cost;

    GameObject obj_soldOut;
    GameObject tex_bg;

    GiftBagData mData;

    EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    public override void Init(UIEventListener handle)
    {
        sp_flag = Get<UISprite>("sp_flag");
        lb_discount = Get<UILabel>("lb_discount");
        sp_money = Get<UISprite>("lb_money/obj_money");
        sp_icon = Get<UISprite>("Item/icon");

        lb_name = Get<UILabel>("lb_name");
        lb_buyCount = Get<UILabel>("lb_count");
        lb_buyLimitType = Get<UILabel>("lb_count/Label");
        lb_cost = Get<UILabel>("lb_money");
        obj_soldOut = Get<GameObject>("sp_soldout");
        tex_bg = Get<GameObject>("giftbag_bg");

        mClientEvent.Reg((uint)CEvent.GiftBagBuyCHange, GiftBagBuyCHange);
    }


    public override void Bind(object data)
    {
        mData = data as GiftBagData;

        CSEffectPlayMgr.Instance.ShowUITexture(tex_bg, "giftbag_bg");

        RefreshAll();

        UIEventListener.Get(tex_bg).onClick = BuyClick;
        UIEventListener.Get(sp_icon.gameObject).onClick = PreviewClick;
    }


    void RefreshAll()
    {
        if (mData == null || mData.config == null) return;

        string greenStr = UtilityColor.GetColorString(ColorType.Green);
        string redStr = UtilityColor.GetColorString(ColorType.Red);

        TABLE.GIFTBAG cfg = mData.config;
        lb_name.text = cfg.name;
        sp_flag.spriteName = cfg.tag1 > 0 ? $"title{cfg.tag1}" : "";
        lb_discount.text = $"{cfg.discount}";
        lb_buyCount.text = $"{mData.buyTimes}/{cfg.limitTime}".BBCode(mData.buyTimes >= cfg.limitTime ? ColorType.Red : ColorType.Green);

        obj_soldOut.SetActive(mData.buyTimes >= cfg.limitTime);

        string oldPrice = cfg.showPrice <= 0 ? "" : $"[dcd5b8][s]{cfg.showPrice}[/s][-] ";
        string nowPrice = $"[ffcc00]{cfg.para}[-]";
        lb_cost.text = $"{oldPrice}{nowPrice}";
        sp_money.spriteName = $"tubiao{CSGiftBagInfo.Instance.GetMoneyId(cfg.gainType)}";

        sp_icon.spriteName = cfg.showPic > 0 ? cfg.showPic.ToString() : ItemTableManager.Instance.GetItemIcon(cfg.rewards);
        sp_icon.color = mData.buyTimes >= cfg.limitTime ? Color.black : CSColor.white;
    }


    void BuyClick(GameObject go)
    {
        if (mData == null || mData.config == null) return;
        if (mData.buyTimes >= mData.config.limitTime) return;

        if (CSGiftBagInfo.Instance.GetMoneyId(mData.config.gainType).GetItemCount() < mData.config.para)
        {
            UtilityTips.ShowPromptWordTips(6, () => 
            {
                UtilityPanel.JumpToPanel(12305);
                UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
            });
            return;
        }

        UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
        {
            (f as UIBuyConfirmPanel).OpenPanelByGiftBag(mData);
        });

        //Net.CSDailyPurchaseBuyMessage(mData.id);
    }


    void PreviewClick(GameObject go)
    {
        if (mData == null || mData.config == null) return;
        UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
        {
            (f as UIUnsealRewardPanel).Show(mData.config.rewards);
        });
    }



    void GiftBagBuyCHange(uint id, object data)
    {
        GiftBagData info = data as GiftBagData;
        if (mData == null || mData.config == null || info == null) return;
        if (mData.id == info.id)
        {
            RefreshAll();
        }
    }
    

    public override void OnDestroy()
    {
        if (mClientEvent != null) mClientEvent.UnRegAll();
        mClientEvent = null;

        sp_flag = null;
        lb_discount = null;
        sp_money = null;
        sp_icon = null;
        lb_name = null;
        lb_buyCount = null;
        lb_buyLimitType = null;
        lb_cost = null;
        obj_soldOut = null;

        if (tex_bg != null) CSEffectPlayMgr.Instance.Recycle(tex_bg);
        tex_bg = null;

        mData = null;
    }
}