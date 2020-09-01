using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public static class QuDaoConstant
{
    //qudao功能的配置列表
    public static QuDaoConfig qudaoConfigInfo;

    public static QuDaoConfig GetPlatformData()
    {
        if (qudaoConfigInfo == null)
        {
            qudaoConfigInfo = new QuDaoConfig();
            qudaoConfigInfo.SetConfigChannelID();
        }
        return qudaoConfigInfo;
    }
    
    #region 服务器列表和配置文件控制变量
    
    private static bool _isOpenVoice = true;//是否开启语音
    private static bool _isOpenRecharge = true;//是否开启充值
    private static bool _isOpenTranslate = true;//是否开启语音翻译
    private static bool _isOpenCheckVersion = false;//是否开启版本检测
    private static bool _isOpenFeedBack = true; // 设置界面,是否开启问题反馈
    private static bool _isOpenPush = true;// 设置界面,是否开启推送
    private static bool _isChannelCloseRegister = false;//渠道是否关闭注册
    
    public static bool OpenVoice
    {
        get { return _isOpenVoice; }
        set { _isOpenVoice = value; }
    }
    
    public static bool OpenRecharge
    {
        get { return _isOpenRecharge; }
        set { _isOpenRecharge = value; }
    }
    
    public static bool OpenTranslate
    {
        get { return _isOpenTranslate; }
        set { _isOpenTranslate = value; }
    }
    
    public static bool OpenCheckVersion
    {
        get { return _isOpenCheckVersion; }
        set { _isOpenCheckVersion = value; }
    }
    
    public static bool OpenFeedBack
    {
        get { return _isOpenFeedBack; }
        set { _isOpenFeedBack = value; }
    }
    
    public static bool OpenPush
    {
        get { return _isOpenPush; }
        set { _isOpenPush = value; }
    }
    
    public static bool ChannelCloseRegister
    {
        get { return _isChannelCloseRegister; }
        set { _isChannelCloseRegister = value; }
    }
    #endregion
    
    public static bool isEditorMode()
    {
        return GetPlatformData().platformID == (int)QuDaoPlatform.UnityEditor
            || GetPlatformData().platformID == (int) QuDaoPlatform.AndroidOutNetTest;
    }
}

public enum LoginCode
{
    OneParameter,//登入的时候只传入1个参数的时候
    MultiParameter,//登入的时候传入多个参数,如userId,Token,ext
    //Special,//需要特殊处理登入界面的qudao
}

public enum RequestCode
{
    Normal,//正常请求登入
    Special,//特殊请求登入
    DirectConnection,//直接连接
}

public enum PayCode
{
    Normal,//直接调用qudao的zhifu方法
    Special,//往服务端请求一个sign,获得成功后再调用qudaozhifu方法
}

public enum ExitCode
{
    Normal = 0,//调用游戏内的退出界面
    QUDAO,//调用android内logout方法
    NoDo,//android做重写的返回键的方法
}

//用于备注
public enum QuDaoPlatform
{
    UnityEditor = 0,
    //贪玩高热
    TWGR = 1,
    //出包测试
    AndroidOutNetTest = 101,
}

public class QuDaoConfig
{
    public int platformID = 0;                                           //渠道ID
    public LoginCode loginCode = LoginCode.MultiParameter;               //登入方式类型
    public RequestCode requestCode = RequestCode.DirectConnection;       //服务器请求类型
    public PayCode payCode = PayCode.Normal;                             //zhifu方式类型
    public ExitCode exitCode = ExitCode.Normal;                          //退出登入方式
    public bool submitData = true;                                       //是否上传数据
    public bool switchAccount = true;                                    //服务器选择界面,是否开启切换账号按钮(调用qudao内的changeAccount()方法)
    public bool exitAccount = true;                                     //返回登入是否退出账号(调用qudao内的changeAccount()方法)
    
    public QuDaoConfig()
    {
#if UNITY_EDITOR || EDITORMODE
        submitData = false;
        switchAccount = false;
        exitAccount = false;
#endif
    }

    //设置渠道ID
    public void SetConfigChannelID()
    {
        platformID = QuDaoInterface.Instance.GetChannelId();
    }

    public void SetConfig(Dictionary<string, object> jsonDic)
    {
        if (jsonDic.ContainsKey("loginCode"))
        {
            int value = 1;
            int.TryParse(jsonDic["loginCode"].ToString(), out value);
            SetConfigLoginCode(value);
        }
        if (jsonDic.ContainsKey("requestCode"))
        {
            int value = 2;
            int.TryParse(jsonDic["requestCode"].ToString(), out value);
            SetConfigRequestCode(value);
        }
        if (jsonDic.ContainsKey("payCode"))
        {
            int value = 0;
            int.TryParse(jsonDic["payCode"].ToString(), out value);
            SetConfigPayCode(value);
        }
        if (jsonDic.ContainsKey("exitCode"))
        {
            int value = 0;
            int.TryParse(jsonDic["exitCode"].ToString(), out value);
            SetConfigExitCode(value);
        }
        if (jsonDic.ContainsKey("submitData"))
        {
            SetConfigSubmitData(jsonDic["submitData"].ToString());
        }
        if (jsonDic.ContainsKey("switchAccount"))
        {
            SetConfigSwitchAccount(jsonDic["switchAccount"].ToString());
        }
        if (jsonDic.ContainsKey("exitAccount"))
        {
            SetConfigExitAccount(jsonDic["exitAccount"].ToString());
        }
    }

    //设置登录时传入参数
    public void SetConfigLoginCode(int loginCode)
    {
        this.loginCode = (LoginCode)loginCode;
    }

    //设置登录请求方式
    public void SetConfigRequestCode(int requestCode)
    {
        this.requestCode = (RequestCode)requestCode;
    }

    //设置支付请求方式
    public void SetConfigPayCode(int payCode)
    {
        this.payCode = (PayCode)payCode;
    }

    //设置退出调用方式
    public void SetConfigExitCode(int exitCode)
    {
        this.exitCode = (ExitCode)exitCode;
    }

    //是否上报数据
    public void SetConfigSubmitData(string submitData)
    {
        this.submitData = submitData.Equals("true");
    }

    //是否开启登录页用户退出按钮
    public void SetConfigSwitchAccount(string switchAccount)
    {
        this.switchAccount = switchAccount.Equals("true");
    }

    //返回登入是否退出账号(调用qudao内的changeAccount()方法)
    public void SetConfigExitAccount(string exitAccount)
    {
        this.exitAccount = exitAccount.Equals("true");
    }
}
