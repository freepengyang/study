using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ILSDKCallback
{
    public static void Init()
    {
        HotFix_Invoke.Instance.mOnLogoutAccount = OnLogoutAccount;
        HotFix_Invoke.Instance.mReqPay = ReqPay;
        HotFix_Invoke.Instance.mACTION_BATTERY_CHANGED = ACTION_BATTERY_CHANGED;
        HotFix_Invoke.Instance.mOnQuitGameTips = OnQuitGame;
    }

    public static void OnLogoutAccount()
    {
        CSHotNetWork.Instance.LogoutGameAccout();
    }

    public static void ReqPay(string msg)
    {
        Net.ReqRechargeMessage(msg);
    }

    public static void ACTION_BATTERY_CHANGED(int level)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.BATTERY_CHANGED, level);
    }
    
    public static void OnQuitGame()
    {
        FNDebug.Log("调出退出框");
        UtilityTips.ShowPromptWordTips(CSStringTip.EXIT_GAME , null, () =>
        {
            QuDaoInterface.Instance.FinishGame();
        });
    }
}
