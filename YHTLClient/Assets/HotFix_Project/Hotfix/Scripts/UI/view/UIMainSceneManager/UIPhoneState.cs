using UnityEngine;
using System.Collections;
using System;
using Object = UnityEngine.Object;

public class UIPhoneState : UIBase
{
    private Schedule mPingConnectSchedule = null;
    private Schedule mPingConnectingSchedule = null;
    private Schedule mBatterPowerCheckingSchedule = null;
    private Schedule mRefreshNowTimeSchedule = null;
    private enum PingState
    {
        Low = 1,
        Normal = 2,
        High = 3
    }
    private int mCheckPowerRate = 300; // 电池检测频率间隔（sec）
    //private readonly int mPingLimit = 20;       // ping上限时间(sec)      //Remove Unused
    private int mPingSundryId = 169;

    private TABLE.SUNDRY mtbSundryData = null;
    private int mMinPing = 0;
    private int mMaxPing = 0;

    private int mPingTime = 0;      //ms
    private int mPower = 0;
    private NetworkReachability mSignalState;
    private PingState mPingState;
    //昼夜图标切换
    DateTime nowTime;
    string SpNameDay = "main_sun";
    string SpNameNight = "main_moon";

    private UISprite _spbattery;
    private UISprite mspBattery { get { return _spbattery ?? (_spbattery = Get<UISprite>("battery")); } }
    private UISprite _spbatteryPower;
    private UISprite mspBatteryPower { get { return _spbatteryPower ?? (_spbatteryPower = Get<UISprite>("battery/spr_power")); } }
    private UISprite _spnetWorkState;
    private UISprite mspNetWorkState { get { return _spnetWorkState ?? (_spnetWorkState = Get<UISprite>("network")); } }
    private UILabel _lbDisNetWork;
    private UILabel mlbDisNetWork { get { return _lbDisNetWork ?? (_lbDisNetWork = Get<UILabel>("lb_disnw")); } }
    private UILabel _lbPingNum;
    private UILabel mlbPingNum { get { return _lbPingNum ?? (_lbPingNum = Get<UILabel>("lb_ping")); } }

    private UILabel _lbTime;
    private UILabel mlbTime { get { return _lbTime ?? (_lbTime = Get<UILabel>("lb_time")); } }
    UISprite _sp_dayNight;
    UISprite sp_dayNight { get { return _sp_dayNight ?? (_sp_dayNight = Get<UISprite>("sp_day")); } }


    public override void Init()
    {
        mCheckPowerRate = 300;
        mPingSundryId = 169;
        mSignalState = NetworkReachability.NotReachable;
        mPingState = PingState.High;
        //IsRun1 = true;
        //IsRun2 = true;

        mClientEvent.AddEvent(CEvent.BATTERY_CHANGED, OnBATTERY_CHANGED);

    }

