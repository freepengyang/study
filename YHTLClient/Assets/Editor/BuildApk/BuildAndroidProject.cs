using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


public class BuildAndroidProject
{
    //[MenuItem("Tools/Apk/BuildUnityAndroidProject")]
    public static BuildResult BuildUnityAndroidProject()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        PlayerSettings.productName = "zscy";
        string apkPath = (Application.dataPath.Replace("/Assets", "")) + "apk";
        DeleteDir(apkPath);
        BuildReport build = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "apk", BuildTarget.Android,
            BuildOptions.AcceptExternalModificationsToPlayer);

        if (build.summary.result != BuildResult.Succeeded)
            return build.summary.result;
        
        Debug.Log(build.summary.outputPath);

        MoveScript.MoveScriptByAuto($"{build.summary.outputPath}/unityLibrary");
        
        BuildAndroidProject.DeleteDir($"{APKEnvironmentPath.RESOURCE_PATH}/bin/");
        
        BuildAndroidProject.CopyDirectory($"{build.summary.outputPath}/unityLibrary/src/main/assets/bin", $"{APKEnvironmentPath.RESOURCE_PATH}");


        return build.summary.result;
    }
    
    public static void DeleteDir(string file)
    {
        try
        {
            if (Directory.Exists(file))
            {
                foreach (string f in Directory.GetFileSystemEntries(file))
                {
                    if (File.Exists(f))
                    {
                        File.Delete(f);
                        Console.WriteLine(f);
                    }
                    else
                        DeleteDir(f);
                }

                Directory.Delete(file);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
    
    public static void CopyDirectory(string srcdir, string desdir)
    {
        srcdir = srcdir.Replace("\\", "/");
        desdir = desdir.Replace("\\", "/");
        string folderName = srcdir.Substring(srcdir.LastIndexOf("/", StringComparison.Ordinal) + 1);

        string desfolderdir = desdir + "/" + folderName;

        if (desdir.LastIndexOf("/", StringComparison.Ordinal) == (desdir.Length - 1))
        {
            desfolderdir = desdir + folderName;
        }
        string[] filenames = Directory.GetFileSystemEntries(srcdir);

        foreach (string file in filenames)// 遍历所有的文件和目录
        {
             var replace = file.Replace("\\", "/");
            if (Directory.Exists(replace))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string currentdir = desfolderdir + "/" + replace.Substring(replace.LastIndexOf("/", StringComparison.Ordinal) + 1);
                if (!Directory.Exists(currentdir))
                {
                    Directory.CreateDirectory(currentdir);
                }

                CopyDirectory(replace, desfolderdir);
            }

            else // 否则直接copy文件
            {
                string srcfileName = replace.Substring(replace.LastIndexOf("/", StringComparison.Ordinal) + 1);

                srcfileName = desfolderdir + "/" + srcfileName;

                if (!Directory.Exists(desfolderdir))
                {
                    Directory.CreateDirectory(desfolderdir);
                }

                File.Copy(replace, srcfileName, true);
            }
        }
    }

    
    private static string[] GetBuildScene()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            if (s == null) continue;
            if (s.enabled)
                names.Add(s.path);
        }
        return names.ToArray();
    }

    static void ProjectSettting()
    {
        Dictionary<string, string> settings = new Dictionary<string, string>();
        string[] args = System.Environment.GetCommandLineArgs();
        ParseArgs(settings, args);

        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
    }

    private static void ParseArgs(Dictionary<string, string> settings, string[] args)
    {
        UnityEngine.Debug.Log(args.Length);
        for (int i = 0; i < args.Length; i++)
        {
            UnityEngine.Debug.Log(args[i]);
            if (settings.ContainsKey(args[i]))
            {
                settings[args[i]] = args[i + 1];
            }
        }
    }
}

[InitializeOnLoad]
public class UnityScriptCompiling : AssetPostprocessor
{
    public static System.Action action;

    [UnityEditor.Callbacks.DidReloadScripts]
    static void AllScriptsReloaded()
    {
        if (action != null)
        {
            action();
            action = null;
        }
    }
}