using UnityEngine;
using System.Collections;
using System;
using AssetBundles;
using Main_Project.Script.Update;
using Main_Project.SDKScript.SDK;
using UnityEngine.Networking;

public class ServerViewCtrl : UIBase
{
    public static string host = string.Empty;
    public static int gamePort = 0;

    private UILabel mServerIp = null;
    private UISprite mServerIpSpr = null;
    private GameObject btn_enter = null;
    private GameObject btn_choose = null;
    private int mServerIndex = 0;
    private int mRecommendServerIndex = 0;
    private UnityWebRequest www = null;
    private bool isStart = false;
    private UIInput CeShiServerId;
    private UIInput CeShiPhoneModel;
    private UILabel serverState = null;

    private Coroutine mEnterFailTipsCoroutine;

    public void Start()
    {
#if UNITY_EDITOR
        CeShiServerId = Get<UIInput>("center/view/CeShiServerId");
        if (CeShiServerId != null) CeShiServerId.gameObject.SetActive(false);
        CeShiPhoneModel = Get<UIInput>("center/view/CeShiPhoneModel");
        if (CeShiPhoneModel != null) CeShiPhoneModel.gameObject.SetActive(false);
#endif
        mServerIp = Get<UILabel>("center/view/serverIp");
        mServerIpSpr = Get<UISprite>("center/view/serverIp/Sprite");
        btn_enter = Get("center/view/btn_enter").gameObject;
        btn_choose = Get("center/view/btn_choose").gameObject;
        serverState = Get<UILabel>("center/view/serverIp/lb_state");

        UIEventListener.Get(Get("top_left/event/btn_notice").gameObject).onClick = OnOfficialNoticeClick;
        UIEventListener.Get(Get("top_left/event/btn_xy").gameObject).onClick = OnOfficialXYClick;
        UIEventListener.Get(Get("top_left/event/btn_switchuser").gameObject).onClick = OnChangerAccountClick;
        
        //UIEventListener.Get(Get("serverview/top_left/event/btn_notice").gameObject).onClick = OnNoticeClick;
        
        if (QuDaoConstant.GetPlatformData() != null && !QuDaoConstant.GetPlatformData().switchAccount)
        {
            Transform tempTrans = Get("top_left/event/btn_switchuser");
            if (tempTrans != null)
            {
                tempTrans.gameObject.SetActive(false);
            }
        }

        mClientEvent.Reg((uint)CEvent.Connect, OnLoginSucceed);
        mClientEvent.Reg((uint)CEvent.ConnectFail, OnLoginFail);
        mClientEvent.Reg((uint)CEvent.Updae_ServerId, RefreshAddress);

        if (btn_enter != null) UIEventListener.Get(btn_enter).onClick = EnterGameClick;
        if (btn_choose) UIEventListener.Get(btn_choose).onClick = OnButtonClick;

        if (PlayerPrefs.HasKey("ServerIndex")) mServerIndex = PlayerPrefs.GetInt("ServerIndex");
        mRecommendServerIndex = HttpRequest.Instance.GetRecommendServer;
        if (mServerIndex <= 0)
        {
            mServerIndex = mRecommendServerIndex;
            PlayerPrefs.SetInt("ServerIndex", mServerIndex);
        }
        ServerListData s_data = HttpRequest.Instance.CurGameService(mServerIndex);
        StartRequestNotice();
        RefreshAddressInfo(s_data);

        HotMain.UpdateCallBack += Update;
        
        if (!PlayerPrefs.HasKey("allowUIAgreementPanel"))
            OnOfficialXYClick(null);
    }

    public void StartRequestNotice()
    {
        CoroutineManager.DoCoroutine(RequestNotice());
    }

