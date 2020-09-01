using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIFastAccessTwoPanel : UIBasePanel
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	List<FastAccessTwoData> dataList = new List<FastAccessTwoData>();
	ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();
	List<int> listId = new List<int>();

	public override void Init()
	{
		base.Init();
		AddCollider();
		mBtnClose.onClick = OnBtnClose;
	}
	public override void Show()
	{
		base.Show();
	}
	public void RefreshUI(int wayId)
	{
		string tempStr = GetWayTableManager.Instance.GetGetWayTips(wayId);
		List<int> listShopId = UtilityMainMath.SplitStringToIntList(tempStr);
		//购买
		int shopId,itemId,payType;
		dataList.Clear();
		for (int i = 0; i < listShopId.Count; i++)
		{
			FastAccessTwoData data = mPoolHandleManager.GetCustomClass<FastAccessTwoData>();
			shopId = listShopId[i];
			itemId = ShopTableManager.Instance.GetShopItemId(shopId);
			data.itemId = itemId;
			data.shopId = shopId;
			payType = ShopTableManager.Instance.GetShopPayType(shopId);
			data.costIcon = $"tubiao{ItemTableManager.Instance.GetItemIcon(payType)}";
			data.itemQuality = ItemTableManager.Instance.GetItemQuality(itemId);
			data.itemName = ItemTableManager.Instance.GetItemName(itemId);
			data.itemNum = ShopTableManager.Instance.GetShopNum(shopId);
			data.cost = ShopTableManager.Instance.GetShopValue(shopId);
			dataList.Add(data);
		}
		mItemGrid.Bind<FastAccessTwoData, FastAccessTwoItem>(dataList, mPoolHandleManager);
		mItemView.SetDynamicArrowVertical(mSpScroll);

		//获取来源
		string getWayStr;
		string[] getWayStrList;
		int getWayId;
		List<int> tempGetWayIdList = mPoolHandleManager.GetSystemClass<List<int>>();
		List<int> getWayIdList = mPoolHandleManager.GetSystemClass<List<int>>();
		tempGetWayIdList.Clear();
		getWayIdList.Clear();
		for (int i = 0; i < dataList.Count; i++)
		{
			getWayStr = ItemTableManager.Instance.GetItemGetWay(dataList[i].itemId);
			getWayStrList = UtilityMainMath.StrToStrArr(getWayStr);
			if (getWayStrList == null)
				break;
			else
			{
				for (int j = 0; j < getWayStrList.Length; j++)
				{
					int.TryParse(getWayStrList[j], out getWayId);
					if (getWayId > 0)
						tempGetWayIdList.Add(getWayId);
				}
			}
		}
		for(int i=0;i < tempGetWayIdList.Count;i++)
		{
			if (!getWayIdList.Contains(tempGetWayIdList[i]))
			{
				getWayIdList.Add(tempGetWayIdList[i]);
			}
		}
		CSGetWayInfo.Instance.GetGetWays(listId, ref getWayList);
		mTextGrid.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);

		//设置尺寸
		UIWidget wid = mBGSprite.GetComponent<UIWidget>();
		Vector3 tempGetWayPos;
		if (listShopId.Count < 3)
		{
			tempGetWayPos = mGetWay.transform.localPosition;
			mGetWay.transform.localPosition = new Vector3(tempGetWayPos.x, tempGetWayPos.y + (3 - listShopId.Count) * 100, tempGetWayPos.z);
			wid.height -= 100 * (3 - listShopId.Count);
		}
		if (getWayIdList.Count < 2)
			wid.height -= 56 * (2 - getWayIdList.Count);
	}
	private void OnBtnClose(GameObject go)
	{
		Close();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		mPoolHandle?.OnDestroy();
		mItemGrid.UnBind<FastAccessTwoItem>();
		mTextGrid.UnBind<GetWayBtn>();
		getWayList.Clear();

		mPoolHandle = null;
		getWayList = null;
		dataList = null;
		listId = null;
	}
}
public class FastAccessTwoItem : UIBinder
{
	Transform targetTrs;
	FastAccessTwoData mData;
	UIItemBase itemBase;
	UILabel lbName,lbCost;
	UISprite spIcon;
	GameObject buyObj;
	public override void Init(UIEventListener handle)
	{
		targetTrs = Get<Transform>("target");
		lbName = Get<UILabel>("lb_itemname");
		lbCost = Get<UILabel>("lb_itemcost");
		spIcon = Get<UISprite>("Sprite");
		buyObj = Get<GameObject>("btn_buy");
	}
	public override void Bind(object data)
	{
		mData = data as FastAccessTwoData;
		if (mData == null) return;

		itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, targetTrs, itemSize.Size66);
		itemBase.Refresh(mData.itemId);
		itemBase.SetCount(mData.itemNum);
		lbName.text = mData.itemName;
		lbName.color = UtilityCsColor.Instance.GetColor(mData.itemQuality);
		lbCost.text = mData.cost.ToString();
		spIcon.spriteName = mData.costIcon;
		UIEventListener.Get(buyObj).onClick = OnBuyClick;
	}
	private void OnBuyClick(GameObject _go)
	{
		int buyTimesLimit,buyTimes;
		TABLE.SHOP shopTab;
		if (!ShopTableManager.Instance.TryGetValue(mData.shopId, out shopTab)) return;
		int.TryParse(shopTab.frequency,out buyTimesLimit);
		buyTimes = CSShopInfo.Instance.GetBuyTimes(mData.shopId);
		UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
		{
			(f as UIBuyConfirmPanel).OpenPanel(mData.itemId, shopTab, buyTimesLimit, buyTimes,true);
		});
	}
	public override void OnDestroy()
	{
		if(itemBase != null)
		{
			UIItemManager.Instance.RecycleSingleItem(itemBase);
			itemBase = null;
		}
		targetTrs = null;
		mData = null;
		lbName = null;
		lbCost = null;
	}
}
public class FastAccessTwoData : IDispose
{
	public FastAccessTwoData() { }
	public FastAccessTwoData(int _itemId,int _shopId,int _itemQuality,int _itemNum,int _cost,string _itemName,string _costIcon)
	{
		itemId = _itemId;
		shopId = _shopId;
		itemQuality = _itemQuality;
		itemName = _itemName;
		itemNum = _itemNum;
		cost = _cost;
		costIcon = _costIcon;
	}
	public int itemId = 0;
	public int shopId = 0; 
	public int itemQuality = 0;
	public int cost = 0;
	public int itemNum = 0;
	public string itemName = "";
	public string costIcon = "";

	public void Dispose() { }
}