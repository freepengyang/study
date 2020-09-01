using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Main_Project.SDKScript.SDK;

public class IOSQuDaoCallback : MonoBehaviour 
{
	private static IOSQuDaoCallback instance;

	public static IOSQuDaoCallback InitCallback()
	{

		if (instance == null) {
			GameObject callbackgo = GameObject.Find ("IOSQuDaoCallback");
			if (callbackgo == null) {
				callbackgo = new GameObject ("IOSQuDaoCallback");
				DontDestroyOnLoad (callbackgo);
				instance = callbackgo.AddComponent<IOSQuDaoCallback> ();
			} else
				instance = callbackgo.GetComponent<IOSQuDaoCallback> ();
		}
		return instance;
	}

	/// <summary>
	/// QuDao初始化成功回调
	/// </summary>
	public void OnQuDaoInitSuc()
	{
	}

    /// <summary>
    /// QuDao登录成功回调
    /// </summary>
    /// <param name="jsonData">登录QuDao成功信息</param>
    public void OnQuDaoLoginSuc(string jsonData)
	{
		object jsonParsed = MiniJSON.Json.Deserialize (jsonData);

		if (jsonParsed != null) 
		{
			Dictionary<string,object> jsonMap = jsonParsed as Dictionary<string,object>;

			LoginInfo tempLoginInfo = new LoginInfo ();

            if (jsonMap.ContainsKey("UserID"))
            {
                tempLoginInfo.userId = CSConstant.platformid + ":" + jsonMap["UserID"].ToString();
            }

            if (jsonMap.ContainsKey ("Token"))
				tempLoginInfo.token = jsonMap ["Token"].ToString ();

			if (jsonMap.ContainsKey ("type"))
				tempLoginInfo.loginType = jsonMap ["type"].ToString ();

			if (jsonMap.ContainsKey ("ext"))
				tempLoginInfo.ext = jsonMap ["ext"].ToString ();

			if (jsonMap.ContainsKey ("Time"))
				tempLoginInfo.time = jsonMap ["Time"].ToString ();

            if (jsonMap.ContainsKey("IsBindPhone"))
                tempLoginInfo.isBindPhone = bool.Parse(jsonMap["IsBindPhone"].ToString());
            else
                tempLoginInfo.isBindPhone = false;

			if (jsonMap.ContainsKey ("IsWhitePaper"))
				tempLoginInfo.isWhitePaper = bool.Parse (jsonMap ["IsWhitePaper"].ToString());
			else
				tempLoginInfo.isWhitePaper = false;

			if (QuDaoInterface.Instance.OnIOSSDKLoginSuc != null) 
			{
                QuDaoInterface.Instance.OnIOSSDKLoginSuc(tempLoginInfo);
			}
		}
	}

	/// <summary>
	/// 玩家退出登录回调
	/// 收到回调玩家返回登录界面
	/// </summary>
	/// <param name="json">退出登录信息</param>
	public void OnQuDaoLogoutSuc(string json)
	{
//		if (QuDaoInterface.Instance.Logout != null)
//			QuDaoInterface.Instance.Logout ();

		HotFix_Invoke.Instance.OnLogoutAccount();
		//CSNetwork.Instance.LogoutGameAccout ();
	}

	/// <summary>
	/// 支付成功回调
	/// </summary>
	/// <param name="jsonData">Json data.</param>
	public void OnQuDaoPaySuc(string jsonData)
	{
        //UnityEngine.Debug.LogError ("-Unity-IOSQuDaoCallback-------------------OnQuDaoPaySuc");
    }

    /// <summary>
    /// 绑定手机成功回调
    /// </summary>
    /// <param name="isSuccess">Success 表示绑定成功</param>
    public void OnQuDaoBindPhoneSuc(string isSuccess)
	{
		if (isSuccess.Equals ("Success")) 
		{
			if (QuDaoInterface.Instance.BindPhoneSuccess != null)
				QuDaoInterface.Instance.BindPhoneSuccess (true);
		}
	}

	/// <summary>
	/// 绑定身份证成功回调
	/// </summary>
	/// <param name="isSuccess">Success 表示绑定成功</param>
	public void OnQuDaoBindIDCardSucc(string isSuccess)
	{
		if (isSuccess.Equals ("Success"))
		{
			if (QuDaoInterface.Instance.BindIDCardSuccess != null)
				QuDaoInterface.Instance.BindIDCardSuccess (true);
		}
	}
}
