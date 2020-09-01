using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
public static class FileCommon 
{
    public static Dictionary<string, bool> ResLoadPreDic = new Dictionary<string, bool>();
    public static AssetBundleManifest AssetBundleManifestObject;
    public static void ResetResLoadPre()
    {
        ResLoadPreDic.Clear();
        string path = Application.dataPath.Replace("Assets", "ResLoadPre.txt");
        if (File.Exists(path))
        {
            string old = ReadToEnd(path);
            string[] strs = old.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(int i=0;i<strs.Length;i++)
            {
                string s = strs[i];
                if(!string.IsNullOrEmpty(s))
                {
                    if(!ResLoadPreDic.ContainsKey(s))
                    {
                        ResLoadPreDic.Add(s,true);
                    }
                }
            }
        }
        string p = Application.dataPath.Replace("Client/Branch/ClientAndroid/Assets", "Data/Branch/CurrentUseData/wzcq_android/Android/Android");
        AssetBundle ab = AssetBundle.LoadFromFile(p);
        AssetBundleManifestObject = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    public static void SaveUIResLoadPre(string assetBundleName)
    {
        string[] dependencies = AssetBundleManifestObject.GetAllDependencies(assetBundleName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            SaveResLoadPre("Android/"+dependencies[i]);
        }
    }

    public static void SaveResLoadPre(string str)
    {
        if (!ResLoadPreDic.ContainsKey(str))
        {
            ResLoadPreDic.Add(str, true);
            FileCommon.Write(Application.dataPath.Replace("Assets", "ResLoadPre.txt"), str+"\r\n");
        }
    }

    public static void Write(string path, string content, bool append = true)
    {
        string old = ReadToEnd(path);
        DetectCreateDirectory(path);
        using (StreamWriter sw = new StreamWriter(path, append, System.Text.Encoding.UTF8))
        {
            sw.Write(content);
        }
    }

    public static string ReadToEnd(string path)
    {
        string content = "";
        if (!File.Exists(path)) return content;
        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fileStream, System.Text.Encoding.UTF8))
            {
                content = sr.ReadToEnd();
            }
        }
        return content;
    }

    public static void DetectCreateDirectory(string path)
    {
        int index = path.LastIndexOf(".");
        if (index == -1 || path.Substring(index).Contains("/"))
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        else
        {
            if (!Directory.Exists(path + "/../"))
                Directory.CreateDirectory(path + "/../");
        }
    }
}
