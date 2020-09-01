using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UILianTiPanel : UIBasePanel
{
	protected CSPool.Pool<AttrItemData> starAttrDatas;
	private int myCareer = CSMainPlayerInfo.Instance.Career;
	private int nextID = 0;
	LianTiData lianTiData = new LianTiData();
	List<List<int>> listArribute = new List<List<int>>();
	List<List<int>> nlistArribute = new List<List<int>>();
	List<List<int>> templistArribute = new List<List<int>>();
	List<long> listCost = new List<long>();
	string[] numList;
	IntArray buyProps;
	public override void Init()
	{
		base.Init();
		mBtnTeleport.onClick = OnBtnTeleport;
		mBtnLvUp.onClick = OnBtnLvUp;
		mBtnHelp.onClick = OnBtnHelp;
		mIcon1.onClick = OnBtnIcon;
		mIcon2.onClick = OnBtnIcon;
		starAttrDatas = GetListPool<AttrItemData>();
		mClientEvent.AddEvent(CEvent.GetLianTiInfo, GetLianTiInfo);
		mClientEvent.AddEvent(CEvent.MoneyChange, RefreshCost);
		RegisterRed(mRedPoint, RedPointType.LianTi);
	}

	public override void Show()
	{
		base.Show();
		RefreshUI(-1);
		CSEffectPlayMgr.Instance.ShowUITexture(mStageEffectBg, "lianti_bg2");
	}

	void RefreshUI(int idx)
	{
		RefreshLianTiData();
		RefreshUIInfo();
		RefreshAttributes();
		RefreshCostItems();
		RefreshIconEffect(idx);
	}
	private void RefreshCost(uint id, object argv)
	{
		if (id == (uint)CEvent.MoneyChange && argv != null)
		{
			if (argv.GetType() == typeof(MoneyType) && (int)argv == (int)MoneyType.zhenqi)
			{
				RefreshCostItems();
			}
		}
	}
	private void RefreshLianTiData()
	{
		lianTiData.id = CSLianTiInfo.Instance.LianTiID;
		lianTiData.limitLv = LianTiTableManager.Instance.GetLianTiLevel(lianTiData.id);
		lianTiData.cost = LianTiTableManager.Instance.GetLianTiCost(lianTiData.id);
		lianTiData.showClass = CSLianTiInfo.Instance.GetLianTiClass();
		lianTiData.liantiName = LianTiTableManager.Instance.GetLianTiName(lianTiData.id);
		//lianTiData.liantiPicRound = LianTiTableManager.Instance.GetLianTiPicRound(lianTiData.id);
		lianTiData.liantiPic = LianTiTableManager.Instance.GetLianTiPic(lianTiData.id);
		lianTiData.liantiPic1 = LianTiTableManager.Instance.GetLianTiPic1(lianTiData.id);
		lianTiData.liantiPic2 = LianTiTableManager.Instance.GetLianTiPic2(lianTiData.id);
		buyProps = LianTiTableManager.Instance.GetLianTiBuyprops(lianTiData.id);
	}

	private void RefreshUIInfo()
	{
		CSStringBuilder.Clear();
		string colorStr;
		string temp = CSString.Format(744);
		int lv = lianTiData.limitLv;

		if (!CSLianTiInfo.Instance.IsNotLvReach())
		{
			colorStr = UtilityColor.GetColorString(ColorType.Red);
			mNeedLVLabel.text = CSStringBuilder.Append(temp, colorStr, lv).ToString();
		}
		else
		{
			colorStr = UtilityColor.GetColorString(ColorType.Green);
			mNeedLVLabel.text = CSStringBuilder.Append(temp, colorStr, lv).ToString();
		}
		CSStringBuilder.Clear();
	}

	private void RefreshAttributes()
	{
		var datas = mPoolHandleManager.GetSystemClass<List<AttrItemData>>();
		datas.Clear();
		//starAttrDatas.RecycleAllItems();//装备回收，再点面板报错
		nextID = CSLianTiInfo.Instance.NextID();
		//属性
		listArribute = SetnlistArribute(myCareer, lianTiData.id);
		var kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, listArribute);
		templistArribute.Clear();
		if (nextID != 0)
		{
			nlistArribute = SetnlistArribute(myCareer, nextID);
			templistArribute = nlistArribute;
		}
		else
		{
			templistArribute = listArribute;
		}
		var nextKvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, templistArribute);
		for (int i = 0; i < kvs.Count; i++)
		{
			var currentValue = starAttrDatas.Get();
			datas.Add(currentValue);
			currentValue.pooledHandle = mPoolHandleManager;
			currentValue.keyValue = kvs[i];
			currentValue.nKeyValue = nextKvs[i];
		}
		nextKvs.Clear();
		mPoolHandleManager.Recycle(nextKvs);
		kvs.Clear();
		mPoolHandleManager.Recycle(kvs);
		mGridLowStageEffect.Bind<AttrItemData, AttrItem>(datas, mPoolHandleManager);
		datas.Clear();
		mPoolHandleManager.Recycle(datas);
	}
	private List<List<int>> SetnlistArribute(int myCareer, int id)
	{
		if (myCareer == 1)
			return UtilityMainMath.SplitStringToIntLists(LianTiTableManager.Instance.GetLianTiZsAttr(id));
		else if(myCareer == 2)
			return UtilityMainMath.SplitStringToIntLists(LianTiTableManager.Instance.GetLianTiFsAttr(id));
		else
			return UtilityMainMath.SplitStringToIntLists(LianTiTableManager.Instance.GetLianTiDsAttr(id));
	}
	private void RefreshCostItems()
	{
		int nextId = CSLianTiInfo.Instance.NextID();
		if (nextId != 0)
		{
			UISprite sp_icon;
			UILabel lb_num;
			UIEventListener addBtn;
			ScriptBinder TempBinder;
			int cfgId;
			long owned,needed;
			string itemIcon, tempColor;
			string mainColor = UtilityColor.GetColorString(ColorType.MainText);
			if (string.IsNullOrWhiteSpace(lianTiData.cost)) return;
			listCost = UtilityMainMath.SplitStringToLongList(lianTiData.cost);
			if (listCost == null || listCost.Count < 2) return;
			
			cfgId = (int)listCost[0];
			needed = listCost[1];
			owned = cfgId.GetItemCount();
			itemIcon = ItemTableManager.Instance.GetItemIcon(cfgId);
			for (int i = 0; i < mGridUpStarCosts.MaxCount; i++)
			{
				TempBinder = mGridUpStarCosts.controlList[i];
				sp_icon = TempBinder.GetObject("sp_icon") as UISprite;
				lb_num = TempBinder.GetObject("lb_value") as UILabel;
				addBtn = TempBinder.GetObject("btn_add") as UIEventListener;
				sp_icon.spriteName = $"tubiao{itemIcon}";
				addBtn.onClick = OnAddBtn;
				if (i == 0)
				{
					lb_num.text = $"{mainColor}{UtilityMath.GetDecimalTenThousandValue(needed, "F0")}";
				}
				else
				{
					tempColor = owned >= needed ? UtilityColor.GetColorString(ColorType.Green) : UtilityColor.GetColorString(ColorType.Red);
					lb_num.text = $"{tempColor}{UtilityMath.GetDecimalTenThousandValue(owned, "F0")}";
				}
			}
		}
		mNeedLVLabel.gameObject.SetActive(nextId != 0);
		mGridUpStarCosts.gameObject.SetActive(nextId != 0);
		mBtnLvUp.gameObject.SetActive(nextId != 0);
		mLVMaxTip.SetActive(nextId == 0);
	}
	private void OnAddBtn(GameObject _go)
	{
		int itemId = (int)listCost[0];
		Utility.ShowGetWay(itemId);
	}
	//index 播放一次升级特效
	private void RefreshIconEffect(int index)
	{
		ScriptBinder TempBinder;
		UISprite tempSp;
		GameObject effect1, effect2, effect3;
		if(numList == null)
		{
			string numStr = CSString.Format(2002);
			numList = UtilityMainMath.StrToStrArr(numStr);
			if (numList == null) return;
		}
		mTitLeLabel.text = numList[lianTiData.showClass];
		mSp1.spriteName = lianTiData.liantiPic;
		mSp2.spriteName = lianTiData.liantiPic1;

		//圆环icon设置
		int idx = 1;
		if (lianTiData.id > 1)
			idx = (lianTiData.id - 1) / 6;
		else
			idx = 0;
		idx = idx * 6;

		for (int i = 0; i < mSixGrid.MaxCount; i++)
		{
			TempBinder = mSixGrid.controlList[i];
			tempSp = TempBinder.GetObject("icon") as UISprite;
			effect1 = TempBinder.GetObject("effect") as GameObject;
			effect2 = TempBinder.GetObject("effect2") as GameObject;
			effect3 = TempBinder.GetObject("effect3") as GameObject;
			//select = TempBinder.GetObject("select") as GameObject;

			//设置圆环
			idx += 1;
			tempSp.spriteName = LianTiTableManager.Instance.GetLianTiPicRound(idx);
			effect1.gameObject.SetActive(i < lianTiData.showClass);
			if (i < lianTiData.showClass)
			{
				tempSp.color = Color.white;
				CSEffectPlayMgr.Instance.ShowUIEffect(effect1, 17069);
			}
			else
				tempSp.color = Color.black;

			//将要进阶的下一级特效
			if (lianTiData.id < 1 && i == 0)
			{
				effect3.SetActive(true);
				CSEffectPlayMgr.Instance.ShowUIEffect(effect3, 17121);
			}
			else if (lianTiData.showClass < 6 && i == lianTiData.showClass)
			{
				CSEffectPlayMgr.Instance.ShowUIEffect(effect3, 17121);
				effect3.SetActive(true);
			}
			else
				effect3.SetActive(false);

			//一次升级特效
			effect2.SetActive(index == i);
			if(lianTiData.id < 1 && i == 0)
				CSEffectPlayMgr.Instance.ShowUIEffect(effect2, 17070, 10, false);
			else if (index == i)
				CSEffectPlayMgr.Instance.ShowUIEffect(effect2, 17070, 10, false);
		}
	}
	void OnBtnTeleport(GameObject go)
	{
		UtilityPanel.JumpToPanel(18000);
	}
	void OnBtnLvUp(GameObject go)
	{
		if (CSLianTiInfo.Instance.NextID() == 0)
		{
			UtilityTips.ShowRedTips(705);
			return;
		}
		if (!CSLianTiInfo.Instance.IsNotLvReach())
		{
			int playLv = CSMainPlayerInfo.Instance.Level;
			Utility.ShowLimitLvWay(playLv, lianTiData.limitLv);
			return;
		}
		if (!CSLianTiInfo.Instance.IsEnoughItem())
		{
			int itemId = (int)listCost[0];
			Utility.ShowGetWay(itemId);
			return;
		}
		Net.CSLianTiUpLevelMessage();
	}
	void OnBtnHelp(GameObject go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HELP_LIANTI);
	}
	private void OnBtnIcon(GameObject _go)
	{
		int itemId = (int)listCost[0];
		UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemId);
	}

	protected void GetLianTiInfo(uint id, object data)
	{
		lianTiData.showClass = CSLianTiInfo.Instance.GetLianTiClass();
		RefreshUI(lianTiData.showClass - 1);
		UtilityTips.ShowGreenTips(563);
	}
	/// <summary>
	/// 炼体信息
	/// </summary>
	private class LianTiData : IDispose
	{
		public LianTiData() { }
		public LianTiData(int liantiId, int lv, string costStr, int _showClass, string name, string picRound, string pic, string pic1, string pic2)
		{
			id = liantiId;
			limitLv = lv;
			cost = costStr;
			showClass = _showClass;
			liantiName = name;
			liantiPicRound = picRound;
			liantiPic = pic;
			liantiPic1 = pic1;
			liantiPic2 = pic2;
		}
		/// <summary>
		/// 炼体Id
		/// </summary>
		public int id = 0;
		/// <summary>
		/// 炼体等级限制
		/// </summary>
		public int limitLv = 0;
		/// <summary>
		/// 炼体升级消耗
		/// </summary>
		public string cost = "";
		/// <summary>
		/// 显示阶数
		/// </summary>
		public int showClass = 0;
		/// <summary>
		/// 炼体阶数
		/// </summary>
		public string liantiName = "";
		/// <summary>
		/// 炼体icon图片
		/// </summary>
		public string liantiPicRound = "";
		public string liantiPic = "";
		public string liantiPic1 = "";
		public string liantiPic2 = "";
		public void Dispose()
		{
			id = 0;
			limitLv = 0;
			showClass = 0;
			cost = "";
			liantiName = "";
			liantiPicRound = "";
			liantiPic = "";
			liantiPic1 = "";
			liantiPic2 = "";
		}
	}

	private void RefycleLianTiEffect()
	{
		ScriptBinder TempBinder;
		UISprite tempSp;
		GameObject effect1, effect2, effect3;
		for (int i = 0; i < mSixGrid.MaxCount; i++)
		{
			TempBinder = mSixGrid.controlList[i];
			tempSp = TempBinder.GetObject("icon") as UISprite;
			effect1 = TempBinder.GetObject("effect") as GameObject;
			effect2 = TempBinder.GetObject("effect2") as GameObject;
			effect3 = TempBinder.GetObject("effect3") as GameObject;

			tempSp.spriteName = "";
			CSEffectPlayMgr.Instance.Recycle(effect1);
			CSEffectPlayMgr.Instance.Recycle(effect2);
			CSEffectPlayMgr.Instance.Recycle(effect3);
		}
	}
	protected override void OnDestroy()
	{
		//base.OnDestroy();   //CSPool.ListPoolHandle.DestroyList(mCachedList[i]);回收报错注释
		CSEffectPlayMgr.Instance.Recycle(mStageEffectBg);
		CSEffectPlayMgr.Instance.Recycle(mStageEffect);
		mGridLowStageEffect.UnBind<AttrItem>();
		RefycleLianTiEffect();
		templistArribute.Clear();
		listArribute.Clear();
		nlistArribute.Clear();
		listCost.Clear();
		nextID = 0;
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetLianTiInfo, GetLianTiInfo);
			mClientEvent.RemoveEvent(CEvent.MoneyChange, RefreshCost);
		}
		numList = null;
		mGridLowStageEffect = null;
		listArribute = null;
		nlistArribute = null;
		listCost = null;
		mGridUpStarCosts = null;
		starAttrDatas = null;
		numList = null;
	}
}