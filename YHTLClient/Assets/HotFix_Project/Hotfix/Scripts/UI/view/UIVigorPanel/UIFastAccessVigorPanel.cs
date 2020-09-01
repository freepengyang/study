using UnityEngine;
public partial class UIFastAccessVigorPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    UIItemBase item;
    int ItemId = 12;
    int exNum = 0;
    int maxCount = 0;
    TABLE.ENERGYEXCHANGE costCfg;
    ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();

    public override void Init()
    {
        base.Init();
        AddCollider();
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, mtrans_itemPar, itemSize.Default);
        mClientEvent.AddEvent(CEvent.SCEnergyExchangeInfoMessage, SCEnergyExchangeInfoMessage);
        mClientEvent.AddEvent(CEvent.MoneyChange, GetMoneyChange);
        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, FastAccessJumpToPanel);
        item.Refresh(ItemId);
        UIEventListener.Get(mbtn_exchange).onClick = GetBtnClick;
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_eff, 17904);
        mobj_eff.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        Refresh();
        ShowGetWay();
        mlb_name.text = ItemTableManager.Instance.GetItemName(ItemId);
    }

    protected override void OnDestroy()
    {
        mgrid_getWayItem.UnBind<GetWayBtn>();
        if (item != null)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
        base.OnDestroy();
    }
    void FastAccessJumpToPanel(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIFastAccessVigorPanel>();
    }
    void SCEnergyExchangeInfoMessage(uint id, object data)
    {
        Refresh();
    }
    void GetMoneyChange(uint id, object data)
    {
        Refresh();
    }
    void ShowGetWay()
    {
        getWayList.Clear();
        CSGetWayInfo.Instance.GetGetWays(ItemTableManager.Instance.GetItemGetWay(ItemId), ref getWayList);
        mgrid_getWayItem.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
    }
    void Refresh()
    {
        exNum = CSVigorInfo.Instance.GetDoneChangeCount();
        maxCount = CSVigorInfo.Instance.GetTodayTotalCount();
        mlb_exchangeCount.text = $"{exNum}/{maxCount}";
        mlb_exchangeCount.color = (exNum < maxCount) ? CSColor.green : CSColor.red;
        mobj_eff.SetActive((exNum < maxCount) ? true : false);
        costCfg = EnergyExchangeTableManager.Instance.GetSingleCfgByTimes(exNum + 1);
        if (costCfg != null)
        {
            int cfgId = (costCfg.costType1 == 0) ? costCfg.costType2 : costCfg.costType1;
            int num = (costCfg.costType1 == 0) ? costCfg.costNum2 : costCfg.costNum1;
            msp_icon.spriteName = string.Concat("tubiao", ItemTableManager.Instance.GetItemIcon(cfgId));
            mlb_costNum.text = num.ToString();
            mlb_costNum.color = (num <= CSItemCountManager.Instance.GetItemCount(cfgId)) ? CSColor.green : CSColor.red;
            item.SetCount(costCfg.energy);
        }
    }

    void GetBtnClick(GameObject go)
    {
        int cfgId = (costCfg.costType1 == 0) ? costCfg.costType2 : costCfg.costType1;
        int num = (costCfg.costType1 == 0) ? costCfg.costNum2 : costCfg.costNum1;
        if (cfgId == 3 && num > CSItemCountManager.Instance.GetItemCount(cfgId))
        {
            Utility.ShowGetWay(cfgId);
            return;
        }
        Net.CSExchageEnergyMessage();
    }
    void CloseBtnClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIFastAccessVigorPanel>();
    }
}
