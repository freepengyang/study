using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using AssetBundles;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class DoAssetbundle : MonoBehaviour {

    private static string uiAssetBundlesPath
    {
        get
        {
            string curPath = Application.dataPath;
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/Android/");
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/iOS/");
            }
            else
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/Android/");
            }
        }
    }

    private static string uiAssetManifestPath
    {
        get
        {
            string curPath = Application.dataPath;
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/Android/Android.u3d");
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/iOS/iOS.u3d");
            }
            else
            {
                return curPath.Replace("YHTLClient/Assets", "Normal/zt_android/Android/Android.u3d");
            }
        }
    }

    /// <summary>
    /// 自动打包所有资源（设置了Assetbundle Name的资源）---------------------注意：打包时删除manifest
    /// </summary>
    [MenuItem("AssetBundle/Create All AssetBundles")] //设置编辑器菜单选项
    public static void CreateAllAssetBundles()
    {
        string manifestPath = uiAssetManifestPath;
        if (File.Exists(manifestPath))
        {
            File.Delete(manifestPath);
            File.Delete(manifestPath + ".manifest");
        }

        Caching.ClearCache();
        //ClearAssetBundlesName();
        SetMainAssetBundleName();
        //打包资源的路径，打包在对应平台的文件夹下
        //string targetPath = Application.streamingAssetsPath + "/"+Utility.GetPlatformName()+"/";

        if (!Directory.Exists(uiAssetBundlesPath))
        {
            Directory.CreateDirectory(uiAssetBundlesPath);
        }

        //打包资源
        BuildPipeline.BuildAssetBundles(uiAssetBundlesPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        FNDebug.Log("Build AssetBundle Success!!");
        /////----------------------创建文件列表-----------------------
        //string newFilePath = targetPath + "/files.txt";
        //if (File.Exists(newFilePath)) File.Delete(newFilePath);

        //paths.Clear();
        //files.Clear();
        //Recursive(targetPath);

        //FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        //StreamWriter sw = new StreamWriter(fs);
        //for (int i = 0; i < files.Count; i++)
        //{
        //    string file = files[i];
        //    if (file.EndsWith(".meta") || file.Contains(".DS_Store"))
        //    {
        //        continue;
        //    }

        //    string md5 = md5file(file);
        //    string value = file.Replace(targetPath, string.Empty);
        //    sw.WriteLine(value + "|" + md5);
        //}
        //sw.Close();
        //fs.Close();
        //刷新编辑器
        //AssetDatabase.Refresh();

    }

    //[MenuItem("AssetBundle/Encrypt AssetBundle")]
    public static void EncryptAB()
    {
        string path = uiAssetBundlesPath + "ui/uiclientloadpanel.u3d";
        byte[] bytes = File.ReadAllBytes(path);
        bytes[0] += 28;
        //byte nullBytes = new byte();
        using(FileStream stream = File.Open(path, FileMode.OpenOrCreate))
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
    }
    
    //[MenuItem("AssetBundle/DeEncrypt AssetBundle")]
    public static void DeEncryptAB()
    {
        string path = uiAssetBundlesPath + "ui/uiclientloadpanel.u3d";
        byte[] bytes = File.ReadAllBytes(path);
        bytes[0] -= 28;
        
        using(FileStream stream = File.Open(path, FileMode.OpenOrCreate))
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
    }
    
    //[MenuItem("AssetBundle/LoadAssetBundle")]
    public static void LoadAssetBundle()
    {
        string path = uiAssetBundlesPath + "ui/uiclientloadpanel.u3d";
        byte[] bytes = File.ReadAllBytes(path);
        bytes[0] -= 28;
        AssetBundle.UnloadAllAssetBundles(true);
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
        assetBundle.LoadAsset<TextAsset>("UIHotResPanel");
        assetBundle.Unload(true);
    }
    
    //计算MD5
    //[MenuItem("AssetBundle/Calcuate Resources MD5")]
    public static void GetResourcesMD5()
    {
        string tempPath = EditorUtility.OpenFolderPanel("选取文件夹", EditorApplication.applicationPath, "");
        string newFilePath = tempPath + "/ResourcesMD5.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);
        paths.Clear();
        files.Clear();
        Recursive(tempPath);
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        EditorUtility.DisplayProgressBar("计算MD5值", "正在计算Asset MD5中...", 0f);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            if (file.EndsWith(".meta") || file.Contains(".manifest"))
            {
                continue;
            }
            string md5 = md5file(file);
            //string[] names = file.Split('/');
            //string value = names[names.Length -1];
            string value = file.Replace(tempPath + '/', string.Empty);
            sw.WriteLine(value + "#" + md5);
            EditorUtility.DisplayProgressBar("计算MD5值", "...", 1f * i / files.Count);
        }
        sw.Close();
        fs.Close();
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    /// <summary>
    /// 将某一文件夹中的资源进行分离打包，即把依赖资源分离出来打包
    /// </summary>
    [MenuItem("AssetBundle/Set Main AssetbundleName")]
    public static void SetMainAssetBundleName()
    {
        //string fullPath = Application.dataPath + "/Resources/UI/Prefabs/";    //将Assets/Prefab/文件夹下的所有预设进行打包

        //SetAssetBundleName(fullPath, true);
        SetPrefabsName();
        SetTextureName();
        SetFontName();
        SetChartName();
        SetScriptName();
    }

    /// <summary>
    /// 设置资源的资源包名称
    /// </summary>
    /// <param name="path">资源主路径</param>
    /// <param name="ContainDependences">资源包中是否包含依赖资源的标志位：true表示分离打包，false表示整体打包</param>
    static void SetAssetBundleName(string path, bool ContainDependences = false)
    {
        //ClearAssetBundlesName();    //先清楚之前设置过的AssetBundleName，避免产生不必要的资源也打包

        if (Directory.Exists(path))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(path);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    string assetName = fileInfo.FullName.Substring(path.Length);  //预设的Assetbundle名字，带上一级目录名称
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName = assetName;  //设置预设的AssetBundleName名称
                        //importer.SaveAndReimport();
                    }
                    //Debug.Log("主资源的路径：" + basePath);
                    if (ContainDependences)    //把依赖资源分离打包
                    {
                        //获得他们的所有依赖，不过AssetDatabase.GetDependencies返回的依赖是包含对象自己本身的
                        string[] dps = AssetDatabase.GetDependencies(basePath); //获取依赖的相对路径Assets/...
                        FNDebug.Log(string.Format("There are {0} dependencies!", dps.Length));
                        //遍历设置依赖资源的Assetbundle名称，用哈希Id作为依赖资源的名称
                        for (int j = 0; j < dps.Length; j++)
                        {
                            FNDebug.Log(dps[j]);
                            //要过滤掉依赖的自己本身和脚本文件，自己本身的名称已设置，而脚本不能打包
                            if (dps[j].Contains(assetName) || dps[j].Contains(".cs"))
                                continue;
                            else
                            {
                                AssetImporter importer2 = AssetImporter.GetAtPath(dps[j]);
                                string dpName = AssetDatabase.AssetPathToGUID(dps[j]);  //获取依赖资源的哈希ID
                                importer2.assetBundleName = "alldependencies/" + dpName;
                            }
                        }
                    }
                    
                }
            }

            EditorUtility.ClearProgressBar();   //清除进度条
        }
    }

    /// <summary>
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包 
    /// 因为只要设置了AssetBundleName的，都会进行打包，不论在什么目录下 
    /// </summary> 
    [MenuItem("AssetBundle/Clear All Assetbundle Name")]
    public static void ClearAssetBundlesName()
    {
        string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();

        EditorUtility.DisplayProgressBar("清除AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            EditorUtility.DisplayProgressBar("清除AssetName名称", "正在清除AssetName名称中...", 1f * j / oldAssetBundleNames.Length);
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void SetPrefabsName()
    {
        string fullPath = Application.dataPath + "/UIAsset/Prefabs/";    //将Assets/Prefab/文件夹下的所有预设进行打包
        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(fullPath);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    string assetName = fileInfo.FullName.Substring(fullPath.Length);  //预设的Assetbundle名字，带上一级目录名称
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName = $"ui/{assetName}.u3d";  //设置预设的AssetBundleName名称
                    }
                }
            }
            EditorUtility.ClearProgressBar();   //清除进度条
        }
    }

    public static void SetTextureName()
    {
        string fullPath = Application.dataPath + "/UIAsset/texture/";    //将Assets/Prefab/文件夹下的所有预设进行打包
        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(fullPath);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    string assetName = fileInfo.FullName.Substring(fullPath.Length);  //预设的Assetbundle名字，带上一级目录名称
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName = $"texture/{assetName}.u3d";  //设置预设的AssetBundleName名称
                    }
                }
            }
            EditorUtility.ClearProgressBar();   //清除进度条
        }
    }

    public static void SetFontName()
    {
        string fullPath = Application.dataPath + "/UIAsset/font/";    //将Assets/Prefab/文件夹下的所有预设进行打包
        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(fullPath);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    string assetName = fileInfo.FullName.Substring(fullPath.Length);  //预设的Assetbundle名字，带上一级目录名称
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName = $"font/{assetName}.u3d";  //设置预设的AssetBundleName名称
                    }
                }
            }
            EditorUtility.ClearProgressBar();   //清除进度条
        }
    }

    public static void SetChartName()
    {
        string fullPath = Application.dataPath + "/UIAsset/chart/";    //将Assets/Prefab/文件夹下的所有预设进行打包
        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(fullPath);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    string assetName = fileInfo.FullName.Substring(fullPath.Length);  //预设的Assetbundle名字，带上一级目录名称
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName = $"chart/{assetName}.u3d";  //设置预设的AssetBundleName名称
                    }
                }
            }
            EditorUtility.ClearProgressBar();   //清除进度条
        }

    }
    
    public static void SetScriptName()
    {
        string fullPath = Application.dataPath + "/UIAsset/ui/";    //将Assets/Prefab/文件夹下的所有预设进行打包
        if (Directory.Exists(fullPath))
        {
            string assetName = "UIClientLoadPanel";
            
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);   //显示进程加载条
            DirectoryInfo dir = new DirectoryInfo(fullPath);    //获取目录信息
            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);  //获取所有的文件信息
            for (var i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))   //判断去除掉扩展名为“.meta”的文件
                {
                    string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                    //string assetName = fileInfo.FullName.Substring(fullPath.Length);  //预设的Assetbundle名字，带上一级目录名称
                    //assetName = assetName.Substring(0, assetName.LastIndexOf('.')); //名称要去除扩展名
                    //assetName = assetName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
                    AssetImporter importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != assetName)
                    {
                        importer.assetBundleName =  $"ui/{assetName}.u3d";  //设置预设的AssetBundleName名称
                    }
                }
            }
            EditorUtility.ClearProgressBar();   //清除进度条
        }

    }

}



