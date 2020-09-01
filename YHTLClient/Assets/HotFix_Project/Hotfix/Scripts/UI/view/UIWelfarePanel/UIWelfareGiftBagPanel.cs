using System;
using System.Collections;
using System.Collections.Generic;
using dailypurchase;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIWelfareGiftBagPanel : UIBasePanel
{
    #region 循环滚动相关变量

    /// <summary>
    /// 每页最多能显示的物品数量
    /// </summary>
    private const int OnePagMaxCount = 4;

    /// <summary>
    /// 上一个wrap中的真实索引（所有整数）
    /// </summary>
    int lastRealIndex = 0;

    /// <summary>
    /// 当前页数
    /// </summary>
    private int curPage = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    private int maxPage = 1;

    /// <summary>
    /// grid0当前页数
    /// </summary>
    private int gridPage0 = 1;

    /// <summary>
    /// grid1当前页数
    /// </summary>
    private int gridPage1 = 1;

    /// <summary>
    /// 当前在page中间位置的grid，0为grid0，1为grid1
    /// </summary>
    //private int curCenterGrid = 1;

    /// <summary>
    /// 当前中间grid
    /// </summary>
    private UIGridContainer curGrid;

    #endregion

    /// <summary>
    /// 当前页签下礼包最大数量
    /// </summary>
    private int curMaxCount = 0;

    private DiscountGiftBagTab curTab = DiscountGiftBagTab.OpenService;

    private ILBetterList<DiscountGiftBagGroupData> listOpenServiceBags;
    private ILBetterList<DiscountGiftBagGroupData> listSpecialOfferBags;
    private ILBetterList<DiscountGiftBagGroupData> listDiscountBags;

    /// <summary>
    /// 现在页签下的礼包列表
    /// </summary>
    private ILBetterList<DiscountGiftBagGroupData> curListBags;

    Dictionary<int, int> rewardsDic = new Dictionary<int, int>();

    RepeatedField<int> viewedIds;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.BagItemsDrag, GetItemsDrag);
        mClientEvent.Reg((uint) CEvent.DailyPurchaseInfo, RefreshData);
        mClientEvent.Reg((uint) CEvent.DailyPurchaseBuyDiscount, DailyPurchaseBuy);
        mClientEvent.Reg((uint) CEvent.GiftBagOpen, RefreshData);
        mClientEvent.Reg((uint) CEvent.GiftBagClose, RefreshData);
        mClientEvent.Reg((uint) CEvent.LookGift, RefreshData);
        mClientEvent.Reg((uint) CEvent.LookPosition, RefreshRedPointData);
        UIEventListener.Get(mtoggle_tab1.gameObject, DiscountGiftBagTab.OpenService).onClick = OnClickTab;
        UIEventListener.Get(mtoggle_tab2.gameObject, DiscountGiftBagTab.SpecialOffer).onClick = OnClickTab;
        UIEventListener.Get(mtoggle_tab3.gameObject, DiscountGiftBagTab.Discount).onClick = OnClickTab;
        mWrapContent.onInitializeItem = OnUpdateItem;
        mbtn_locktab2.onClick = OnClickLockTips;
        mbtn_locktab3.onClick = OnClickLockTips;

        mDragScrollView.GetComponent<UIEventListener>().onDrag = OnDrag;
        mDragScrollView.GetComponent<UIEventListener>().onDragEnd = OnDragEnd;

        CSEffectPlayMgr.Instance.ShowUITexture(mbanner25, "banner25");
        mmaskPanel.SetActive(false);


        listOpenServiceBags = CSDiscountGiftBagInfo.Instance.ListOpenServiceBags;
        listSpecialOfferBags = CSDiscountGiftBagInfo.Instance.ListSpecialOfferBags;
        listDiscountBags = CSDiscountGiftBagInfo.Instance.ListDiscountBags;
        RefreshOpenServiceBagsRedPiont();
        RefreshSpecialOfferBagsRedPiont();
        RefreshDiscountBagsRedPiont();
    }

    void RefreshData(uint id, object data)
    {
        RefreshOpenServiceBagsRedPiont();
        RefreshSpecialOfferBagsRedPiont();
        RefreshDiscountBagsRedPiont();
        SelectChildPanel((int) curTab);
    }
    
    void RefreshRedPointData(uint id, object data)
    {
        RefreshOpenServiceBagsRedPiont();
        RefreshSpecialOfferBagsRedPiont();
        RefreshDiscountBagsRedPiont();   
    }

    void DailyPurchaseBuy(uint id, object data)
    {
        if (data == null) return;
        DailyPurchaseBuyResponse msg = (DailyPurchaseBuyResponse) data;
        TABLE.GIFTBAG giftbag;
        if (GiftBagTableManager.Instance.TryGetValue(msg.giftBuyInfo.giftId, out giftbag))
        {
            rewardsDic?.Clear();
            BoxTableManager.Instance.GetBoxAwardById(giftbag.rewards, rewardsDic);
            Utility.OpenGiftPrompt(rewardsDic);
            // SelectChildPanel((int)curTab);
        }

        RefreshData(0, null);
    }

    void GetItemsDrag(uint id, object data)
    {
        if (data == null) return;
        ItemBaseDragPara eventData = (ItemBaseDragPara) data;
        if (eventData.mtype != PropItemType.Normal) return;
        vector = eventData.mvector;
        CalScrollPosDragEnd();
    }

    void OnClickLockTips(GameObject go)
    {
        UtilityTips.ShowRedTips(1835);
    }

    public override void Show()
    {
        base.Show();
        mScrollView_giftbag.ResetPosition();
        ScriptBinder.InvokeRepeating(0f, 1f, OnScheDule);
        // SelectChildPanel();
    }

    public override void SelectChildPanel(int type = 1)
    {
        listOpenServiceBags = CSDiscountGiftBagInfo.Instance.ListOpenServiceBags;
        listSpecialOfferBags = CSDiscountGiftBagInfo.Instance.ListSpecialOfferBags;
        listDiscountBags = CSDiscountGiftBagInfo.Instance.ListDiscountBags;
        if (type <= 1 && listOpenServiceBags.Count <= 0)
            type = listSpecialOfferBags.Count > 0 ? 2 : 3;

        if (type == 2 && listSpecialOfferBags.Count <= 0)
            type = listOpenServiceBags.Count > 0 ? 1 : 3;

        if (type == 3 && listDiscountBags.Count <= 0)
            type = listOpenServiceBags.Count > 0 ? 1 : 2;

        mtoggle_tab1.gameObject.SetActive(listOpenServiceBags.Count > 0);
        mgrid_btns.repositionNow = true;
        mgrid_btns.Reposition();


        msp_tab2.spriteName = listSpecialOfferBags.Count > 0 ? "btn_gift_bag2" : "btn_gift_bag1";
        mlb_tab2.color = UtilityColor.HexToColor(listSpecialOfferBags.Count > 0 ? "#3d1400" : "#2a1b13");
        msp_checkmark2.gameObject.SetActive(listSpecialOfferBags.Count > 0);
        msp_lock2.SetActive(listSpecialOfferBags.Count <= 0);
        mbtn_locktab2.transform.position = msp_tab2.transform.position;
        mbtn_locktab2.gameObject.SetActive(listSpecialOfferBags.Count <= 0);


        msp_tab3.spriteName = listDiscountBags.Count > 0 ? "btn_gift_bag2" : "btn_gift_bag1";
        mlb_tab3.color = UtilityColor.HexToColor(listDiscountBags.Count > 0 ? "#3d1400" : "#2a1b13");
        msp_checkmark3.gameObject.SetActive(listDiscountBags.Count > 0);
        msp_lock3.SetActive(listDiscountBags.Count <= 0);
        mbtn_locktab3.transform.position = msp_tab3.transform.position;
        mbtn_locktab3.gameObject.SetActive(listDiscountBags.Count <= 0);

        InitTab(type);
        InitGridWrap(type);
    }

    /// <summary>
    /// 初始化页签
    /// </summary>
    void InitTab(int type)
    {
        curTab = (DiscountGiftBagTab) type;
        switch (type)
        {
            case 1:
                curListBags = listOpenServiceBags;
                break;
            case 2:
                curListBags = listSpecialOfferBags;
                break;
            case 3:
                curListBags = listDiscountBags;
                break;
        }

        AddViewedIds(curListBags);
        CSGame.Sington.StartCoroutine(SetToggle(type));
        if (!CSDiscountGiftBagInfo.Instance.ListPositionRedpoints.Contains(type))
            Net.CSLookPositionMessage(type);
    }


    IEnumerator SetToggle(int type)
    {
        yield return null;
        switch (type)
        {
            case 1:
                mtoggle_tab1.Set(true);
                break;
            case 2:
                if (listSpecialOfferBags.Count > 0)
                    mtoggle_tab2.Set(true);
                break;
            case 3:
                if (listDiscountBags.Count > 0)
                    mtoggle_tab3.Set(true);
                break;
        }
    }

    void OnClickTab(GameObject go)
    {
        if (go == null) return;
        DiscountGiftBagTab type = (DiscountGiftBagTab) UIEventListener.Get(go).parameter;
        curTab = type;
        switch (type)
        {
            case DiscountGiftBagTab.OpenService:
                curListBags = listOpenServiceBags;
                if (!CSDiscountGiftBagInfo.Instance.ListPositionRedpoints.Contains(1))
                    Net.CSLookPositionMessage(1);
                break;
            case DiscountGiftBagTab.SpecialOffer:
                curListBags = listSpecialOfferBags;
                if (!CSDiscountGiftBagInfo.Instance.ListPositionRedpoints.Contains(2))
                    Net.CSLookPositionMessage(2);
                break;
            case DiscountGiftBagTab.Discount:
                curListBags = listDiscountBags;
                if (!CSDiscountGiftBagInfo.Instance.ListPositionRedpoints.Contains(3))
                    Net.CSLookPositionMessage(3);
                break;
        }

        AddViewedIds(curListBags);
        mScrollView_giftbag.ResetPosition();
        InitGridWrap((int) type);
    }


    /// <summary>
    /// 添加已查看新礼包
    /// </summary>
    void AddViewedIds(ILBetterList<DiscountGiftBagGroupData> list)
    {
        if (list == null || list.Count <= 0) return;
        if (viewedIds == null)
            viewedIds = new RepeatedField<int>();

        for (int i = 0; i < list.Count; i++)
        {
            DiscountGiftBagGroupData discountGiftBagGroupData = list[i];
            for (int j = 0; j < discountGiftBagGroupData.ListGiftBags.Count; j++)
            {
                DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[j];
                if (directPurchaseData.IsNew)
                    viewedIds.Add(directPurchaseData.Giftbag.id);
            }
        }
    }

    /// <summary>
    /// 刷新开服礼包红点
    /// </summary>
    void RefreshOpenServiceBagsRedPiont()
    {
        mredpoint_tab1.SetActive(CSDiscountGiftBagInfo.Instance.IsHasRedPointForPosition1());
    }

    /// <summary>
    /// 刷新开服礼包红点
    /// </summary>
    void RefreshSpecialOfferBagsRedPiont()
    {
        mredpoint_tab2.SetActive(CSDiscountGiftBagInfo.Instance.IsHasRedPointForPosition2());
    }

    /// <summary>
    /// 刷新优惠礼包红点
    /// </summary>
    void RefreshDiscountBagsRedPiont()
    {
        mredpoint_tab3.SetActive(CSDiscountGiftBagInfo.Instance.IsHasRedPointForPosition3());
    }

    #region 循环滚动相关函数

    /// <summary>
    /// 初始化无限滚动组件
    /// </summary>
    void InitGridWrap(int type)
    {
        RefreshMaxPag(type);
        curPage = 1;
        gridPage0 = 1;
        mWrapContent.enabled = false;
        if (maxPage < 2)
        {
            mWrapContent.minIndex = 0;
            mWrapContent.maxIndex = 1;
            dicBinderTime.Clear();
            RefreshOnePagesItems(0, gridPage0);
        }
        else
        {
            mWrapContent.minIndex = 0;
            mWrapContent.maxIndex = maxPage - 1;
            gridPage1 = gridPage0 + 1;
            dicBinderTime.Clear();
            RefreshOnePagesItems(0, gridPage0);
            RefreshOnePagesItems(1, gridPage1);
        }
    
        mWrapContent.cullContent = false;
        mWrapContent.SortBasedOnScrollMovement();
        mWrapContent.enabled = true;
    }

    /// <summary>
    /// 刷新最大页数
    /// </summary>
    void RefreshMaxPag(int type)
    {
        switch (type)
        {
            case 1:
                curMaxCount = listOpenServiceBags.Count;
                break;
            case 2:
                curMaxCount = listSpecialOfferBags.Count;
                break;
            case 3:
                curMaxCount = listDiscountBags.Count;
                break;
        }

        maxPage = curMaxCount == 0 ? 1 : Mathf.CeilToInt((float) curMaxCount / OnePagMaxCount);
        mScrollView_giftbag.enabled = maxPage > 1;
        msp_left_scroll.SetActive(false);
        msp_right_scroll.SetActive(maxPage > 1);
    }

    Dictionary<int, UIWelfareGiftBagBinder> dicBinderTime = new Dictionary<int, UIWelfareGiftBagBinder>();

    /// <summary>
    /// 刷新某一页grid的物品
    /// </summary>
    /// <param name="wrapIndex">0对应grid1,1对应grid2</param>
    /// <param name="page">刷新对应新生成的页数，起始页为1</param>
    void RefreshOnePagesItems(int wrapIndex, int page)
    {
        int passCount = (page - 1) * OnePagMaxCount; //当前index在此基础上往上加
        UIGridContainer gridCur = wrapIndex == 0 ? mgrid_item1 : mgrid_item2;
        gridCur.MaxCount = page < maxPage ? OnePagMaxCount : curMaxCount - OnePagMaxCount * (maxPage - 1);
        // bool isOne = false;
        // if (gridCur.MaxCount == 1)
        // {
        //     gridCur.MaxCount = 2;
        //     isOne = true;
        // }

        GameObject gp;
        for (int i = 0; i < gridCur.MaxCount; i++)
        {
            gp = gridCur.controlList[i];
            int realIndex = i + passCount;
            if (realIndex < curMaxCount && realIndex >= 0)
            {
                var eventHandle = UIEventListener.Get(gp);
                UIWelfareGiftBagBinder Binder;
                if (eventHandle.parameter == null)
                {
                    Binder = new UIWelfareGiftBagBinder();
                    Binder.Setup(eventHandle);
                    Binder.OnDrag = OnDrag;
                    Binder.OnDragEnd = OnDragEnd;
                }
                else
                {
                    Binder = eventHandle.parameter as UIWelfareGiftBagBinder;
                }

                DiscountGiftBagGroupData discountGiftBagGroupData = curListBags[realIndex];
                Binder.Bind(discountGiftBagGroupData);

                if (!dicBinderTime.ContainsKey(realIndex) && Binder.sec > 0)
                    dicBinderTime.Add(realIndex, Binder);
            }
        }
    }

    private Vector2 vector;

    void OnDrag(GameObject go, Vector2 vec)
    {
        if (vector == null)
            vector = Vector2.zero;

        vector = vector + vec;
    }

    void OnDragEnd(GameObject go)
    {
        CalScrollPosDragEnd();
    }

    void CalScrollPosDragEnd()
    {
        Vector3 vec = new Vector3(-700 * (curPage - 1), 0, 0);
        if (vector.x >= 205)
        {
            vec.x = vec.x + 700;
            curPage--;
            curPage = (curPage < 1) ? 1 : curPage;
            msp_left_scroll.SetActive(curPage > 1);
            msp_right_scroll.SetActive(curPage < maxPage);
        }
        else if( vector.x <= -205)
        {
            vec.x = vec.x - 700;
            curPage++;
            curPage = (curPage > maxPage) ? maxPage : curPage;
            msp_left_scroll.SetActive(curPage > 1);
            msp_right_scroll.SetActive(curPage < maxPage);
        }

        vec = new Vector3(-700 * (curPage - 1), 0, 0);
        mmaskPanel.SetActive(true);
        SpringPanel.Begin(mScrollView_giftbag.gameObject, vec, 40f).onFinished = ScrollFinish;
    }

    void ScrollFinish()
    {
        mmaskPanel.SetActive(false);
        vector = Vector2.zero;
        mScrollView_giftbag.restrictWithinPanel = true;
    }

    void OnUpdateItem(GameObject go, int wrapIndex, int realIndex)
    {
        // int realIndexAbs = Math.Abs(realIndex);
        // int page = realIndexAbs + 1;
        int page = lastRealIndex > realIndex ? curPage - 1 : curPage + 1;
        // page = page <= 0 ? MaxShopPage : page > MaxShopPage ? 1 : page;//循环用
        page = page <= 0 ? 1 : page > maxPage ? maxPage : page; //非循环用
        lastRealIndex = realIndex;
        if (wrapIndex == 0) gridPage0 = page;
        else gridPage0 = page;
        dicBinderTime.Clear();
        RefreshOnePagesItems(wrapIndex, page);
    }

    #endregion

    void OnScheDule()
    {
        if (dicBinderTime.Count > 0)
        {
            for (var it = dicBinderTime.GetEnumerator(); it.MoveNext();)
            {
                UIWelfareGiftBagBinder binder = it.Current.Value;
                if (binder.sec >= 0)
                {
                    binder.SetTime();
                    binder.sec--;
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        mgrid_item1.UnBind<UIWelfareGiftBagBinder>();
        mgrid_item2.UnBind<UIWelfareGiftBagBinder>();
        if (viewedIds != null && viewedIds.Count > 0)
            Net.CSLookGiftMessage(viewedIds);

        CSEffectPlayMgr.Instance.Recycle(mbanner25);
        base.OnDestroy();
    }

    public override void OnHide()
    {
        ScriptBinder.StopInvoke();
        mgrid_item1.UnBind<UIWelfareGiftBagBinder>();
        mgrid_item2.UnBind<UIWelfareGiftBagBinder>();
        base.OnHide();
    }
}