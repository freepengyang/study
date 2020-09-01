using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDreamInstancePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override UILayerType PanelLayerType
    {
        get => UILayerType.Resident;
    }

    private long sec = 0;

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ChangeDreamLandTime, ChangeDreamLandTime);
        base.Init();
    }

    void ChangeDreamLandTime(uint id, object data)
    {
        ScriptBinder.StopInvokeRepeating();
        InitData();
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        if (CSDreamLandInfo.Instance.MyDreamLandData!=null)
        {
            sec = CSDreamLandInfo.Instance.MyDreamLandData.myTime / 1000;
            if (sec>=0)
            {
                ScriptBinder.InvokeRepeating(0f, 1f, OnDesParticle);
            }
        }
    }

    void OnDesParticle()
    {
        if (sec>=0)
        {
            mlb_time.text = CSServerTime.Instance.FormatLongToTimeStr(sec);
            sec--;
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
            // Close();
        }
        
    }

    public override void OnHide()
    {
        ScriptBinder.StopInvokeRepeating();
        base.OnHide();
    }

    protected override void OnDestroy()
    {
        ScriptBinder.StopInvokeRepeating();
        base.OnDestroy();
    }
}

