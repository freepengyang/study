using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
	Normal = 1,//普通
	Wolong,//卧龙装备
}

/// <summary>
/// 获取装备按钮
/// </summary>
public enum NormalEquipObtain
{
	FirstCharge,//首充
	OpenServicePackage,//开服礼包
	FieldBoss,//野外Boss
	PersonalBoss,//个人Boss
	DungeonSiege,//地牢围攻
}


public enum WolongEquipObtain
{
	FieldBoss,//野外Boss
	PersonalBoss,//个人Boss
	WorldBoss,//世界Boss
	SeekTreasure,//寻宝
}


public partial class UIRoleEquipObtainPanel : UIBasePanel
{
	public override bool ShowGaussianBlur
	{
		get => false;
	}

	private EquipType curEquipType = EquipType.Normal;
	private int posIndex = 0;
	
	private ILBetterList<NormalEquipObtain> listNormalButtons = new ILBetterList<NormalEquipObtain>();
	private ILBetterList<WolongEquipObtain> listWolongButtons = new ILBetterList<WolongEquipObtain>();
	
	public override void Init()
	{
		base.Init();
		// AddCollider();
		mbtn_bg.onClick = Close;
	}
	
	public override void Show()
	{
		base.Show();
	}
	
	private string[] arrayStr;
	public void ShowRoleEquipObtain(EquipType equip, int index)
	{
		bool isFirstRecharge = CSVipInfo.Instance.IsFirstRecharge();
		curEquipType = equip;
		posIndex = index;
		switch (equip)
		{
			case EquipType.Normal:
				if (!isFirstRecharge)
					listNormalButtons.Add(NormalEquipObtain.FirstCharge);
				
				listNormalButtons.Add(NormalEquipObtain.OpenServicePackage);
				listNormalButtons.Add(NormalEquipObtain.FieldBoss);
				listNormalButtons.Add(NormalEquipObtain.PersonalBoss);
				listNormalButtons.Add(NormalEquipObtain.DungeonSiege);
				
				arrayStr = UtilityMainMath.StrToStrArr(CSString.Format(1311));
				CSBagInfo.Instance.ScreenButtonForNormalEquipObtain(listNormalButtons);
				break;
			case EquipType.Wolong:
				listWolongButtons.Add(WolongEquipObtain.FieldBoss);
				listWolongButtons.Add(WolongEquipObtain.PersonalBoss);
				listWolongButtons.Add(WolongEquipObtain.WorldBoss);
				listWolongButtons.Add(WolongEquipObtain.SeekTreasure);
				
				arrayStr = UtilityMainMath.StrToStrArr(CSString.Format(1317));
				CSBagInfo.Instance.ScreenButtonForWolongEquipObtain(listWolongButtons);
				break;
		}
		InitData();
		SetPosition();
	}


	private Map<int, float> mapPosY;
	/// <summary>
	/// 自适应位置
	/// </summary>
	void SetPosition()
	{
		if (posIndex < 1) return;
		if (mapPosY==null)
			mapPosY = new Map<int, float>();
		
		mapPosY.Add(1, (415f-msp_bg.height*1f)/2);
		mapPosY.Add(2, (415f-msp_bg.height*1f)/2 - 82f);
		mapPosY.Add(5, (415f-msp_bg.height*1f)/2 - 164f);
		mapPosY.Add(7, (415f-msp_bg.height*1f)/2 - 246f);	
		mapPosY.Add(3, (415f-msp_bg.height*1f)/2);
		mapPosY.Add(4, (415f-msp_bg.height*1f)/2 - 82f);
		mapPosY.Add(6, (415f-msp_bg.height*1f)/2 - 164f);
		mapPosY.Add(8, (415f-msp_bg.height*1f)/2 - 246f);
		mapPosY.Add(10, (msp_bg.height*1f-555f)/2 + 82f);
		mapPosY.Add(9, (msp_bg.height*1f-555f)/2);
		mapPosY.Add(11, (msp_bg.height*1f-555f)/2 + 82f);
		mapPosY.Add(12, (msp_bg.height*1f-555f)/2);
		switch (posIndex)
		{
			//左上
			case 1:
			case 2:
			case 5:
			case 7:
			//左下
			case 10:
			case 9:
				UIPrefab.transform.localPosition = new Vector3(-350f, mapPosY[posIndex], 0);
				break;
			//右上
			case 3:
			case 4:
			case 6:
			case 8:
			//右下
			case 11:
			case 12:
				UIPrefab.transform.localPosition = new Vector3(-160f, mapPosY[posIndex], 0);
				break;
		}
	}

	void InitData()
	{
		mgrid_btn.MaxCount = curEquipType==EquipType.Normal? listNormalButtons.Count:listWolongButtons.Count;
		int gridHeight = mgrid_btn.MaxCount * (int) mgrid_btn.CellHeight;
		msp_bg.height = 40 + gridHeight;
		msp_bgButton.height = gridHeight;
		GameObject gp;
		UILabel lb_btn;
		object obj;
		for (int i = 0; i < mgrid_btn.MaxCount; i++)
		{
			gp = mgrid_btn.controlList[i];
			lb_btn = gp.transform.GetChild(0).GetComponent<UILabel>();
			int index = curEquipType == EquipType.Normal ? (int) listNormalButtons[i] : (int) listWolongButtons[i];
			lb_btn.text = arrayStr[index];
			obj = curEquipType == EquipType.Normal ? (object)listNormalButtons[i] : listWolongButtons[i];
			UIEventListener.Get(gp, obj).onClick = OnClickButton;
		}
	}

	void OnClickButton(GameObject go)
	{
		if (go == null) return;
		int levellimt = 0;
		switch (curEquipType)
		{
			case EquipType.Normal:
				NormalEquipObtain typeNormalEquipObtain = (NormalEquipObtain)UIEventListener.Get(go).parameter;
				switch (typeNormalEquipObtain)
				{
					case NormalEquipObtain.FirstCharge:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(38);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(21000);
						break;
					case NormalEquipObtain.OpenServicePackage:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(21);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(23101);
						break;
					case NormalEquipObtain.FieldBoss:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(10420);
						break;
					case NormalEquipObtain.PersonalBoss:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(10400);
						break;
					case NormalEquipObtain.DungeonSiege:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(34);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(12000);
						break;
				}
				break;
			case EquipType.Wolong:
				WolongEquipObtain typeWolongEquipObtain = (WolongEquipObtain)UIEventListener.Get(go).parameter;
				switch (typeWolongEquipObtain)
				{
					case WolongEquipObtain.FieldBoss:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(30);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(10420);
						break;
					case WolongEquipObtain.PersonalBoss:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(11);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(10400);
						break;
					case WolongEquipObtain.WorldBoss:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(14);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(10410);
						break;
					case WolongEquipObtain.SeekTreasure:
						levellimt = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(29);
						if (CSMainPlayerInfo.Instance.Level < levellimt)
						{
							UtilityTips.ShowRedTips(1618, levellimt);
							return;
						}
						UtilityPanel.JumpToPanel(13000);
						break;
				}
				break;
		}
	
		Close();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
