using System;
using System.Collections.Generic;
using UnityEngine;
public partial class UIServerActivityEquipRewardsPanel : UIBasePanel
{
	public enum GetState
	{
		None = 0,
		CanGet = 1,//可领取
		HasGet = 2,//已领取
		NotGet = 3,//不可领取
	}
	ILBetterList<EquipRewardsData> dataList;
	ILBetterList<EquipRewardsItem> itemList;
	int activityId = 0;
	int woLongId = 0;
	int perId = 0;
	long leftTime = 0;
	int activityType;
	int index = 1;
	int realIndexAbs = 0;
	bool isShowRed = false;
	GameObject go;
	EquipRewardsItem binder;
	public override void Init()
	{
		base.Init();
		mToggle1.onChange.Add(new EventDelegate(OnBtn));
		mBar.onChange.Add(new EventDelegate(OnChange));
		mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);
		mClientEvent.AddEvent(CEvent.FastAccessTransferNpc, CloseEvent);
		mClientEvent.AddEvent(CEvent.GetEquipRewardsInfo, RefreshDataAndUI);
	}
	public override void Show()
	{
		base.Show();
		RefreshTime();
	}
	#region 通用数据刷新
	private void RefreshCommonData()
	{
		if(woLongId == 0)
			woLongId = CSEquipRewardsInfo.Instance.GetWoLongId();
		if(perId == 0)
			perId = CSEquipRewardsInfo.Instance.GetPerId();

		activityId = CSEquipRewardsInfo.Instance.GetAcitvityId();
		if(activityId == woLongId)
			activityType = (int)OpenActivityType.EquipRewards;
		else if (activityId == perId)
			activityType = (int)OpenActivityType.PerEquipRewards;
	}
	#endregion
	private void OnBtn()
	{
		if(mToggle1.value)
			index = 1;
		else
			index = 2;
			
		RefreshDataAndUI(0, null);
	}
	private void RefreshDataAndUI(uint id, object argv)
	{
		RefreshCommonData();
		RefreshUI();
	}
	private void CloseEvent(uint id, object argv)
	{
		UIManager.Instance.ClosePanel<UIServerActivityPanel>();
	}
	private void OnChange()
	{
		mSpScroll.gameObject.SetActive(mBar.value <= 0.99);
	}
	private void RefreshUI()
	{
		dataList = CSEquipRewardsInfo.Instance.GetRewardsList(activityType, index);
		if (dataList.Count <= 0) return;
		mGridItem.MaxCount = 4;
		mWrap.minIndex = -dataList.Count + 1;
		mWrap.maxIndex = 0;
		mWrap.cullContent = false;
		mWrap.enabled = true;
		mWrap.ResetChildPositions();
		mitemView.ResetPosition();

		if (itemList == null)
			itemList = new ILBetterList<EquipRewardsItem>();
		else
			itemList.Clear();
		for (int i = 0; i < mGridItem.MaxCount; i++)
		{
			if (itemList.Count < mGridItem.MaxCount)
			{
				go = mGridItem.controlList[i];
				binder = go.GetOrAddBinder<EquipRewardsItem>(mPoolHandleManager);
				itemList.Add(binder);
			}
			itemList[i].Bind(dataList[i]);
		}
		mWrap.onInitializeItem = OnUpdateItem;
		mLbText.text = SpecialActivityTableManager.Instance.GetSpecialActivityTips(activityId);
		if (index == 1)
		{
			mLbOperator.text = CSString.Format(2015);
			CSEffectPlayMgr.Instance.ShowUITexture(mtex_title, "banner15");
		}
		else
		{
			mLbOperator.text = CSString.Format(2016);
			CSEffectPlayMgr.Instance.ShowUITexture(mtex_title, "banner27");
		}
		if (activityId == woLongId)
			isShowRed = CSEquipRewardsInfo.Instance.WoLongRedPoint();
		else if (activityId == perId)
			isShowRed = CSEquipRewardsInfo.Instance.PerRedPoint();
		mRedPoint.CustomActive(isShowRed);
	}
	public void SetItemClick(int type)
	{
		if (type == 1) return;
		activityType = type;
		index = 2;
		mToggle2.value = true;
	}
	private void OnUpdateItem(GameObject go, int index, int realIndex)
	{
		realIndexAbs = Math.Abs(realIndex);
		itemList[index].Bind(dataList[realIndexAbs]);
	}
	private void RefreshTime()
	{
		leftTime = UIServerActivityPanel.GetEndTime(activityId);
		mCSInvoke?.InvokeRepeating(0f, 1f, CountDown);
	}
	private void CountDown()
	{
		if (leftTime <= 0)
		{
			mLbTime.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(0, 1));
			mCSInvoke?.StopInvokeRepeating();
		}
		else
		{
			mLbTime.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));
			leftTime--;
		}
	}
	public override void OnHide()
	{
		mCSInvoke?.StopInvokeRepeating();
	}

	protected override void OnDestroy()
	{
		mCSInvoke?.StopInvokeRepeating();
		CSEffectPlayMgr.Instance.Recycle(mtex_title);
		mGridItem.UnBind<EquipRewardsItem>();
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.FastAccessJumpToPanel, CloseEvent);
			mClientEvent.RemoveEvent(CEvent.FastAccessTransferNpc, CloseEvent);
			mClientEvent.RemoveEvent(CEvent.GetEquipRewardsInfo, RefreshDataAndUI);
		}
		itemList = null;
		mBar.onChange = null;
		go = null;
		binder = null;

		activityId = 0;
		woLongId = 0;
		perId = 0;
		leftTime = 0;
		activityType = 0;
		index = 0;
		realIndexAbs = 0;

		isShowRed = false;
		base.OnDestroy();
	}
}
public class EquipRewardsItem : UIBinder
{
	int quality, boxId;
	string targetMes, wayStr;
	int itemId = 0;

