using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ActionManager : CSInfo<ActionManager>
{
    void Register()
    {
        mActionFactories.Add((int)EnumAction.OpenFrame,new ActionOpenFrame.Factory());
        mActionFactories.Add((int)EnumAction.FindNpc, new ActionFindNpc.Factory());
        mActionFactories.Add((int)EnumAction.FindMap, new ActionFindMap.Factory());
        mActionFactories.Add((int)EnumAction.ActionQueue, new ActionQueue.Factory());
        mActionFactories.Add((int)EnumAction.FindPos, new ActionFindPosition.Factory());
    }
}