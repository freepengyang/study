
//-------------------------------------------------------------------------
//Resource load
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

//using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using Main_Project.Script.Update;
using UnityEngine.SceneManagement;

public class CSResourceManager : CSGameMgrBase2<CSResourceManager>
{
    public override bool IsDonotDestroy
    {
        get
        {
            return true;
        }
    }

    public bool mIsChangingScene = false;
    public bool IsChangingScene
    {
        get { return mIsChangingScene; }
        set
        {
            if (mIsChangingScene == value) return;
            mIsChangingScene = value;
            if (onChangeScene != null)
                onChangeScene(mIsChangingScene);
        }
    }

    public static Dictionary<long, string> staticResPathDic = new Dictionary<long, string>();

    private Dictionary<string, CSResource> waitingQueueDic = new Dictionary<string, CSResource>(48);//等待加载的列表

    private List<CSResource> waitingWWWQueue = new List<CSResource>(48);//正在等待加载的WWW资源

    private Dictionary<string, CSResource> loadingQueue = new Dictionary<string, CSResource>(16);//正在下载的WWW资源

    private List<CSResource> loadingQueueList = new List<CSResource>(16);

    private Dictionary<string, CSResource> loadedQueue = new Dictionary<string, CSResource>(128);//下载完成的资源，包括WWW和Resources

    private List<CSResource> assistList = new List<CSResource>(128);

    private Dictionary<string, CSResource> inDestroyDic = new Dictionary<string, CSResource>(16);
    public float nextDestroyTime = 0;

    private CSResource curLastLoadingRes = null;//记录当前最后一个下载的资源（如果是队列下载，那么只有等待这个资源加载完成，才下载下一个）
    private List<CSResource> waitReloadingList = new List<CSResource>();
    private int mAssistType = ResourceAssistType.None;
    bool mIsUnLoadUnUsedAssets = false;
    float mUnloadUnusedAssetsTime = 0;
    int mTouchCount = -2;
    bool mIsMousePress = false;

    //public static System.Action<float> UpdateProgress = null;

    private static CSResourceManager mSingleton;

    public static CSResourceManager Singleton
    {
        get
        {
            return mSingleton;
        }
        set
        {
            mSingleton = value;
            if(mSingleton == null)
            {
                if (FNDebug.developerConsoleVisible)
                {
                    FNDebug.Log("<color=#ff0000>CSResourceManager Singleton is null </color>");
                }
            }
        }
    }

    #region 当前加载的模型图集数量
    public int curPlayerAtlasNum = 0;
    public int curWeaponAtlasNUm = 0;
    public int curMountAtlasNUm = 0;
    #endregion

    private int mInitMaxPlayerAtlasNum = 10;
    public int InitMaxPlayerAtlasNum
    {
        get { return mInitMaxPlayerAtlasNum; }
        set { mInitMaxPlayerAtlasNum = value; }
    }

    public int maxPlayerAtlasNum = int.MaxValue;
    public int MaxPlayerAtlasNum
    {
        get { return maxPlayerAtlasNum; }
        set { maxPlayerAtlasNum = value; }
    }

    private int mInitMaxWeaponAtlasNum = 10;
    public int InitMaxWeaponAtlasNum
    {
        get { return mInitMaxWeaponAtlasNum; }
        set { mInitMaxWeaponAtlasNum = value; }
    }

    public int maxWeaponAtlasNum = int.MaxValue;
    public int MaxWeaponAtlasNum
    {
        get { return maxWeaponAtlasNum; }
        set { maxWeaponAtlasNum = value; }
    }

    private int mInitMaxMountAtlasNum = 26;
    public int InitMaxMountAtlasNum
    {
        get { return mInitMaxMountAtlasNum; }
        set { mInitMaxMountAtlasNum = value; }
    }

    public int maxMountAtlasNum = int.MaxValue;
    public int MaxWingAtlasNum
    {
        get { return maxMountAtlasNum; }
        set { maxMountAtlasNum = value; }
    }
    private int mInitMaxPlayerNum = 100;
    public int InitMaxPlayerNum
    {
        get { return mInitMaxPlayerNum; }
        set { mInitMaxPlayerNum = value; }
    }
    public int maxPlayerNum = int.MaxValue;//玩家上限
    public int MaxPlayerNum
    {
        get { return maxPlayerNum; }
        set { maxPlayerNum = value; }
    }
    public int disposeTime
    {
        get
        {
#if UNITY_EDITOR || UNITY_ANDROID
            return 1;
#elif UNITY_IOS
            return 0;
#endif
            return 1;
        }
    }
    public int maxSyncLoadingNum
    {
        get
        {
#if UNITY_EDITOR || UNITY_ANDROID
            return 10;

#elif UNITY_IOS
            return 5;
#endif
            return 10;
        }
    }
    public bool IsUnLoadUnUsedAssets
    {
        get { return mIsUnLoadUnUsedAssets; }
        set { mIsUnLoadUnUsedAssets = value; }
    }

