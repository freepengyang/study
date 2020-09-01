using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.IO;
using System.Collections.Generic;

public class CSPreLoadingBase : MonoBehaviour
{
    private static CSPreLoadingBase m_instance;

    public static CSPreLoadingBase Instance
    {
        get { return m_instance; }
    }

    protected bool mIsTerrainDataLoaded = false;

    public bool IsTerrainDataLoaded
    {
        get { return mIsTerrainDataLoaded; }
        set { mIsTerrainDataLoaded = value; }
    }

    private CSCameraShake mCameraShake;

    protected GameObject mWorld;

    public UnityEngine.GameObject World
    {
        get { return mWorld; }
        set { mWorld = value; }
    }

    protected static Transform mCahceWorldTrans;

    public static Transform CahceWorldTrans
    {
        get { return mCahceWorldTrans; }
    }

    protected Transform mScaleZeroWorld; //防止创捷的对象不到world顶层，导致地表显示异常

    public Transform ScaleZeroWorld
    {
        get
        {
            if (mScaleZeroWorld == null)
            {
                mScaleZeroWorld = new GameObject("ScaleZeroWorld").transform;
                mScaleZeroWorld.localScale = Vector3.zero;
            }

            return mScaleZeroWorld;
        }
    }

    private Transform mMonsterAnchor;

    public Transform MonsterAnchor
    {
        get
        {
            if (mMonsterAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mMonsterAnchor = new GameObject("MonsterAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mMonsterAnchor.gameObject);
                    return mMonsterAnchor;
                }

                return ScaleZeroWorld;
            }

            return mMonsterAnchor;
        }
    }

    private Transform mPlayerAnchor;

    public Transform PlayerAnchor
    {
        get
        {
            if (mPlayerAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mPlayerAnchor = new GameObject("PlayerAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mPlayerAnchor.gameObject);
                    return mPlayerAnchor;
                }

                return ScaleZeroWorld;
            }

            return mPlayerAnchor;
        }
    }

    private Transform mNpcAnchor;

    public Transform NpcAnchor
    {
        get
        {
            if (mNpcAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mNpcAnchor = new GameObject("NpcAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mNpcAnchor.gameObject);
                    return mNpcAnchor;
                }

                return ScaleZeroWorld;
            }

            return mNpcAnchor;
        }
    }

    private Transform mItemAnchor;

    public Transform ItemAnchor
    {
        get
        {
            if (mItemAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mItemAnchor = new GameObject("ItemAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mItemAnchor.gameObject);
                    return mItemAnchor;
                }

                return ScaleZeroWorld;
            }

            return mItemAnchor;
        }
    }

    private Transform mPetAnchor;

    public UnityEngine.Transform PetAnchor
    {
        get
        {
            if (mPetAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mPetAnchor = new GameObject("PetAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mPetAnchor.gameObject);
                    return mPetAnchor;
                }

                return ScaleZeroWorld;
            }

            return mPetAnchor;
        }
    }

    private Transform mGuardActor;

    public Transform GuardActor
    {
        get
        {
            if (mGuardActor == null)
            {
                if (mWorldRoot != null)
                {
                    mGuardActor = new GameObject("GuardActor").transform;
                    NGUITools.SetParent(mWorldRoot, mGuardActor.gameObject);
                    return mGuardActor;
                }

                return ScaleZeroWorld;
            }

            return mGuardActor;
        }
    }

    private Transform mSkillAnchor;

    public Transform SkillAnchor
    {
        get
        {
            if (mSkillAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mSkillAnchor = new GameObject("SkillAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mSkillAnchor.gameObject);
                    return mSkillAnchor;
                }

                return ScaleZeroWorld;
            }

            return mSkillAnchor;
        }
    }

    private Transform mOtherAnchor;

    public Transform OtherAnchor
    {
        get
        {
            if (mOtherAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mOtherAnchor = new GameObject("OtherAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mOtherAnchor.gameObject);
                    return mOtherAnchor;
                }

                return ScaleZeroWorld;
            }

            return mOtherAnchor;
        }
    }

    protected Transform mSkillPoolAnchor;

    public Transform SkillPoolAnchor
    {
        get
        {
            if (mSkillPoolAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mSkillPoolAnchor = new GameObject("SkillPoolAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mSkillPoolAnchor.gameObject);
                    return mSkillPoolAnchor;
                }

                return ScaleZeroWorld;
            }

            return mSkillPoolAnchor;
        }
    }

    protected Transform mTerrainAnchor;

    public Transform TerrainAnchor
    {
        get { return mTerrainAnchor; }
        set { mTerrainAnchor = value; }
    }

    protected Transform mEffectAnchor;

    public Transform EffectAnchor
    {
        get
        {
            if (mEffectAnchor == null)
            {
                if (mWorldRoot != null)
                {
                    mEffectAnchor = new GameObject("EffectAnchor").transform;
                    NGUITools.SetParent(mWorldRoot, mEffectAnchor.gameObject);
                    return mEffectAnchor;
                }

                return ScaleZeroWorld;
            }

            return mEffectAnchor;
        }
    }

