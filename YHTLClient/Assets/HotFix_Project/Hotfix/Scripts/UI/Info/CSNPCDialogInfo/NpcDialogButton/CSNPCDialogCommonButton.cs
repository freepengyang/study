using System;
using task;
using UnityEngine;

public class CSNPCDialogCommonButton : CSNPCDialogButtonBase
{
    public override int GetButtonCount()
    {
        if (_Data.CurTaskState == TaskState.TaskStateNone)
            return InitDefault();
        else
        {
            switch (_Data.CurTaskState)
            {
                case TaskState.Acceptable:
                    return 1;
                case TaskState.Accepted:
                    return 0;
                case TaskState.Completed:
                    return 1;
            }
        }

        return 0;
    }

    public override void SetButton(GameObject go, Action<GameObject, string, System.Action, bool> callback, int index)
    {
        base.SetButton(go, callback, index);
    }
}