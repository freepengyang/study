using System;
using UnityEngine;
using System.Collections;

public class MainPlayerModelLoad : ModelLoadBase,IModelLoad
{
    private bool isOnlyCallBackWhenLoaded = false;
    private Vector3 wPotion = Vector3.zero;

    //Main_Project
    public override void PreBeginLoad()
    {
        for (int i = 0; i < mObjectList.Count; i++)
        {
            long key = mObjectList[i];
            if (!mLoadedDic.ContainsKey(key)) continue;
            int modelID = 0, motion = 0, direction = 0;
            SplitKey(key, ref modelID, ref motion, ref direction);
            if (motion == CSMotion.Run) continue;//跑步动作的是否可删除在PreLoadForever和RestoreForverChange中
            if (motion == CSMotion.Stand) continue;
            ModelLoadData data = mLoadedDic[key];
            string path = CSResourceManager.Singleton.GetKeyPath(key);
            if (CSResourceManager.Singleton == null) return;
            if (!string.IsNullOrEmpty(path))
            {
                CSResource res = CSResourceManager.Singleton.GetRes(path);
                if (res != null) res.IsCanBeDelete = true;
            }
            else
            {
                string Model = CSMisc.GetCombineModel(modelID, motion, direction);
                path = CSResource.GetPath(Model, data.type, false);
                CSResource res = CSResourceManager.Singleton.GetRes(path);
                if (res != null) res.IsCanBeDelete = true;
            }
        }
    }

    public override void BeginLoadModelResCallBack(CSResource res,ModelLoadData data)
    {
        base.BeginLoadModelResCallBack(res,data);
        if (res == null) return;
        res.IsCanBeDelete = false;
    }

    public override void Analyze()
    {
        base.BeginLoad();

        curMotion = this.Action.Motion;
        curDirection = this.Action.Direction;

        LoadBody();

        LoadWeapon();

        LoadWing();

        base.EndLoad(OnFinishAllCallBack);
    }

    private void LoadBody()
    {
        if (this.BodyId == 0) return;
  
        // 模型装备
        base.Load(this.BodyId, curMotion, curDirection, ModelStructure.Body, ResourceType.PlayerAtlas, ResourceAssistType.Charactar);

        if (lastBodyModel != this.BodyId)
        {
            SetFiveDirectionForever(this.BodyId, lastBodyModel, ResourceType.PlayerAtlas);
        }
        lastBodyModel = this.BodyId;

        //avater.ShowModelshadow(true);
    }

    private void LoadWeapon()
    {
        if (curMotion == (uint)CSMotion.Dead)
        {
            return;
        }
      
        if (this.WeaponId == 0) return;

        base.Load(this.WeaponId, curMotion, curDirection, ModelStructure.Weapon, ResourceType.WeaponAtlas, ResourceAssistType.Charactar);

        if (lastWeapon != this.WeaponId)
        {
            SetFiveDirectionForever(this.WeaponId, lastWeapon, ResourceType.WeaponAtlas);
        }
        lastWeapon = this.WeaponId;
    }

    private void LoadWing()
    {
        if (curMotion == (uint)CSMotion.Dead)
        {
            return;
        }
        
        if(this.WingId <= 0) return;

        base.Load(this.WingId, curMotion, curDirection, ModelStructure.Wing, ResourceType.WingAtlas, ResourceAssistType.Charactar);
        if (lastWing != this.WingId)
        {
            SetFiveDirectionForever(this.WingId, lastWing, ResourceType.WingAtlas);
        }
        lastWing = this.WingId;
    }

    private void SetFiveDirectionForever(int modelId, int lastModelId, int type)
    {
        RestoreFiveDirectionForverChange(lastModelId, CSMotion.Stand, type);
        PreLoadFiveDirectForever(modelId, CSMotion.Stand, type);
        RestoreFiveDirectionForverChange(lastModelId, CSMotion.Run, type);
        PreLoadFiveDirectForever(modelId, CSMotion.Run, type);
    }

    public override void DetectSetEmpty()
    {
        if (!isOnlyCallBackWhenLoaded)
        {
            base.DetectSetEmpty();
        }
    }

    protected override void LoadedOne(CSResource res)
    {
        base.OnFinishAllCallBack(this);
    }

    public override void OnFinishAllCallBack(ModelLoadBase b)
    {
        base.OnFinishAllCallBack(b);
    }

    /// <summary>
    /// 设置永久缓存+切换场景不删除下载的资源
    /// </summary>
    /// <param name="modelID"></param>
    /// <param name="motion"></param>
    /// <param name="resType"></param>
    void RestoreFiveDirectionForverChange(int modelID, int motion, int resType)
    {
        if (modelID == 0) return;
        if (CSObjectPoolMgr.Instance == null) return;
        for (int i = 0; i <= 4; i++)
        {
            CSStringBuilder.Clear();
            string resName = CSStringBuilder.Append(modelID.ToString(), "_", CSMisc.stringMotionDic[motion], "_", i.ToString()).ToString();
            string resPath = CSResource.GetPath(resName, resType, false);
            CSObjectPoolBase pool = CSObjectPoolMgr.Instance.GetPool(resPath);
            if (pool != null)
            {
                pool.MarkForeverCanChange(true);
                pool.isForever = false;
            }
            if (CSResourceManager.Singleton == null) return;
            CSResource res = CSResourceManager.Singleton.GetRes(resPath);
            if (res != null) res.IsCanBeDelete = true;
        }
    }

    public void PreLoadFiveDirectForever(int modelID, int motion, int resType)
    {
        for (int i = 0; i <= 4; i++)
        {
            CSStringBuilder.Clear();
            string resName = CSStringBuilder.Append(modelID.ToString(), "_", CSMisc.stringMotionDic[motion], "_", i.ToString()).ToString();
            CSResource res = CSResourceManager.Singleton.AddQueue(resName, resType, OnFiveDirectForeverLoaded, ResourceAssistType.None);
            res.IsCanBeDelete = false;
        }
    }

    void OnFiveDirectForeverLoaded(CSResource res)
    {
        if (CSObjectPoolMgr.Instance == null) return;
        GameObject go = res.MirrorObj as GameObject;
        if (go == null) return;
        UIAtlas atlas = go.GetComponent<UIAtlas>();
        if (atlas == null) return;
        atlas.ResPath = res.Path;
        CSObjectPoolItem poolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Resource(atlas.name, atlas.ResPath, null,true);
        CSObjectPoolBase pool = CSObjectPoolMgr.Instance.GetPool(atlas.ResPath);
        if(pool != null)pool.MarkForeverCanChange(false);
        CSObjectPoolMgr.Instance.RemovePoolItem(poolItem);
    }
}
