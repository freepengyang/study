using AssetBundles;
using UnityEngine;

public class AssetBundleLoad
{
    static public GameObject LoadUIAsset(string assetName, bool isMiniLoad)
    {
#if UNITY_EDITOR
        string[] assetPaths =
            UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName($"ui/{assetName.ToLower()}.u3d", assetName);
        if (assetPaths.Length == 0)
        {
            if (FNDebug.developerConsoleVisible)
                FNDebug.LogError("There is no asset with name \"" + assetName + "\" in " + assetName);
            return null;
        }

        UnityEngine.Object target = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);

        return target as GameObject;
#else
        if (isMiniLoad)
        {
            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle($"ui/{assetName.ToLower()}.u3d");

            if (bundle == null)
            {
                UnityEngine.Debug.LogError("assetBudle == null");
                return null;
            }
            return bundle.m_AssetBundle.LoadAsset<GameObject>(assetName);
        }

        LoadedAssetBundle assetBudle = AssetBundleManager.LoadUIAssetAsync($"ui/{assetName.ToLower()}.u3d");
        if (assetBudle == null)
        {
            UnityEngine.Debug.LogError("assetBudle == null");
            return null;
        }
        return assetBudle.m_AssetBundle.LoadAsset<GameObject>(assetName);
#endif
    }

    public static UnityEngine.Object Prestrain()
    {
#if UNITY_EDITOR
        string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName("chart/icon.u3d", "icon");

        if (assetPaths.Length == 0)
        {
            if (FNDebug.developerConsoleVisible)
                FNDebug.LogError("There is no asset with name \"" + "icon" + "\" in " + "icon");
            return null;
        }

        UnityEngine.Object target = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
        //FileCommon.SaveUIResLoadPre("chart/icon");
        return target;
#else
        LoadedAssetBundle assetBudle = AssetBundleManager.LoadUIAssetAsync("chart/icon.u3d");
        if (assetBudle == null) return null;

        UnityEngine.Object objs = assetBudle.m_AssetBundle.LoadAsset<UnityEngine.Object>("icon");

        return objs;

#endif
    }
}