    public delegate void OnChangeScene(bool isChangeScene);
    public event OnChangeScene onChangeScene;
    int lastUnuseAssetsResCount = 0;

    //public static map.PositionChangeReason ChangeSceneReason = map.PositionChangeReason.MoveTooQuick;
    MainEventHanlderManager mClientEventHandler;
    public override void Awake()
    {
        base.Awake();
        if (Singleton == null) Singleton = this;
        maxPlayerNum = int.MaxValue;
        maxMountAtlasNum = int.MaxValue;
        maxWeaponAtlasNum = int.MaxValue;
        maxPlayerAtlasNum = int.MaxValue;
        mTouchCount = -2;
        //TableLoader.Instance.onLoadAllTable += OnLoadAllTable;
    }

    void OnLoadAllTable()
    {
        InitMaxAtlas();
    }

    public void InitMaxAtlas()
    {
        ////TABLE.SUNDRY tbl;
        ////if (SundryTableManager.Instance.TryGetValue(83, out tbl))
        //{
        //   // string[] strs = tbl.effect_name.Split('#');
        //    //if (strs.Length == 4)
        //    {
        //int.TryParse(strs[0], out mInitMaxPlayerAtlasNum);
        //int.TryParse(strs[1], out mInitMaxWeaponAtlasNum);
        //int.TryParse(strs[2], out mInitMaxMountAtlasNum);
        //int.TryParse(strs[3], out mInitMaxPlayerNum);

        //        if (maxPlayerAtlasNum == int.MaxValue) maxPlayerAtlasNum = mInitMaxPlayerAtlasNum;
        //        if (maxWeaponAtlasNum == int.MaxValue) maxWeaponAtlasNum = mInitMaxWeaponAtlasNum;
        //        if (maxMountAtlasNum == int.MaxValue) maxMountAtlasNum = mInitMaxMountAtlasNum;
        //        if (maxPlayerNum == int.MaxValue) maxPlayerNum = mInitMaxPlayerNum;
        //    }
        //}
    }

    public void UpdateMaxAtlas(int pAtlas, int weaponAtlas, int mountAtla, int pNum)
    {
        maxPlayerAtlasNum = pAtlas;
        maxWeaponAtlasNum = weaponAtlas;
        maxMountAtlasNum = mountAtla;
        maxPlayerNum = pNum;
    }

    void Update()
    {
        loadWWWLine();
        DetectMemory();
        UpdateLoading();
        UpdateReloadErrorRes();
        UpdateDestroyQueue();
    }

    void UpdateDestroyQueue()
    {
        if (IsChangingScene) return;
        if (nextDestroyTime == 0) return;
        if (Time.time < nextDestroyTime) return;
#if UNITY_EDITOR||UNITY_ANDROID
        nextDestroyTime = Time.time + 0.5f;
#elif UNITY_IOS
        nextDestroyTime = Time.time + Time.deltaTime;
#endif
        if(inDestroyDic.Count == 0) return;
        var cur = inDestroyDic.GetEnumerator();
        while (cur.MoveNext())
        {
            bool isDestroy = DestroyResource(cur.Current.Key, false, true, true);
#if UNITY_EDITOR||UNITY_ANDROID
            if (!isDestroy) break;
#endif
            inDestroyDic.Remove(cur.Current.Key);
            break;
        }
    }

    void UpdateLoading()
    {
        if(loadingQueueList.Count == 0) return;
        for (int i = loadingQueueList.Count - 1; i >= 0; i--)
        {
            CSResource res = loadingQueueList[i];
            if (res != null)
            {
                res.UpdateLoading();
            }
        }
    }

