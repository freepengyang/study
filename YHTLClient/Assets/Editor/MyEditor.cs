using UnityEngine;
using UnityEditor;
public class MyEditor : EditorWindow
{
    private GameObject go;
    [MenuItem("GameObject/window")]
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 500);
        MyEditor window = (MyEditor)EditorWindow.GetWindowWithRect(typeof(MyEditor), wr, true, "widow name");
        window.Show();
    }

    //绘制窗口时调用
    void OnGUI()
    {
        GUILayout.Label("打包流程: Clear Asset Name  --> Set Asset Name --> Create Asset Name");
        GUILayout.BeginArea(new Rect(0, 20, 500, 200));
        bool build = GUILayout.Button("Build UI", GUILayout.Width(200), GUILayout.Height(25));
        if (build) 
        {
            this.ShowNotification(new GUIContent("build succeed"));
        }
        GUILayout.EndArea();
        GUILayout.Space(30);
        GUILayout.Label("场景单个资源打包->->->切记一定要设置资源名字");

        GUILayout.BeginArea(new Rect(0, 70, 500, 200));
        {
            GUILayout.BeginHorizontal();
            bool buildResources = GUILayout.Button("Independent resources", GUILayout.Width(200), GUILayout.Height(25));
            GUILayout.Space(10);
            go = EditorGUILayout.ObjectField(go, typeof(GameObject), true, GUILayout.Width(400),GUILayout.Height(25)) as GameObject;
            GUILayout.EndHorizontal();
            if (buildResources) IndependentResources(go);
        }
        GUILayout.EndArea();
    }

    void IndependentResources(GameObject gb)
    {
        if (gb == null)
        {
            this.ShowNotification(new GUIContent("未选中任何资源"));
        }
        else
        {

            GameObject g = GameObject.Instantiate(gb) as GameObject;
            
            AssetBundleBuild[] abb = new AssetBundleBuild[1];
            abb[0].assetBundleName = gb.name;
            abb[0].assetNames = new string[1] { gb.name };
            BuildPipeline.BuildAssetBundles(Application.dataPath, abb, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
    }

}