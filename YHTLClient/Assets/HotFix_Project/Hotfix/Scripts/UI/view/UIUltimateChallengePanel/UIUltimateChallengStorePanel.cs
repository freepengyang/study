using System;
using Google.Protobuf.Collections;
using shop;
using ultimate;
using UnityEngine;

public partial class UIUltimateChallengStorePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    private const int MAX_COUNT_PAGE = 6;

    private ShopInfoResponse _additionAttrs;
    private FixedCircleArray<UltimateStore> _GainEffectList;

    private int CurShopPage = 1;
    private int MaxShopPage = 1;
    private float DragOffsetX;

    public override void Init()
    {
        base.Init();
        AddCollider();
        
        CSEffectPlayMgr.Instance.ShowUITexture(mbg, "extremity_challenge_store");

        UIEventListener.Get(mscrollViewDrag).onDrag = OnDrag;
        UIEventListener.Get(mscrollViewDrag).onDragEnd = OnDragEnded;
        mbtn_close.onClick = Close;
        
        mClientEvent.Reg((uint)CEvent.ShopBuyTimesChange, OnBuyTimesChange);
        mClientEvent.AddEvent(CEvent.UpdateShopMessage, UpdateShopMessage);
        mClientEvent.AddEvent(CEvent.Scene_ChangeMap, UpdateMap);
        Net.CSShopInfoMessage(3, 1);
        _GainEffectList = new FixedCircleArray<UltimateStore>(MAX_COUNT_PAGE);

        SetMoneyIds(1, 4);
    }
    
    private void Refresh()
    {
        MaxShopPage = Mathf.CeilToInt(_additionAttrs.shopIds.Count / (float) MAX_COUNT_PAGE); //当前标签页下商品总页数
        RefreshCenterShopItems(1);
    }

    void RefreshCenterShopItems(int page = 1)
    {
        CurShopPage = page;
        int passCount = (page - 1) * MAX_COUNT_PAGE; //小于当前页的物品个数
        int leftCount = _additionAttrs.shopIds.Count - passCount;//大于等于当前页物品个数      
        mGrid.MaxCount = leftCount <= MAX_COUNT_PAGE ? leftCount : MAX_COUNT_PAGE;
        for (int i = 0; i < mGrid.MaxCount; i++)
        {
            int shopCfg = _additionAttrs.shopIds[i + passCount];
            int buyCount = 0;
            for (var i1 = 0; i1 < _additionAttrs.shopItemBuyInfos.Count; i1++)
            {
                if (_additionAttrs.shopItemBuyInfos[i1].shopId == shopCfg)
                {
                    buyCount = _additionAttrs.shopItemBuyInfos[i1].buyTimes;
                    break;
                }
            }
            UltimateStore gainEffect = _GainEffectList[i];
            SetUIShopItem(mGrid.controlList[i], shopCfg, gainEffect, buyCount);
        }
    }
    
    private void SetUIShopItem(GameObject go, int shopId, UltimateStore gainEffect, int buyCount)
    {
        if (gainEffect == null) gainEffect = new UltimateStore();
        gainEffect.gameObject = go;
        gainEffect.RefreshUI(go, shopId, buyCount);
        
        //策划说不滑动，，后面要滑动时，打开此注释
        //gainEffect.SetUIDragAction(null, OnDrag, OnDragEnded);
    }

    void OnDrag(GameObject _go, Vector2 offSet)
    {
        DragOffsetX += offSet.x;
    }

    void OnDragEnded(GameObject _go)
    {
        int page = CurShopPage;
        if (MaxShopPage > 1)
        {
            if (DragOffsetX > 0)
            {
                page = CurShopPage == 1 ? MaxShopPage : CurShopPage - 1;
            }
            else if (DragOffsetX < 0)
            {
                page = CurShopPage == MaxShopPage ? 1 : CurShopPage + 1;
            }

            RefreshCenterShopItems(page);
        }

        DragOffsetX = 0f;
    }

    private void UpdateShopMessage(uint id , object data)
    {
        if(data == null) return;
        _additionAttrs = data as ShopInfoResponse;
        CSUltimateInfo.Instance._ShopInfoResponse = _additionAttrs;
        if(_additionAttrs == null) return;
        
        Refresh();
    }
    private void UpdateMap(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIUltimateChallengStorePanel>();
    }
    
    void OnBuyTimesChange(uint id, object data)
    {
        //RefreshCenterShopItems(CurShopPage);
        Net.CSShopInfoMessage(3, 1);
    }

    protected override void OnDestroy()
    {
        if (_GainEffectList != null)
        {
            for (var i = 0; i < _GainEffectList.Count; i++)
            {
                _GainEffectList[i].Dispose();
            }

            _GainEffectList.Clear();
        }

        _additionAttrs = null;
        CSEffectPlayMgr.Instance.Recycle(mbg);
        _GainEffectList = null;
        base.OnDestroy();
    }
}


public class UltimateStore : GridContainerBase
{
    private Transform _trans_itemBase;

    private Transform trans_itemBase
    {
        get { return _trans_itemBase ? _trans_itemBase : (_trans_itemBase = Get("itemTrans")); }
    }

    private UILabel _lb_name;

