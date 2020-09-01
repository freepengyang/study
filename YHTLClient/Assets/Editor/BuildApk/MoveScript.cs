using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using AssetBundles;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class MoveScript : MonoBehaviour
{
    //[MenuItem("AssetBundle/Move Script/Use Library")] //设置编辑器菜单选项
    public static void MoveScriptUseLibrary()
    {
        StartMoveScript(APKEnvironmentPath.HOT_SCRIPT_NAME, APKEnvironmentPath.ScriptAssembliesPath, false);
        StartMoveScript(APKEnvironmentPath.MAIN_SCRIPT_NAME, APKEnvironmentPath.ScriptAssembliesPath, false);
        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("Move Script Success!!");
    }

    //[MenuItem("AssetBundle/Move Script/Use APK")] //设置编辑器菜单选项
    public static void MoveScriptUseAPK()
    {
        bool failed;
        failed = !StartMoveScript(APKEnvironmentPath.HOT_SCRIPT_NAME, APKEnvironmentPath.ApkScriptPath, true);
        failed |= !StartMoveScript(APKEnvironmentPath.MAIN_SCRIPT_NAME, APKEnvironmentPath.ApkScriptPath, true);

        if (failed)
        {
            UnityEngine.Debug.LogError("Move Script Error!!");
        }
        else
        {
            UnityEngine.Debug.Log("Move Script Success!!");
        }

        AssetDatabase.Refresh();
    }

    public static void MoveScriptByAuto(string outPath)
    {
        bool failed;
        failed = !StartMoveScript(APKEnvironmentPath.HOT_SCRIPT_NAME, $"{outPath}{APKEnvironmentPath.realthPath}",
            true);
#if ILRunTime
        failed |=
 !StartMoveScript(APKEnvironmentPath.MAIN_SCRIPT_NAME, $"{outPath}{APKEnvironmentPath.realthPath}", true);
#endif
        if (failed)
        {
            UnityEngine.Debug.LogError("Move Script Error!!");
        }
        else
        {
            UnityEngine.Debug.Log("Move Script Success!!");
        }

        AssetDatabase.Refresh();
    }

    private static bool StartMoveScript(string dllName, string sourcePath, bool deleteSource)
    {
        string tarFile = APKEnvironmentPath.TarPath + dllName + ".bytes";
        if (File.Exists(tarFile))
        {
            File.Delete(tarFile);
        }

        string source = sourcePath + dllName + ".dll";
        if (!File.Exists(source)) return false;

        File.Copy(source, tarFile);

        if (deleteSource)
        {
            File.Delete(source);
        }

        return true;
    }
}