    private IEnumerator RequestNotice()
    {
        UnityWebRequest noticeWWW = UnityWebRequest.Get(AppUrl.noticeUrl);

        yield return noticeWWW.SendWebRequest();

        if (noticeWWW.error == null)
        {
            string wwwText = noticeWWW.downloadHandler.text;
            if (!string.IsNullOrEmpty(noticeWWW.downloadHandler.text))
            {
                Constant.IsHaveNotice = false;
                wwwText = wwwText.Remove(0, 1);
                wwwText = wwwText.Remove(wwwText.Length - 1);
            }
            if (PlayerPrefs.HasKey("ServerNotice"))
            {
                Constant.IsHaveNotice = true;
                string localNotice = PlayerPrefs.GetString("ServerNotice");
                if (!localNotice.Contains(wwwText))
                {
                    PlayerPrefs.SetString("ServerNotice", wwwText);
                    if (!string.IsNullOrEmpty(wwwText)&& PlayerPrefs.HasKey("allowUIAgreementPanel"))
                    {
                        
                        //UIRedPointManager.Instance.IsGetNewNotice = true;
                        UIManager.Instance.CreatePanel<UIOfficialNoticePanel>();
                    }
                }
            }
            else
            {
                Constant.IsHaveNotice = true;
                PlayerPrefs.SetString("ServerNotice", wwwText);
                if (!string.IsNullOrEmpty(wwwText) && PlayerPrefs.HasKey("allowUIAgreementPanel"))
                {
                    //UIRedPointManager.Instance.IsGetNewNotice = true;
                    UIManager.Instance.CreatePanel<UIOfficialNoticePanel>();
                }
            }
        }
        else
        {
            FNDebug.LogErrorFormat("ServerViewCtrl RequestNotice error  {0}  code: {1}    url:{2}", noticeWWW.error,
                noticeWWW.responseCode, noticeWWW.uri);
        }
        noticeWWW.Dispose();
    }

    private void RefreshAddressInfo(ServerListData s_data)
    {
        if (s_data != null && mServerIp != null )
        {
            if(mServerIpSpr != null) mServerIpSpr.spriteName = GetPoints(s_data.S_State);
            serverState.text = GetServerName(s_data.S_State);

            if (s_data.S_State == 1 && mServerIndex == mRecommendServerIndex)
            {
                mServerIp.text = CSString.Format(CSStringTip.NEW_TESXT, s_data.S_Name);
            }
            else
            {
                mServerIp.text = s_data.S_Name;
            }
        }
    }
    
    private string GetServerName(int num)
    {
        if (num == 1) return CSStringTip.SERVER_NEW;//新服
        if (num == 2) return CSStringTip.SERVER_UNBLOCKED;//畅通
        if (num == 3) return CSStringTip.SERVER_FULL;//爆满
        if (num == 4) return CSStringTip.SERVER_MAINTAIN;//维护
        return "";
    }

    private string GetPoints(int num)
    {
        if (num == 1) return "11";//新服
        if (num == 2) return "11";//畅通
        if (num == 3) return "14";//爆满
        return "12";//维护
    }

    private void RefreshAddress(uint id, object data)
    {
        if (data == null) return;

        ServerListData s_data = data as ServerListData;

        if (s_data == null) return;

        PlayerPrefs.SetInt("ServerIndex", s_data.S_ID);
        mServerIndex = s_data.S_ID;
        RefreshAddressInfo(s_data);
        
        ChechUpdate(s_data);
    }
    
