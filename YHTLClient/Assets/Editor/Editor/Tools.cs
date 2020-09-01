
//-------------------------------------------------------------------------
//Tool
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;
using System.IO;
using System.Diagnostics;
using System;

[ExecuteInEditMode]

enum BuildType
{
    UI,
    Effect,
    Model,
    Skill,
};

public class Tools : MonoBehaviour
{

    static string UI = "Assets/UIAsset/chart/res";
    static string UIEFFECT = "Assets/AssetBundleRes/UIEffect/res";
    static string EFFECT = "Assets/AssetBundleRes/Effect/res";


    [MenuItem("Game/开始游戏")]
    static void Init()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/FirstScene.unity");
        EditorApplication.isPlaying = true;
    }

    

    [MenuItem("Game/NguiMeshView")]

    static public void NguiMeshView()
    {
        foreach (var panel in UIPanel.list)
        {
            foreach (var dc in panel.drawCalls)
            {
                if (dc.gameObject.hideFlags != HideFlags.DontSave)
                {
                    dc.gameObject.hideFlags = HideFlags.DontSave;
                }
                else
                {
                    dc.gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }
    }
    static string assetPaht = string.Empty;
    static string buildPath = string.Empty;
    [MenuItem("SVN/Update")]
    static public void SvnUpdate()
   {
        try
        {
            string str = Application.dataPath;
            assetPaht = str.Replace("/YHTLClient/Assets", ""); ;
            if (!Directory.Exists(assetPaht))
            {
                UnityEngine.Debug.LogError("-------路径不对-------");
                return;
            }
            buildPath = string.Format(@"/command:update /path:{0} /closeonend:0", assetPaht);
            processCommand("TortoiseProc.exe", buildPath);
        }
        catch (System.Exception exp)
        {
            UnityEngine.Debug.LogError("找不到改路径 == " + exp.Message);
        }
    }

    [MenuItem("SVN/打包图集 #p")]
    static public void TexturePackerUI()
    {
        string sPath = Environment.GetEnvironmentVariable("path");
        if (!sPath.Contains("TexturePacker"))
        {
            FNDebug.LogError("请先安装TexturePacker，然后把TexturePacker的路径设置到系统环境变量中");
            return;
        }

        UnityEngine.Object[] objs = Selection.GetFiltered((typeof(UnityEngine.Object)), SelectionMode.Assets);
        string path = AssetDatabase.GetAssetPath(objs[0]);
        if (path.Contains("UI_")) //根据选择目录是否包含UI_判断是否打包ui图集
        {            
            FNDebug.Log("当前：" + path);
            TexturePacker(UI);
        }
        else if (path.Contains("UIEffect"))
        {
            FNDebug.Log("当前：" + path);
            TexturePacker(UIEFFECT);
        }
        else if (path.Contains("Effect"))
        {
            FNDebug.Log("当前：" + path);
            TexturePacker(EFFECT);
        }
        else
        {
            FNDebug.LogError("当前目录不能打包：" + path);
        }
        AssetDatabase.Refresh();
    }




    static public void TexturePacker(string BType)
    {
        string mDataPath = Application.dataPath;
        string assetPaht = mDataPath.Replace("Assets", "") + BType;

        if (!Directory.Exists(assetPaht))
        {
            UnityEngine.Debug.LogError("-------路径不对-------");
            return;
        }

        UnityEngine.Object[] objs = Selection.GetFiltered((typeof(UnityEngine.Object)), SelectionMode.Assets);

        if (objs.Length <= 0) return;
        for (int i = 0; i < objs.Length; i++)
        {
            string assetsName = objs[i].name;
            string mPath = mDataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(objs[i]);
            string mCommand = "--scale 1  --size-constraints POT  --disable-rotation --pack-mode Best  --trim-mode Trim --algorithm MaxRects  --max-size 2048 --padding 1 --format unity --sheet {0}/{1}.png --data  {0}/{1}.txt  {2}";
            buildPath = string.Format(mCommand, assetPaht, assetsName, mPath);
            if (!Directory.Exists(assetPaht)) return;
            processCommand("TexturePacker.exe", buildPath);

            if (File.Exists(assetPaht.Replace("res", "") + objs[i].name + ".prefab"))
            {
                FNDebug.Log(assetPaht.Replace("res", "") + objs[i].name + ".prefab");
                UpSpriteData(objs[i].name, BType);
            }
            else
            {
                FNDebug.Log("Create prefab:" + assetPaht.Replace("res", "") + objs[i].name + ".prefab");
                CreateTexture(objs[i].name, BType);
            }

        }
        //         ProcessStartInfo process = new ProcessStartInfo
        //         {
        //             CreateNoWindow = false,
        //             UseShellExecute = false,
        //             RedirectStandardError = false,
        //             RedirectStandardOutput = false,
        //             RedirectStandardInput = false,
        //             ErrorDialog = true,
        //             FileName = "TexturePacker.exe",
        //             Arguments = buildPath,
        //         };
        // 
        //         Process pp = Process.Start(process);
        //         pp.WaitForExit();
        //         int ret = pp.ExitCode;

    }

    public static void processCommand(string command, string argument)
    {
        ProcessStartInfo start = new ProcessStartInfo(command);
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = true;

        if (start.UseShellExecute)
        {
            start.RedirectStandardOutput = false;
            start.RedirectStandardError = false;
            start.RedirectStandardInput = false;
        }
        else
        {
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        }
        Process p = Process.Start(start);
        if (!start.UseShellExecute)
        {
            UnityEngine.Debug.Log(p.StandardOutput.ReadToEnd());
            UnityEngine.Debug.Log(p.StandardError.ReadToEnd());
        }
        p.WaitForExit();
        p.Close();
        AssetDatabase.Refresh();
    }
    static void UpSpriteData(string name, string outpath)
    {
        string pfabth = outpath.Replace("res", "") + name + ".prefab";
        string path = outpath + "/" + name + ".png";
        if (File.Exists(pfabth))
        {
            if (File.Exists(path))
            {
                if (AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)))
                {
                    GameObject prb = AssetDatabase.LoadAssetAtPath(pfabth, typeof(GameObject)) as GameObject;
                    prb.layer = 5;
                    UIAtlas uiAtlas = prb.GetComponent<UIAtlas>();
                    TextAsset ta = AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)) as TextAsset;
                    NGUIJson.LoadSpriteData(uiAtlas, ta);
                    uiAtlas.MarkAsChanged();
                }
            }

            AssetDatabase.SaveAssets();


        }

        AssetDatabase.Refresh();
    }

    static void CreateTexture(string name, string outpath)
    {
        // PitchObjs = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

        string path = outpath + "/" + name + ".png";
        if (File.Exists(path))
        {
            if (AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)))
            {
                #region 设置png属性
                TextureImporter titex = AssetImporter.GetAtPath(path) as TextureImporter;
                titex.textureType = TextureImporterType.Default;
                titex.mipmapEnabled = false;                
                titex.wrapMode = TextureWrapMode.Clamp;
                titex.filterMode = FilterMode.Trilinear;
                titex.anisoLevel = 4;
                titex.textureFormat = TextureImporterFormat.ARGB32;
                AssetDatabase.ImportAsset(path);

                #endregion

                #region 第一步：根据图片创建游戏对象、材质对象
                GameObject atlase = new GameObject();
                atlase.name = name;
                Material mat = new Material(Shader.Find("Unlit/Transparent Colored"));
                mat.name = name;
                AssetDatabase.CreateAsset(mat, path.Replace(".png", ".mat"));
                #endregion

                #region 第二步：给对象添加组件、给材质球关联着色器及纹理同时关联tp产生的坐标信息文件
                atlase.AddComponent<UIAtlas>();
                atlase.layer = 5;
                mat.mainTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                UIAtlas uiAtlas = atlase.GetComponent<UIAtlas>();

                uiAtlas.spriteMaterial = mat;
                TextAsset ta = AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)) as TextAsset;
                NGUIJson.LoadSpriteData(uiAtlas, ta);

                uiAtlas.MarkAsChanged();
                #endregion

                #region 第三步：创建预设
                CreatePrefab(atlase, name, outpath);
                #endregion
            }
        }

        AssetDatabase.Refresh();
    }

    static UnityEngine.Object CreatePrefab(GameObject go, string name, string prepath)
    {
        UnityEngine.Object tmpPrefab = PrefabUtility.CreateEmptyPrefab(prepath.Replace("res", "") + name + ".prefab");        
        tmpPrefab = PrefabUtility.ReplacePrefab(go, tmpPrefab, ReplacePrefabOptions.ConnectToPrefab);
        UnityEngine.Object.DestroyImmediate(go);
        return tmpPrefab;
    }

}
