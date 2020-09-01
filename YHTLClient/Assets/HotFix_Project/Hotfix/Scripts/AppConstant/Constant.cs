using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

//---------------------常用变量定义-----------------------//

// 临时存一些后缀。地址，。等变量
//-------------------------------------------------------//


public class Constant
{
    public static string mServerName = "";//登录服务器名称
    public static string loginServerId = "";//记录登录登录时输入的serverId
    public static string mToken = string.Empty;
    public static long time = 0;
    public static string sign = "";
    public static string loginSign = "";//登录code（code是临时记录登录信息,K7977专用）
    public static string loginType = "";//登录类型(在yinyongbao中存在qq，登录方式)
    public static string loginCallBack = "";//登入回调信息
    public static string loginExt = "";//登入的額外信息
    public static string gameID = "";//游戏ID   gameID
    public static string extToken = "";//用来接收服务器传下的参数,用作付费等作用
    public static int AddValue = 8000;

    public static float mspeed = 0.237f;
    public static bool IsChangeLineConnect = false;//识别是否切线断线
    public static long mRoleId = 0;
    public static bool IsChangeLine = false;
    public static string mWhiteListIp = string.Empty;
    public static ServerType mCurServer = ServerType.GameServer;//默认是游戏服


    public static bool isBindPhone = false;//是否绑定手机
    public static List<int> ShowTipsOnceList = new List<int>(); //提示框勾选下次不再提示，则本次登录不再显示
    public static bool IsWeddingStarTips = false;
    public static bool IsWeddingCeremonyTips = false;
    public const float UpdateInterval = 0.5f;
    public static string phoneModel = string.Empty;//手机型号


    public static bool IsHaveNotice = false;

    public static bool isAccountException = false;

    

    public static bool isOpenOnlineServer = false;//在线客服开关

    //渠道id
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

#region 聊天常用相关设置

    /// <summary>
    /// 聊天
    /// </summary>
    public static int mChatMaxCount = 50;
    public static int mChatPrivateMaxCount = 30;

    public static bool autoPlay_World = false;      //自动播放世界语音
    public static bool autoPlay_Team = true;        //自动播放'队伍语音
    public static bool autoPlay_Guild = true;      //自动播放家族语音
    public static bool autoPlay_Private = true;   //自动播放私聊语音
    public static bool autoPlay_Country = true;   //自动播放国家语音
    public static bool autoPlay_FuJin = true;   //自动播放附近语音
    public static bool autoPlay_ColorWorld = true;   //自动播放彩色世界语音

    public static bool showWorldMsg = true;        //显示世界聊天信息
    public static bool showTeamMsg = true;         //显示队伍聊天信息
    public static bool showGuildMsg = true;         //显示家族聊天信息
    public static bool showNearMsg = true;         //显示附近聊天信息
    public static bool showPrivateMsg = true;      //显示私聊聊天信息

    //是否显示彩世提示
    public static bool isShowColoursWorld = true;
    #endregion


    public static int mHeartbeatNum = 0;           //心跳包统计数

    public static Ping gamePing = null;

    //public static Vector2 viewRange = new Vector2(11, 9);


    public static PositionChangeReason ChangeSceneReason = PositionChangeReason.MoveTooQuick;

    public const int CONST_SKILL_SHORTCUT_LENGTH = 6;


    #region 实名认证 和 邀请码
    /// <summary>
    /// 是否输入过实名信息
    /// </summary>
    ///
    private static bool _isIdCardNumberEntered = false;
    public static bool isIdCardNumberEntered
    {
        get { return _isIdCardNumberEntered; }
        set
        {
            _isIdCardNumberEntered = value;
        }
    }

    /// <summary>
    /// 是否成年
    /// </summary>
    public static bool isOver18 = false;
    /// <summary>
    /// 实名认证类型，，0：不验证    1：渠道sdk验证   2：游戏验证
    /// </summary>
    public static int RealNameSystemType = 0;
    /// <summary>
    /// 是否开启邀请码验证
    /// </summary>
    public static bool isOpenInviteCode = false;


    #endregion
}
