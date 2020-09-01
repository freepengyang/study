using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using TABLE;
using UnityEngine.Networking;

namespace Main_Project.Script.Update
{
    public enum UpdateCheckCode
    {
        Normal,//正常
        Update,//更新
        ServerListError,//服务器配置文件下载错误
    }
    
    public class CSCheckResourceManager : Singleton2<CSCheckResourceManager>
    {
        public static readonly string RESOURCELISTNAME = "ResourceList.bytes";
        public static readonly string DEFAULTSERVERNAME = "0";
        
        private Dictionary<string, RESLISTANDROID> ClientResourceListDic = new Dictionary<string, RESLISTANDROID>(2048);

        private Dictionary<string, RESLISTANDROID> ClientDownAppendListDic =
            new Dictionary<string, RESLISTANDROID>(20);

        private RESLISTANDROIDARRAY reslistarray = new RESLISTANDROIDARRAY();

        private byte[] resBytes;

        private bool isLoadClientDown;
        private bool isLoadServerResList;
        private bool isCompare;
        private UpdateCheckCode checkCode;

        public string ServerResourceVersion { get; set; }

        private string lastServerVersion = "-1";

        private CSGame mGame;

        private Action<UpdateCheckCode> onFinishCheck;
        private Action<bool> onInitFinish;
        private event Action ClientLoadCoplete;
        
        public void Init(string serverVersion, Action<bool> initFinish)
        {
            Init();
            onInitFinish = initFinish;
            lastServerVersion = serverVersion;
            ClientLoadCoplete += FinishInit;
            LoadClientData();
        }
        
        public void StartCheckUpdate(string serverVersion, Action<UpdateCheckCode> checkFinish)
        {
            onFinishCheck = checkFinish;
            if(lastServerVersion == serverVersion)
            {
                if(isCompare && checkCode != UpdateCheckCode.ServerListError)
                {
                    FinishCheckVersion();
                    return;
                }
                Init();
                isLoadClientDown = true;
            }else
            {
                Init();
                ClientLoadCoplete += FinishLoad;
                LoadClientData();
            }
            lastServerVersion = serverVersion;
            
            CheckUpdate();
        }
        
        private void FinishLoad()
        {
            CompareVersion();
            ClientLoadCoplete -= FinishLoad;
        }

        private void FinishInit()
        {
            if (onInitFinish != null)
                onInitFinish(true);
            ClientLoadCoplete -= FinishInit;
        }
        
        
        private void FinishCheckVersion()
        {
            isCompare = true;
            if (onFinishCheck != null)
            {
                onFinishCheck(checkCode);
                onFinishCheck = null;
            }

            if (!CSResUpdateManager.Instance.NeedUpdate)
            {
                CSVersionManager.Instance.ClientVersion.SavePreClientVersion(ServerResourceVersion);
            }else
            {
                //多次更新后，文件越来越大，清除旧数据
                SaveDownResListNewText();
            }
        }

        private void Init()
        {
            checkCode = UpdateCheckCode.Normal;
            mGame = CSGame.Sington;
            isLoadClientDown = false;
            isLoadServerResList = false;
            isCompare = false;
        }

        private void CheckUpdate()
        {
            ServerResourceVersion = CSVersionManager.Instance.ServerVersion.ResVersion;
            if (CompareResourceMd5())
            {
                mGame.StartCoroutine(LoadServerResourcelist());
            }
            else
            {
                isLoadServerResList = true;
                CompareVersion();
            }
        }

#region Load Client PreDownload
        
        private void LoadClientData()
        {
            if (!File.Exists(AppUrlMain.mClientDownResourceFile))
            {
                LoadResInApk();
                return;
            }
            mGame.StartCoroutine(LoadDownResource());
        }

        private void LoadResInApk()
        {
            switch (Platform.mPlatformType)
            {
                case PlatformType.EDITOR:
                    mGame.StartCoroutine(LoadAndroidResListInApk());
                    break;
                case PlatformType.IOS:
                    LoadIosResListInApk();
                    break;
                case PlatformType.ANDROID:
                    mGame.StartCoroutine(LoadAndroidResListInApk());
                    break;
            }
        }

