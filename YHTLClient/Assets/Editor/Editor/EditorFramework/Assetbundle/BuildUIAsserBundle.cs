using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;
using System;

public class BuildUIAsserBundle
{

    static BuildAssetBundleOptions options = BuildAssetBundleOptions.UncompressedAssetBundle |
                                         BuildAssetBundleOptions.DeterministicAssetBundle;

    //保存所有Resource信息
    static List<string> mResources = new List<string>();

    static string BuildAssetInfoPath = Application.dataPath + "/UIVersion/";

    //WindowsPlayer打包
    [MenuItem("Tools/UI/BuildUI")]
    public static void BuildWindows()
    {
        HandleExampleBundle();
    }

    //[MenuItem("Tools/UI/DestroyComponent")]
    //static void DestroyComponents()
    //{
    //    Object mainAsset = null;        //主素材名，单个
    //    //刷新数据
    //    Caching.ClearCache();

    //    AssetDatabase.Refresh();

    //    string paths = Application.dataPath + "/UIAsset/Prefabs/";
    //    mResources.Clear();

    //    string[] filths = Directory.GetFiles(paths, "*.prefab");

    //    List<string> newfiles = new List<string>();

    //    for (int i = 0; i < filths.Length; i++)
    //    {
    //        string str = filths[i].Replace(Application.dataPath, "Assets");
    //        mResources.Add(str);
    //    }

    //    for (int j = 0; j < mResources.Count; j++)
    //    {
    //        string path = mResources[j];

    //        mainAsset = LoadAsset(mResources[j]);

    //        if (mainAsset != null)
    //        {
    //            GameObject gp = mainAsset as GameObject;

    //            CSAtlasCollect[] list = gp.GetComponentsInChildren<CSAtlasCollect>(true);

    //            for (int num = 0; num < list.Length; num++)
    //            {
    //                Debug.LogError(list[num]);
    //                GameObject.DestroyImmediate(list[num], true);
    //            }
    //        }
    //    }

    //    Debug.LogError("chenggog");

    //    AssetDatabase.Refresh();
    //}

    [MenuItem("Assets/UI/SelectObjects")]
    static void SelectObjects()
    {
        //刷新数据
        Caching.ClearCache();

        AssetDatabase.Refresh();


        Object[] objs = Selection.objects;

        if (objs == null) return;

        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];

            if (obj == null) { FNDebug.LogError("obj == null"); continue; }

            GameObject gp = obj as GameObject;

            //Component[] list = gp.GetComponentsInChildren<CSAtlasCollect>(true);

            //for (int num = 0; num < list.Length; num++)
            //{
            //    Debug.LogError(list[num]);
            //    GameObject.DestroyImmediate(list[num], true);
            //}
            
            //if (gp.GetComponent<CSAtlasCollect>() != null)
            //{
            //    GameObject.DestroyImmediate(gp.GetComponent<CSAtlasCollect>(), true);
            //}


           // BuildSingleBundle(obj);

            string path = AssetDatabase.GetAssetPath(obj);

            string[] strs = AssetDatabase.GetDependencies(new string[] { path });

