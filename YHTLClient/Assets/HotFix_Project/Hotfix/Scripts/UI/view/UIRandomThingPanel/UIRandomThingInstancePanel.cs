using System.Collections.Generic;
using UnityEngine;

public partial class UIRandomThingInstancePanel : UIBasePanel
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	CSBetterLisHot<GoldKeyPickUpData> pickUpDataList;
	UILabel lb_num;
	public override UILayerType PanelLayerType
	{
		get { return UILayerType.Resident; }
	}
	public override void Init()
	{
		base.Init();
		lb_num = UIPrefabTrans.Find("left/view/lb_num").GetComponent<UILabel>();
		mClientEvent.AddEvent(CEvent.GetMapDescInfo, RefreshMapPlayerNum);
		mClientEvent.AddEvent(CEvent.GetGoldKeyPickUpItemList, GetGoldKeyPickUpItemList);
	}
	public override void Show()
	{
		base.Show();
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetRandomThingInfo, true);
		UIManager.Instance.CreatePanel<UIRandomBtnPanel>();
		RefreshMapPlayerNum(0, null);
	}
	private void RefreshMapPlayerNum(uint uiEvtID, object data)
	{
		lb_num.text = CSRandomThingInstanceInfo.Instance.ReturnMapRoleNum().ToString();
	}
	private void GetGoldKeyPickUpItemList(uint uiEvtID, object data)
	{
		CSMisc.Dot2 playerPosition =CSAvatarManager.MainPlayer.NewCell.Coord;
		pickUpDataList = CSRandomThingInstanceInfo.Instance.GetGoldKeyPickUpItemList();
		List<int> itemIdList = mPoolHandle.GetSystemClass<List<int>>();
		List<Vector3> itemVector3List = mPoolHandle.GetSystemClass<List<Vector3>>();
		Vector3 tempVector3 = new Vector3();
		float uiRootScale = 0.003125f;
		for (int i=0;i< pickUpDataList.Count;i++)
		{
			itemIdList.Add(pickUpDataList[i].itemId);
			tempVector3.x = (pickUpDataList[i].x - playerPosition.x) * 60 * uiRootScale;
			tempVector3.y = (pickUpDataList[i].y -playerPosition.y-2f) * 40 * uiRootScale;
			itemVector3List.Add(tempVector3);
		}
		if (itemIdList.Count > 0 )
		{
			Utility.ShowFlyToPlayerEffect(itemIdList, itemVector3List);
		}
	}
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetMapDescInfo, RefreshMapPlayerNum);
			mClientEvent.RemoveEvent(CEvent.GetGoldKeyPickUpItemList, GetGoldKeyPickUpItemList);
		}
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetRandomThingInfo, false);
		UIManager.Instance.ClosePanel<UIRandomBtnPanel>();
		UIManager.Instance.ClosePanel<UIFastAccessPanel>();
		mPoolHandle?.OnDestroy();
		mPoolHandle = null;
		lb_num = null;
	}
}
