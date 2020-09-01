using instance;
using UnityEngine;

public class UIExitInstanceBtn : UIBase
{
    private UILabel _CountDown;

    public UILabel CountDown
    {
        get { return _CountDown ? _CountDown : _CountDown = Get<UILabel>("timeDown"); }
    }

    private bool isHide;

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(UIPrefab).onClick = OnExitInstanceClick;
        mClientEvent.Reg((uint) CEvent.ResInstanceInfo, GetInstanceInfo);
        mClientEvent.Reg((uint) CEvent.GetEnterInstanceInfo, GetEnterInstance);
        mClientEvent.Reg((uint) CEvent.ECM_SCInstanceFinishMessage, GetInstanceFinish);
        mClientEvent.Reg((uint) CEvent.LeaveInstance, GetLeaveInstance);
        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter,OnSetActive);
        
        info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info != null && InstanceTableManager.Instance.GetInstanceType(CSMainPlayerInfo.Instance.MapID)!=2/*不是幻境*/)
        {
            ScriptBinder.InvokeRepeating(0, 1f, TimeCountDown);
        }
    }

    private void OnSetActive(uint uievtid, object data)
    {

        if (CSInstanceInfo.Instance.GetInstanceInfo() != null)
        {
            UIPrefab.SetActive(true);
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
            CountDown.text = "";
            leftTime = 0;
            UIPrefab.SetActive(false);
        }
    }
        

    void TimeCountDown()
    {
        if (leftTime <= 0)
        {
            leftTime = 0;
            ScriptBinder.StopInvokeRepeating();
            CountDown.text = CSString.Format(1130);
            return;
        }

        CountDown.text = CSServerTime.Instance.FormatLongToTimeStrMin(leftTime);
        leftTime--;
    }

    void GetInstanceInfo(uint id, object data)
    {

        //UIPrefab.SetActive(true);
        RefreshTime();
    }

    void GetEnterInstance(uint id, object data)
    {
        //UIPrefab.SetActive(true);
        RefreshTime();
    }

    void GetInstanceFinish(uint id, object data)
    {
        //UIPrefab.SetActive(true);
        RefreshTime();
    }

    void GetLeaveInstance(uint id, object data)
    {
        //isHide = true;
        //UIPrefab.SetActive(false);
        ScriptBinder.StopInvokeRepeating();
    }

    InstanceInfo info;
    long leftTime = 0;

    void RefreshTime()
    {
        /*
        * message InstanceInfo {
            int64 startTime = 1; //副本开始时间
            int32 killedBoss = 2; //杀boss数
            int32 killedMonsters = 3; //杀怪数
            int32 finishCount = 4; //大于0表示副本关闭倒计时
            bool rewarded = 5; //奖励是否已领
            bool success = 6; //挑战是否成功
            int64 usedTime=7; //副本已经过去的时间;
            int32 instanceId = 8;
            int32 state=9; //副本状态;0:create;1:wait;2:started;3:finished;4:closed;
            int64 endTime=10; //结束时间戳;
            }
        */
        info = CSInstanceInfo.Instance.GetInstanceInfo();
        //state == 0 时，是服务器初始状态，，副本还未开始
        if(info.state == 0)
        {
            ScriptBinder.StopInvokeRepeating();
            CountDown.text = "";
            return;
        }
        
        if (info != null)
        {
            if (info.state == 2 || info.state == 0)
            {
                if (info.endTime > 0)
                    leftTime = (info.endTime - CSServerTime.Instance.TotalMillisecond) / 1000;
            }
            else if (info.state == 3)
            {
                leftTime = info.finishCount;
            }
            else if (info.state == 4)
            {
                leftTime = 0;
            }
        }

        if (leftTime != 0 && InstanceTableManager.Instance.GetInstanceType(CSMainPlayerInfo.Instance.MapID)!=2/*不是幻境*/)
        {
            ScriptBinder.InvokeRepeating(0, 1f, TimeCountDown);
        }
        else
        {
            CountDown.text = CSString.Format(1130);
        }
    }

    public override void Show()
    {
        base.Show();
        RefreshCommonInstanceTime();
        UIPrefab.SetActive(true);
        RefreshTime();
    }


    /// <summary>
    /// 进入副本时，优先获取显示时间， 然后再判断服务器是否刷新时间，，RefreshTime方法中，不可将 leftTime初始为0
    /// </summary>
    private void RefreshCommonInstanceTime()
    {
        int timerid = InstanceTableManager.Instance.GetInstanceTimerId(CSScene.GetMapID());
        if (timerid > 0)
        {
            string endTime = TimerTableManager.Instance.GetTimerEndTime(timerid);
            if (!string.IsNullOrEmpty(endTime))
            {
                long end = UtilityMath.CronTimeStringParseToTamp(endTime);
                if (end > 0)
                    leftTime = end - CSServerTime.Instance.TotalSeconds;
            }
        }
    }

    private void OnExitInstanceClick(GameObject go)
    {
        info = CSInstanceInfo.Instance.GetInstanceInfo();
        int type = InstanceTableManager.Instance.GetInstanceType(info.instanceId);
        int promptWord = 24;

        //极限挑战 特殊文字
        if (type == 5)
        {
            promptWord = 25;
        }
		//每日充值地图，小红点检测，向服务端请求数据
		else if(type == 19)
		{
			if (CSDayChargeInfo.Instance.IsSendNet()) { Net.CSDayChargeInfoMessage(); }
		}

        UtilityTips.ShowPromptWordTips(promptWord,null,() => 
        {
            Net.ReqLeaveInstanceMessage(true);
            UIPrefab.SetActive(false);
        });
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ResInstanceInfo, GetInstanceInfo);
        mClientEvent.RemoveEvent(CEvent.GetEnterInstanceInfo, GetEnterInstance);
        mClientEvent.RemoveEvent(CEvent.ECM_SCInstanceFinishMessage, GetInstanceFinish);
        mClientEvent.RemoveEvent(CEvent.LeaveInstance, GetLeaveInstance);
        mClientEvent.RemoveEvent(CEvent.Scene_EnterSceneAfter,OnSetActive);
        base.OnDestroy();
    }
}