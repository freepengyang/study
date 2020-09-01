using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectDefine : EditorWindow
{
    /// <summary>
    /// 选择编译环境    ILRuntime
    /// </summary>
    [MenuItem("Define/Define For ILRuntime")] //设置编辑器菜单选项
    public static void DefineForILRunTime()
    {
        BuildTargetGroup buildType;
#if UNITY_ANDROID
        buildType = BuildTargetGroup.Android;
#elif UNITY_IOS
        buildType = BuildTargetGroup.iOS;
#endif
        string settting = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildType);
        if (!settting.Contains("EnableLog"))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType,
                "ILRuntime;DISABLE_ILRUNTIME_DEBUG;EnableLog");
        }
        else
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType,
                "ILRuntime;DISABLE_ILRUNTIME_DEBUG");
        }
    }

    /// <summary>
    /// 选择编译环境    Mono
    /// </summary>
    [MenuItem("Define/Define For Mono")] //设置编辑器菜单选项
    public static void DefineForMono()
    {
        BuildTargetGroup buildType;
#if UNITY_ANDROID
        buildType = BuildTargetGroup.Android;
#elif UNITY_IOS
        buildType = BuildTargetGroup.iOS;
#endif
        string settting = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildType);
        if (!settting.Contains("EnableLog"))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType, "Mono;EnableLog");
        }
        else
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType, "Mono");
        }
    }

    /// <summary>
    /// 选择输出debug
    /// </summary>
    [MenuItem("Define/Define Add Debug")] //设置编辑器菜单选项
    public static void DefineAddDebug()
    {
        BuildTargetGroup buildType;
#if UNITY_ANDROID
        buildType = BuildTargetGroup.Android;
#elif UNITY_IOS
        buildType = BuildTargetGroup.iOS;
#endif
        string settting = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildType);
        if (!settting.Contains("EnableLog"))
        {
            settting += ";EnableLog";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType, settting);
        }
    }

    /// <summary>
    /// 选择不输出debug
    /// </summary>
    [MenuItem("Define/Define Remove Debug")] //设置编辑器菜单选项
    public static void DefineRemoveDebug()
    {
        BuildTargetGroup buildType;
#if UNITY_ANDROID
        buildType = BuildTargetGroup.Android;
#elif UNITY_IOS
        buildType = BuildTargetGroup.iOS;
#endif
        string settting = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildType);
        if (settting.Contains("EnableLog"))
        {
            settting = settting.Replace(";EnableLog", "").Replace("EnableLog;", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildType, settting);
        }
    }
}