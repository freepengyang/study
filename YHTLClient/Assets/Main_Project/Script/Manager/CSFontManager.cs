using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSFontManager : Singleton2<CSFontManager>
{
    public static Font msyhFont;
    public void SaveStaticFont()
    {
        UnityEngine.Object obj = PrestrainFont();
        msyhFont = obj as Font;
        if (msyhFont != null && msyhFont.material != null && msyhFont.material.mainTexture != null)
        {
            msyhFont.material.mainTexture.filterMode = FilterMode.Point;
            msyhFont.material.mainTexture.anisoLevel = 1;
            CSMisc.FixBrokenWord(msyhFont);
        }
    }


    public static UnityEngine.Object PrestrainFont()
    {
#if UNITY_EDITOR
        string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName("font/msyh.u3d", "msyh");

        if (assetPaths.Length == 0)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("There is no asset with name \"" + "font" + "\" msyh " + "msyh");
            return null;
        }
        UnityEngine.Object target = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
        return target;
#else
        LoadedAssetBundle assetBudle = AssetBundleManager.LoadUIAssetAsync("font/msyh.u3d");
        if (assetBudle == null)
        {
            UnityEngine.Debug.LogError("font/msyh == null");
            return null;
        }
        UnityEngine.Object objs = assetBudle.m_AssetBundle.LoadAsset<UnityEngine.Object>("msyh");
        return objs;
#endif
    }
}
