﻿using UnityEngine;
using System.Collections;

public class UIDebugScrollServerMsg_Normal : UIDebugScrollBase
{
    protected override CSBetterList<UIDebugInfo.GroupData> GetGroupList()
    {
        return UIDebugInfo.GetGroupList(ELogToggleType.NormalMSG);
    }
}