	EquipRewardsData mData;
	UIItemBase itemBase;
	List<UIItemBase> itemBaseList;
	List<int> itemList,numList;

	GameObject getObj,recObj,goObj,effObj;
	Transform targetTrs;
	UIGridContainer uiGrid;
	UILabel lb_name, lb_item;
	UIEventListener recBtn, goBtn;

	TABLE.SPECIALACTIVEREWARD rewards;
	TABLE.BOX box;
	TABLE.ITEM itemTab;
	public override void Init(UIEventListener handle)
	{
		lb_name = Get<UILabel>("lb_name");
		lb_item = Get<UILabel>("lb_item");
		targetTrs = Get<Transform>("targetItemBase");
		uiGrid = Get<UIGridContainer>("Grid");
		getObj = Get<GameObject>("sp_get");
		recObj = Get<GameObject>("btn_receive");
		goObj = Get<GameObject>("btn_go");
		recBtn = Get<UIEventListener>("btn_receive");
		goBtn = Get<UIEventListener>("btn_go");
		effObj = Get<GameObject>("btn_receive/effect");
		recBtn.onClick = OnBtnReceive;
		goBtn.onClick = OnBtnGo;
		if (itemBase == null)
			itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, targetTrs, itemSize.Size66);
		if (itemBaseList == null)
			itemBaseList = new List<UIItemBase>();
	}
	public override void Bind(object data)
	{
		mData = data as EquipRewardsData;
		if (mData == null) return;
		SpecialActiveRewardTableManager.Instance.TryGetValue(mData.id,out rewards);
		if (rewards == null) return;
		boxId = rewards.reward;
		targetMes = rewards.termNum;
		if(targetMes != null) int.TryParse(targetMes, out itemId);
		BoxTableManager.Instance.TryGetValue(boxId,out box);
		if (box == null) return;
		if (itemList == null) itemList = new List<int>();
		if (numList == null) numList = new List<int>();
		UtilityMainMath.SplitStringToIntList(itemList, box.item,'&');
		UtilityMainMath.SplitStringToIntList(numList,box.num, '&');
		ItemTableManager.Instance.TryGetValue(itemId, out itemTab);
		if (itemTab == null) return;
		quality = itemTab.quality;

		lb_item.text = itemTab.name;
		lb_item.color = UtilityCsColor.Instance.GetColor(quality);

		if(mData.goalId == 1)
		{
			if (!string.IsNullOrEmpty(mData.name))
				lb_name.text = $"[492A07]{mData.name}[-]";
			else
				lb_name.text = CSString.Format(1869).BBCode(ColorType.Red);
			getObj.CustomActive(false);
			recObj.CustomActive(false);
			goObj.CustomActive(false);
		}
		else if(mData.goalId == 2)
		{
			lb_name.text = "";
			getObj.CustomActive(mData.xuanShangType == (int)UIServerActivityEquipRewardsPanel.GetState.HasGet);
			recObj.CustomActive(mData.xuanShangType == (int)UIServerActivityEquipRewardsPanel.GetState.CanGet);
			goObj.CustomActive(mData.xuanShangType == (int)UIServerActivityEquipRewardsPanel.GetState.NotGet);
			if(recObj.activeSelf)
				CSEffectPlayMgr.Instance.ShowUIEffect(effObj, 17903);
		}

		uiGrid.MaxCount = itemList.Count;
		for (int i = 0; i < uiGrid.MaxCount; i++)
		{
			if(itemBaseList.Count < i+1)
			{
				itemBaseList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, uiGrid.controlList[i].transform, itemSize.Size66));
			}
			itemBaseList[i].Refresh(itemList[i]);
			itemBaseList[i].SetCount(numList[i], Color.white);
		}
		itemBase.Refresh(itemId);
	}
	private void OnBtnReceive(GameObject _go)
	{
		Net.ReqSpecialActivityRewardMessage(mData.activityId, mData.id);
	}
	private void OnBtnGo(GameObject _go)
	{
		if (mData.isWoLong)
			wayStr = SundryTableManager.Instance.GetSundryEffect(1096);
		else
			wayStr = SundryTableManager.Instance.GetSundryEffect(1097);
		UtilityPanel.ShowCompleteWayWithSelfAdapt(wayStr, goObj.GetComponent<UISprite>());
	}
	public override void OnDestroy()
	{
		if (itemBaseList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList);
			itemBaseList = null;
		}
		if (itemBase != null) { UIItemManager.Instance.RecycleSingleItem(itemBase); itemBase = null; }
		if (effObj != null) { CSEffectPlayMgr.Instance.Recycle(effObj); effObj = null; }
		targetTrs = null;
		uiGrid = null;
		lb_name = null;
		lb_item = null;
		mData = null;
		itemList = null;
		numList = null;
		rewards = null;
		box = null;
		itemTab = null;
		getObj = null;
		recObj = null;
		goObj = null;
		if(recBtn != null)
		{
			recBtn.onClick = null;
			recBtn = null;
		}
		if(goBtn != null)
		{
			goBtn.onClick = null;
			goBtn = null;
		}
		itemId = 0;
		quality = 0;
		boxId = 0;
		targetMes = string.Empty;
		wayStr = string.Empty;
	}
}