using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSDynamicChangeSetting : MonoBehaviour 
{
    private static CSDynamicChangeSetting mInstance;
    public static CSDynamicChangeSetting Instance
    {
        get { return mInstance; }
        set { mInstance = value; }
    }
    private float mNextCheckLeftMemoryTime = 0;

    private float mModelNumApseat = 1;
    public float ModelNumApseat
    {
        get { return mModelNumApseat; }
        set { mModelNumApseat = value; }
    }


    //private float mPoolReleaseApseat = 1;
    private float mPoolRealseApseat = 1;
    public float PoolRealseApseat
    {
        get { return mPoolRealseApseat; }
        set { mPoolRealseApseat = value; }
    }

    public bool mIsLowMemory = false;
    public bool IsLowMemory
    {
        get { return mIsLowMemory; }
        set { mIsLowMemory = value; }
    }

    private bool mIsHasNoMomory = false;
    public bool IsHasNoMomory
    {
        get { return mIsHasNoMomory; }
        set { mIsHasNoMomory = value; }
    }

    private float mFPSAverage= 0;//5秒内 帧数与45帧的比值（超过45的，算45）
    //private float mNextDeltectPlayerNum = 0; 
    private int mDyPlayerNum = 0;
    private Queue<float> fpsList = new Queue<float>();
    private float mFPSTotal;
    private float mFPS;
    private float mMinFps;
    private float mMaxFps;
    private float mLeftMemory = 0;//去除内存边界后剩余的内存
    public int DetectFPS = 30;
    public float FPS
    {
        get { return mFPS; }
        set
        {
            mFPS = value;
            if (fpsList.Count < 10)//用优化队列CSBetterQueue来组织先进先出。fps每0.5a秒更新一次，5秒内帧率比例
            {
                mFPSTotal += value;
                fpsList.Enqueue(value);
            }
            if (fpsList.Count != 10) return;
            mFPSAverage = mFPSTotal / fpsList.Count;
            mFPSTotal = 0;
            fpsList.Clear();
        }
    }

    public static bool IsApplyLimit
    {
        get
        {
            return true;
        }
    }

    void Awake()
    {
        mInstance = this;
    }

    void Start()
    {
        ExtendTableLoader.Instance.onLoadAllTable -= OnAllTableLoad;
        ExtendTableLoader.Instance.onLoadAllTable += OnAllTableLoad;
    }

    void OnAllTableLoad()
    {
        ExtendTableLoader.Instance.onLoadAllTable -= OnAllTableLoad;
        if (CSResourceManager.Singleton != null) CSResourceManager.Singleton.InitMaxAtlas();
        StartDetectMemory();
    }

	void Update()
    {
        UpdateDetectLowMemory();
    }

    void UpdateDetectLowMemory()
    {
#if UNITY_EDITOR || UNITY_ANDROID
        if (mNextCheckLeftMemoryTime == 0 || Time.time > mNextCheckLeftMemoryTime)
        {
            mNextCheckLeftMemoryTime = Time.time + 3;

            mIsLowMemory = false;

            mIsHasNoMomory = false;

            long t = 0;
            if (!SFOut.IsLoadLocalRes)
            {
                t = QuDaoInterface.Instance.getMemoryLimitResidue();
            }
            mt = (long)(t / 1024f);
            mLeftMemory = mt;
            if (mt != 0 && mt < 10)
            {
                mIsHasNoMomory = false;
            }

            if (mt != 0 && mt < 50)
            {
                mIsLowMemory = true;
            }
            UpdatePlayerNum();
        }
#endif
    }
    long mt = 0;
    void StartDetectMemory()
    {
#if UNITY_EDITOR||UNITY_ANDROID

        long t = 0;
        if (!SFOut.IsLoadLocalRes)
        {
            t = QuDaoInterface.Instance.getMemoryLimitResidue();
        }
        long mt = (long)(t / 1024f);
        mLeftMemory = mt;
        mIsLowMemory = false;

        mIsHasNoMomory = false;
        if (mt != 0 && mt < 10)
        {
            mIsHasNoMomory = true;
        }

        if (mt == 0)
        {
            mModelNumApseat = 1;
        }
        else if (mt < 50)
        {
            mModelNumApseat = 0.1f;
            mIsLowMemory = true;
        }
        else if (mt < 100)
        {
            mModelNumApseat = 0.15f;
        }
        else if (mt < 500)
        {
            mModelNumApseat = mt / 500f;
        }
        else
        {
            mModelNumApseat = 1;
        }

        if (mt == 0)
        {
            mPoolRealseApseat = 1;
        }
        else if (mt < 50)
        {
            mPoolRealseApseat = 0.03f;
        }
        else if (mt < 100)
        {
            mPoolRealseApseat = 0.1f;
        }
        else if (mt < 500)
        {
            mPoolRealseApseat = mt / 500f;
        }
        else
        {
            mPoolRealseApseat = 1f;
        }
        UpdatePlayerNum(false);
#endif
    }

    void UpdatePlayerNum(bool isUpdate = true)
    {
        if (mFPSAverage != 0)
        {
            if (mFPSAverage < DetectFPS)
            {
                if (CSScene.Sington != null)
                {
                    //TODO:ddn
                    //int limitNum = CSScene.Sington.CurPlayerLimitNum;
                    if (mFPSAverage != 0)
                    {
                        mDyPlayerNum = (int)(CSResourceManager.Singleton.InitMaxPlayerNum * mFPSAverage / DetectFPS);//30*10/20
                        if (mDyPlayerNum < 5) mDyPlayerNum = 5;
                    }
                    else//如果没有模型都在30帧一下
                    {
                        mDyPlayerNum = 5;
                    }
                }
            }
            else
            {
                if (CSResourceManager.Singleton != null) mDyPlayerNum = CSResourceManager.Singleton.InitMaxPlayerNum;
            }
            if (isUpdate)
            {
                UpdateMaxAtlas();
            }
        }
        if (!isUpdate)
        {
            UpdateMaxAtlas();
        }
    }

#if false
    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 300),"");
        Color c = GUI.color;
        GUI.color = Color.green;
        GUILayout.Label("mFPS = " + mFPS);
        GUILayout.Label("mt = " + mt);
        GUILayout.Label("mFPSAverage = " + mFPSAverage);
        GUILayout.Label("mModelNumApseat = " + mModelNumApseat);
        GUILayout.Label("mDyPlayerNum = " + mDyPlayerNum);
        GUILayout.Label("mPoolRealseApseat = " + mPoolRealseApseat);
        GUILayout.Label("mLeftMemory = " + mLeftMemory);
        if (CSScene.Sington != null) GUILayout.Label("当前人数 = " + CSScene.Sington.CurPlayerLimitNum);
        if (CSResourceManager.Singleton != null)
        {
            GUILayout.Label("maxPlayerNum = " + CSResourceManager.Singleton.maxPlayerNum);
            GUILayout.Label("maxPlayerAtlasNum = " + CSResourceManager.Singleton.maxPlayerAtlasNum);
            GUILayout.Label("maxWeaponAtlasNum = " + CSResourceManager.Singleton.maxWeaponAtlasNum);
            GUILayout.Label("maxWingAtlasNum = " + CSResourceManager.Singleton.maxMountAtlasNum);
        }
        GUI.color = c;
    }

#endif
    /// <summary>
    /// 模型数量上限
    /// </summary>
    public void UpdateMaxAtlas()
    {
        if (CSResourceManager.Singleton == null) return;
        int maxPlayerAtlasNum = (int)(CSResourceManager.Singleton.InitMaxPlayerAtlasNum * mModelNumApseat);
        int maxWeaponAtlasNum = (int)(CSResourceManager.Singleton.InitMaxWeaponAtlasNum * mModelNumApseat);
        int maxMountAtlasNum = (int)(CSResourceManager.Singleton.InitMaxMountAtlasNum * mModelNumApseat);
        int maxPlayerNum = mDyPlayerNum != 0 ? mDyPlayerNum : (int)(CSResourceManager.Singleton.InitMaxPlayerNum);
        CSResourceManager.Singleton.UpdateMaxAtlas(maxPlayerAtlasNum, maxWeaponAtlasNum, maxMountAtlasNum, maxPlayerNum);
    }

    public bool IsLimitSkillNum()
    {
        if (mModelNumApseat == 1) return false;
        int i = Mathf.CeilToInt(10 * mModelNumApseat);
        return false;
    }
} 
