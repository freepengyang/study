public partial class CSNetYuanbao : CSNetBase
{
	void ECM_ResYuanBaoInfoMessage(NetInfo info)
	{
		yuanbao.YuanBaoInfo msg = Network.Deserialize<yuanbao.YuanBaoInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foryuanbao.YuanBaoInfo");
			return;
		}
		CSGiveMeIngotInfo.Instance.SetIngotInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SetIngotInfo);
	}
}
