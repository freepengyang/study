using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public static class QuDaoConstantHot
{
    public static bool LinkCheckState = false;
    static TABLE.SUNDRY linkCheckSundry;
    
    public static bool LinkCheck()
    {
        if (Platform.mPlatformType == PlatformType.EDITOR) return true;

        if (QuDaoConstant.GetPlatformData() != null)
        {
            if (QuDaoConstant.GetPlatformData().requestCode == RequestCode.Normal && QuDaoConstantHot.LinkCheckState == false)
            {
                UtilityTips.ShowTips(100974);
                return false;
            }
        }
        return true;
    }
}