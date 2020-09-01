using System.Collections.Generic;
using UnityEngine;
using System;

public class LianTiItem : IDispose
{
	GameObject go;
	GameObject select;
	GameObject flag;
	UILabel bossNum;
	UILabel mapName;
	UISprite bg;
	UISprite buffBg;
	public TABLE.INSTANCE data;
	public LianTiLandData lianTiLandData;
	bool isChoose = false;
	Action<LianTiItem> action;
	bool LevelNot = false;
	public void SetData(GameObject _go, TABLE.INSTANCE _data, LianTiLandData _lianTiLandData, Action<LianTiItem> _action = null)
	{
		go = _go;
		data = _data;
		action = _action;
		lianTiLandData = _lianTiLandData;
		InitComponent();
	}
	void InitComponent()
	{
		select = go.transform.Find("select").gameObject;
		flag = go.transform.Find("flag").gameObject;
		bossNum = go.transform.Find("lb_boss").GetComponent<UILabel>();
		mapName = go.transform.Find("lb_level").GetComponent<UILabel>();
		bg = go.transform.Find("bg").GetComponent<UISprite>();
		buffBg = go.transform.Find("sp_buff").GetComponent<UISprite>();
	}
	public void Refresh()
	{
		CSStringBuilder.Clear();
		string tempBoss,lianTiName,tempText;
		if(data.openLianTiLevel <= CSLianTiInfo.Instance.LianTiID)
		{
			LevelNot = true;
			tempBoss = CSString.Format(726);
			tempText = $"{lianTiLandData.surBossNum}/{lianTiLandData.bossNum}";
			if (lianTiLandData.surBossNum > 0)
			{
				bossNum.text = CSStringBuilder.Append(tempBoss, UtilityColor.GetColorString(ColorType.Green), tempText).ToString();
			}
			else
			{
				bossNum.text = CSStringBuilder.Append(tempBoss, UtilityColor.GetColorString(ColorType.Red), tempText).ToString();
			}
			bg.color = Color.white;
			buffBg.color = Color.white;
		}
		else
		{
			LevelNot = false;
			lianTiName = LianTiTableManager.Instance.GetLianTiName(data.openLianTiLevel);
			tempBoss = CSString.Format(1303, lianTiName);
			bossNum.text = CSStringBuilder.Append(UtilityColor.GetColorString(ColorType.Red),tempBoss).ToString();
			bg.color = Color.gray;
			buffBg.color = Color.gray;
		}
		flag.SetActive(lianTiLandData.isFirstKill);
		mapName.text = CSString.Format(ClientTipsTableManager.Instance.GetClientTipsContext(878), data.level);
		UIEventListener.Get(go).onClick = Click;
		CSStringBuilder.Clear();
	}
	void Click(GameObject _go)
	{
		if (action != null)
		{
			action(this);
		}
	}
	public void ChangeSelected(bool _state)
	{
		isChoose = _state;
		select.SetActive(isChoose);
	}
	public bool GetLevelState()
	{
		return LevelNot;
	}
	public void Dispose()
	{
		go = null;
		select = null;
		flag = null;
		bossNum = null;
		mapName = null;
		data = null;
		bg = null;
		buffBg = null;
	}
}

public partial class UILianTiLandPanel : UIBasePanel
{
	List<LianTiItem> itemList = new List<LianTiItem>();
	CSBetterLisHot<TABLE.INSTANCE> dataList;
	LianTiItem currentItem;
	List<UIItemBase> lianTiTipItemList = new List<UIItemBase>();
	string[] count;
	bool isTeleport = false;
	int itemId = 0;
	int mapId = 0;
	int itemNum = 0;
	long itemHave = 0;

	public override void Init()
	{
		base.Init();
		AddCollider();
		mCloseBtn.onClick = OnBtnClose;
		mTeleportBtn.onClick = OnTeleportBtn;
		mIcon.onClick = OnIconBtn;
		mClientEvent.AddEvent(CEvent.GetLianTiLandInfo, RefreshMap);
		mClientEvent.AddEvent(CEvent.ItemListChange, RefreshMap);
	}
	public override void Show()
	{
		base.Show();
		//RefreshMap();
	}

