using System.Collections.Generic;
using UnityEngine;

public partial class UIDailyNPCCostPanel : UIBasePanel
{
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}

	public override PrefabTweenType PanelTweenType
	{
		get { return PrefabTweenType.NpcDialog; }
	}

	private TABLE.TASKS _taskTab;
	private int itemId;
	private int itemCount;
	private UIItemBar mItemBar;

	public override void Init()
	{
		base.Init();
		AddCollider();
		mbtnClose.onClick = Close;
		mbtn_task.onClick = OnReceiveTaskClick;
		mItemBar = UIDialogBarManager.Instance.Create(mcostroot.gameObject);
	}

	public void Show(TABLE.TASKS taskTab)
	{
		_taskTab = taskTab;
		if(taskTab == null) return;

		ShowTitle();
		ShowContent();
	}

	private void ShowTitle()
	{
		mlb_title.text = NpcTableManager.Instance.GetNpcName(_taskTab.fromNPC);
	}

	private void ShowContent()
	{
		List<List<int>> costList = UtilityMainMath.SplitStringToIntLists(_taskTab.cost, '&', '#');
		int curRound = CSMissionManager.Instance._MissionExtraData.DayCycleNum - 1;

		if (costList.Count >= curRound)
		{
			if(costList[curRound].Count < 2) return;
			itemId = costList[curRound][0];
			itemCount = costList[curRound][1];
			
			
			mlb_say.text = string.Format(mlb_say.FormatStr, itemCount, _taskTab.roundLimit);

			string countStr = $"{_taskTab.roundLimit - curRound}/{_taskTab.roundLimit}";
			countStr = (_taskTab.roundLimit - curRound) > 0
				? countStr.BBCode(ColorType.Green)
				: countStr.BBCode(ColorType.Red);
			mlb_count.text = $"{CSString.Format(1262)}{countStr}";

			InitCostItem(itemId, itemCount);
		}
	}

	void InitCostItem(int cfgId,int needed)
	{
        var itemData = UIDialogBarManager.Instance.Get();
		itemData.cfgId = cfgId;
		itemData.needed = needed;
		itemData.owned = cfgId.GetItemCount();
        itemData.flag = (int)ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL | (int)ItemBarData.ItemBarType.IBT_RED_GREEN;
		if(ItemTableManager.Instance.TryGetValue(cfgId,out TABLE.ITEM itemCfg) && itemCfg.type == (int)ItemType.Money)
		{
			itemData.flag |= (int)ItemBarData.ItemBarType.IBT_ONLY_COST;
		}
        itemData.eventHandle = mClientEvent;
		mItemBar.Bind(itemData);
		//itemData.bgWidth = 116;176
	}

	protected override void OnDestroy()
	{
		if (null != mItemBar)
		{
			UIDialogBarManager.Instance.Recycle(mItemBar.Handle.gameObject);
			mItemBar = null;
		}
	}

	private void OnReceiveTaskClick(GameObject go)
	{
		if (itemCount <= CSItemCountManager.Instance.GetItemCount(itemId))
		{
			Net.CSBuyTaskMessage(_taskTab.id);
			Close();
		}
		else
		{
			string itemName = ItemTableManager.Instance.GetItemName(itemId);
			UtilityTips.ShowRedTips(CSString.Format(965, itemName));
		}
	}
}
