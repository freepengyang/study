using System.Collections.Generic;
using UnityEngine;

public partial class UIPetBaseEquipGetWayPanel : UIBasePanel
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	public override void Init()
	{
		base.Init();
		AddCollider();
	}
	public override void Show()
	{
		base.Show();
		RefreshBtnData();
		RefreshUIBtn();
	}
	private void RefreshBtnData()
	{
		int type;
		int curPetSuitId = CSPetBasePropInfo.Instance.GetCurClickSuitId();
		string getWayStr = ZhanHunSuitTableManager.Instance.GetZhanHunSuitGetWay(curPetSuitId);
		string sundryStr;
		ScriptBinder tempBinder;
		UILabel lb_name;
		GameObject go;
		List<int> getWayList = UtilityMainMath.SplitStringToIntList(getWayStr);
		getWayList = Utility.DealWithFirstRecharge(getWayList);
		List<int> sundryList = mPoolHandle.GetSystemClass<List<int>>();
		mBtnGrid.MaxCount = getWayList.Count;
		for (int i=0;i< getWayList.Count;i++)
		{
			go = mBtnGrid.controlList[i];
			tempBinder = go.GetComponent<ScriptBinder>();
			sundryStr = SundryTableManager.Instance.GetSundryEffect(getWayList[i]);
			sundryList = UtilityMainMath.SplitStringToIntList(sundryStr);
			type = sundryList[1];
			lb_name = tempBinder.GetObject("LbName") as UILabel;
			lb_name.text = CSString.Format(sundryList[0]);
			UIEventListener.Get(go, sundryList).onClick = OnClick;
		}
	}
	private void OnClick(GameObject _go)
	{
		List<int> sundryList = (List<int>)UIEventListener.Get(_go).parameter;
		int type = sundryList[1];
		int id = sundryList[2];
		if (type == 1)
			UtilityPath.FindWithDeliverId(id);
		else if (type == 2)
			UtilityPanel.JumpToPanel(id);
		else if (type == 3)
			UtilityPath.FlyToNpc(id);

		UIManager.Instance.ClosePanel<UIPetBaseEquipGetWayPanel>();
		UIManager.Instance.ClosePanel<UIWarPetCombinedPanel>();
	}
	private void RefreshUIBtn()
	{
		float disHeight = mBtnGrid.CellHeight;
		float x, y, z;
		UIWidget bgWid = mBG.GetComponent<UIWidget>();
		UIWidget buttonBgWid = mButtonBg.GetComponent<UIWidget>();
		if (mBtnGrid.MaxCount >=5)
		{
			bgWid.height += (int)(disHeight * 3.5f);
			buttonBgWid.height += (int)(disHeight * 3.5f);
		}
		else if(mBtnGrid.MaxCount > 1)
		{
			bgWid.height += (int)(disHeight * (mBtnGrid.MaxCount-1));
			buttonBgWid.height += (int)(disHeight * (mBtnGrid.MaxCount - 1));
		}
		x = mViewTrs.localPosition.x;
		y = mViewTrs.localPosition.y + disHeight * (mBtnGrid.MaxCount-1);
		z = mViewTrs.localPosition.z;
		mViewTrs.localPosition = new Vector3(x, y,z);
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		mPoolHandle?.OnDestroy();
		mPoolHandle = null;
	}
}