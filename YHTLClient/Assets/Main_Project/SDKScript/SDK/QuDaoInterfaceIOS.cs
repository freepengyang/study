using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Main_Project.SDKScript.SDK;
#if UNITY_IPHONE
public class QuDaoInterfaceIOS : QuDaoInterface
{
    #region 
    public override bool getIsLowMemoryState()
    {
        throw new NotImplementedException();
    }
    public override string getDeviceBrand()
    {
        throw new NotImplementedException();
    }
    public override long getMemoryThreshold()
    {
        throw new NotImplementedException();
    }
    public override long getProcessMemoryInfo()
    {
        throw new NotImplementedException();
    }
    public override string getSystemLanguage()
    {
        throw new NotImplementedException(); 
    }
    public override string getSystemModel()
    {
        return "";
    }
    public override string getSystemVersion()
    {
        throw new NotImplementedException();
    }
    public override long systemAvaialbeMemorySize()
    {
        throw new NotImplementedException();
    }
    
    public override string GetYSDKRefreshData()
    {
        throw new NotImplementedException();
    }
    #endregion

    public override void Login()
    {
        //Debug.Log("QuDaoInterfaceIOS----------Login");
        IOSQuDao.Login();
    }

    public override void SlientLogin()
    {
        IOSQuDao.IOSSlientLogin();
    }

    public override void Login(string info)
    {
        //Debug.Log("QuDaoInterfaceIOS----------Login");
        IOSQuDao.Login();
    }

    public override void ChangeAccount()
    {
        //Debug.Log("--QuDaoInterfaceIOS---------SwitchAccount---------");
        IOSQuDao.IOSSwitchAccount();
    }

    public override void Logout()
    {
        //Debug.Log("QuDaoInterfaceIOS--------LogOut");
        IOSQuDao.IOSLoginOut();
    }

    public override void SubmitGameData(int extraType, ExtraGameData gameData)
    {
        //TODO:ddn
        string json = encodeGameData(gameData);

        IOSQuDao.IOSSubmitGamedata(json);
    }

    public override string GetVersion()
    {
        return IOSQuDao.IOSGetAppVersion();
    }

    public override void JumpAppStore()
    {
        IOSQuDao.IOSJumpAppstore();
    }

    public override void FuKuan(QuDaoPayParams data)
    {
        //TODO:ddn
        string info = encodePayParams(data);
        IOSQuDao.FuKuan(info);
    }

    //隐藏悬浮窗
    public override void SetFloatWindowState(bool isShow)
    {
        IOSQuDao.IOSSetFloatWindowState();
    }

    public override string GetPhoneType()
    {
        return IOSQuDao.IOSGetPhoneType();
    }

    public override string GetAndroidID()
    {
        return IOSQuDao.IOSGetPhoneID();
    }

    public override void CopyFile(string sourcePath, string targetPath)
    {
    }

    public override string GetServerConfigData()
    {
        return IOSQuDao.IOSGetServerConfigData();
    }

    public override string GetServerVersionURL()
    {
        return IOSQuDao.IOSGetServerConfigURL();
    }

    public override void VerificationPlayerInfo(string host)
    {
        IOSQuDao.IOSVerificationPlayerInfo(host);
    }

    public override void RestartGame()
    {

    }

    public override void BindPhone()
    {
        IOSQuDao.IOSBindPhone();
    }

    public override void SubmitBuyData(BuyItemInfo data)
    {
        string json = EncodeItemInfo(data);
        IOSQuDao.IOSSubmitBuydata(json);
    }

    public override void SendToken(string token)
    {
    }

    public override string GetPushClientid()
    {
        return IOSQuDao.IOSGetPushClientId();
    }

    public override bool SetPushTags(string tag)
    {
        UnityEngine.Debug.LogWarning("该功能在android手机端实现");
        return false;
    }

    public override void turnOffPush()
    {
        IOSQuDao.IOSPushTurnOff();
    }

    public override void turnOnPush()
    {
        IOSQuDao.IOSPushTurnOn();
    }

    public override bool GetPushTurnedOnState()
    {
        UnityEngine.Debug.LogWarning("该功能在android手机端实现");
        return false;
    }

    public override string GetDefaultPackageName()
    {
        return string.Empty;
    }

    public override string GetPackageName()
    {
        return IOSQuDao.IOSGetPackageName();
    }

