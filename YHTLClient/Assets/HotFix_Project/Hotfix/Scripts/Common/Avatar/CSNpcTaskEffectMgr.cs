using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcTaskEffectData : IDispose
{
    public int npcCfgId;

    public int taskState;

    public CSNpc npc;

    public CSSceneEffect effect;

    public List<CSMissionBase> missionList;


    public void Init(CSNpc _npc)
    {
        taskState = 0;
        if (_npc == null) return;
        npc = _npc;
        npcCfgId = (int)_npc.BaseInfo.ConfigId;
    }

    public void Dispose()
    {
        npc = null;
        missionList?.Clear();
        missionList = null;
    }


}


/// <summary>
/// npc头顶特效管理。暂时只有任务特效，问号感叹号
/// </summary>
public class CSNpcTaskEffectMgr : Singleton<CSNpcTaskEffectMgr>
{

    Dictionary<long, NpcTaskEffectData> taskDataDic = new Dictionary<long, NpcTaskEffectData>();


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    public CSNpcTaskEffectMgr()
    {
        HotManager.Instance.EventHandler.AddEvent(CEvent.Task_GoalUpdate, TaskUpdate);

    }


    public void AddNpc(CSNpc npc)
    {
        if (npc == null || taskDataDic == null) return;
        long id = npc.ID;
        if (taskDataDic.ContainsKey(id))
        {
            FNDebug.LogErrorFormat("CSNpcTaskEffectMgr.AddNpc出错，已存在NPC{0}", id);
            return;
        }

        NpcTaskEffectData data = mPoolHandle.GetCustomClass<NpcTaskEffectData>();
        if (data != null)
        {
            data.Init(npc);
            taskDataDic.Add(id, data);

            RefreshNpcTask(data);
        }

    }
    


    public void RemoveNpc(CSNpc npc)
    {
        if (npc == null || taskDataDic == null || taskDataDic.Count < 1) return;
        long id = npc.ID;
        if (taskDataDic.ContainsKey(id))
        {
            var data = taskDataDic[id];
            data.Dispose();
            DestroyTaskEffect(data);
            mPoolHandle.Recycle(data);
            taskDataDic.Remove(id);
        }
    }


    void TaskUpdate(uint id, object param)
    {
        //if (param == null) return; 目前不可通过此判断来优化
        if (taskDataDic == null || taskDataDic.Count < 1) return;

        for (var it = taskDataDic.GetEnumerator(); it.MoveNext();)
        {
            NpcTaskEffectData data = it.Current.Value;
            RefreshNpcTask(data);
        }
    }


    void RefreshNpcTask(NpcTaskEffectData data)
    {
        if (data == null) return;
        int state = GetNpcTaskState(data);
        if (state == data.taskState) return;
        data.taskState = state;
        ShowTaskEffect(data);
    }



    int GetNpcTaskState(NpcTaskEffectData npcData)
    {
        if (npcData == null || npcData.npc == null) return 0;
        bool hasMission = CSMissionManager.Instance.GetNpcMission(npcData.npcCfgId, ref npcData.missionList);
        if (!hasMission) return 0;

        var missionList = npcData.missionList;
        //优先判断可交任务，再判断可接任务
        bool completed = missionList.Any(x => { return x.TaskState == TaskState.Completed; });
        if (completed) return (int)TaskState.Completed;

        bool canAccept = missionList.Any(x => { return x.TaskState == TaskState.Acceptable; });
        if (canAccept) return (int)TaskState.Acceptable;

        return 0;
    }


    void ShowTaskEffect(NpcTaskEffectData data)
    {
        if (data == null) return;
        int _taskState = data.taskState;
        CSSceneEffectMgr.Instance.Release(ref data.effect);

        if (data.npc == null) return;

        var transform = data.npc.ModelModule.Top.transform;
        var height = data.npc.tblNpc != null ? data.npc.tblNpc.headHeight + 40 : 160;
        if (_taskState == (int)TaskState.Completed)
        {
            data.effect = CSSceneEffectMgr.Instance.Create(transform, 6036, new Vector3(0, height, -100000));
        }
        else if (_taskState == (int)TaskState.Acceptable)
        {
            data.effect = CSSceneEffectMgr.Instance.Create(transform, 6037, new Vector3(0, height, -100000));
        }
    }


    private void DestroyTaskEffect(NpcTaskEffectData data)
    {
        if (data == null) return;
        CSSceneEffectMgr.Instance?.Destroy(data.effect);
        data.effect = null;
    }


    public void Destroy()
    {
        HotManager.Instance?.EventHandler.RemoveEvent(CEvent.Task_GoalUpdate, TaskUpdate);
        if (taskDataDic != null)
        {
            for (var it = taskDataDic.GetEnumerator(); it.MoveNext();)
            {
                NpcTaskEffectData data = it.Current.Value;
                data?.Dispose();
                DestroyTaskEffect(data);
            }
            taskDataDic.Clear();
            taskDataDic = null;
        }
       
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;

        Instance = null;
    }
}
