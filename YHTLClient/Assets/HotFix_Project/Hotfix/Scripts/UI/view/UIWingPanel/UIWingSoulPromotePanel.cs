using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;

public partial class UIWingSoulPromotePanel : UIBasePanel
{
	private UIItemBase itemBaseA;
	private UIItemBase itemBaseB;
	private int curPosition = 0;
	private ILBetterList<int> listCost = new ILBetterList<int>();
	// private Dictionary<int, int> dicCost = new Dictionary<int, int>();
	
	RepeatedField<int> ids = new RepeatedField<int>();
	RepeatedField<int> values = new RepeatedField<int>();

	public override void Init()
	{
		base.Init();
		mClientEvent.Reg((uint) CEvent.YuLingInfo, RefreshData);
		mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshData);
		AddCollider();
		mbtn_close.onClick = Close;
		mbtn_upgrade.onClick = OnClickUpGrade;
		mbtn_no.onClick = OnClickNo;
	}
	
	void RefreshData(uint id, object data)
	{
		OpenWingSoulPromotePanel(curPosition);
	}
	
	public override void Show()
	{
		base.Show();
		CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17751);
	}

	public void OpenWingSoulPromotePanel(int position)
	{
		curPosition = position;
		itemBaseA = UIItemManager.Instance.GetItem(PropItemType.Normal, mitem72A.transform, itemSize.Size72);
		int itemAId = CSWingInfo.Instance.DicSlotData[position].yuLingSoulId;
		itemBaseA.Refresh(itemAId);
		itemBaseB = UIItemManager.Instance.GetItem(PropItemType.Normal, mitem72B.transform, itemSize.Size72);
		int itemBId = CSWingInfo.Instance.DicSlotNextId[position];
		itemBaseB.Refresh(itemBId);
		mlb_nameA.text = YuLingSoulTableManager.Instance.GetYuLingSoulName(itemAId);
		mlb_nameA.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemAId));
		mlb_nameB.text = YuLingSoulTableManager.Instance.GetYuLingSoulName(itemBId);
		mlb_nameB.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemBId));
		
		listCost.Clear();
		// dicCost.Clear();
		ids.Clear();
		values.Clear();
		LongArray allCost = CSWingInfo.Instance.DicSlotData[position].YulingsoulCfg.cost;
		for (int i = 0 , max = allCost.Count; i < max; i++)
		{
			var cost = allCost[i];
			int key = cost.key();
			int value = cost.value();
			// dicCost.Add(key, value);
			if (i<max-1)
			{
				ids.Add(key);
				values.Add(value);
			}
			for (int j = 0; j < value; j++)
			{
				listCost.Add(key);
			}
		}

		mgrid_cost.MaxCount = listCost.Count-1;
		GameObject gp;
		for (int i = 0, max = mgrid_cost.MaxCount; i < max; i++)
		{
			gp = mgrid_cost.controlList[i];
			var eventHandle = UIEventListener.Get(gp);
			UIWingSoulPromoteBinder Binder;
			if (eventHandle.parameter == null)
			{
				Binder = new UIWingSoulPromoteBinder();
				Binder.Setup(eventHandle);
			}
			else
			{
				Binder = eventHandle.parameter as UIWingSoulPromoteBinder;
			}
			
			Binder.Bind(listCost[i]);
		}
		
		string name = String.Empty; 
		switch (position)
		{
			case 1:
				name = CSString.Format(1936);
				break;
			case 2:
				name = CSString.Format(1937);
				break;
			case 3:
				name = CSString.Format(1938);
				break;
		}

		if (allCost.Count>0)
		{
			long curCount = CSItemCountManager.Instance.GetItemCount(allCost[0].key());
			int costCount = allCost[0].value()-1;
			string a = $"{CSString.Format(1940)}{name}".BBCode(ColorType.SecondaryText);
			string b = $"({curCount}/{costCount})".BBCode(curCount >= costCount ? ColorType.Green : ColorType.Red);
			mlb_title.text = $"{a}{b}";
		}
		
		bool isEnough = ItemHelper.IsItemsEnough(ids, values);
		mlb_no.SetActive(!isEnough);
		mbtn_upgrade.gameObject.SetActive(isEnough);
	}

	void OnClickUpGrade(GameObject go)
	{
		Net.CSYuLingSoulUpgradeMessage(curPosition);
		Close();//点提升则关闭提升界面
	}

	void OnClickNo(GameObject go)
	{
		if (listCost!=null && listCost.Count>0)
			Utility.ShowGetWay(listCost[0]);
	}

	protected override void OnDestroy()
	{
		mgrid_cost.UnBind<UIWingSoulPromoteBinder>();
		UIItemManager.Instance.RecycleSingleItem(itemBaseA);
		UIItemManager.Instance.RecycleSingleItem(itemBaseB);
		CSEffectPlayMgr.Instance.Recycle(meffect);
		base.OnDestroy();
	}
}

public class UIWingSoulPromoteBinder : UIBinder
{
	private GameObject item72;
	private UILabel lb_name;
	private UIItemBase itemBase;
	private int itemId = 0;

	public override void Init(UIEventListener handle)
	{
		item72 = Get<GameObject>("item72");
		lb_name = Get<UILabel>("lb_name");
	}

	public override void Bind(object data)
	{
		if (data == null) return;
		itemId = (int) data;
		RefreshUI();
	}

	void RefreshUI()
	{
		itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, item72.transform, itemSize.Size72);
		itemBase.Refresh(itemId);
		lb_name.text = YuLingSoulTableManager.Instance.GetYuLingSoulName(itemId);
		lb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemId));
	}

	public override void OnDestroy()
	{
		item72 = null;
		lb_name = null;
		UIItemManager.Instance.RecycleSingleItem(itemBase);
	}
}
