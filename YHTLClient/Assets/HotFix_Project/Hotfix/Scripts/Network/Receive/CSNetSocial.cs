public partial class CSNetSocial : CSNetBase
{
	/// <summary>
	/// ����ϵ���
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResSocialInfoMessage(NetInfo info)
	{
		social.SocialInfo msg = Network.Deserialize<social.SocialInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.SocialInfo");
			return;
		}
		CSFriendInfo.Instance.OnResetFriendInfos(msg.relations);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SocialInfoUpdate);
	}
	/// <summary>
	/// ���������Ӧ
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResAddRelationMessage(NetInfo info)
	{
		social.AddRelationResponse msg = Network.Deserialize<social.AddRelationResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.AddRelationResponse");
			return;
		}

		CSFriendInfo.Instance.RemoveSearchFriends(msg.added);

		CSFriendInfo.Instance.AddFriends(msg.added);

		bool touchedListChanged = false;
		for (int i = 0; i < msg.added.Count; ++i)
		{
			var relation = msg.added[i];
			CSFriendInfo.Instance.RemoveApplyFromList(relation.roleId);
			if(relation.relation == (int)FriendType.FT_BLACK_LIST)
			{
				CSFriendInfo.Instance.RemovePlayerFromTouchList(relation.roleId);
				touchedListChanged = true;
				CSChatManager.Instance.RemovePrivateChatDatas(relation.roleId);
			}
		}
		if(touchedListChanged)
			HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecvNewPrivateChatMsg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnApplyListChanged);
    }

	//�����б� -> ���Һ���
	void ECM_ResFindPlayerByNameMessage(NetInfo info)
	{
		social.FindPlayerByNameResponse msg = Network.Deserialize<social.FindPlayerByNameResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.FindPlayerByNameResponse");
			return;
		}
		CSFriendInfo.Instance.AddSearchFriends(msg.players);
	}

	void ECM_ResAddedFriendByPlayerMessage(NetInfo info)
	{
		//social.AddedFriendByPlayerResponse msg = Network.Deserialize<social.AddedFriendByPlayerResponse>(info);
		//if(null == msg)
		//{
		//	UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.AddedFriendByPlayerResponse");
		//	return;
		//}
	}
	/// <summary>
	/// ɾ�����ѽ����Ӧ
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResDeleteRelationMessage(NetInfo info)
	{
		social.DeleteRelationRequest msg = Network.Deserialize<social.DeleteRelationRequest>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.DeleteRelationRequest");
			return;
		}

		CSFriendInfo.Instance.DeleteFriends(msg.roleId);
	}
	/// <summary>
	/// ��ȡ���к�����Ӧ
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResGetAllSocialInfoMessage(NetInfo info)
	{
		social.RelationAllResponse msg = Network.Deserialize<social.RelationAllResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.RelationAllResponse");
			return;
		}
		CSFriendInfo.Instance.OnResetFriendRelation(msg.relationInfo);
	}

	//�����б� -> �ܾ���Ӻ���
	void ECM_RejectSingleAckMessage(NetInfo info)
	{
		social.RejectSingleAck msg = Network.Deserialize<social.RejectSingleAck>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.RejectSingleAck");
			return;
		}

		CSFriendInfo.Instance.RemoveApplyFromList(msg.roleId);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnApplyListChanged);
	}

	void ECM_QueryLatelyTouchAckMessage(NetInfo info)
	{
		social.QueryLatelyTouchAck msg = Network.Deserialize<social.QueryLatelyTouchAck>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.QueryLatelyTouchAck");
			return;
		}

		CSFriendInfo.Instance.UpdateLatelyTouchList(msg.touchList);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SocialInfoUpdate);
    }
	void ECM_ApplyFriendNotifyMessage(NetInfo info)
	{
		social.ApplyFriendList msg = Network.Deserialize<social.ApplyFriendList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.ApplyFriendList");
			return;
		}

		if(!CSFriendInfo.Instance.AcceptFriendInvite)
		{
			FNDebug.LogFormat("<color=#00ff00>[开启了好友邀请屏蔽]:收到好友邀请通知</color>");
            var applyList = msg.applys;
			if(null != applyList)
			{
                for (int i = 0; i < applyList.Count; ++i)
                {
                    FNDebug.LogFormat("<color=#00ff00>[开启了好友邀请屏蔽]:屏蔽玩家[{0}]</color>", applyList[i].name);
                    Net.RejectSingleReqMessage(applyList[i].roleId);
                }
            }
            return;
		}
        CSFriendInfo.Instance.ResetApplyList(msg.applys);
	}
	void ECM_SCSettingMessage(NetInfo info)
	{
		social.ResSetting msg = Network.Deserialize<social.ResSetting>(info);
        if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsocial.ResSetting");
			return;
		}
        CSConfigInfo.Instance.SetBool(ConfigOption.ForbidFriend, msg.socialFlag == 2);
        CSConfigInfo.Instance.SetBool(ConfigOption.ForbidStranger, msg.strangerFlag == 2);
        CSConfigInfo.Instance.SetBool(ConfigOption.ForbidGuild, msg.guildFlag == 2);
    }
}
