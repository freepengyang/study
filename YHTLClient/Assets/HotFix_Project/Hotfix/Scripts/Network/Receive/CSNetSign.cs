public partial class CSNetSign : CSNetBase
{
	void ECM_ResCardInfoMessage(NetInfo info)
	{
		sign.CardInfo msg = Network.Deserialize<sign.CardInfo>(info);
		if(null != msg)
		{
			//UnityEngine.Debug.LogError("@@@@@ECM_ResCardInfoMessage");
            CSSignCardInfo.Instance.SC_CardPoolInfo(msg);
            return;
		}
	}
	void ECM_ResSignInfoMessage(NetInfo info)
	{
		sign.SignInfo msg = Network.Deserialize<sign.SignInfo>(info);
		if(null != msg)
		{
			//UnityEngine.Debug.LogError("@@@@@ECM_ResSignInfoMessage");
            CSSignCardInfo.Instance.SC_SignInfo(msg);
			return;
		}
	}
	void ECM_ResCardChangeMessage(NetInfo info)
	{
		sign.CardChange msg = Network.Deserialize<sign.CardChange>(info);
		if(null != msg)
		{
            CSSignCardInfo.Instance.SC_CardChange(msg);
            return;
		}
	}
	void ECM_ResFinalSignRewardMessage(NetInfo info)
	{
		sign.ResFinalSignReward msg = Network.Deserialize<sign.ResFinalSignReward>(info);
		if(null != msg)
		{
            CSSignCardInfo.Instance.SC_UltAchievementReached(msg);
            return;
		}
	}
	void ECM_ResFragmentChangeMessage(NetInfo info)
	{
		sign.FragmentChange msg = Network.Deserialize<sign.FragmentChange>(info);
		if(null != msg)
		{
            CSSignCardInfo.Instance.SC_PiecesChange(msg);

            return;
		}
	}
	void ECM_ResLockCardMessage(NetInfo info)
	{
		sign.LockCard msg = Network.Deserialize<sign.LockCard>(info);
		if(null != msg)
		{
            CSSignCardInfo.Instance.SC_CollectionLockChange(msg);
			return;
		}
	}
	void ECM_ResCollectionChangeMessage(NetInfo info)
	{
		sign.CollectionChange msg = Network.Deserialize<sign.CollectionChange>(info);
		if(null != msg)
		{
            CSSignCardInfo.Instance.SC_CollectionChange(msg);
            return;
		}
	}
	void ECM_ResHonorChangeMessage(NetInfo info)
	{
		sign.HonorChange msg = Network.Deserialize<sign.HonorChange>(info);
        //Debug.LogError("@@@@@@@ECM_ResHonorChangeMessage:");
        if (null != msg)
		{
            CSSignCardInfo.Instance.SC_HonorChange(msg);
            return;
		}
	}
}
