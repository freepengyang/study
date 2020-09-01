public partial class CSNetStonetreasure : CSNetBase
{
	void ECM_SCFloorInfoMessage(NetInfo info)
	{
		stonetreasure.FloorInfoResponse msg = Network.Deserialize<stonetreasure.FloorInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forstonetreasure.FloorInfoResponse");
			return;
		}
		CSStonetreasureInfo.Instance.SetFloorInfoMessage(msg);
		if(msg.type == 1)
		{
			UIManager.Instance.CreatePanel<UITombTreasurePanel>();
		}
		else
		{
			HotManager.Instance.EventHandler.SendEvent(CEvent.GetTombTreasureUpdateInfo);
		}
	}
	void ECM_SCStoneLocationMessage(NetInfo info)
	{
		stonetreasure.StoneLocationResponse msg = Network.Deserialize<stonetreasure.StoneLocationResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forstonetreasure.StoneLocationResponse");
			return;
		}
		CSStonetreasureInfo.Instance.SetStoneLocationMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetTombTreasureGridInfo);
	}
	void ECM_SCDownLocationMessage(NetInfo info)
	{
		stonetreasure.DownLocationResponse msg = Network.Deserialize<stonetreasure.DownLocationResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forstonetreasure.DownLocationResponse");
			return;
		}
		CSStonetreasureInfo.Instance.SetDownLocationMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetTombTreasureDoorInfo);
	}
	void ECM_SCGetNormalAndDownMessage(NetInfo info)
	{
		stonetreasure.GetNormalAndDownResponse msg = Network.Deserialize<stonetreasure.GetNormalAndDownResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forstonetreasure.GetNormalAndDownResponse");
			return;
		}
		CSStonetreasureInfo.Instance.SetNormalAndDownMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetTombTreasureNormalInfo);
	}
}
