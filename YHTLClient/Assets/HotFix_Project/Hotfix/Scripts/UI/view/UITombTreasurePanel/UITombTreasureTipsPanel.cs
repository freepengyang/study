using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UITombTreasureTipsPanel : UIBasePanel
{
	CSBetterLisHot<shizhenRewardsData> checkAllRewards = new CSBetterLisHot<shizhenRewardsData>();
	List<UIItemBase> itemList = new List<UIItemBase>();
	public override void Init()
	{
		base.Init();
		AddCollider();
		checkAllRewards = CSStonetreasureInfo.Instance.GetCheckAllRewards();
	}
	public override void Show()
	{
		base.Show();
		RefreshSize();
		RefreshIcon();
	}
	private void RefreshSize()
	{	
		int lineNum = (int)Math.Ceiling((float)checkAllRewards.Count/mGrid.MaxPerLine);
		UIWidget wid = mBG.GetComponent<UIWidget>();
		if (lineNum < 4)
		{
			wid.height = 50 + 75 * lineNum;
			SetPosition(mPosition,lineNum);
			SetPosition(mView, lineNum);
		}
		else
		{
			wid.height = 365;
			ResetPosition(mPosition);
			ResetPosition(mView);
			if (lineNum > 4)
			{
				mScrollBar.onChange.Add(new EventDelegate(OnRareChange)); ;
			}
		}
	}
	private void OnRareChange()
	{
		Transform directionTrs = mDirection.transform;
		directionTrs.GetChild(0).gameObject.SetActive(mScrollBar.value >= 0.05);
		directionTrs.GetChild(1).gameObject.SetActive(mScrollBar.value <= 0.95);
	}
	private void SetPosition(GameObject _go,int _lineNum)
	{
		float x, y, z;
		x = _go.transform.localPosition.x;
		y = _go.transform.localPosition.y;
		z = _go.transform.localPosition.z;
		_go.transform.localPosition = new Vector3(x, y - 75 * (4 - _lineNum)+80, z);
	}
	private void ResetPosition(GameObject _go)
	{
		_go.transform.localPosition = Vector3.zero;
	}
	private void RefreshIcon()
	{
		Transform itemParent, itemFlagGet,itemFlag;
		Color textColor;
		int type;
		mGrid.MaxCount = checkAllRewards.Count;
		for (int i = 0; i < checkAllRewards.Count; i++)
		{
			itemFlag = mGrid.controlList[i].transform.GetChild(0);
			itemParent = mGrid.controlList[i].transform.GetChild(1);
			itemFlagGet = mGrid.controlList[i].transform.GetChild(2);
			itemList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, itemParent, itemSize.Size66));
			itemList[i].Refresh(checkAllRewards[i].itemId);
			textColor = checkAllRewards[i].getNum >= checkAllRewards[i].sum ? CSColor.red : CSColor.green;
			if (checkAllRewards[i].getNum < checkAllRewards[i].sum)
				itemList[i].SetCount($"{checkAllRewards[i].sum - checkAllRewards[i].getNum}/{checkAllRewards[i].sum}", textColor);
			itemFlagGet.gameObject.SetActive(checkAllRewards[i].getNum >= checkAllRewards[i].sum);
			type = ShiZhenRewardTableManager.Instance.GetShiZhenRewardType(checkAllRewards[i].id);
			itemFlag.gameObject.SetActive(type == 4);
		}
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if(itemList != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(itemList);
			itemList.Clear();
			itemList = null;
		}
	}
}