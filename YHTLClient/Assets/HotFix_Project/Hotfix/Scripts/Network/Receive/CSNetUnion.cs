using TABLE;

public partial class CSNetUnion : CSNetBase
{
	void ECM_SCGetUnionInfoMessage(NetInfo info)
	{
		union.UnionInfo msg = Network.Deserialize<union.UnionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionInfo");
			return;
		}

		//如果有公会返回这里
		CSGuildInfo.Instance.SetGuildInfo(msg);
	}
	void ECM_SCUnionListMessage(NetInfo info)
	{
		union.UnionList msg = Network.Deserialize<union.UnionList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionList");
			return;
		}

		//如果没有公会返回这里
		CSGuildInfo.Instance.SetUnionList(msg);
	}
	/// <summary>
	/// ��������
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCCreateUnionMessage(NetInfo info)
	{
		union.UnionInfo msg = Network.Deserialize<union.UnionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionInfo");
			return;
		}

        CSGuildInfo.Instance.JoinFamilyTime = CSServerTime.Instance.TotalMillisecond;
        UIManager.Instance.ClosePanel<UICreateGuildPanel>();
        //Net.CSGetUnionTabMessage(UnionTab.UnionActivities);
        //UIRedPointManager.Instance.DispatchOpenUnionRedPoint();

        CSGuildInfo.Instance.SetGuildInfo(msg);
	}
	void ECM_SCInviteUnionMessage(NetInfo info)
	{
		union.InviteUnionMsg msg = Network.Deserialize<union.InviteUnionMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.InviteUnionMsg");
			return;
		}

		CSGuildInfo.Instance.inviteUnion = msg;
	}
	void ECM_SCUnionPositionChangedMessage(NetInfo info)
	{
		union.ChangePositionMsg msg = Network.Deserialize<union.ChangePositionMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ChangePositionMsg");
			return;
		}

		if(msg.roleId == CSMainPlayerInfo.Instance.ID)
		{
			CSMainPlayerInfo.Instance.GuildPos = msg.position;
		}
		else
		{
            var avatar = CSAvatarManager.Instance.GetAvatarInfo(msg.roleId);
            if (avatar is CSPlayerInfo player)
            {
                player.GuildPos = msg.position;
            }
        }

		Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
    }
	void ECM_SCLeaveUnionMessage(NetInfo info)
	{
		union.LeaveUnionResponse msg = Network.Deserialize<union.LeaveUnionResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.LeaveUnionResponse");
			return;
		}

        if (msg.roleId == CSMainPlayerInfo.Instance.ID)
        {
			CSMainPlayerInfo.Instance.GuildName = string.Empty;
			CSMainPlayerInfo.Instance.GuildPos = 0;
			CSMainPlayerInfo.Instance.GuildLevel = 0;
			CSMainPlayerInfo.Instance.GuildId = 0;
			CSMainPlayerInfo.Instance.GuildCreateId = 0;
            if (msg.kickBy != null && msg.kickBy != CSMainPlayerInfo.Instance.Name)
            {
                if (msg.kickBy == null || msg.kickBy.Trim() == string.Empty)
                {
					UtilityTips.ShowTips(930);
                }
                else
                {
                    UtilityTips.ShowTips(931, 1.5f, ColorType.Yellow, msg.kickBy);
                }
            }
            VoiceChatManager.Instance.Logout();
        }
		else
		{
			CSGuildInfo.Instance.RefreshCurrentTab();
		}
        //UIRedPointManager.Instance.DispatchFamilyPractice();
        //UIRedPointManager.Instance.DispatchOpenUnionRedPoint();
    }
	/// <summary>
	/// �����Ҿ���
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCUnionDonateGoldMessage(NetInfo info)
	{
		union.UnionDonateResponse msg = Network.Deserialize<union.UnionDonateResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionDonateResponse");
			return;
		}

		Net.CSGetUnionTabMessage((int)UnionTab.UnionLogMessages);
		Net.CSGetUnionTabMessage((int)UnionTab.MainInfo);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildDonateSucceed);
	}
	void ECM_SCUnionDonateEquipMessage(NetInfo info)
	{
		union.UnionDonateResponse msg = Network.Deserialize<union.UnionDonateResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionDonateResponse");
			return;
		}

        CSGuildInfo.Instance.OnDonateEquipAddBagItemInfo(msg.oldItem,msg.newItem);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildBagChange);
		//HotManager.Instance.EventHandler.SendEvent(CEvent.Bag_UpdateList); //Ϊ��ˢ�±������
    }

	void ECM_SCJoinUnionMessage(NetInfo info)
	{
		union.UnionInfo msg = Network.Deserialize<union.UnionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionInfo");
			return;
		}

		CSGuildInfo.Instance.SetGuildInfo(msg);
		CSGuildInfo.Instance.JoinFamilyTime = CSServerTime.Instance.TotalMillisecond;
		Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnJoinedGuildSucceed);
        //UIRedPointManager.Instance.DispatchOpenUnionRedPoint();
    }

	void ECM_SCUnionExchangeEquipMessage(NetInfo info)
	{
		union.UnionExchangeEquipResponse msg = Network.Deserialize<union.UnionExchangeEquipResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionExchangeEquipResponse");
			return;
		}

		var bagItemInfo = CSGuildInfo.Instance.GetItemBagInfo(msg.guid);
		CSGuildInfo.Instance.RemoveItemByCount(msg.guid, msg.count);
  //      if (null != bagItemInfo)
  //          UtilityTips.ShowCenterMoveUpInfo(CSString.Format(933, bagItemInfo.QualityName()));
  //      else
  //          UtilityTips.ShowCenterMoveUpInfo(CSString.Format(933, msg.itemId.QualityName()));
    }
	void ECM_SCUnionDeclareWarMessage(NetInfo info)
	{
		union.DeclareWarResponse msg = Network.Deserialize<union.DeclareWarResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.DeclareWarResponse");
			return;
		}
		var guildId = CSMainPlayerInfo.Instance.GuildId;
		var enemyUnionIds = CSGuildInfo.Instance.EnemyUnionIds;
        if (guildId.Equals(msg.union1.unionId))
        {
            if (!enemyUnionIds.Contains(msg.union2.unionId))
            {
				enemyUnionIds.Add(msg.union2.unionId);
				HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildWarDeclare, msg.union2);
            }
        }
        else if (guildId.Equals(msg.union2.unionId))
        {
            if (!enemyUnionIds.Contains(msg.union1.unionId))
            {
				enemyUnionIds.Add(msg.union1.unionId);
				HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildWarDeclare, msg.union1);
            }
        }
    }
	void ECM_SCUnionWarTimeoutMessage(NetInfo info)
	{
		union.WarTimeout msg = Network.Deserialize<union.WarTimeout>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.WarTimeout");
			return;
		}
		CSGuildInfo.Instance.EnemyUnionIds.Remove(msg.enemyUnionId);
	}
	void ECM_SCGetUnionTabMessage(NetInfo info)
	{
		union.GetUnionTabResponse msg = Network.Deserialize<union.GetUnionTabResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.GetUnionTabResponse");
			return;
		}
		CSGuildInfo.Instance.SetUnionTabMessage(msg);
	}
	void ECM_SCGetSouvenirWealthMessage(NetInfo info)
	{
		union.GetSouvenirWealthResponse msg = Network.Deserialize<union.GetSouvenirWealthResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.GetSouvenirWealthResponse");
			return;
		}

		HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecievedRedPackges, msg.infos);
	}
	void ECM_SCUnionInfoUpdatedMessage(NetInfo info)
	{
		union.UnionInfoUpdate msg = Network.Deserialize<union.UnionInfoUpdate>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionInfoUpdate");
			return;
		}

		if(msg.param == 2)
		{
			Net.CSGetUnionTabMessage((int)UnionTab.SouvenirWealthPacks);
		}
		else if (msg.param == 0 || msg.param == 1)
		{
            Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
        }
		else
		{
			Net.CSGetUnionTabMessage((int)UnionTab.SouvenirWealthPacks);
			Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
		}
	}
	void ECM_SCUnionJoinInfoMessage(NetInfo info)
	{
		union.JoinList msg = Network.Deserialize<union.JoinList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.JoinList");
			return;
		}

        if (msg.havePerson && CSMainPlayerInfo.Instance.GuildPos <= (int)GuildPos.Presbyter)
        {
			//�����������
            UtilityPanel.JumpToPanel(11820);
        }
        else
        {
			UtilityTips.ShowPromptWordTips(50, null);
        }
    }
	void ECM_SCImpeachementMessage(NetInfo info)
	{
		union.ImpeachementMsg msg = Network.Deserialize<union.ImpeachementMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ImpeachementMsg");
			return;
		}

        CSGuildInfo.Instance.mImpeachMsg = msg;

        if (msg.timeS > 0) //data trueͶ�� falseûͶ��Ʊ
        {
            if (!msg.data)
            {
				FNDebug.LogFormat("[<color=#00ff00>[弹劾信息更新]:[弹劾中][还未投票]</color>]");
				CSGuildInfo.Instance.mIsShowImpeachBubble = true;
            }
            else
            {
				FNDebug.LogFormat("[<color=#00ff00>[弹劾信息更新]:[弹劾中][已经投票]</color>]");
				CSGuildInfo.Instance.mIsShowImpeachBubble = false;
            }
        }
        else
        {
			CSGuildInfo.Instance.mImpeachMsg = null;
			CSGuildInfo.Instance.mIsShowImpeachBubble = false;
			FNDebug.LogFormat("[<color=#00ff00>[弹劾信息更新]:[未启动弹劾]</color>]");
		}

		HotManager.Instance.EventHandler.SendEvent(CEvent.OnImpeachBubbleVisible);
    }

	void ECM_SCCanApplyUnionMessage(NetInfo info)
	{
		union.canApplyUnions msg = Network.Deserialize<union.canApplyUnions>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.canApplyUnions");
			return;
		}

		FNDebug.Log("<color=#00ff00>[收到公会申请弹出列表]</color>");

		int sundryId = 688;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out TABLE.SUNDRY sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('#');
        if (tokens.Length != 2)
            return;
        int low = 0;
        int high = 0;
        if (!int.TryParse(tokens[0], out low) || !int.TryParse(tokens[1], out high))
            return;

        if (CSScene.IsLanuchMainPlayer)
        {
			FNDebug.Log("<color=#00ff00>[收到公会申请弹出列表AAA]</color>");
			int lv = CSMainPlayerInfo.Instance.Level;
			if (lv < low || lv > high)
				return;
			
            UIManager.Instance.CreatePanel<UIChooseGuildPanel>((f) =>
            {
                if (f != null)
                {
                    (f as UIChooseGuildPanel).Show(msg);
                }
            });
        }
		else
		{
			FNDebug.Log("<color=#00ff00>[收到公会申请弹出列表BBB]</color>");
			OnceEventTrigger.Instance.Register(OnceEvent.OnLogginTrigger,
            () =>
            {
                if (!CSScene.IsLanuchMainPlayer)
                    return;

                int lv = CSMainPlayerInfo.Instance.Level;
                if (lv < low || lv > high)
                    return;

                UIManager.Instance.CreatePanel<UIChooseGuildPanel>((f) =>
                {
                    if (f != null)
                    {
                        (f as UIChooseGuildPanel).Show(msg);
                    }
                });
            });
        }
    }

	void ECM_SCChangeLeaderMessage(NetInfo info)
	{
		union.ApplyChangeLeader msg = Network.Deserialize<union.ApplyChangeLeader>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ApplyChangeLeader");
			return;
		}

		UtilityTips.ShowPromptWordTips(51,() =>
		{
            Net.CSSetApplyLeaderMessage(false);
        },
		() =>
        {
            Net.CSSetApplyLeaderMessage(true);
        }, null, msg.name);
    }

	void ECM_SCQueryCombineUnionMessage(NetInfo info)
	{
		union.combineUnion msg = Network.Deserialize<union.combineUnion>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.combineUnion");
			return;
		}

        if (msg.operationType == 1)
        {
            int proid = msg.combineType == 1 ? 52 : 53;
            string name = msg.combineType == 1 ? msg.unionName : msg.unionName1;
            string name2 = msg.combineType == 1 ? msg.unionName1 : msg.unionName;

			UtilityTips.ShowPromptWordTips(proid, () =>
			 {
				 int combinetype = msg.combineType == 1 ? 2 : 1;
				Net.CSCombineUnionMessage(2, combinetype, msg.unionId, msg.unionName, msg.unionId1, msg.unionName1, false);
			},
			() =>
			{
				int combinetype = msg.combineType == 1 ? 2 : 1;
				Net.CSCombineUnionMessage(2, combinetype, msg.unionId, msg.unionName, msg.unionId1, msg.unionName1, true);
			}, 60, name, name2);
        }
    }
	/// <summary>
	/// �޸Ĺ��淵��
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCUnionBulletinAckMessage(NetInfo info)
	{
		union.UnionBulletinAck msg = Network.Deserialize<union.UnionBulletinAck>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionBulletinAck");
			return;
		}
		if (!msg.isSucccess)
			return;

		var guildInfo = CSGuildInfo.Instance.GetGuildInfo();
		if(null != guildInfo)
		{
			guildInfo.bulletinCount = msg.submitTimes;
		}

		UtilityTips.ShowGreenTips(939);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildBulletChanged, msg);
	}

	void ECM_SCImpeachmentEndNtfMessage(NetInfo info)
	{
		union.ImpeachmentEndNtf msg = Network.Deserialize<union.ImpeachmentEndNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ImpeachmentEndNtf");
			return;
		}

        CSGuildInfo.Instance.mImpeachMsg = null;
		CSGuildInfo.Instance.mIsShowImpeachBubble = false;
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnImpeachBubbleVisible);
    }

	void ECM_SCRoleApplyUnionMessage(NetInfo info)
	{
		union.ApplyUnionListResponse msg = Network.Deserialize<union.ApplyUnionListResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ApplyUnionListResponse");
			return;
		}

		CSGuildInfo.Instance.applyUnionList = msg;
	}

	void ECM_SCImproveInfosMessage(NetInfo info)
	{
		union.ImproveInfos msg = Network.Deserialize<union.ImproveInfos>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ImproveInfos");
			return;
		}

		CSGuildInfo.Instance.InitImproves(msg.infos);
	}

	void ECM_SCImproveMessage(NetInfo info)
	{
		union.ImproveResponse msg = Network.Deserialize<union.ImproveResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.ImproveResponse");
			return;
		}

		CSGuildInfo.Instance.Improve(msg.improve.position,msg.improve.level);
	}

	void ECM_SCUnionDestroyItemMessage(NetInfo info)
	{
		union.UnionDestroyItemResponse msg = Network.Deserialize<union.UnionDestroyItemResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UnionDestroyItemResponse");
			return;
		}

		CSGuildInfo.Instance.RemoveItem(msg.itemId);
		HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildBagChange);
	}

	void ECM_SCSplitYuanbaoMessage(NetInfo info)
	{
		union.SplitYuanbaoResponse msg = Network.Deserialize<union.SplitYuanbaoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.SplitYuanbaoResponse");
			return;
		}

		if(msg.state == 1)
		{
			UtilityTips.ShowGreenTips(1079);
			Net.CSGetUnionTabMessage((int)UnionTab.MainInfo);
			Net.CSGetUnionTabMessage((int)UnionTab.UnionLogMessages);
			return;
		}

		if(msg.state == -1)
		{
            UtilityTips.ShowRedTips(1116);
            return;
        }

        if (msg.state == -2)
        {
            UtilityTips.ShowRedTips(1117);
            return;
        }
	}

	void ECM_SCImpeachmentAckMessage(NetInfo info)
	{
		
	}

	void ECM_SCUnionRecommendAckMessage(NetInfo info)
	{
        if (CSScene.IsLanuchMainPlayer)
        {
            UtilityTips.ShowPromptWordTips(60, () =>
            {
                UIManager.Instance.CreatePanel<UICreateGuildPanel>();
            });
        }
        else
        {
            OnceEventTrigger.Instance.Register(OnceEvent.OnLogginTrigger,
            () =>
            {
                UtilityTips.ShowPromptWordTips(60, () =>
                {
                    UIManager.Instance.CreatePanel<UICreateGuildPanel>();
                });
            });
        }
    }
	void ECM_ResUpdateSpeakLimitsMessage(NetInfo info)
	{
		union.UpdateCanSpeakMsg msg = Network.Deserialize<union.UpdateCanSpeakMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forunion.UpdateCanSpeakMsg");
			return;
		}

		FNDebug.LogFormat("<color=#00ff00>[语音权限]:设置个数:{0}</color>", msg.roleIds.Count);
		for (int i = 0,max = msg.roleIds.Count;i < max;++i)
		{
			if(CSAvatarManager.Instance.GetAvatarInfo(msg.roleIds[i]) is CSPlayerInfo playerInfo)
			{
				FNDebug.LogFormat("<color=#00ff00>[语音权限]:[roleId:{0}]:[option:{1}]</color>", msg.roleIds[i],msg.canSpeak);
				playerInfo.CanSpeak = msg.canSpeak;
			}
		}

        if (!QuDaoConstant.OpenVoice)
            return;

		if(msg.roleIds.Contains(CSMainPlayerInfo.Instance.ID))
		{
            if (!msg.canSpeak)
            {
                VoiceChatManager.Instance.SwitchVoiceSpeakState(() =>
                {
                    UtilityTips.ShowRedTips(1819);
                }, true);
            }
            else
            {
                UtilityTips.ShowRedTips(1820);
            }
        }

		VoiceChatManager.Instance.RefushBtnState();
		Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
	}
	void ECM_ResUnionCallInfoMessage(NetInfo info)
	{
		union.UnionCallInfo msg = Network.Deserialize<union.UnionCallInfo>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forunion.UnionCallInfo");
			return;
		}

		CSGuildInfo.Instance.CallMap(msg.playerName, msg.mapId, msg.posx, msg.posy);
	}
}
