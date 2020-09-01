using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UISecretAreaPanel : UIBasePanel
{
	Dictionary<int, SecretAreaData> secretAreaDic;
	ILBetterList<SecretAreaItem> itemList;
	List<List<int>> showList;
	List<int> instanceIdList;
	List<UIItemBase> itemBaseList = new List<UIItemBase>();
	List<int> needItemList;
	int itemIdx = 0;
	int itemId = 0;
	int itemNeed = 0;
	long itemHave = 0;
	int mapId = 0;
	bool isFree = false;
	public override void Init()
	{
		base.Init();
		mBtnAdd.onClick = OnBtnAdd;
		mSpIconBtn.onClick = OnBtnIcon;
		mBtnGo.onClick = OnBtnGo;
		mClientEvent.AddEvent(CEvent.ItemListChange, RefreshItemCost);
		HotManager.Instance.EventHandler.AddEvent(CEvent.SetSecretAreaIndex, RefreshRightUI);
		HotManager.Instance.EventHandler.AddEvent(CEvent.SecretAreaFreeInstance, RefreshAll);
		HotManager.Instance.EventHandler.AddEvent(CEvent.FastAccessJumpToPanel, FastAccessJumpToPanel);
	}
	public override void Show()
	{
		base.Show();
		RefreshAll(0, null);
	}
	private void RefreshAll(uint id, object argv)
	{
		RefreshLeftUI(0, null);
		itemIdx = CSSecretAreaInfo.Instance.GetItemIdx();
		HotManager.Instance.EventHandler.SendEvent(CEvent.SetSecretAreaIndex, itemIdx);
	}
	private void FastAccessJumpToPanel(uint id, object argv)
	{
		int panelId = Convert.ToInt32(argv);
		if (UtilityPanel.CheckGameModelPanelIsThis<UIRecallSecretCombinedPanel>(panelId))
		{
			return;
		}
		UIManager.Instance.ClosePanel<UIRecallSecretCombinedPanel>();
	}
	private void RefreshLeftUI(uint id, object argv)
	{
		secretAreaDic = CSSecretAreaInfo.Instance.GetSecretAreaDic();
		if (secretAreaDic == null) return;
		if (itemList == null)
			itemList = mPoolHandleManager.GetSystemClass<ILBetterList<SecretAreaItem>>();
		else
			RecycleList();
		if (instanceIdList == null)
			instanceIdList = CSSecretAreaInfo.Instance.GetInstanceList();
		mGridTab.MaxCount = secretAreaDic.Count;
		GameObject go;
		SecretAreaData data;
		SecretAreaItem binder;
		int mapIdIt = 0;
		for (int i = 0; i < mGridTab.MaxCount; i++)
		{
			mapIdIt = instanceIdList[i];
			go = mGridTab.controlList[i];
			data = secretAreaDic[mapIdIt];
			binder = go.GetOrAddBinder<SecretAreaItem>(mPoolHandleManager);
			binder.Bind(data);
			binder.index = i;
			itemList.Add(binder);
		}
	}
	private void RefreshRightUI(uint id, object argv)
	{
		int index = (int)argv;
		int playLv = CSMainPlayerInfo.Instance.Level;
		int timerId,curTime,startTime,endTime;
		string startStr, endStr,activeTimeStr,itemStr,itemIcon;
		ColorType color;
		if (instanceIdList != null && secretAreaDic != null)
		{
			mapId = instanceIdList[index];
			if (secretAreaDic.ContainsKey(mapId))
			{
				SecretAreaData data = secretAreaDic[mapId];
				timerId = InstanceTableManager.Instance.GetInstanceTimerId(mapId);
				activeTimeStr = TimerTableManager.Instance.GetTimerDesc(timerId);
				color = playLv >= data.mapLv ? ColorType.Green:ColorType.Red;
				mLbLevel.text = CSString.Format(1919, data.mapLv).BBCode(color);
				mLbDesc.text = data.mapDesc;
				//Ê±¼äÅÐ¶Ï
				curTime = CSServerTime.Now.Hour * 10000 + CSServerTime.Now.Minute * 100 + CSServerTime.Now.Second;
				startStr = TimerTableManager.Instance.GetTimerStartTime(timerId);
				endStr = TimerTableManager.Instance.GetTimerEndTime(timerId);
				startTime = UtilityMath.CronTimeStringParseToHMS(startStr);
				endTime = UtilityMath.CronTimeStringParseToHMS(endStr);
				if(curTime >= startTime && curTime <= endTime)
				{
					isFree = data.isFree;
					mLbFree.gameObject.CustomActive(data.isFree);
					if (data.isFree)
					{
						mLbFree.text = CSString.Format(1920);
						mLbBtn.text = CSString.Format(1922);
						mUIItemBar.CustomActive(false);
					}
					else
					{
						mUIItemBar.CustomActive(true);
						mLbFree.text = "";
						itemStr = InstanceTableManager.Instance.GetInstanceRequireItems(mapId);
						needItemList = UtilityMainMath.SplitStringToIntList(itemStr);
						itemId = needItemList[0];
						itemNeed = needItemList[1];
						itemIcon = ItemTableManager.Instance.GetItemIcon(itemId);
						mSpIcon.spriteName = $"tubiao{itemIcon}";
						RefreshItemCost(0,null);
					}
					mLimit.CustomActive(true);
					mLbTime.text = "";
				}
				else
				{
					mLbTime.text = activeTimeStr;
					mLimit.CustomActive(false);
				}
				//µôÂäÔ¤ÀÀ
				showList = UtilityMainMath.SplitStringToIntLists(data.show);
				//if (itemBaseList == null) itemBaseList = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();
				mGrid.MaxCount = showList.Count;
				for (int i=0;i< showList.Count;i++)
				{
					if(itemBaseList.Count < i+1)
					{
						UIItemBase itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, 
							mGrid.controlList[i].transform, itemSize.Default);
						itemBaseList.Add(itemBase);
					}
					if(itemBaseList[i] != null)
					{
						itemBaseList[i].Refresh(showList[i][0]);
						itemBaseList[i].SetCount(showList[i][1]);
					}
				}
				//±³¾°ÏÔÊ¾
				CSEffectPlayMgr.Instance.ShowUITexture(mTextBg, data.bg);
			}
		}
	}
	private void RefreshItemCost(uint id, object argv)
	{
		ColorType color;
		itemHave = itemId.GetItemCount();
		if (itemHave >= itemNeed)
			mLbBtn.text = CSString.Format(1922);
		else
			mLbBtn.text = CSString.Format(1921);

		color = itemHave >= itemNeed ? ColorType.Green : ColorType.Red;
		mLbValue.text = $"{itemHave}/{itemNeed}".BBCode(color);
	}
	private void OnBtnAdd(GameObject _go)
	{
		if(itemId > 0)
			Utility.ShowGetWay(itemId);
	}
	private void OnBtnIcon(GameObject _go)
	{
		if (itemId > 0)
			UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemId);
	}
	private void OnBtnGo(GameObject _go)
	{
		if(isFree)
			GoMap();
		else
		{
			if (itemHave >= itemNeed)
				GoMap();
			else
				Utility.ShowGetWay(itemId);
		}
	}
	private void GoMap()
	{
		Net.ReqEnterInstanceMessage(mapId);
		UIManager.Instance.ClosePanel<UIRecallSecretCombinedPanel>();
	}
	private void RecycleList()
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			mPoolHandleManager.Recycle(itemList[i]);
		}
		itemList.Clear();
	}
	protected override void OnDestroy()
	{
		itemIdx = 0;
		itemId = 0;
		itemNeed = 0;
		itemHave = 0;
		mapId = 0;
		isFree = false;
		mGridTab.UnBind<SecretAreaItem>();
		HotManager.Instance.EventHandler.RemoveEvent(CEvent.SetSecretAreaIndex, RefreshRightUI);
		HotManager.Instance.EventHandler.RemoveEvent(CEvent.SecretAreaFreeInstance, RefreshAll);
		HotManager.Instance.EventHandler.RemoveEvent(CEvent.FastAccessJumpToPanel, FastAccessJumpToPanel);
		CSEffectPlayMgr.Instance.Recycle(mTextBg);
		if (itemBaseList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList);
			itemBaseList.Clear();
			itemBaseList = null;
		}
		showList?.Clear();
		needItemList?.Clear();
		showList = null;
		needItemList = null;
		base.OnDestroy();
	}

	private class SecretAreaItem : UIBinder
	{
		GameObject select,flag;
		UISprite headItem,sp_select,sp_nameBg;
		UILabel lb_name;
		SecretAreaData mData;
		public override void Init(UIEventListener handle)
		{
			select = Get<GameObject>("select");
			headItem = Get<UISprite>("headitem");
			sp_select = Get<UISprite>("select");
			flag = Get<GameObject>("flag");
			lb_name = Get<UILabel>("lb_name");
			sp_nameBg = Get<UISprite>("lb_name/Sprite");
			handle.onClick = OnBtnClick;
			HotManager.Instance.EventHandler.AddEvent(CEvent.SetSecretAreaIndex, SetSecretArea);
		}
		public override void Bind(object data)
		{
			mData = data as SecretAreaData;
			if (mData == null) return;
			flag.CustomActive(mData.isFree);
			headItem.spriteName = mData.head;
			sp_select.spriteName = mData.select;
			lb_name.text = mData.mapName.BBCode(ColorType.MainText);
		}
		private void OnBtnClick(GameObject _go)
		{
			HotManager.Instance.EventHandler.SendEvent(CEvent.SetSecretAreaIndex, index);
		}
		private void SetSecretArea(uint id, object argv)
		{
			int idx = (int)argv;
			select.CustomActive(idx == index);
			if (idx == index)
				sp_nameBg.spriteName = "recall_secret_label_1";
			else
				sp_nameBg.spriteName = "recall_secret_label_0";
		}
		public override void OnDestroy()
		{
			HotManager.Instance.EventHandler.RemoveEvent(CEvent.SetSecretAreaIndex, SetSecretArea);
			select = null;
			sp_select = null;
			sp_nameBg = null;
			flag = null;
			headItem = null;
			lb_name = null;
			mData = null;
		}
	}
}