        private void LoadIosResListInApk()
        {
            RESLISTANDROIDARRAY reslistinapk = null;
            try
            {
                reslistinapk = RESLISTANDROIDARRAY.Parser.ParseFrom(File.ReadAllBytes(AppUrlMain.ResInApkPath));
            }
            catch (Exception e)
            {
                FNDebug.LogError($"CSCheckResourceManager  LoadAndroidResListInApk error :  {e.Message}");
            }

            isLoadClientDown = true;
            if (reslistinapk != null) SetDownResource(reslistinapk.rows);
            LoadClientComplate();
        }

        private IEnumerator LoadAndroidResListInApk()
        {
            UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.ResInApkPath);
            yield return www.SendWebRequest();
            isLoadClientDown = true;
            if (string.IsNullOrEmpty(www.error))
            {
                RESLISTANDROIDARRAY reslistinapk = RESLISTANDROIDARRAY.Parser.ParseFrom(www.downloadHandler.data);
                if (reslistinapk != null) SetDownResource(reslistinapk.rows);
                LoadClientComplate();
            }
            else
            {
                FNDebug.LogError($"CSCheckResourceManager  LoadAndroidResListInApk error :  {www.error}");
            }
            www.Dispose();
        }

        private IEnumerator LoadDownResource()
        {
            UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.mClientDownResourceURL);
            yield return www.SendWebRequest();
            if (string.IsNullOrEmpty(www.error))
            {
                RESLISTANDROIDARRAY array = RESLISTANDROIDARRAY.Parser.ParseFrom(www.downloadHandler.data);
                isLoadClientDown = true;
                if (array != null) SetDownResource(array.rows);
            }
            else
            {
                FNDebug.LogError($"CSCheckResourceManager  LoadPreDownResource error :  {www.error}");
            }
            www.Dispose();
            LoadClientComplate();
        }
        
        private void LoadClientComplate()
        {
            if(ClientLoadCoplete != null)
            {
                ClientLoadCoplete();
            }
        }

        private void SetDownResource(IList<RESLISTANDROID> rows)
        {
            if (rows == null) return;
            RESLISTANDROID res;
            ClientResourceListDic.Clear();
            for (var i = 0; i < rows.Count; i++)
            {
                res = rows[i];
                if (res == null) continue;
                if(string.IsNullOrEmpty(res.server)) res.server = DEFAULTSERVERNAME;
                if (!ClientResourceListDic.ContainsKey(res.name))
                {
                    ClientResourceListDic.Add(res.name, res);
                }
                else
                    ClientResourceListDic[res.name] = res;
            }
        }

