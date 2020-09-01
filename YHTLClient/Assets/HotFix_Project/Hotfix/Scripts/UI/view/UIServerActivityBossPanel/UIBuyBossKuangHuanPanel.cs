using System;
using System.Collections.Generic;
using UnityEngine;
public partial class UIBuyBossKuangHuanPanel : UIBasePanel
{
	UIItemBase itemBase;
	BossKuangHuanData productData;
	UISprite sp_icon;
	UILabel lb_value,lb_num;
	GameObject addBtn, reduceBtn;
	UIInput lbNumInput;
	string costicon = "";
	int costIdx = 1;
	int activityId = 10120;
	int tableId, stockNum, itemId, itemNum, quality, costNum,costCan, limitNum;
	long costHaveNum;
	string itemName;
	public override void Init()
	{
		base.Init();
		AddCollider();

		costicon = CSBossKuangHuanInfo.Instance.GetCostIcon();

		sp_icon = mUIItemBarTotal.transform.GetChild(0).GetComponent<UISprite>();
		lb_value = mUIItemBarTotal.transform.GetChild(1).GetComponent<UILabel>();
		lb_num = mUIItemBarPrefab.transform.GetChild(1).GetComponent<UILabel>();
		lbNumInput = lb_num.GetComponent<UIInput>();

		reduceBtn = mUIItemBarPrefab.transform.GetChild(0).gameObject;
		addBtn = mUIItemBarPrefab.transform.GetChild(2).gameObject;

		mBtnBuy.onClick = OnBtnBuy;
		mBtnClose.onClick = OnBtnClose;
		UIEventListener.Get(reduceBtn, 1).onClick = OnAddOrReduceBtn;
		UIEventListener.Get(addBtn,2).onClick = OnAddOrReduceBtn;
		lbNumInput.onChange.Add(new EventDelegate(CountInputChanged));
	}
	public override void Show()
	{
		base.Show();
		RefreshUI();
	}
	private void RefreshUI()
	{
		productData = CSBossKuangHuanInfo.Instance.GetBossKuangHuanData();
		tableId = productData.tableId;
		stockNum = productData.stockNum;
		itemId = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsRewardId(tableId);
		itemNum = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsPointNum(tableId);
		quality = ItemTableManager.Instance.GetItemQuality(itemId);
		costNum = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsNum(tableId);
		itemName = ItemTableManager.Instance.GetItemName(itemId);
		costHaveNum = CSBossKuangHuanInfo.Instance.GetBossKuangHuanIntegralTicketNum();//拥有的积分券
		costCan = (int)(costHaveNum / costNum);//计算玩家凭积分券可购买的数量
		limitNum = stockNum > costCan ? costCan : stockNum;
		limitNum = limitNum > 0 ? limitNum : 1;
		mLbItemName.text = itemName;
		mLbItemName.color = UtilityCsColor.Instance.GetColor(quality);
		sp_icon.spriteName = costicon;
		lb_value.text = (costIdx * costNum).ToString();
		itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase, itemSize.Size66);
		itemBase.Refresh(itemId);
		itemBase.SetCount(itemNum,Color.white);
	}
	private void OnAddOrReduceBtn(GameObject _go)
	{
		int index = (int)UIEventListener.Get(_go).parameter;
		//减
		if (index == 1)
		{
			if(costIdx > 1)
			{
				costIdx -= 1;
			}
			else
			{
				costIdx = 1;
				UtilityTips.ShowRedTips(1138);
			}
		}
		//加
		else if (index == 2)
		{
			if(costIdx >= limitNum)
			{
				costIdx = limitNum;
				UtilityTips.ShowRedTips(1139);
			}
			else
			{
				costIdx += 1;
			}
			lb_value.text = (costIdx * costNum).ToString();
		}
		lb_num.text = costIdx.ToString();
	}
	private void OnBtnBuy(GameObject _go)
	{
		if( costHaveNum >= costIdx*costNum )
		{
			Net.CSKuangHuanRewardMessage(activityId, tableId, costIdx);
			Close();
		}
		else
		{
			UtilityTips.ShowRedTips(1137);
		}
	}
	private void OnBtnClose(GameObject _go)
	{
		Close();
	}
	private void CountInputChanged()
	{
		if (string.IsNullOrEmpty(lbNumInput.value)) return;
		if (!int.TryParse(lbNumInput.value, out costIdx) || costIdx < 1)
		{
			costIdx = 1;
		}
		if (costIdx > limitNum) costIdx = limitNum;
		lbNumInput.value = costIdx.ToString();
		lb_value.text = (costIdx * costNum).ToString();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (itemBase != null) { UIItemManager.Instance.RecycleSingleItem(itemBase); }
		productData = null;
		sp_icon = null;
		lb_value = null;

		costIdx = 1;
		activityId = 0;
		tableId = 0;
		stockNum = 0;
		itemId = 0;
		itemNum = 0;
		quality = 0;
		costNum = 0;
		costHaveNum = 0;
		costCan = 0;
		limitNum = 0;
		itemName = "";
	}
}