public partial class CSNetCode : CSNetBase
{
	void ECM_SCCodeRewardMessage(NetInfo info)
	{
		code.ResCodeReward msg = Network.Deserialize<code.ResCodeReward>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forcode.ResCodeReward");
			return;
		}
	}
}
