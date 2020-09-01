using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using System;
using heart;
using UnityEngine;

public class HotFix_Invoke
{
    public static MainEventHanlderManager EventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);

    private static HotFix_Invoke mInstance;
    public static HotFix_Invoke Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new HotFix_Invoke();
            }
            return mInstance;
        }
        set { mInstance = value; }
    }

   #region ILSDKCallback
    public Action mOnQuitGameTips;
    /// <summary>
    /// 退出游戏弹框
    /// </summary>
    public void QuitGameTips()
    {
        if (mOnQuitGameTips != null)
        {
            mOnQuitGameTips();
        }
    }
    
    public Action mOnLogoutAccount;
    public void OnLogoutAccount()
    {
        if (mOnLogoutAccount != null)
        {
            mOnLogoutAccount();
        }
    }
    public Action<string> mReqPay;
    public void ReqPay(string msg)
    {
        if (mReqPay != null)
        {
            mReqPay(msg);
        }
    }
    public Action<int> mACTION_BATTERY_CHANGED;
    public void ACTION_BATTERY_CHANGED(int msg)
    {
        if (mACTION_BATTERY_CHANGED != null)
        {
            mACTION_BATTERY_CHANGED(msg);
        }
    }
    
#endregion

   #region CSServerTime
    public Action<heart.Heartbeat> mRefreshTime;

    public void RefreshTime(Heartbeat rsp)
    {
        if (mRefreshTime != null)
        {
            mRefreshTime(rsp);
        }
    }
    #endregion
}