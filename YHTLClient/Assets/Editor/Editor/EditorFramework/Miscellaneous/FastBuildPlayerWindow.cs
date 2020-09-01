using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class FastBuildPlayerWindow : EditorWindow
{
    private EditorWindow buildPlayerWindow;

    [MenuItem("Tools/Miscellaneous/FastBuildPlayerWindow")]
    public static void FastBuildPlayerWindowProc()
    {
        EditorWindow win = GetWindow(typeof(FastBuildPlayerWindow));
        win.Show();
    }

    void Awake()
    {
        var unityEditor = Assembly.LoadFile(EditorApplication.applicationContentsPath + "/Managed/UnityEditor.dll");
        var buildPlayerWindowType = unityEditor.GetType("UnityEditor.BuildPlayerWindow");

        var array = Resources.FindObjectsOfTypeAll(buildPlayerWindowType);
        var noBuildPlayerWindow = array == null || array.Length <= 0;

        buildPlayerWindow = GetWindow(buildPlayerWindowType, true);

        minSize = buildPlayerWindow.minSize;
        maxSize = buildPlayerWindow.maxSize;
        position = buildPlayerWindow.position;
        title = buildPlayerWindow.title;

        if (noBuildPlayerWindow)
            buildPlayerWindow.Close();
    }

    void OnDestroy()
    {

    }

    static BuildTarget GetSelectedBuildTarget()
    {
        switch (EditorUserBuildSettings.selectedBuildTargetGroup)
        {
            //case BuildTargetGroup.WebPlayer:
            //    if (EditorUserBuildSettings.webPlayerStreamed)
            //        return BuildTarget.WebPlayerStreamed;

            //    return BuildTarget.WebPlayer;

            case BuildTargetGroup.Standalone:
                return EditorUserBuildSettings.selectedStandaloneTarget;

            case BuildTargetGroup.iOS:
                return BuildTarget.iOS;

            case BuildTargetGroup.Android:
                return BuildTarget.Android;

            case BuildTargetGroup.Switch:
                return BuildTarget.Switch;

            case BuildTargetGroup.XboxOne:
                return BuildTarget.XboxOne;

            case BuildTargetGroup.Lumin:
                return BuildTarget.Lumin;

            case BuildTargetGroup.tvOS:
                return BuildTarget.tvOS;

            case BuildTargetGroup.PS4:
                return BuildTarget.PS4;

            case BuildTargetGroup.WebGL:
                return BuildTarget.WebGL;

            case BuildTargetGroup.WSA:
                return BuildTarget.WSAPlayer;

            default:
                return EditorUserBuildSettings.activeBuildTarget;
        }
    }

    static void DirectoryMerge(string sourceDirName, string destDirName)
    {
        if (!Directory.Exists(sourceDirName))
            return;

        const string title = "Hold On";

        string info = "Delete files from dest not in source";
        float progress = 0;

        if (Directory.Exists(destDirName))
        {
            var destFiles = Directory.GetFiles(destDirName, "*.*", SearchOption.AllDirectories);
            foreach (var file in destFiles)
            {
                if (!File.Exists(sourceDirName + file.Remove(0, destDirName.Length)))
                    File.Delete(file);

                progress += 1.0f / destFiles.Length;
                EditorUtility.DisplayProgressBar(title, info, progress);
            }
        }

        info = "Copy changed and new files from source to dest";
        progress = 0;

        // Then copy changed and new files from source to dest
        var sourceFiles = Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories);
        foreach (var file in sourceFiles)
        {
            var destFile = destDirName + file.Remove(0, sourceDirName.Length);

            progress += 1.0f / sourceFiles.Length;
            EditorUtility.DisplayProgressBar(title, info, progress);

            if (File.Exists(destFile) && File.GetLastWriteTimeUtc(file) == File.GetLastWriteTimeUtc(destFile) && new FileInfo(file).Length == new FileInfo(destFile).Length)
                continue;

            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            File.Copy(file, destFile, true);
            File.SetCreationTime(destFile, File.GetCreationTime(file));
            File.SetLastAccessTime(destFile, File.GetLastAccessTime(file));
            File.SetLastWriteTime(destFile, File.GetLastWriteTime(file));
        }

        EditorUtility.ClearProgressBar();
    }

    void OnGUI()
    {
        try
        {
            AccessExtensions.Invoke(buildPlayerWindow,"OnGUI");
            //buildPlayerWindow.SendEvent("OnGUI");
        }
        catch (Exception)
        {

        }

        var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
        var selectedBuildTarget = GetSelectedBuildTarget();

        GUI.enabled = /*selectedBuildTarget != activeBuildTarget && typeof(BuildPipeline).Invoke<bool>("IsBuildTargetSupported", selectedBuildTarget)*/true;

        GUILayout.BeginVertical();

        if (GUILayout.Button("Fast Switch Platform"))
        {
            const string metadataPath = "Library/metadata";

            var activeBuildTargetCachePath = "Fast Switch Platform/" + activeBuildTarget + "_metadata";
            var selectedBuildTargetCachePath = "Fast Switch Platform/" + selectedBuildTarget + "_metadata";

            DirectoryMerge(metadataPath, activeBuildTargetCachePath);

            DirectoryMerge(selectedBuildTargetCachePath, metadataPath);

            EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTarget);

            // Look for missing metadata files
            var currentAsset = 0;
            var assetPaths = AssetDatabase.GetAllAssetPaths();
            var assetCount = assetPaths.Length;

            AssetDatabase.StartAssetEditing();

            foreach (var path in assetPaths.Where(path => path.StartsWith("Assets")))
            {
                EditorUtility.DisplayProgressBar("Hold on", "Reimporting changed assets...", currentAsset / (float)assetCount);

                var filename = Path.GetFileName(path);
                var guid = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid) && !File.Exists(metadataPath + "/" + guid.Substring(0, 2) + "/" + guid))
                {
                    EditorUtility.DisplayProgressBar("Hold on", "Reimporting changed assets... (" + filename + ")", currentAsset / (float)assetCount);

                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }

                currentAsset++;
            }

            EditorUtility.DisplayProgressBar("Hold on", "Reimporting changed assets... (processing batch)", 1.0f);
            AssetDatabase.StopAssetEditing();

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
        }

        GUILayout.Space(10f);

        GUILayout.EndVertical();
    }


    [DidReloadScripts]
    static void ReopenBuildPlayerWindow()
    {
        var array = Resources.FindObjectsOfTypeAll(typeof(FastBuildPlayerWindow));
        if (array == null || array.Length <= 0)
            return;

        GetWindow<FastBuildPlayerWindow>(true).Close();
        ShowBuildPlayerWindow();
    }

    [MenuItem("Window/Fast Switch Platform")]
    static void ShowBuildPlayerWindow()
    {
        GetWindow<FastBuildPlayerWindow>(true).Show();
    }
}