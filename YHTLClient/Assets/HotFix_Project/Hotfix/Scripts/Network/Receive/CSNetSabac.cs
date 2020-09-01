public partial class CSNetSabac : CSNetBase
{
	void ECM_SCSabacDataInfoMessage(NetInfo info)
	{
		sabac.SabacDataResponse msg = Network.Deserialize<sabac.SabacDataResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsabac.SabacDataResponse");
			return;
		}
		
		CSGuildFightManager.Instance.SetFightInfo(msg.curPkId, msg.infos,msg.takenUnionId,msg.takenUnionName);

		if (CSScene.IsLanuchMainPlayer)
			CSGuildFightManager.Instance.TriggerGuildFightMessageBox();
		else
		{
			OnceEventTrigger.Instance.Register(OnceEvent.OnLogginTrigger,CSGuildFightManager.Instance.TriggerGuildFightMessageBox);
		}
	}

	void ECM_SCNotifySabacStateMessage(NetInfo info)
	{
		sabac.SabacStateResponse msg = Network.Deserialize<sabac.SabacStateResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsabac.SabacStateResponse");
			return;
		}

		if (CSScene.IsLanuchMainPlayer)
			CSGuildFightManager.Instance.SabacState = msg;
		else
			OnceEventTrigger.Instance.Register(OnceEvent.OnLogginTrigger, () =>
			 {
				 CSGuildFightManager.Instance.SabacState = msg;
			 });
	}

	void ECM_SCSabacRankInfoMessage(NetInfo info)
	{
		sabac.ResponseRankInfo msg = Network.Deserialize<sabac.ResponseRankInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsabac.ResponseRankInfo");
			return;
		}

		int myRank = 11;
		for (int i = 0,max = msg.info.Count; i < max;++i)
		{
			var myrankInfo = msg.info[i];
			if(myrankInfo.roleId == CSMainPlayerInfo.Instance.ID)
			{
				myRank = myrankInfo.rank;
				break;
			}
		}
		int myScore = msg.myRankData;
		CSGuildFightManager.Instance.SetRankInfos(msg.pkId,msg.info,msg.usage, myScore, myRank, msg.models);
	}

	void ECM_SCSabacResultMessage(NetInfo info)
	{
		sabac.SabacResultResponse msg = Network.Deserialize<sabac.SabacResultResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsabac.SabacResultResponse");
			return;
		}

		//高飞说沙城争霸结束后奖励界面不要了
		return;
 
		// if(!msg.hasJoinReward)
		// {
		// 	FNDebug.LogFormat("û�в��뽱��");
		// 	return;
		// }
		//
		// int mailId = 0;
		// if(CSGuildFightManager.Instance.IsSabacOwner)
		// {
		// 	mailId = 81;
		// 	FNDebug.Log("<color=#00ff00>�в��뽱�� [ɳ�Ϳ˳���]</color>");
		// }
		// else if(CSGuildFightManager.Instance.IsSabakeMember)
		// {
		// 	mailId = 82;
		// 	FNDebug.Log("<color=#00ff00>�в��뽱�� [ɳ�Ϳ˳�Ա]</color>");
		// }
		// else
		// {
		// 	mailId = 83;
		// 	FNDebug.Log("<color=#00ff00>�в��뽱�� [���뽱��]</color>");
		// }
		//
		// TABLE.MAIL mailItem;
		// if (!MailTableManager.Instance.TryGetValue(mailId, out mailItem))
		// {
		// 	FNDebug.LogErrorFormat("�ʼ������ô��� ID = {0}", mailId);
		// 	return;
		// }
		//
		// CSGuildFightManager.Instance.TryOpenSabacFightResultPanel(mailItem.items);
	}
	void ECM_SCSabacTransDoorInfoMessage(NetInfo info)
	{
		map.RoundTrigger msg = Network.Deserialize<map.RoundTrigger>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsabac.RoundTrigger");
			return;
		}
		
		HotManager.Instance.EventHandler.SendEvent(CEvent.SabacDoorChange,msg);
		
	}
}
