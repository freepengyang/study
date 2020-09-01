﻿using UnityEngine;
using System.Collections;

public class UIDebugScrollExcetion : UIDebugScrollBase
{
    protected override CSBetterList<UIDebugInfo.GroupData> GetGroupList()
    {
        return UIDebugInfo.GetGroupList(ELogToggleType.Exception);
    }
}
