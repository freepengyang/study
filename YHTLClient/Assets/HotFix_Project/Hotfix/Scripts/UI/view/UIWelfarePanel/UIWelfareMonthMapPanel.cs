using System;
using System.Collections.Generic;
using UnityEngine;


public partial class UIWelfareMonthMapPanel : UIBasePanel
{
    int ticketAId;
    int ticketBId;

    int needACount;
    int needBCount;


    public override void Init()
	{
		base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mbanner24, "banner24");

        mClientEvent.AddEvent(CEvent.ItemListChange, RefreshTicketCount);

        UIEventListener.Get(mbtn_brave_not_purchased).onClick = BuyCardClick;
        UIEventListener.Get(mbtn_king_not_purchased).onClick = BuyCardClick;
        mbtn_addTicketA.onClick = AddTicketAClick;
        mbtn_addTicketB.onClick = AddTicketBClick;
        UIEventListener.Get(mbtn_brave_purchased, 1).onClick = EnterMapClick;
        UIEventListener.Get(mbtn_king_purchased, 2).onClick = EnterMapClick;
        UIEventListener.Get(msp_ticketA.gameObject, 1).onClick = TicketClick;
        UIEventListener.Get(msp_ticketB.gameObject, 2).onClick = TicketClick;

        ticketAId = CSMonthCardInfo.Instance.ticketAId;
        ticketBId = CSMonthCardInfo.Instance.ticketBId;

        needACount = CSMonthCardInfo.Instance.needACount;
        needBCount = CSMonthCardInfo.Instance.needBCount;

        msp_ticketA.spriteName = $"tubiao{ticketAId}";
        msp_ticketB.spriteName = $"tubiao{ticketBId}";

        mlb_tips.text = ClientTipsTableManager.Instance.GetClientTipsContext(1860);
    }
	
	public override void Show()
	{
		base.Show();

        CSMonthCardInfo.Instance.OpenCardMapPanel();

        RefreshUI();

    }
	
	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(mbanner24);

        base.OnDestroy();
	}

    public override void OnHide()
    {
        base.OnHide();
    }

    void RefreshUI()
    {
        bool hasA = CSMonthCardInfo.Instance.HasMonthCard(1);
        bool hasB = CSMonthCardInfo.Instance.HasMonthCard(2);
        mobj_hasA.CustomActive(hasA);
        mobj_hasB.CustomActive(hasB);
        mobj_notHasA.CustomActive(!hasA);
        mobj_notHasB.CustomActive(!hasB);

        RefreshTicketCount(0, null);
    }


    void RefreshTicketCount(uint id, object param)
    {
        var countA = ticketAId.GetItemCount();
        var countB = ticketBId.GetItemCount();
        mlb_valueA.text = $"{countA}/{needACount}".BBCode(countA >= needACount ? ColorType.Green : ColorType.Red);
        mlb_valueB.text = $"{countB}/{needBCount}".BBCode(countB >= needBCount ? ColorType.Green : ColorType.Red);
    }


    void BuyCardClick(GameObject go)
    {
        UtilityPanel.JumpToPanel(12600);
    }


    void AddTicketAClick(GameObject go)
    {
        Utility.ShowGetWay(ticketAId);
    }

    void AddTicketBClick(GameObject go)
    {
        Utility.ShowGetWay(ticketBId);
    }


    void EnterMapClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);

        if (param == 1)
        {
            if(ticketAId.GetItemCount() < needACount)
            {
                AddTicketAClick(go);
            }
            else
            {
                UtilityPath.FindWithDeliverId(111);
                UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
            }
        }
        else if (param == 2)
        {
            if (ticketBId.GetItemCount() < needBCount)
            {
                AddTicketBClick(go);
            }
            else
            {
                UtilityPath.FindWithDeliverId(112);
                UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
            }
        }
    }


    void TicketClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        int id = param == 1 ? ticketAId : ticketBId;
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, id);
    }
}
