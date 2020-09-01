using System.Collections.Generic;
using bag;
using UnityEngine;
using user;

public partial class UICheckDragonPanel : UIBasePanel
{
	public override bool ShowGaussianBlur { get =>false; }
	
	private int[] wolongEquipIndex = { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112 };//格子对应的服务器pos
	
	List<WolongInfoItem> attrList = new List<WolongInfoItem>();
	
	private OtherPlayerInfo checkRoleInfo;

	Dictionary<int, bag.BagItemInfo> mWLEquipData;
	List<EquipItem> WLequipItems;
	
	Dictionary<int, bag.BagItemInfo> mEquipData;
	
	int maxId = 0;

	public override void Init()
	{
		base.Init();
		CSEffectPlayMgr.Instance.ShowUITexture(meffect, "rolezlbg2_cq_1");
		mbtn_ToNormal.onClick = OnClickToNormal;
		mbtn_help.onClick = OnClickHelp;
		
		attrList.Clear();
		attrList.Add(new WolongInfoItem(matt));
		attrList.Add(new WolongInfoItem(mphydef));
		attrList.Add(new WolongInfoItem(mmagicdef));
		attrList.Add(new WolongInfoItem(mhp));
	}
	
	public override void Show()
	{
		base.Show();
		InitData();
	}
	
	void InitData()
	{
		checkRoleInfo = CSOtherPlayerInfo.Instance.OtherPlayerInfo;
		SetLeftData();
		SetRightData();
	}

	void SetLeftData()
	{
		mlb_rolename.text = checkRoleInfo.roleBrief.roleName;
		mlb_rolelevel.text = $"Lv{checkRoleInfo.roleBrief.level}";
		
		WLequipItems = new List<EquipItem>();
		WLequipItems.Clear();
		for (int i = 0; i < mwolongEquip.transform.childCount; i++)
		{
			WLequipItems.Add(new EquipItem(mwolongEquip.transform.GetChild(i).gameObject, i, 2, true));
		}
		
		mWLEquipData = new Dictionary<int, BagItemInfo>();
		mWLEquipData.Clear();
		for (int i = 0; i < checkRoleInfo.equips.Count; i++)
		{
			bag.EquipInfo equipInfo = checkRoleInfo.equips[i];
			if (equipInfo.position >= 101 && equipInfo.position <= 112)
			{
				mWLEquipData.Add(checkRoleInfo.equips[i].position, checkRoleInfo.equips[i].equip);
			}
		}
		
		if (mWLEquipData != null)
		{
			for (int i = 0; i < WLequipItems.Count; i++)
			{
				EquipItem item = WLequipItems[i];
				bag.BagItemInfo info;
				if (mWLEquipData.ContainsKey(wolongEquipIndex[i]))
				{
					info = mWLEquipData[wolongEquipIndex[i]];
					WLequipItems[i].RefreshItem(mWLEquipData[wolongEquipIndex[i]]);
				}
				else
				{
					WLequipItems[i].UnInit();
				}
			}
		}
		

		//普通道具格子数据
		mEquipData = new Dictionary<int, BagItemInfo>();
		mEquipData.Clear();
		for (int i = 0; i < checkRoleInfo.equips.Count; i++)
		{
			bag.EquipInfo equipInfo = checkRoleInfo.equips[i];
			if (equipInfo.position >= 1 && equipInfo.position <= 12)
			{
				mEquipData.Add(checkRoleInfo.equips[i].position, checkRoleInfo.equips[i].equip);
			}
		}
		
		//武器
		bag.BagItemInfo weaponInfo;
		if (mEquipData != null && mEquipData.ContainsKey(1))
		{
			weaponInfo = mEquipData[1];
			CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
				ItemTableManager.Instance.GetItemModel(weaponInfo.configId).ToString(), ResourceType.UIWeapon);
		}

		//衣服
		bag.BagItemInfo clothesInfo;
		if (mEquipData != null && mEquipData.ContainsKey(2))
		{
			clothesInfo = mEquipData[2];
			CSEffectPlayMgr.Instance.ShowUIEffect(mCloth,
				ItemTableManager.Instance.GetItemModel(clothesInfo.configId).ToString(), ResourceType.UIPlayer);
			mRole.SetActive(false);
			mCloth.SetActive(true);
		}
		else
		{
			mRole.SetActive(true);
			mCloth.SetActive(false);
			string str = checkRoleInfo.roleBrief.sex == 1 ? "615000" : "625000";
			CSEffectPlayMgr.Instance.ShowUIEffect(mRole, str, ResourceType.UIPlayer);
		}
	}

	void SetRightData()
	{
		int wolongLv = checkRoleInfo.roleBrief.wolongLevel;
		mlb_dragonLevel.text = wolongLv.ToString();
		maxId = WoLongLevelTableManager.Instance.GetMaxId();
		mmaxState.SetActive(wolongLv == maxId);
		for (int i = 0; i < attrList.Count; i++)
		{
			attrList[i].Refresh(i + 1, wolongLv, wolongLv == maxId);
		}
	}

	void OnClickToNormal(GameObject go)
	{
		UIManager.Instance.CreatePanel<UICheckInfoCombinePanel>(p =>
		{
			(p as UICheckInfoCombinePanel).OpenChildPanel((int) UICheckInfoCombinePanel.ChildPanelType.CPT_Role);
		});
	}

	void OnClickHelp(GameObject go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WoLongLevel);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CSEffectPlayMgr.Instance.Recycle(mWeapon);
		CSEffectPlayMgr.Instance.Recycle(mCloth);
		CSEffectPlayMgr.Instance.Recycle(mRole);
		WLequipItems = null;
		for (int i = 0; i < attrList.Count; i++)
		{
			CSEffectPlayMgr.Instance.Recycle(attrList[i].GetEffObj());

		}
		CSEffectPlayMgr.Instance.Recycle(meffect);
	}
}
