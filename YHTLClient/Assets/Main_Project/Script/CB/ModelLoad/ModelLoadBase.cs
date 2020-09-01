using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelLoadBase
{
    public class ModelLoadData
    {
        public int structure;
        public GameObject go;
        public UIAtlas atlas;
        public int type;
        public int assistType;
    }

    protected int curMotion = 0;
    protected int curDirection = 0;
    protected int lastBodyModel = 0;
    protected int lastWeapon = 0;
    protected int lastWing = 0;

    protected CSAction Action { get; set; }

    protected int BodyId { get; set; }

    protected int WeaponId { get; set; }

    protected int WingId { get; set; }


    public int eAvatarType = EAvatarType.None;

    public bool isSetEmpty = false;

    /// <summary>
    /// key:modelID*10000+motion*100+direction 如果是技能modelID*10000+direction
    /// </summary>
    protected static Dictionary<long, ModelLoadData> mLoadedDic = new Dictionary<long, ModelLoadData>();

    protected static Dictionary<int, Dictionary<int, int>> mAtlasNumDic = new Dictionary<int, Dictionary<int, int>>();

    public Action<ModelLoadBase> onFinishAll;//特别注意：在回调里面不能加载模型，不然onFinishAll（this）;onFinishAll=null;会把回调清空
    public Action<ModelLoadBase> onExternalFinishCallBack;
    /// <summary>
    /// 所有key加载完成后才进行回调
    /// </summary>
    protected CSBetterList<long> mObjectList = new CSBetterList<long>();
    private CSBetterList<bool> mObjectFinishStateList = new CSBetterList<bool>();
    private List<string> mHasUseModelResList = new List<string>();
    private long[] mStructures = new long[10];
    private CSBetterList<uint> mStructtrueSkill = new CSBetterList<uint>();//如果有技能要下载，也需要等待技能加载完才回调

    public void UpdateModel(CSAction action, int bodyId, Action<ModelLoadBase> onFinishCallBack)
    {
        UpdateModel(action, bodyId, 0, 0, onFinishCallBack);
    }

    public void UpdateModel(CSAction action, int bodyId, int weaponId, Action<ModelLoadBase> onFinishCallBack)
    {
        UpdateModel(action, bodyId, weaponId, 0, onFinishCallBack);
    }

    public void UpdateModel(CSAction action, int bodyId, int weaponId, int wingId, Action<ModelLoadBase> onFinishCallBack)
    {
        this.Action = action;
        this.BodyId = bodyId;
        this.WeaponId = weaponId;
        this.WingId = wingId;
        this.onExternalFinishCallBack = onFinishCallBack;
        Analyze();
    }

    public virtual void Analyze()
    {

    }

    public void Release()
    {
        mObjectList.Release();
        mObjectFinishStateList.Release();
        mStructtrueSkill.Release();
    }

    public void SplitKey(long key, ref int modelID, ref int motion, ref int direction)
    {
        modelID = (int)(key / 10000);
        motion = (int)(key % 10000 / 100);
        direction = (int)(key % 100);
    }

    public void BeginLoad()
    {
        PreBeginLoad();
        mObjectList.Clear();
        mObjectFinishStateList.Clear();
        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < mStructures.Length; i++)
        {
            mStructures[i] = 0;
        }
    }

    public virtual void PreBeginLoad()
    {

    }

    public void EndLoad(Action<ModelLoadBase> callBack)
    {
        onFinishAll = callBack;

        if (mStructtrueSkill.Count == 0 && mObjectList.Count == 0)//如果没有加载请求，那么直接调用完成回调，如果有请求，在BeginLoadRes中会调用
        {
            if (IsAllFinish())//如果FlagFinish成功，表示是最新的，如果由于动作切换太快，后面的动作比前面动作加载快，那么Loaded都会调用，但是onFinishAll之后后面的动作到了之后才会调用
            {
                if (onFinishAll != null)
                {
                    onFinishAll(this);
                }
            }
        }
        else
        {
            BeginLoadRes();
        }
    }

    public void Load(int modelID, int motion, int direction, int structure, int type, int assistType)
    {
        if (CSResourceManager.Singleton == null) return;

        long key = CSMisc.GetKey(modelID, motion, direction);

        if (mLoadedDic.ContainsKey(key))
        {
            ModelLoadData data = mLoadedDic[key];
            if (data != null && data.atlas != null && !data.atlas.HasBeenDestroy)
            {
                mStructures[structure] = key;
                string path = CSResourceManager.Singleton.GetKeyPath(key);
                CSResource res = CSResourceManager.Singleton.GetRes(path);
                if (res != null) BeginLoadModelResCallBack(res, data);
                return;
            }
        }
        else
        {
            ModelLoadData data = new ModelLoadData();
            data.type = type;
            data.assistType = assistType;
            data.structure = structure;
            mLoadedDic.Add(key, data);
        }
        mStructures[structure] = key;
        mObjectList.Add(key);
        mObjectFinishStateList.Add(false);
    }

    private void BeginLoadRes()
    {
        for (int i = 0; i < mObjectList.Count; i++)
        {
            long key = mObjectList[i];
            ModelLoadData data = mLoadedDic[key];
            string path = CSResourceManager.Singleton.GetKeyPath(key);
            if (!string.IsNullOrEmpty(path))
            {
                AddHasUseModelResList(path);
                CSResource res = CSResourceManager.Singleton.AddQueue(path, data.type, Loaded, data.assistType, true, key);
                BeginLoadModelResCallBack(res, data);
            }
            else
            {
                int modelID = 0, motion = 0, direction = 0;
                SplitKey(key, ref modelID, ref motion, ref direction);
                string Model = CSMisc.GetCombineModel(modelID, motion, direction);
                path = CSResource.GetPath(Model, data.type, false);
                AddHasUseModelResList(path);
                CSResourceManager.Singleton.AddKeyPath(key, path);
                CSResource res = CSResourceManager.Singleton.AddQueue(path, data.type, Loaded, data.assistType, true, key);
                BeginLoadModelResCallBack(res, data);
            }
        }
    }

    public virtual void DetectSetEmpty()
    {
        if (!IsAllFinish())//没有下载到之前 制空
        {
            if (onFinishAll != null)
            {
                isSetEmpty = true;
                onFinishAll(this);
                isSetEmpty = false;
            }
        }
    }

    public virtual void BeginLoadModelResCallBack(CSResource res, ModelLoadData data)
    {
    }

    protected virtual void LoadedOne(CSResource res)
    {

    }

    private void Loaded(CSResource res)
    {
        if (!mLoadedDic.ContainsKey(res.Key)) return;
        GameObject go = res.MirrorObj as GameObject;

        if (go != null)
        {
            mLoadedDic[res.Key].atlas = go.GetComponent<UIAtlas>();

            if (mLoadedDic[res.Key].atlas != null)
            {
                mLoadedDic[res.Key].atlas.ResPath = res.Path;
            }
            AtlasPoolItemDeal(mLoadedDic[res.Key].atlas);
        }

        if (FlagFinish(res.Key) && IsAllFinish())//如果FlagFinish成功，表示是最新的，如果由于动作切换太快，后面的动作比前面动作加载快，那么Loaded都会调用，但是onFinishAll之后后面的动作到了之后才会调用
        {
            if (onFinishAll != null)
            {
                onFinishAll(this);
            }
        }
        else
        {
            LoadedOne(res);
        }
    }

    public virtual void OnFinishAllCallBack(ModelLoadBase b)
    {
        if (onExternalFinishCallBack != null)
        {
            onExternalFinishCallBack(b);
        }
    }

    public UIAtlas GetAtlas(int structure)
    {
        long key = mStructures[structure];

        if (mLoadedDic.ContainsKey(key))
        {
            return mLoadedDic[key].atlas;
        }
        return null;
    }

    private bool FlagFinish(long key)
    {
        int index = mObjectList.IndexOf(key);

        if (index != -1)
        {
            mObjectFinishStateList[index] = true;
            return true;
        }
        return false;
    }

    private bool IsAllFinish()
    {
        isSetEmpty = false;
        for (int i = 0; i < mObjectFinishStateList.Count; i++)
        {
            if (!mObjectFinishStateList[i]) return false;
        }

        return true;
    }

    public bool IsCanLoad(int type, int modelID)
    {
        if (mAtlasNumDic.ContainsKey(type))
        {
            if (mAtlasNumDic[type].ContainsKey(modelID)) return true;

            int maxNum = 0;

            switch (type)
            {
                case ResourceType.PlayerAtlas:
                    maxNum = CSResourceManager.Singleton.maxPlayerAtlasNum;
                    break;
                case ResourceType.WeaponAtlas:
                    maxNum = CSResourceManager.Singleton.maxWeaponAtlasNum;
                    break;
                case ResourceType.MountAtlas:
                    maxNum = CSResourceManager.Singleton.maxMountAtlasNum;
                    break;
            }

            if (eAvatarType == EAvatarType.Player)
            {
                //TODO:ddn
                //CSPlayerInfo info = IAvatar.getBaseInfo as CSPlayerInfo;
                //if (info != null && !info.IsMaShow && !info.IsXingXingShow && mAtlasNumDic[type].Count >= maxNum) return false;
            }
            else
            {
                if (mAtlasNumDic[type].Count >= maxNum) return false;
            }

        }
        return true;
    }

    public void AddAtlasNum(int type, int modelID)
    {
        if (!mAtlasNumDic.ContainsKey(type))
        {
            mAtlasNumDic.Add(type, new Dictionary<int, int>());
        }
        if (!mAtlasNumDic[type].ContainsKey(modelID))
        {
            mAtlasNumDic[type].Add(modelID, 0);
        }
        mAtlasNumDic[type][modelID] = mAtlasNumDic[type][modelID] + 1;
    }

    public void RemoveAtlasNum(int type, int modelID)
    {
        if (mAtlasNumDic.ContainsKey(type) && mAtlasNumDic[type].ContainsKey(modelID))
        {
            mAtlasNumDic[type][modelID] = mAtlasNumDic[type][modelID] - 1;

            if (mAtlasNumDic[type][modelID] < 0) mAtlasNumDic[type][modelID] = 0;

            if (mAtlasNumDic[type][modelID] == 0)
            {
                mAtlasNumDic[type].Remove(modelID);
            }
        }
    }

    private void AtlasPoolItemDeal(UIAtlas atlas)
    {
        if (CSObjectPoolMgr.Instance != null && atlas != null)
        {
            CSObjectPoolItem poolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Resource(atlas.name, atlas.ResPath, null);
            CSObjectPoolMgr.Instance.RemovePoolItem(poolItem);
        }
    }

    public static void Clear()
    {
        mLoadedDic.Clear();
        mAtlasNumDic.Clear();
    }

    public virtual void Destroy()
    {
        onFinishAll = null;
        onExternalFinishCallBack = null;
        ClearWaitModelRes();
    }

    private void AddHasUseModelResList(string path)
    {
        if (!mHasUseModelResList.Contains(path))
            mHasUseModelResList.Add(path);
    }

    private void ClearWaitModelRes()
    {
        for (int i = 0; i < mHasUseModelResList.Count; i++)
        {
            if (CSResourceManager.Singleton == null) continue;
            CSResource res = CSResourceManager.Singleton.GetRes(mHasUseModelResList[i]);
            if (res != null)
            {
                res.onLoaded -= Loaded;
                CSResourceManager.Singleton.RemoveWaitingQueueDic(res.Path);
            }
        }
        mHasUseModelResList.Clear();
    }
}
