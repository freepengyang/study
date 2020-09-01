using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using MiniJSON;
using UnityEngine;
using UnityEngine.Networking;


namespace Main_Project.Script.Update
{
    public class CSVersionManager : Singleton2<CSVersionManager>
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
            private set
            {
                _serverVersion = value;
                InitServerVersionData(_serverVersion);
            }
        }

        private ClientVersion _clientVersion;

        public ClientVersion ClientVersion
        {
            get { return _clientVersion; }
        }

        private Action<bool> onInitFinishCheck;

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
        
        public void Init(Action<bool> onInitFinish)
        {
            onInitFinishCheck = onInitFinish;
            string configStr = LoadServerConfig();
            if (!string.IsNullOrEmpty(configStr))
                OnLoadServerConfigComplete(configStr);
        }

        public void CheckVersion(string serverVersion, Action<UpdateCheckCode> onCheckFinish)
        {
            OnLoadServerVersion(serverVersion);
            CSCheckResourceManager.Instance.StartCheckUpdate(serverVersion, onCheckFinish);
        }
        
        private string LoadServerConfig()
        {
            string configStr = "";
            try
            {
                switch (Platform.mPlatformType)
                {
                    case PlatformType.EDITOR:
                        CSGame.Sington.StartCoroutine(ALoadServerConfig());
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
                FNDebug.LogError("UnityWebRequest server config error " + www.error);
                if (onInitFinishCheck != null)
                    onInitFinishCheck(false);
            }
            else
            {
                OnLoadServerConfigComplete(www.downloadHandler.text);
            }
            www.Dispose();
        }
        
        private void OnLoadServerConfigComplete(string configStr)
        {
            if (string.IsNullOrEmpty(configStr))
            {
                FNDebug.LogError($"Load server config error {configStr}");
                if (onInitFinishCheck != null)
                    onInitFinishCheck(false);
            }
            else
            {
                _serverConfig = new ServerConfig(configStr);
            }

            OnLoadDefaultVersion();
        }

        private void OnLoadDefaultVersion()
        {
            OnLoadServerVersion(CSConstant.ServerType);
            CSCheckResourceManager.Instance.Init(CSConstant.ServerType, onInitFinishCheck);
        }

        private void OnLoadServerVersion(string serverVersion)
        {
            if (ServerConfig == null)
            {
                FNDebug.LogError($"Load server OnLoadDefaultServer error : ServerConfig == null");
                if (onInitFinishCheck != null)
                    onInitFinishCheck(false);
                return;
            }
            ServerVersion = ServerConfig.GetServerVersion(serverVersion);
            if(_clientVersion == null || _clientVersion.ServerVersion != serverVersion)
                _clientVersion = new ClientVersion(serverVersion);
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
        }

        public ServerVersion GetServerVersion(string serverVersion)
        {
            if (!string.IsNullOrEmpty(serverVersion) && ServerVersions.ContainsKey(serverVersion))
                return ServerVersions[serverVersion];
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

        public bool mOpenCheckVersion
        {
            get { return string.IsNullOrEmpty(openCheckVersion) || openCheckVersion == "1"; }
        }

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

        private string serverVersion;

        public string ServerVersion
        {
            get { return serverVersion; }
        }

        private string preKey;
        private string gameKey;

        private const string PREDOWNMD5 = "preDownMD5";
        private const string GAMEDOWNMD5 = "gameDownMD5";

        public ClientVersion(string _serverVersion)
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

            serverVersion = _serverVersion;
            preKey = $"{serverVersion}:{PREDOWNMD5}";
            gameKey = $"{serverVersion}:{GAMEDOWNMD5}";
            ParseLocalVersion();
        }

        public void SavePreClientVersion(string _preMd5)
        {
            preDownMD5 = _preMd5;
            PlayerPrefs.SetString(preKey, preDownMD5);
        }

        public void SaveGameClientVersion(string _gameMd5)
        {
            gameDownMD5 = _gameMd5;
            PlayerPrefs.SetString(gameKey, gameDownMD5);
        }

        private void ParseLocalVersion()
        {
            if (PlayerPrefs.HasKey(preKey))
            {
                preDownMD5 = PlayerPrefs.GetString(preKey);
            }
            else
            {
                preDownMD5 = "";
                PlayerPrefs.SetString(preKey, preDownMD5);
            }

            if (PlayerPrefs.HasKey(gameKey))
            {
                gameDownMD5 = PlayerPrefs.GetString(gameKey);
            }
            else
            {
                gameDownMD5 = "";
                PlayerPrefs.SetString(gameKey, gameDownMD5);
            }

            if (!File.Exists(AppUrlMain.mClientDownResourceFile))
            {
                gameDownMD5 = "";
                preDownMD5 = "";
            }
        }
    }
}