    void UpdateReloadErrorRes()
    {
        if (waitReloadingList.Count == 0) return;

        CSResource res = waitReloadingList[0];

        if (Time.time - res.loadedTime > 1)
        {
            waitReloadingList.RemoveAt(0);
            res.ResHotUpdateCallBack_HttpLoad();

            if (!loadingQueueList.Contains(res))
            {
                loadingQueueList.Add(res);
            }
        }
    }

    private void DetectMemory()
    {
        if (CSConstant.IsLanuchMainPlayer)
        {
            if(!CSConstant.isMainPlayerMoving && !IsChangingScene)
            {
                if (mTouchCount != Input.touchCount)
                {
                    if (Input.touchCount > 0)
                    {
                        mUnloadUnusedAssetsTime = 0;
                    }
                    else
                    {
                        mUnloadUnusedAssetsTime = Time.time;
                    }
                    mTouchCount = Input.touchCount;
                }
                bool isMousePress = Input.GetMouseButton(0);
                if (mIsMousePress != isMousePress)
                {
                    if (isMousePress)
                    {
                        mUnloadUnusedAssetsTime = 0;
                    }
                    else
                    {
                        mUnloadUnusedAssetsTime = Time.time;
                    }
                    mIsMousePress = isMousePress;
                }

                if (mUnloadUnusedAssetsTime != 0 && Time.time - mUnloadUnusedAssetsTime > 59)
                {
                    mUnloadUnusedAssetsTime = Time.time;
                    mIsUnLoadUnUsedAssets = true;
                }
            }
            else
            {
                mTouchCount = -2;
                mIsMousePress = false;
                mUnloadUnusedAssetsTime = 0;
            }

            if(CSConstant.lastStandTimeUnloadAsset != 0 && Time.time - CSConstant.lastStandTimeUnloadAsset > 120)
            {
                CSConstant.lastStandTimeUnloadAsset = Time.time;
                mIsUnLoadUnUsedAssets = true;
            }
        }

        if (mIsUnLoadUnUsedAssets)
        {
            mIsUnLoadUnUsedAssets = false;
            ResourcesUnloadUnusedAssets();
        }
    }

    public void ResourcesUnloadUnusedAssets()
    {
        lastUnuseAssetsResCount = 0;
        Resources.UnloadUnusedAssets();
        mUnloadUnusedAssetsTime = Time.time;
        mTouchCount = -2;
        mIsMousePress = false;
        if(CSConstant.IsLanuchMainPlayer)
        {
            CSConstant.lastStandTimeUnloadAsset = Time.time;
        }
    }

    private void loadWWWLine()
    {
        if (waitingWWWQueue.Count == 0) return;

        CSResource res = waitingWWWQueue[0];
        if (res != null)
        {
            if (res.AssistType == ResourceAssistType.ForceLoad)
            {
                curLastLoadingRes = res;
                waitingWWWQueue.RemoveAt(0);
                res.waitingCallBackCount = 0;
                RemoveWaitingQueueDic(res);
                AddLoadingQueue(res);
                //TODO 2020.7.19 暂时注释 (原因: 底层已经wwwLoaded一次)
                // res.onLoaded.AddFrontCallBack(wwwLoaded);

                res.Load();
            }
            else
            {
                if (loadingQueue.Count > maxSyncLoadingNum) return;
                curLastLoadingRes = res;
                waitingWWWQueue.RemoveAt(0);
                res.waitingCallBackCount = 0;
                RemoveWaitingQueueDic(res);
                AddLoadingQueue(res);
                //TODO 2020.7.19 暂时注释 (原因: 底层已经wwwLoaded一次)
                //res.onLoaded.AddFrontCallBack(wwwLoaded);
                res.Load();
            }
        }
        else
        {
            waitingWWWQueue.RemoveAt(0);
        }
    }

    public void wwwLoaded(CSResource res)
    {
        CheckClearCurLastLoadingRes(res);
        RemoveLoadingQueue(res);
        AddLoadedQueue(res);
    }

    void AddAtlasCount(CSResource res)
    {
        if (res == null) return;

        switch (res.LocalType)
        {
            case ResourceType.PlayerAtlas:
                curPlayerAtlasNum++;
                break;
            case ResourceType.WeaponAtlas:
                curWeaponAtlasNUm++;
                break;
            case ResourceType.MountAtlas:
                curMountAtlasNUm++;
                break;
        }
    }