    public override void Show()
    {
        if (mtbSundryData == null)
        {
            if (SundryTableManager.Instance.TryGetValue(mPingSundryId, out mtbSundryData))
            {
                string[] strs = mtbSundryData.effect.Split('#');
                if (strs.Length > 0) int.TryParse(strs[0], out mMinPing);
                if (strs.Length > 1) int.TryParse(strs[1], out mMaxPing);
            }
        }

        StartCheckPing();
        //如果从android端获取不到电量,那么调用以前的电量显示
        if (QuDaoInterface.Instance.GetBattery() == -1)
            StartCheckBatteryPower();
        else
            RefreshUIBatteryPower(QuDaoInterface.Instance.GetBattery());
        //位置放不下  策划要求去掉时间
        //StartShowTime();
        mlbTime.gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        CancelPingConnect();
        Timer.Instance.CancelInvoke(mPingConnectSchedule);
        Timer.Instance.CancelInvoke(mPingConnectingSchedule);
        Timer.Instance.CancelInvoke(mBatterPowerCheckingSchedule);
        Timer.Instance.CancelInvoke(mRefreshNowTimeSchedule);

        _spbatteryPower = null;
        _lbPingNum = null;
        _spnetWorkState = null;
        mClientEvent?.UnRegAll();
        mClientEvent = null;
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.BATTERY_CHANGED, OnBATTERY_CHANGED);
    }

    private void StartShowTime()
    {
        if (!Timer.Instance.IsInvoking(mRefreshNowTimeSchedule))
            mRefreshNowTimeSchedule = Timer.Instance.InvokeRepeating(0, 1f, (fun) => { RefreshNowTime(); });
    }

    //
    //与心跳包同步检测，15s
    //
    public void StartCheckPing()
    {
        mPingTime = 0;
        mPingCount = 0;
        Constant.gamePing = new Ping(CSNetwork.Instance.Host);
        if (!Timer.Instance.IsInvoking(mPingConnectingSchedule))
            mPingConnectingSchedule = Timer.Instance.InvokeRepeating(0, 0.1f, (fun) => { PingConnecting(); DayAndNightState(); });
    }

    public void StartCheckBatteryPower()
    {
        if (!Timer.Instance.IsInvoking(mBatterPowerCheckingSchedule))
            mBatterPowerCheckingSchedule = Timer.Instance.InvokeRepeating(0, mCheckPowerRate, (fun) => { BatterPowerChecking(); });
    }

    private int mPingCount = 0;
    private void PingConnecting()
    {
        if (Constant.gamePing == null) return;

        if (Constant.gamePing.isDone)
        {
            RefreshUIPingNetWork(Constant.gamePing.time);
            CancelPingConnect();
        }
        else if (mPingCount > 20)
        {
            RefreshUIPingNetWork(-1);
            CancelPingConnect();
        }

        mPingCount++;
    }

    void DayAndNightState()
    {
        nowTime = CSServerTime.Instance.ServerNows;
        if (6 <= nowTime.Hour && nowTime.Hour <= 18)
        {
            sp_dayNight.spriteName = SpNameDay;
        }
        else
        {
            sp_dayNight.spriteName = SpNameNight;
        }
    }

    private void CancelPingConnect()
    {
        mPingCount = 0;
        if (Constant.gamePing != null)
        {
            Constant.gamePing.DestroyPing();
            Constant.gamePing = null;
        }
        Timer.Instance.CancelInvoke(mPingConnectSchedule);
    }

    private void OnBATTERY_CHANGED(uint uiEvtID, object data)
    {
        RefreshUIBatteryPower(int.Parse(data.ToString()));
    }

    private void BatterPowerChecking()
    {
        int power = ToolPhoneStateGet.GetBatteryLevel();
        RefreshUIBatteryPower(power);
    }

    private void RefreshUIPingNetWork(int pingTime)
    {
        NetworkReachability signal = ToolPhoneStateGet.GetSignal();
        PingState pingState = mPingState;

        if (mlbPingNum != null && mPingTime != pingTime)
        {
            if (pingTime >= 0)
            {
                string color = string.Empty;
                //if (pingTime <= mMinPing)
                //{
                //    pingState = PingState.Low;
                //    color = "[00ff00]";
                //}
                //else if (pingTime >= mMaxPing)
                //{
                //    pingState = PingState.High;
                //    color = "[ff0000]";
                //}
                //else
                //{
                //    pingState = PingState.Normal;
                //    color = "[fbd671]";
                //}
                if (Constant.gamePing.time < 200)
                {
                    pingState = PingState.Low;
                    color = "[00ff00]";
                }
                else if (200 <= Constant.gamePing.time && Constant.gamePing.time <= 800)
                {
                    color = "[fbd671]";
                }
                else
                {
                    color = "[ff0000]";
                }
                mlbPingNum.text = string.Format("{0}{1} ms", color, Constant.gamePing.time > 999 ? 999 : Constant.gamePing.time);
                mlbPingNum.gameObject.SetActive(true);
            }
            else
            {
                mlbPingNum.gameObject.SetActive(false);
            }
        }

        mPingState = pingState;
        mSignalState = signal;
        mPingTime = pingTime;
    }

    private void RefreshUIBatteryPower(int powerNum)
    {
        if (mspBatteryPower != null && powerNum != mPower)
        {
            if (powerNum >= 0)
            {
                float value = powerNum * 0.01f;
                mspBatteryPower.fillAmount = value;
                mPower = powerNum;
                mspBattery.gameObject.SetActive(true);
            }
            else
            {
                mspBattery.gameObject.SetActive(false);
            }
        }
        mPower = powerNum;
    }

    //bool IsRun1 = true;
    //bool IsRun2 = true;
    private void RefreshNowTime()
    {
        if (CSServerTime.Instance.TotalSeconds != 0)
        {
            mlbTime.text = CSServerTime.Instance.GetNowTimeToTimeString().BBCode(ColorType.White);
            mlbTime.gameObject.SetActive(true);
        }
        else
            mlbTime.gameObject.SetActive(false);
    }



}