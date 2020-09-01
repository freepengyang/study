using System;
using System.Collections.Generic;
using UnityEngine;
public partial class UITombTreasurePanel : UIBasePanel
{
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	int costId, costNeed, floor, floorId, floorType;
	bool isSurpriseAnim;
	long have1;
	ColorType lbColor1;
	string stoneTypeId;
	string[] sizeStr = { "4#4", "5#5", "5#6", "5#7", "4#6", "6#4", "6#6" };
	string[] posStr = { "0#34", "-34#70", "1#70", "-34#70", "1#33", "1#33", "1#34" };
	string[] colorMes;
	string[] stoneBgStr;
	GameObject eff_kickSingele, eff_colorSingele;
	Dictionary<string, string> openPosition = new Dictionary<string, string>();
	CSBetterLisHot<shizhenRewardsData> rareRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> surpriseTwoRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenTypeData> shizhenTypeList = new CSBetterLisHot<shizhenTypeData>();
	CSBetterLisHot<shizhenTypeData> shizhenNormalList = new CSBetterLisHot<shizhenTypeData>();
	List<UIItemBase> rareItemBaseList = new List<UIItemBase>();
	List<UIItemBase> surpriseTwoItemBaseList = new List<UIItemBase>();
	public override void Init()
	{
		base.Init();

		mLbCheckAll.onClick = onCheckBtn;
		mCloseBtn.onClick = OnCloseBtn;
		mIconTips.onClick = OnClickIconBtn;
		mBtnAdd.onClick = OnAddBtn;

		mClientEvent.AddEvent(CEvent.GetTombTreasureGridInfo, GetTombTreasureGridInfo);
		mClientEvent.AddEvent(CEvent.GetTombTreasureDoorInfo, GetTombTreasureDoorInfo);
		mClientEvent.AddEvent(CEvent.GetTombTreasureUpdateInfo, GetTombTreasureUpdateInfo);
		mClientEvent.AddEvent(CEvent.GetTombTreasureNormalInfo, GetTombTreasureNormalInfo);
		mClientEvent.AddEvent(CEvent.ItemListChange, ItemCostUpdate);

        SetMoneyIds(1, 4);
	}
	public override void Show()
	{
		base.Show();

		string[] mes = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(465));
		if (mes == null) return;
		costId = int.Parse(mes[0]);
		costNeed = int.Parse(mes[1]);
		if (colorMes == null) colorMes = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(516));
		if (colorMes == null) return;
		if (stoneBgStr == null) stoneBgStr = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(678));
		if (stoneBgStr == null) return;
		for (int i = 0; i < sizeStr.Length; i++)
		{
			if(openPosition.ContainsKey(sizeStr[i]))
				openPosition[sizeStr[i]] = posStr[i];
			else
				openPosition.Add(sizeStr[i], posStr[i]);
		}

		RefreshStone();
		ShowFreeNumOrItemCost();
		RefreshItemCost();
		RefreshItemGet();
		RefreshScrollViewPosition(mRareScrollView,mRareScrollBar);
		GetTombTreasureDoorInfo(0, null);
		GetTombTreasureNormalInfo(0, null);
		RegisterRed(mTombredpoint, RedPointType.TombTreasure);
		CSEffectPlayMgr.Instance.ShowUITexture(mTexBG, "tomb_bg");
	}
	private void RefreshScrollViewPosition(UIScrollView view,UIScrollBar bar)
	{
		if(bar.value > 0)
			view.ResetPosition();
	}
	private void RefreshStone()
	{
		floorId = CSStonetreasureInfo.Instance.GetFloorId();
		floor = CSStonetreasureInfo.Instance.GetFloor();
		floorType = ShiZhenTypeTableManager.Instance.GetShiZhenTypeType(floorId);
		isSurpriseAnim = CSStonetreasureInfo.Instance.GetIsSurpriseAnim();
		string size = ShiZhenTypeTableManager.Instance.GetShiZhenTypeShizhensize(floorId).ToString();
		string[] mesPosition = UtilityMainMath.StrToStrArr(openPosition[size]);
		string[] mes = UtilityMainMath.StrToStrArr(size);
		if (mesPosition == null || mes == null) return;
		int wNum, hNum, x, y, tombX, tombY;
		int.TryParse(mes[0], out wNum);
		int.TryParse(mes[1], out hNum);
		int.TryParse(mesPosition[0], out x);
		int.TryParse(mesPosition[1], out y);
		mStonesGrid.transform.localPosition = new Vector3(x, y, 0);
		mStonesGrid.MaxCount = wNum * hNum;
		mStonesGrid.MaxPerLine = hNum;
		shizhenTypeList = CSStonetreasureInfo.Instance.GetShiZhenTypeList();
		stoneTypeId = floorType == 3 ? stoneBgStr[1] : stoneBgStr[0];

		for (int i = 0; i < mStonesGrid.MaxCount; i++)
		{
			TombItem item = mPoolHandleManager.GetCustomClass<TombItem>();
			tombX = i % hNum;
			tombY = i / hNum;
			item.SetData(mStonesGrid.controlList[i], colorMes,
				tombX, tombY, costId, costNeed, stoneTypeId, shizhenTypeList, ItemClick);
			item.TombItemRecycle();
			item.Refresh();
		}
		//ÆÕÍ¨²ã
		if (floorType == 1 || floorType == 2)
			mFloorName.text = CSString.Format(1024, floor).BBCode(ColorType.MainText);
		//¾ªÏ²²ã
		if (floorType == 3)
			mFloorName.text = CSString.Format(1025, floor).BBCode(ColorType.MainText);
		//×î¸ß²ã
		if (floor == -4)
			mFloorName.text = CSString.Format(1026).BBCode(ColorType.Red);
		//¾ªÏ²²ãÌØÐ§
		if (floorType >= 3 && isSurpriseAnim)
			CSEffectPlayMgr.Instance.ShowUIEffect(mSp_hint, 17352, 10, false);
	}
	private void ItemClick(TombItem item)
	{
		item.ClickEffect();
	}
	private void ItemCostUpdate(uint id, object data)
	{
		RefreshItemCost();
	}
	private void ShowFreeNumOrItemCost()
	{
		int freeNum = CSStonetreasureInfo.Instance.GetFreeNum();
		string tips = ClientTipsTableManager.Instance.GetClientTipsContext(1074);
		if (floorType >= 3 && freeNum > 0)
		{
			mLbFreeNum.gameObject.CustomActive(true);
			mLbFreeNum.text = CSString.Format(tips, freeNum);
			mUIItem.CustomActive(false);
		}
		else
		{
			mLbFreeNum.gameObject.CustomActive(false);
			mUIItem.CustomActive(true);
		}
	}
	private void RefreshItemCost()
	{
		
		have1 = costId.GetItemCount();
		lbColor1 = have1 >= costNeed ? ColorType.Green : ColorType.Red;
		mspIcon.spriteName = ItemTableManager.Instance.GetItemIcon(costId);
		mLbValue.text = $"{have1}/{costNeed}".BBCode(lbColor1);
	}
	private void OnAddBtn(GameObject _go)
	{
		Utility.ShowGetWay(costId);
	}
	private void OnClickIconBtn(GameObject _go)
	{
		UITipsManager.Instance.CreateTips(TipsOpenType.Normal, costId);
	}
	//Ï¡ÓÐ½±Àø
	private void RefreshItemGet()
	{
		rareRewards = CSStonetreasureInfo.Instance.GetRareRewards();
		RefreshGet(rareItemBaseList, rareRewards, mRareGrid, mRareScrollView, mRareScrollBar, mRareDirection.transform);
		RefreshSpecialGet();
	}
	//ÌØÊâ/¾ªÏ²½±Àø
	private void RefreshSpecialGet()
	{
		if (mSpecialLayer.activeSelf && floorType < 2)
			RefreshScrollViewPosition(mSpecialScrollView,mSpecialScrollBar);
		mSpecialLayer.CustomActive(floorType >= 2);
		if (floorType >= 2)
		{
			surpriseTwoRewards = CSStonetreasureInfo.Instance.GetSurpriseTwoRewards();
			RefreshGet(surpriseTwoItemBaseList, surpriseTwoRewards, mSpecialGrid, mSpecialScrollView, mSpecialScrollBar, mSpecialDirection.transform);
		}
	}
	//Ë¢ÐÂ½±Àø£¬itemÁÐ±í£¬½±ÀøÁÐ±í£¬grid£¬view,bar,¼ýÍ·µÄ¸¸ÎïÌå
	private void RefreshGet(List<UIItemBase> itemList, CSBetterLisHot<shizhenRewardsData> rewards,
		UIGridContainer grid, UIScrollView view, UIScrollBar bar, Transform trs)
	{
		Transform itemFlagGet, itemFlag;
		Color textColor;
		InitGrid(itemList, rewards, view, grid, bar, trs);
		for (int i = 0; i < grid.MaxCount; i++)
		{
			grid.controlList[i].CustomActive(i < rewards.Count);
			if (i < rewards.Count)
			{
				itemFlag = grid.controlList[i].transform.GetChild(0);
				itemFlagGet = grid.controlList[i].transform.GetChild(2);
				itemList[i].Refresh(rewards[i].itemId);
				textColor = rewards[i].getNum >= rewards[i].sum ? CSColor.red : CSColor.green;
				if(rewards[i].getNum < rewards[i].sum)
					itemList[i].SetCount($"{rewards[i].sum - rewards[i].getNum}/{rewards[i].sum}", textColor);
				itemFlagGet.gameObject.CustomActive(rewards[i].getNum >= rewards[i].sum);
				itemFlag.gameObject.CustomActive(rewards[i].isShow);
			}
		}
	}
	private void InitGrid(List<UIItemBase> itemList, CSBetterLisHot<shizhenRewardsData> rewards,
		UIScrollView view, UIGridContainer grid, UIScrollBar bar, Transform trs)
	{
		Transform parentTrs;
		UIItemBase item;
		bar.onChange.Clear();
		SetLeftRightHide(trs);
		view.SetDynamicArrowHorizontal(trs.GetChild(1).gameObject, trs.GetChild(0).gameObject);
		if (itemList.Count >= rewards.Count) return;

		grid.MaxCount = rewards.Count;
		for (int i = itemList.Count; i < grid.MaxCount; i++)
		{
			parentTrs = grid.controlList[i].transform.GetChild(1);
			item = UIItemManager.Instance.GetItem(PropItemType.Normal, parentTrs, itemSize.Size66);
			itemList.Add(item);
		}
	}
	private void SetLeftRightHide(Transform trs)
	{
		for (int i = 0; i < trs.childCount; i++)
		{
			trs.GetChild(i).gameObject.CustomActive(false);
		}
	}
	private void GetTombTreasureGridInfo(uint id, object data)
	{
		shizhenTypeData szTypeSingleData = CSStonetreasureInfo.Instance.GetStoneLocationMessage();
		SetClickUI(szTypeSingleData);
		ShowFreeNumOrItemCost();
		RefreshItemGet();
	}
	private void GetTombTreasureDoorInfo(uint id, object data)
	{
		shizhenTypeData szTypeDoorData = CSStonetreasureInfo.Instance.GetDownLocationMessage();
		if (szTypeDoorData.nextFloor > 0)
		{
			SetClickUI(szTypeDoorData);
		}
	}
	private void SetClickUI(shizhenTypeData szData)
	{
		int index = szData.y * mStonesGrid.MaxPerLine + szData.x;
		int colorid, itemNum, nextFloor, roundNum;
		string iconId;
		if (index < mStonesGrid.MaxCount)
		{
			GameObject go = mStonesGrid.controlList[index];
			GameObject sp_mask = go.transform.Find("sp_mask").gameObject;
			UISprite sp_bg = go.transform.Find("sp_bg").GetComponent<UISprite>();
			UISprite sp_icon = go.transform.Find("sp_icon").GetComponent<UISprite>();
			UILabel lb_num = go.transform.Find("lb_num").GetComponent<UILabel>();
			UILabel lb_round = go.transform.Find("lb_round").GetComponent<UILabel>();
			eff_kickSingele = go.transform.Find("eff_kick").gameObject;
			eff_colorSingele = go.transform.Find("eff_color").gameObject;

			iconId = szData.iconId;
			colorid = szData.color;
			itemNum = szData.itemNum;
			nextFloor = szData.nextFloor;
			roundNum = szData.roundNum;

			if (!string.IsNullOrEmpty(iconId))
				sp_icon.spriteName = iconId;
			if (itemNum > 1)
				lb_num.text = itemNum.ToString();
			if (roundNum >= 0)
				lb_round.text = roundNum.ToString();
			if (colorid >= 0)
			{
				if (sp_bg.spriteName == stoneTypeId || sp_mask.activeSelf)
				{
					CSEffectPlayMgr.Instance.ShowUIEffect(eff_kickSingele, 17351, 10, false);
				}
				sp_bg.spriteName = colorMes[colorid];
			}

			if (nextFloor > 0)
			{
				UIAtlas doorAtlas = eff_colorSingele.transform.GetComponent<UISprite>().atlas;
				if (doorAtlas == null || !CSEffectPlayMgr.Instance.IsContains(eff_colorSingele.GetHashCode()))
					CSEffectPlayMgr.Instance.ShowUIEffect(eff_colorSingele, 17350);
			}
			sp_mask.CustomActive(false);
		}
	}
	private void GetTombTreasureUpdateInfo(uint id, object data)
	{
		RefreshStone();
		ShowFreeNumOrItemCost();
		RefreshItemGet();
		GetTombTreasureDoorInfo(0, null);
		GetTombTreasureNormalInfo(0, null);
	}
	//ÆÕÍ¨½±ÀøÏÔÊ¾
	private void GetTombTreasureNormalInfo(uint id, object data)
	{
		shizhenNormalList = CSStonetreasureInfo.Instance.GetShiZhenNormalList();
		int colorid, itemNum, index;
		shizhenTypeData szData;
		GameObject go, sp_mask;
		UISprite sp_bg, sp_icon, sp_maskSet;
		UILabel lb_num,lb_round;
		for (int i = 0; i < shizhenNormalList.Count; i++)
		{
			szData = shizhenNormalList[i];
			index = szData.y * mStonesGrid.MaxPerLine + szData.x;
			if (index < mStonesGrid.MaxCount)
			{
				go = mStonesGrid.controlList[index];
				sp_bg = go.transform.Find("sp_bg").GetComponent<UISprite>();
				sp_icon = go.transform.Find("sp_icon").GetComponent<UISprite>();
				lb_num = go.transform.Find("lb_num").GetComponent<UILabel>();
				sp_mask = go.transform.Find("sp_mask").gameObject;
				lb_round = go.transform.Find("lb_round").GetComponent<UILabel>();
				sp_maskSet = sp_mask.GetComponent<UISprite>();

				colorid = szData.color;
				itemNum = szData.itemNum;
				if (!string.IsNullOrEmpty(szData.iconId) || szData.id == 0)
				{
					sp_mask.CustomActive(true);
					sp_maskSet.spriteName = stoneTypeId;
					sp_icon.spriteName = szData.iconId;
					lb_round.text = szData.roundNum >= 0 ? szData.roundNum.ToString() : "";
					var alphaTween = sp_mask.GetComponent<TweenAlpha>();
					alphaTween?.PlayForward();
				}
				lb_num.text = itemNum > 1 ? itemNum.ToString() : "";
				sp_bg.spriteName = colorid >= 0 ? colorMes[colorid] : "";
			}
		}
	}
	private void OnCloseBtn(GameObject _go)
	{
		Close();
	}
	
	private void onCheckBtn(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UITombTreasureTipsPanel>();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetTombTreasureGridInfo, GetTombTreasureGridInfo);
			mClientEvent.RemoveEvent(CEvent.GetTombTreasureDoorInfo, GetTombTreasureDoorInfo);
			mClientEvent.RemoveEvent(CEvent.GetTombTreasureUpdateInfo, GetTombTreasureUpdateInfo);
			mClientEvent.RemoveEvent(CEvent.GetTombTreasureNormalInfo, GetTombTreasureNormalInfo);
			mClientEvent.RemoveEvent(CEvent.ItemListChange, ItemCostUpdate);
		}
		CSEffectPlayMgr.Instance.Recycle(mTexBG);
		CSEffectPlayMgr.Instance.Recycle(mSp_hint);
		if(rareItemBaseList!=null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(rareItemBaseList);
			rareItemBaseList.Clear();
			rareItemBaseList = null;
		}
		if (surpriseTwoItemBaseList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(surpriseTwoItemBaseList);
			surpriseTwoItemBaseList.Clear();
			surpriseTwoItemBaseList = null;
		}
		
		CSEffectPlayMgr.Instance.Recycle(eff_kickSingele);
		CSEffectPlayMgr.Instance.Recycle(eff_colorSingele);
		mTexBG = null;
		mSp_hint = null;
		eff_kickSingele = null;
		eff_colorSingele = null;
		openPosition = null;
		colorMes = null;
		
		sizeStr = null;
		posStr = null;
		stoneBgStr = null;

		isSurpriseAnim = false;
		have1 = 0;
		lbColor1 = 0;
		costId = 0;
		costNeed = 0;
		floor = 0;
		floorId = 0;
		floorType = 0;
		stoneTypeId = "";
	}
}
public class TombItem : IDispose
{
	GameObject go;
	//±³¾°Í¼Æ¬£¬µÀ¾ßÍ¼Æ¬£¬´òËéÌØÐ§Å¶£¬½¥Òþ½¥ÏÖ
	UISprite sp_bg, sp_icon;
	GameObject eff_kick, eff_color, sp_mask;
	UILabel lb_num, lb_round;
	CSBetterLisHot<shizhenTypeData> shizhenTypeList;
	shizhenTypeData szDoorData;
	Action<TombItem> action;
	TweenAlpha alphaTween;
	string[] colorMes;
	int tombX, tombY, costId, costNeed;
	string stoneTypeIdStr;
	public void SetData(GameObject _go,string[] _colorMes, 
		int _x,int _y,int _costId,int _costNeed,string _stoneTypeId,
		CSBetterLisHot<shizhenTypeData> _shizhenTypeList,Action<TombItem> _action = null)
	{
		go = _go;
		action = _action;
		colorMes = _colorMes;
		tombX = _x;
		tombY = _y;
		costId = _costId;
		costNeed = _costNeed;
		stoneTypeIdStr = _stoneTypeId;
		shizhenTypeList = _shizhenTypeList;
		InitComponent();
	}
	void InitComponent()
	{
		sp_bg = go.transform.Find("sp_bg").GetComponent<UISprite>();
		sp_icon = go.transform.Find("sp_icon").GetComponent<UISprite>();
		eff_kick = go.transform.Find("eff_kick").gameObject;
		eff_color = go.transform.Find("eff_color").gameObject;
		lb_num = go.transform.Find("lb_num").GetComponent<UILabel>();
		sp_mask = go.transform.Find("sp_mask").gameObject;
		lb_round = go.transform.Find("lb_round").GetComponent<UILabel>();
		alphaTween = sp_mask.GetComponent<TweenAlpha>();
	}
	public void TombItemRecycle()
	{
		sp_bg.spriteName = stoneTypeIdStr;
		sp_icon.spriteName = "";
		RecycleEffect(eff_kick);
		RecycleEffect(eff_color);
		sp_mask.CustomActive(false);
		lb_num.text = "";
		lb_round.text = "";
		alphaTween?.PlayReverse();
	}
	private void RecycleEffect(GameObject _go)
	{
		UIAtlas doorAtlas = _go.GetComponent<UISprite>().atlas;
		if (doorAtlas != null)
			CSEffectPlayMgr.Instance.Recycle(_go);
	}
	public void Refresh()
	{
		int x, y, colorid, itemNum, nextFloor, id, roundNum;
		string iconId;
		int idx = -1;
		if (colorMes == null) return;
		for (int i = 0; i < shizhenTypeList.Count; i++)
		{
			x = shizhenTypeList[i].x;
			y = shizhenTypeList[i].y;
			if (tombX == x && tombY == y)
			{
				idx = i;
				break;
			}
		}
		if(idx >= 0)
		{
			id = shizhenTypeList[idx].id;
			iconId = shizhenTypeList[idx].iconId;
			colorid = shizhenTypeList[idx].color;
			itemNum = shizhenTypeList[idx].itemNum;
			nextFloor = shizhenTypeList[idx].nextFloor;
			roundNum = shizhenTypeList[idx].roundNum;
			if (!string.IsNullOrEmpty(iconId))
				sp_icon.spriteName = iconId;
			if (itemNum > 1)
				lb_num.text = itemNum.ToString();
			if (roundNum >= 0)
				lb_round.text = roundNum.ToString();
			if (nextFloor > 0)
			{
				UIAtlas doorAtlas = eff_color.transform.GetComponent<UISprite>().atlas;
				if (doorAtlas == null)
					CSEffectPlayMgr.Instance.ShowUIEffect(eff_color, 17350);
			}

			if (colorid >= 0)
				sp_bg.spriteName = colorMes[colorid];
			else
			{
				if (id == -5 || roundNum >= 0)
					sp_bg.spriteName = "";
				else
					sp_bg.spriteName = stoneTypeIdStr;
			}
		}
		UIEventListener.Get(go).onClick = Click;
	}
	private void ConfirmBtnClickMoveNext()
	{
		TombItemRecycle();
		CSStonetreasureInfo.Instance.ClearDownLocationMessage();
		CSStonetreasureInfo.Instance.GetShiZhenNormalList().Clear();
		Net.CSDownStoneMessage();
	}
	public void ClickEffect()
	{
		long have = costId.GetItemCount();
		int itemId = 0;
		bool isClicked = false;
		bool isNext = false;
		int floor = CSStonetreasureInfo.Instance.GetFloor();
		shizhenTypeList = CSStonetreasureInfo.Instance.GetShiZhenTypeList();
		szDoorData = CSStonetreasureInfo.Instance.GetDownLocationMessage();
		ClickedData(shizhenTypeList, ref isClicked, ref itemId, ref isNext);
		if(floor != -4)
		{
			if (isNext)
			{
				if (CSStonetreasureInfo.Instance.IsMoveTips())
					UtilityTips.ShowPromptWordTips(63, ConfirmBtnClickMoveNext);
				else
					ConfirmBtnClickMoveNext();
			}
			else
			{
				if (!isClicked)
				{
					if (CSStonetreasureInfo.Instance.GetFreeNum() > 0)
					{
						CSStonetreasureInfo.Instance.SetFreeNum();
						Net.CSStoneLocationMessage($"{tombX}#{tombY}");
					}
					else
					{
						if (have >= costNeed)
						{
							Net.CSStoneLocationMessage($"{tombX}#{tombY}");
						}
							
						else
							Utility.ShowGetWay(costId);
					}
				}
				else
				{
					if (itemId > 0)
						UITipsManager.Instance.CreateTips(TipsOpenType.Normal, ItemTableManager.Instance.GetItemCfg(itemId));
				}
			}
		}
	}
	void ClickedData(CSBetterLisHot<shizhenTypeData> shizhenTypeList, ref bool isClicked, ref int itemId, ref bool isNext)
	{
		int x, y;
		for (int i = 0; i < shizhenTypeList.Count; i++)
		{
			x = shizhenTypeList[i].x;
			y = shizhenTypeList[i].y;
			if (x == tombX && y == tombY)
			{
				isClicked = true;
				itemId = shizhenTypeList[i].itemId;
				if (shizhenTypeList[i].nextFloor > 0)
				{
					isNext = true;
					return;
				}
			}
		}
		if (tombX == szDoorData.x && tombY == szDoorData.y)
			isNext = true;
		else
			isNext = false;
	}
	void Click(GameObject _go)
	{
		if (action != null)
		{
			action(this);
		}
	}
	public void Dispose()
	{
		RecycleEffect(eff_kick);
		RecycleEffect(eff_color);
		go = null;
		sp_bg = null;
		sp_icon = null;
		eff_kick = null;
		eff_color = null;
		sp_mask = null;
		lb_num = null;
		lb_round = null;
		action = null;
		colorMes = null;
		alphaTween = null;
		tombX = 0;
		tombY = 0;
		costId = 0;
		costNeed = 0;
		stoneTypeIdStr = "";
	}
}