    private void ChechUpdate(ServerListData s_data)
    {
        if(SFOut.IsLoadLocalRes)
        {
            EnterGame();
            return;
        }
        if(s_data == null) return;
        
        if(QuDaoConstant.ChannelCloseRegister || s_data.S_Type != "0" && !s_data.S_OpenRegis)
        {
            bool isRole = false;
            ILBetterList<ServerListData> list = HttpRequest.Instance.GetMySevrverList();
            for (var i = 0; i < list.Count; i++)
            {
                if(list[i].S_OnlyID == s_data.S_OnlyID)
                {
                    isRole = true;
                    break;
                }
            }
            if(!isRole)
            {
                if(QuDaoConstant.ChannelCloseRegister)
                {                    
                    UtilityTips.ShowPromptWordTips(CSStringTip.CHANNEL_CLOSE, "", CSStringTip.CONFIG, null, null);
                }else
                {
                    UtilityTips.ShowPromptWordTips(CSStringTip.SERVER_CLOSE, "", CSStringTip.CONFIG, null, null);
                }
                return;
            }
        }
        if(CSConstant.ServerType != s_data.S_Type)
        {
            UtilityTips.ShowPromptWordTips(CSStringTip.RESTART_GAME, CSStringTip.CANCEL, CSStringTip.CONFIG, null, ()=>
            {
                CSConstant.ServerType = s_data.S_Type;
                PlayerPrefs.SetString("loginServerType", CSConstant.ServerType);
                
                if(PlayerPrefs.HasKey("loginServerType"))
                {
                    UnityEngine.Debug.LogError($"存储服务器类型： {PlayerPrefs.GetString("loginServerType")}");
                }else
                {
                    UnityEngine.Debug.LogError($"服务器类型存储不成功： {CSConstant.ServerType}");
                }
                QuDaoInterface.Instance.RestartGame();
            });
            return;
        }
        UIManager.Instance.OpenPanel<UIWaiting>().Show(false);
        CSVersionManager.Instance.CheckVersion(s_data.S_Type, OnCheckFinish);
    }
    
    private void OnCheckFinish(UpdateCheckCode checkCode)
    {
        UIManager.Instance.ClosePanel<UIWaiting>();

        if(checkCode == UpdateCheckCode.Update)
        {
            float count = CSResUpdateManager.Instance.preDownloadByteNum * 1.0f / 1024 / 1024;
            string size = count.ToString("F2");
            UtilityTips.ShowPromptWordTips(CSStringTip.UPDATE_TIPS, CSStringTip.CANCEL, CSStringTip.UPDATE,null, () =>
            {
                UIManager.Instance.CreatePanel<UIDownloading>((f) =>
                {
                    (f as UIDownloading).RefreshData(DownloadUIType.Download);
                });
            }, size);
        }else if(checkCode == UpdateCheckCode.ServerListError)
        {
            UtilityTips.ShowRedTips(CSStringTip.RESOURCELISTMD5ERROR);
        }else
        {
            EnterGame();
        }
    }

    private void OnButtonClick(GameObject gp)
    {
        UIManager.Instance.CreatePanel<UIServerListPanel>();
    }

    private IEnumerator EnterFailTips(string tips)
    {
        yield return new WaitForSeconds(5f);
        UtilityTips.ShowRedTips(tips);
        if (mServerIp != null)
        {
            UnityEngine.Debug.LogError(CSConstant.loginName + " " + mServerIp.text + " " + tips + " 登入不了本服 Server= " + " ipFromClient = " + host + " portFromClient=" + gamePort);
        }
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGameClick(GameObject go)
    {
        //EnterGame();
        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_8);
        ServerListData s_data = HttpRequest.Instance.CurGameService(mServerIndex);
        if(s_data == null)
        {
            UnityEngine.Debug.LogError($"获取服务器信息为空    mServerIndex ： {mServerIndex}");
            return;
        }
        ChechUpdate(s_data);
    }

