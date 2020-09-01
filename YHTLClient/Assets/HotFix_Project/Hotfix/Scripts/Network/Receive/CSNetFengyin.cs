using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等级封印系统网络响应
/// </summary>
public partial class CSNetFengyin : CSNetBase
{
    /// <summary>
    /// 封印开启
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCFengYinOpenMessage(NetInfo info)
    {
        fengyin.FengYinOpen msg = Network.Deserialize<fengyin.FengYinOpen>(info);
        CSSealGradeInfo.Instance.GetFengYinOpenMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OpenSeal, msg);
    }

    /// <summary>
    /// 封印时间缩短
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCFengYinTimeShortenMessage(NetInfo info)
    {
        fengyin.FengYinOpen msg = Network.Deserialize<fengyin.FengYinOpen>(info);
        CSSealGradeInfo.Instance.GetFengYinTimeShortenMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ShortenSeal, msg);
    }

    /// <summary>
    /// 封印结束
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCFengYinCloseMessage(NetInfo info)
    {
        CSSealGradeInfo.Instance.GetFengYinCloseMessage();
        HotManager.Instance.EventHandler.SendEvent(CEvent.CloseSeal);
    }

    /// <summary>
    /// 幻境打开
    /// </summary>
    void ECM_SCHuanJingOpenMessage(NetInfo info)
    {
        fengyin.HuanJingOpen msg = Network.Deserialize<fengyin.HuanJingOpen>(info);
        CSDreamLandInfo.Instance.GetDreamLandInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OpenDreamLand, msg);
    }

    /// <summary>
    /// 幻境关闭
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCHuanJingCloseMessage(NetInfo info)
    {
        //Debug.Log("------------------------------幻境关闭");
        CSDreamLandInfo.Instance.HandleDreamLand();
        HotManager.Instance.EventHandler.SendEvent(CEvent.CloseDreamLand);
    }

    /// <summary>
    /// 幻境时间变化
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCHuanJingChangeMessage(NetInfo info)
    {
        fengyin.HuanJingChange msg = Network.Deserialize<fengyin.HuanJingChange>(info);
        CSDreamLandInfo.Instance.ChangeDreamLandTime(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeDreamLandTime);
    }
	void ECM_ResWorldLevelMessage(NetInfo info)
	{
		fengyin.WorldLevel msg = Network.Deserialize<fengyin.WorldLevel>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfengyin.WorldLevel");
			return;
		}
        CSSealGradeInfo.Instance.SetNowWorldLevel(msg.level);
    }
}
