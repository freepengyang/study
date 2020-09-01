using UnityEngine;
using System;
public static class SFOut
{
    #region Not Clear
    public static ISFResUpdateMgr IResUpdateMgr;
    public static ISFResourceManager IResourceManager;
    public static string URL_mServerResURL = string.Empty;
    public static string URL_mClientResPath = string.Empty;
    public static string URL_mClientResURL = string.Empty;
    public static string CdnVersion = string.Empty;
    #endregion
    
    public static bool IsLoadLocalRes = true;
    public static bool IsLowMemory = false;
}
