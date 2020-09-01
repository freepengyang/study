using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UISpecialShopCombinePanel : UIBasePanel
{
    private enum ShopPanelType {
        RechargeShop = 1, 
        ExchangeShop = 2,
        Recharge = 3,
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }



    public override void Init()
    {
        base.Init();

        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);
        mClientEvent.AddEvent(CEvent.FastAccessTransferNpc, CloseEvent);

        mbtn_close.onClick = CloseClick;
        RegChildPanel<UIRechargeShopPanel>((int)ShopPanelType.RechargeShop, mURechargeShopPanel, mbtn_rechargeShop);
        RegChildPanel<UIExchangeShopPanel>((int)ShopPanelType.ExchangeShop, mUIExchangeShopPanel, mbtn_exchangeShop);
        RegChildPanel<UIRechargePanel>((int)ShopPanelType.Recharge, mUIRechargePanel, mbtn_recharge);

        SetMoneyIds(1, 4);

        RegisterRed(mbtn_rechargeShop.transform.Find("redpoint").gameObject, RedPointType.RechargeShop);
        RegisterRed(mbtn_exchangeShop.transform.Find("redpoint").gameObject, RedPointType.ExchangeShop);
        RegisterRed(mbtn_recharge.transform.Find("redpoint").gameObject, RedPointType.MonthRecharge);
    }

    public override void SelectChildPanel(int type, int subType)
    {
        if (type == (int)ShopPanelType.RechargeShop)
        {
            OpenChildPanel((int)ShopPanelType.RechargeShop, false).SelectChildPanel(subType);
        }
        else
        {
            OpenChildPanel(type);
        }
    }
    
    public override void Show()
    {
        base.Show();
    }
	
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    void CloseEvent(uint id, object param)
    {
        int panelId = System.Convert.ToInt32(param);
        if (UtilityPanel.CheckGameModelPanelIsThis<UISpecialShopCombinePanel>(panelId))
        {
            return;
        }
        Close();
    }

    void CloseClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
    }
}