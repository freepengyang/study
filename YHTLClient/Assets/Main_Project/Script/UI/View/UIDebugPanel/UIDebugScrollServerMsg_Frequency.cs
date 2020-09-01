﻿using UnityEngine;
using System.Collections;

public class UIDebugScrollServerMsg_Frequency : UIDebugScrollBase
{
    protected override CSBetterList<UIDebugInfo.GroupData> GetGroupList()
    {
        return UIDebugInfo.GetGroupList(ELogToggleType.FrequencyMSG);
    }
}