    private void EnterGame()
    {
        mServerIndex = PlayerPrefs.GetInt("ServerIndex");
        ServerListData s_data = HttpRequest.Instance.CurGameService(mServerIndex);
        if (s_data != null)
        {
            Constant.mServerName = s_data.S_Name;
            CSConstant.mOnlyServerId = s_data.S_OnlyID;
            CSConstant.mSeverId = s_data.S_ID;
            QuDaoConfig mCurPlatform = QuDaoConstant.GetPlatformData();

            if (mCurPlatform == null)
            {
                UtilityTips.ShowRedTips(1643);
                return;
            }
            host = s_data.S_DomainID;
            gamePort = s_data.S_Port + Constant.AddValue;
            int state = s_data.S_State;

            //bool isWhite = AdminUserTableManager.Instance.IsAdminUser();
            bool isWhiteIp = HttpRequest.Instance.IsAdminUser(Constant.mWhiteListIp);

            if (state != 4 || isWhiteIp)
            {
                if (string.IsNullOrEmpty(host)) return;

                UIManager.Instance.OpenPanel<UIWaiting>().Show(false);

                switch (Platform.mPlatformType)
                {
                    case PlatformType.EDITOR:
                        ConnectNetwork(CSStringTip.NETWORKERROR_409);
                        break;
                    case PlatformType.ANDROID:
                        QuDaoConfig mConfig = QuDaoConstant.GetPlatformData();
                        if (mConfig != null)
                        {
                            if (mConfig.requestCode == RequestCode.Normal)
                            {
                                RequestDataAndroid();
                            }
                            else if (mConfig.requestCode == RequestCode.DirectConnection)
                            {
                                ConnectNetwork(CSStringTip.NETWORKERROR_411);
                            }
                        }
                        else
                        {
                            ConnectNetwork(CSStringTip.NETWORKERROR_412);
                        }
                        break;
                    case PlatformType.IOS:
                        //if (CSVersionMgr.Instance.IsAppStorePassed() || QuDaoInterface.Instance.NeedVerification)
                        //    QuDaoInterface.Instance.VerificationPlayerInfo(host);
                        //else
                        //{
                        //    ConnectNetwork("连接失败2");
                        //}
                        ConnectNetwork(CSStringTip.NETWORKERROR_410);
                        break;
                }
            }
            else
            {
                UtilityTips.ShowTips(CSStringTip.NETWORKERROR_1644);
            }
        }
    }

    void Update()
    {
        if (isStart && www != null && www.isDone)
        {
            if (www.error != null)
            {
                QuDaoConstantHot.LinkCheckState = false;
                UIManager.Instance.ClosePanel<UIWaiting>();
            }
            else
            {
                CorrectSuccess();
            }

            isStart = false;
            www.Dispose();
            www = null;
        }
#if UNITY_EDITOR
        GM();
#endif

    }

