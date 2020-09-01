using System.Collections.Generic;
using UnityEngine;
public partial class UIServerActivityMapPanel : UIBasePanel
{
	int mapId = 0;
	int id = 0;
	int itemNum = 0;
	long have = 0;
	List<UIItemBase> itemList = new List<UIItemBase>();
	public override void Init()
	{
		base.Init();
		mBtnEnter.onClick = OnEnter;
		mBtnMoney.onClick = OnMoney;
		mIconTips.onClick = OnItemTip;
		mClientEvent.AddEvent(CEvent.ItemListChange, RefreshUI);
	}
	public override void Show()
	{
		base.Show();
		
		//每日充值地图判断小红点用
		if (CSDayChargeInfo.Instance.GetIsShowMapRed())
		{
			CSDayChargeInfo.Instance.SetIsShowMapRed();
			HotManager.Instance.EventHandler.SendEvent(CEvent.GetDayChargeMapFirst);
		}
		RefreshUI(0,null);
		CSEffectPlayMgr.Instance.ShowUITexture(mBanner13, "banner13");
	}
	private void RefreshUI(uint _id, object argv)
	{
		int num = CSMainPlayerInfo.Instance.RoleExtraValues.todayTimes;
		mBtnEnter.gameObject.SetActive(num > 0);
		mBtnMoney.gameObject.SetActive(num <= 0);

		mapId = CSDayChargeInfo.Instance.GetDayChargeMapId();
		string costStr = InstanceTableManager.Instance.GetInstanceRequireItems(mapId);
		List<int> costList = UtilityMainMath.SplitStringToIntList(costStr);

		if (costList.Count > 0)
		{
			id = costList[0];
			itemNum = costList[1];
			have = id.GetItemCount();
		}
		ColorType colorType;
		string itemIcon;
		if (id > 0)
		{
			colorType = have >= itemNum ? ColorType.Green : ColorType.Red;
			itemIcon = ItemTableManager.Instance.GetItemIcon(id);
			mLbValue.text = $"{have}/{itemNum}".BBCode(colorType);
			mSpIcon.spriteName = $"tubiao{itemIcon}";
			UIEventListener.Get(mBtnAdd, id).onClick = OnBtnAdd;
		}
		//奖励预览
		string showStr = InstanceTableManager.Instance.GetInstanceShow(mapId);
		List<List<int>> showList = UtilityMainMath.SplitStringToIntLists(showStr);
		Transform parentTrs;
		//if (itemList == null)
		//	itemList = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();
		mGrid.MaxCount = showList.Count;
		for (int i=0;i< mGrid.MaxCount;i++)
		{
			parentTrs = mGrid.controlList[i].transform;
			if(itemList.Count < i+1)
			{
				UIItemBase itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, parentTrs, itemSize.Size60);
				itemList.Add(itemBase);
			}
			itemList[i].Refresh(showList[i][0]);
			itemList[i].SetCount(showList[i][1]);
			
		}
	}
	private void OnBtnAdd(GameObject _go)
	{
		int id = (int)UIEventListener.Get(_go).parameter;
		Utility.ShowGetWay(id);
	}
	private void OnEnter(GameObject _go)
	{
		if(have >= itemNum)
		{
			Net.ReqEnterInstanceMessage(mapId);
			UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
		}
		else
			Utility.ShowGetWay(id);
	}
	private void OnItemTip(GameObject _go)
	{
		UITipsManager.Instance.CreateTips(TipsOpenType.Normal, id);
	}
	private void OnMoney(GameObject _go)
	{
		UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
		UtilityPanel.JumpToPanel(12305);
	}
	protected override void OnDestroy()
	{
		CSEffectPlayMgr.Instance.Recycle(mBanner13);
		mapId = 0;
		id = 0;
		itemNum = 0;
		have = 0;
		if(itemList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(itemList);
			itemList.Clear();
			itemList = null;
		}
		if(mClientEvent != null)
			mClientEvent.RemoveEvent(CEvent.ItemListChange, RefreshUI);
		base.OnDestroy();
	}
}