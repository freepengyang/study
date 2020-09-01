//-------------------------------------------
//资源
//author jiabao
//time 2015.12.28
//-------------------------------------------

using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using ILRuntime.CLR.TypeSystem;
using System.Reflection;
using ILRuntime.CLR.Method;
using Main_Project.Script.Update;
using TABLE;



/// <summary>
/// 支持回调参数是Object对象,清除所有回调  
/// </summary>
public class CSEventDelegate<T>
{
    BetterList<Action<T>> onLoadedList = new BetterList<Action<T>>(4);

    void AddCallBack(Action<T> onLoaded)
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);
        onLoadedList.Insert(0, onLoaded); //保证调用的先后性,插到最前面,调用是从最后一个往前调用的
    }

    public void AddFrontCallBack(Action<T> onLoaded)
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);
        onLoadedList.Add(onLoaded);
    }

    void RemoveCallBack(Action<T> onloaded)
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);
        onLoadedList.Remove(onloaded);
    }

    public void CallBack(T obj)
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);

        for (int i = onLoadedList.Count - 1; i >= 0; i--) //防止回调里面做了-=的操作
        {
            if (onLoadedList.Count > i && onLoadedList[i] != null)
                onLoadedList[i](obj);
        }
    }

    public void Clear()
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);
        onLoadedList.Clear();
    }

    public void Release()
    {
        if (onLoadedList == null)
            onLoadedList = new BetterList<Action<T>>(4);
        onLoadedList.Release();
    }

    public static CSEventDelegate<T> operator +(CSEventDelegate<T> dele, Action<T> onload)
    {
        dele.AddCallBack(onload);
        return dele;
    }

    public static CSEventDelegate<T> operator -(CSEventDelegate<T> dele, Action<T> onload)
    {
        dele.RemoveCallBack(onload);
        return dele;
    }
}

public class CSResource
{
    public UnityEngine.Object MirrorObj;
    public byte[] MirroyBytes;

    private static string applicationDataPath = string.Empty;
    private static string assetbunldeStr = ".u3d";
    protected int mResourceAssistType = ResourceAssistType.None;
    public int waitingCallBackCount = 0;
    public bool isHotLoading = false;

    public bool isResValid
    {
        get
        {
            if (MirrorObj != null || MirroyBytes != null) return true;
            return false;
        }
    } //下载失败的资源切换场景后需要删除,只针对已经在下载完成的列表

    public int AssistType
    {
        get { return mResourceAssistType; }
        set { mResourceAssistType = value; }
    }

    public float loadedTime = 0;

    public int mCount { get; set; }

    private bool mIsCanBeDelete = true;

    public bool IsCanBeDelete
    {
        get { return mIsCanBeDelete; }
        set
        {
            if (mIsCanBeDelete == value) return;
            mIsCanBeDelete = value;
        }
    }


