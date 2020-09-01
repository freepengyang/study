using UnityEngine;
using System.Collections;

public enum MsgBoxType
{
    MBT_OK = 0,
    MBT_CANCEL = 1,
    MBT_CLOSE = 2,
}

public partial class UISummonPanel : UIBasePanel
{
    System.Action<int,bool> cb;
    float time;
    float MaxTime;
    long callBackId;

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public  override void Init()
    {
        base.Init();

        mClientEvent.AddEvent(CEvent.CloseSummonPanel, ClosePanelEvent);

        mBtnCancel.onClick = OnCancel;
        mBtnOK.onClick = OnOK;
    }

    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Tips;
        }
    }

    protected void OnTimeDown()
    {
        var deltaTime = Time.realtimeSinceStartup - time;

        mSlider.value = 1.0f - Mathf.Clamp01(deltaTime / MaxTime);

        if (Time.realtimeSinceStartup >= MaxTime + time)
        {
            ScriptBinder.StopInvokeRepeating();
            cb?.Invoke((int)MsgBoxType.MBT_CANCEL, true);
            TryToClose();
        }
    }

    protected override void OnDestroy()
    {
        cb = null;
    }

    public void RefreshUI(string desc, System.Action<int, bool> callback, int time,long ID)
    {
        if (null != mDesc)
            mDesc.text = desc;

        this.cb = callback;
        MaxTime = time;
        this.time = Time.realtimeSinceStartup;
        callBackId = ID;
        if (time > 0)
            ScriptBinder.InvokeRepeating(0, 0.01f, OnTimeDown);
    }

    private void OnCancel(GameObject go = null)
    {
        cb?.Invoke((int)MsgBoxType.MBT_CANCEL, true);
        TryToClose();
    }

    private void OnOK(GameObject go)
    {
        cb?.Invoke((int)MsgBoxType.MBT_OK, true);
        TryToClose();
    }

    private void SetBg()
    {
        if(null != mBG && null != mDesc)
        {
            mBG.width = mDesc.width + 20;
        }
    }

    void TryToClose()
    {
        CSSummonMgr.Instance.TryToClose(this);
    }


    void ClosePanelEvent(uint id, object param)
    {
        TryToClose();
    }

}
