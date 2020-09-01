using System.Collections.Generic;
using UnityEngine;

public partial class UIExitInstanceCountDownPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get
        {
            return false;
        }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Tips; }
    }

    string presetTipsStr;
    string timeStr;

    long leftTime;

	public override void Init()
	{
		base.Init();

        mClientEvent.AddEvent(CEvent.LeaveInstance, GetLeaveInstance);

        presetTipsStr = ClientTipsTableManager.Instance.GetClientTipsContext(1645);

        mlb_time.text = "";
        //mlb_time.gameObject.SetActive(false);
    }
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{

		base.OnDestroy();
	}

    
    public void SetTimeAndStartCount(long seconds)
    {
        leftTime = seconds;
        //mlb_time.gameObject.SetActive(true);
        ScriptBinder.InvokeRepeating(0f, 1f, RefreshTime);
    }


    void RefreshTime()
    {
        if (leftTime > 0)
        {
            timeStr = CSServerTime.Instance.FormatLongToTimeStrMin(leftTime);
            mlb_time.text = CSString.Format(presetTipsStr, timeStr);
            leftTime--;
        }
        else ClosePanel();
    }


    void ClosePanel()
    {
        ScriptBinder.StopInvokeRepeating();
        UIManager.Instance.ClosePanel<UIExitInstanceCountDownPanel>();
    }

    void GetLeaveInstance(uint id, object data)
    {
        ClosePanel();
    }

}
