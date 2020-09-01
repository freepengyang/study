using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAvatarDirector : Singleton<CSAvatarDirector>
{
    private static Dictionary<int, float> mCreateIntervalDic = new Dictionary<int, float>()
    {
        { EAvatarType.Player, 0.07f},
        { EAvatarType.Monster, 0.05f },
        { EAvatarType.Pet, 0.05f },
        { EAvatarType.NPC, 0.01f },
    };

    private readonly float mRemoveIntervalTime = 0.01f;

    ///// <summary>
    ///// <avatar's parent(anchor),CSWaitFrameDeal>
    ///// </summary>
    //public Dictionary<GameObject, CSWaitFrameDeal> AnchorToWaitDealDic = null;
    //public CSAvatarDirector()
    //{
    //    AnchorToWaitDealDic = new Dictionary<GameObject, CSWaitFrameDeal>();
    //}

    public void CreateMainPlayer(Transform rootScene)
    {
        CSCharacter mainPlayer = null;

        if (CSAvatarManager.MainPlayer == null)
        {
            CSAvatarManager.MainPlayer = new CSCharacter();
        }
        mainPlayer = (CSAvatarManager.MainPlayer) as CSCharacter;
        mainPlayer.Init(CSMainPlayerInfo.Instance, rootScene);
        CSCameraManager.Instance.InitMainCamera(mainPlayer.CacheTransform);
        CSResourceManager.Singleton.BeginQueueDeal();
        mainPlayer.ReplaceEquip();
        CSResourceManager.Singleton.EndQueueDeal();
        mainPlayer.IsLoad = true;
    }

    //TODO 为了改造MainPlayerModelLoad 暂时注释掉
    //public void OnLoadedMainPlayer(ModelLoadBase p)
    //{
    //    MainPlayerModelLoad load = p as MainPlayerModelLoad;
    //    load.avater.IsLoad = true;
    //    load.avater.SetModelAtlas(p);
    //    CSConstant.IsLanuchMainPlayer = true;
    //    CSTerrain.Instance.refreshDisplayMeshCoord(load.avater.NewCell);
    //    //TODO:ddn for tranform effect
    //    HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_UpdateRoleMove);
    //    HotManager.Instance.EventHandler.SendEvent(CEvent.MainPlayer_OnLoaded);


    //}

    public void CreateAvatar<T>(long id, object data, Transform transAnchor) where T : CSAvatar
    {
        if (CSAvatarManager.Instance.IsMainPlayer(id))
        {
            return;
        }
        CSAvatarManager.Instance.CheckRemoveAvater(id);
        CSAvatar avatar = AddPoolItem<T>();
        if (avatar == null)
        {
            if (FNDebug.developerConsoleVisible)
            {
                FNDebug.Log(typeof(T).ToString() + "is null");
            }
            return;
        }
        avatar.Init(data, transAnchor);
        CSAvatarManager.Instance.AddAvatar(avatar);
        AddWaitDeal(transAnchor.gameObject, avatar, OnCreateAvatar);
    }


    public void ShowAvatar(Transform transAnchor, CSAvatar avater)
    {
        if(transAnchor != null)
        {
            AddWaitDeal(transAnchor.gameObject, avater, OnCreateAvatar, mRemoveIntervalTime,true);
        }
    }

    public void RemoveAvatar(Transform transAnchor, CSAvatar avater)
    {
        if(transAnchor != null)
        {
            OnRemoveAvatar(avater,null);
            //AddWaitDeal(transAnchor.gameObject, avater, OnRemoveAvatar, mRemoveIntervalTime);
        }
    }

    /// <summary>
    /// 创建Avatar
    /// </summary>
    /// <param name="obj">avatar</param>
    /// <param name="param">回调参数</param>
    /// <returns></returns>
    public bool OnCreateAvatar(object obj, object param)
    {
        if (obj == null) return false;
        CSAvatar avatar = obj as CSAvatar;
        if (avatar == null) return false;
        if (CSAvatarManager.Instance.IsCheckRemoveAvater(avatar))
        {
            return false;
        }
        avatar.InitAvatarGo();
        if(param != null)
        {
            avatar.ReplaceEquip();
        }
        return true;
    }

    public bool OnRemoveAvatar(CSAvatar avatar, object param)
    {
        if (avatar == null)
        {
            return false;
        }
        avatar.ShowAvatarGo();
        return false;
    }

    /// <summary>
    /// 添加等待加载的Avatar
    /// </summary>
    /// <param name="goAnchor">avatar 对应的父节点</param>
    /// <param name="avater"></param>
    /// <param name="onload">加载完成回调</param>
    /// <param name="waitFrame">等待加载的时间</param>
    /// <param name="param">加载完成回调预留参数</param>
    /// <param name="isInsert">true:往等待加载队列前插</param>
    public static void AddWaitDeal(GameObject goAnchor, CSAvatar a, Func<object, object, bool> onload, float waitFrame = 0, object param = null, bool isInsert = false)
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        if (goAnchor == null) return;
        //CSAvatar a = avater as CSAvatar;
        if (a != null && a.BaseInfo == null) return;

        if(waitFrame < 0.0001f)
        {
            if (mCreateIntervalDic.ContainsKey(a.AvatarType))
            {
                waitFrame = mCreateIntervalDic[a.AvatarType];
            }
        }

        if (!CSWaitFrameDealManager.Instance.AnchorToWaitDealDic.ContainsKey(goAnchor) || CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor] == null)
        {
            CSWaitFrameDeal deal = goAnchor.AddComponent<CSWaitFrameDeal>();
            deal.isNeedReset = true;
            CSWaitFrameDealManager.Instance.AnchorToWaitDealDic.Add(goAnchor, deal);
            if (isInsert)
            {
                deal.InsertFront(a != null ? a.BaseInfo.ID : 0);
            }
            else
            {
                deal.Add(a, onload, waitFrame, param, a != null ? a.BaseInfo.ID : 0);
            }
        }
        else
        {
            if (isInsert)
            {
                CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor].InsertFront(a != null ? a.BaseInfo.ID : 0);
            }
            else
            {
                CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor].Add(a, onload, waitFrame, param, a != null ? a.BaseInfo.ID : 0);
            }
        }
    }

    private T AddPoolItem<T>() where T: CSAvatar
    {
        if (CSObjectPoolMgr.Instance == null)
        {
            return null;
        }
        Type type = typeof(T);
        string strType = type.ToString();
        CSObjectPoolItem poolItem = Utility.GetAndAddPoolItem_Class(strType, strType, null, type, null);
        CSAvatar avatar = poolItem.objParam as T;
        if(avatar != null)
        {
            avatar.PoolItem = poolItem;
        }
        return (poolItem.objParam as T);
    }

    public void Destroy()
    {
        //if (AnchorToWaitDealDic != null)
        //{
        //    var dic = AnchorToWaitDealDic.GetEnumerator();
        //    while (dic.MoveNext())
        //    {
        //        CSWaitFrameDeal a = dic.Current.Value;
        //        a.Release();
        //    }
        //    AnchorToWaitDealDic.Clear();
        //}
    }

}