    protected Camera mCamera;

    public UnityEngine.Camera Camera
    {
        get { return mCamera; }
        set { mCamera = value; }
    }

    protected Transform mWorldRoot;

    public Transform WorldRoot
    {
        get { return mWorldRoot; }
        set { mWorldRoot = value; }
    }

    public CSWaitFrameDeal WaitFrameDeal_Object = null;

    static int mSceneMapID = 0; //当前地图所在map id和CSScene里面的getMapID都是mapinfo表格id，只是概率上不一样

    public static int SceneMapID
    {
        get { return mSceneMapID; }
        set { mSceneMapID = value; }
    }

    public static Vector2 Size = Vector2.zero;

    public static GameObject go;

    public void Clear()
    {
        mWorld = null;
        mMonsterAnchor = null;
        mPlayerAnchor = null;
        mSkillPoolAnchor = null;
        mNpcAnchor = null;
        mItemAnchor = null;
        mPetAnchor = null;
        mGuardActor = null;
        mSkillAnchor = null;
        mOtherAnchor = null;
        mTerrainAnchor = null;
        mEffectAnchor = null;
        //mTerrain = null;
        //mMesh = null;
        mWorldRoot = null;
    }

    // 创建地图
    public void MakeTerrainData()
    {
        CSTerrain.Instance.Build(mTerrainAnchor, CSMainParameterManager.tableMapInfo.mapSize);
    }

    // 创建网格
    public void MakeCellData()
    {
        IsTerrainDataLoaded = false;
        CSConstant.mapImg = CSMainParameterManager.tableMapInfo.img;
        string str = string.Format("C_{0}.byte", CSMainParameterManager.tableMapInfo.id);
        CSResourceManager.Singleton.AddQueue(str, ResourceType.MapBytes, loadTerrainData, ResourceAssistType.ForceLoad);
    }

    CSResourceWWW reswww = null;

    public void loadTerrainData(CSResource res)
    {
        res.IsCanBeDelete = true;
        CreateWorld();
        CreateTerrain();
        MakeTerrainData();
        Vector3 local = mTerrainAnchor.transform.localPosition;
        local.z = 10000;
        mTerrainAnchor.SetParent(CahceWorldTrans);
        mTerrainAnchor.localPosition = local;
        mTerrainAnchor.localScale = Vector3.one;
        LoadTerrain(res);
    }

    private void LoadTerrain(CSResource res)
    {
        try
        {
            ParseMapByte(res);
        }
        catch (System.Exception ex)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.Log("Parse map bytes error: " + ex.Message);

            if (res is CSResourceWWW)
            {
                StartCoroutine(DealNeedWaitHotUpdate(res));
            }

            return;
        }

