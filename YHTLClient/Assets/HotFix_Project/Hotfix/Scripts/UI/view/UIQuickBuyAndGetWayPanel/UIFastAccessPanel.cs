using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class UIFastAccessPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    private enum QuickBuyState { None, JustIcon, CanBuy }

    #region forms
    private GameObject _btn_close;
    private GameObject btn_close { get { return _btn_close ?? (_btn_close = Get("events/btn_close").gameObject); } }

    private GameObject _obj_normalView;
    private GameObject obj_normalView { get { return _obj_normalView ?? (_obj_normalView = Get("view/normal").gameObject); } }


    private GameObject _obj_normalItem;
    private GameObject obj_normalItem { get { return _obj_normalItem ?? (_obj_normalItem = Get("view/normal/item").gameObject); } }


    private UILabel _lb_normalItemName;
    private UILabel lb_normalItemName { get { return _lb_normalItemName ?? (_lb_normalItemName = Get<UILabel>("view/normal/item/lb_itemname")); } }


    private GameObject _obj_storeView;
    private GameObject obj_storeView { get { return _obj_storeView ?? (_obj_storeView = Get("view/store").gameObject); } }


    private GameObject _obj_shopItem;
    private GameObject obj_shopItem { get { return _obj_shopItem ?? (_obj_shopItem = Get("view/store/item1").gameObject); } }

    private UILabel _lb_shopItemName;
    private UILabel lb_shopItemName { get { return _lb_shopItemName ?? (_lb_shopItemName = Get<UILabel>("view/store/item1/lb_itemname")); } }

    private UILabel _lb_shopItemCost;
    private UILabel lb_shopItemCost { get { return _lb_shopItemCost ?? (_lb_shopItemCost = Get<UILabel>("view/store/item1/lb_itemcost")); } }

    private UISprite _sp_money;
    private UISprite sp_money { get { return _sp_money ?? (_sp_money = Get<UISprite>("view/store/item1/Sprite")); } }

    private GameObject _btn_buyItem;
    private GameObject btn_buyItem { get { return _btn_buyItem ?? (_btn_buyItem = Get("view/store/item1/btn_buy").gameObject); } }


    private Transform _trans_HQTJ;
    private Transform trans_HQTJ { get { return _trans_HQTJ ?? (_trans_HQTJ = Get("view/Label")); } }

    private Transform _trans_ScrollView;
    private Transform trans_ScrollView { get { return _trans_ScrollView ?? (_trans_ScrollView = Get("view/Scroll View")); } }

    private UISprite _sp_windowBg;
    private UISprite sp_windowBg { get { return _sp_windowBg ?? (_sp_windowBg = Get<UISprite>("window/Sprite")); } }


    private UIGridContainer _grid_getWayItem;
    private UIGridContainer grid_getWayItem { get { return _grid_getWayItem ?? (_grid_getWayItem = Get<UIGridContainer>("view/Scroll View/grid")); } }


    #endregion


    private TABLE.ITEM ItemCfg;
    private TABLE.SHOP ShopCfg;
    private UIItemBase ItemBase;

    ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();


    public override void Init()
    {
        base.Init();
        AddCollider();

        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);

        UIEventListener.Get(btn_close).onClick = CloseBtnClick;
        UIEventListener.Get(btn_buyItem).onClick = QuickBuyBtnClick;
    }


    /// <summary>
    /// 通过给定物品Id来刷新界面
    /// </summary>
    /// <param name="itemId"></param>
    public void RefreshUI(int itemId)
    {
        QuickBuyState quickBuyState = QuickBuyState.None;

        if (!ItemTableManager.Instance.TryGetValue((int)itemId, out ItemCfg)) return;

        if (string.IsNullOrEmpty(ItemCfg.icon))
        {
            quickBuyState = QuickBuyState.None;
        }
        else
        {
            Func<TABLE.SHOP, bool> isItemCanBuy = delegate (TABLE.SHOP x)
            {
                if (x.itemId == itemId && (x.npcId == 2 || x.npcId == 1))
                {
                    if (CSMainPlayerInfo.Instance.Level >= x.showLevel && (x.maxLevel == 0 || CSMainPlayerInfo.Instance.Level <= x.maxLevel)) return true;

                    return false;
                }
                return false;
            };
            
            quickBuyState = QuickBuyState.JustIcon;
            var arr = ShopTableManager.Instance.array.gItem.handles;
            for (int k = 0, max = arr.Length; k < max; ++k)
            {
                var x = arr[k].Value as TABLE.SHOP;
                if (isItemCanBuy(x))
                {
                    quickBuyState = QuickBuyState.CanBuy;
                    ShopCfg = x;
                    break;
                }
            }

        }


        obj_normalView.SetActive(quickBuyState == QuickBuyState.JustIcon);
        obj_storeView.SetActive(quickBuyState == QuickBuyState.CanBuy);

        if (quickBuyState == QuickBuyState.JustIcon) RefreshNormalView();
        else if (quickBuyState == QuickBuyState.CanBuy) RefreshShopView();

        GetGetWaysInfo(itemId);
        ResetPosition(quickBuyState);
    }

    /// <summary>
    /// 通过给定GetWayId字符串来刷新界面
    /// </summary>
    /// <param name="getWayStr">以#连接的GetWayId字符串</param>
    public void RefreshUI(string getWayStr)
    {
        obj_normalView.SetActive(false);
        obj_storeView.SetActive(false);
        GetGetWaysInfo(getWayStr);
        ResetPosition(QuickBuyState.None);
    }

    /// <summary>
    /// 给定GetWay的列表打开获取途径面板
    /// </summary>
    /// <param name="listGetWay">GetWay列表</param>
    public void RefreshUI(List<int> listGetWay)
    {
        if (listGetWay == null) return;
        obj_normalView.SetActive(false);
        obj_storeView.SetActive(false);
        GetGetWaysInfo(listGetWay);
        ResetPosition(QuickBuyState.None);
    }



    void RefreshNormalView()
    {
        if (ItemCfg == null) return;
        lb_normalItemName.text = ItemCfg.name;
        lb_normalItemName.color = UtilityCsColor.Instance.GetColor((int)ItemCfg.quality);

        ItemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, obj_normalItem.transform);
        ItemBase.Refresh(ItemCfg, ItemClick);
    }

    void RefreshShopView()
    {
        if (ItemCfg == null || ShopCfg == null) return;
        lb_shopItemName.text = ItemCfg.name;
        lb_shopItemName.color = UtilityCsColor.Instance.GetColor((int)ItemCfg.quality);

        lb_shopItemCost.text = ShopCfg.value.ToString();

        TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg((int)ShopCfg.payType);
        sp_money.spriteName = $"tubiao{costCfg.icon}";

        ItemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, obj_shopItem.transform);
        ItemBase.Refresh(ItemCfg, ItemClick);
    }

    void ResetPosition(QuickBuyState quickBuyState)
    {
        switch (quickBuyState)
        {
            case QuickBuyState.None:
                trans_HQTJ.localPosition = new Vector3(0, 65, 0);
                trans_ScrollView.localPosition = new Vector3(-292, 3, 0);
                sp_windowBg.height = 300;
                break;
            case QuickBuyState.JustIcon:
                trans_HQTJ.localPosition = new Vector3(0, -115, 0);
                trans_ScrollView.localPosition = new Vector3(-292, -177, 0);
                sp_windowBg.height = 480;
                break;
            case QuickBuyState.CanBuy:
                trans_HQTJ.localPosition = new Vector3(0, -35, 0);
                trans_ScrollView.localPosition = new Vector3(-292, -97, 0);
                sp_windowBg.height = 400;
                break;
            default:
                trans_HQTJ.localPosition = new Vector3(0, 65, 0);
                trans_ScrollView.localPosition = new Vector3(-292, 3, 0);
                sp_windowBg.height = 300;
                break;
        }

        if (grid_getWayItem.MaxCount < 3.5)
        {
            sp_windowBg.height -= (int)((3.5 - grid_getWayItem.MaxCount) * grid_getWayItem.CellHeight);
        }
    }

    /// <summary>
    /// 通过Item的id获取GetWay列表
    /// </summary>
    /// <param name="itemId">Item的Id</param>
    /// <returns></returns>
    void GetGetWaysInfo(int itemId)
    {
        TABLE.ITEM item;
        if (ItemTableManager.Instance.TryGetValue(itemId, out item))
        {
            string getWayStr = item.getWay;
            GetGetWaysInfo(getWayStr);
        }
    }

    /// <summary>
    /// 通过GetWay的id字符串获取GetWay列表
    /// </summary>
    /// <param name="idStr">GetWay的id字符串，通过#连接</param>
    /// <returns></returns>
    void GetGetWaysInfo(string idStr)
    {
        if (getWayList == null) getWayList = new ILBetterList<GetWayData>();
        else getWayList.Clear();

        CSGetWayInfo.Instance.GetGetWays(idStr, ref getWayList);
        grid_getWayItem.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
    }

    void GetGetWaysInfo(List<int> listId)
    {
        if (listId == null) return;

        if (getWayList == null) getWayList = new ILBetterList<GetWayData>();
        else getWayList.Clear();

        CSGetWayInfo.Instance.GetGetWays(listId, ref getWayList);
        grid_getWayItem.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
    }


    protected override void OnDestroy()
    {
        grid_getWayItem.UnBind<GetWayBtn>();
        if (ItemBase != null)
        {
            UIItemManager.Instance.RecycleSingleItem(ItemBase);
        }

        getWayList?.Clear();
        getWayList = null;
        base.OnDestroy();
    }


    void CloseEvent(uint id, object data)
    {
        Close();
    }


    #region btnClick
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIFastAccessPanel>();
    }

    void QuickBuyBtnClick(GameObject _go)
    {
        if (ItemCfg == null) return;
        if (ShopCfg.npcId != 1 && ShopCfg.npcId != 2) return;

        UIManager.Instance.ClosePanel<UIFastAccessPanel>();
        UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
        {
            //(f as UIBuyConfirmPanel).OpenPanel(ItemCfg.id, ShopCfg, () => {
            //    Utility.ShowGetWay(ItemCfg.id);
            //});
            (f as UIBuyConfirmPanel).OpenPanel(ItemCfg, ShopCfg);
        });
    }

    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    #endregion

}



