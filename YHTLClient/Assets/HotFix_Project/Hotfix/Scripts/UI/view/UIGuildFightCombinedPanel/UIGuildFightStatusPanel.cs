using System;
using UnityEngine;

public partial class UIGuildFightStatusPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }

    public override void Init()
	{
		base.Init();

		mbtnRank.onClick = OnClickRank;
		//mbtnGoFight.onClick = OnGoFight;
		ScriptBinder.InvokeRepeating(0.0f, 1.0f, Refresh);
		//ScriptBinder.InvokeRepeating2(0.0f, 5.0f, OnRequestSabacState);
		mClientEvent.AddEvent(CEvent.OnGuildFightStateChanged, OnGuildFightStateChanged);

		Refresh();
		Net.CSSabacStateMessage();
	}

	protected void OnRequestSabacState()
	{
		Net.CSSabacStateMessage();
    }

	protected void Refresh()
	{
		var fightMgr = CSGuildFightManager.Instance;
		if (null == fightMgr)
			return;

        var fight = fightMgr.CurrentFight;
        if (null == fight)
            return;
        int stage = fightMgr.GetStage();

        //设置标题
        long time = Math.Max(0, (fight.endTime - CSServerTime.Instance.TotalMillisecond) / 1000);//转换成秒
        mendTime.text = CSString.Format(1081, CSServerTime.Instance.FormatLongToTimeStr(time));

		//设置状态描述
        if (null != mstatus_desc)
        {
			mstatus_desc.text = CSString.Format(stage == 1 ? 1082 : 1080);
		}

		//设置公会名称
		mguildName.CustomActive(stage == 2);
		if(stage == 2)
		{
			if(null != mguildName)
			{
				mguildName.text = string.IsNullOrEmpty(fightMgr.WinGuildName) ? CSString.Format(1078) : fightMgr.WinGuildName.BBCode(ColorType.MainText);
			}
		}

		//设置皇宫大门血条
		mSlider.CustomActive(stage == 1);
		if (stage == 1)
		{
			var percent = fightMgr.GetDoorBloodPercentValue() * 0.01f;
			if (null != mSlider)
			{
				mSlider.value = percent;
			}
			if(null != mSliderPercent)
			{
				mSliderPercent.text = $"{fightMgr.GetDoorBloodPercentValue()}%";
			}
        }
		
		//设置占领图标
		if(null != mStageImage)
		{
			mStageImage.spriteName = stage == 1 ? "1" : "2";
		}

		//设置提示语
		if(null != mHint)
		{
			mHint.text = CSString.Format(stage == 1 ? 1083 : 1084);
		}

		//设置城门链接
		if(null != mbtnDoorLink)
		{
			if(stage == 1)
			{
				mbtnDoorLink.onClick = OnLink2SabacDoor;
				mbtnRank.transform.localPosition = new Vector3(52, -80, 0);
				mbtnGoFight.CustomActive(true);
				mbtnGoFight.onClick = OnLink2SabacDoor;
			}
			else if(stage == 2)
			{
				mbtnDoorLink.onClick = OnLink2SabacPlaced;
				mbtnRank.transform.localPosition = new Vector3(52, -80, 0);
				mbtnGoFight.CustomActive(true);
				mbtnGoFight.onClick = OnLink2SabacPlaced;
			}
			else
			{
				mbtnGoFight.CustomActive(false);
				mbtnGoFight.onClick = null;
				mbtnRank.transform.localPosition = Vector3.zero;
				mbtnDoorLink.onClick = null;
			}
		}
    }

	protected void OnLink2SabacDoor(GameObject go)
	{
		FNDebug.LogFormat("[Link]:OnLink2SabacDoor");
		ActionManager.Instance.Run(EnumAction.ActionQueue,
			ActionManager.Instance.Create(EnumAction.FindMap, CSGuildFightManager.Instance.SabacMapId),
            ActionManager.Instance.Create(EnumAction.FindPos, CSGuildFightManager.Instance.SabacMapId, 122, 138));
    }

    protected void OnLink2SabacPlaced(GameObject go)
	{
		FNDebug.LogFormat("[Link]:OnLink2SabacPlaced");
        ActionManager.Instance.Run(EnumAction.ActionQueue,
		ActionManager.Instance.Create(EnumAction.FindMap, CSGuildFightManager.Instance.SabacPalaceId),
		ActionManager.Instance.Create(EnumAction.FindPos, CSGuildFightManager.Instance.SabacPalaceId, 14, 9));
    }

	protected void OnClickRank(GameObject go)
	{
		UIManager.Instance.CreatePanel<UIGuildScoreListPanel>();
	}

	protected void OnGoFight(GameObject go)
	{
		//CSGuildFightManager.Instance.DeliverToSabacFightZone();
	}
	
	public override void Show()
	{
		base.Show();
		Refresh();
	}

	protected void OnGuildFightStateChanged(uint id,object argv)
	{
		Refresh();
	}

	protected override void OnDestroy()
	{
		mClientEvent.RemoveEvent(CEvent.OnGuildFightStateChanged, OnGuildFightStateChanged);
        base.OnDestroy();
	}
}
