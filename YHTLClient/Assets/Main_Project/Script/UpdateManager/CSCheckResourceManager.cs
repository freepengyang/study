/*using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using Main_Project.Script.Update;
using TABLE;
using UnityEngine.Networking;

public class CSCheckResourceManager : Singleton2<CSCheckResourceManager>
{
    public static readonly string RESOURCELISTNAME = "ResourceList.bytes";

    private readonly Dictionary<string, RESLISTANDROID> ClientPreDownListDic = new Dictionary<string, RESLISTANDROID>();
    private readonly Dictionary<string, RESLISTANDROID> ClientGameDownListDic = new Dictionary<string, RESLISTANDROID>();

    private readonly Dictionary<string, RESLISTANDROID> ClientPreDownAppendListDic =
        new Dictionary<string, RESLISTANDROID>();

    private readonly Dictionary<string, RESLISTANDROID> ClientGameDownAppendListDic =
        new Dictionary<string, RESLISTANDROID>();

    private readonly RESLISTANDROIDARRAY reslistarray = new RESLISTANDROIDARRAY();

    private byte[] resBytes;

    private bool isLoadClientPreDown;
    private bool isLoadClientGameDown;
    private bool isLoadServerResList;
    private bool isCompare;

    public string ServerResourceVersion { get; set; }

    private CSGame mGame;

    private Action<bool> onFinishCheck;

    public void StartCheckUpdate(Action<bool> checkFinish)
    {
        onFinishCheck = checkFinish;
        Init();
        CheckUpdate();
    }

    private void FinishCheckVersion()
    {
        isCompare = true;
        if (onFinishCheck != null)
        {
            onFinishCheck(CSResUpdateManager.Instance.NeedUpdate);
        }

        if (!CSResUpdateManager.Instance.NeedUpdate)
        {
            CSVersionManager.Instance.ClientVersion.SavePreClientVersion(ServerResourceVersion);
        }
    }

    private void Init()
    {
        mGame = CSGame.Sington;
        isLoadClientPreDown = false;
        isLoadClientGameDown = false;
        isLoadServerResList = false;
        isCompare = false;
        ClientPreDownListDic.Clear();
        ClientGameDownListDic.Clear();
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
        RESLISTINAPKARRAY reslistinapk = null;
        try
        {
            reslistinapk = RESLISTINAPKARRAY.Parser.ParseFrom(File.ReadAllBytes(AppUrlMain.ResInApkPath));
        }
        catch (Exception e)
        {
            Debug.LogError($"CSCheckResourceManager  LoadAndroidResListInApk error :  {e.Message}");
        }

        isLoadClientPreDown = true;
        if (reslistinapk != null) SetResListInApk(reslistinapk.rows);
        CompareVersion();
    }

    private IEnumerator LoadAndroidResListInApk()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.ResInApkPath);
        yield return www.SendWebRequest();
        isLoadClientPreDown = true;
        if (string.IsNullOrEmpty(www.error))
        {
            RESLISTINAPKARRAY reslistinapk = RESLISTINAPKARRAY.Parser.ParseFrom(www.downloadHandler.data);
            if (reslistinapk != null) SetResListInApk(reslistinapk.rows);
            CompareVersion();
        }
        else
        {
            Debug.LogError($"CSCheckResourceManager  LoadAndroidResListInApk error :  {www.error}");
        }
    }

    public void SetResListInApk(IList<RESLISTINAPK> rows)
    {
        if (rows == null) return;
        RESLISTINAPK res;
        for (var i = 0; i < rows.Count; i++)
        {
            res = rows[i];

            RESLISTANDROID reslistandroid = new RESLISTANDROID()
            {
                name = res.name,
                md5 = res.md5
            };
            if (res.resType.Equals("1"))
            {
                if (!ClientPreDownListDic.ContainsKey(res.name))
                {
                    ClientPreDownListDic.Add(res.name, reslistandroid);
                }
                else
                {
                    ClientPreDownListDic[res.name] = reslistandroid;
                }
            }
            else
            {
                if (!ClientGameDownListDic.ContainsKey(res.name))
                {
                    ClientGameDownListDic.Add(res.name, reslistandroid);
                }
                else
                {
                    ClientGameDownListDic[res.name] = reslistandroid;
                }
            }
        }
    }

    private IEnumerator LoadPreDownResource()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.mClientPreDownResourceURL);
        yield return www.SendWebRequest();
        if (string.IsNullOrEmpty(www.error))
        {
            RESLISTANDROIDARRAY array = RESLISTANDROIDARRAY.Parser.ParseFrom(www.downloadHandler.data);
            isLoadClientPreDown = true;
            if (array != null) SetPreDownResource(array.rows);
        }
        else
        {
            Debug.LogError($"CSCheckResourceManager  LoadPreDownResource error :  {www.error}");
        }

        CompareVersion();
    }

    private void SetPreDownResource(IList<RESLISTANDROID> rows)
    {
        if (rows == null) return;
        RESLISTANDROID res;
        for (var i = 0; i < rows.Count; i++)
        {
            res = rows[i];
            if (res == null) continue;
            if (!ClientPreDownListDic.ContainsKey(res.name))
            {
                ClientPreDownListDic.Add(res.name, res);
            }
            else
                ClientPreDownListDic[res.name] = res;
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
            Debug.LogError($"Load Server Resource List Error : url: {www.url}  error: {www.error}");
        }

        isLoadServerResList = true;

        CompareVersion();
    }

    private bool CompareResourceMd5()
    {
        if (!File.Exists(AppUrlMain.mClientPreDownResourceFile))
        {
            LoadResInApk();
            CheckGameDown(false);
            return true;
        }
        else if (!ServerResourceVersion.Equals(CSVersionManager.Instance.ClientVersion.PreDownMD5))
        {
            mGame.StartCoroutine(LoadPreDownResource());
            CheckGameDown();
            return true;
        }
        else
        {
            isLoadClientPreDown = true;
            return CheckGameDown();
        }
    }

    /// <summary>
    /// 检测边玩边下，，返回值为是否需要解析服务器数据
    /// </summary>
    private bool CheckGameDown(bool isLoadApk = true)
    {
        if (!File.Exists(AppUrlMain.mClientGameDownResourceFile))
        {
            isLoadClientGameDown = true;
            if (isLoadApk)
            {
                LoadResInApk();
                return true;
            }

            CompareVersion();
        }
        else if (!ServerResourceVersion.Equals(CSVersionManager.Instance.ClientVersion.GameDownMD5))
        {
            mGame.StartCoroutine(LoadGameDownResource());
            return true;
        }
        else
        {
            isLoadClientGameDown = true;
            CompareVersion();
        }

        return false;
    }

    #endregion

    #region Load Client GameDownLoad

    private IEnumerator LoadGameDownResource()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.mClientGameDownResourceURL);
        yield return www.SendWebRequest();
        isLoadClientGameDown = true;

        if (string.IsNullOrEmpty(www.error))
        {
            RESLISTANDROIDARRAY array = RESLISTANDROIDARRAY.Parser.ParseFrom(www.downloadHandler.data);
            if (array != null) SetGameDownResource(array.rows);
        }
        else
        {
            Debug.LogError($"CSCheckResourceManager  LoadGameDownResource error :  {www.error}");
        }

        CompareVersion();
    }

    private void SetGameDownResource(IList<RESLISTANDROID> rows)
    {
        ClientGameDownListDic.Clear();
        RESLISTANDROID res;
        for (var i = 0; i < rows.Count; i++)
        {
            res = rows[i];
            if (!ClientGameDownListDic.ContainsKey(res.name))
                ClientGameDownListDic.Add(res.name, res);
            else
                ClientGameDownListDic[res.name] = res;
        }
    }

    #endregion

    private void CompareVersion()
    {
        if (!isLoadClientPreDown || !isLoadClientGameDown || !isLoadServerResList || isCompare) return;
        if (!File.Exists(AppUrlMain.mClientPreDownResourceFile))
        {
            SavePreDownResListNewText();
        }

        if (!File.Exists(AppUrlMain.mClientGameDownResourceFile))
        {
            SaveGameDownResListNewText();
        }

        if (ClientPreDownListDic.Count > 0 || ClientGameDownListDic.Count > 0)
            CSResUpdateManager.Instance.CompareResUpdate(ClientPreDownListDic, ClientGameDownListDic);

        FinishCheckVersion();
    }


    public void UpdatePreDownRes(Resource res)
    {
        if (res == null) return;
        if (ClientPreDownAppendListDic.ContainsKey(res.RelatePath))
            ClientPreDownAppendListDic[res.RelatePath].md5 = res.mMd5;
        else
        {
            RESLISTANDROID resList = new RESLISTANDROID()
            {
                name = res.RelatePath,
                md5 = res.mMd5
            };
            ClientPreDownAppendListDic.Add(res.RelatePath, resList);
        }
    }

    private int index;

    public void UpdateGameDownRes(Resource res)
    {
        if (res == null) return;
        index++;
        if (ClientGameDownAppendListDic.ContainsKey(res.RelatePath))
            ClientGameDownAppendListDic[res.RelatePath].md5 = res.mMd5;
        else
        {
            RESLISTANDROID resList = new RESLISTANDROID()
            {
                name = res.RelatePath,
                md5 = res.mMd5
            };
            ClientGameDownAppendListDic.Add(res.RelatePath, resList);
        }

        if (index >= 20)
        {
            index = 0;
            SaveGameDownResListText();
        }
    }

    public void SavePreDownResListText()
    {
        SaveResListText(ClientPreDownAppendListDic, AppUrlMain.mClientPreDownResourceFile);
        ClientPreDownAppendListDic.Clear();
    }

    public void SaveGameDownResListText()
    {
        SaveResListText(ClientGameDownAppendListDic, AppUrlMain.mClientGameDownResourceFile);
        ClientGameDownAppendListDic.Clear();
    }

    public void SavePreDownResListNewText()
    {
        SaveResListNewText(ClientPreDownListDic, AppUrlMain.mClientPreDownResourceFile);
    }

    public void SaveGameDownResListNewText()
    {
        SaveResListNewText(ClientGameDownListDic, AppUrlMain.mClientGameDownResourceFile);
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
        if(string.IsNullOrEmpty(dName)) return;

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
        var preDic = resDic.GetEnumerator();

        if (!File.Exists(path))
        {
            SaveResListNewText(resDic, path);
        }

        reslistarray.rows.Clear();

        while (preDic.MoveNext())
        {
            reslistarray.rows.Add(preDic.Current.Value);
        }

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

        preDic.Dispose();
    }
}*/