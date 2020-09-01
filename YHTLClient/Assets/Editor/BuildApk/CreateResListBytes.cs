using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Google.Protobuf;
using TABLE;
using UnityEditor;
using UnityEngine;

public class CreateResListBytes
{

    //[MenuItem("Tools/Res/Create ResourceList Bytes")]
    static void CreateResListToBytes()
    {
        string fileStr = File.ReadAllText(APKEnvironmentPath.RESOURCETOOL_PATH + "/ResourceList.txt");
        if (string.IsNullOrEmpty(fileStr)) return;
        string[] strArr = fileStr.Split('\n');
        TABLE.RESOURCELISTARRAY reslistarray = new RESOURCELISTARRAY();
        
        
        for (var i = 0; i < strArr.Length; i++)
        {
            string str = strArr[i];
            if (string.IsNullOrEmpty(str)) continue;
            str = str.Replace("\r", "");
            string[] data = str.Split('#');
            if (data.Length < 4) return;
            
            reslistarray.rows.Add(new RESOURCELIST()
            {
                name = data[0],
                length = int.Parse(data[1]),
                resType = data[2],
                md5 = data[3]
            });
        }
        
        byte[] resBytes = reslistarray.ToByteArray();
        string path = APKEnvironmentPath.ZT_ANDROID_PATH + "/ResourceList.bytes";
        if(File.Exists(path))
            File.Delete(path);
        File.WriteAllBytes(path, resBytes);
        
        UnityEngine.Debug.Log("卐  Create ResourceList Bytes   卍");
    }
    
    //[MenuItem("Tools/Res/Create ResListInApk Bytes")]
    static void CreateResListInApkToBytes()
    {
        string fileStr = File.ReadAllText(APKEnvironmentPath.RESOURCETOOL_PATH + "/ResListInApk.txt");
        if (string.IsNullOrEmpty(fileStr)) return;
        string[] strArr = fileStr.Split('\n');
        TABLE.RESLISTANDROIDARRAY reslistarray = new RESLISTANDROIDARRAY();
        
        
        for (var i = 0; i < strArr.Length; i++)
        {
            string str = strArr[i];
            if (string.IsNullOrEmpty(str)) continue;
            str = str.Replace("\r", "");
            string[] data = str.Split('#');
            if (data.Length < 2) continue;
            
            reslistarray.rows.Add(new RESLISTANDROID()
            {
                name = data[0],
                md5 = data[1]
            });
        }
        
        byte[] resBytes = reslistarray.ToByteArray();
        string path = APKEnvironmentPath.RESOURCE_PATH + "/ResListInApk.bytes";
        if(File.Exists(path))
                    File.Delete(path);
        File.WriteAllBytes(path, resBytes);
        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("卐  Create ResListInApk Bytes   卍");
    }
    
    //[MenuItem("Tools/Res/Create ResListInApk ResourceList Bytes")]
    public static void CrateAllResBytes()
    {
        CreateResListToBytes();
        CreateResListInApkToBytes();
    }
    
    //[MenuItem("Tools/Res/Create ResListInApk txt")]
    static void CreateResListInApkToTxt()
    {
        StartPython("-Apk");
    }
    
    //[MenuItem("Tools/Res/Create ResourceList txt")]
    static void CreateResourceListToTxt()
    {
        StartPython("-Res");
    }

    //[MenuItem("Tools/Res/Create ResourceList and ResListInApk")]
    public static void CreateAllToTxt()
    {
        StartPython("-A");
    }

    private static void StartPython(string command)
    {
        string fileDirectory = APKEnvironmentPath.RESOURCETOOL_PATH;
        string defaultDirectory = Directory.GetCurrentDirectory();

        if (string.IsNullOrEmpty(fileDirectory))
        {
            UnityEngine.Debug.LogError($"卐  CreateTxt {fileDirectory} no exits  卍");
            return;
        }
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fileDirectory);
            fileDirectory = directoryInfo.FullName;
            Directory.SetCurrentDirectory(fileDirectory);
            ProcessHelper.Run("python", "ResourceTools.py " + command, fileDirectory, true);

            Directory.SetCurrentDirectory(defaultDirectory);

            UnityEngine.Debug.Log("卐  CreateTxt Success   卍");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(ex);
            Directory.SetCurrentDirectory(defaultDirectory);
        }
    }
}
