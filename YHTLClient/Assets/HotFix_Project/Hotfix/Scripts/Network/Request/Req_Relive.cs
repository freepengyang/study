using map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Net
{
    /// <summary>
    /// 请求复活
    /// </summary>
    /// <param name="type">复活类型</param>
    /// <param name="isUseYuanbao">是否使用元宝</param>
    public static void ReqReliveMessage(ReliveType type, bool isUseYuanbao)
    {
        ReliveRequest data = CSProtoManager.Get<ReliveRequest>();
        data.type = (int)type;
        data.useYuanbao = isUseYuanbao;
        CSNetwork.Instance.SendMsg((int)ECM.ReqReliveMessage, data);
    }
}
