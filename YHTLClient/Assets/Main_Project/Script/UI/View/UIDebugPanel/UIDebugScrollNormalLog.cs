﻿using UnityEngine;
using System.Collections;

public class UIDebugScrollNormalLog : UIDebugScrollBase
{
    protected override CSBetterList<UIDebugInfo.GroupData> GetGroupList()
    {
        return UIDebugInfo.GetGroupList(ELogToggleType.NormalLog);
    }
}
