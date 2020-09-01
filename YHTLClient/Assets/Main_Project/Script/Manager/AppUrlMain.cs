using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppUrlMain  {

    public static string mClientResURL
    {
        get
        {
            switch (Platform.mPlatformType)
            {
                case PlatformType.ANDROID: return Application.persistentDataPath + "/";
                case PlatformType.IOS: return Application.persistentDataPath + "/";
                default: return  Application.persistentDataPath + "/";
            }
        }
    }
    
    /// <summary>
    /// 中心服地址
    /// </summary>
    public static string centerUrl
    {
        get { return "http://ht.cslc.wgaore.com/kingapi"; }
    }

    /// <summary>
    /// 下载失败文件地址
    /// </summary>
    public static string mFailedListFileName = $"{CSConstant.ServerType}/FailedList.txt";
    
    /// <summary>
    /// 修复错误文件地址
    /// </summary>
    public static string mRepairListFileName = $"{CSConstant.ServerType}/RepairList.txt";

    /// <summary>
    /// 客户端资源路径
    /// </summary>
    public static string mClientResPath = Application.persistentDataPath + "/";

    /// <summary>
    /// 错误日志收集地址
    /// </summary>
    public const string errorUrl = "http://log-collect.cslc.wgaore.com/putlog.php";

    /// <summary>
    /// 错误日志收集地址2
    /// </summary>
    public const string errorUrl2 = "http://log-collect.cslc.wgaore.com/zhongyaolog.php";
    /// <summary>
    /// 报错收集日志3--收集 资源下载错误日志
    /// </summary>
    public const string errorUrl3 = "http://log-collect.cslc.wgaore.com/putlog.php";

    /// <summary>
    /// 服务器差异化资源地址
    /// </summary>
    public static string mServerResURL = "http://192.168.5.119/20190513/";


    /// <summary>
    /// CDN资源版本号
    /// </summary>
    public static string cdnVersion = string.Empty;

    /// <summary>
    /// 客户端本地版本地址
    /// </summary>
    public static string mClientVersionPath = $"{Application.persistentDataPath}/{CSConstant.ServerType}/Version.json";
    /// <summary>
    /// 客户端本地版本URL
    /// </summary>
    public static string mClientVersionURL
    {
        get
        {
#if UNITY_EDITOR
            return $"file:///{mClientVersionPath}";
#else
            return $"file://{mClientVersionPath}";
#endif
        }
    }
    
    /// <summary>
    /// 客户端本地资源列表URL
    /// </summary>
    public static string mClientDownResourceURL
    {
        get
        {
#if UNITY_EDITOR
            return $"file:///{mClientDownResourceFile}";
#else
            return $"file://{mClientDownResourceFile}";
#endif
        }
    }
    /// <summary>
    /// 客户端本地资源列表文件地址
    /// </summary>
    public static string mClientDownResourceFile
    {
        get { return $"{Application.persistentDataPath}/client/{CSConstant.ServerType}_ClientResource.bytes";}
    }

    /// <summary>
    /// 手机机型适配
    /// </summary>
    public static string getRatioAdapt
    {
        get
        {
            return "http://list.cslc.wgaore.com/clientlist/ResolutionRatioAdapt.txt";
        }
    }

    /// <summary>
    /// 首包资源列表地址
    /// </summary>
    public static string ResInApkPath
    {
        get
        {
            return Application.streamingAssetsPath + "/ResListInApk.bytes";
        }
    }

    /// <summary>
    /// 服务器版本地址
    /// </summary>
    public static string mServerVersionURL
    {
        get
        {
            return "http://192.168.4.65/tulongtest.json";
        }
    }
    
    /// <summary>
    /// 更新资源版本号
    /// </summary>
    public static Version updateVersion = new Version();


    /// <summary>
    /// 充值回调地址
    /// </summary>
    public static string rechargeUrl
    {
        get
        {
            string plantName = GetRechargeUrlString(CSConstant.platformid);

            return string.Format("http://pay.cslc.wgaore.com:9898/recharge/{0}", plantName);
        }
    }

    private static string GetRechargeUrlString(int id)
    {
        if (id == 1)
        {
            return "twgr";
        }

        return id.ToString();
    }
    
    /// <summary>
    /// 获取服务器列表
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string serverListUrl()
    {
        string platStr = "";
        
        if (CSConstant.platformid == (int) QuDaoPlatform.UnityEditor)
        {
            return "http://list.cslc.wgaore.com/clientlist/serverlist_ad_0.txt";
        }

        platStr = GetServerListPlat();
        platStr += CSConstant.platformid;
        return string.Format("http://list.cslc.wgaore.com/serverlist/serverlist_{0}.txt", platStr);
    }
    
    public static string GetServerListPlat()
    {
        if (CSConstant.platformid > 5000)
        {
            return "ios_";
        }
        else
        {
            return "ad_";
        }
    }
}
