
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.XGame.NetWork;

/// <summary>
/// 数据协议回调及类型注册类
/// </summary>
public class NetParam
{
    public Google.Protobuf.MessageParser Parser;
    public Action<System.Object> mfun;
    //private Type type;
    //private object onRec_RoomInfoMsg;

    public NetParam(Google.Protobuf.MessageParser cParser, Action<System.Object> fun)
    {
        this.mfun = fun;
        Parser = cParser;
    }
}

/// <summary>
/// 网络数据管理类
/// </summary>
public class NetDataMgr
{
    static NetDataMgr m_instance;
    public static NetDataMgr Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new NetDataMgr();
            }
            return m_instance;
        }
    }

    public Dictionary<int, NetParam> Mrgs = new Dictionary<int, NetParam>();
    //private bool isGetInfo = false;
    public void Init()
    {
        #region 登陆初始化基础数据
        // 进行添加协议及回调信息

        // 心跳接口处理
        //mMrgs.Add(ServiceNo.s2c_heart_check, new NetParam(typeof(s2c_heart_check), OnHeartCheck));
        // 统一错误接收处理
        // mMrgs.Add(ServiceNo.s2c_error_msg, new NetParam(typeof(s2c_error_msg), OnErrorMsg));
        // 统一属性接收处理
        #endregion

    }

    /// <summary>热更跨域枚举不支持改成Int</summary>
    /// <param name="res"></param>
    /// <param name="param"></param>
    public static void AddMessage(int res, Google.Protobuf.MessageParser Parser, Action<System.Object> mfun)
    {
        NetParam param = new NetParam(Parser, mfun);
        NetDataMgr.Instance.Mrgs.Add(res, param);

        param = null;
    }

    /// <summary>热更跨域枚举不支持改成Int</summary>
    /// <param name="res"></param>
    /// <param name="param"></param>
    public static void AddMessage(object res, NetParam param)
    {
        NetDataMgr.Instance.Mrgs.Add((int)res, param);

        param = null;
    }

    private void OnErrorMsg(object obj)
    {
        throw new NotImplementedException();
    }

    #region 登陆初始化基础数据
    /// <summary> 心跳回调 </summary>
    private void OnHeartCheck(object obj)
    {
        /*
         s2c_heart_check s2c = obj as s2c_heart_check;
         Main.heartTime = GlobalFunction.getTimeStamp(false);
         Main.isHeart = true;
         TimerManager.instance.UpdateServerTime(s2c.time);

         if (isGetInfo) // 这里处理其他的第一次登陆玩家基础数据
         {
             isGetInfo = false;
             //获取其他基础数据
             this.QuerryInitInfo();
         }
         */
    }
    #endregion




}

