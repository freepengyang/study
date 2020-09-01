public partial class CSNetGameversion : CSNetBase
{
	void ECM_ClientVersionNtfMessage(NetInfo info)
	{
		gameversion.ClientVersionNtf msg = Network.Deserialize<gameversion.ClientVersionNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forgameversion.ClientVersionNtf");
			return;
		}
		if (!QuDaoConstant.OpenCheckVersion) return;
		System.Version version = new System.Version(msg.ver);
		if(version > AppUrlMain.updateVersion) {
			UIManager.Instance.CreatePanel<UIUpdateExitGamePanel>();
		}
	}
	void ECM_ClientUpdateNtfMessage(NetInfo info)
	{
		gameversion.UpdateState msg = Network.Deserialize<gameversion.UpdateState>(info);
		if (msg != null)
		{
			if (msg.state == 2)
			{
				UIManager.Instance.CreatePanel<UIUpdateExitGamePanel>();
			}
			else
			{
				HotManager.Instance.EventHandler.SendEvent(CEvent.UpdateGame);
			}
		}
	}
}