    protected string fileName;

    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }

    /// <summary>
    /// 辅助Key，主要在模型加载里面用
    /// </summary>
    private long mKey = 0;

    public long Key
    {
        get { return mKey; }
        set { mKey = value; }
    }

    public string GetRelatePath()
    {
        string relatePath = mPath.Replace(AppUrlMain.mClientResURL, "");
        relatePath = relatePath.Substring(relatePath.IndexOf('/') + 1);
        return relatePath;
    }

    public static string GetPath(string fileName, int LocalType, bool isLocal)
    {
        string path = "";

        if (isLocal)
        {
            CSStringBuilder.Clear();
            CSStringBuilder.Append(GetModelTypePath(LocalType), fileName);
            path = CSStringBuilder.ToString(); //Resource.Load(存的是相对路径)
        }
        else
        {
            CSStringBuilder.Clear();

            bool isAssetBundle = false;
            switch (LocalType)
            {
                case ResourceType.Map:
                case ResourceType.MiniMap:
                case ResourceType.PlayerAtlas:
                case ResourceType.MonsterAtlas:
                case ResourceType.NpcAtlas:
                case ResourceType.WeaponAtlas:
                case ResourceType.MountAtlas:
                case ResourceType.Effect:
                case ResourceType.Skill:
                case ResourceType.SkillAtlas:
                case ResourceType.Audio:
                case ResourceType.WingAtlas:
                case ResourceType.UIMount:
                case ResourceType.ScaleMap:
                case ResourceType.UIEffect:
                case ResourceType.UIWing:
                case ResourceType.UIPlayer:
                case ResourceType.UIWeapon:
                case ResourceType.ResourceRes:
                case ResourceType.UITexture:
                case ResourceType.UIMonster:
                {
                    isAssetBundle = true;
                }
                    break;
            }

            if (!CSMisc.isUnityEditor)
            {
                string typeName = isAssetBundle ? assetbunldeStr : "";
                string relateName = $"{GetModelTypePath(LocalType)}{fileName}{typeName}";
                string resType = CSCheckResourceManager.Instance.GetResourcePathType(relateName);

                if (string.IsNullOrEmpty(applicationDataPath)) applicationDataPath = Application.persistentDataPath;
                path = CSStringBuilder.Append(applicationDataPath, "/", resType, "/", relateName).ToString();
            }
            else
            {
                if (SFOut.IsLoadLocalRes)
                {
                    if (string.IsNullOrEmpty(applicationDataPath)) applicationDataPath = Application.dataPath;
                    path = CSResourceManager.GetIsLoadLocalResPath(applicationDataPath, LocalType, fileName,
                        isAssetBundle, assetbunldeStr);
                }
                else
                {
                    string typeName = isAssetBundle ? assetbunldeStr : "";
                    string relateName = $"{GetModelTypePath(LocalType)}{fileName}{typeName}";
                    string resType = CSCheckResourceManager.Instance.GetResourcePathType(relateName);
                    if (string.IsNullOrEmpty(applicationDataPath)) applicationDataPath = Application.persistentDataPath;
                    path = CSStringBuilder.Append(applicationDataPath, "/", resType, "/", relateName).ToString();
                }
            }
        }

        return path;
    }

    protected string mPath;

    public string Path
    {
        get { return mPath; }
        set { mPath = value; }
    }

    public bool IsDone = false;

    private int mLocalType;

    public int LocalType
    {
        get { return mLocalType; }
        set { mLocalType = value; }
    }

    public CSEventDelegate<CSResource> onLoaded = new CSEventDelegate<CSResource>();

    public void ReleaseCallBack()
    {
        onLoaded.Release();
    }

    public CSResource(string name, string path, int type)
    {
        fileName = name;
        LocalType = type;
    }

    public static string GetModelTypePath(int type)
    {
        switch (type)
        {
            case ResourceType.PlayerAtlas: return "Model/Player/";
            case ResourceType.MonsterAtlas: return "Model/Monster/";
            case ResourceType.NpcAtlas: return "Model/NPC/";
            case ResourceType.MountAtlas: return "Model/Mount/";
            case ResourceType.WeaponAtlas: return "Model/Weapon/";
            case ResourceType.Effect: return "Model/Effect/";
            case ResourceType.SkillAtlas: return "Model/Skill/";
            case ResourceType.TableBytes: return "Table/";
            case ResourceType.Map: return "Map/";
            case ResourceType.MapBytes: return "Map/";
            case ResourceType.MiniMap: return "MiniMap/";
            case ResourceType.WingAtlas: return "Model/Wing/";
            case ResourceType.Audio: return "Audio/";
            case ResourceType.UIMount: return "Model/UIMount/";
            case ResourceType.ScaleMap: return "ScaleMap/";
            case ResourceType.UIEffect: return "Model/UIEffect/";
            case ResourceType.UIWing: return "Model/UIWing/";
            case ResourceType.UIPlayer: return "Model/UIPlayer/";
            case ResourceType.UIWeapon: return "Model/UIWeapon/";
            case ResourceType.ResourceRes: return "ResourceRes/";
            case ResourceType.UITexture: return "Model/UITexture/";
            case ResourceType.UIMonster: return "Model/UIMonster/";
        }

        return "";
    }

    public virtual void Load()
    {
    }

    public UnityEngine.Object GetObjInst()
    {
        if (MirrorObj == null) return null;

        if (null != MirrorObj as Texture) return MirrorObj;

        return UnityEngine.Object.Instantiate(MirrorObj);
    }

    public virtual void UpdateLoading()
    {
    }

    public virtual void ResHotUpdateCallBack_HttpLoad()
    {
    }
}