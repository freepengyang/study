using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIStrengthenPanel : UIBasePanel
{
	Dictionary<int, List<int>> strongDic;
	BtnItem curBtnItem;
	int itemIdx = 0;
	string[] titList;
	public override void Show()
	{
		base.Show();
		RefreshBtnUI();
	}
	public void SetItemClick(int _idx)
	{
		itemIdx = _idx;
		if (mGridTab.MaxCount > 0)
			ItemClick(mGridTab.controlList[_idx - 1]);
	}
	private void RefreshBtnUI()
	{
		CSStrengthInfo.Instance.SetStrengthenData();
		strongDic = CSStrengthInfo.Instance.GetDealWithStrongDic();
		if(strongDic!= null && strongDic.Count > 0)
		{
			if(titList == null)
				titList = CSStrengthInfo.Instance.GetTitList();
			mGridTab.MaxCount = strongDic.Count;
			for(int i=0;i< mGridTab.MaxCount;i++)
			{
				BtnItem btnItem = mPoolHandleManager.GetCustomClass<BtnItem>();
				btnItem.SetData(mGridTab.controlList[i],i, ItemClick);
				btnItem.SetBtnName(titList[i]);
			}
			if (itemIdx > 0)
				ItemClick(mGridTab.controlList[itemIdx - 1]);
			else
				ItemClick(mGridTab.controlList[0]);
		}
	}
	private void ItemClick(GameObject _go)
	{
		if (curBtnItem != null)
		{
			curBtnItem.ChangeSelectState(false);
		}
		curBtnItem = (BtnItem)UIEventListener.Get(_go).parameter;
		curBtnItem.ChangeSelectState(true);
		RefreshInfo(curBtnItem);
	}
	private void RefreshInfo(BtnItem curBtnItem)
	{
		if(curBtnItem.idx + 1 <= strongDic.Count)
		{
			ScriptBinder tempBinder;
			GameObject btnGo;
			UILabel lb_name, lb_info, lb_tip;
			UIGridContainer gridStar;

			int funcOpenId = 0;
			int playerLv = CSMainPlayerInfo.Instance.Level;
			int funcOpenNeedLevel;
			List<int> infoList = strongDic[curBtnItem.idx + 1];
			TABLE.BESTRONG beStrong = null;
			mGridInfo.MaxCount = infoList.Count;
			for (int i=0;i< infoList.Count;i++)
			{
				BeStrongTableManager.Instance.TryGetValue(infoList[i], out beStrong);
				if (beStrong == null) return;
				tempBinder = mGridInfo.controlList[i].GetComponent<ScriptBinder>();
				btnGo = tempBinder.GetObject("BtnGo") as GameObject;
				lb_name = tempBinder.GetObject("lb_name") as UILabel;
				lb_info = tempBinder.GetObject("lb_info") as UILabel;
				lb_tip = tempBinder.GetObject("lb_tip") as UILabel;
				gridStar = tempBinder.GetObject("GridStar") as UIGridContainer;

				lb_name.text = beStrong.title2.BBCode(ColorType.MainText);
				lb_info.text = beStrong.tips.BBCode(ColorType.SecondaryText);
				gridStar.MaxCount = beStrong.star;
				funcOpenId = beStrong.funcopen;
				if (funcOpenId > 0)
				{
					funcOpenNeedLevel = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(funcOpenId);
					if (playerLv < funcOpenNeedLevel)
					{
						btnGo.CustomActive(false);
						lb_tip.text = CSString.Format(1774, funcOpenNeedLevel).BBCode(ColorType.Red);
					}
					else if (!UICheckManager.Instance.RegBtnAndCheck(funcOpenId, btnGo))
						lb_tip.text = CSString.Format(1775).BBCode(ColorType.Red);
					else
						lb_tip.text = "";
				}
				UIEventListener.Get(btnGo, infoList[i]).onClick = OnGoClick;
			}
			mInfoScrollView.ResetPosition();
		}
	}
	private void OnGoClick(GameObject _go)
	{
		int id = (int)UIEventListener.Get(_go).parameter;
		int npcId = BeStrongTableManager.Instance.GetBeStrongNpc(id);
		int gameModleId = BeStrongTableManager.Instance.GetBeStrongGameModel(id);
		if (gameModleId > 0)
			UtilityPanel.JumpToPanel(gameModleId);
		else if (npcId > 0)
			UtilityPath.FlyToNpc(npcId);

		UIManager.Instance.ClosePanel<UIStrengthenCombinedPanel>();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		curBtnItem = null;
		itemIdx = 0;
	}

	private class BtnItem : IDispose
	{
		GameObject go, checkMark;
		UILabel noName, yesName;
		Action<GameObject> action;
		UIEventListener goBtn;
		public int idx = 0;
		public void SetData(GameObject _go,int _idx, Action<GameObject> _action)
		{
			go = _go;
			action = _action;
			idx = _idx;
			checkMark = go.transform.Find("checkmark").gameObject;
			noName = go.transform.Find("no_name").GetComponent<UILabel>(); 
			yesName = go.transform.Find("checkmark/yes_name").GetComponent<UILabel>();
			goBtn = UIEventListener.Get(go, this);
			goBtn.onClick = action;
		}
		public void SetBtnName(string name)
		{
			yesName.text = name;
			noName.text = name;
		}
		public void ChangeSelectState(bool _state)
		{
			checkMark.CustomActive(_state);
		}
		public void Dispose()
		{
			idx = 0;
			if(goBtn != null)
			{
				goBtn.onClick = null;
				goBtn = null;
			}
			go = null;
			checkMark = null;
			noName = null;
			yesName = null;
			action = null;
		}
	}
}