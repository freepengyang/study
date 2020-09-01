using System.Collections.Generic;
using UnityEngine;

public partial class UIEquipRewardShowPanel : UIBasePanel
{
	int quality, boxId;
	string targetMes;
	int itemId = 0;
	EquipRewardsData mData = new EquipRewardsData();
	List<int> itemList, numList;
	UILabel lb_reward;

	TABLE.SPECIALACTIVEREWARD rewards;
	TABLE.BOX box;
	TABLE.ITEM itemTab;
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}
	public override UILayerType PanelLayerType
	{
		get { return UILayerType.Panel; }
	}
	public override void Init()
	{
		base.Init();
		AddCollider();
		mBtnGo.onClick = OnBtnGo;
		mBtnClose.onClick = OnBtnClose;
		mTog.onChange.Add(new EventDelegate(OnChange));
	}
	public override void Show()
	{
		base.Show();
		RefreshUI();
	}
	public void RefreshUI()
	{
		mData = CSEquipRewardsInfo.Instance.GetRewardData();
		if (mData == null) return;
		SpecialActiveRewardTableManager.Instance.TryGetValue(mData.id, out rewards);
		if (rewards == null) return;
		boxId = rewards.reward;
		targetMes = rewards.termNum;
		if (targetMes != null) int.TryParse(targetMes, out itemId);
		BoxTableManager.Instance.TryGetValue(boxId, out box);
		if (box == null) return;
		if (itemList == null) itemList = new List<int>();
		if (numList == null) numList = new List<int>();
		UtilityMainMath.SplitStringToIntList(itemList, box.item, '&');
		UtilityMainMath.SplitStringToIntList(numList, box.num, '&');
		ItemTableManager.Instance.TryGetValue(itemId, out itemTab);
		if (itemTab == null) return;
		quality = itemTab.quality;

		mLbEquipName.text = itemTab.name;
		mLbEquipName.color = UtilityCsColor.Instance.GetColor(quality);
		mGrid.MaxCount = itemList.Count;
		for (int i=0;i< mGrid.MaxCount;i++)
		{
			lb_reward = mGrid.controlList[i].GetComponent<UILabel>();
			ItemTableManager.Instance.TryGetValue(itemList[i], out itemTab);
			if (itemTab == null) return;
			lb_reward.color = UtilityCsColor.Instance.GetColor(itemTab.quality);
			lb_reward.text = $"{itemTab.name}*{numList[i]}";
		}
	}
	private void OnChange()
	{
		CSEquipRewardsInfo.Instance.SetIsShowTipPanel(!mTog.value);
	}
	private void OnBtnGo(GameObject _go)
	{
		if (mData == null) return;
		if (mData.isWoLong)
		{
			UIManager.Instance.CreatePanel<UIServerActivityPanel>(p =>
			{
				(p as UIServerActivityPanel).SelectChildPanel((int)OpenActivityType.EquipRewards, (int)OpenActivityType.EquipRewards);
			});
		}
		else
		{
			UIManager.Instance.CreatePanel<UIServerActivityPanel>(p =>
			{
				(p as UIServerActivityPanel).SelectChildPanel((int)OpenActivityType.PerEquipRewards, (int)OpenActivityType.PerEquipRewards);
			});
		}
		Close();
	}
	private void OnBtnClose(GameObject _go)
	{
		Close();
	}
	protected override void OnDestroy()
	{
		mData = null;
		itemList = null;
		numList = null;
		rewards = null;
		box = null;
		itemTab = null;
		lb_reward = null;

		targetMes = string.Empty;
		itemId = 0;
		quality = 0;
		boxId = 0;

		base.OnDestroy();
	}
}