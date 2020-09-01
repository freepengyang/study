using Google.Protobuf.Collections;
using energy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Net
{
    /// <summary>
    /// 请求精力数据
    /// </summary>
    public static void CSEnergyInfoMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.CSEnergyInfoMessage, null);
    }
    /// <summary>
    /// 请求能免费领取的数据
    /// </summary>
    public static void CSEnergyFreeGetInfoMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.CSEnergyFreeGetInfoMessage, null);
    }
    /// <summary>
    /// 领取免费的精力数据
    /// </summary>
    /// <param name="_id"></param>
    public static void CSGetFreeEnergyMessage(int _id)
    {
        FNDebug.Log(_id);
        GetFreeEnergyRequest req = CSProtoManager.Get<GetFreeEnergyRequest>();
        req.timerId = _id;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSGetFreeEnergyMessage, req);
    }
    /// <summary>
    /// 获取玩家兑换数据
    /// </summary>
    public static void CSGetEnergeExchangeInfoMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.CSGetEnergeExchangeInfoMessage, null);
    }
    /// <summary>
    /// 玩家兑换
    /// </summary>
    public static void CSExchageEnergyMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.CSExchageEnergyMessage, null);
    }

}
