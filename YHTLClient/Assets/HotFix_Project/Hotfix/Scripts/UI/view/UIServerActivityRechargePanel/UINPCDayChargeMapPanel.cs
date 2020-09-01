using System.Collections.Generic;
using UnityEngine;
public partial class UINPCDayChargeMapPanel : UIActivityBasePanel
{
	CSBetterLisHot<TABLE.INSTANCE> dataList;
	public override void Show()
	{
		base.Show();
		RefreshUIInfo();
	}
	private void RefreshUIInfo()
	{
		DayChargeData dayChargeData = CSDayChargeInfo.Instance.GetChargeData();
		UILabel lb_btn = mbtn_enter.transform.GetChild(1).GetComponent<UILabel>();
		dataList = InstanceTableManager.Instance.GetTableDataByType(19);
		RefreshItem(dataList[0].show);
		if (dayChargeData.dayCharge > 0)
			lb_btn.text = CSString.Format(1152);
		else
			lb_btn.text = CSString.Format(1120);
	}
	public override void EnterClick(GameObject obj)
	{
		int num = CSMainPlayerInfo.Instance.RoleExtraValues.todayTimes;
		if (num > 0)
			UtilityPanel.JumpToPanel(12604);
		else
			UtilityPanel.JumpToPanel(12305);
		Close();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		dataList.Clear();
		dataList = null;
	}
}