    public override int GetBattery()
    {
        int level = (int)(IOSQuDao.IOSGetBatteryLevel() * 100);
        return level;
    }

    public override string GetDeviceId()
    {
        return IOSQuDao.IOSGetPhoneID();
    }

    private string encodePayParams(QuDaoPayParams data)
    {
        Dictionary<string, object> tempmap = new Dictionary<string, object>();

        tempmap.Add("app_User_Id", data.app_User_Id);
        tempmap.Add("game_Role_Id", data.game_Role_Id);
        tempmap.Add("notify_Uri", data.notify_Uri);
        tempmap.Add("amount", data.amount);
        tempmap.Add("app_Ext1", data.app_Ext1);
        tempmap.Add("app_Ext2", data.app_Ext2);
        tempmap.Add("app_name", data.app_name);
        tempmap.Add("app_order_Id", data.app_order_Id);
        tempmap.Add("app_user_Name", data.app_user_Name);
        tempmap.Add("product_Id", data.product_Id);
        tempmap.Add("sid", data.sid);
        tempmap.Add("serverName", data.serverName);
        tempmap.Add("product_name", data.product_name);
        tempmap.Add("sign", data.sign);
        tempmap.Add("roleLevel", data.roleLevel);
        tempmap.Add("remainder", data.remainder);
        tempmap.Add("vipLevel", data.vipLevel);

        return MiniJSON.Json.Serialize(tempmap);
    }

    private string encodeGameData(ExtraGameData data)
    {
        Dictionary<string, object> tempmap = new Dictionary<string, object>();
        tempmap.Add("dataType", data.dataType);
        tempmap.Add("roleID", data.roleID);
        tempmap.Add("roleName", data.roleName);
        tempmap.Add("roleLevel", data.roleLevel);
        tempmap.Add("serverID", data.serverID);
        tempmap.Add("serverName", data.serverName);
        tempmap.Add("moneyNum", data.moneyNum);
        tempmap.Add("vipLevel", data.vipLevel);
        tempmap.Add("vipExp", data.vipExp);
        tempmap.Add("guildName", data.guildName);
        tempmap.Add("createRoleTime", data.createRoleTime);
        tempmap.Add("updateRoleTime", data.updateLevelTime);

        tempmap.Add("guildLevel", data.guildLevel);
        tempmap.Add("guildID", data.guildID);
        tempmap.Add("guildLeaderName", data.guildLeaderName);
        tempmap.Add("rolePower", data.rolePower);
        tempmap.Add("professionID", data.professionID);
        tempmap.Add("profession", data.profession);
        tempmap.Add("professionRoleName", data.professionRoleName);
        tempmap.Add("sex", data.sex);
        tempmap.Add("remainder", data.remainder);

        return MiniJSON.Json.Serialize(tempmap);
    }

    private string EncodeItemInfo(BuyItemInfo info)
    {
        Dictionary<string, object> tempmap = new Dictionary<string, object>();
        tempmap.Add("roleName", info.roleName);
        tempmap.Add("roleID", info.roleID);
        tempmap.Add("serverID", info.serverID);

        tempmap.Add("coinCount", info.coinCount);
        tempmap.Add("bindCoinCount", info.bindCoinCount);
        tempmap.Add("reminCoinCount", info.reminCoinCount);
        tempmap.Add("reminBindCounCount", info.reminBindCounCount);
        tempmap.Add("buyItemCount", info.buyItemCount);
        tempmap.Add("itemName", info.itemName);
        tempmap.Add("itemDesc", info.itemDesc);
        return MiniJSON.Json.Serialize(tempmap);
    }
    
    public override long getMemoryLimitResidue()
    {
        FNDebug.LogWarning("该功能在android手机端实现");
        return 0;
    }

    public override void FinishGame()
    {
        FNDebug.LogWarning("该功能在android手机端实现");
    }

    public override int GetChannelId()
    {
        return IOSQuDao.IOSGetChannelId();
    }

    public override void CopyTextToClipboard(string text)
    {
        IOSQuDao.IOScopyTextToClipboard(text);
    }
    
    public override string GetUnityCPath(string name)
    {
        return IOSQuDao.GetUnityCPath(name);
    }

    public override int GetScreenDisplayCutout()
    {
        FNDebug.LogWarning("该功能在android手机端实现");
        return 0;
    }
    
    public override string GetDefaultLoginServerType()
    {
        //IOS等后续需求再看是否需要该功能
        return "0";
    }
}
#endif