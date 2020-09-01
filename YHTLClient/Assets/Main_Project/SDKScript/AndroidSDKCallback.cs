using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Main_Project.SDKScript.SDK;

public class AndroidSDKCallback : MonoBehaviour
{
    private static AndroidSDKCallback instance;

    public static AndroidSDKCallback Instance
    {
        get { return instance; }
    }

    public string ServerConfigData;

    private static object _lock = new object();

    public static AndroidSDKCallback InitCallback()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject callback = GameObject.Find("(AndroidSDKCallback)");
                if (callback == null)
                {
                    callback = new GameObject("(AndroidSDKCallback)");
                    UnityEngine.Object.DontDestroyOnLoad(callback);
                    instance = callback.AddComponent<AndroidSDKCallback>();
                }
                else
                    instance = callback.GetComponent<AndroidSDKCallback>();
            }
        }

        return instance;
    }

    /// <summary>
    /// 包含了一些sdk中与时间相关的小功能
    /// </summary>
    /*void Update()
    {
        //当开启异步确认zhifu请求
        if (openSyncPayConfirm)
        {
            time += Time.deltaTime;
            if (time >= 5)
            {
                time = 0;
                startPayConfirm();
            }
        }
    }*/

    public void OnLoginSuc(string jsonData)
    {
        LoginResult data = parseLoginResult(jsonData); //将字符串转换为数据
        if (data == null) return;

        if (QuDaoInterface.Instance.OnLoginSuc != null)
            QuDaoInterface.Instance.OnLoginSuc.Invoke(data);
    }

    private LoginResult parseLoginResult(string str)
    {
        LoginResult data = new LoginResult();

#if UNITY_IOS
		string[] loginInfo = str.Split ('#');
		if (loginInfo.Length > 1) {
			data.openUid = loginInfo[0];
			data.message = loginInfo[1];
			return data;
		}
        return null;
#elif UNITY_ANDROID
        object jsonParsed = MiniJSON.Json.Deserialize(str);
        if (jsonParsed == null) return null;

        Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;
        if (jsonMap.ContainsKey("Code"))
            data.code = int.Parse(jsonMap["Code"].ToString());
        if (jsonMap.ContainsKey("Message"))
            data.message = jsonMap["Message"].ToString();
        if (jsonMap.ContainsKey("OpenUid"))
            data.openUid = jsonMap["OpenUid"].ToString();
        if (jsonMap.ContainsKey("IsBindPhone"))
            data.isBindPhone = bool.Parse(jsonMap["IsBindPhone"].ToString());
        else //数据解析错误
            data.isBindPhone = false;
        return data;
#else
        return null;
#endif
    }

    public void OnAndroidLoginSuccess(string info)
    {
        OnMultiLoginSuc(info);
    }

    public void OnMultiLoginSuc(string info)
    {
        LoginInfo data = GetLoginInfo(info);
        if (data == null) return;

        if (QuDaoInterface.Instance.OnMultiLoginSuc != null)
            QuDaoInterface.Instance.OnMultiLoginSuc.Invoke(data);
    }

    LoginInfo GetLoginInfo(string info)
    {
        object jsonParsed = MiniJSON.Json.Deserialize(info);
        if (jsonParsed == null) return null;

        Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;
        LoginInfo data = new LoginInfo();
        if (jsonMap.ContainsKey("userid"))
        {
            data.userId = CSConstant.platformid + ":" + jsonMap["userid"].ToString();
        }

        if (jsonMap.ContainsKey("Token"))
            data.token = jsonMap["Token"].ToString();
        if (jsonMap.ContainsKey("type"))
            data.loginType = jsonMap["type"].ToString();
        if (jsonMap.ContainsKey("time"))
            data.time = jsonMap["time"].ToString();
        if (jsonMap.ContainsKey("ext"))
            data.ext = jsonMap["ext"].ToString();
        if (jsonMap.ContainsKey("sign"))
            data.sign = jsonMap["sign"].ToString();
        if (jsonMap.ContainsKey("gid"))
            data.gid = jsonMap["gid"].ToString();

        return data;
    }

    /// <summary>
    /// 切换帐号回调
    /// </summary>
    public void OnChangeAccount()
    {
        OnLogoutAccount();
    }

    public void OnLogout()
    {
        OnLogoutAccount();
    }

    /// <summary>
    /// 退出账号
    /// 账号退出时候使用,目前android端切换账号普通使用这个
    /// </summary>
    public void OnLogoutAccount()
    {
        HotFix_Invoke.Instance.OnLogoutAccount();
    }

    //拷贝资源成功、失败回调
    public void CopyFileComplete(string totalSize)
    {
        if (QuDaoInterface.Instance.OnCopySuc != null)
            QuDaoInterface.Instance.OnCopySuc.Invoke(long.Parse(totalSize));
    }

    public void GetConfigDataSuc(string data)
    {
        ServerConfigData = data;
    }

    /// <summary>
    /// 绑定手机号回调
    /// </summary>
    public void BindPhone(string isSuccess)
    {
        if (QuDaoInterface.Instance != null)
        {
            if (QuDaoInterface.Instance.BindPhoneSuccess != null)
                QuDaoInterface.Instance.BindPhoneSuccess.Invoke(true);
        }
    }

    public void GetPayData(string jsonData)
    {
        ReqPay(jsonData);
    }

    public static void ReqPay(string msg)
    {
        HotFix_Invoke.Instance.ReqPay(msg);
    }

    #region 为了防止yinyongbao等渠道付费时候,由于异常操作,导致没有进行完整的付费流程确认,导致付费没有导致,每次登入账号的时候,进行查询

    public void OnPaySuc(string jsonData)
    {
        //if (QuDaoConstant.platformName == QuDaoConstant.YingYongBao ||
        //    QuDaoConstant.platformName == QuDaoConstant.PaPaSDK_YSDK)
        //{
        //    if (payjsonData == null) payjsonData = new List<string>();
        //    payjsonData.Add(jsonData);
        //    if (!openSyncPayConfirm)
        //    {
        //        openSyncPayConfirm = true;
        //    }
        //}
    }

    public List<string> payjsonData;
    bool openSyncPayConfirm = false;

    public void startPayConfirm()
    {
        if (payjsonData != null && payjsonData.Count > 0)
        {
            for (int i = 0; i < payjsonData.Count; i++)
                StartCoroutine(queryPayInfo(payjsonData[i]));
        }
        else
            openSyncPayConfirm = false;
    }

    private IEnumerator queryPayInfo(string msg)
    {
        WWWForm from = new WWWForm();
        from.AddField("MSG", msg);
        WWW www = new WWW(AppUrlMain.rechargeUrl, from);

        yield return www;
        if (www.text == "0")
        {
            try
            {
                payjsonData.Remove(msg);
            }
            catch
            {
            }
        }
    }

    #endregion

    /// <summary>
    /// 调起游戏内部的退出游戏弹框
    /// </summary>
    public void QuitGame()
    {
        //TODO:ddn
        HotFix_Invoke.Instance.QuitGameTips();
    }

    /// <summary>
    /// 电量改变通知
    /// </summary>
    /// <param name="data"></param>
    public void ACTION_BATTERY_CHANGED(string data)
    {
        object jsonParsed = MiniJSON.Json.Deserialize(data);
        if (jsonParsed != null)
        {
            try
            {
                Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;
                int level;
                int scale; //最大上限
                level = int.Parse(jsonMap["intLevel"].ToString());
                scale = int.Parse(jsonMap["intScale"].ToString());
                HotFix_Invoke.Instance.ACTION_BATTERY_CHANGED(level);
                //HotManager.Instance.EventHandler.SendEvent(CEvent.BATTERY_CHANGED, level);
            }
            catch
            {
            }
        }
    }

    //判断OPPO用户是否是从游戏中心登录
    public bool IsLoginFromGameCenter = false;

    public void LoginFormGameCenter(string isLoginFromGameCenter)
    {
        if (isLoginFromGameCenter == null || isLoginFromGameCenter == "") return;
        if (isLoginFromGameCenter == "1") IsLoginFromGameCenter = true;
    }

    //登录时接收Oppo发送的ssoid
    public string SSOID = "";

    public void GetOppoSSOID(string ssoid)
    {
        if (ssoid != null && ssoid != "") SSOID = ssoid;
    }

    public void CheckIsVirtual(string result)
    {
        if (result == "1")
            FNDebug.Log("use virtual");
        else
            FNDebug.Log("use phone");
    }

    public void AntiAddictionVerify(string over18Flag)
    {
        // int.TryParse(over18Flag, out Constant.over18Flag);
    }

    public void ScreenChange(string direction)
    {
        int dir;
        if (int.TryParse(direction, out dir))
        {
            NGUIConstant.SetScreen(dir);
        }
    }
}