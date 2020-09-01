using System.Collections.Generic;
using map;
public class CSRandomThingInstanceInfo : CSInfo<CSRandomThingInstanceInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	CSBetterLisHot<GoldKeyPickUpData> pickUpDataList = new CSBetterLisHot<GoldKeyPickUpData>();
	int roleNum = 0;
	public void SetMapDetailsMessage(MapDetails msg)
	{
		roleNum = msg.curPlayerNum;
	}
	public int ReturnMapRoleNum()
	{
		return roleNum;
	}
	public void SetGoldKeyPickUpItemMessage(GoldKeyPickUpItems msg)
	{
		pickUpDataList.Clear();
		for (int i=0;i<msg.pickUpItems.Count;i++)
		{
			GoldKeyPickUpData goldKeyPickUpData = mPoolHandle.GetCustomClass<GoldKeyPickUpData>();
			goldKeyPickUpData.itemId = msg.pickUpItems[i].itemConfigId;
			goldKeyPickUpData.itemNum = msg.pickUpItems[i].itemNum;
			goldKeyPickUpData.x = msg.pickUpItems[i].x;
			goldKeyPickUpData.y = msg.pickUpItems[i].y;
			pickUpDataList.Add(goldKeyPickUpData);
		}
	}
	public CSBetterLisHot<GoldKeyPickUpData> GetGoldKeyPickUpItemList()
	{
		return pickUpDataList;
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		pickUpDataList.Clear();
		mPoolHandle = null;

		roleNum = 0;
	}
}
public class GoldKeyPickUpData : IDispose
{
	public GoldKeyPickUpData() { }
	public GoldKeyPickUpData(int _itemId,int _itemNum,int _x,int _y)
	{
		itemId = _itemId;
		itemNum = _itemNum;
		x = _x;
		y = _y;
	}
	public int itemId = 0;
	public int itemNum = 0;
	public int x = 0;
	public int y = 0;
	public void Dispose() { }
}