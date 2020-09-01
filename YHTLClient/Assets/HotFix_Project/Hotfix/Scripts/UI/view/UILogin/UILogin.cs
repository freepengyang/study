using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;
using System;
using System.Text.RegularExpressions;
using Main_Project.Script.Update;
using Main_Project.SDKScript.SDK;
using UnityEngine.Networking;

public class UILogin : UIBase
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    private GameObject _serverViewCtrl;

    private GameObject serverViewCtrl
    {
        get { return _serverViewCtrl ?? (_serverViewCtrl = Get("serverview").gameObject); }
    }

    private GameObject _serverview;

    private GameObject mServerview
    {
        get { return _serverview ?? (_serverview = Get<GameObject>("serverview")); }
    }

    private GameObject _loginview;

    private GameObject mLoginView
    {
        get { return _loginview ?? (_loginview = Get<GameObject>("loginview")); }
    }

    private UILabel _banhao;

    private UILabel banhao
    {
        get { return _banhao ?? (_banhao = Get<UILabel>("loginview/Label")); }
    }

    private UILabel _lb_version;

    private UILabel lb_version
    {
        get { return _lb_version ?? (_lb_version = Get<UILabel>("bottom_right/lb_version")); } 
    }

    private UIInput _input;

    private UIInput input
    {
        get { return _input ?? (_input = Get<UIInput>("loginview/center/view/input_name")); }
    }

    private Transform _btLogin;

    private Transform btLogin
    {
        get { return _btLogin ?? (_btLogin = Get("loginview/center/view/btn_login")); }
    }

    private UITexture _mLogo;

    private UITexture mLogo
    {
        get { return _mLogo ?? (_mLogo = Get<UITexture>("center/logo")); }
    }

    private UILabel _plateNumber;

    private UILabel plateNumber
    {
        get { return _plateNumber ?? (_plateNumber = Get<UILabel>("loginview/bottom/plateNumber")); }
    }

    UISpriteAnimation _loginDoor;

    UISpriteAnimation loginDoor
    {
        get { return _loginDoor ?? (_loginDoor = Get<UISpriteAnimation>("center/window/logindoor")); }
    }

    GameObject _loginLight;

    GameObject loginLight
    {
        get { return _loginLight ?? (_loginLight = Get<GameObject>("center/window/effect/loginlight")); }
    }

    GameObject _loginLight2;

    GameObject loginLight2
    {
        get { return _loginLight2 ?? (_loginLight2 = Get<GameObject>("center/window/effect/loginlight2")); }
    }

    ServerViewCtrl ctrl;
    
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.UI_Login, OnLoginOrBack);
        mClientEvent.AddEvent(CEvent.UI_LoginBack, OnLoginOrBack);
        mClientEvent.AddEvent(CEvent.ResLoginMessage, OnReceivedLoginData);


        ctrl = new ServerViewCtrl();
        ctrl.UIPrefab = serverViewCtrl;

        UIManager.Instance.ClosePanel<UIWaiting>();
        if (mLogo != null) mLogo.MakePixelPerfect();
        UIEventListener.Get(btLogin.gameObject).onClick = OnButtonClick;

        if (!CSGame.Sington.isLoadLocalRes)
        {
            if (lb_version != null && CSVersionManager.Instance != null)
                lb_version.text = "V" + CSVersionManager.Instance.Version;
        }

        ShowOrCloseInput();

        CSGameManager.Instance.SaveStatiIcon();
        CSFontManager.Instance.SaveStaticFont();
        QuDaoConstantHot.LinkCheckState = false;

        ReadLogo();

        if (CSAudioMgr.Instance != null) CSAudioMgr.Instance.PlayLoginBGM();
        OpenUnityLog();
    }


    private void ShowOrCloseInput()
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                input.gameObject.SetActive(true);
                if (input != null) input.value = PlayerPrefs.GetString("userName");
                break;
            case PlatformType.ANDROID:
            case PlatformType.IOS:
                input.gameObject.SetActive(false);
                if (QuDaoConstant.GetPlatformData() != null)
                {
                    if (QuDaoConstant.isEditorMode())
                    {
                        input.gameObject.SetActive(true);
                        if (input != null) input.value = PlayerPrefs.GetString("userName");
                    }
                }

                break;
        }
    }

    public void SetLoginCallBak()
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                break;
            case PlatformType.ANDROID:
                SetAndroidLoginListener();
                break;
            case PlatformType.IOS:
                HandleIosLogin();
                break;
        }
    }

    private void ReadLogo()
    {
        if(!Platform.IsEditor)
        {
            mLogo.gameObject.SetActive(true);
            CoroutineManager.DoCoroutine(LoadLog());
            CoroutineManager.DoCoroutine(ReadPlateNumbet());
        }
    }

    private IEnumerator ReadPlateNumbet()
    {
        if (plateNumber != null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(AppUrl.PlateNumbetUrl);
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                plateNumber.text = www.downloadHandler.text;
            }else
            {
                FNDebug.LogErrorFormat("UILogin ReadPlateNumbet error  {0}  code: {1}    url:{2}", www.error,
                    www.responseCode, www.uri);
            }

            www.Dispose();
        }
    }

    private IEnumerator LoadLog()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(AppUrl.GameLogoUrl);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Texture tex = DownloadHandlerTexture.GetContent(www);
            if (tex != null)
            {
                mLogo.mainTexture = tex;
                mLogo.MakePixelPerfect();
            }
        }

        www.Dispose();
    }

    public override void Show()
    {
        base.Show();
        InitLoginState();

        SetLoginCallBak();
        if (QuDaoConstant.GetPlatformData() != null)
        {
            QuDaoInterface.Instance.Login("auto");
        }
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(loginLight2);
        CSEffectPlayMgr.Instance.Recycle(loginLight);
        base.OnDestroy();
        _serverview = null;
        _loginview = null;

        ctrl.Destroy();
        ctrl = null;
    }

    private void OnLoginOrBack(uint id, object data)
    {
        mLoginView.SetActive(false);
        mServerview.SetActive(true);
        if (ctrl != null) ctrl.Start();
        
        UIManager.Instance.ClosePanel<UIWaiting>();
    }

    void InitLoginState()
    {
        mLoginView.SetActive(true);
        mServerview.SetActive(false);
    }

    private void OnReceivedLoginData(uint id, object data)
    {
        if (data == null) return;

        //Android端判定判断是否绕过了二次检测
        if (Platform.IsAndroid && !QuDaoConstantHot.LinkCheck())
            return;

        user.LoginResponse s_login = Network.Deserialize<user.LoginResponse>(data);

        UserManager.Instance.UpdateRoleList(s_login);
        UserManager.mUserId = s_login.userId;
        ShowOpenDoorTween();
    }

    public void OnButtonClick(GameObject gp)
    {
        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_6);
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                EditorLogin();
                break;
            case PlatformType.ANDROID:
                if (QuDaoConstant.isEditorMode())
                {
                    EditorLogin();
                }
                else
                {
                    SetAndroidLoginListener(); //避免在第一次登入成功后,情况了登入回调,但是没有获取到服务器列表导致不能进去,每次进行登入前,重新设置登入回调
                    QuDaoInterface.Instance.Login();
                }

                break;
            case PlatformType.IOS:
                if (QuDaoConstant.isEditorMode())
                {
                    EditorLogin();
                }
                else
                {
                    QuDaoInterface.Instance.Login();
                }

                break;
        }
    }

    public void EditorLogin()
    {
        if (string.IsNullOrEmpty(input.value)) return;
        string[] data = input.value.Split('#');
        if (data.Length == 2)
        {
            CSConstant.loginName = data[0];
            Constant.loginServerId = data[1];
        }
        else
            CSConstant.loginName = input.value;

        PlayerPrefs.SetString("userName", input.value);
        PlayerPrefs.SetString("loginServerType", CSConstant.ServerType);
        UIManager.Instance.OpenPanel<UIWaiting>().Show(false);

        RequestData();
    }

    private void LoginMultiSuccess(LoginInfo data)
    {
        if (data == null) return;
        QuDaoInterface.Instance.OnMultiLoginSuc = null;
        CSConstant.loginName = data.userId.ToString();
        Constant.mToken = data.token;
        Constant.loginExt = data.ext;
        Constant.gameID = data.gid;
        Constant.loginType = data.loginType;
        if (!string.IsNullOrEmpty(data.sign))
            Constant.loginSign = data.sign;
        PlayerPrefs.SetString("userName", data.userId.ToString());

        QuDaoLoginSuc();
    }

    private void LoginSuccess(LoginResult data)
    {
        if (data == null) return;
        QuDaoInterface.Instance.OnLoginSuc = null;
        Constant.loginSign = data.openUid; //走这个流程里面的mOpenUid为token
        CSConstant.loginName = ""; //走这个流程的sdk,loginname在验证服务器的时候获取
        QuDaoLoginSuc();
    }

    void QuDaoLoginSuc()
    {
        UIManager.Instance.OpenPanel<UIWaiting>().Show(false);
        RequestData();
    }

    private void RequestData()
    {
        Constant.isAccountException = false;
        HttpRequest.Instance.StartGetServerList();
        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_7);
        CSSubmitDataManager.Instance.SendLoginData();
    }

    private void HandleIosLogin()
    {
        try
        {
            QuDaoInterface.Instance.OnIOSSDKLoginSuc = OnIOSSDKLoginSuc;
        }
        catch (Exception e)
        {
            FNDebug.LogError(e.Message);
        }
    }


    void SetAndroidLoginListener()
    {
        try
        {
            if (QuDaoConstant.GetPlatformData() != null)
            {
                if (QuDaoConstant.GetPlatformData().loginCode == LoginCode.MultiParameter)
                {
                    QuDaoInterface.Instance.OnMultiLoginSuc = LoginMultiSuccess;
                }

                if (QuDaoConstant.GetPlatformData().loginCode == LoginCode.OneParameter)
                {
                    QuDaoInterface.Instance.OnLoginSuc = LoginSuccess;
                }
            }
            else
            {
                QuDaoInterface.Instance.OnLoginSuc = LoginSuccess; //编辑器模式/kaiyingsdk回调(默认kaiying回调)
                QuDaoInterface.Instance.OnMultiLoginSuc = LoginMultiSuccess;
            }
        }
        catch (Exception e)
        {
            FNDebug.LogError(e.Message);
        }
    }

    void OnIOSSDKLoginSuc(LoginInfo _loginInfo)
    {
        CSConstant.loginName = _loginInfo.userId;
        Constant.loginExt = _loginInfo.ext;
        Constant.isBindPhone = _loginInfo.isBindPhone;
        CSConstant.isWhitePaper = _loginInfo.isWhitePaper;
        UIManager.Instance.OpenPanel<UIWaiting>().Show(false);
        RequestData();
    }


    void ShowOpenDoorTween()
    {
        UIManager.Instance.ClosePanel<UIWaiting>();
        mServerview.gameObject.SetActive(false);
        loginDoor.enabled = true;
        loginDoor.Play();
        loginLight.SetActive(true);
        mLogo.gameObject.SetActive(false);
        ScriptBinder.Invoke(0.5f, ChangeDoorAngle);
    }

    void ChangeDoorAngle()
    {
        CSEffectPlayMgr.Instance.ShowUIEffect(loginLight2, "loginlight2", ResourceType.UIEffect,15,true,true,null);
        ScriptBinder.Invoke2(0.4f, Enter);
    }

    void Enter()
    {
        UIManager.Instance.ClosePanel<UIServerListPanel>();
        UIManager.Instance.ClosePanel<UILogin>();

        UIManager.Instance.CreatePanel<UILoading>(f =>
        {
            UILoading panel = f as UILoading;
            if (panel != null)
            {
                panel.StartLoadTable();
            }
        });
    }

    public void OpenUnityLog()
    {
        if (Platform.mPlatformType != PlatformType.EDITOR && QuDaoConstant.isEditorMode())
        {
            //ReporterManager.instance.ShowLog();
        }
    }
}