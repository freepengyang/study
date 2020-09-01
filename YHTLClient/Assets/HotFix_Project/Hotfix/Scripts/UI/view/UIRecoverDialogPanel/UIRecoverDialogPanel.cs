using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIRecoverDialogPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.NpcDialog;
    }


    int needYuanBao;

	public override void Init()
	{
		base.Init();
        AddCollider();

        mbtn_icon.onClick = ItemTipsClick;
        mbtn_add.onClick = GetWayClick;
        mbtn_recover.onClick = RecoverClick;

        string str = SundryTableManager.Instance.GetSundryEffect(1139);
        int.TryParse(str, out needYuanBao);

        mClientEvent.AddEvent(CEvent.MoneyChange, RefreshMoney);
    }
	
	public override void Show()
	{
		base.Show();

        RefreshMoney(0, null);
    }
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}


    void RefreshMoney(uint id, object data)
    {
        var curCount = ((int)MoneyType.yuanbao).GetItemCount();
        mlb_value.text = $"{needYuanBao}".BBCode(curCount >= needYuanBao ? ColorType.Green : ColorType.Red);
    }


    void ItemTipsClick(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, 3);
    }


    void GetWayClick(GameObject go)
    {
        Utility.ShowGetWay(3);
    }

    void RecoverClick(GameObject go)
    {
        if (CSMainPlayerInfo.Instance.HP >= CSMainPlayerInfo.Instance.MaxHP 
            && CSMainPlayerInfo.Instance.MP >= CSMainPlayerInfo.Instance.MaxMP)
        {
            UtilityTips.ShowTips(2047);
            Close();
            return;
        }

        if (((int)MoneyType.yuanbao).GetItemCount() < needYuanBao)
        {
            GetWayClick(null);
            return;
        }

        Net.CSRecoverHpMessage();

        Close();
    }
}
