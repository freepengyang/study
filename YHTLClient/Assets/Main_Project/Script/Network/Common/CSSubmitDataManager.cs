using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class CSSubmitDataManager : Singleton2<CSSubmitDataManager>
{
    public void StartWrite()
    {
        if (Platform.mPlatformType != PlatformType.EDITOR)
        {
            CoroutineManager.DoCoroutine(WriteRoleNum());
        }
    }

    public void SendSubmitData(SubmitDataType type)
    {
        if (Platform.mPlatformType != PlatformType.EDITOR)
        {
            CoroutineManager.DoCoroutine(SendSubmitDataRequest(type));
        }
    }


    public void SendLoginData()
    {
        if(Platform.mPlatformType != PlatformType.EDITOR)
        {
            CoroutineManager.DoCoroutine(SendLoginDataPost());
        }
    }
    
    IEnumerator WriteRoleNum()
    {
        WWWForm from = new WWWForm();
        try
        {
            from.AddField("action", "queryloginname");
            from.AddField("platform", CSConstant.platformid.ToString());
            from.AddField("type", "update");
            from.AddField("loginname", CSConstant.loginName);
            from.AddField("serverid", CSConstant.mSeverId.ToString());
            from.AddField("rolenum", CSConstant.RoleCount.ToString());
        }
        catch (Exception e)
        {
            FNDebug.LogError("CSSubmitDataManager  WriteRoleNum  error");
        }

        UnityWebRequest www = UnityWebRequest.Post(AppUrlMain.centerUrl, from);
        yield return www.SendWebRequest();
        www.Dispose();
    }

    IEnumerator SendSubmitDataRequest(SubmitDataType type)
    {
        WWWForm from = new WWWForm();
        try
        {
            from.AddField("action", "mobiledata");
            from.AddField("platform", CSConstant.platformid.ToString());
            from.AddField("type", ((int)type).ToString());
            from.AddField("mobile", QuDaoInterface.Instance.GetAndroidID());
            from.AddField("uname", CSConstant.loginName);
            if (Platform.IsAndroid)
            {
                string mImei = QuDaoInterface.Instance.GetDeviceId();
                if (!string.IsNullOrEmpty(mImei)) from.AddField("imei", mImei);
                string systemModel = QuDaoInterface.Instance.getSystemModel();
                string deviceBrand = QuDaoInterface.Instance.getDeviceBrand();
                if (!String.IsNullOrEmpty(systemModel) && !string.IsNullOrEmpty(deviceBrand))
                {
                    from.AddField("model", string.Format("{0}-{1}", deviceBrand, systemModel));
                }
            }
            else
            {
                string phoneType = QuDaoInterface.Instance.GetPhoneType();
                if (!string.IsNullOrEmpty(phoneType))
                    from.AddField("model", phoneType);
                from.AddField("imei", "1");
            }
        }
        catch (Exception e)
        {
            FNDebug.LogError($"CSSubmitDataManager  SendSubmitDataRequest  error  type: {type}  msg:{e.Message}");
        }

        UnityWebRequest www = UnityWebRequest.Post(AppUrlMain.centerUrl, from);

        yield return www.SendWebRequest();
        www.Dispose();
    }
    
    IEnumerator SendLoginDataPost()
    {
        WWWForm from = new WWWForm();
        try
        {
            from.AddField("action", "Mobileloginname");
            from.AddField("platform", CSConstant.platformid.ToString());
            from.AddField("imei", QuDaoInterface.Instance.GetAndroidID());
            from.AddField("loginname", CSConstant.loginName);
            NetworkInterface[] mis = NetworkInterface.GetAllNetworkInterfaces();
            if(mis.Length > 0)
            {
                from.AddField("mac", mis[0].GetPhysicalAddress().ToString());
            }else
            {                
                from.AddField("mac", "");
            }
        }
        catch (Exception e)
        {
            FNDebug.LogError($"CSSubmitDataManager  SendLoginData  error  {e.Message}");
        }
        UnityWebRequest www = UnityWebRequest.Post(AppUrlMain.centerUrl, from);

        yield return www.SendWebRequest();
        www.Dispose();
    }

    public static WWWForm GetQueryloginnameForm()
    {
        WWWForm from = new WWWForm();
        from.AddField("action", "queryloginname");
        from.AddField("platform", CSConstant.platformid);
        from.AddField("type", "query");

        if (!String.IsNullOrEmpty(CSConstant.loginName))
            from.AddField("loginname", CSConstant.loginName);
        else
        {
            string loginName = "";
            if (PlayerPrefs.HasKey("userName"))
                loginName = PlayerPrefs.GetString("userName");
            from.AddField("loginname", loginName);
        }

        return from;
    }
}

public enum SubmitDataType
{
    None,
    SUBMIT_1 = 1,    //启动app
    SUBMIT_2 = 2,    //出现闪屏logo
    SUBMIT_3 = 3,    //开启解压资源包
    SUBMIT_4 = 4,    //解压资源包完成
    SUBMIT_5 = 5,    //启动unity
    SUBMIT_6 = 6,    //进入登录页面弹出登录框
    SUBMIT_7 = 7,    //登录完成
    SUBMIT_8 = 8,    //点击服务器列表
    SUBMIT_9 = 9,    //进入角色列表
    SUBMIT_10 = 10,  //登录游戏
    SUBMIT_11 = 11,  //进入游戏场景
}