using System.IO;
using UnityEngine;

public class APKEnvironmentPath
{
    public static readonly string RESOURCETOOL_PATH =
        Path.GetDirectoryName(Application.dataPath + "/../../Normal/resourceTool/");

    public static readonly string RESOURCE_PATH =
        Path.GetDirectoryName(Application.dataPath + "/../../Normal/Resource/");
    
    public static readonly string ZT_ANDROID_PATH =
        Path.GetDirectoryName(Application.dataPath + "/../../Normal/zt_android/");
    
    public static string ScriptAssembliesPath
    {
        get { return Application.dataPath + "/../Library/ScriptAssemblies/"; }
    }

    public static string ApkScriptPath
    {
        get { return Application.dataPath + "/../apk/unityLibrary/src/main/assets/bin/Data/Managed"; }
    }
    
    public static string realthPath
    {
        get { return "/src/main/assets/bin/Data/Managed/"; }
    }

    public static string TarPath
    {
        get { return Application.dataPath + "/UIAsset/ui/"; }
    }

    public const string HOT_SCRIPT_NAME = "UIHotResPanel";

    public const string MAIN_SCRIPT_NAME = "UIMainResPanel";
    
}