    void RemoveAtlasCount(CSResource res)
    {
        if (res == null) return;

        switch (res.LocalType)
        {
            case ResourceType.PlayerAtlas:
                curPlayerAtlasNum--;
                break;
            case ResourceType.WeaponAtlas:
                curWeaponAtlasNUm--;
                break;
            case ResourceType.MountAtlas:
                curMountAtlasNUm--;
                break;
        }
    }

    void CheckClearCurLastLoadingRes(CSResource res)
    {
        if (res == curLastLoadingRes)
        {
            curLastLoadingRes = null;
        }
    }

    public CSResource GetLoadingRes(string path)
    {
        if (loadingQueue.ContainsKey(path))
        {
            return loadingQueue[path];
        }
        return null;
    }

    public CSResource GetWaitingQueueRes(string path)
    {
        if (waitingQueueDic.ContainsKey(path))
        {
            return waitingQueueDic[path];
        }
        return null;
    }

    public CSResource GetRes(string path)
    {
        CSResource res = GetLoadedRes(path);
        if (res != null) return res;
        res = GetLoadingRes(path);
        if (res != null) return res;
        res = GetWaitingQueueRes(path);
        if (res != null) return res;
        return null;
    }

    public CSResource GetLoadedRes(string path)
    {
        if (loadedQueue.ContainsKey(path))
        {
            return loadedQueue[path];
        }
        return null;
    }

    void AddWaitingQueueDic(CSResource res)
    {
        if (!waitingQueueDic.ContainsKey(res.Path))
        {
            waitingQueueDic.Add(res.Path, res);
        }
    }

    void RemoveWaitingQueueDic(CSResource res)
    {
        if (waitingQueueDic.ContainsKey(res.Path))
        {
            waitingQueueDic.Remove(res.Path);
        }
    }

    public void RemoveWaitingQueueDic(string path)
    {
        if (waitingQueueDic.ContainsKey(path))
        {
            CSResource res = waitingQueueDic[path];
            res.waitingCallBackCount--;
            if (res.waitingCallBackCount <= 0 && res.IsCanBeDelete)
            {
                waitingQueueDic.Remove(path);
                waitingWWWQueue.Remove(res);
            }
        }
    }

    void AddLoadingQueue(CSResource res)
    {
        if (!loadingQueue.ContainsKey(res.Path))
        {
            loadingQueue.Add(res.Path, res);
        }
        if (!loadingQueueList.Contains(res))
        {
            loadingQueueList.Add(res);
        }
    }

    void RemoveLoadingQueue(CSResource res)
    {
        if (loadingQueue.ContainsKey(res.Path))
        {
            loadingQueue.Remove(res.Path);
            
        }

        loadingQueueList.Remove(res);
    }

    void AddLoadedQueue(CSResource res)
    {
        if (!loadedQueue.ContainsKey(res.Path))
        {
            loadedQueue.Add(res.Path, res);
        }
    }

    public void ResHotUpdateCallBack_HttpLoad(string path, bool isSucceed)
    {
        if (string.IsNullOrEmpty(path)) return;

        CSResource res = GetRes(path);

        ResHotUpdateCallBack_HttpLoad(res, isSucceed);
    }