public class GetWayBtn : UIBinder
{
    private UILabel _name;
    private GameObject _arrow;
    private GameObject _arrowLabel;
    private GameObject _flag;

    TABLE.GETWAY getWay;
    GetWayData mData;

    public override void Init(UIEventListener handle)
    {
        _name = Get<UILabel>("Label");
        _arrow = Get<GameObject>("btn_go");
        _arrowLabel = Get<GameObject>("Label2");
        _flag = Get<GameObject>("flag");
    }

    public override void Bind(object data)
    {
        mData = data as GetWayData;
        if (mData == null || mData.Config == null) return;
        getWay = mData.Config;
        if (_name != null) _name.text = getWay.name;

        bool canTransfer = false;
        if (Handle != null)
        {
            switch (getWay.type)
            {
                case 0:
                    UIEventListener.Get(Handle.gameObject, getWay.Tips).onClick = CannotTransferCallback;
                    break;
                case 1:
                case 4:
                case 5:
                    canTransfer = true;
                    UIEventListener.Get(Handle.gameObject, getWay).onClick = OpenPanelCallback;
                    break;
                case 2:
                    canTransfer = true;
                    UIEventListener.Get(Handle.gameObject, getWay).onClick = TransferNpcCallback;
                    break;
            }
        }

        _arrow?.CustomActive(canTransfer);
        _arrowLabel?.CustomActive(canTransfer);
        _flag?.CustomActive(getWay.recommend == 1);
    }

