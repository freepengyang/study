using System.Collections.Generic;
using System.Data;
using UnityEngine;

public partial class UIServerActivityEquipPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		mClientEvent.Reg((uint)CEvent.SCCollectActivityData, RefreshGrid);
		mClientEvent.Reg((uint)CEvent.ResSpecialActivityDataMessage, RefreshTime);

		mbtn_rule.onClick = OnClickRule;
		CSEffectPlayMgr.Instance.ShowUITexture(mbanner6, "banner6");
	}
	
	public override void Show()
	{
		base.Show();
		mScrollView.ResetPosition();
	}

	void OnClickRule(GameObject go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EquipCollection);
	}

	void RefreshGrid(uint id, object data)
	{
		InitGrid();
	}

	void RefreshTime(uint id, object data)
	{
		if (data == null) return;
		activity.SpecialActivityData msg = data as activity.SpecialActivityData;
		if (msg.activityId==10102)	
		{
			SetTime();
		}
	}

	
	Schedule schedule;
	// private long sec = 0;
	private long leftTime = 0;
	void SetTime()
	{
		if (CSOpenServerACInfo.Instance.Rewards.ContainsKey(10102))
		{
			leftTime = UIServerActivityPanel.GetEndTime(10102);
			// // long endTime = CSOpenServerACInfo.Instance.Rewards[10102].endTime;
			// sec = (endTime - CSServerTime.Instance.TotalMillisecond) / 1000;
			if (Timer.Instance.IsInvoking(schedule))
				Timer.Instance.CancelInvoke(schedule);
			schedule = Timer.Instance.InvokeRepeating(0f, 1f, OnSchedule);
		}
	}
	
	void OnSchedule(Schedule schedule)
	{
		if (leftTime>=0)
		{
			mlb_time.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));
			leftTime--;
		}
		else
		{
			if (Timer.Instance.IsInvoking(schedule))
				Timer.Instance.CancelInvoke(schedule);
		}
	}

	void InitGrid()
	{
		List<activity.CollectActivityData> collectActivityDatas = CSOpenServerACInfo.Instance.EquipCollects;
		if (collectActivityDatas.Count <= 0) return;
		mgrid_equipCollection.MaxCount = collectActivityDatas.Count;
		GameObject gp;
		for (int i = 0; i < mgrid_equipCollection.MaxCount; i++)
		{
			gp = mgrid_equipCollection.controlList[i];
			var eventHandle = UIEventListener.Get(gp);
			UIEquipCollectionItemBinder Binder;
			if (eventHandle.parameter == null)
			{
				Binder = new UIEquipCollectionItemBinder();
				Binder.Setup(eventHandle);
			}
			else
			{
				Binder = eventHandle.parameter as UIEquipCollectionItemBinder;
			}
			
			UIEquipCollectionItemBinderData mData = new UIEquipCollectionItemBinderData();
			mData.reward = collectActivityDatas[i].reward;
			mData.count = collectActivityDatas[i].count;
			mData.goalId = collectActivityDatas[i].goalId;
			Binder.Bind(mData);
		}
	}

	public override void OnHide()
	{
		base.OnHide();
		if (Timer.Instance.IsInvoking(schedule))
			Timer.Instance.CancelInvoke(schedule);
	}

	protected override void OnDestroy()
	{
		CSEffectPlayMgr.Instance.Recycle(mbanner6);
		mgrid_equipCollection.UnBind<UIEquipCollectionItemBinder>();
		if (Timer.Instance.IsInvoking(schedule))
			Timer.Instance.CancelInvoke(schedule);
		
		base.OnDestroy();
	}
}
