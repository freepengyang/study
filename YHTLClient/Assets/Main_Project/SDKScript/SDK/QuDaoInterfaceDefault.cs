using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Main_Project.SDKScript.SDK;

public class QuDaoInterfaceDefault : QuDaoInterface
{

    public override void Login()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void Login(string info)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void ChangeAccount()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetYSDKRefreshData()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }

    public override void Logout()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void SubmitGameData(int extraType, ExtraGameData gameData)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void FuKuan(QuDaoPayParams data)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    //隐藏悬浮窗
    public override void SetFloatWindowState(bool isShow)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetAndroidID()
    {
        FNDebug.LogWarning("该功能在手机端实现");

        return "";
    }

    public override void CopyFile(string sourcePath, string targetPath)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetServerVersionURL()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return default(string);
    }

    public override void RestartGame()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void BindPhone()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void SubmitBuyData(BuyItemInfo data)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void SendToken(string token)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetPushClientid()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }

    public override bool SetPushTags(string tag)
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return true;
    }

    public override void turnOffPush()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override void turnOnPush()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override bool GetPushTurnedOnState()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return true;
    }

    public override string GetDefaultPackageName()
    {
        return string.Empty;
    }

    public override string GetPackageName()
    {
        return string.Empty;
    }

    public override int GetBattery()
    {
        return -1;
    }

    public override string GetDeviceId()
    {
        return string.Empty;
    }

    public override string getSystemLanguage()
    {
        return string.Empty;
    }

    public override string getSystemVersion()
    {
        return string.Empty;
    }

    public override string getSystemModel()
    {
        return CSConstant.phoneModel;
    }

    public override string getDeviceBrand()
    {
        return string.Empty;
    }

    public override long systemAvaialbeMemorySize()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return 0;
    }

    public override long getProcessMemoryInfo()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return 0;
    }

    public override long getMemoryThreshold()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return 0;
    }

    public override bool getIsLowMemoryState()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return false;
    }

    public override void SlientLogin()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetVersion()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }

    public override string GetServerConfigData()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }

    public override void VerificationPlayerInfo(string host)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetPhoneType()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }

    public override long getMemoryLimitResidue()
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return 0;
    }
    public override void JumpAppStore()
    {
        FNDebug.LogWarning("该功能在iOS手机端实现");
    }

    public override void FinishGame()
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override int GetChannelId()
    {
        return 0;
    }

    public override void CopyTextToClipboard(string text)
    {
        FNDebug.LogWarning("该功能在手机端实现");
    }

    public override string GetUnityCPath(string name)
    {
        FNDebug.LogWarning("该功能在手机端实现");
        return "";
    }
    
    public override int GetScreenDisplayCutout()
    {
        FNDebug.LogWarning("该功能在android手机端实现");
        return 0;
    }

    public override string GetDefaultLoginServerType()
    {
        FNDebug.LogWarning("该功能在android手机端实现");
        return "0";
    }
}