	private void RefreshMap(uint id, object argv)
	{
		int t_ind = 0;
		int tempInd = CSLianTiInfo.Instance.GetLianTiLandTind();
		float width = mMapGrid.CellWidth;
		CSBetterLisHot<LianTiLandData> lianTiLandList = CSLianTiInfo.Instance.GetLianTiLandList();
		dataList = InstanceTableManager.Instance.GetTableDataByType(4);
		mMapGrid.MaxCount = dataList.Count;
		mMapGrid.MaxPerLine = dataList.Count;
		mScrollBar.onChange.Add(new EventDelegate(OnChange));
		itemList.Clear();
		for (int i = 0; i < mMapGrid.controlList.Count; i++)
		{
			LianTiItem item = mPoolHandleManager.GetCustomClass<LianTiItem>();
			item.SetData(mMapGrid.controlList[i], dataList[i], lianTiLandList[i], ItemClick);
			itemList.Add(item);
			itemList[i].Refresh();
			if (itemList[i].GetLevelState())
				t_ind = i;
				
			if ((i+1)%2 != 1)
			{
				mMapGrid.transform.GetChild(i + 1).localPosition = new Vector3(i * width, 68, 0);
			}
		}
		t_ind = tempInd > 0 ? tempInd - 1 : t_ind;
		mMapsScrollView.ResetPosition();
		if (t_ind > 4)
		{
			TweenPosition.Begin(mMapGrid.gameObject, 0.2f, new Vector3(-290 - (width * (t_ind - 4)), 0, 0));
		}
		ItemClick(itemList[t_ind]);
		if (null != mBG)
		{
			CSEffectPlayMgr.Instance.ShowUITexture(mBG, "lianti_bg");
		}
	}
	void ItemClick(LianTiItem _go)
	{
		if (currentItem != null)
		{
			currentItem.ChangeSelected(false);
		}
		currentItem = _go;
		currentItem.ChangeSelected(true);
		ShowItemTips(_go);
		ShowItemCost(_go);
	}
	void ShowItemTips(LianTiItem item)
	{
		isTeleport = item.GetLevelState();
		mLBLevel.text = item.data.tips;
		count = UtilityMainMath.StrToStrArr(item.data.show,'&');
		if (count == null) return;
		for (int i = 0; i < count.Length; i++)
		{
			if(lianTiTipItemList.Count < i + 1)
				lianTiTipItemList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mTipsGrid.transform, itemSize.Size66));
			string[] mes = UtilityMainMath.StrToStrArr(count[i]);
			if (mes == null || lianTiTipItemList[i] == null) return;
			lianTiTipItemList[i].Refresh(ItemTableManager.Instance.GetItemCfg(int.Parse(mes[0])));
			lianTiTipItemList[i].SetCount(int.Parse(mes[1]), CSColor.white);
		}
		mTipsGrid.Reposition();
	}
	private void ShowItemCost(LianTiItem item)
	{
		mapId = item.lianTiLandData.mapId;
		string itemIcon;
		ColorType colorType;
		DealWithCost(mapId, ref itemId, ref itemNum,ref itemHave);
		if (itemId > 0)
		{
			colorType = itemHave >= itemNum ? ColorType.Green : ColorType.Red ;
			itemIcon = ItemTableManager.Instance.GetItemIcon(itemId);
			mLbValue.text = $"{itemHave}/{itemNum}".BBCode(colorType);
			mSpIcon.spriteName = $"tubiao{itemIcon}";
			UIEventListener.Get(mBtnAdd, itemId).onClick = OnBtnAdd;
		}
	}
	private void DealWithCost(int mapId,ref int id,ref int num,ref long have)
	{
		string costStr = InstanceTableManager.Instance.GetInstanceRequireItems(mapId);
		List<int> costList = UtilityMainMath.SplitStringToIntList(costStr);
		if (costList.Count > 0)
		{
			id = costList[0];
			num = costList[1];
			have = id.GetItemCount();
		}
	}
	private void OnBtnAdd(GameObject _go)
	{
		int id = (int)UIEventListener.Get(_go).parameter;
		Utility.ShowGetWay(id);
	}
	private void OnChange()
	{
		mLeftObj.SetActive(mScrollBar.value >= 0.05);
		mRightObj.SetActive(mScrollBar.value <= 0.95);
	}
	#region btnClick
	void OnBtnClose(GameObject go)
	{
		Close();
	}

	void OnTeleportBtn(GameObject go)
	{
		if (!isTeleport)
		{
			string tip = $"{currentItem.lianTiLandData.tip}[-]";
			string mapName = currentItem.lianTiLandData.mapName;
			UtilityTips.ShowPromptWordTips(80, ConfirmMoveUIRoleLianTi, mapName, tip);
		}
		else if(itemHave < itemNum)
			Utility.ShowGetWay(itemId);
		else
		{
			if (currentItem != null)
			{
				Net.ReqEnterInstanceMessage(currentItem.lianTiLandData.mapId);
			}
			UIManager.Instance.ClosePanel<UILianTiLandPanel>();
			UIManager.Instance.ClosePanel<UIRolePanel>();
		}
		
	}
	private void OnIconBtn(GameObject go)
	{
		UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemId);
	}
	private void ConfirmMoveUIRoleLianTi()
	{
		CSLianTiInfo.Instance.SetIsTurn(false);
		UtilityPanel.JumpToPanel(12910);
		Close();
	}
	protected new void Close()
	{
		base.Close();
		bool isTurn = CSLianTiInfo.Instance.GetIsTurn();
		if (isTurn)
			UtilityPanel.JumpToPanel(10440);
	}
	#endregion
	protected override void OnDestroy()
	{
		if (lianTiTipItemList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(lianTiTipItemList);
			lianTiTipItemList.Clear();
			lianTiTipItemList = null;
		}
		CSEffectPlayMgr.Instance.Recycle(mBG);
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetLianTiLandInfo, RefreshMap);
			mClientEvent.RemoveEvent(CEvent.ItemListChange, RefreshMap);
		}
		itemList.Clear();
		dataList.Clear();
		itemList = null;
		dataList = null;
		currentItem = null;
		mTipsGrid = null;
		count = null;
		isTeleport = false;
		itemId = 0;
		mapId = 0;
		itemNum = 0;
		itemHave = 0;
		base.OnDestroy();
	}
}

