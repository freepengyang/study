
using UnityEngine;

public partial class UIFastAccessWoLongPanel : UIBasePanel
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	ILBetterList<GetWayData> getWayList;
	int itemId = 0;
	public enum ShowState
	{
		Exp = 0,
		Wolong = 1,
	}
	public override void Init()
	{
		base.Init();
		AddCollider();
		mBtnClose.onClick = OnBtnClose;
		mTips2Btn.onClick = OnBtnItem;
	}
	public override void Show()
	{
		base.Show();
	}
	public void RefreshUI(int playLv,int needLv,int showState)
	{
		int titleId = 0;
		int tips1Id = 0;
		int tips2Id = 0;
		//获取来源
		switch(showState)
		{
			case (int)ShowState.Exp:
				itemId = 5;
				titleId = 1909;
				tips1Id = 1911;
				tips2Id = 1913;
				break;
			case (int)ShowState.Wolong:
				itemId = 11;
				titleId = 1910;
				tips1Id = 1912;
				tips2Id = 1914;
				break;
		}
		if(itemId>0)
		{
			//文本显示
			string itemName = ItemTableManager.Instance.GetItemName(itemId);
			mTitle.text = CSString.Format(titleId);
			mLbTips1.text = CSString.Format(tips1Id, needLv, playLv);
			mLbTips2.text = CSString.Format(tips2Id, $"[u]{itemName}[/u]");
			//获取途径
			if (getWayList == null)
				getWayList = mPoolHandle.GetSystemClass<ILBetterList<GetWayData>>();
			else
			{
				mPoolHandle.Recycle(getWayList);
				getWayList.Clear();
			}
			string getWayStr = ItemTableManager.Instance.GetItemGetWay(itemId);
			CSGetWayInfo.Instance.GetGetWays(getWayStr, ref getWayList);
			mTextGrid.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
		}
	}
	private void OnBtnClose(GameObject go)
	{
		Close();
	}
	private void OnBtnItem(GameObject _go)
	{
		UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemId);
	}
	protected override void OnDestroy()
	{
		mPoolHandle?.OnDestroy();
		mTextGrid.UnBind<GetWayBtn>();
		getWayList.Clear();
		mPoolHandle = null;
		getWayList = null;
		itemId = 0;
		base.OnDestroy();
	}
}