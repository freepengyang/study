
//-------------------------------------------------------------------------
//Game Sate
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Reflection;
using ILRuntime.CLR.TypeSystem;
using Object = UnityEngine.Object;
using FlyBirds.Model;

public class CSGame : MonoBehaviour
{
    [HideInInspector]
    public bool IsDebugLog = true;
    [HideInInspector]
    public bool isLoadLocalRes = true;
    /// <summary>是否是发布版本</summary>
    [HideInInspector]
    public bool IsRelease;
    [HideInInspector]
    public bool IsMiniApp;

    [HideInInspector]
    public GameState mCurState = GameState.EmptyState;
    [HideInInspector]
    public bool IsToRoleListPanel = false;
    [HideInInspector]
    public bool IsFirstTo = false;
    
    private static CSGame mSington;
    public static CSGame Sington
    {
        get
        {
            return mSington;
        }
    }
    
    public static MainEventHanlderManager MainEventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);

    void Awake()
    {
        mSington = this;
        
        if(CSStartLoadAssembly.Sington != null)
        {
            IsDebugLog = CSStartLoadAssembly.Sington.IsDebugLog;
            if(Platform.IsEditor)
            {
                isLoadLocalRes = CSStartLoadAssembly.Sington.IsLoadLocalRes;
            }else
            {
                isLoadLocalRes = false;
            }
            IsRelease = CSStartLoadAssembly.Sington.IsRelease;
            IsMiniApp = CSStartLoadAssembly.Sington.IsMiniApp;
        }
        
        SFOut.IsLoadLocalRes = isLoadLocalRes;
        if (PlayerPrefs.HasKey("loginServerType"))
            CSConstant.ServerType = PlayerPrefs.GetString("loginServerType");
        else
        {
            CSConstant.ServerType = QuDaoInterface.Instance.GetDefaultLoginServerType();
            PlayerPrefs.SetString("loginServerType", CSConstant.ServerType);
        }
    }
    void InitCSGame()
    {
#if UNITY_EDITOR
        CSMisc.InitEditorData(true, UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android ? "Android" : "iOS");
#elif UNITY_ANDROID || UNITY_IPHONE
        isLoadLocalRes = false;
#endif
        FNDebug.developerConsoleVisible = IsDebugLog;
        GameObject.DontDestroyOnLoad(this);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        CSConstant.PixelRatio = 1 * 2f / CSConstant.ContentSize.y;
        Application.targetFrameRate = 300;
        InitCBOut();
        OutLog.InitCallback();
        //CSDebug.InitCallback();
        ScreenAdapterTools screenAdapterTools = new ScreenAdapterTools();
        screenAdapterTools.Init();
    }

    void Start()
    {
        Init(null);
        InitCSGame();
        IsOpenUnityEngineLog();
    }

    void Update()
    {
        Network.Instance.Update();
        YvVoiceMgr.Instance.Update();
        CSTimer.Instance.Update();
        this.HotMainUpdate?.Run(null);
        
        if (Input.GetMouseButtonDown(1))
        {
            CSDirectPath.Clear();
        }
    }

    void InitCBOut()
    {
        SFOut.URL_mClientResPath = AppUrlMain.mClientResPath;
        SFOut.URL_mServerResURL = AppUrlMain.mServerResURL;
        SFOut.URL_mClientResURL = AppUrlMain.mClientResURL;
    }

    public void InitTable()
    {
        SkillDirectionTableManager.Instance.OnLoad("SkillDirection");
        SkillEffectTableManager.Instance.OnLoad("SkillEffect");
    }

    private void OnDestroy()
    {
        this.hotfixOnDestroy?.Run();
        this.hotfixOnDestroy = null;
        this.hotfixInitialize = null;
#if ILRuntime
        ILRuntime.Runtime.Generated.CLRBindings.Shutdown(appdomain);
#endif
        CSNetwork.Instance.Close();
        CSNetwork.Instance.CloseThread();
    }
    
    ~CSGame()
    {
#if ILRuntime
        if (DllStream != null)
            DllStream.Close();
        if (p != null)
            p.Close();
        DllStream = null;
        p = null;
#endif
    }

    public void ChangeStateBackFromGame(bool isToRoleListPanel = false)
    {
        IsToRoleListPanel = isToRoleListPanel;
    }

    public bool IsLoadLocalRes
    {
        get { return isLoadLocalRes; }
        set { isLoadLocalRes = value; }
    }

    //UnityEngine打印日志
    private void IsOpenUnityEngineLog()
    {
#if UNITY_EDITOR
        UnityEngine.Debug.unityLogger.logEnabled = true;
#else
        UnityEngine.Debug.unityLogger.logEnabled = IsDebugLog;
#endif
    }

#region ILRuntime

    public static CSGame Ins;

    System.Action CompleteCall;

    public string DllRoot_Steam
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

#if ILRuntime
    //AppDomain是ILRuntime的入口，最好是在一个单例类中保存，整个游戏全局就一个，这里为了示例方便，每个例子里面都单独做了一个
    //大家在正式项目中请全局只创建一个AppDomain
    public static ILRuntime.Runtime.Enviorment.AppDomain appdomain;
    System.IO.MemoryStream DllStream;
    System.IO.MemoryStream p;
#else
   public System.Reflection.Assembly assembly;
