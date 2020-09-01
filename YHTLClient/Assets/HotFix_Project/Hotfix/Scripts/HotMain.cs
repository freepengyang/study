using System;
using System.Collections;
using AssetBundles;
using Main_Project.Script.Update;
using UnityEngine;

public class HotMain
{
    public static void Init()
    {
        HotManager.Instance.Init();
    }

    //Update回调，需要时绑定事件
    public static event System.Action UpdateCallBack;

    public static void Update(object obj)
    {
        Timer.Instance.Update();
        ItemCDManager.Instance.Update();
        CSMapManager.Instance.Update();
        ActionManager.Instance.Update();
        if (UpdateCallBack != null)
            UpdateCallBack();
    }

    public static void LoginStateInit()
    {
        UIManager.Instance.CreatePanel<UILoginRolePanel>();
    }

    public static void LoginInit()
    {
        UIManager.Instance.CreatePanel<UILogin>();
    }

    public static void CreateDownLoad(int type)
    {
        UIManager.Instance.CreatePanel<UIDownloading>((f) =>
        {
            (f as UIDownloading).RefreshData((DownloadUIType) type);
        });
    }

    public static void AddLog(ELogToggleType type, string log, ELogColorType colorType = ELogColorType.White)
    {
        UIDebugInfo.AddLog(type, log, colorType);
    }

    //CSGame 反射调用
    public static void OnDestroy()
    {
        if (CSResUpdateManager.Instance != null)
        {
            CSResUpdateManager.Instance.CloseDownloadThread();
        }
        HotFix_Invoke.Instance.mRefreshTime = null;
        HotFix_InvokeThird.UIPlaySoundInCSAudioAction = null;
        HotFix_Invoke.Instance.mOnLogoutAccount = null;
        HotFix_Invoke.Instance.mOnLogoutAccount = null;
        HotFix_Invoke.Instance.mReqPay = null;
        HotFix_Invoke.Instance.mACTION_BATTERY_CHANGED = null;
        HotFix_Invoke.Instance.mOnQuitGameTips = null;
        HotFix_InvokeThird.UIPlayChatVoiceAction = null;
        HotFix_InvokeThird.UIStopChatVoiceAction = null;
    }
}

public class HotManager : Singleton<HotManager>
{
    public EventHanlderManager EventHandler;
    public EventHanlderManager SocketHandler;

    public MainEventHanlderManager MainEventHandler;    

    public HotManager()
    {
        EventHandler = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
        SocketHandler = new EventHanlderManager(EventHanlderManager.DispatchType.Socket);

        MainEventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
    }


    public void Init()
    {
        CSHandleMainEventManager.Instance.Init();
        InitSDK();
        Initialize();
        InitMethod();
        ILSDKCallback.Init(); //初始化android回调
        CSGame.Sington.StartCoroutine(StartInit());
        InitBugly();
    }

    IEnumerator StartInit()
    {
        yield return null;
        NetMsgParser.InitNetMsg(); //初始化协议
    }
    
    public void InitMethod()
    {
        HotFix_Invoke.Instance.mRefreshTime = CSServerTime.Instance.refreshTime;
        HotFix_InvokeThird.UIPlaySoundInCSAudioAction = CSAudioManager.Instance.PlayUIInThird;
        HotFix_InvokeThird.UIPlayChatVoiceAction = VoiceChatManager.Instance.PlayChatVoice;
        HotFix_Invoke.Instance.mOnLogoutAccount = CSHotNetWork.Instance.LogoutGameAccout;
        HotFix_InvokeThird.UIStopChatVoiceAction = VoiceChatManager.Instance.StopChatVoice;
    }

    private void InitSDK()
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
            case PlatformType.ANDROID:
                AndroidSDKCallback.InitCallback(); //Android回调初始化
                break;
            case PlatformType.IOS:
                IOSQuDaoCallback.InitCallback();
                break;
        }
    }


    public void Initialize()
    {
        UILayerMgr.Instance.LayerInit();
        if (CSGame.Sington.isLoadLocalRes)
        {
            ChangeState("LoginScene");
        }
        else
        {
            ChangeState("LoginScene", true, true);
        }
    }

    public void ChangeState(string sceneName, bool isFirstTo = false, bool isSelectChactorToMainScene = false,
        bool isToRoleListPanel = false)
    {
        CSGame.Sington.IsFirstTo = isFirstTo;

        ChangeGameState(sceneName);

        if (sceneName == "LoginScene" && !isFirstTo)
        {
            CSGame.Sington.IsToRoleListPanel = isToRoleListPanel;
            CSSceneLoadManager.Instance.loadScene(sceneName, isToRoleListPanel);
        }
        else
        {
            CSSceneLoadManager.Instance.LoadScenePassEmptyScene(sceneName, isFirstTo, isSelectChactorToMainScene);
        }
    }

    public void ChangeStateBackFromGame(bool isToRoleListPanel = false)
    {
        ChangeGameState("LoginScene");

        CSGame.Sington.IsToRoleListPanel = isToRoleListPanel;
        CSSceneLoadManager.Instance.loadSceneBackFromGame("LoginScene", isToRoleListPanel);
    }

    private GameState ChangeGameState(string name)
    {
        switch (name)
        {
            case "LoginScene":
                CSGame.Sington.mCurState = GameState.LoginScene;
                break;
            case "FirstScene":
                CSGame.Sington.mCurState = GameState.FirstScene;
                break;
            case "MainScene":
                CSGame.Sington.mCurState = GameState.MainScene;
                CSResUpdateManager.Instance.StartBackDownload();
                break;
            case "EmptyState":
                CSGame.Sington.mCurState = GameState.EmptyState;
                break;
        }

        return CSGame.Sington.mCurState;
    }

    public void UnRegEventHandle()
    {
        EventHandler?.UnRegAll();
        SocketHandler?.UnRegAll();
        MainEventHandler?.UnRegAll();
    }
    
    
    private void InitBugly()
    {
        try
        {
            BuglyAgent.ConfigDefault(QuDaoInterface.Instance.GetChannelId().ToString(), QuDaoInterface.Instance.GetVersion(), "",0);

        
#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId ("bffb925501");
#elif UNITY_ANDROID
            BuglyAgent.InitWithAppId ("bffb925501");
#endif
        
            BuglyAgent.EnableExceptionHandler();
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}