    void GM()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CeShiServerId != null) CeShiServerId.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CeShiServerId != null) CeShiServerId.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (CeShiPhoneModel != null) CeShiPhoneModel.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (CeShiPhoneModel != null) CeShiPhoneModel.gameObject.SetActive(false);
        }
    }

    void RequestDataAndroid()
    {
        QuDaoConstantHot.LinkCheckState = true;//通过了链接状态
        string loginUrl = string.Format(AppUrl.platformLoginUrl, Constant.platformid);
        WWWForm form = new WWWForm();
        form.AddField("token", Constant.mToken);

        if (!string.IsNullOrEmpty(CSConstant.loginName))
            form.AddField("username", CSConstant.loginName);

        if (!string.IsNullOrEmpty(Constant.loginSign))
            form.AddField("sign", Constant.loginSign);

        if (!string.IsNullOrEmpty(Constant.loginType))
            form.AddField("type", Constant.loginType);

        if (!string.IsNullOrEmpty(Constant.loginExt))
            form.AddField("ext", Constant.loginExt);

        if (!string.IsNullOrEmpty(Constant.gameID))
            form.AddField("gid", Constant.gameID);

        www = UnityWebRequest.Post(loginUrl, form);
        isStart = true;
        if (FNDebug.developerConsoleVisible) FNDebug.LogError("登录数据： " + CSConstant.loginName + "=============" + Constant.mToken);
    }

    /// <summary>
    /// 矫正服务器成功
    /// </summary>
    private void CorrectSuccess()
    {
        if (FNDebug.developerConsoleVisible) FNDebug.LogError("================================================================" + www.downloadHandler.text);

        //Android端判定判断是否绕过了二次检测
        if (Platform.IsAndroid && !QuDaoConstantHot.LinkCheck()) return;

        LoginNormalReq tempReq = LoginNormalParam.parseLoginResult(www.downloadHandler.text);
        CSConstant.loginName = tempReq.loginName;
        Constant.time = tempReq.time;
        PlayerPrefs.SetString("userName", tempReq.loginName);

        ConnectNetwork(CSStringTip.NETWORKERROR_413);
    }

    public void ConnectNetwork(string errorTip)
    {
        CSNetwork.Instance.Connect(host, gamePort);
        if (mEnterFailTipsCoroutine != null)
        {
            CoroutineManager.StopCoroutine(mEnterFailTipsCoroutine);
        }
        mEnterFailTipsCoroutine = CoroutineManager.DoCoroutine(EnterFailTips(errorTip));
        if (FNDebug.developerConsoleVisible) FNDebug.Log("host = " + host + " gamePort = " + gamePort);
    }

    public void OnLoginSucceed(uint id, object data)
    {
        PlayerPrefs.SetInt("ServerIndex", mServerIndex);
        CSConstant.mSeverId = mServerIndex;
        string str = CSConstant.loginName;

        if (string.IsNullOrEmpty(str))
        {
            //Utility.ShowTips(100076);
            return;
        }

        //Android端判定判断是否绕过了二次检测
        if (Platform.IsAndroid && !QuDaoConstantHot.LinkCheck()) return;


        if (Platform.mPlatformType == PlatformType.EDITOR) str = str.Replace(Environment.NewLine, "");


        if (QuDaoConstant.isEditorMode() && !string.IsNullOrEmpty(Constant.loginServerId))
            Int32.TryParse(Constant.loginServerId, out CSConstant.mOnlyServerId);
#if UNITY_EDITOR
        if (CeShiServerId != null && !String.IsNullOrEmpty(CeShiServerId.value))
        {
            Int32.TryParse(CeShiServerId.value, out CSConstant.mOnlyServerId);
        }
        if (CeShiPhoneModel != null && !string.IsNullOrEmpty(CeShiPhoneModel.value))
        {
            Constant.phoneModel = CeShiPhoneModel.value;
        }
#endif
        Net.ReqLoginMessage(str, Constant.platformid, CSConstant.mOnlyServerId, Constant.sign, Constant.time, QuDaoInterface.Instance.getSystemModel());
    }

    public void OnLoginFail(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIWaiting>();
    }

    /// <summary>
    /// 公告
    /// </summary>
    /// <param name="obj"></param>
    void OnOfficialNoticeClick(GameObject obj)
    {
        //ceshi -------暂时注释掉
        UIManager.Instance.CreatePanel<UIOfficialNoticePanel>();

    }

    /// <summary>
    /// 协议
    /// </summary>
    /// <param name="obj"></param>
    void OnOfficialXYClick(GameObject obj)
    {
        UIManager.Instance.CreatePanel<UIAgreementPanel>();
    }

    /// <summary>
    /// 切换账号
    /// </summary>
    /// <param name="obj"></param>
    void OnChangerAccountClick(GameObject obj)
    {
        UIManager.Instance.ClosePanel<UIAgreementPanel>();
        UIManager.Instance.ClosePanel<UIOfficialNoticePanel>();
        UIManager.Instance.ClosePanel<UIServerListPanel>();
        if (Platform.IsEditor)
        {
            CSHotNetWork.Instance.OnReturn();
        }
        else
        {
            QuDaoInterface.Instance.ChangeAccount();
        }
    }

    protected override void OnDestroy()
    {
        if (mEnterFailTipsCoroutine != null)
        {
            CoroutineManager.StopCoroutine(mEnterFailTipsCoroutine);
        }
        if (mClientEvent != null) mClientEvent.UnRegAll();
        mClientEvent = null;
        mServerIp = null;
        btn_enter = null;
        HotMain.UpdateCallBack -= Update;
    }

    void OnIOSCorrectSucc(LoginInfo loginInfo)
    {
        if (loginInfo.isError)
        {
            UIManager.Instance.ClosePanel<UIWaiting>();
            return;
        }

#if EDITORMODE
        
#else
        CSConstant.loginName = loginInfo.userId;
#endif
        long time = 0;
        if (long.TryParse(loginInfo.time, out time))
            Constant.time = time;

        Constant.sign = loginInfo.token;

        PlayerPrefs.SetString("userName", CSConstant.loginName);

        CSNetwork.Instance.Connect(host, gamePort);
        if (mEnterFailTipsCoroutine != null)
        {
            CoroutineManager.StopCoroutine(mEnterFailTipsCoroutine);
        }

        mEnterFailTipsCoroutine = CoroutineManager.DoCoroutine(EnterFailTips(CSString.Format(414)));
    }


}
