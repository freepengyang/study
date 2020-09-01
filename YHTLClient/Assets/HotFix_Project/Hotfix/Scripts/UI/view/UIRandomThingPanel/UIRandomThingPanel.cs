using System.Collections.Generic;
using UnityEngine;

public partial class UIRandomThingPanel : UIActivityBasePanel
{
	// private enum RandomThingState
	// {
	// 	/// <summary> 未开启</summary>
	// 	NoOpen,
	// 	/// <summary> 开启</summary>
	// 	Open,
	// 	/// <summary> 已结束</summary>
	// 	Finish,
	// }
	//private GameObject mlb_activity;
	//RandomThingState state;
	CSBetterLisHot<TABLE.INSTANCE> dataList;
	int needLv;
	public override void Show()
	{
		base.Show();
		dataList = InstanceTableManager.Instance.GetTableDataByType(15);
		needLv = dataList[0].openLevel;
		RefreshItem(dataList[0].show);
		RefreshUI(10);
	}
	
	
	public override void EnterClick(GameObject obj)
	{
		int playLv = CSMainPlayerInfo.Instance.Level;
		if (playLv >= needLv)
		{
			Net.ReqEnterInstanceMessage(dataList[0].mapId);
			UIManager.Instance.ClosePanel<UIRandomThingPanel>();
		}
		else
		{
			UtilityTips.ShowRedTips(928);
		}
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		dataList.Clear();
		dataList = null;
		needLv = 0;
	}
}
