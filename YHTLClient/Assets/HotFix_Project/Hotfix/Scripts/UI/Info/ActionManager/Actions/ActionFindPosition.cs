using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFindPosition : ActionBase
{
    public class Factory : ActionFactory<ActionFindPosition,ActionFindPosParam>
    {

    }

    public override int ID
    {
        get
        {
            return (int)EnumAction.FindPos;
        }
    }

    /// <summary>
    /// 0 init 1 running 2 done
    /// </summary>
    int status;
    ActionFindPosParam posParam;

    public override void Init(IActionParam argv)
    {
        base.Init(argv);
        posParam = argv as ActionFindPosParam;
        status = 0;
        Succeed = false;
        HotManager.Instance.EventHandler.AddEvent(CEvent.CancelPathFind, OnCancelPathFind);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
    }

    void OnCancelPathFind(uint id,object argv)
    {
        FNDebug.LogFormat("<color=#00ff00>[ActionFindPosition]:寻路被中断</color>");
        OnFailed();
    }

    void OnMainPlayerStopMoving(uint id,object argv)
    {
        if(null == posParam)
        {
            OnFailed();
            return;
        }

        if(CSScene.GetMapID() != posParam.mapId)
        {
            OnFailed();
            FNDebug.LogFormat("<color=#00ff00>[寻路失败]:目标地图[mapId:{0}] => 当前地图[mapId:{1}]</color>", posParam.mapId, CSScene.GetMapID());
            return;
        }

        if(!Utility.IsNearPlayerInMap(posParam.x,posParam.y,5))
        {
            status = 0;
            FNDebug.LogFormat("<color=#00ff00>[重新开始寻路]:({0},{1})</color>", posParam.x, posParam.y);
            return;
        }

        OnSucceed();
    }

    public override bool IsDone()
    {
        if (status == 2)
            return true;

        if (status == 0)
        {
            if (null == posParam)
            {
                FNDebug.LogError("[寻路位置失败]:位置[x,y]参数为空");
                OnFailed();
                return false;
            }

            if(posParam.mapId == 0)
            {
                posParam.mapId = CSScene.GetMapID();
            }

            if (Utility.IsNearPlayerInMap(posParam.x, posParam.y))
            {
                OnSucceed();
                return false;
            }

            var mainPlayer =CSAvatarManager.MainPlayer;
            if (mainPlayer == null)
            {
                FNDebug.LogError("[寻路位置失败]:主角不存在");
                OnFailed();
                return false;
            }

            if (mainPlayer.IsBeControl)
            {
                FNDebug.LogError("[寻路位置失败]:主角被控制中");
                OnFailed();
                return false;
            }

            status = 1;
            CSMisc.Dot2 dot;
            dot.x = posParam.x;
            dot.y = posParam.y;
            FNDebug.LogFormat("<color=#00ff00>[ActionFindPosition]:[启动寻路]:[mapId:{0}]({1},{2})</color>", posParam.mapId, posParam.x, posParam.y);
            mainPlayer.TowardsTarget(dot);
        }

        //if (status == 1)
        //{
        //    var mainPlayer =CSAvatarManager.MainPlayer;
        //    if (mainPlayer == null)
        //    {
        //        Debug.LogError("[寻路异常]:主角不存在");
        //        OnFailed();
        //        return false;
        //    }

        //    if (mainPlayer.Model.GetMotion() != CSMotion.Run)
        //    {
        //        OnFailed();
        //        Debug.LogFormat("<color=#00ff00>[ActionFindPosition]:[停止寻路]</color>");
        //        return false;
        //    }
        //}

        return false;
    }

    void OnSucceed()
    {
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        FNDebug.LogFormat("<color=#00ff00>[ActionFindPosition]:{0} Succeed</color>", actionParam);
        Succeed = true;
        status = 2;
    }

    void OnFailed()
    {
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        if (null != posParam)
        {
            FNDebug.LogFormat("<color=#ff0000>[ActionFindPosition]:[{0},{1}] Failed</color>", posParam.x, posParam.y);
        }
        else
        {
            FNDebug.Log("<color=#ff0000>[ActionFindPosition]:Failed Arguments Error</color>");
        }
        Succeed = false;
        status = 2;
    }

    public override void OnRecycle()
    {
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerStopMoving);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CancelPathFind, OnCancelPathFind);
        posParam = null;
        base.OnRecycle();
    }
}