    private UILabel lb_name
    {
        get { return _lb_name ? _lb_name : (_lb_name = Get<UILabel>("lb_name")); }
    }

    private GameObject _obj_flag;

    private GameObject obj_flag
    {
        get { return _obj_flag ? _obj_flag : (_obj_flag = Get<GameObject>("flag")); }
    }

    private UISprite _sp_flag;

    private UISprite sp_flag
    {
        get { return _sp_flag ? _sp_flag : (_sp_flag = Get<UISprite>("flag")); }
    }

    private UILabel _lb_money;

    private UILabel lb_money
    {
        get { return _lb_money ? _lb_money : (_lb_money = Get<UILabel>("lb_money")); }
    }

    private UISprite _sp_money;

    private UISprite sp_money
    {
        get { return _sp_money ? _sp_money : (_sp_money = Get<UISprite>("sp_money")); }
    }

    private UILabel _lb_limit;

    private UILabel lb_limit
    {
        get { return _lb_limit ? _lb_limit : (_lb_limit = Get<UILabel>("lb_count")); }
    }

    private UISprite _sp_bg;

    private UISprite sp_bg
    {
        get { return _sp_bg ? _sp_bg : (_sp_bg = Get<UISprite>("bg")); }
    }
    
    private UIItemBase itemBase;

    public int _shopId;
    private TABLE.SHOP _shopTab;
    private int BuyTimesLimit;
    private int BuyTimes;

    private readonly string[] flagIcon = {"title5", "title3", "title2", "title4", "title1"};

    public void RefreshUI(GameObject go, int shopId, int buyCount)
    {
        _shopId = shopId;
        BuyTimes = buyCount;
        if(!ShopTableManager.Instance.TryGetValue(shopId, out _shopTab)) return;
        int itemId = _shopTab.itemId2 == 0 ? _shopTab.itemId : _shopTab.itemId2;
        TABLE.ITEM itemCfg = ItemTableManager.Instance.GetItemCfg(itemId);
        if (itemCfg == null) return;
        itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, trans_itemBase);
        itemBase.Refresh(itemCfg);
        itemBase.SetCount(_shopTab.num);

        lb_name.text = itemCfg.name;
        lb_name.color = UtilityCsColor.Instance.GetColor((int) itemCfg.quality);

        BuyTimesLimit = GetLimitCount(_shopTab.frequency);

        string orgCost = _shopTab.showMoney > 0 ? _shopTab.showMoney.ToString() : "";
        string curCost = _shopTab.value.ToString();
        lb_money.text = string.Format("[s]{0}[/s] {1}", orgCost, curCost);
        TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg((int) _shopTab.payType);
        sp_money.spriteName = $"tubiao{costCfg.icon}";

        if (BuyTimesLimit > 0)
        {
            lb_limit.gameObject.SetActive(true);   
            lb_limit.text = (BuyTimesLimit - BuyTimes).ToString();
        }
        else
        {
            lb_limit.gameObject.SetActive(false);   
        }
        
        if(BuyTimesLimit != 0 && BuyTimesLimit - BuyTimes == 0)
            sp_bg.color = Color.black;
        else
            sp_bg.color = Color.white;

        SetIconFlag();


        //mClientEvent.Reg((uint)CEvent.ShopBuyTimesChange, OnBuyTimesChange);
        UIEventListener.Get(gameObject).onClick = QuickBuyBtnClick;
    }


    int GetLimitCount(string limitStr)
    {
        if (!string.IsNullOrEmpty(limitStr) && limitStr != "0")
        {
            string[] str1 = limitStr.Split('#');
            if (str1.Length == 1) return int.Parse(str1[0]);
            if (str1.Length > 1) return int.Parse(str1[1]);
        }
        return 0;
    }


    void SetIconFlag()
    {
        /*int flag = 0;
        obj_flag.SetActive(false);
        if (string.IsNullOrEmpty(shopCfg.Recommend)) return;
        if (!int.TryParse(shopCfg.Recommend, out flag)) return;
        if (flag < 1 || flag >= flagIcon.Length) return;

        obj_flag.SetActive(true);
        sp_flag.spriteName = flagIcon[flag - 1];*/
    }


    public void SetUIDragAction(Action<GameObject> onDragStart, Action<GameObject, Vector2> onDrag,
        Action<GameObject> onDragEnd)
    {
        if (onDragStart != null) UIEventListener.Get(gameObject).onDragStart = onDragStart;
        if (onDrag != null) UIEventListener.Get(gameObject).onDrag = onDrag;
        if (onDragEnd != null) UIEventListener.Get(gameObject).onDragEnd = onDragEnd;
    }

    void QuickBuyBtnClick(GameObject _go)
    {
        if (itemBase.itemCfg == null) return;

        UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
        {
            (f as UIBuyConfirmPanel).OpenPanel(itemBase.itemCfg.id, _shopTab, BuyTimesLimit, BuyTimes);
        });
    }

    public override void Dispose()
    {
        if (itemBase != null) UIItemManager.Instance.RecycleSingleItem(itemBase);
        itemBase = null;
        _shopTab = null;
        _trans_itemBase = null;
        _lb_name = null;
        _obj_flag = null;
        _sp_flag = null;
        _lb_money = null;
        _sp_money = null;
        _lb_limit = null;
    }
}