/*************************************************************************
** File: AppUrl.cs
** Author: jiabao
** Time: 2019.3.19
*************************************************************************/

using UnityEngine;

public class AppUrl
{
    /// <summary>
    /// 获取白名单IP地址
    /// </summary>
    public static string blackUrl
    {
        get { return "http://ht.cslc.wgaore.com/kingapi?action=checkBlack"; }
    }


    public static string VIPUrl
    {
        get { return string.Format("http://ht.cslc.wgaore.com/extapi?action=vipSet&pid={0}", Constant.platformid); }
    }

    /// <summary>
    /// 登录界面版号地址
    /// </summary>
    public static string PlateNumbetUrl
    {
        get { return string.Format("{0}/agreement.txt", Application.streamingAssetsPath); }
    }

    /// <summary>
    /// 游戏LOGO地址	
    /// </summary>
    public static string GameLogoUrl
    {
        get
        {
#if UNITY_EDITOR
            return string.Format("file:///{0}/logo.png", Application.streamingAssetsPath);
#elif UNITY_IPHONE
             return string.Format("file://{0}/logo.png", Application.streamingAssetsPath);
#else
            return string.Format("{0}/logo.png", Application.streamingAssetsPath);
#endif
        }
    }

    /// <summary>
    /// 获取公告内容地址
    /// </summary>
    public static string noticeUrl
    {
#if UNITY_EDITOR
        get { return string.Format("http://list.cslc.wgaore.com/notelist/notelist_1.txt", Constant.platformid); }
#else
		get { return string.Format("http://list.cslc.wgaore.com/notelist/notelist_{0}.txt", Constant.platformid); }
#endif
    }
    
    /// <summary>
    /// 获取登录页面弹框内容
    /// </summary>
    public static string LoadingUrl
    {
#if UNITY_EDITOR
        get { return string.Format("http://list.cslc.wgaore.com/notelist/welcomenotelist_1.txt", Constant.platformid); }
#else
		get { return string.Format("http://list.cslc.wgaore.com/notelist/welcomenotelist_{0}.txt", Constant.platformid); }
#endif
    }

    /// <summary>
    /// 获取协议内容地址
    /// </summary>
    public static string agreementUrl
    {
        get
        {
#if UNITY_EDITOR
            return string.Format("http://list.cslc.wgaore.com/agreementlist/agreementlist_1.txt");
#else
			return string.Format("http://list.cslc.wgaore.com/agreementlist/agreementlist_{0}.txt", Constant.platformid);
#endif
        }
    }
    
    /// <summary>
    /// 登录域名，需要二次验证的走验证域名
    /// </summary>
    public static string platformLoginUrl
    {
        get
        {
            /*if (QuDaoConstant.GetPlatformData().requestCode == RequestCode.Normal)
                return "http://login.zt.17tanwan.com:3000/login/{0}";
            else
                return "http://cz.zt.17tanwan.com:3000/login/{0}";*/
            return "";
        }
    }

    /// <summary>
    /// 特殊SDK充值地址--目前没有特殊充值，，，以后有需求，在修改地址
    /// </summary>
    public static string GetPaySignUrl
    {
        get
        {
            //return string.Format("http://cz.zt.361757.com:3000/order/{0}", Constant.platformid);
            return "";
        }
    }
}