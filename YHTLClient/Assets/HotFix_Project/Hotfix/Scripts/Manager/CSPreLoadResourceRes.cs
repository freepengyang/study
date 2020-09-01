using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CSPreLoadResourceRes
{
    public enum EType
    {
        None,
        CSGameStaticObj,
        CSGameStaticAtlas,
        FirstSceneUIObj,
        UITexture,
        HeadPool,
        ShaderTxt,//包含哪些shader需要热更
        Shader,
    }
    public class Data
    {
        public EType type;
        public UnityEngine.Object obj;
    }
    private static Dictionary<string, Data> mDic = new Dictionary<string, Data>();
    private static int mCurNum = 0;
    public static int CurNum
    {
        get { return mCurNum; }
        set { mCurNum = value; }
    }
    private static int mMaxNum = 0;
    public static int MaxNum
    {
        get { return mMaxNum; }
        set { mMaxNum = value; }
    }
    private static bool mIsInit = false;

    public static Data GetData(string str)
    {
        if (mDic.ContainsKey(str)) return mDic[str];
        return null;
    }

    public static void PreLoad()
    {
        if (mIsInit) return;
        mIsInit = true;
        mCurNum = 0;
        mMaxNum = 0;
        PreLoad("fightnums", EType.CSGameStaticAtlas);
        PreLoad("Scelect", EType.CSGameStaticAtlas);
        PreLoad("effetc_npc_select", EType.CSGameStaticAtlas);
        PreLoad("sceneitem", EType.CSGameStaticObj);
        PreLoad("actor_title", EType.CSGameStaticAtlas);
    }

    static void PreLoad(string resName, EType type)
    {
        if (mDic.ContainsKey(resName)) return;
        Data data = new Data();
        data.type = type;
        mDic.Add(resName, data);
        mMaxNum++;
        CSResource res = CSResourceManager.Singleton.AddQueue(resName, ResourceType.ResourceRes, OnLoaded, ResourceAssistType.ForceLoad);
        res.FileName = resName;
        res.IsCanBeDelete = false;
    }

    static void OnLoaded(CSResource res)
    {
        if (!mDic.ContainsKey(res.FileName)) return;
        Data data = mDic[res.FileName];
        mCurNum++;
        ExtendTableLoader.Instance.ChecLoadedkProgress();
        data.obj = res.MirrorObj;
        EType type = data.type;

        switch (type)
        {
            case EType.CSGameStaticObj:
                CSGameManager.Instance.AddDicData(res.FileName, res.MirrorObj);
                break;
            case EType.CSGameStaticAtlas:
                CSGameManager.Instance.AddDicDataAtlas(res.FileName, res.MirrorObj);
                break;
        }
     }
}
   

