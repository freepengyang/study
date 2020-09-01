using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UIWarSoulPanel : UIBasePanel
{
	CSBetterLisHot<PetShowItem> petIconList = new CSBetterLisHot<PetShowItem>();
	PetBaseInfoData petInfoData = new PetBaseInfoData();
	CSBetterLisHot<PetBaseSkillData> activeList;
	CSBetterLisHot<PetBaseSkillData> petPassiveSkillList;
	CSBetterLisHot<PetBasePropInfoData> allList;
	PetBaseSkillData specialSkill;
	PetShowItem curPetItem;
	GameObject curCheckMark;
	Vector3? mViewPos;
	int itemId = 0;        //组件id
	int maxSuitId = 0;    //zhanHunSuit,Id最大值
	int countDown = 0;  //战宠死亡倒计时

	public override void Init()
	{
		base.Init();
		mBtnDesc.onClick = OnBtnDesc;
		mHelpBtn.onClick = OnBtnHelp;
		mBtnLvUp.onClick = OnBtnLvUp;

		UIEventListener.Get(mAttBtn, 1).onClick = ShowTypeChange;
		UIEventListener.Get(mSkillBtn, 2).onClick = ShowTypeChange;

		mClientEvent.AddEvent(CEvent.GetWarPetBaseInfo, RefreshWarPetBaseInfoUI);
		RegisterRed(mLvRedPoint, RedPointType.PetLevelUp);
	}
	public override void Show()
	{
		base.Show();
		Net.CSWoLongPetInfoMessage();//请求战宠信息
		CSEffectPlayMgr.Instance.ShowUITexture(mTexBG, "pet_bg1");
	}
	private void RefreshWarPetBaseInfoUI(uint id, object data)
	{
		RefreshData();
		RefreshPetIcon();
		RefreshPetLvInfo();
	}
	private void RefreshData()
	{
		int suitIdx;
		petInfoData = CSPetBasePropInfo.Instance.GetPetInfoData();
		itemId = itemId > 0 ? itemId : petInfoData.suitId;
		suitIdx = ZhanHunSuitTableManager.Instance.array.gItem.id2offset.Count;
		maxSuitId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitId(suitIdx);
	}
	/// <summary>
	/// 刷新战宠头像
	/// </summary>
	void RefreshPetIcon()
	{
		petIconList.Clear();
		mLeftGrid.MaxCount = ZhanHunSuitTableManager.Instance.array.gItem.id2offset.Count;
		for (int i = 0, max = mLeftGrid.MaxCount; i < max; i++)
		{
			int petid = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(i + 1);
			PetShowItem petShowItem = mPoolHandleManager.GetCustomClass<PetShowItem>();
			petIconList.Add(petShowItem);
			petIconList[i].SetData(mLeftGrid.controlList[i], ItemClick);
			petIconList[i].RefreshByPetId(petid, i + 1);
			petIconList[i].RefreshFlagIcon(i <= (petInfoData.suitId - 1), i == (petInfoData.suitId - 1));
		}
		mLeftScrollView.ResetPosition();
		if (itemId > 0)
		{
			ItemClick(mLeftGrid.controlList[itemId - 1]);

			float height = mLeftGrid.CellHeight;
			float index = (itemId - 1);
			float max = mLeftGrid.MaxCount - 6;
			index = index > max ? max : index;
			if (mViewPos == null)
				mViewPos = mLeftScrollView.transform.localPosition;
			Vector3 vec = (Vector3)mViewPos + Vector3.up * index * height;
			SpringPanel.Begin(mLeftScrollView.gameObject, vec, 10);
		}
		else
			ItemClick(mLeftGrid.controlList[0]);
		mLeftScrollView.SetDynamicArrowVertical(mLeftDownIcon);
		ShowTypeChange(mAttBtn);
	}
	/// <summary>
	/// 刷新战宠等级显示
	/// </summary>
	private void RefreshPetLvInfo()
	{
		int suitId = petInfoData.suitId;
		mLvItem.CustomActive(suitId > 0);
		if (suitId > 0)
		{
			int lv = petInfoData.level;
			int maxLv = ZhanHunSuitTableManager.Instance.GetZhanHunSuitMaxLevel(suitId);
			int exp = petInfoData.exp;
			int maxExp = ZhanHunLevelTableManager.Instance.GetZhanHunLevelNeedExp(lv);
			float sliderValue = 0f;
			string temp = ClientTipsTableManager.Instance.GetClientTipsContext(1689);
			mLvNum.text = $"{exp}/{maxExp}".BBCode(ColorType.MainText);
			if (lv >= maxLv)
			{

				string tempStr1 = CSString.Format(1810, $"{temp}{lv}/{maxLv}").BBCode(ColorType.MainText);
				string tempStr2 = ClientTipsTableManager.Instance.GetClientTipsContext(1743);
				tempStr2 = $"[url=func:7:gamemodel:28000][u]{tempStr2}[/u][/url]".BBCode(ColorType.Green);
				mLvName.text = $"{tempStr1}{tempStr2}";
				mLvName.SetupLink();
			}
			else
				mLvName.text = CSString.Format(1810, $"{temp}{lv}/{maxLv}").BBCode(ColorType.MainText);

			if (exp > 0 && exp < maxExp)
				sliderValue = (float)exp / maxExp;
			else if (exp >= maxExp)
				sliderValue = 1f;
			mSliderExp.value = sliderValue;
		}
	}
	public void SetItemClick(int subType)
	{
		itemId = subType;
	}
	/// <summary>
	/// 点击头像
	/// </summary>
	/// <param name="_go"></param>
	void ItemClick(GameObject _go)
	{
		if (curPetItem != null)
		{
			curPetItem.ChangeSelectState(false);
		}
		curPetItem = (PetShowItem)UIEventListener.Get(_go).parameter;
		curPetItem.ChangeSelectState(true);

		RefreshModel(curPetItem);
		RefreshText(curPetItem);
		RefreshRightPetInfo(curPetItem);
		RefreshPetState(curPetItem);
	}
	private void RefreshPetState(PetShowItem curPetItem)
	{
		mNotActive.CustomActive(curPetItem.suitid > petInfoData.suitId);
		mYesActive.CustomActive(curPetItem.suitid < petInfoData.suitId);
		if (curPetItem.suitid == petInfoData.suitId)
		{
			int mapId = CSMainPlayerInfo.Instance.MapID;
			int stateId = CSPetBasePropInfo.Instance.GetPetState();
			int withHero = MapInfoTableManager.Instance.GetMapInfoWithHero(mapId);
			bool isActivePvP = CSPetBasePropInfo.Instance.IsActivePvP;
			bool isWithHero = false;
			if (withHero == 2)
				isWithHero = !isActivePvP;
			else
				isWithHero = withHero > 2;
			if(isWithHero)
				mPetStateLabel.text = CSString.Format(2048);
			else
			{
				if (stateId == (int)PetHeadState.Battle)
					mPetStateLabel.text = CSString.Format(1635);
				else if (stateId == (int)PetHeadState.Dead)
				{
					countDown = CSPetBasePropInfo.Instance.GetCountDwon();
					mPetStateLabel.text = CSString.Format(1636, countDown);
					mCSInvoke.InvokeRepeating(0f, 1f, DeadCountDown);
				}
				else
					mPetStateLabel.text = "";
			}
		}
		else
			mPetStateLabel.text = "";
	}
	//战宠死亡倒计时
	private void DeadCountDown()
	{
		if (countDown > 0)
			mPetStateLabel.text = CSString.Format(1636, countDown);
		else
		{
			mCSInvoke.StopInvokeRepeating();
			mPetStateLabel.text = CSString.Format(1635);
		}
		countDown -= 1;
	}
	/// <summary>
	/// 刷新模型
	/// </summary>
	/// <param name="curPetItem"></param>
	void RefreshModel(PetShowItem curPetItem)
	{
		int weaponModel, clothModel;
		TABLE.ZHANHUNSUIT suitTable = null;
		ZhanHunSuitTableManager.Instance.TryGetValue(curPetItem.suitid, out suitTable);
		if (suitTable == null) return;
		weaponModel = suitTable.weaponmodel;
		clothModel = suitTable.clothesmodel;
		CSEffectPlayMgr.Instance.ShowUIEffect(mWeaponModel, weaponModel.ToString(), ResourceType.UIWeapon);
		CSEffectPlayMgr.Instance.ShowUIEffect(mColothModel, clothModel.ToString(), ResourceType.UIPlayer);
	}
	/// <summary>
	/// 刷新文本信息，选中的suitId
	/// </summary>
	/// <param name="curPetItem"></param>
	void RefreshText(PetShowItem curPetItem)
	{
		string petTip = "";
		int curSuitId = 0;//展示属性用的suitId
		int curPetId;
		bool isShowNext = false;
		//最高级
		if (petInfoData.suitId >= maxSuitId)
			petTip = ClientTipsTableManager.Instance.GetClientTipsContext(1642);
		//激活状态
		else if (petInfoData.suitId == curPetItem.suitid)
		{
			curSuitId = curPetItem.suitid + 1;
			isShowNext = true;
		}
		//第一个宠物主线任务未做完状态
		else if (!petInfoData.active && curPetItem.suitid == 1)
			petTip = CSString.Format(1638);
		//未激活状态
		else
			curSuitId = curPetItem.suitid;

		if (curSuitId > 0)
		{
			int havaNum, allNum, clientId;
			ColorType colorType;
			string lvStr, suitName, tempGet, tempNumStr;
			TABLE.ZHANHUNSUIT suitTable = null;
			ZhanHunSuitTableManager.Instance.TryGetValue(curSuitId, out suitTable);
			if (suitTable == null) return;
			curPetId = suitTable.suitSummoned;
			allNum = suitTable.suitNum;
			suitName = suitTable.name;
			lvStr = MonsterInfoTableManager.Instance.GetMonsterInfoLevel(curPetId).ToString().BBCode(ColorType.Green);
			havaNum = CSPetBasePropInfo.Instance.GetEquipNum(curSuitId);
			colorType = havaNum >= allNum ? ColorType.Green : ColorType.Red;
			tempNumStr = $"({havaNum}/{allNum})".BBCode(colorType);
			suitName = $"[url=func:7:gamemodel:24000][u]{suitName}[/u][/url]".BBCode(ColorType.Green);
			tempGet = $"[url=func:7:gamemodel:25000][u]{ClientTipsTableManager.Instance.GetClientTipsContext(1640)}[/u][/url]";
			clientId = isShowNext ? 1639 : 1641;
			mPeTipstLabel.text = $"{CSString.Format(clientId, allNum, lvStr, suitName, tempNumStr).BBCode(ColorType.MainText)}{tempGet}";
			mPeTipstLabel.SetupLink();
		}
		else
			mPeTipstLabel.text = petTip;
		CSPetBasePropInfo.Instance.SetCurClickSuitId(curPetItem.suitid);
		mPetName.text = ZhanHunSuitTableManager.Instance.GetZhanHunSuitName(curPetItem.suitid);
	}
	void ShowTypeChange(GameObject _go)
	{
		int type = (int)UIEventListener.Get(_go).parameter;
		if (curCheckMark != null)
			curCheckMark.CustomActive(false);
		curCheckMark = _go.transform.Find("Checkmark").gameObject;
		curCheckMark.CustomActive(true);
		mAttWindow.CustomActive(type == 1);
		mSkillWindow.CustomActive(type == 2);
		//宠物技能
		if (type == 2)
			RefreshSkillUI();
	}
	void RefreshRightPetInfo(PetShowItem curPetItem)
	{
		//战宠属性
		int descBtnIdx = 1;
		//未完成任务
		if (!petInfoData.active)
		{
			CSPetBasePropInfo.Instance.SetAttProp(descBtnIdx);
			RefreshBasePoro();
		}
		else
		{
			if (petInfoData.suitId > 0)
			{
				descBtnIdx = petInfoData.suitId;
				RefreshBasePoro();
			}
		}

		mAttView.SetDynamicArrowVertical(mAttDownIcon);
		mBtnDesc.gameObject.CustomActive(curPetItem.suitid == descBtnIdx);
	}
	private void OnBtnDesc(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UIWarSoulPropDescPanel>();
	}
	private void OnBtnLvUp(GameObject _go)
	{
		CSPetLevelUpInfo.Instance.JudgeOpenPetLevelUpPanel(() => { UIManager.Instance.ClosePanel<UIWarPetCombinedPanel>(); });
	}
	private void RefreshBasePoro()
	{
		CSPetBasePropInfo.Instance.ChangePetBasePropInfo();
		allList = CSPetBasePropInfo.Instance.GetAllList();
		allList = CSPetBasePropInfo.Instance.DealWithList(allList);
		mBasicGrid.Bind<PetBasePropInfoData, PropItem>(allList, mPoolHandleManager);
	}
	private void RefreshSkillUI()
	{
		activeList = CSPetBasePropInfo.Instance.GetAtcivePetBaseList();
		petPassiveSkillList = CSPetBasePropInfo.Instance.GetPassivePetBaseList();
		specialSkill = CSPetBasePropInfo.Instance.GetSpecialSkillData();
		int bdjnUnlockLevel = CSPetTalentInfo.Instance.GetPassiveSkillUnlockLv();
		int curTalentLevel = CSSkillInfo.Instance.GetPetTalentLevel();
		bool unloced = curTalentLevel >= bdjnUnlockLevel;
		mActiveGrid.MaxCount = activeList.Count;
		mPassiveGrid.MaxCount = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSkillNum(curPetItem.suitid) + 1;
		SetSkillIcon(mActiveGrid, activeList, false, specialSkill);//主动技能
		mPassiveGrid.gameObject.CustomActive(unloced);
		if (!unloced)
		{
			mTalentUnlock.text = CSString.Format(1534, bdjnUnlockLevel);
			mTalentUnlock.SetupLink();
		}
		else
		{
			mTalentUnlock.text = "";
			SetSkillIcon(mPassiveGrid, petPassiveSkillList, true, specialSkill);//被动技能
		}
		//设置坐标
		Vector3 tempPos;
		float tempY;
		float distance = Vector3.Distance(mActiveTit.transform.localPosition, mPassiveTit.transform.localPosition);
		int lineNum = (int)Math.Ceiling((float)mActiveGrid.MaxCount / mActiveGrid.MaxPerLine);
		float tempHeight = lineNum * mActiveGrid.CellHeight;
		if (distance < tempHeight)
		{
			tempPos = mPassiveTit.transform.localPosition;
			tempY = mActiveTit.transform.localPosition.y - lineNum * mActiveGrid.CellHeight - 60;
			mPassiveTit.transform.localPosition = new Vector3(tempPos.x, tempY, tempPos.z);

			tempPos = mPassiveGrid.transform.localPosition;
			tempY -= 80;
			mPassiveGrid.transform.localPosition = new Vector3(tempPos.x, tempY, tempPos.z);
		}
	}
	private void SetSkillIcon(UIGridContainer grid, CSBetterLisHot<PetBaseSkillData> skillList,
		bool isPassive, PetBaseSkillData specialSkill)
	{
		ScriptBinder tempBinder;
		UILabel lb_name, lb_lv;
		UISprite sp_icon;
		GameObject go, obj_num, obj_addBtn, obj_skillNameBg;
		for (int i = 0; i < grid.MaxCount; i++)
		{
			go = grid.controlList[i];
			tempBinder = go.GetComponent<ScriptBinder>();
			lb_name = tempBinder.GetObject("LbSkillName") as UILabel;
			lb_lv = tempBinder.GetObject("LbSkillLv") as UILabel;
			sp_icon = tempBinder.GetObject("SPSkillIcon") as UISprite;
			obj_num = tempBinder.GetObject("SkillNumObj") as GameObject;
			obj_addBtn = tempBinder.GetObject("BtnAdd") as GameObject;
			obj_skillNameBg = tempBinder.GetObject("SkillNameBG") as GameObject;
			if (i < skillList.Count)
			{
				SetValue(sp_icon, lb_name, lb_lv, skillList[i], go);
				obj_addBtn.CustomActive(false);
				obj_skillNameBg.CustomActive(true);
			}
			else if (isPassive && i == grid.MaxCount - 1)
			{
				SetValue(sp_icon, lb_name, lb_lv, specialSkill, go);
				obj_addBtn.CustomActive(false);
				obj_skillNameBg.CustomActive(true);
			}
			else
			{
				sp_icon.spriteName = "";
				lb_name.text = CSString.Format(1705);
				obj_num.CustomActive(i < skillList.Count);
				obj_addBtn.CustomActive(i >= skillList.Count && i < grid.MaxCount - 1);
				UIEventListener.Get(go, 0).onClick = ShowSkillTip;
				obj_skillNameBg.CustomActive(i < skillList.Count || i >= grid.MaxCount - 1);
			}
		}
	}
	private void SetValue(UISprite sp_icon, UILabel lb_name, UILabel lb_lv, PetBaseSkillData skillData, GameObject go)
	{
		sp_icon.spriteName = skillData.icon;
		lb_name.text = skillData.name.BBCode(ColorType.MainText);
		lb_lv.text = skillData.level.ToString().BBCode(ColorType.MainText);
		if (skillData.isGet)
			sp_icon.color = Color.white;
		else
			sp_icon.color = Color.black;
		UIEventListener.Get(go, skillData.skillId).onClick = ShowSkillTip;
	}
	/// <summary>
	/// 获取技能tip或者点击跳转面板
	/// </summary>
	/// <param name="_go"></param>
	private void ShowSkillTip(GameObject _go)
	{
		int skillId = (int)UIEventListener.Get(_go).parameter;
		if (skillId > 0)
			Utility.OpenUIWarPetSkillTipsPanel(skillId, SkillTipsAdaptiveType.TopLeft);
		else
		{
			UIManager.Instance.CreatePanel<UIWarPetCombinedPanel>(p =>
			{
				(p as UIWarPetCombinedPanel).OpenChildPanel((int)UIWarPetCombinedPanel.ChildPanelType.CPT_Refine);
			});
		}
	}
	void OnBtnHelp(GameObject go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WarSoul);
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CSEffectPlayMgr.Instance.Recycle(mTexBG);
		CSEffectPlayMgr.Instance.Recycle(mWeaponModel);
		CSEffectPlayMgr.Instance.Recycle(mColothModel);
		mCSInvoke.StopInvokeRepeating();
		mBasicGrid.UnBind<PropItem>();
		petIconList.Clear();
		if (mClientEvent != null)
			mClientEvent.RemoveEvent(CEvent.GetWarPetBaseInfo, RefreshWarPetBaseInfoUI);

		petIconList = null;
		curPetItem = null;
		mViewPos = null;

		itemId = 0;
		maxSuitId = 0;
		countDown = 0;
	}

	private class PropItem : UIBinder
	{
		UILabel lb_name, lb_num;
		PetBasePropInfoData mData;
		public override void Init(UIEventListener handle)
		{
			lb_name = Get<UILabel>("lb_name");
			lb_num = Get<UILabel>("lb_num");
		}
		public override void Bind(object data)
		{
			mData = data as PetBasePropInfoData;
			if (mData == null) return;
			if (mData.maxValue > 0)
			{
				lb_name.text = mData.specialName.BBCode(ColorType.SecondaryText);
				lb_num.text = $"{mData.value}-{mData.maxValue}".BBCode(ColorType.MainText);
			}
			else
			{
				lb_name.text = mData.propName.BBCode(ColorType.SecondaryText);
				lb_num.text = CSPetBasePropInfo.Instance.GetDealWithValue(mData.id, mData.value).BBCode(ColorType.MainText);
			}
		}
		public override void OnDestroy()
		{
			lb_name = null;
			lb_num = null;
			mData = null;
		}
	}
	private class PetShowItem : IDispose
	{
		GameObject go;
		GameObject select;
		UISprite icon;
		GameObject flag;
		Action<GameObject> action;
		UIEventListener btn;
		public int petid;       //战宠id
		public int suitid;      //战魂suit表id
		public void SetData(GameObject _go, Action<GameObject> _dele)
		{
			go = _go;
			action = _dele;
			select = go.transform.Find("select").gameObject;
			icon = go.transform.Find("headitem").GetComponent<UISprite>();
			flag = go.transform.Find("flag").gameObject;
			btn = UIEventListener.Get(go, this);
			btn.onClick = action;
		}
		public void RefreshByPetId(int _petId, int _suitId)
		{
			petid = _petId;
			suitid = _suitId;
			icon.spriteName = MonsterInfoTableManager.Instance.GetMonsterInfoHead(petid).ToString();
		}
		public void RefreshFlagIcon(bool _isShow, bool _isFlag)
		{
			flag.CustomActive(_isFlag);
			if (_isShow)
				icon.color = Color.white;
			else
				icon.color = Color.black;
		}
		public void ChangeSelectState(bool _state)
		{
			select.CustomActive(_state);
		}
		public void Dispose()
		{
			go = null;
			select = null;
			icon = null;
			flag = null;
			action = null;
			if (btn != null)
			{
				btn.onClick = null;
				btn = null;
			}
			petid = 0;
			suitid = 0;
		}
	}
}