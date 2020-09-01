using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class UIRandomBtnPanel : UIBasePanel
{
	private Schedule mSchedule;
	private float cdTimeMax = 0f;
	private float curCdTime = 0f;
	private bool isCd;
	int id;
	int need;
	public override void Init()
	{
		base.Init();
		isCd = true;
		mClientEvent.AddEvent(CEvent.ItemListChange, RefreshBtnUIIt);
		mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MoveUIRandomBtnPanel);
		mBtnRandom.onClick = OnBtnUse;
	}
	public override void Show()
	{
		base.Show();
		CSAvatarManager.MainPlayer.SetMoveStateControlled();
		RefreshTween(true);
		RefreshBtnUI();
	}
	private void RefreshTween(bool isShow)
	{
		if (isShow)
		{
			mRootTween.PlayForward();
		}
		else
		{
			mRootTween.PlayReverse();
		}
	}
	private void MoveUIRandomBtnPanel(uint uiEvtID, object data)
	{
		RefreshTween(!(bool)data);
	}
	private void RefreshBtnUIIt(uint uiEvtID, object data)
	{
		RefreshBtnUI();
	}
	private void RefreshBtnUI()
	{
		float.TryParse(SundryTableManager.Instance.GetSundryEffect(464), out cdTimeMax);
		string[] mes = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(456));
		if (mes == null) return;
		id = int.Parse(mes[0]);
		need = int.Parse(mes[1]);
		long have = id.GetItemCount();
		string equipNameColor = "";
		mLbName.text = ItemTableManager.Instance.GetItemName(id);
		equipNameColor = have >= need ? UtilityColor.GetColorString(ColorType.Green) : UtilityColor.GetColorString(ColorType.Red);
		mLbNum.text = $"{equipNameColor}{have}/{need}";
		//mItemIcon.spriteName = ItemTableManager.Instance.GetItemIcon(id);
	}
	private void OnBtnUse(GameObject _go)
	{
		long have = id.GetItemCount();
		if (have >= need)
		{
			if (isCd)
			{
				isCd = false;
				CancelDelayInvoke();
				Net.SCSuiJiDuoBaoMessage();
				mSchedule = Timer.Instance.InvokeRepeating(0f, 0.1f, CDSchedule);
			}
			else { UtilityTips.ShowRedTips(917);}
		}
		else{ Utility.ShowGetWay(id);}
	}
	void CancelDelayInvoke()
	{
		if (Timer.Instance.IsInvoking(mSchedule))
		{
			Timer.Instance.CancelInvoke(mSchedule);
		}
	}
	private void CDSchedule(Schedule schedule)
	{
		curCdTime += 0.1f;
		mCdMask.fillAmount = 1f - curCdTime/ cdTimeMax;
		mCdMaskBlack.fillAmount = 1f - curCdTime / cdTimeMax;
		if (curCdTime >= cdTimeMax && cdTimeMax > 0f)
		{
			CancelDelayInvoke();
			curCdTime = 0f;
			mCdMask.fillAmount = 0f;
			mCdMaskBlack.fillAmount = 0f;
			isCd = true;
		}
	}
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}
	protected override void OnDestroy()
	{
		cdTimeMax = 0f;
		curCdTime = 0f;
		id = 0;
		need = 0; 
		isCd = true;
		CancelDelayInvoke();
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.ItemListChange, RefreshBtnUIIt);
			mClientEvent.RemoveEvent(CEvent.MoveUIMainScenePanel, MoveUIRandomBtnPanel);
		}
		base.OnDestroy();
	}
}