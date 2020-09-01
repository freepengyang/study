using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Main_Project.SDKScript.SDK;
using UnityEngine;


/// <summary>
/// U8SDK Android平台接口的调用
/// </summary>
public class SDKInterfaceAndroid : QuDaoInterface
{
    private AndroidJavaObject jo;

    public SDKInterfaceAndroid()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    private T SDKCall<T>(string method, params object[] param)
    {
        try
        {
            return jo.Call<T>(method, param);
        }
        catch (Exception e)
        {
            FNDebug.LogError(e);
        }

        return default(T);
    }

    private void SDKCall(string method, params object[] param)
    {
        try
        {
            jo.Call("runOnUiThread", new AndroidJavaRunnable(delegate() { jo.Call(method, param); }));
        }
        catch (Exception e)
        {
            FNDebug.LogError("=============================" + e);
        }
    }

    public override void Login()
    {
        SDKCall("login");
    }

    public override void Login(string info)
    {
        SDKCall("login", info);
    }

    public override void JumpAppStore()
    {
    }

    /// <summary>
    /// 登录页面点击用户
    /// </summary>
    public override void ChangeAccount()
    {
        SDKCall("changeAccount");
    }

    /// <summary>
    /// 返回登录
    /// </summary>
    public override void Logout()
    {
        SDKCall("logout");
    }

    //隐藏悬浮窗
    public override void SetFloatWindowState(bool isShow)
    {
        SDKCall("setFloatWindowState", isShow);
    }

    public override string GetServerConfigData()
    {
        try
        {
            return jo.Call<string>("getServerConfigData");
        }
        catch (Exception e)
        {
            FNDebug.LogError("SDKInterfaceAndroid  GetServerConfigData error");
        }

        return "";
    }

    public override string GetServerVersionURL()
    {
        try
        {
            return jo.Call<string>("GetServerVersionURL");
        }
        catch (Exception e)
        {
            FNDebug.LogError("SDKInterfaceAndroid  GetServerVersionURL error");
        }

        return "";
    }

    /// <summary>
    /// 玩家数据收集
    /// </summary>
    /// <param name="extraType">1;//选择服务器,2;//创建角色,3;//进入游戏,4;//等级提升,5;//退出游戏</param>
    public override void SubmitGameData(int extraType, ExtraGameData gameData)
    {
        string json = encodeGameData(gameData);
        SDKCall("submitExtendData", json);
    }

    public override void SubmitBuyData(BuyItemInfo data)
    {
        string json = EncodeItemInfo(data);
        SDKCall("submitmyBuyData", json);
    }

    public override string GetVersion()
    {
        try
        {
            return jo.Call<string>("getVersionCode");
        }
        catch (Exception e)
        {
            FNDebug.LogError("SDKInterfaceAndroid  GetVersion error");
        }

        return "";
    }

    public override void FuKuan(QuDaoPayParams data)
    {
        //TODO:ddn
        string json = encodePayParams(data);
        SDKCall("pay", json);
    }

    public override void CopyFile(string sourcePath, string targetPath)
    {
        string json = EncodeFilePath(sourcePath, targetPath);

        SDKCall("copyFile", json);
    }

    public override void RestartGame()
    {
        SDKCall("restartGame");
    }

    public override void BindPhone()
    {
        SDKCall("showBindPhonePage");
    }

    private string encodeGameData(ExtraGameData data)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();
        map.Add("dataType", data.dataType);
        map.Add("userID", data.userID);
        map.Add("roleID", data.roleID);
        map.Add("roleName", data.roleName);
        map.Add("roleLevel", data.roleLevel);
        map.Add("serverID", data.serverID);
        map.Add("serverName", data.serverName);
        map.Add("moneyNum", data.moneyNum);
        map.Add("vipLevel", data.vipLevel);
        map.Add("vipExp", data.vipExp);
        map.Add("createRoleTime", data.createRoleTime);
        map.Add("updateRoleTime", data.updateLevelTime);

        map.Add("guildLevel", data.guildLevel);
        map.Add("guildID", data.guildID);
        map.Add("guildName", data.guildName);
        map.Add("guildLeaderName", data.guildLeaderName);
        map.Add("rolePower", data.rolePower);
        map.Add("professionID", data.professionID);
        map.Add("profession", data.profession);
        map.Add("professionRoleID", 0);
        map.Add("professionRoleName", data.professionRoleName);
        map.Add("sex", data.sex);
        map.Add("extToken", CSConstant.extToken);
        map.Add("mainTaskId", data.mainTaskId);

