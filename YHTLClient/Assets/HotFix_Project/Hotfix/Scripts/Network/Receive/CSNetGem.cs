public partial class CSNetGem : CSNetBase
{


    void ECM_SCPosGemChangeMessage(NetInfo info)
	{
		gem.PosGemChange msg = Network.Deserialize<gem.PosGemChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgem.PosGemChange");
			return;
		}
        CSGemInfo.Instance.SetPosGemChangeData(msg);

    }

	void ECM_SCPosGemInfoMessage(NetInfo info)
	{
        //Debug.Log("ECM_SCPosGemInfoMessage"); 

		gem.PosGemInfo msg = Network.Deserialize<gem.PosGemInfo>(info);
        //Debug.Log("ECM_SCPosGemInfoMessage" + msg.unlockingPosition);
        if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgem.PosGemInfo");
			return;
		}
        CSGemInfo.Instance.SetData(msg);
    }

	void ECM_SCUnlockGemPositionMessage(NetInfo info)
	{
        //Debug.Log("ECM_SCUnlockGemPositionMessage");
		gem.UnlockGemPosition msg = Network.Deserialize<gem.UnlockGemPosition>(info);
        //Debug.Log("ECM_SCUnlockGemPositionMessageS" + msg.unlockingPosition);
        if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgem.UnlockGemPosition");
			return;
		}
        CSGemInfo.Instance.SetUnlockData(msg);
        

    }

	void ECM_SCGemSuitMessage(NetInfo info)
	{
        //Debug.Log("ECM_SCGemSuitMessage");
		gem.GemSuit msg = Network.Deserialize<gem.GemSuit>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgem.GemSuit");
			return;
		}
        CSGemInfo.Instance.SetSuit(msg);
    }
	void ECM_SCGemBossCountChangeMessage(NetInfo info)
	{
		gem.GemBossCountChange msg = Network.Deserialize<gem.GemBossCountChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgem.GemBossCountChange");
			return;
		}

		CSGemInfo.Instance.SetBossCount(msg.bossCounter);
	}
}
