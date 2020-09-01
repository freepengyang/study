using System;
using System.IO;
using UnityEngine;

public class ResourceLoadManager
{
    private static ResourceLoadManager m_Instance = null;

    public static ResourceLoadManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ResourceLoadManager();
            }

            return m_Instance;
        }
    }

    private AssetBundle _assetBundle;

    public readonly string endName = ".bytes";
    private string serverType;
    private readonly string abName = "uiclientloadpanel.u3d";
    private byte abFileOf;

    private void Init()
    {
        GetDefaultSetting();
        if (PlayerPrefs.HasKey("loginServerType"))
            serverType = PlayerPrefs.GetString("loginServerType");

        UnityEngine.Debug.Log($"启动版本：  serverType: {serverType}");
        string dllURL = GetURL(abName);
        if (!File.Exists(dllURL))
        {
            UnityEngine.Debug.Log("启动版本不存在，读取默认版本");

            dllURL = GetDefaultURL(abName);
        }

        byte[] bytes = File.ReadAllBytes(dllURL);
        bytes[0] -= abFileOf;
        _assetBundle = AssetBundle.LoadFromMemory(bytes);
        //_assetBundle = AssetBundle.LoadFromFile(dllURL);
    }

    private void GetDefaultSetting()
    {
#if UNITY_EDITOR
        serverType = "0";
        abFileOf = 28;

#elif UNITY_ANDROID
        AndroidJavaObject jo;

        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
    
        try
        {
            serverType = jo.Call<string>("getDefaultLoginServerType");
            abFileOf = jo.Call<byte>("getAssetBundleFileOf");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
#endif
    }

    public byte[] GetResource(string name)
    {
        if (_assetBundle == null) Init();
        name += endName;
        TextAsset textAsset = _assetBundle.LoadAsset<TextAsset>(name);
        if (textAsset != null)
        {
            return textAsset.bytes;
        }

        return null;
    }

    public void UnLoad()
    {
        if (_assetBundle != null) _assetBundle.Unload(true);
    }

    public string GetURL(string dllName)
    {
        return Path.Combine(mILRunTimeResURL, dllName);
    }

    public string GetDefaultURL(string dllName)
    {
        return Path.Combine(mILRunTimeDefaultResURL, dllName);
    }

    public void Destroy()
    {
        UnLoad();
        m_Instance = null;
    }

    private string mILRunTimeResURL
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + "/../Library/ScriptAssemblies/";
#elif UNITY_ANDROID
             return $"{Application.persistentDataPath}/{serverType}/Android/ui/";
#elif UNITY_IPHONE || UNITY_IOS
            return $"{Application.persistentDataPath}/{serverType}/iOS/ui/";
#endif
        }
    }

    private string mILRunTimeDefaultResURL
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + "/../Library/ScriptAssemblies/";
#elif UNITY_ANDROID
             return $"{Application.persistentDataPath}/0/Android/ui/";
#elif UNITY_IPHONE || UNITY_IOS
            return $"{Application.persistentDataPath}/0/iOS/ui/";
#endif
        }
    }
}