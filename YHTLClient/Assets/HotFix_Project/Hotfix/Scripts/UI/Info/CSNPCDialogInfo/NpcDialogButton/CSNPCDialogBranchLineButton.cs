using System;
using System.Collections.Generic;
using task;
using UnityEngine;

public class CSNPCDialogBranchLineButton : CSNPCDialogButtonBase
{
    public override int GetButtonCount()
    {
        switch (_Data.CurTaskState)
        {
            case TaskState.Acceptable:
                return 1;
            case TaskState.Accepted:
                return 1;
            case TaskState.Completed:
                return 1;
        }

        return 1;
    }

    public override void SetButton(GameObject go, Action<GameObject, string, System.Action, bool> callback, int index = 0)
    {
        if (_Data.CurTaskState == TaskState.Acceptable)
        {
            callback(go, CSString.Format(560), () =>
            {
                Net.ReqAcceptTaskMessage(_Data.CurTaskId);
                CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;
            }, true);
        }
        else if (_Data.CurTaskState == TaskState.Accepted)
        {
            callback(go, CSString.Format(929), () =>
            {
                CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;
                CSMissionBase missionBase = CSMissionManager.Instance.GetMissionByType(2);
                CSMissionManager.Instance.AutoDoMission(missionBase);
            }, true);
        }
        else if (_Data.CurTaskState == TaskState.Completed)
        {
            callback(go, CSString.Format(562), () =>
            {
                Net.ReqSubmitTaskMessage(_Data.CurTaskId);
                CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;
            }, true);
        }
    }
}