using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICountDownLeavePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override UILayerType PanelLayerType
    {
        get => UILayerType.Resident;
    }

    UILabel mlb_countdown_leave;
    
    int time;

    public System.Action endAction;
    public System.Action cycleAction;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.LeaveInstance, LeaveInstance);
    }

    void LeaveInstance(uint id, object data)
    {
        Close();
    }

    protected override void _InitScriptBinder()
	{
		mlb_countdown_leave = ScriptBinder.GetObject("lb_countdown_leave") as UILabel;
	}

    public override void Show()
    {
        base.Show();
    }

    public void InitData(int second, System.Action action1 = null, System.Action action2 = null)
    {
        if (second <= 0) return;
        time = second;
        endAction = action1;
        cycleAction = action2;
        ScriptBinder.InvokeRepeating(0f, 1f, OnSchedule);
    }

    void OnSchedule()
    {
        if (time<0)
        {
            ScriptBinder.StopInvokeRepeating();
            endAction?.Invoke();
            Close();
            return;
        }
        mlb_countdown_leave.text = time.ToString();
        time--;
        cycleAction?.Invoke();
    }

    protected override void OnDestroy()
    {
        ScriptBinder.StopInvokeRepeating();
        base.OnDestroy();
    }
}