    public void ResHotUpdateCallBack_HttpLoad(CSResource res, bool isSucceed)
    {
        if (res == null) return;

        if (isSucceed)
        {
            res.ResHotUpdateCallBack_HttpLoad();
            if (!loadingQueueList.Contains(res)) loadingQueueList.Add(res);
        }
        else
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("ResHotUpdateCallBack_HttpLoad error");
            res.loadedTime = Time.time;
            waitReloadingList.Remove(res);
            waitReloadingList.Add(res);
        }
    }

    public void BeginQueueDeal()
    {
        mAssistType = ResourceAssistType.QueueDeal;
    }

    public void EndQueueDeal()
    {
        mAssistType = ResourceAssistType.None;
    }

    public string GetKeyPath(long key)
    {
        if (staticResPathDic.ContainsKey(key))
        {
            return staticResPathDic[key];
        }
        return string.Empty;
    }

    /// <summary>
    /// path 通过CSResource.GetPath获得
    /// </summary>
    /// <param name="key"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public void AddKeyPath(long key, string path)
    {
        if (staticResPathDic.ContainsKey(key))
        {
            return;
        }
        staticResPathDic.Add(key, path);
    }

    public CSResource AddQueue(string name, int type, Action<CSResource> onLoadCallBack,
        int assistType, bool isPath = false, long key = 0)
    {
        string path = isPath ? name : CSResource.GetPath(name, type, false);
        inDestroyDic.Remove(path);
        int aType = assistType != ResourceAssistType.None ? assistType : mAssistType;
        CSResource res = GetLoadedRes(path);
        if (res == null) res = GetLoadingRes(path);
        if (res == null)
        {
            res = GetWaitingQueueRes(path);

            if (res != null)
            {
                if (aType > res.AssistType) res.AssistType = aType;//优先级 只曾不减，防止主角正在等待下载时，来了一个其他玩家优先级较低的下载
                AdjustProri(res);
                res.waitingCallBackCount++;
            }
        }

        if (res == null)
        {
            res = new CSResourceWWW(name, path, type);
            if ((int)aType > (int)res.AssistType) res.AssistType = aType;
            res.waitingCallBackCount = 1;
            AdjustProri(res);
            AddWaitingQueueDic(res);
            AddAtlasCount(res);
        }
        res.Key = key;
        if (onLoadCallBack != null)
        {
            if (res.IsDone)
            {
                if (res.isHotLoading)
                {
                    res.onLoaded -= onLoadCallBack;
                    res.onLoaded += onLoadCallBack;
                }
                else
                {
                    if (onLoadCallBack != null)
                        onLoadCallBack(res);
                }
            }
            else
            {
                res.onLoaded -= onLoadCallBack;
                res.onLoaded += onLoadCallBack;
            }
        }
        return res;
    }

    void AdjustProri(CSResource res)
    {
        if (waitingQueueDic.ContainsKey(res.Path))
        {
            waitingWWWQueue.Remove(res);
            int index = FindProriIndex(res);
            waitingWWWQueue.Insert(index, res);
        }
        else
        {
            int index = FindProriIndex(res);
            waitingWWWQueue.Insert(index, res);
        }
    }

    int FindProriIndex(CSResource newRes)
    {
        int index = -1;
        bool isFind = false;

        for (int i = 0; i < waitingWWWQueue.Count; i++)
        {
            CSResource res = waitingWWWQueue[i];
            if ((int)newRes.AssistType > (int)res.AssistType)
            {
                index = i;
                isFind = true;
                break;
            }
        }
        if (!isFind) return waitingWWWQueue.Count;
        return index + 1;
    }

    //------------------CSResource-WWW-----------------------------------------------------


    public void ResetMapTexCanDelete()
    {
        var cur = loadedQueue.GetEnumerator();
        while (cur.MoveNext())
        {
            if (cur.Current.Value != null && cur.Current.Value.LocalType == ResourceType.Map)
            {
                cur.Current.Value.IsCanBeDelete = true;
            }
        }
    }

    bool IsCanDeleteChangeScene(CSResource res)
    {
        if (res.IsCanBeDelete) return true;
        if (!res.isResValid && CSResUpdateManager.Instance != null && !CSResUpdateManager.Instance.IsDownloading(res.GetRelatePath())) return true;
        return false;
    }

    public void DestroyAllResource(bool isWithoutChangeScene = false)
    {
        curLastLoadingRes = null;
        using (var cur = loadedQueue.GetEnumerator())
        {
            while (cur.MoveNext())
            {
                if (IsUIRes(cur.Current.Value)) continue;

                if (Time.time - cur.Current.Value.loadedTime <= disposeTime) continue;

                if (IsCanDeleteChangeScene(cur.Current.Value))//如果一个资源下载失败，就算他被标记成不可被删除在切换场景的时候也将这个引用删除，让其在用到的时候在下载
                {

                    if (isWithoutChangeScene && cur.Current.Value.isResValid && cur.Current.Value.LocalType != ResourceType.MapBytes)//加载完成后就释放了
                    {
                        if (cur.Current.Value.isResValid)
                        {
                            if (!inDestroyDic.ContainsKey(cur.Current.Key))
                            {
                                inDestroyDic.Add(cur.Current.Key, cur.Current.Value);
                            }
                            cur.Current.Value.onLoaded.Clear();
                        }
                        else
                        {
                            DestroyResource(cur.Current.Key, false, false);
                            assistList.Add(cur.Current.Value);
                        }
                    }
                    else
                    {
                        DestroyResource(cur.Current.Key, false, false);
                        assistList.Add(cur.Current.Value);
                    }
                }
            }
        }

        for (int i = 0; i < assistList.Count; i++)
        {
            if (loadedQueue.ContainsKey(assistList[i].Path))
            {
                loadedQueue.Remove(assistList[i].Path);
            }
        }

        assistList.Clear();

        for (int i = waitingWWWQueue.Count - 1; i >= 0; i--)
        {
            CSResource res = waitingWWWQueue[i];

            if (IsUIRes(res) || !res.IsCanBeDelete)
            {
                assistList.Add(res);
            }
        }

        //waitingWWWQueue.Release();
        waitingQueueDic.Clear();

        for (int i = 0; i < assistList.Count; i++)
        {
            CSResource res = assistList[i];

            waitingWWWQueue.Add(res);

            if (!waitingQueueDic.ContainsKey(res.Path))
            {
                waitingQueueDic.Add(res.Path, res);
            }
        }

        for (int i = loadingQueueList.Count - 1; i >= 0; i--)
        {
            CSResource res = loadingQueueList[i];
            if (IsUIRes(res)) continue;
            if (res != null && res.IsCanBeDelete)
            {
                res.ReleaseCallBack();
                res.onLoaded.AddFrontCallBack(wwwLoaded);
            }
        }

        staticResPathDic.Clear();
        assistList.Clear();
        curPlayerAtlasNum = 0;
        curWeaponAtlasNUm = 0;
        curMountAtlasNUm = 0;
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
            res.LocalType == ResourceType.MonsterAtlas || //boss界面使用
            res.LocalType == ResourceType.MiniMap || 
            res.LocalType == ResourceType.UIMonster) return true;
        return false;
    }

    /// <summary>
    /// 注意二进制文件使用完，destroy 减少占用堆内存
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isUnLoadUnUsedAssets"></param>
    /// <param name="isRemoveFromDic"></param>
    public bool DestroyResource(string path, bool isUnLoadUnUsedAssets = false, bool isRemoveFromDic = true, bool isRemoveCallBack = false)
    {
        if (string.IsNullOrEmpty(path)) return false;

        if (loadedQueue.ContainsKey(path))
        {
            //xxxx
            CSResourceWWW res = loadedQueue[path] as CSResourceWWW;


            if (!IsCanDeleteChangeScene(res)) return false;

            if (IsUIRes(res)) res.mCount--;
            if (IsUIRes(res) && res.mCount > 0) return false;

            if (res.LocalType == ResourceType.PlayerAtlas ||
                res.LocalType == ResourceType.SkillAtlas ||
                res.LocalType == ResourceType.WeaponAtlas ||
                res.LocalType == ResourceType.MountAtlas)
            {
#if UNITY_EDITOR||UNITY_ANDROID
                if (res.isResValid)
                {
                    if (lastUnuseAssetsResCount >= 50)
                    {
                        return false;
                    }
                    lastUnuseAssetsResCount++;
                }
#endif
            }

            RemoveAtlasCount(res);
            GameObject go = res.MirrorObj as GameObject;

            if (go != null)
            {
                DestroyUIAtals(go);

                DestroyCSSprite(go);

                DestroyUISprite(go);
            }

            Texture tex = res.MirrorObj as Texture;

            if (tex != null)
            {
                DestroyTexture(tex);
            }

            AudioClip aClip = res.MirrorObj as AudioClip;

            if (aClip != null)
            {
                DestroyAudioClip(aClip);
            }

            RemoveRefRes(res);
            res.MirrorObj = null;
            res.MirroyBytes = null;
            res.ClearTablCallBack();
        }
        else
        {
            CSResource res = GetLoadingRes(path);
            if (res != null)
            {
                if (res.IsCanBeDelete)
                {
                    res.ReleaseCallBack();
                    res.onLoaded.AddFrontCallBack(wwwLoaded);
                }
            }
            else
            {
                res = GetWaitingQueueRes(path);
                if (res != null)
                {
                    if (res.IsCanBeDelete)
                    {
                        res.ReleaseCallBack();
                    }
                }
            }
        }

        if (isRemoveFromDic)
        {
            loadedQueue.Remove(path);
            RemoveWaitingQueueDic(path);
        }

        mIsUnLoadUnUsedAssets = isUnLoadUnUsedAssets;
        return true;
    }

    private void DestroyUIAtals(GameObject go)
    {
        UIAtlas[] atlass = go.GetComponentsInChildren<UIAtlas>(true);
        for (int i = 0; i < atlass.Length; i++)
        {
            UIAtlas atlas = atlass[i];
            if (atlas != null && atlas.spriteMaterial != null)
            {
                if (atlas.spriteMaterial.mainTexture != null)
                {
                    UnityEngine.Object.DestroyImmediate(atlas.spriteMaterial.mainTexture, true);
                }
                if (atlas.spriteMaterial.HasProperty("_AlphaTex"))
                {
                    Texture alphaTex = atlas.spriteMaterial.GetTexture("_AlphaTex");
                    if (alphaTex != null)
                    {
                        UnityEngine.Object.DestroyImmediate(alphaTex, true);
                    }
                }
                atlas.HasBeenDestroy = true;
            }
        }
    }

    private void DestroyCSSprite(GameObject go)
    {
        CSSprite[] sps = go.GetComponentsInChildren<CSSprite>(true);
        for (int i = 0; i < sps.Length; i++)
        {
            CSSprite sp = sps[i];
            UIAtlas atlas = sp != null ? sp.Atlas : null;
            if (atlas != null && atlas.spriteMaterial != null)
            {
                if (atlas.spriteMaterial.mainTexture != null)
                {
                    UnityEngine.Object.DestroyImmediate(atlas.spriteMaterial.mainTexture, true);
                }

                if (atlas.spriteMaterial.HasProperty("_AlphaTex"))
                {
                    Texture alphaTex = atlas.spriteMaterial.GetTexture("_AlphaTex");
                    if (alphaTex != null)
                    {
                        UnityEngine.Object.DestroyImmediate(alphaTex, true);
                    }
                }
                atlas.HasBeenDestroy = true;
            }
        }
    }

    private void DestroyUISprite(GameObject go)
    {
        UISprite[] uisps = go.GetComponentsInChildren<UISprite>(true);
        for (int i = 0; i < uisps.Length; i++)
        {
            UISprite sp = uisps[i];
            UIAtlas atlas = sp != null ? sp.atlas : null;
            if (atlas != null && atlas.spriteMaterial != null)
            {
                if (atlas.spriteMaterial.mainTexture != null)
                {
                    UnityEngine.Object.DestroyImmediate(atlas.spriteMaterial.mainTexture, true);
                }

                if (atlas.spriteMaterial.HasProperty("_AlphaTex"))
                {
                    Texture alphaTex = atlas.spriteMaterial.GetTexture("_AlphaTex");
                    if (alphaTex != null)
                    {
                        UnityEngine.Object.DestroyImmediate(alphaTex, true);
                    }
                }
                atlas.HasBeenDestroy = true;
            }
        }
    }

    private void DestroyTexture(Texture tex)
    {
        UnityEngine.Object.DestroyImmediate(tex, true);
    }

    private void DestroyAudioClip(AudioClip aClip)
    {
        UnityEngine.Object.DestroyImmediate(aClip, true);
    }

    private void RemoveRefRes(CSResource res)
    {
        if (res == null) return;
        res.onLoaded.Clear();
    }

    public static string GetIsLoadLocalResPath(string applicationDataPath, int LocalType, string fileName, bool isAssetBundle, string assetbunldeStr)
    {
        string path = string.Empty;
#if UNITY_EDITOR

        if (SFOut.IsLoadLocalRes)
        {
            if (UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup == UnityEditor.BuildTargetGroup.iOS)
            {
                path = applicationDataPath.Replace("YHTLClient/Assets", "Normal/zt_android/");
            }
            else
            {
                path = applicationDataPath.Replace("YHTLClient/Assets", "Normal/zt_android/");
            }

            path = CSStringBuilder.Append(path, CSResource.GetModelTypePath(LocalType), fileName, isAssetBundle ? assetbunldeStr : "").ToString();
        }
#endif
        return path;
    }

    public void DestroyCallBack(string mPath)
    {
        if (string.IsNullOrEmpty(mPath)) return;

        if (loadedQueue.ContainsKey(mPath))
        {
            CSResourceWWW res = loadedQueue[mPath] as CSResourceWWW;
            if (res != null)
            {
                res.ClearTablCallBack();
            }
        }
    }

    public void ReleaseCallBack(string mPath, Action<CSResource> onLoadCallBack)
    {
        if (string.IsNullOrEmpty(mPath) || onLoadCallBack == null) return;

        if (loadedQueue.ContainsKey(mPath))
        {
            CSResourceWWW res = loadedQueue[mPath] as CSResourceWWW;
            if (res != null) res.onLoaded -= onLoadCallBack;
        }
    }
}
