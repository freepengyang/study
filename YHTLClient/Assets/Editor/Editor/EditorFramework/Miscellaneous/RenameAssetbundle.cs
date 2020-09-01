using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class RenameAssetbundle : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/RenameAssetbundle")]
        public static void RenameAssetbundleProc()
        {
            EditorWindow win = GetWindow(typeof(RenameAssetbundle));
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Deal"))
            {
                base.BeginHandle();
            }

            if (base.CanHandle())
            {
                for (int i = 0; i < 5; i++)
                {
                    if (base.CanHandle())
                    {
                        Deal();
                        base.MoveHandle();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        void Deal()
        {
            UnityEngine.Object obj = base.GetCurHandleObj();
            string path = base.GetCurHandlePath();
            path = Application.dataPath.Replace("Assets","") + path;
            path = path.Replace("AssetBundleRes", "StreamingAssets");
            path = path.Replace("SplitAtlas/", "");
            path = path.Replace("AtlasAction/", "");
            path = path.Replace("SplitAtlasDirection/", "");
            path = path.Replace("AtlasDirection/", "");
            string fileName = FileUtility.GetFileName(path);
            string dir = FileUtility.GetDirectory(path);
            string assetBundlePath = dir + "/" + fileName.ToLower();
            //Debug.Log(assetBundlePath);
            //Debug.Log(dir);
            //Debug.Log(fileName);
            path = FileUtility.GetFileWithoutExtension(path);
            //Debug.Log(path);
            if (File.Exists(assetBundlePath))
            {
                FileUtility.Rename(assetBundlePath, path);
            }
            EditorUtility.UnloadUnusedAssetsImmediate();
        }
    }
}