        IsTerrainDataLoaded = true;
        if (CSConstant.IsLanuchMainPlayer)
        {
            //CSScene.MainPlayer.UpdatePosTerrainDataIsLoaded();
            CSTerrain.Instance.refreshDisplayMeshCoord(CSMainParameterManager.mainPlayerOldCell);
        }
    }

    private void OnloadTerrain(CSResource res)
    {
        res.onLoaded -= OnloadTerrain;
        LoadTerrain(res);
        try
        {
            ParseMapByte(res);
        }
        catch (System.Exception ex)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.Log("Parse map bytes error: " + ex.Message);
            if (res is CSResourceWWW)
            {
                StartCoroutine(DealNeedWaitHotUpdate(res));
            }

            return;
        }

        IsTerrainDataLoaded = true;
        if (CSConstant.IsLanuchMainPlayer)
        {
            //CSScene.MainPlayer.UpdatePosTerrainDataIsLoaded();
            CSTerrain.Instance.refreshDisplayMeshCoord(CSMainParameterManager.mainPlayerOldCell);
        }
    }

    private void ParseMapByte(CSResource res)
    {
        if (res != null && res.MirroyBytes != null)
        {
            byte[] data = res.MirroyBytes;
            MapEditor.MapInfoList info = null;
            using (MemoryStream stream = new MemoryStream(data))
            {
                info = MapEditor.MapInfoList.Parser.ParseFrom(data);
                data = null;
            }

            if (info != null)
            {
                CSMesh.Instance.ClientInfo = info;
                CSMesh.Instance.Init(CSTerrain.Instance.OldSize);
            }
        }
        else
        {
            CSMesh.Instance.Build();
        }
    }

    IEnumerator DealNeedWaitHotUpdate(CSResource res)
    {
        yield return null;
        reswww = res as CSResourceWWW;
        if (reswww != null)
        {
            reswww.DealNeedWaitHotUpdate();
            reswww.ReleaseCallBack();
            reswww.onLoaded += OnloadTerrain;
        }
    }

    private void CreateWorld()
    {
        if (this == null) return;

        if (mWorld == null)
        {
            mWorld = new GameObject("World");
            mWorldRoot = mWorld.transform;
            mCahceWorldTrans = mWorld.transform;
            mCahceWorldTrans.parent = transform;
            mWorld.transform.localScale = Vector3.one;
        }
    }

    private void CreateTerrain()
    {
        if (mTerrainAnchor == null)
        {
            mTerrainAnchor = new GameObject("Terrain").transform;
            mTerrainAnchor.parent = null;
            mTerrainAnchor.localScale = Vector3.one;
        }
    }

    public void CreateCamera()
    {
        if (mCamera != null) return;
        GameObject go = new GameObject("Camera");
        GameObject goParent = new GameObject("CameraParent");
        goParent.transform.parent = transform;
        go.transform.parent = goParent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        mCamera = go.AddComponent<Camera>();
        mCamera.tag = "MainCamera";
        mCamera.cullingMask = 1 << 0;
        mCamera.orthographic = true;
        mCamera.orthographicSize = 1;
        mCamera.nearClipPlane = -1000;
        mCamera.farClipPlane = 1000;
        mCamera.backgroundColor = Color.black;
        mCamera.allowHDR = false;
        mCamera.allowMSAA = false;
        //go.AddComponent<GUILayer>();
        //打开预制 场景模糊
        GaussianBlurEffect3 effect = go.AddComponent<GaussianBlurEffect3>();
        mCameraShake = goParent.AddComponent<CSCameraShake>();
    }

    public static void CreatePreLoading()
    {
        //if (CSScene.Sington != null) return;
        if (go == null) go = new GameObject("Scene");
        //GameObject go = new GameObject("Scene");
        //CSScene.Sington = go.AddComponent<CSScene>();
        m_instance = go.AddComponent<CSPreLoadingBase>();
        UIRoot root = go.AddComponent<UIRoot>();
        root.scalingStyle = UIRoot.Scaling.ConstrainedOnMobiles;
        root.manualWidth = (int) CSConstant.ContentSize.x;
        root.manualHeight = (int) CSConstant.ContentSize.y;
        root.fitWidth = true;
        root.fitHeight = true;
        DontDestroyOnLoad(go);
    }

    public void PreLoadingProc()
    {
        CreateCamera();
        MakeCellData();
    }

    public void DestroyWorld()
    {
        if (mWorldRoot != null)
        {
            GameObject.Destroy(mWorldRoot.gameObject);
            mWorldRoot = null;
        }
    }

    public void CreateSceneActhor()
    {
        mTerrainAnchor.parent = mWorldRoot;
        if (mMonsterAnchor == null)
        {
            mMonsterAnchor = new GameObject("MonsterAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mMonsterAnchor.gameObject);
        }

        if (mGuardActor == null)
        {
            mGuardActor = new GameObject("GuardActor").transform;
            NGUITools.SetParent(mWorldRoot, mGuardActor.gameObject);
        }

        if (mOtherAnchor == null)
        {
            mOtherAnchor = new GameObject("OtherAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mOtherAnchor.gameObject);
        }

        if (mSkillPoolAnchor == null)
        {
            mSkillPoolAnchor = new GameObject("SkillPoolAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mSkillPoolAnchor.gameObject);
        }

        if (mPlayerAnchor == null)
        {
            mPlayerAnchor = new GameObject("PlayerAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mPlayerAnchor.gameObject);
        }

        if (mNpcAnchor == null)
        {
            mNpcAnchor = new GameObject("NpcAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mNpcAnchor.gameObject);
        }

        if (mItemAnchor == null)
        {
            mItemAnchor = new GameObject("ItemAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mItemAnchor.gameObject);
        }

        if (mPetAnchor == null)
        {
            mPetAnchor = new GameObject("PetAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mPetAnchor.gameObject);
        }

        if (mSkillAnchor == null)
        {
            mSkillAnchor = new GameObject("SkillAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mSkillAnchor.gameObject);
        }

        if (mEffectAnchor == null)
        {
            mEffectAnchor = new GameObject("EffectAnchor").transform;
            NGUITools.SetParent(mWorldRoot, mEffectAnchor.gameObject);
        }
    }


    public void ShakeCamera(float shakeDelay, float shakeTime, float shakeAmplitude)
    {
        mCameraShake.SetShakeInfo(shakeDelay, shakeTime, shakeAmplitude);
    }


    public Transform GetAvatarAnchor(int avatarType)
    {
        switch (avatarType)
        {
            case EAvatarType.Player:
                return CSPreLoadingBase.Instance.PlayerAnchor;
            case EAvatarType.Monster:
                return CSPreLoadingBase.Instance.MonsterAnchor;
            case EAvatarType.NPC:
                return CSPreLoadingBase.Instance.NpcAnchor;
            case EAvatarType.Pet:
            case EAvatarType.ZhanHun:
                return CSPreLoadingBase.Instance.PetAnchor;
            default:
                return null;
        }
    }
}