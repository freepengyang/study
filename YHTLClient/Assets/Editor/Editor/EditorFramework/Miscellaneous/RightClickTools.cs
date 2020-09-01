using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
using System.Collections.Generic;

public class RightClickTools : SelectionBase
{
    static List<string> selectPathList = new List<string>();

    [MenuItem("Assets/Tools/SVN日志")]
    public static void SVNLogRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.LogSVN(selectPathList);
    }

    [MenuItem("Assets/Tools/SVN更新")]
    public static void SVNUpdateRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.UpdateSVN(selectPathList,Application.dataPath.Replace("Client/Branch/Client/Assets", "Data/Branch/CurrentUseData/wzcq_20170110/"));
    }

    [MenuItem("Assets/Tools/SVN提交")]
    public static void SVNCommitRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.CommitToSVN(selectPathList);
    }

    [MenuItem("Assets/Tools/SVN还原")]
    public static void SVNRevertRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.RevertSVN(selectPathList);
    }

    [MenuItem("Assets/Tools/SVN解决冲突")]
    public static void SVNResolveRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.ResolveSVN(selectPathList);
    }

    [MenuItem("Assets/Tools/SVN追溯")]
    public static void SVNBlameRightClick()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        FileUtility.BlameSVN(selectPathList);
    }

    [MenuItem("Assets/Tools/SVN更新Proto和Table文件夹")]
    public static void SVNUpdateProtoAndTableRightClick()
    {
        FileUtility.UpdateProtoAndTable();
    }

    [MenuItem("Assets/Tools/SVN提交proto和Table文件夹")]
    public static void SVNCommitProtoAndTableRightClick()
    {
        FileUtility.CommitProtoAndTable();
    }

    [MenuItem("Assets/Tools/打开Proto文件夹")]
    public static void OpenProtoRightClick()
    {
        string protoPath = Application.dataPath + "/../../proto";
        FileUtility.Open(protoPath);
    }

    [MenuItem("Assets/Tools/打开Table文件夹")]
    public static void OpenTableRightClick()
    {
        string tablePath = Application.dataPath + "/../../table/workbook";
        FileUtility.Open(tablePath);
    }


    //[MenuItem("Assets/Tools/Copy")]
    //public static void CopyRightClick()
    //{
    //    FileUtility.CopyFile();
    //}

    //[MenuItem("Assets/Tools/Paste")]
    //public static void PasteRightClick()
    //{
    //    FileUtility.PasteFile();
    //}

    [MenuItem("Assets/Tools/UpdateToClassLibrary")]
    public static void UpdateToClassLibrary()
    {
        selectPathList.Clear();
        FileUtility.GetSelectAssetPaths(selectPathList);
        List<string> fileList = new List<string>();
        for(int i=0;i<selectPathList.Count;i++)
        {
            FileUtility.GetDeepAssetPaths(Application.dataPath.Replace("Assets","")+selectPathList[i], fileList);
        }

        FileUtility.UpdateToClassLibrary(fileList);
    }

    [MenuItem("Assets/Tools/更新ClassLibrary")]
    public static void UpdateFromClassLibrary()
    {
        FileUtility.UpdateFromClassLibrary();
    }
}
