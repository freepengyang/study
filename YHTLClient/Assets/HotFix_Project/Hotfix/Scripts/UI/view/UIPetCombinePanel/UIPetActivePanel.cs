using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UIPetActivePanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		AddCollider();
	}
	public override void Show()
	{
		base.Show();
		RefreshUI();
		CSEffectPlayMgr.Instance.ShowUITexture(mBGTexture, "pet_bg2");
		CSEffectPlayMgr.Instance.ShowUIEffect(mEffect, 17370, 10, false);
	}
	void RefreshUI()
	{
		int weaponModel, clothModel;
		PetBaseInfoData petInfoData = CSPetBasePropInfo.Instance.GetPetInfoData();
		weaponModel = ZhanHunSuitTableManager.Instance.GetZhanHunSuitWeaponmodel(petInfoData.suitId);
		clothModel = ZhanHunSuitTableManager.Instance.GetZhanHunSuitClothesmodel(petInfoData.suitId);
		CSEffectPlayMgr.Instance.ShowUIEffect(mWeaponModel, weaponModel.ToString(), ResourceType.UIWeapon);
		CSEffectPlayMgr.Instance.ShowUIEffect(mClothModel, clothModel.ToString(), ResourceType.UIPlayer);
		//’Ω≥Ë Ù–‘
		CSBetterLisHot<PetBasePropInfoData> allList = CSPetBasePropInfo.Instance.GetAllList();
		allList = CSPetBasePropInfo.Instance.DealWithList(allList);
		mPetName.text = ZhanHunSuitTableManager.Instance.GetZhanHunSuitName(petInfoData.suitId);
		ScriptBinder tempBinder;
		UILabel lb_name, lb_num;
		string strClose = SundryTableManager.Instance.GetSundryEffect(702);
		int id;
		float colseCountDown = 0f;
		float.TryParse(strClose, out colseCountDown);
		mBasicGrid.MaxCount = allList.Count;
		for(int i =0;i< mBasicGrid.MaxCount;i++)
		{
			tempBinder = mBasicGrid.controlList[i].GetComponent<ScriptBinder>();
			lb_name = tempBinder.GetObject("lb_name") as UILabel;
			lb_num = tempBinder.GetObject("lb_num") as UILabel;
			id = allList[i].id;
			if(allList[i].maxValue > 0)
			{
				lb_name.text = allList[i].specialName.BBCode(ColorType.SecondaryText);
				lb_num.text = $"{allList[i].value}-{allList[i].maxValue}".BBCode(ColorType.MainText);
			}
			else
			{
				lb_name.text = allList[i].propName.BBCode(ColorType.SecondaryText);
				lb_num.text = CSPetBasePropInfo.Instance.GetDealWithValue(id, allList[i].value).BBCode(ColorType.MainText);
			}
		}
		mScrAttView.SetDynamicArrowVertical(mDownIcon);
		if (colseCountDown > 0)
			mCSInvoke.Invoke(colseCountDown, CloseEffect);
	}
	private void CloseEffect()
	{
		Close();
	}
	protected override void Close()
	{
		base.Close();
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetBaseActiveEffect,mStartPos.position);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CSEffectPlayMgr.Instance.Recycle(mBGTexture);
		CSEffectPlayMgr.Instance.Recycle(mEffect);
		CSEffectPlayMgr.Instance.Recycle(mWeaponModel);
		CSEffectPlayMgr.Instance.Recycle(mClothModel);
		mCSInvoke.StopInvokeRepeating();
	}
}