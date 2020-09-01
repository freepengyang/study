using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFindMap : ActionBase
{
    public class Factory : ActionFactory<ActionFindMap, ActionFindMapParam>
    {

    }

    public override int ID
    {
        get
        {
            return (int)EnumAction.FindMap;
        }
    }

    /// <summary>
    /// 0 init 1 running 2 done 3 waiting
    /// </summary>
    int status;
    /// <summary>
    /// 地图表数据
    /// </summary>
    TABLE.MAPINFO mapItem;

    /// <summary>
    /// 地图结点
    /// </summary>
    CSBetterList<CSAStarScene.Node_Scene> sceneNodes;

    ActionFindMapParam mapParam;

    public override void Init(IActionParam argv)
    {
        base.Init(argv);

        status = 0;
        mapItem = null;

        mapParam = argv as ActionFindMapParam;

        if (null == mapParam)
        {
            OnFailed();
            return;
        }

        if(!MapInfoTableManager.Instance.TryGetValue(mapParam.mapId, out mapItem))
        {
            OnFailed();
            return;
        }

        FNDebug.LogFormat("<color=#00ff00>[ActionFindMap]:[目标:{0}]</color>",mapItem.name);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.AddEvent(CEvent.CancelPathFind, OnCancelPathFind);
        Succeed = true;
    }

    void OnCancelPathFind(uint id, object argv)
    {
        FNDebug.LogFormat("<color=#00ff00>[ActionFindMap]:[寻路被中断]</color>");
        if (mapParam.mapId == CSScene.GetMapID())
        {
            OnSucceed();
        }
        else
        {
            OnFailed();
        }
    }

    void OnMainPlayerStopMoving(uint id,object argv)
    {
        if (mapParam.mapId == CSScene.GetMapID())
        {
            OnSucceed();
        }
        else
        {
            status = 0;
            FNDebug.LogFormat("<color=#00ff00>[ActionFindMap]:[切换到状态 0]</color>");
        }
    }

    public override bool IsDone()
    {
        if(status == 2)
        {
            return true;
        }

        if (mapParam.mapId == CSScene.GetMapID())
        {
            OnSucceed();
            return false;
        }

        if (status == 0)
        {
            sceneNodes = CSAStarScene.FindPath(CSScene.GetMapID(), mapParam.mapId);
            status = 1;
        }

        if(status == 1)
        {
            if (sceneNodes == null || sceneNodes.Count <= 0)
            {
                FNDebug.LogErrorFormat("[寻路异常]:[MAPID = {0}]无法找到路径点", mapParam.mapId);
                OnFailed();
                return false;
            }

            TABLE.EVENT tbl_event = sceneNodes[0].EventTblToNext;
            sceneNodes.RemoveAt(0);

            if (tbl_event == null)
            {
                FNDebug.LogError("[寻路异常]:事件表[EventTable]配置错误");
                OnFailed();
                return false;
            }

            var mainPlayer =CSAvatarManager.MainPlayer;
            if (mainPlayer == null)
            {
                FNDebug.LogError("[寻路异常]:主角不存在");
                OnFailed();
                return false;
            }

            CSMisc.Dot2 dot2;
            dot2.x = tbl_event.x;
            dot2.y = tbl_event.y;

            if(mainPlayer.IsBeControl)
            {
                FNDebug.LogError("[寻路异常]:被手动中断");
                OnFailed();
                return false;
            }
            mainPlayer.TowardsTarget(dot2);

            status = 3;
        }

        //if (status == 3)
        //{
        //    var mainPlayer =CSAvatarManager.MainPlayer;
        //    if (mainPlayer == null)
        //    {
        //        Debug.LogError("[寻路异常]:主角不存在");
        //        OnFailed();
        //        return false;
        //    }

        //    //if (mainPlayer.Model.GetMotion() != CSMotion.Run)
        //    //{
        //    //    OnFailed();
        //    //    Debug.LogFormat("<color=#00ff00>[ActionFindMap]:[停止寻路]</color>");
        //    //    return false;
        //    //}
        //}

        return false;
    }

    void OnSucceed()
    {
        FNDebug.LogFormat("<color=#00ff00>[ActionFindMap]:{0} Succeed</color>", mapParam.mapId);
        Succeed = true;
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        status = 2;
    }

    void OnFailed()
    {
        FNDebug.LogFormat("<color=#ff0000>[ActionFindMap]:{0} Failed</color>", mapParam.mapId);
        Succeed = false;
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        status = 2;
    }

    public override void OnRecycle()
    {
        FNDebug.LogFormat("<color=#00ff00>[ActionFindMap]:[action has been recycled]</color>");
        Succeed = false;
        status = 2;
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        mapItem = null;
        mapParam = null;
        base.OnRecycle();
    }
}