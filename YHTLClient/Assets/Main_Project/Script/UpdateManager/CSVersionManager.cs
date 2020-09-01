/*using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiniJSON;
using UnityEngine.Networking;


public class CSVersionManager : CSGameMgrBase2<CSVersionManager>
{
    private ServerConfig _serverConfig;

    public ServerConfig ServerConfig
    {
        get { return _serverConfig; }
    }

    private ServerVersion _serverVersion;

    public ServerVersion ServerVersion
    {
        get { return _serverVersion; }
    }

    private ClientVersion _clientVersion;

    public ClientVersion ClientVersion
    {
        get { return _clientVersion; }
    }

    private Action<bool> onFinishCheck;

    public string Version
    {
        get
        {
            if (ClientVersion != null)
            {
                return ClientVersion.Version;
            }
            return string.Empty;
        }
    }

    public void CheckVersion(Action<bool> onFinish)
    {
        onFinishCheck = onFinish;
        string configStr = LoadServerConfig();
        if (!string.IsNullOrEmpty(configStr))
            OnLoadServerConfigComplete(configStr);
    }


    private string LoadServerConfig()
    {
        string configStr = "";
        try
        {
            switch (Platform.mPlatformType)
            {
                case PlatformType.EDITOR:
                    StartCoroutine(ALoadServerConfig());
                    break;
                case PlatformType.ANDROID:
                case PlatformType.IOS:
                    configStr = QuDaoInterface.Instance.GetServerConfigData();
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return configStr;
    }

    // 版本控制配置文件
    private IEnumerator ALoadServerConfig()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.mServerVersionURL);
        yield return www.SendWebRequest();

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("UnityWebRequest server config error " + www.error);
        }
        else
        {
            OnLoadServerConfigComplete(www.downloadHandler.text);
        }
    }

    private void OnLoadServerConfigComplete(string configStr)
    {
        if (string.IsNullOrEmpty(configStr))
        {
            Debug.LogError($"Load server config error {configStr}");
        }
        else
        {
            _serverConfig = new ServerConfig(configStr);
        }

        LoadClientVersion();
    }

    private void LoadClientVersion()
    {
        try
        {
            FileTool.CheckFile(AppUrlMain.mClientVersionPath);
            StartCoroutine(DownClientVersion());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private IEnumerator DownClientVersion()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.mClientVersionURL);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            _clientVersion = new ClientVersion(www.downloadHandler.data);
        }
        else
        {
            Debug.LogError($"Down Client Version Is Error :   {www.error}");
        }

        OnLoadConfigComplete();
    }

    private void OnLoadConfigComplete()
    {
        if (ServerConfig == null || ClientVersion == null) return;
        _serverVersion = ServerConfig.GetServerVersion(ClientVersion);
        InitServerVersionData(_serverVersion);

        CSCheckResourceManager.Instance.StartCheckUpdate(onFinishCheck);
    }


    private void InitServerVersionData(ServerVersion version)
    {
        if (version == null) return;
        AppUrlMain.mServerResURL = version.ResUrl;
        AppUrlMain.cdnVersion = version.CDNVersion;
        SFOut.URL_mServerResURL = AppUrlMain.mServerResURL;
        SFOut.CdnVersion = AppUrlMain.cdnVersion;
    }
}

public class ServerConfig
{
    private readonly Dictionary<string, ServerVersion> _serverVersions = new Dictionary<string, ServerVersion>();

    public Dictionary<string, ServerVersion> ServerVersions
    {
        get { return _serverVersions; }
    }


    private const string VERSIONLISTSTR = "VersionList";
    private const string DEFAULYNAME = "default";
    private const string OPENVOICE = "openVoice";
    private const string OPENRECHARGE = "openRecharge";
    private const string OPENTRANSFER = "openTranslate";

    public ServerConfig(string config)
    {
        ParseServerConfig(config);
    }

    private void ParseServerConfig(string config)
    {
        if (string.IsNullOrEmpty(config)) return;
        string str = EncryptionMain.DeEncrypVersionConfig(config);
        Dictionary<string, object> jsonDic = Json.Deserialize(str) as Dictionary<string, object>;
        if (jsonDic == null) return;

        QuDaoConstant.GetPlatformData().SetConfig(jsonDic);

        if (jsonDic.ContainsKey(VERSIONLISTSTR))
        {
            List<object> listTemp = jsonDic[VERSIONLISTSTR] as List<object>;
            for (var i = 0; i < listTemp.Count; i++)
            {
                ServerVersion sv = new ServerVersion(listTemp[i]);
                _serverVersions.Add(sv.Version, sv);
            }
        }

        if (jsonDic.ContainsKey(OPENVOICE))
        {
            QuDaoConstant.OpenVoice = jsonDic[OPENVOICE].ToString().Equals("1");
        }

        if (jsonDic.ContainsKey(OPENRECHARGE))
        {
            QuDaoConstant.OpenRecharge = jsonDic[OPENRECHARGE].ToString().Equals("1");
        }

        if (jsonDic.ContainsKey(OPENTRANSFER))
        {
            QuDaoConstant.OpenTranslate = jsonDic[OPENTRANSFER].ToString().Equals("1");
        }
    }

    public ServerVersion GetServerVersion(ClientVersion clientVersion)
    {
        if (clientVersion == null) return null;
        if (ServerVersions.ContainsKey(clientVersion.Version))
            return ServerVersions[clientVersion.Version];
        else
            return ServerVersions[DEFAULYNAME];
    }
}

public class ServerVersion
{
    private readonly string version;

    public string Version
    {
        get { return version; }
    }

    private readonly string resUrl;

    public string ResUrl
    {
        get { return resUrl; }
    }

    private readonly string cdnVersion;

    public string CDNVersion
    {
        get { return cdnVersion; }
    }

    private readonly System.Version updateVersion;

    public System.Version UpdateVersion
    {
        get { return updateVersion; }
    }

    private readonly string resVersion;

    public string ResVersion
    {
        get { return resVersion; }
    }
    
    private readonly string openCheckVersion;
    public bool mOpenCheckVersion { get { return string.IsNullOrEmpty(openCheckVersion) || openCheckVersion == "1"; } }

    private const string VERSION = "version";
    private const string RESURL = "resUrl";
    private const string CDNVERSION = "cdnVersion";
    private const string UPDATEVERSION = "updateVersion";
    private const string RESVERSION = "resVersion";
    private const string OPENCHECKVERSION = "openCheckVersion";

    public ServerVersion(object versionData)
    {
        if (versionData == null) return;
        Dictionary<string, object> dicData = versionData as Dictionary<string, object>;
        if (dicData == null) return;

        version = ParseJsonToString(dicData, VERSION);
        resUrl = ParseJsonToString(dicData, RESURL);
        cdnVersion = ParseJsonToString(dicData, CDNVERSION);
        string updateVerison = ParseJsonToString(dicData, UPDATEVERSION);
        resVersion = ParseJsonToString(dicData, RESVERSION);
        openCheckVersion = ParseJsonToString(dicData, OPENCHECKVERSION);
        updateVersion = new Version(updateVerison);
        AppUrlMain.updateVersion = updateVersion;
        QuDaoConstant.OpenCheckVersion = mOpenCheckVersion;
    }

    public string ParseJsonToString(Dictionary<string, object> dicData, string key)
    {
        if (dicData.TryGetValue(key, out object data))
        {
            return data as string;
        }

        return "";
    }
}

public class ClientVersion
{
    private string version;

    public string Version
    {
        get { return version; }
    }

    private string preDownMD5;

    public string PreDownMD5
    {
        get { return preDownMD5; }
    }

    private string gameDownMD5;

    public string GameDownMD5
    {
        get { return gameDownMD5; }
    }

    private const string PREDOWNMD5 = "preDownMD5";
    private const string GAMEDOWNMD5 = "gameDownMD5";

    public ClientVersion(byte[] bytes)
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                version = CSConstant.localVersion;
                break;
            case PlatformType.ANDROID:
                version = QuDaoInterface.Instance.GetVersion();
                break;
            case PlatformType.IOS:
                version = QuDaoInterface.Instance.GetVersion();
                break;
        }

        ParseLocalVersion(bytes);
    }

    public void SavePreClientVersion(string _preMd5)
    {
        preDownMD5 = _preMd5;
        SaveClientVersion();
    }

    public void SaveGameClientVersion(string _gameMd5)
    {
        if (gameDownMD5 == _gameMd5) return;
        gameDownMD5 = _gameMd5;
        SaveClientVersion();
    }

    private void ParseLocalVersion(byte[] data)
    {
        if (data == null || data.Length <= 0)
        {
            ResetAsDefault();
            return;
        }

        try
        {
            string strjson = Encoding.UTF8.GetString(data);
            Dictionary<string, object> strDic = Json.Deserialize(strjson) as Dictionary<string, object>;
            if (strDic == null) return;
            if (strDic.ContainsKey(PREDOWNMD5))
            {
                preDownMD5 = strDic[PREDOWNMD5] as string;
            }

            if (strDic.ContainsKey(GAMEDOWNMD5))
            {
                gameDownMD5 = strDic[GAMEDOWNMD5] as string;
            }

            if (!File.Exists(AppUrlMain.mClientGameDownResourceFile))
                gameDownMD5 = "";
            if (!File.Exists(AppUrlMain.mClientPreDownResourceFile))
                preDownMD5 = "";
        }
        catch (Exception e)
        {
            ResetAsDefault();
        }
    }

    private void ResetAsDefault()
    {
        preDownMD5 = "";
        gameDownMD5 = "";
        SaveClientVersion();
    }

    Dictionary<string, object> jsonDic = new Dictionary<string, object>();

    private void SaveClientVersion()
    {
        jsonDic.Clear();
        jsonDic.Add(PREDOWNMD5, preDownMD5);
        jsonDic.Add(GAMEDOWNMD5, gameDownMD5);

        string jsonStr = Json.Serialize(jsonDic);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
        try
        {
            FileTool.CreateFile(AppUrlMain.mClientVersionPath, bytes);
        }
        catch (Exception e)
        {
            if (Debug.developerConsoleVisible) Debug.Log("Save local version exception " + e.Message);
        }
    }
}*/