    public override void OnDestroy()
    {
        _name = null;
        _arrow = null;
        _arrowLabel = null;
        _flag = null;
        getWay = null;
        mData = null;
    }


    public void RefreshUI(TABLE.GETWAY getWay)
    {
        if (getWay == null) return;

    }


    void CannotTransferCallback(GameObject obj)
    {
        string tips = (string)UIEventListener.Get(obj).parameter;
        if (!string.IsNullOrEmpty(tips))
            UtilityTips.ShowRedTips(tips);
    }


    public virtual void OpenPanelCallback(GameObject obj)
    {
        TABLE.GETWAY getWay = (TABLE.GETWAY)UIEventListener.Get(obj).parameter;
        int funcId = (int)getWay.function;
        //if (tb.server == 1 &&CSAvatarManager.MainPlayerInfo.ServerType == ServerType.SharedService)
        //{
        //    Utility.ShowRedTips(105119);
        //}

        if (funcId != 0)
        {
            if (funcId == 27101)
            {
                if (CSPetLevelUpInfo.Instance.JudgeOpenPetLevelUpPanel())
                {
                    HotManager.Instance.EventHandler.SendEvent(CEvent.FastAccessJumpToPanel, funcId);
                }
                return;
            }

            if (UtilityPanel.JumpToPanel(funcId))
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.FastAccessJumpToPanel, funcId);
                //UIManager.Instance.ClosePanel<UIFastAccessPanel>();
            }
        }


    }


    void TransferNpcCallback(GameObject obj)
    {
        TABLE.GETWAY getWay = (TABLE.GETWAY)UIEventListener.Get(obj).parameter;
        int funcId = (int)getWay.function;
        if (funcId != 0)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.FastAccessTransferNpc);
            UtilityPath.FindWithDeliverId(funcId);
            //UIManager.Instance.ClosePanel<UIFastAccessPanel>();
        }
    }


}