#endif

    private IStaticMethod hotfixInitialize;
    private IStaticMethod hotfixOnDestroy;
    public IStaticMethod LoginStateInit
    {
        private set;get;
    }
    public IStaticMethod LoginInit
    {
        private set; get;
    }
    public IStaticMethod CreateDownLoad
    {
        private set; get;
    }
    public IStaticMethod ProcessNetwork
    {
        private set;get;
    }
    
    public IStaticMethod HotMainUpdate
    {
        private set;get;
    }
 
    public void Init(System.Action call)
    {
        CompleteCall = call;
        StartCoroutine(LoadHotFixAssembly());
    }

    IEnumerator LoadHotFixAssembly()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //这个DLL文件是直接编译HotFix_Stone.sln生成的，已经在项目中设置好输出目录为StreamingAssets，在VS里直接编译即可生成到对应目录，无需手动拷贝
        byte[] res;
        UnityWebRequest www;
        if (Platform.IsEditor)
        {
            string dllURL = ResourceLoadManager.instance.GetURL("UIHotResPanel.dll");
            FNDebug.Log("Load ILRuntime DLL Path : " + dllURL);
            res = File.ReadAllBytes(dllURL);
        }
        else
        {
            res = ResourceLoadManager.instance.GetResource("UIHotResPanel");
            if (res == null)
            {
                UnityEngine.Debug.LogError($"TextAsset  error  CSGame:  "); 
                yield break;
            }
        }

#if ILRuntime
        //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        //正常项目中应该是自行从其他地方下载dll，或者打包在AssetBundle中读取，平时开发以及为了演示方便直接从StreammingAssets中读取，
        //正式发布的时候需要大家自行从其他地方读取dll
        
        if (!IsRelease)
        {
            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
            byte[] pdb = null;
            string pdbURL = ResourceLoadManager.instance.GetURL("UIHotResPanel.pdb");
            if (Platform.IsEditor)
            {
                pdb = File.ReadAllBytes(pdbURL);
            }
            else
            {

                www = UnityWebRequest.Get(pdbURL);

                yield return www.SendWebRequest();


                if (www.isHttpError || www.isNetworkError)
                {
                    if (!string.IsNullOrEmpty(www.error))
                        UnityEngine.Debug.LogError(www.error);
                }
                else
                {
                    pdb = www.downloadHandler.data;
                }

                www.Dispose();
            }
            DllStream = new MemoryStream(res);

            if(pdb != null)
            {
                p = new MemoryStream(pdb);
                appdomain.LoadAssembly(DllStream, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }else
            {
                appdomain.LoadAssembly(DllStream);
            }
        }
        else
        {
            DllStream = new MemoryStream(res);
            appdomain.LoadAssembly(DllStream);
        }

        ILRuntimeRegister.Instance.InitializeILRuntime();

        ILRuntimeRegister.Instance.SetupCLRRedirection();

        this.hotfixInitialize = new ILStaticMethod(appdomain, "HotMain", "Init", 0);
        this.hotfixOnDestroy = new ILStaticMethod(appdomain, "HotMain", "OnDestroy", 0);
        this.LoginStateInit = new ILStaticMethod(appdomain, "HotMain", "LoginStateInit", 0);
        this.LoginInit = new ILStaticMethod(appdomain, "HotMain", "LoginInit", 0);
        this.CreateDownLoad = new ILStaticMethod(appdomain, "HotMain", "CreateDownLoad", 1);
        this.ProcessNetwork = new ILStaticMethod(appdomain, "NetMsgParser", "ProcessNetwork", 1);
        this.HotMainUpdate = new ILStaticMethod(appdomain, "HotMain", "Update", 1);

#else
        FNDebug.Log($"当前使用的是Mono模式");
        if (!IsRelease)
        {
            byte[] pdb = null;
            string pdbURL = ResourceLoadManager.instance.GetURL("UIHotResPanel.pdb");
            if (Platform.IsEditor)
            {
                pdb = File.ReadAllBytes(pdbURL);
            }
            this.assembly = Assembly.Load(res, pdb);
        }
        else
        {
           this.assembly = Assembly.Load(res);
        }
        Type hotfixMainType = this.assembly.GetType("HotMain");
        this.hotfixInitialize = new MonoStaticMethod(hotfixMainType, "Init");
        this.hotfixOnDestroy = new MonoStaticMethod(hotfixMainType, "OnDestroy");
        this.HotMainUpdate = new MonoStaticMethod(hotfixMainType, "Update");
        this.LoginStateInit = new MonoStaticMethod(hotfixMainType, "LoginStateInit");
        this.LoginInit = new MonoStaticMethod(hotfixMainType, "LoginInit");
        this.CreateDownLoad = new MonoStaticMethod(hotfixMainType, "CreateDownLoad");
        Type hotfixNetMsgType = this.assembly.GetType("NetMsgParser");
        this.ProcessNetwork = new MonoStaticMethod(hotfixNetMsgType, "ProcessNetwork");
#endif
        
        GameObject gameStateGo = GameObject.Find("GameState");
        if (gameStateGo != null)
        {
            gameStateGo.AddComponent<CSGameManager>();
        }
        OnHotFixLoaded();
        
        ResourceLoadManager.instance.Destroy();
        yield return null;
        System.GC.Collect();

    }
    
    MonoBehaviourAdapter.Adaptor GetComponent(ILType type)
    {
        var arr = GetComponents<MonoBehaviourAdapter.Adaptor>();
        for (int i = 0; i < arr.Length; i++)
        {
            var instance = arr[i];
            if (instance.ILInstance != null && instance.ILInstance.Type == type)
            {
                return instance;
            }
        }
        return null;
    }
    void OnHotFixLoaded()
    {
        if (CompleteCall != null)
        {
            CompleteCall();
            CompleteCall = null;
        }

        this.hotfixInitialize?.Run();
    }

#endregion
}