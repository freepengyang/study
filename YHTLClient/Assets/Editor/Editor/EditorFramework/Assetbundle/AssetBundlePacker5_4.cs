#if (!UNITY_4_7&&!UNITY_4_6)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
namespace ExtendEditor
{
    enum EPack5_4Type
    {
        AssetBundleBuild,
        AssetBundleNames,
    }
    public class AssetBundlePacker5_4 :AssetBundlePacker 
    {
        EPack5_4Type mEPack5_4Type = EPack5_4Type.AssetBundleBuild;
        [MenuItem("Assets/AssetBundle/Pack Select to AssetBundle Unity5.4")]
        public static void RightClick5_4()
        {
            AssetBundlePacker5_4 window = GetWindow(typeof(AssetBundlePacker5_4)) as AssetBundlePacker5_4;
            //window.titleContent =new GUIContent("Unity5.4打包");
            window.Show();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            mEPack5_4Type = (EPack5_4Type)EditorPrefsUtility.GetInt("EPackType5_4", (int)mEPack5_4Type);
        }

        public override void OnGUI()
        {
            if (base.CanHandle()) return;
            base.OnGUI();
            EPack5_4Type type = (EPack5_4Type)EditorGUILayout.EnumPopup("EPackType5_4", mEPack5_4Type);
            if (type != mEPack5_4Type)
            {
                mEPack5_4Type = type;
                EditorPrefsUtility.SetInt("EPackType5_4", (int)mEPack5_4Type);
            }
            //if (GUILayout.Button("MoveNotUseRes"))
            //{
            //    string s = Application.dataPath.Replace("Client/Branch/ClientAndroid/Assets", "Data/Branch/CurrentUseData/Normal/wzcq_android/");
            //    string s1 = Application.dataPath.Replace("Client/Branch/ClientAndroid/Assets", "Data/Branch/CurrentUseData/Normal/Use/");
            //    string s2 = Application.dataPath.Replace("Client/Branch/ClientAndroid/Assets", "Data/Branch/CurrentUseData/Normal/NotUse/");
            //    if (Directory.Exists(s))
            //    {
            //        List<string> paths = new List<string>();
            //        FileUtility.GetDeepAssetPaths(s, paths);
            //    }
            //}
        }

        public override void OnClickPack()
        {
            base.OnClickPack();
        }

        public override bool PackExtend(string path, Object Object)
        {
            //return base.PackExtend(path);
            if (mEPack5_4Type == EPack5_4Type.AssetBundleBuild)
            {
                PackByAssetBundleBuild(path, Object);
            }
            else if (mEPack5_4Type == EPack5_4Type.AssetBundleNames)
            {
                PackByAssetBundleNames(path, Object);
            }
            return true;
        }

        void PackByAssetBundleBuild(string path, Object Object)
        {
            mNextPackTime = EditorApplication.timeSinceStartup + 1f;
            //Debug.Log("mNextPackTime = " + mNextPackTime);
            //Debug.Log("path = " + path);
            string fileName = FileUtility.GetFileName(path);
            //Debug.Log("fileName = " + fileName);
            string resPath = AssetDatabase.GetAssetPath(Object).Replace(Application.dataPath, "Assets").Replace(".assetbundle", "");
            //Debug.Log("resPath = " + resPath);
            
            string targetResPath = path.Replace(Application.dataPath, "Assets").Replace(".assetbundle", "");
            //Debug.Log("targetResPath = " + targetResPath);
            string targetDirPath = FileUtility.GetDirectory(targetResPath);
            //Debug.Log("targetDirPath = " + targetDirPath);

            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = fileName + ".u3d";
            string[] enemyAssets = new string[1];
            enemyAssets[0] = resPath;
            buildMap[0].assetNames = enemyAssets;
            //Debug.Log(EditorApplication.timeSinceStartup + " " + targetDirPath);
            BuildPipeline.BuildAssetBundles(targetDirPath, buildMap, BuildAssetBundleOptions.None, mBuildTarget);
            
            //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, mBuildTarget);
            //EditorUtility.UnloadUnusedAssetsImmediate();
        }

        void PackByAssetBundleNames(string path, Object Object)
        {

        }
    }
}
#endif