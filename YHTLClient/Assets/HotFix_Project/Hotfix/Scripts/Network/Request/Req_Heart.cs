using Google.Protobuf.Collections;
// 包结构集合点
// author: jiabao
// time：  2016.2.17
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有请求服务器方法
/// </summary>
public partial class Net
{
    //心跳请求
    public static void ReqHeartbeatMessage(long time = 0)
    {
        heart.Heartbeat data = CSProtoManager.Get<heart.Heartbeat>(); //new heart.Heartbeat();
        data.clientTime = time;
        CSHotNetWork.Instance.SendMsg((int) ECM.ReqHeartbeatMessage, data);

        //TODO:ddn
        //if (UIMainSceneManager.Instance != null && UIMainSceneManager.Instance.PhoneState != null)
        //{
        //    UIMainSceneManager.Instance.PhoneState.StartCheckPing();
        //}
    }
}
