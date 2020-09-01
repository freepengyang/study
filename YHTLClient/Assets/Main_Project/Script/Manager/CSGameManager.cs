using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Main_Project.Script.Update;

public class CSGameManager : MonoBehaviour
{
    public MainEventHanlderManager EventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
    
    private static CSGameManager mInstance;
    public static CSGameManager Instance
    {
        get { return mInstance; }
        set { mInstance = value; }
    }
    private GameObject mRoot;
    public UnityEngine.GameObject Root
    {
        get
        {
            if (mRoot == null)
            {
                mRoot = new GameObject("GameManager");
                DontDestroyOnLoad(mRoot);
            }
            return mRoot;
        }
        set { mRoot = value; }
    }

    public class StaticObj
    {
        public string name;
        public UnityEngine.Object obj;
    }

    public Dictionary<string, StaticObj> staticDic;
    public Dictionary<string, UIAtlas> atlasDic;
    

    public UnityEngine.Object GetStaticObj(string name)
    {
        if (staticDic.ContainsKey(name)) return staticDic[name].obj;
        return null;
    }

    public UIAtlas GetStaticAtlas(string name)
    {
         
        if (atlasDic.ContainsKey(name)) return atlasDic[name];
        return null;
    }

    public void SaveStatiIcon()
    {
        if (!staticDic.ContainsKey("ItemIcon"))
        {
            StaticObj obj = new StaticObj();
            obj.name = "ItemIcon";
            obj.obj = AssetBundleLoad.Prestrain();
            staticDic.Add("ItemIcon", obj);
        }
    }

    System.Action mDelayNextFrameFunc;
    public void DelayCallNextFrame(System.Action func)
    {
        mDelayNextFrameFunc = func;
        StartCoroutine(DelayNextFrameFunc());

    }

    private IEnumerator DelayNextFrameFunc()
    {
        yield return null;
        mDelayNextFrameFunc();
    }

    System.Action mFunc;
    public void GameInvoke(System.Action func, float delay)
    {
        mFunc = func;
        StartCoroutine(DelayFunc(delay));
    }

    private IEnumerator DelayFunc(float delay)
    {
        yield return new WaitForSeconds(delay);
        mFunc();
    }

    public void Awake()
    {
        Instance = this;
        InitHttpConnect();

        Init();

        staticDic = new Dictionary<string, StaticObj>();
        atlasDic = new Dictionary<string, UIAtlas>();
    }

    public void AddDicDataAtlas(string objName, UnityEngine.Object obj)
    {
        if (obj == null) return;
        GameObject go = obj as GameObject;
        if (go == null) return;
        UIAtlas atlas = go.GetComponent<UIAtlas>();
        if (atlas == null) return;
        atlas.InitShaderMiss();
        if (!atlasDic.ContainsKey(objName))
        {
            atlasDic.Add(objName, atlas);
        }
    }

    public void AddDicData(string resName, UnityEngine.Object obj)
    {
        if (!staticDic.ContainsKey(resName))
        {
            StaticObj data = new StaticObj();
            data.name = resName;
            staticDic.Add(resName, data);
        }
        staticDic[resName].obj = obj;
    }

    public void Init()
    {
        //FPS.CreateInstance(Root.transform);
        CSResUpdateManager.CreateInstance(Root.transform);
        SFOut.IResUpdateMgr = CSResUpdateManager.Instance;
        CSObjectPoolMgr.CreateInstance(Root.transform);
        //CSAvatarManager.CreateInstance(Root.transform);
        CSAudioMgr.CreateInstance(Root.transform);
        CSSceneLoadManager.CreateInstance(Root.transform);
        CSResourceManager.CreateInstance(Root.transform);
        //CSTouchEvent.CreateInstance(Root.transform);

        //UIItemBarManager.CreateInstance(Root.transform);
        //UIItemMoneyManager.CreateInstance(Root.transform);
        //UISabacItemManager.CreateInstance(Root.transform);
        EventHandler.SendEvent(MainEvent.InitCSGameManager);
        
        if (Platform.IsEditor)
        {
            CSDrawCellGizmos.CreateInstance(Root.transform);
        }
    }

    void InitFont()
    {
        Font font = GetStaticObj("msyh") as Font;
        if (font != null && font.material != null && font.material.mainTexture != null)
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
            font.material.mainTexture.anisoLevel = 1;
        }
    }
    
    void InitHttpConnect()
    {
        ServicePointManager.DefaultConnectionLimit = 512;
        ServicePointManager.Expect100Continue = false;
    }

    

    public void Clear()//切换场景调用
    {
        if(this == null)
        {
            FNDebug.LogError("======> CSGameManager this is null");
            return;
        }
        CSObjectPoolMgr.Instance.Destroy();
        EventHandler.SendEvent(MainEvent.DestroyCSGameManager);

        //if(CSAvatarManager.Instance != null) CSAvatarManager.Instance.Destroy();
        if(CSAudioMgr.Instance != null) CSAudioMgr.Instance.Destroy();
        if(CSSceneLoadManager.Instance != null) CSSceneLoadManager.Instance.Destroy();
        if(CSResourceManager.Instance != null) CSResourceManager.Instance.Destroy();
        //if(CSTouchEvent.Instance != null) CSTouchEvent.Instance.Destroy();
        //CSPoolManager.Dispose();
    }
}