        return MiniJSON.Json.Serialize(map);
    }

    private string encodePayParams(QuDaoPayParams data)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();

        map.Add("app_User_Id", data.app_User_Id);
        map.Add("game_Role_Id", data.game_Role_Id);
        map.Add("app_user_Name", data.app_user_Name);
        map.Add("notify_Uri", data.notify_Uri);
        map.Add("amount", data.amount);
        map.Add("app_Ext1", data.app_Ext1);
        map.Add("app_Ext2", data.app_Ext2);
        map.Add("app_name", data.app_name);
        map.Add("app_order_Id", data.app_order_Id);
        map.Add("product_Id", data.product_Id);
        map.Add("sid", data.sid);
        map.Add("serverName", data.serverName);
        map.Add("product_name", data.product_name);
        map.Add("product_desc", data.product_desc);
        map.Add("vipLevel", data.vipLevel);
        map.Add("roleLevel", data.roleLevel);
        map.Add("guildName", data.UnionName);
        map.Add("createTime", data.CreateTime);
        map.Add("balance", data.balance);
        map.Add("sign", data.sign);
        map.Add("extToken", CSConstant.extToken);
        return MiniJSON.Json.Serialize(map);
    }

    /// <summary>
    /// 刷新应用宝充值数据
    /// </summary>
    /// <returns></returns>
    public override string GetYSDKRefreshData()
    {
        try
        {
            string str = SDKCall<string>("getYSDKRefreshData");

            if (string.IsNullOrEmpty(str)) return "";

            object obj = MiniJSON.Json.Deserialize(str);

            if (obj == null) return "";

            return SDKCall<string>("getYSDKRefreshData");
        }
        catch
        {
        }

        return "";
    }


    public override string GetAndroidID()
    {
        return SDKCall<string>("getAndroidID");
    }

    private string EncodeFilePath(string srcPath, string targetPath)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();
        map.Add("SourcePath", srcPath);
        map.Add("TargetPath", targetPath);

        return MiniJSON.Json.Serialize(map);
    }

    private string EncodeItemInfo(BuyItemInfo info)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();

        map.Add("coinCount", info.coinCount);
        map.Add("bindCoinCount", info.bindCoinCount);
        map.Add("reminCoinCount", info.reminCoinCount);
        map.Add("reminBindCounCount", info.reminBindCounCount);
        map.Add("buyItemCount", info.buyItemCount);
        map.Add("itemName", info.itemName);
        map.Add("itemDesc", info.itemDesc);
        return MiniJSON.Json.Serialize(map);
    }

    public override void SendToken(string token)
    {
        try
        {
            SDKCall("SendToken", token);
        }
        catch
        {
        }
    }

    public override string GetPushClientid()
    {
        return SDKCall<string>("getPushClientid");
    }

    public override bool SetPushTags(string tag)
    {
        try
        {
            return jo.Call<bool>("setPushTags", tag);
        }
        catch
        {
            return false;
        }
    }

    public override void turnOffPush()
    {
        SDKCall("turnOffPush");
    }

    public override void turnOnPush()
    {
        SDKCall("turnOnPush");
    }

    public override bool GetPushTurnedOnState()
    {
        try
        {
            return jo.Call<bool>("getPushTurnedOnState");
        }
        catch
        {
            return false;
        }
    }

    public override string GetDefaultPackageName()
    {
        return SDKCall<string>("getDefaultPackageName");
    }

    public override string GetPackageName()
    {
        return SDKCall<string>("getPackageName");
    }

    public override int GetBattery()
    {
        try
        {
            int level = jo.Call<int>("getBattery");
            return level;
        }
        catch
        {
            return -1;
        }
    }

    public override string GetDeviceId()
    {
        return SDKCall<string>("getDeviceId");
    }

    public override string getSystemLanguage()
    {
        return SDKCall<string>("getSystemLanguage");
    }

    public override string getSystemVersion()
    {
        return SDKCall<string>("getSystemVersion");
    }

    public override string getSystemModel()
    {
        return SDKCall<string>("getSystemModel");
    }

    public override string getDeviceBrand()
    {
        return SDKCall<string>("getDeviceBrand");
    }

    public override long systemAvaialbeMemorySize()
    {
        return SDKCall<long>("getSystemAvailableMemorySize");
    }

    public override long getProcessMemoryInfo()
    {
        return SDKCall<long>("GetProcessMemoryInfo");
    }

    public override long getMemoryThreshold()
    {
        return SDKCall<long>("getMemoryThreshold");
    }

    public override bool getIsLowMemoryState()
    {
        try
        {
            return jo.Call<bool>("isLowMemory");
        }
        catch
        {
            return false;
        }
    }

    public override long getMemoryLimitResidue()
    {
        return SDKCall<long>("getMemoryLimitResidue");
    }

    public override void SlientLogin()
    {
        UnityEngine.Debug.LogError("ios 实现方法，Android 未定义！");
    }

    public override void VerificationPlayerInfo(string host)
    {
        UnityEngine.Debug.LogError("ios 实现方法，Android 未定义！");
    }

    public override string GetPhoneType()
    {
        UnityEngine.Debug.LogError("ios 实现方法，Android 未定义！");

        return "";
    }

    public override void FinishGame()
    {
        SDKCall("finishGame");
    }

    public override int GetChannelId()
    {
        try
        {
            return SDKCall<int>("getChannelId");
        }
        catch
        {
            return 0;
        }
    }

    public void RestartUnity()
    {
        SDKCall("restartUnity");
    }

    public override void CopyTextToClipboard(string text)
    {
        SDKCall("copyTextToClipboard", text);
    }

    public override string GetUnityCPath(string name)
    {
        try
        {
            return SDKCall<string>("getUnityCPath", name);
        }
        catch
        {
            return "";
        }
    }

    public override int GetScreenDisplayCutout()
    {
        try
        {
            return SDKCall<int>("getScreenDisplayCutout");
        }
        catch
        {
            return 0;
        }
    }
    
    public override string GetDefaultLoginServerType()
    {
        try
        {
            return SDKCall<string>("getDefaultLoginServerType");
        }
        catch
        {
            return "0";
        }
    }

}