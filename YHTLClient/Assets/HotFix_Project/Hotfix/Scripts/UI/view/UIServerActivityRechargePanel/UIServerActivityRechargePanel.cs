using System.Collections.Generic;
using UnityEngine;
public partial class UIServerActivityRechargePanel : UIBasePanel
{
	List<UIItemBase> itemBaseList = new List<UIItemBase>();
	DayChargeData dayChargeData = new DayChargeData();
	public override void Init()
	{
		base.Init();
		UIEventListener.Get(mBtnGet).onClick = OnBtnGet;
		mClientEvent.AddEvent(CEvent.GetDayChargeInfo, GetDayChargeInfo);
		mClientEvent.AddEvent(CEvent.GetEveryTimeDayChargeInfo, GetEveryTimeDayChargeInfo);		//跨天更新ui
	}
	public override void Show()
	{
		base.Show();
		RefreshUI();
		CSEffectPlayMgr.Instance.ShowUITexture(mBGBanner10, "banner10");
	}
	private void GetDayChargeInfo(uint id, object data)
	{
		if (dayChargeData.state == 0)
			UtilityTips.ShowRedTips(1124);
		else if (dayChargeData.state == 2)
			UtilityTips.ShowRedTips(1125);
		else
			RefreshUI();
	}
	private void GetEveryTimeDayChargeInfo(uint id, object data)
	{
		RefreshUI();
	}
	private void RefreshUI()
	{
		UIItemBase itemBase;
		Transform itemParent;
		UILabel btnLb;
		int itemId, itemNum;
		btnLb = mBtnGet.transform.GetChild(0).GetComponent<UILabel>();
		dayChargeData = CSDayChargeInfo.Instance.GetChargeData();
		mLbMoney.text = dayChargeData.leftDayCharge.ToString();
		string boxItemStr = BoxTableManager.Instance.GetBoxItem(dayChargeData.boxId);
		string boxItemNumStr = BoxTableManager.Instance.GetBoxNum(dayChargeData.boxId);
		string[] boxItems = UtilityMainMath.StrToStrArr(boxItemStr,'&');
		string[] boxItemNums = UtilityMainMath.StrToStrArr(boxItemNumStr,'&');
		if (boxItems == null || boxItemNums == null) return;
		mGrid.MaxCount = boxItems.Length;
		UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList);
		itemBaseList.Clear();
		for (int i = 0; i < mGrid.MaxCount; i++)
		{
			int.TryParse(boxItems[i], out itemId);
			int.TryParse(boxItemNums[i], out itemNum);
			itemParent = mGrid.controlList[i].transform;
			itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, itemParent, itemSize.Size66);
			itemBase.Refresh(itemId);
			itemBase.SetCount(itemNum, Color.white);
			itemBaseList.Add(itemBase);
		}
		if (dayChargeData.btnShow == 1)
			btnLb.text = ClientTipsTableManager.Instance.GetClientTipsContext(1121);
		else
			btnLb.text = ClientTipsTableManager.Instance.GetClientTipsContext(1120);
	}
	private void OnBtnGet(GameObject _go)
	{
		if (dayChargeData.btnShow == 1)
		{
			//dayChargeData.curId+1，是服务端索引从1开始
			Net.CSDayChargeRewardGetMessage(dayChargeData.curId + 1);
		}
		else
		{
			UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
			UtilityPanel.JumpToPanel(12305);
		}
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (itemBaseList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList);
			itemBaseList.Clear();
			itemBaseList = null;
		}
		dayChargeData = null;
		CSEffectPlayMgr.Instance.Recycle(mBGBanner10);
	}
}