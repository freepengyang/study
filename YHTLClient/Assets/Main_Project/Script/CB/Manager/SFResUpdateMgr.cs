/*************************************************************
** File: SFResUpdateMgr.cs
** Author: jiabao
** Time: 2017.5.26
*************************************************************/

using System.Net;
using System.IO;
using TABLE;
using UnityEngine;

public interface ISFResUpdateMgr
{
    bool CheckIsNeedDownload(string relatePath);
    void AddToCompleteQueue(Resource res);
}

public class Resource : HttpDownload
{
    public enum ResourceType
    {
        None = -1,
        CommonResources,
        Others,
    }

    //private RESOURCELIST _resourcelist;

    private string relatePath;
    public string RelatePath
    {
        get { return relatePath; }
    }
    
    //存储在本地的实际地址
    private string localRelatePath;
    public string LocalRelatePath
    {
        get
        {
            return localRelatePath;
        }
    }

    private int _byteNum;
    public int ByteNum
    {
        get { return _byteNum; }
        set { _byteNum = value; }
    }

    private int needPreDownload = -1;

    public bool NeedPreDownload
    {
        get
        {
            if (needPreDownload < 0)
            {
                return relatePath.Contains("ScaleMap") ||
                       relatePath.Contains("byte") ||
                       relatePath.Contains("Android/") ||
                       relatePath.Contains("iOS/") ||
                       relatePath.Contains("ResourceRes");
            }
            else
            {
                return needPreDownload == 1;
            }
        }
    }

    private string _md5;
    public string mMd5
    {
        get { return _md5; }
    }
    
    public void Init(RESOURCELIST resourcelist)
    {
        if (resourcelist == null) return;
        relatePath = resourcelist.name;
        localRelatePath = $"{CSConstant.ServerType}/{RelatePath}";
        _md5 = resourcelist.md5;
        _byteNum = resourcelist.length;
        needPreDownload = int.Parse(resourcelist.resType);
    }

    public void Dispose()
    {
        needPreDownload = -1;
    }

    private int GetResourceType(string relatePath)
    {
        if (string.IsNullOrEmpty(relatePath))
        {
            return -1;
        }
        else
        {
            if (relatePath.Contains("Table") ||
                relatePath.Contains("Android/") ||
                relatePath.Contains("iOS/"))
                return 1;
            else
                return 0;
        }
    }

    public override string url
    {
        get
        {
            return $"{SFOut.URL_mServerResURL}{RelatePath}{SFOut.CdnVersion}";
        }
    }

    public override string localPath
    {
        get { return $"{SFOut.URL_mClientResPath}{LocalRelatePath}"; }
    }

    protected override void FinishDownload()
    {
        SFOut.IResUpdateMgr.AddToCompleteQueue(this);
        base.FinishDownload();
    }

    protected override void Destroy()
    {
    }
}