#endregion

    #region Load Server ResourceList

        private IEnumerator LoadServerResourcelist()
        {
            UnityWebRequest www =
                UnityWebRequest.Get($"{AppUrlMain.mServerResURL}{RESOURCELISTNAME}{AppUrlMain.cdnVersion}");
            yield return www.SendWebRequest();
            if (string.IsNullOrEmpty(www.error))
            {
                RESOURCELISTARRAY array = RESOURCELISTARRAY.Parser.ParseFrom(www.downloadHandler.data);
                if (array != null) CSResUpdateManager.Instance.AddFullResourceList(array.rows);
            }
            else
            {
                checkCode = UpdateCheckCode.ServerListError;
                FNDebug.LogError($"Load Server Resource List Error : url: {www.url}  error: {www.error}");
            }
            www.Dispose();
            isLoadServerResList = true;

            CompareVersion();
        }

        private bool CompareResourceMd5()
        {
            if (!ServerResourceVersion.Equals(CSVersionManager.Instance.ClientVersion.PreDownMD5) || 
                !ServerResourceVersion.Equals(CSVersionManager.Instance.ClientVersion.GameDownMD5))
            {
                return true;
            }
            return false;
        }

    #endregion

        private void CompareVersion()
        {
            if (!isLoadClientDown || !isLoadServerResList || isCompare) return;
            if (!File.Exists(AppUrlMain.mClientDownResourceFile))
            {
                SaveDownResListNewText();
            }

            if (ClientResourceListDic.Count > 0)
                CSResUpdateManager.Instance.CompareResUpdate(ClientResourceListDic);

            if(checkCode == UpdateCheckCode.Normal || checkCode == UpdateCheckCode.Update)
            {
                if(CSResUpdateManager.Instance.NeedUpdate)
                {
                    checkCode = UpdateCheckCode.Update;
                }else
                {
                    checkCode = UpdateCheckCode.Normal;
                }
            }
            FinishCheckVersion();
        }
        
        public string GetResourcePathType(string resName)
        {
            if(ClientResourceListDic.ContainsKey(resName))
            {
                return ClientResourceListDic[resName].server;
            }
            return CSConstant.ServerType;
        }
        
        
        private int index;

        public void UpdateClientDownRes(Resource res)
        {
            if (res == null) return;
            index++;
            if (ClientDownAppendListDic.ContainsKey(res.RelatePath))
            {
                ClientDownAppendListDic[res.RelatePath].server = CSConstant.ServerType;
                ClientDownAppendListDic[res.RelatePath].md5 = res.mMd5;
            }
            else
            {
                RESLISTANDROID resList = new RESLISTANDROID
                {
                    name = res.RelatePath,
                    md5 = res.mMd5,
                    server = CSConstant.ServerType
                };
                ClientDownAppendListDic.Add(res.RelatePath, resList);
            }
            
            if (index >= 20)
            {
                index = 0;
                SaveDownResListText();
            }
        }

        public void SaveDownResListText()
        {
            SaveResListText(ClientDownAppendListDic, AppUrlMain.mClientDownResourceFile);
            ClientDownAppendListDic.Clear();
        }

        public void SaveDownResListNewText()
        {
            SaveResListNewText(ClientResourceListDic, AppUrlMain.mClientDownResourceFile);
        }

        private void SaveResListNewText(Dictionary<string, RESLISTANDROID> resDic, string path)
        {
            if (resDic.Count == 0) return;
            var preDic = resDic.GetEnumerator();

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string dName = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dName)) return;

            if (!Directory.Exists(dName)) //判断目录是否存在
            {
                Directory.CreateDirectory(dName);
            }

            reslistarray.rows.Clear();
            while (preDic.MoveNext())
            {
                reslistarray.rows.Add(preDic.Current.Value);
            }

            resBytes = reslistarray.ToByteArray();

            if (resBytes != null && resBytes.Length > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(resBytes, 0, resBytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }


            preDic.Dispose();
        }

        private void SaveResListText(Dictionary<string, RESLISTANDROID> resDic, string path)
        {
            if (resDic.Count == 0) return;

            if (!File.Exists(path))
            {
                SaveResListNewText(resDic, path);
            }

            if(reslistarray == null)
            {
                reslistarray = new RESLISTANDROIDARRAY();
            }
            reslistarray.rows.Clear();
            
            var preDic = resDic.GetEnumerator();
            while (preDic.MoveNext())
            {
                reslistarray.rows.Add(preDic.Current.Value);
            }
            preDic.Dispose();

            resBytes = reslistarray.ToByteArray();

            if (resBytes != null && resBytes.Length > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.Append))
                {
                    stream.Write(resBytes, 0, resBytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }

        }
        
        public void SaveResListNewTextTest(Dictionary<string, RESOURCELIST> resDic, string path)
        {
            if (resDic.Count == 0) return;
            var preDic = resDic.GetEnumerator();

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string dName = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dName)) return;

            if (!Directory.Exists(dName)) //判断目录是否存在
            {
                Directory.CreateDirectory(dName);
            }

            RESOURCELISTARRAY reslistarray1 = new RESOURCELISTARRAY();
            reslistarray1.rows.Clear();
            while (preDic.MoveNext())
            {
                reslistarray1.rows.Add(preDic.Current.Value);
            }

            resBytes = reslistarray1.ToByteArray();

            if (resBytes != null && resBytes.Length > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(resBytes, 0, resBytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }


            preDic.Dispose();
        }
        
        public void OnDispose()
        {
            ClientResourceListDic = null;
            ClientDownAppendListDic = null;
            reslistarray = null;
        }
    }
}