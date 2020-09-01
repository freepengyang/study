public partial class CSNetIntensify : CSNetBase
{
    void ECM_SCIntensifyMessage(NetInfo info)
	{
		intensify.IntensifyResponse msg = Network.Deserialize<intensify.IntensifyResponse>(info);
		if(null != msg)
		{
            CSEnhanceInfo.Instance.EnhanceResponse(msg);
        }
	}
	void ECM_SCIntensifyInfoMessage(NetInfo info)
	{
		intensify.IntensifyInfoResponse msg = Network.Deserialize<intensify.IntensifyInfoResponse>(info);
		if(null != msg)
		{
            CSEnhanceInfo.Instance.SCEnhanceInfo(msg);
		}
	}
	void ECM_SCIntensifySuitInfoMessage(NetInfo info)
	{
		intensify.IntensifySuitInfoResponse msg = Network.Deserialize<intensify.IntensifySuitInfoResponse>(info);
		if(null != msg)
		{
            CSEnhanceInfo.Instance.SCSuitInfo(msg);
        }
	}
}
