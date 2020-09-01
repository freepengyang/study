using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UIMaFaPanel : UIBasePanel
{
	ILBetterList<MaFaData> dataList;
	ILBetterList<MaFaItem> itemList;
	MaFaActiveData activeData;
	int keyId = 0;
	long leftTime = 0;
	long keyHave = 0;
	int keyNeed = 0;
	int min = 0;
	string countDownStr = "";
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	public enum ItemState
	{
		None = 0 ,		//默认状态都不显示
		CanGet  = 1,	//可获得，但未点击
		Get = 2,		//已获得
		NotReach = 3,	//未达成
		Locked = 4,		//锁定的
	}
	public override void Init()
	{
		base.Init();
		mCloseBtn.onClick = OnBtnClose;
		mBtnWant.onClick = OnBtnWant;
		mBtnOpen.onClick = OnBtnOpen;
		mBtnOneKey.onClick = OnBtnOneKey;
		mBtnBuy.onClick = OnBtnBuy;
		mBtnHelp.onClick = OnBtnHelp;
		mAdvancedBox.onClick = OnAdvancedBox;
		mBtnAdd.onClick = OnItemAddClick;
		UIEventListener.Get(mBoxEffectObj).onClick = ShowPreviewBox;
		UIEventListener.Get(mKeyIcon.gameObject).onClick = OnItemAddClick;

		mClientEvent.AddEvent(CEvent.RefreshMaFaLayerInfo, RefreshMaFaLayerInfo);
		mClientEvent.AddEvent(CEvent.RefreshMaFaBoxInfo, RefreshMaFaBoxInfo);
		RegisterRed(mBoxRedPoint, RedPointType.MaFaBox);
		RegisterRed(mOneKeyRedPoint, RedPointType.MaFaRewards);

		SetMoneyIds(1, 4);
	}
	public override void Show()
	{
		base.Show();
		RefreshCommonData();
		RefreshLeft();
		RefreshRightUpUI();
		RefreshItem();
	}
	private void RefreshMaFaLayerInfo(uint id, object data)
	{
		RefreshCommonData();
		RefreshLeft();
		RefreshRightUpUI();
		RefreshItem();
	}
	private void RefreshMaFaBoxInfo(uint id, object data)
	{
		RefreshCommonData();
		RefreshLeft();
	}
	#region 通用数据，避免重复读表
	private void RefreshCommonData()
	{
		if(keyNeed <=0)
		{
			keyNeed = CSMaFaInfo.Instance.GetKeyNeed();
			keyId = CSMaFaInfo.Instance.GetKeyId();
		}
		activeData = CSMaFaInfo.Instance.GetAcitveData();
		keyHave = keyId.GetItemCount();
		dataList = CSMaFaInfo.Instance.GetItemDataList();
		min = CSMaFaInfo.Instance.GetMinLayer();
		min = min > dataList.Count - 6 ? dataList.Count - 6 : min;
	}
	#endregion

	#region 左边界面
	private void RefreshLeft()
	{
		ColorType color = keyHave >= keyNeed ? ColorType.Green:ColorType.Red;
		string activeName = CSMaFaInfo.Instance.GetAcitveName();
		mKeyValue.text = $"{keyHave}/{keyNeed}".BBCode(color);
		mLbBoxName.text = CSString.Format(1799, activeData.boxLevel);
		mKeyIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(keyId)}";
		//mSpTitle.spriteName = activeName;
		int num = CSMaFaInfo.Instance.GetLeftNum();
		//大宝箱
		if (num == 1)
			CSEffectPlayMgr.Instance.ShowUIEffect(mBoxEffectObj, 17820);
		//小宝箱
		else
			CSEffectPlayMgr.Instance.ShowUIEffect(mBoxEffectObj, 17821);
		mBoxTip.text = CSString.Format(1795,num-1);//(1-9)-1次
	}
	#endregion

	#region 右上角界面
	private void RefreshRightUpUI()
	{
		int maxExp = CSMaFaInfo.Instance.GetMaxExp();
		int exp = activeData.exp;
		float fillAmount = 0;
		if (exp > 0 && exp < maxExp)
			fillAmount = (float)exp / maxExp;
		else if (exp >= maxExp)
			fillAmount = 1f;
		mLbRate.text = $"{CSString.Format(1792)}{exp}/{maxExp}".BBCode(ColorType.MainText);
		mLbLayer.text = activeData.layer.ToString();
		mSliderExp.value = fillAmount;
		leftTime = CSMaFaInfo.Instance.GetActiveTime();
		mCSInvoke?.StopInvokeRepeating();
		mCSInvoke?.InvokeRepeating(0f, 1f, CountDown);
		CSEffectPlayMgr.Instance.ShowUITexture(mMaFaTitleBg, "mafa_title_bg");
	}
	private void CountDown()
	{
		if (leftTime < 0)
			mCSInvoke?.StopInvokeRepeating();
		else
		{
			countDownStr = CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1);
			mLbTime.text = CSString.Format(417, countDownStr);
			leftTime--;
		}
	}
	#endregion

	#region 右下角界面
	private void RefreshItem()
	{
		mObjLock.CustomActive(activeData.superUnlock != 1);
		mBtnBuy.gameObject.CustomActive(activeData.superUnlock != 1);
		//设置最小序号的奖励
		GameObject go;
		MaFaData data;
		MaFaItem binder;
		mGridWrap.minIndex = 0 - min;
		mGridWrap.maxIndex = dataList.Count- min-1;
		mGridWrap.cullContent = false;
		mGridWrap.enabled = true;
		mRewardsGrid.MaxCount = 6;
		if (itemList == null)
			itemList = mPoolHandleManager.GetSystemClass<ILBetterList<MaFaItem>>();
		mGridWrap.ResetChildPositions();
		mRewardsView.ResetPosition();
		for (int i = 0; i < mRewardsGrid.MaxCount; i++)
		{
			if(itemList.Count < mRewardsGrid.MaxCount)
			{
				go = mRewardsGrid.controlList[i];
				binder = go.GetOrAddBinder<MaFaItem>(mPoolHandleManager);
				itemList.Add(binder);
			}
			data = dataList[i + min];
			itemList[i].Bind(data);
		}
		mGridWrap.onInitializeItem = ShowRewardInfo;
	}
	private void ShowRewardInfo(GameObject go, int index, int realIndex)
	{
		itemList[index].Bind(dataList[realIndex + min]);
	}
	#endregion

	#region 按钮
	private void OnBtnClose(GameObject _go)
	{
		Close();
	}
	private void OnBtnWant(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UIMaFaGetWayPanel>();
	}
	private void OnBtnOpen(GameObject _go)
	{
		if(keyHave < keyNeed)
			Utility.ShowGetWay(keyId);
		else
			Net.ReqMafaBoxRewardMessage();
	}
	private void OnBtnOneKey(GameObject _go)
	{
		bool isSend = false;
		for(int i=0;i< dataList.Count;i++)
		{
			if (dataList[i].state1 == (int)ItemState.CanGet ||
				dataList[i].state2 == (int)ItemState.CanGet)
			{
				isSend = true;
				break;
			}
		}
		if(isSend)
			Net.ReqMafaLayerRewardMessage(0,0,1);
		else
			UtilityTips.ShowRedTips(1828);
	}
	private void OnBtnBuy(GameObject _go)
	{
		if(activeData.superUnlock != 1)
		{
			UIManager.Instance.CreatePanel<UIMaFaPromptPanel>(p =>
			{
				(p as UIMaFaPromptPanel).RefreshRewardsUI((int)UIMaFaPromptPanel.State.advanced);
			});
		}
	}
	private void OnBtnHelp(GameObject _go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.MaFa);
	}
	private void OnAdvancedBox(GameObject _go)
	{
		if(activeData.superUnlock != 1)
			UtilityTips.ShowTips(1861);
	}
	private void ShowPreviewBox(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UIMaFaPromptPanel>(p =>
		{
			(p as UIMaFaPromptPanel).RefreshRewardsUI((int)UIMaFaPromptPanel.State.preview);
		});
	}
	private void OnItemAddClick(GameObject _go)
	{
		Utility.ShowGetWay(keyId);
	}
	#endregion

	protected override void OnDestroy()
	{
		mCSInvoke?.StopInvokeRepeating();
		if (mBoxEffectObj != null)CSEffectPlayMgr.Instance.Recycle(mBoxEffectObj);
		if (mMaFaTitleBg != null) CSEffectPlayMgr.Instance.Recycle(mMaFaTitleBg);
		mRewardsGrid.UnBind<MaFaItem>();
		itemList = null;
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.RefreshMaFaLayerInfo, RefreshMaFaLayerInfo);
			mClientEvent.RemoveEvent(CEvent.RefreshMaFaBoxInfo, RefreshMaFaBoxInfo);
		}
		keyId = 0;
		leftTime = 0;
		keyHave = 0;
		keyNeed = 0;
		min = 0;
		countDownStr = "";
		base.OnDestroy();
	}
	
	private class MaFaItem : UIBinder
	{
		MaFaData mData;
		UIItemBase itemBase1, itemBase2;
		UILabel lbLayer;
		Transform nItemTarget, aItemTarget;
		GameObject normalP, nYes, nSelect, nMask, advancedP, aYes, aSelect, aMask,bg;
		UISprite sp_aSelect, sp_nSelect;
		Action<UIItemBase> action1;
		Action<UIItemBase> action2;
		public override void Init(UIEventListener handle)
		{
			lbLayer = Get<UILabel>("lb_layer");
			normalP = Get<GameObject>("normal");
			nItemTarget = Get<Transform>("normal/itemTarget");
			nYes = Get<GameObject>("normal/yes");
			nSelect = Get<GameObject>("normal/select");
			nMask = Get<GameObject>("normal/mask");

			advancedP = Get<GameObject>("advanced");
			aItemTarget = Get<Transform>("advanced/itemTarget");
			aYes = Get<GameObject>("advanced/yes");
			aSelect = Get<GameObject>("advanced/select");
			aMask = Get<GameObject>("advanced/mask");
			bg = Get<GameObject>("bg");

			sp_aSelect = aSelect.GetComponent<UISprite>();
			sp_nSelect = nSelect.GetComponent<UISprite>();

			SetItemBase(ref itemBase1, nItemTarget);
			SetItemBase(ref itemBase2, aItemTarget);

			CSEffectPlayMgr.Instance.ShowUITexture(bg, "mafa_scroll1");

			UIEventListener.Get(normalP, 1).onClick = OnClickItem;
			UIEventListener.Get(advancedP, 2).onClick = OnClickItem;
		}
		public override void Bind(object data)
		{
			mData = data as MaFaData;
			if (mData == null) return;
			if (action1 == null)
				action1 = ItemClick1;
			if (action2 == null)
				action2 = ItemClick2;
			bool effect1 = mData.state1 == (int)ItemState.CanGet;
			bool effect2 = mData.state2 == (int)ItemState.CanGet;
			lbLayer.text = CSString.Format(mData.layerStr, mData.layer);
			RefreshItemBase(itemBase1, mData.id1, mData.num1, effect1, action1);
			RefreshItemBase(itemBase2, mData.id2, mData.num2, effect2, action2);
			RefershState(nYes, nSelect, sp_nSelect, nMask, mData.state1);
			RefershState(aYes, aSelect, sp_aSelect, aMask, mData.state2, mData.state1);
		}
		private void SetItemBase(ref UIItemBase itemBase, Transform parentTrs)
		{
			if (itemBase == null)
				itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, parentTrs, itemSize.Size50);
		}
		private void RefreshItemBase(UIItemBase itemBase,int id,int num,bool isEffect , Action<UIItemBase> action)
		{
			itemBase.Refresh(id, action, false);
			itemBase.SetCount(num);
		}
		private void RefershState(GameObject yes, GameObject select, UISprite sp_select, GameObject mask,int state,int specialState = 0)
		{
			yes.CustomActive(state == (int)ItemState.Get);
			select.CustomActive(isShowAdvancedEffect(state,specialState));
			mask.CustomActive(state == (int)ItemState.Get || state == (int)ItemState.Locked);
			if (isShowAdvancedEffect(state, specialState))
			{
				CSEffectPlayMgr.Instance.ShowUIEffect(select, 17929);
				sp_select.width = (int)itemSize.Size50 + 52;
				sp_select.height = (int)itemSize.Size50 + 52;
			}
		}
		private bool isShowAdvancedEffect( int state,int specialState)
		{
			if (state == (int)ItemState.CanGet)
				return true;
			else if (state == (int)ItemState.Locked)
			{
				return specialState == (int)ItemState.CanGet ||
				specialState == (int)ItemState.Get;
			}
			else
				return false;
		}
		#region 点击处理
		private void ItemClick1(UIItemBase item)
		{
			if (item != null)
				ClickDeal(1);
		}
		private void ItemClick2(UIItemBase item)
		{
			if (item != null)
				ClickDeal(2);
		}
		private void OnClickItem(GameObject _go)
		{
			int btnIdx = (int)UIEventListener.Get(_go).parameter;
			ClickDeal(btnIdx);
		}
		private void ClickDeal(int btnIdx)
		{
			if(btnIdx == 1)
				ClickDeal(mData.state1,mData.id1,0);
			else if(btnIdx == 2)
				ClickDeal(mData.state2, mData.id2, 1, mData.state1);
		}
		private void ClickDeal(int state,int id,int boxLevel,int specialState = 0)
		{
			TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
			switch (state)
			{
				case (int)ItemState.Get:
					UtilityTips.ShowRedTips(1798);
					break;
				case (int)ItemState.NotReach:
					UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfg);
					break;
				case (int)ItemState.CanGet:
					Net.ReqMafaLayerRewardMessage(boxLevel, mData.layer, 0);
					break;
				case (int)ItemState.Locked:
					if (specialState == (int)ItemState.CanGet || specialState == (int)ItemState.Get)
					{
						UIManager.Instance.CreatePanel<UIMaFaPromptPanel>(p =>
						{
							(p as UIMaFaPromptPanel).RefreshRewardsUI((int)UIMaFaPromptPanel.State.advanced);
						});
					}
					else
						UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfg);
					break;
			}
		}
		#endregion
		public override void OnDestroy()
		{
			if (itemBase1 != null) { UIItemManager.Instance.RecycleSingleItem(itemBase1); itemBase1 = null; }
			if (itemBase2 != null) { UIItemManager.Instance.RecycleSingleItem(itemBase2); itemBase2 = null; }
			if (nSelect != null) { CSEffectPlayMgr.Instance.Recycle(nSelect); nSelect = null; }
			if (aSelect != null) { CSEffectPlayMgr.Instance.Recycle(nSelect); aSelect = null; }
			if (bg != null) { CSEffectPlayMgr.Instance.Recycle(bg); bg = null; }
			lbLayer = null;
			normalP = null;
			nItemTarget = null;
			nYes = null;
			nMask = null;
			advancedP = null;
			aItemTarget = null;
			aYes = null;
			aMask = null;
			action1 = null;
			action2 = null;
			sp_aSelect = null;
			sp_nSelect = null;
		}
	}
}