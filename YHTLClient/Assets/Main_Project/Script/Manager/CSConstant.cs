using UnityEngine;

public class CSConstant
{
    public static float PixelRatio;       // 游戏分辨率
    public static Vector2 ContentSize = new Vector2(1136, 640);
    //腾讯语音AppId
    public const int YvTMGSDKAppId = 1400207539;  //战神蚩尤 	1400389069   现在没钱，不能替换   语音版本SDK v2.5.4
    public const string YvTMGSDKKey = "8kt3htYqg4vJI9rp";  //战神蚩尤 		SDv8o5rMLifA3RkM
    public static int pixelzoom = 0;
    public static float SpecialAdaptRadio = 1.0f;
    public static string localVersion = "1.0.0.0";
    public static string resVersionInApk = "1.0.0.0";
    public static int mMapId = 0;
    public static int mSeverId = 0;//服务器索引ID
    public static int mOnlyServerId = 0;//服务器唯一ID
    public static string ServerType = "0";//选择服务器列表，，（普通服或者先行服）
    public static string loginName = "";//记录登录id
    public static int RoleCount = 0;//玩家角色数量

    public static string LastServerType;//上次登录的服务器类型 -----表格数据加载完成后记录
    public static bool isCanCrossScene = false;
    public static bool IsLanuchMainPlayer = false;
    public static bool isMainPlayerMoving = false;
    public static float lastStandTimeUnloadAsset = 0;
    public static int mapImg = 0;
    public static float mPoolRealseApseat = 1.0f;
    public static Vector2 mTerrainSize = Vector2.zero;
    public static bool haveBindFunc = false;//是否有绑定手机功能
    public static string phoneModel = string.Empty; //手机型号
    public static string extToken = "";//用来接收服务器传下的参数,用作付费等作用
    public static int mHeartbeatNum = 0;           //心跳包统计数
    public const float UpdateInterval = 0.5f;
    public static string MainPlayerName = "主角尚未加载";//玩家名字，由热更工程传递进来,,用于后台日志
    public static Vector2 viewRange = new Vector2(11, 9);

    //平台id，，一个平台有多个渠道
    public static int plantformBigId;
    #region ios IOS相关功能常量

    public static bool haveWhitePaperFunc = false; //有白名单功能
    public static bool isWhitePaper = false; //是否是白名单
    public static bool AppIsTongGuo = false; //是否通过审核
    public static bool isHuanJue = true; //是否更换选角色
    
    /// <summary>
    /// 没有开启充值功能
    /// </summary>
    public static bool IosNotOpenRecharge()
    {
        return Platform.mPlatformType == PlatformType.IOS && !QuDaoConstant.OpenRecharge;
    }
    #endregion

    public static int platformid
    {
        get
        {
#if UNITY_EDITOR
            return QuDaoInterface.Instance.GetChannelId();
#else
            if (QuDaoConstant.GetPlatformData() != null)
            {
                return QuDaoConstant.GetPlatformData().platformID;
            }
            else
            {
                return 1;
            }
#endif
        }
    }
}
