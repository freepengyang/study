using System;
using System.Collections.Generic;
using UnityEngine;


public partial class UIWelfareVIPMapPanel : UIBasePanel
{
	private FastArrayElementFromPool<UIItemBase> items;
	private List<List<int>> grid;
	public override void Init()
	{
		base.Init();
		CSEffectPlayMgr.Instance.ShowUITexture(mbanner23, "banner23");
		UIEventListener.Get(mbtn_vip).onClick = OnVipClick;
		UIEventListener.Get(mbtn_enter).onClick = OnEnterClick;
		string gridStr = InstanceTableManager.Instance.GetInstanceShow(333101);
		grid = UtilityMainMath.SplitStringToIntLists(gridStr);
		
	}

    

    public override void Show()
	{
		base.Show();
		
        RefreshUI();
        
    }
	
	protected override void OnDestroy()
	{
		items.Clear();
		items = null;
		grid = null;
        CSEffectPlayMgr.Instance.Recycle(mbanner23);
        base.OnDestroy();
	}

	private void RefreshUI()
	{
		int viplv = CSMainPlayerInfo.Instance.VipLevel;
		
		mbtn_vip.SetActive(viplv <= 0);
		mbtn_enter.SetActive(viplv > 0);

		if (items == null)
		{
			items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mGrid.transform,8,itemSize.Size60);
		}
		
		items.Clear();;
		for (int i = 0; i < grid.Count; i++)
		{
			if (grid[i].Count >= 2)
			{
				var item = items.Append();
				item.Refresh(grid[i][0]);
				item.SetCount(grid[i][1]);	
			}	
		}

		mGrid.enabled = true;
	}

	private void OnEnterClick(GameObject obj)
	{
		UtilityPath.FindWithDeliverId(2254);
		UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
	}

	private void OnVipClick(GameObject obj)
	{
		UtilityPanel.JumpToPanel(19000);
		UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
	}
}
