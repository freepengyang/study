public partial class CSNetHeart : CSNetBase
{
	void ECM_ResHeartbeatMessage(NetInfo info)
	{
		heart.Heartbeat msg = Network.Deserialize<heart.Heartbeat>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forheart.Heartbeat");
			return;
		}
	}
}
