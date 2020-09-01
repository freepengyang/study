using UnityEngine;
using System.Runtime.InteropServices;

public class IOSQuDao
{
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern string getAppConfig();
    public static string IOSGetAppConfig()
    {

        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = getAppConfig();
            //UnityEngine.Debug.Log("-----appConfig------" + temp);
            return temp;//getAppConfig();
        }
        return "";
    }

    [DllImport("__Internal")]
    private static extern void LoginClick();
    public static void Login()
    {

        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            //Debug.Log("IOSQuDao-------Login   click");
            LoginClick();
        }
    }

    [DllImport("__Internal")]
    private static extern void SlientLogin();
    public static void IOSSlientLogin()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            SlientLogin();
        }
    }

    [DllImport("__Internal")]
    private static extern void LoginOut();
    public static void IOSLoginOut()
    {

        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            LoginOut();
        }
    }

    [DllImport("__Internal")]
    private static extern void createRole(string roleName, int server);
    public static void IOSCreateRole(string roleName, int server)
    {

        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            createRole(roleName, server);
        }
    }

    [DllImport("__Internal")]
    private static extern void PayClick(string info);
    public static void FuKuan(string info)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            PayClick(info);
        }
    }

    [DllImport("__Internal")]
    private static extern void SwitchAccount();
    public static void IOSSwitchAccount()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            SwitchAccount();
        }
    }

    [DllImport("__Internal")]
    private static extern int CheckIsUnRaring();
    public static int IOSCheckIsUnRaring()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            return CheckIsUnRaring();
        }

        return -1;
    }

    [DllImport("__Internal")]
    private static extern string GetServerConfigData();
    public static string IOSGetServerConfigData()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = GetServerConfigData();
            //UnityEngine.Debug.Log("--------getServerConfigdata-----" + temp);
            return temp;
        }

        return "";
    }

    [DllImport("__Internal")]
    private static extern string GetServerConfigURL();
    public static string IOSGetServerConfigURL()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = GetServerConfigURL();
            //UnityEngine.Debug.Log("-------serverConfigURL------------" + temp);
            return temp; //GetServerConfigURL();
        }

        return "";
    }

    [DllImport("__Internal")]
    private static extern string GetAppVersion();
    public static string IOSGetAppVersion()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = GetAppVersion();
            //UnityEngine.Debug.Log("---------appVersion------" + temp);
            return temp; //GetAppVersion();
        }

        return "";
    }

    [DllImport("__Internal")]
    private static extern void CommitServerID(int id);
    public static void IOSCommitServerID(int id)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            CommitServerID(id);
        }
    }

    [DllImport("__Internal")]
    private static extern void SubmitBuyData(string info);
    public static void IOSSubmitBuydata(string info)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            SubmitBuyData(info);
        }
    }

    [DllImport("__Internal")]
    private static extern void SubmitGameData(string info);
    public static void IOSSubmitGamedata(string info)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            SubmitGameData(info);
        }
    }

    [DllImport("__Internal")]
    private static extern void bindPhone();
    public static void IOSBindPhone()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            bindPhone();
        }
    }

    [DllImport("__Internal")]
    private static extern void VerificationPlayerInfo(string host);
    public static void IOSVerificationPlayerInfo(string host)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            VerificationPlayerInfo(host);
        }
    }

    [DllImport("__Internal")]
    private static extern string getPhoneID();
    public static string IOSGetPhoneID()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = getPhoneID();
            //UnityEngine.Debug.Log("---------phoneID---" + temp);
            return temp;//getPhoneID();
        }

        return null;
    }

    [DllImport("__Internal")]
    private static extern string getPhoneType();
    public static string IOSGetPhoneType()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            string temp = getPhoneType();
            return temp;
        }

        return null;
    }

    [DllImport("__Internal")]
    private static extern float getBatteryLevel();
    public static float IOSGetBatteryLevel()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            float temp = getBatteryLevel();
            return temp;
        }

        return -1;
    }
    [DllImport("__Internal")]
    private static extern void JumpAppstore();
    public static void IOSJumpAppstore()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            JumpAppstore();
        }
    }
    
    [DllImport("__Internal")]
    private static extern void SetFloatWindowState();
    public static void IOSSetFloatWindowState()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            SetFloatWindowState();
        }
    }

    [DllImport("__Internal")]
    private static extern int GetChannelId();
    public static int IOSGetChannelId()
    {
        if(Application.platform != RuntimePlatform.OSXEditor)
        {
           return GetChannelId();
        }
        return 0;
    }

    /* Interface to native implementation */
    [DllImport("__Internal")]
    private static extern void _copyTextToClipboard(string text);

    public static void IOScopyTextToClipboard(string text)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            _copyTextToClipboard(text);
        }
    }

    [DllImport("__Internal")]
    private static extern string GetPushClientId();
    public static string IOSGetPushClientId()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            return GetPushClientId();
        }
        return "";
    }

    [DllImport("__Internal")]
    private static extern void IosPushTurnOff();
    public static void IOSPushTurnOff()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            IosPushTurnOff();
        }
    }

    [DllImport("__Internal")]
    private static extern void IosPushTurnOn();
    public static void IOSPushTurnOn()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            IosPushTurnOn();
        }
    }

    [DllImport("__Internal")]
    private static extern string IosGetPackageName();
    public static string IOSGetPackageName()
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            return IosGetPackageName();
        }
        return "";
    }

    [DllImport("__")]
    private static extern string getUnityCPath(string name);

    public static string GetUnityCPath(string name)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            return getUnityCPath(name);
        }
        return "";
    }
#endif
}
