using System.Collections.Generic;
using dailypurchase;
using Google.Protobuf.Collections;
using UnityEngine;

public partial class UIGiftBagPreviewPanel : UIBasePanel
{
	private DiscountGiftBagGroupData discountGiftBagGroupData;
	
	Dictionary<int,int> rewardsDic = new Dictionary<int, int>();
	
	RepeatedField<int> viewedIds;
	
	public override void Init()
	{
		base.Init();
		mClientEvent.Reg((uint) CEvent.DailyPurchaseInfo, RefreshData);
		mClientEvent.Reg((uint) CEvent.DailyPurchaseBuyDiscount, DailyPurchaseBuy);
		mClientEvent.Reg((uint) CEvent.GiftBagOpen, RefreshData);
		mClientEvent.Reg((uint) CEvent.GiftBagClose, RefreshData);
		// mClientEvent.Reg((uint) CEvent.LookGift, RefreshData);
		AddCollider();
		mbtn_close.onClick = Close;
	}

	void RefreshData(uint id, object data)
	{
		InitData();
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
			InitData();
		}
	}

	public override void Show()
	{
		base.Show();
	}

	public void OpenGiftBagPreviewPanel(DiscountGiftBagGroupData data)
	{
		if (data == null) return;
		discountGiftBagGroupData = data;
		InitData();
	}

	void InitData()
	{
		mlb_name.text = discountGiftBagGroupData.GiftBagData.Giftbag.groupName;
		RefreshGrid();
	}

	void RefreshGrid()
	{
		int listGiftBagsCount = discountGiftBagGroupData.ListGiftBags.Count;
		mgrid_gift.MaxCount = Mathf.Min(3,listGiftBagsCount);

		if (viewedIds==null)
			viewedIds = new RepeatedField<int>();
		viewedIds.Clear();
		for (int i = 0; i < listGiftBagsCount; i++)
		{
			if (i<3)
			{
				DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[i];
				if (directPurchaseData.IsNew)
					viewedIds.Add(directPurchaseData.Giftbag.id);
			}
		}
		GameObject gp;
		for (int i = 0; i < mgrid_gift.MaxCount; i++)
		{
			gp = mgrid_gift.controlList[i];
			var eventHandle = UIEventListener.Get(gp);
			UIGiftBagPreviewBinder Binder;
			if (eventHandle.parameter == null)
			{
				Binder = new UIGiftBagPreviewBinder();
				Binder.Setup(eventHandle);
			}
			else
			{
				Binder = eventHandle.parameter as UIGiftBagPreviewBinder;
			}

			Binder.viewedIds = viewedIds;
			DirectPurchaseData directPurchaseData = discountGiftBagGroupData.ListGiftBags[i];
			Binder.Bind(directPurchaseData);
		}
	}

	protected override void OnDestroy()
	{
		mgrid_gift.UnBind<UIGiftBagPreviewBinder>();
		base.OnDestroy();
	}
}
