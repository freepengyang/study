using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public class ActionFindNpc : ActionBase
{
    public class Factory : ActionFactory<ActionFindNpc,ActionFindNpcParam>
    {

    }

    public override int ID
    {
        get
        {
            return (int)EnumAction.FindNpc;
        }
    }

    /// <summary>
    /// 0 init 1 running 2 done
    /// </summary>
    int status;
    ActionFindNpcParam npcParam;
    IAction actionQueue;

    public override void Init(IActionParam argv)
    {
        base.Init(argv);
        npcParam = argv as ActionFindNpcParam;
        status = 0;
        Succeed = false;

        if (null == npcParam)
        {
            OnFailed();
            return;
        }

        if (!NpcTableManager.Instance.TryGetValue(npcParam.npcId, out TABLE.NPC npcInfo))
        {
            OnFailed();
            return;
        }

        if ((npcInfo.sceneId != 0 && npcInfo.sceneId != CSScene.GetMapID()))
        {
            FNDebug.LogFormat("<color=#00ff00>[ActionFindNpc]:启动:[mapid:{0}(跨地图)]:[pos:({1},{2})]</color>", npcInfo.sceneId,npcInfo.bornX,npcInfo.bornY);
            actionQueue = ActionManager.Instance.Create(EnumAction.ActionQueue,
            ActionManager.Instance.Create(EnumAction.FindMap, npcInfo.sceneId),
            ActionManager.Instance.Create(EnumAction.FindPos, npcInfo.sceneId,npcInfo.bornX, npcInfo.bornY));
        }
        else
        {
            FNDebug.LogFormat("<color=#00ff00>[ActionFindNpc]:启动:[mapId:{0}(本地图)][pos:({1},{2})]</color>", npcInfo.sceneId, npcInfo.bornX, npcInfo.bornY);
            actionQueue = ActionManager.Instance.Create(EnumAction.ActionQueue,
                ActionManager.Instance.Create(EnumAction.FindPos, npcInfo.sceneId,npcInfo.bornX, npcInfo.bornY));
        }
    }

    public override bool IsDone()
    {
        if (status == 2)
            return true;

        if(!actionQueue.IsDone())
        {
            return false;
        }

        if(!actionQueue.Succeed)
        {
            OnFailed();
        }
        else
        {
            OnSucceed();
        }

        return false;
    }

    void OnSucceed()
    {
        FNDebug.LogFormat("<color=#00ff00>[ActionFindNpc]:{0} Succeed</color>", actionParam);
        status = 2;
        //CSNpcGo.OpenNpcFun((int)actionParam);
    }

    void OnFailed()
    {
        if (null != npcParam)
        {
            FNDebug.LogFormat("<color=#ff0000>[ActionFindNpc]:{0} Failed</color>", npcParam.npcId);
        }
        else
        {
            FNDebug.Log("<color=#ff0000>[ActionFindNpc]:Failed Arguments Error</color>");
        }
        status = 2;
    }

    public override void OnRecycle()
    {
        npcParam = null;
        base.OnRecycle();
    }
}