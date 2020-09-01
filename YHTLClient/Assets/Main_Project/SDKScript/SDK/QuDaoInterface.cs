using Main_Project.SDKScript.SDK;

public abstract class QuDaoInterface
{
    private static QuDaoInterface _instance;

    public static QuDaoInterface Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR || UNITY_STANDLONE
                _instance = new QuDaoInterfaceDefault();
#elif UNITY_ANDROID
                _instance = new SDKInterfaceAndroid();
#elif UNITY_IOS
				_instance = new QuDaoInterfaceIOS();
#endif
            }

            return _instance;
        }
    }

    //登录
    public abstract void Login();

    public abstract void SlientLogin(); //IOS静默登陆

    public abstract void Login(string info);

    //切换帐号
    public abstract void ChangeAccount();

    /// <summary>
    /// 返回登录  调用sdk退出账号，， 角色页面按返回也调用次接口
    /// </summary>
    public abstract void Logout();

    //更新跳转appstore
    public abstract void JumpAppStore();

    /// <summary>
    ///     请求应用宝数据
    /// </summary>
    public abstract string GetYSDKRefreshData();

    //隐藏悬浮窗
    public abstract void SetFloatWindowState(bool isShow);

    /// <summary>
    /// 玩家数据收集
    /// </summary>
    /// <param name="extraType">1;//选择服务器,2;//创建角色,3;//进入游戏,4;//等级提升,5;//退出游戏</param>
    public abstract void SubmitGameData(int extraType, ExtraGameData gameData);

    public abstract void SubmitBuyData(BuyItemInfo data);

    public abstract string GetVersion();

    //调用SDKzhifu界面
    public abstract void FuKuan(QuDaoPayParams data);

    public abstract string GetServerConfigData();

    public abstract string GetServerVersionURL();

    /// <summary>
    ///     登录二次验证
    /// </summary>
    public abstract void VerificationPlayerInfo(string host);

    public abstract void CopyFile(string sourcePath, string targetPath);

    public abstract string GetAndroidID();

    public abstract void RestartGame();

    public abstract void BindPhone();

    //获取手机型号
    public abstract string GetPhoneType();

    public abstract void SendToken(string token);

    public abstract string GetPushClientid();
    public abstract bool SetPushTags(string tag);
    public abstract void turnOffPush();
    public abstract void turnOnPush();
    public abstract bool GetPushTurnedOnState();

    /// <summary>
    /// 得到手机imei
    /// </summary>
    /// <returns></returns>
    public abstract string GetDeviceId();

    /// <summary>
    ///     得到默认应用包名(打包时候,自己设置的包名,用来读取对应的配置,避免由于渠道二次打包导致包名变换)
    /// </summary>
    /// <returns></returns>
    public abstract string GetDefaultPackageName();

    /// <summary>
    ///     得到应用包名
    /// </summary>
    /// <returns></returns>
    public abstract string GetPackageName();

    /// <summary>
    ///     得到手机电量
    /// </summary>
    /// <returns></returns>
    public abstract int GetBattery();

    /// <summary>
    ///     手机当前语言
    /// </summary>
    /// <returns></returns>
    public abstract string getSystemLanguage();

    /// <summary>
    ///     手机系统版本
    /// </summary>
    /// <returns></returns>
    public abstract string getSystemVersion();

    /// <summary>
    ///     手机型号
    /// </summary>
    /// <returns></returns>
    public abstract string getSystemModel();

    /// <summary>
    ///     手机厂商
    /// </summary>
    /// <returns></returns>
    public abstract string getDeviceBrand();

    /// <summary>
    ///     获得内存剩余
    /// </summary>
    /// <returns></returns>
    public abstract long systemAvaialbeMemorySize();

    /// <summary>
    ///     获取应用占用内存
    /// </summary>
    /// <returns></returns>
    public abstract long getProcessMemoryInfo();

    /// <summary>
    ///     获取应用内存界限
    /// </summary>
    /// <returns></returns>
    public abstract long getMemoryThreshold();

    /// <summary>
    ///     获取当前是否处于低内存状态
    /// </summary>
    /// <returns></returns>
    public abstract bool getIsLowMemoryState();

    /// <summary>
    ///     获得距离低内存状态还有多少内存可用
    /// </summary>
    /// <returns></returns>
    public abstract long getMemoryLimitResidue();

    public abstract void FinishGame();

    /// <summary>
    ///     获取渠道ID
    /// </summary>
    public abstract int GetChannelId();

    public abstract void CopyTextToClipboard(string text);

    public abstract string GetUnityCPath(string name);

    public abstract int GetScreenDisplayCutout();

    public abstract string GetDefaultLoginServerType();
    
    //绑定身份证成功回调
    public delBindPhone BindIDCardSuccess;
    
    #region 登录/登出回调

    public delegate void LoginSucHandler(LoginResult data);

    public delegate void LoginMultiSucHandler(LoginInfo data);

    public delegate void LoginServerSuc(LoginInfo data);

    public LoginSucHandler OnLoginSuc; //kaiying登录成功登录成功回调
    public LoginMultiSucHandler OnMultiLoginSuc; //多参数登录成功回调  

    public LoginMultiSucHandler OnIOSSDKLoginSuc; //IOS sdk登录成功

    public LoginServerSuc OnLoginServerSuc; //登录服务器校验成功

    #endregion

    #region 资源，下载回调函数

    public delegate void delMovingFile(float percent);

    public delegate void delDownloadingAPK(float percent);

    public delegate void delCopySuccess(long totalSize);

    public delCopySuccess OnCopySuc; //复制资源完成回调

    #endregion

    #region 绑定手机成功回调

    public delegate void delBindPhone(bool isBind);

    public delBindPhone BindPhoneSuccess;

    #endregion
}

