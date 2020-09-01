using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
using System.IO;
namespace ExtendEditor
{
    public class MapAssetbundleMissCheck : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/MapAssetbundleMissCheck")]
        public static void MapAssetbundleMissCheckProc()
        {
            EditorWindow window = GetWindow(typeof(MapAssetbundleMissCheck));
            window.Show();
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
                UnityEngine.Object obj = base.GetCurHandleObj();
                string path = base.GetCurHandlePath();
                path = path.Replace("AssetBundleRes", "StreamingAssets");
                path = Application.dataPath.Replace("Assets", "") + path;
                path = path.Substring(0, path.LastIndexOf(".")) + ".assetbundle";
                FNDebug.Log(path);
                if (!File.Exists(path))
                {
                    FNDebug.LogError(path + " is not exist");
                }
                base.MoveHandle();
            }
        }
    }

}
