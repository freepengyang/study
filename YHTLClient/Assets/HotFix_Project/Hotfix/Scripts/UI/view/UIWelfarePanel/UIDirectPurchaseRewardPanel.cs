using UnityEngine;

public partial class UIDirectPurchaseRewardPanel : UIBasePanel
{
	private ILBetterList<DirectPurchaseRewardsData> listDirectPurchaseRewardsDatas;
	
	public override void Init()
	{
		base.Init();
		mClientEvent.Reg((uint) CEvent.DailyPurchaseReceive, RefreshData);
		AddCollider();
		mbtn_close.onClick = Close;
	}
	
	void RefreshData(uint id, object data)
	{
		InitData();
	}
	
	public override void Show()
	{
		base.Show();
		InitData();
	}

	void InitData()
	{
		listDirectPurchaseRewardsDatas = CSDirectPurchaseInfo.Instance.ListDirectPurchaseRewardsDatas;
		if (listDirectPurchaseRewardsDatas == null || listDirectPurchaseRewardsDatas.Count <= 0) return;
		mgrid_reward.MaxCount = listDirectPurchaseRewardsDatas.Count;
		GameObject gp;
		for (int i = 0; i < mgrid_reward.MaxCount; i++)
		{
			gp = mgrid_reward.controlList[i];
			var eventHandle = UIEventListener.Get(gp);
			UIDirectPurchaseRewardBinder Binder;
			if (eventHandle.parameter == null)
			{
				Binder = new UIDirectPurchaseRewardBinder();
				Binder.Setup(eventHandle);
			}
			else
			{
				Binder = eventHandle.parameter as UIDirectPurchaseRewardBinder;
			}
			Binder.scrollView = mScrollView;
			DirectPurchaseRewardsData directPurchaseRewardsData = listDirectPurchaseRewardsDatas[i];
			Binder.Bind(directPurchaseRewardsData);
		}
	}

	protected override void OnDestroy()
	{
		mgrid_reward.UnBind<UIDirectPurchaseRewardBinder>();
		base.OnDestroy();
	}
}
