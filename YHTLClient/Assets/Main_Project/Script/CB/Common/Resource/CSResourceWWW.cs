//-----------------------------------------
//Resource
//author jiabao
//time 2016.3.28
//-----------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Policy;
using Main_Project.Script.Update;
using TABLE;
using UnityEngine.Networking;

public class CSResourceWWW : CSResource
{
    public AssetBundle assetBundle;
    public event Action<CSResourceWWW> onLoadedTable;
    private bool isReloading = false;
    private byte reloadTime = 0;
    private float mapBeginGetDataEndTime = 0;
    private string relatePath;

    public CSResourceWWW(string name, string path, int type) : base(name, path, type)
    {
        Path = path;
    }

    public override void Load()
    {
        if (IsDone)
        {
            base.onLoaded.CallBack(this);
            base.onLoaded.Clear();
            if (onLoadedTable != null)
            {
                onLoadedTable(this);
            }
        }
        else
        {
            LoadProc(Path);
        }
    }

    private void LoadProc(string path)
    {
        relatePath = path.Replace(SFOut.URL_mClientResURL, "");
        relatePath = relatePath.Substring(relatePath.IndexOf('/') + 1);

        if (!SFOut.IsLoadLocalRes)
        {
            bool isNeed = CSResUpdateManager.Instance.CheckIsNeedDownload(relatePath);
            if (isNeed)
            {
                DealNeedWaitHotUpdate();
                return;
            }
        }

        IsDone = false;
        CoroutineManager.DoCoroutine(GetData(path));
    }

    private IEnumerator GetData(string path)
    {
        if (LocalType == ResourceType.Map || IsUIRes(this))
        {
            mapBeginGetDataEndTime = UnityEngine.Time.time + 2;
        }

        if (LocalType == ResourceType.MapBytes || LocalType == ResourceType.TableBytes)
        {
            path = $"file://{path}";

            UnityWebRequest www = UnityWebRequest.Get(path);

            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                if (FNDebug.developerConsoleVisible) FNDebug.Log(www.error + " " + path);

                if ( /*SFOut.IGame != null &&*/ /*CSResourceWWW.IsLoadLocalRes*/SFOut.IsLoadLocalRes)
                {
                    OnLoadedErrorProc();
                }
                else
                {
                    DealNeedWaitHotUpdate();
                }
            }
            else
            {
                MirroyBytes = www.downloadHandler.data;
                LoadFinish();

                www.Dispose();
            }
        }
        else
        {
            bool isLowMemory = LocalType != ResourceType.ScaleMap ? SFOut.IsLowMemory : false;

            if (!isLowMemory)
            {
                AssetBundleCreateRequest www;
                if(File.Exists(path))
                {
                    www = AssetBundle.LoadFromFileAsync(path);
                    yield return www;
                    assetBundle = www.assetBundle;
                }else
                {
                    assetBundle = null;
                }
                
                if (assetBundle == null)
                {
                    if (SFOut.IsLoadLocalRes)
                    {
                        OnLoadedErrorProc();
                    }
                    else
                    {
                        DealNeedWaitHotUpdate();

                        if (LocalType == ResourceType.Map || IsUIRes(this))
                        {
                            mapBeginGetDataEndTime = 0;
                            yield break;
                        }
                    }
                }
                else
                {
                    // Or retrieve results as binary data  
                    //assetBundle = DownloadHandlerAssetBundle.GetContent(www);
                    //IsDone = true;
                    //www.Dispose();

                    yield return SyncLoadMainAsset();
                }
            }
            else
            {
                if (FNDebug.developerConsoleVisible) FNDebug.Log("内存不足 " + path);

                OnLoadedErrorProc();
            }
        }

        if (LocalType == ResourceType.Map || IsUIRes(this))
        {
            if (MirrorObj == null)
            {
                yield break;
            }
        }

        mapBeginGetDataEndTime = 0;
    }

    public bool IsUIRes(CSResource res)
    {
        if (res == null) return false;

        if (res.LocalType == ResourceType.UIEffect ||
            res.LocalType == ResourceType.UITexture ||
            res.LocalType == ResourceType.UIPlayer ||
            res.LocalType == ResourceType.UIWeapon ||
            res.LocalType == ResourceType.UIWing ||
            res.LocalType == ResourceType.ResourceRes ||
            res.LocalType == ResourceType.MiniMap ||
            res.LocalType == ResourceType.UIMonster) return true;
        return false;
    }

    public override void UpdateLoading()
    {
        if (mapBeginGetDataEndTime != 0 && UnityEngine.Time.time > mapBeginGetDataEndTime)
        {
            if (reloadTime >= 2) //客户端下载两次失败，就不在下载
            {
                mapBeginGetDataEndTime = 0;
                OnLoadedErrorProc();
            }
            else
            {
                isReloading = true;
                reloadTime++;
            }
        }

        if (isReloading)
        {
            isReloading = false;

            //SFOut.Game.StartCoroutine(GetData(Path));
            CoroutineManager.DoCoroutine(GetData(Path));
        }
    }

    public void DealNeedWaitHotUpdate()
    {
        loadedTime = UnityEngine.Time.time;
        IsDone = false;
        CSResourceManager.Instance.wwwLoaded(this);
        DownloadFromServer();
    }

    public override void ResHotUpdateCallBack_HttpLoad()
    {
        isHotLoading = false;
        IsDone = false;
        //SFOut.Game.StartCoroutine(GetData(Path));
        CoroutineManager.DoCoroutine(GetData(Path));
    }

    private void LoadFinish(bool isFromLocalLoad = false)
    {
        if (assetBundle != null)
            assetBundle.Unload(false);

        if (isFromLocalLoad && LocalType == ResourceType.Map)
        {
            if (MirrorObj == null)
            {
                return;
            }
        }

        loadedTime = UnityEngine.Time.time;
        IsDone = true;

        CSResourceManager.Singleton.wwwLoaded(this);

        base.onLoaded.CallBack(this);
        base.onLoaded.Clear();

        if (onLoadedTable != null)
        {
            onLoadedTable(this);
        }
    }

    private IEnumerator SyncLoadMainAsset()
    {
        if ( /*SFOut.Game == null || */assetBundle == null) yield break;

        string[] strs = assetBundle.GetAllAssetNames();

        if (strs.Length > 0)
        {
            if (LocalType == ResourceType.ScaleMap || mResourceAssistType == ResourceAssistType.ForceLoad)
            {
                MirrorObj = assetBundle.LoadAsset(strs[0]);
                LoadFinish();
            }
            else
            {
                string arName = strs[0];
                AssetBundleRequest ar = assetBundle.LoadAssetAsync(arName);
                yield return ar;
                MirrorObj = ar.asset;
                ar = null;
                LoadFinish(true);
            }
        }
    }

    private void OnLoadedErrorProc()
    {
        loadedTime = UnityEngine.Time.time;
        IsDone = true;
        base.onLoaded.CallBack(this);
        base.onLoaded.Clear();
        if (onLoadedTable != null)
            onLoadedTable(this);
        mapBeginGetDataEndTime = 0;
    }

    public void ClearTablCallBack()
    {
        onLoadedTable = null;
    }

    private void DownloadFromServer()
    {
        isHotLoading = true;
        RESOURCELIST resource = CSResUpdateManager.Instance.GetResource(relatePath);
        CSResUpdateManager.Instance.AddToDownloadQueue(resource, DownloadType.GamingDownload);
    }
}