            for (int n = 0; n < strs.Length; n++)
            {
                FNDebug.LogError(strs[n]);
            }
        }

        AssetDatabase.Refresh();
    }

    static void HandleExampleBundle(BuildTarget target)
    {
        Object mainAsset = null;        //主素材名，单个
        Object[] addis = null;     //附加素材名，多个
        string assetfile = string.Empty;  //素材文件名



        string assetPath = BuildAssetInfoPath;

        ///-----------------------------生成共享的关联性素材绑定-------------------------------------
        BuildPipeline.PushAssetDependencies();

        assetfile = assetPath + "Dialog" + ".unity3d";
        mainAsset = LoadAsset("Assets/Resources/UI/UIAtlas/UI/NewPublic.prefab");
        BuildPipeline.BuildAssetBundle(mainAsset, null, assetfile, options, target);

        ///------------------------------生成PromptPanel素材绑定-----------------------------------
        BuildPipeline.PushAssetDependencies();
        mainAsset = LoadAsset("Assets/Resources/UI/Prefabs/RulePanel.prefab");
        addis = new Object[1];
        addis[0] = LoadAsset("Assets/Resources/UI/Prefabs/RulePanel.prefab");
        assetfile = assetPath + "PromptPanel" + ".unity3d";
        BuildPipeline.BuildAssetBundle(mainAsset, addis, assetfile, options, target);
        BuildPipeline.PopAssetDependencies();

        ///-------------------------------刷新---------------------------------------
        BuildPipeline.PopAssetDependencies();
    }

     [MenuItem("Assets/UI/HandleScript")]
    static void HandleScript()
    {
        //string  assetPath = Constant.build_assetbundleFilePath + "UIFont" + ".unity3d";
        //Object mainAsset = LoadAsset("Assets/NGUI/Scripts/UI/UIFont.cs");
        //BuildPipeline.BuildAssetBundle(mainAsset, null, assetPath, options, buildPlatform);

        //assetPath = Constant.build_assetbundleFilePath + "UIAtlas" + ".unity3d";
        //mainAsset = LoadAsset("Assets/NGUI/Scripts/UI/UIAtlas.cs");
        //BuildPipeline.BuildAssetBundle(mainAsset, null, assetPath, options, buildPlatform);
    }

    static void BuildSingleBundle(Object mainAsset)
    {
        //Caching.CleanCache();

        //string assetPath = "";

        //GameObject go = mainAsset as GameObject;

        /////-----------------------------生成共享的关联性素材绑定-------------------------------------  ///

        //List<Object> resourceList = CollectResources(go);

        //BuildPipeline.PushAssetDependencies();

        //Object obj = null;
        //for (int i = 0; i < resourceList.Count; i++)
        //{
        //    obj = LoadAsset(AssetDatabase.GetAssetPath(resourceList[i]));

        //    if (obj != null)
        //    {
        //        assetPath = Constant.build_assetbundleFilePath + obj.name + ".unity3d";
        //        BuildPipeline.BuildAssetBundle(obj, null, assetPath, options, buildPlatform);
        //    }
        //}

        ///// ------------------------------生成PromptPanel素材绑定---------------------------------- -  ///
        //BuildPipeline.PushAssetDependencies();
        //assetPath = Constant.build_assetbundleFilePath + mainAsset.name + ".unity3d";
        //BuildPipeline.BuildAssetBundle(mainAsset, null, assetPath, options, buildPlatform);
        //BuildPipeline.PopAssetDependencies();

        BuildPipeline.PopAssetDependencies();
    }

    static void HandleExampleBundle()
    {
        Object mainAsset = null;        //主素材名，单个
        //刷新数据
        Caching.ClearCache();

        AssetDatabase.Refresh();

        string paths = Application.dataPath + "/Resources/UI/Prefabs/";
        mResources.Clear();

        string[] filths = Directory.GetFiles(paths, "*.prefab");

        List<string> newfiles = new List<string>();

        for (int i = 0; i < filths.Length; i++)
        {
            string str = filths[i].Replace(Application.dataPath, "Assets");
            mResources.Add(str);
        }

        for (int j = 0; j < mResources.Count; j++)
        {
            string path = mResources[j];

            mainAsset = LoadAsset(mResources[j]);

            BuildSingleBundle(mainAsset);
        }

        ///-------------------------------刷新---------------------------------------  ///
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/UI/BuildAssetInfos")]
    static void CreateAssetInfos()
    {

        //刷新数据
        Caching.ClearCache();

        AssetDatabase.Refresh();

        string paths = Application.dataPath + "/Resources/UI/Prefabs/";
        mResources.Clear();

        string[] filths = Directory.GetFiles(paths, "*.prefab");

        List<string> newfiles = new List<string>();

        for (int i = 0; i < filths.Length; i++)
        {
            string str = filths[i].Replace(Application.dataPath, "Assets");
            mResources.Add(str);
        }
        if (mResources == null || mResources.Count <= 0) return;

        ////创建所有资源Asset列表
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("AllAssets");

        object mainAsset = null;

        for (int i = 0; i < mResources.Count; i++)
        {
            mainAsset = LoadAsset(mResources[i]);

            if (mainAsset == null) { FNDebug.LogError(string.Format("{0} == null", mResources[i])); continue; }

            GameObject go = mainAsset as GameObject;

            List<string> dependList = new List<string>();

            List<Object> resourceList = CollectResources(go);

            for (int num = 0; num < resourceList.Count; num++)
            {
                if (!dependList.Contains(resourceList[num].name))
                {
                    dependList.Add(resourceList[num].name);
                }
            }

            XmlElement ele = doc.CreateElement("Asset");

            //设置路径名称
            ele.SetAttribute("uiname", GetAssetName(mResources[i]));

            ele.SetAttribute("depency", GetDepency(dependList));

            root.AppendChild(ele);

        }

        doc.AppendChild(root);
        doc.Save(BuildAssetInfoPath + "AssetInfo.bytes");
        FNDebug.Log("CreateAssetInfo success!!!");
    }

    /// <summary>
    /// 载入素材
    /// </summary>
    static UnityEngine.Object LoadAsset(string file)
    {
        return AssetDatabase.LoadMainAssetAtPath(file);
    }

    static List<Object> CollectResources(GameObject go)
    {
        List<Object> list = new List<Object>();

        UISprite[] sp = go.GetComponentsInChildren<UISprite>(true);

        for (int i = 0; i < sp.Length; i++)
        {
            Object obj = sp[i].atlas;

            if (obj != null && !list.Contains(obj))
            {
                list.Add(obj);
            }
        }

        UILabel[] lb = go.GetComponentsInChildren<UILabel>(true);

        for (int i = 0; i < lb.Length; i++)
        {
            Object obj = lb[i].ambigiousFont;

            if (obj != null && !list.Contains(obj))
            {
                list.Add(obj);
            }
        }


        return list;
    }

    static string GetDepency(List<string> dependList)
    {
        string str = "";
        for (int j = 0; j < dependList.Count; j++)
        {

            if (j != dependList.Count - 1)
                str += dependList[j] + ",";
            else
                str += dependList[j];
        }

        return str;

    }

    static string GetAssetName(string _name)
    {
        return _name.Replace("Assets/Resources/UI/Prefabs/", "").Replace(".prefab", "");
    }

}
