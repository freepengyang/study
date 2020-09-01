using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIShopCombinePanel : UIBasePanel
{
    private enum ShopPanelType { Shop = 1, Recharge }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }



    public override void Init()
	{
		base.Init();
        AddCollider();

        mbtn_close.onClick = CloseBtnClick;

        RegChildPanel<UIShopPanel>((int)ShopPanelType.Shop, mobj_shopPanel, mbtn_shop);
        RegChildPanel<UIRechargePanel>((int)ShopPanelType.Recharge, mobj_rechargePanel, mbtn_recharge);

        SetMoneyIds(1, 4);
    }


    public override void SelectChildPanel(int type, int subType)
    {
        if (type == (int)ShopPanelType.Recharge)
        {
            OpenChildPanel(type);
        }
        else
        {
            OpenChildPanel((int)ShopPanelType.Shop, false).SelectChildPanel(subType);
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

    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIShopCombinePanel>();
    }


}
