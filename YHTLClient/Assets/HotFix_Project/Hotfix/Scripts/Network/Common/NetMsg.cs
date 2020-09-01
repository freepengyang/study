//# generated by tools , Do Not Edit It ...
using System.Collections.Generic;
using System;
using Google.Protobuf;

public static class NetCallabck
{
	public static Type GetCallbackType(int id)
	{
		if (mNetCallbackDic.ContainsKey(id))
			return mNetCallbackDic[id];
		
		return null;
	}
	public static Dictionary<int, Type> mNetCallbackDic = new Dictionary<int, Type>
	{
		/*----------------user----------------*/
		{(int)ECM.Connect,typeof(CSNetUser)},
		{(int)ECM.ConnectFail,typeof(CSNetUser)},
		{(int)ECM.Disconnect,typeof(CSNetUser)},
		{(int)ECM.ResLoginMessage,typeof(CSNetUser)},
		{(int)ECM.ResRandomRoleNameMessage,typeof(CSNetUser)},
		{(int)ECM.ResPlayerInfoMessage,typeof(CSNetUser)},
		{(int)ECM.ResDeleteRoleMessage,typeof(CSNetUser)},
		{(int)ECM.ResPlayAttributeChangedMessage,typeof(CSNetUser)},
		{(int)ECM.ResPlayerEquipChangedMessage,typeof(CSNetUser)},
		{(int)ECM.ResLoginAnotherSessionMessage,typeof(CSNetUser)},
		{(int)ECM.ResDisconnectMessage,typeof(CSNetUser)},
		{(int)ECM.ResPushMessageMessage,typeof(CSNetUser)},
		{(int)ECM.ResExtractInvitationCodeMessage,typeof(CSNetUser)},
		{(int)ECM.ResLoginSignTimeoutMessage,typeof(CSNetUser)},
		{(int)ECM.CheckCreateRoleArgsVaildAckMessage,typeof(CSNetUser)},
		{(int)ECM.ServerTimeNtfMessage,typeof(CSNetUser)},
		{(int)ECM.ServerLoadNtfMessage,typeof(CSNetUser)},
		{(int)ECM.ServerBusyNtfMessage,typeof(CSNetUser)},
		{(int)ECM.CreateRoleNtfMessage,typeof(CSNetUser)},
		{(int)ECM.SCPlayerMoveSpeedMessage,typeof(CSNetUser)},
		{(int)ECM.SCRoleListMessage,typeof(CSNetUser)},
		/*----------------map----------------*/
		{(int)ECM.ResEnterMapMessage,typeof(CSNetMap)},
		{(int)ECM.ResUpdateViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResObjectExitViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResPlayerEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResMonsterEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResObjectMoveMessage,typeof(CSNetMap)},
		{(int)ECM.ResItemEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResNPCEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResPositionChangeMessage,typeof(CSNetMap)},
		{(int)ECM.ResChangeMapMessage,typeof(CSNetMap)},
		{(int)ECM.ResAdjustPositionMessage,typeof(CSNetMap)},
		{(int)ECM.ResReliveMessage,typeof(CSNetMap)},
		{(int)ECM.ResPlayerHPChangedMessage,typeof(CSNetMap)},
		{(int)ECM.ResBufferEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResItemsDropMessage,typeof(CSNetMap)},
		{(int)ECM.ResItemOwnerChangedMessage,typeof(CSNetMap)},
		{(int)ECM.ResPetEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResGuardEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResPetStateChangedMessage,typeof(CSNetMap)},
		{(int)ECM.ResTriggerEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResObjectChangePositionMessage,typeof(CSNetMap)},
		{(int)ECM.ResBossOwnerChangedMessage,typeof(CSNetMap)},
		{(int)ECM.ResBoxEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResSafeAreaCoordEnterViewMessage,typeof(CSNetMap)},
		{(int)ECM.ResMapBossMessage,typeof(CSNetMap)},
		{(int)ECM.ResWeatherChangeMessage,typeof(CSNetMap)},
		{(int)ECM.SmallViewTeammateNtfMessage,typeof(CSNetMap)},
		{(int)ECM.MonsterAppearanceChangedNtfMessage,typeof(CSNetMap)},
		{(int)ECM.NpcsStatNtfMessage,typeof(CSNetMap)},
		{(int)ECM.PlayerStateNtfMessage,typeof(CSNetMap)},
		{(int)ECM.ResPetShapeChangeNtf,typeof(CSNetMap)},
		{(int)ECM.SCMapDetailsMessage,typeof(CSNetMap)},
		{(int)ECM.SCGoldKeyPickUpItemMessage,typeof(CSNetMap)},
		{(int)ECM.SCMainTaskTransmitEventMessage,typeof(CSNetMap)},
		{(int)ECM.SCAddMainTaskTransmitEventMessage,typeof(CSNetMap)},
		{(int)ECM.SCRemoveMainTaskTransmitEventMessage,typeof(CSNetMap)},
		/*----------------bag----------------*/
		{(int)ECM.ResGetBagInfoMessage,typeof(CSNetBag)},
		{(int)ECM.ResBagItemChangedMessage,typeof(CSNetBag)},
		{(int)ECM.ResWealthAmountChangeMessage,typeof(CSNetBag)},
		{(int)ECM.ResEquipItemMessage,typeof(CSNetBag)},
		{(int)ECM.ResSortItemsMessage,typeof(CSNetBag)},
		{(int)ECM.ResSwapItemMessage,typeof(CSNetBag)},
		{(int)ECM.ResUnEquipItemMessage,typeof(CSNetBag)},
		{(int)ECM.ResCallBackItemMessage,typeof(CSNetBag)},
		{(int)ECM.ResPickupItemMessage,typeof(CSNetBag)},
		{(int)ECM.ResBagIsFullMessage,typeof(CSNetBag)},
		{(int)ECM.ResBagToStorehouseMessage,typeof(CSNetBag)},
		{(int)ECM.ResOpenDebugMsgMessage,typeof(CSNetBag)},
		{(int)ECM.ResChangeBagCountMessage,typeof(CSNetBag)},
		{(int)ECM.ItemUseLimitNtfMessage,typeof(CSNetBag)},
		{(int)ECM.LimitItemInfoNtfMessage,typeof(CSNetBag)},
		{(int)ECM.EquipItemModifyNBValueNtfMessage,typeof(CSNetBag)},
		{(int)ECM.EquipRebuildNtfMessage,typeof(CSNetBag)},
		{(int)ECM.EquipXiLianNtfMessage,typeof(CSNetBag)},
		{(int)ECM.SCChooseXiLianResultNtfMessage,typeof(CSNetBag)},
		{(int)ECM.SCNotifyBagItemCdInfoMessage,typeof(CSNetBag)},
		{(int)ECM.SCItemUsedDailyMessage,typeof(CSNetBag)},
		{(int)ECM.SCItemUsedDailyTotalMessage,typeof(CSNetBag)},
		/*----------------tip----------------*/
		{(int)ECM.ResTipMessage,typeof(CSNetTip)},
		{(int)ECM.ResBulletinMessage,typeof(CSNetTip)},
		{(int)ECM.TipNtfMessage,typeof(CSNetTip)},
		{(int)ECM.SCNotifyNoteInfoMessage,typeof(CSNetTip)},
		/*----------------fight----------------*/
		{(int)ECM.ResSkillEffectMessage,typeof(CSNetFight)},
		{(int)ECM.ResAddBufferMessage,typeof(CSNetFight)},
		{(int)ECM.ResRemoveBufferMessage,typeof(CSNetFight)},
		{(int)ECM.ResBufferDeltaHPMessage,typeof(CSNetFight)},
		{(int)ECM.SCAddSkillMessage,typeof(CSNetFight)},
		{(int)ECM.SCRemoveSkillMessage,typeof(CSNetFight)},
		{(int)ECM.SCSkillShortCutMessage,typeof(CSNetFight)},
		{(int)ECM.ResBufferInfoMessage,typeof(CSNetFight)},
		{(int)ECM.ResRemoveSkillMessage,typeof(CSNetFight)},
		{(int)ECM.PkModeChangedNtfMessage,typeof(CSNetFight)},
		{(int)ECM.SCUpgradeSkillMessage,typeof(CSNetFight)},
		{(int)ECM.BufferRemoveRemindNtfMessage,typeof(CSNetFight)},
		{(int)ECM.ClearSkillCDNtfMessage,typeof(CSNetFight)},
		{(int)ECM.SCSetSkillAutoStateMessage,typeof(CSNetFight)},
		{(int)ECM.SCUpdateSkillRefixMessage,typeof(CSNetFight)},
		{(int)ECM.SCPlayerFsmStateMessage,typeof(CSNetFight)},
		/*----------------gm----------------*/
		{(int)ECM.ResCloseServerMessage,typeof(CSNetGm)},
		{(int)ECM.ResReloadScriptMessage,typeof(CSNetGm)},
		/*----------------mail----------------*/
		{(int)ECM.ResMailListMessage,typeof(CSNetMail)},
		{(int)ECM.ResNewMailMessage,typeof(CSNetMail)},
		{(int)ECM.ResGetMailItemMessage,typeof(CSNetMail)},
		{(int)ECM.ResDeleteMailMessage,typeof(CSNetMail)},
		/*----------------player----------------*/
		{(int)ECM.ResRoleExpUpdatedMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResRoleUpgradeMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResRoleExtraValuesMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResDayPassedMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResOtherPlayerInfoMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResCommonMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResOtherPlayerCommonInfoMessage,typeof(CSNetPlayer)},
		{(int)ECM.RoleAttrNtfMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResPlayerDieMessage,typeof(CSNetPlayer)},
		{(int)ECM.ResPkValueUpdateMessage,typeof(CSNetPlayer)},
		{(int)ECM.SCRoleBriefMessage,typeof(CSNetPlayer)},
		{(int)ECM.SCPkValueChangeMessage,typeof(CSNetPlayer)},
		{(int)ECM.SCPkGreyNameStateMessage,typeof(CSNetPlayer)},
		/*----------------paodian----------------*/
		{(int)ECM.ResUPaoDianChangeMessage,typeof(CSNetPaodian)},
		{(int)ECM.SCPaoDianInfoMessage,typeof(CSNetPaodian)},
		{(int)ECM.SCRandomPaoDianMessage,typeof(CSNetPaodian)},
		{(int)ECM.SCPaoDianExpMessage,typeof(CSNetPaodian)},
		/*----------------chat----------------*/
		{(int)ECM.ResChatMessage,typeof(CSNetChat)},
		{(int)ECM.LeftCornerTipNtfMessage,typeof(CSNetChat)},
		{(int)ECM.ReleaseNtfMessage,typeof(CSNetChat)},
		{(int)ECM.VoiceRoomNtfMessage,typeof(CSNetChat)},
		{(int)ECM.RoleDetailNtfMessage,typeof(CSNetChat)},
		{(int)ECM.ForbidChatNtfMessage,typeof(CSNetChat)},
		{(int)ECM.BigExpressionNtfMessage,typeof(CSNetChat)},
		/*----------------team----------------*/
		{(int)ECM.ResTeamInfoMessage,typeof(CSNetTeam)},
		{(int)ECM.ResApplyTeamMessage,typeof(CSNetTeam)},
		{(int)ECM.ResInviteTeamMessage,typeof(CSNetTeam)},
		{(int)ECM.ResJoinTeamMessage,typeof(CSNetTeam)},
		{(int)ECM.ResLeaveTeamMessage,typeof(CSNetTeam)},
		{(int)ECM.ResTeamLeaderChangedMessage,typeof(CSNetTeam)},
		{(int)ECM.ResGetTeamInfoMessage,typeof(CSNetTeam)},
		{(int)ECM.ResGetTeamTabMessage,typeof(CSNetTeam)},
		{(int)ECM.ResTeamCallBackMessage,typeof(CSNetTeam)},
		{(int)ECM.TeamTargetAckMessage,typeof(CSNetTeam)},
		{(int)ECM.TeamCallBackAckMessage,typeof(CSNetTeam)},
		{(int)ECM.SCSetTeamModeMessage,typeof(CSNetTeam)},
		{(int)ECM.SCPlayerHpMpInfoMessage,typeof(CSNetTeam)},
		{(int)ECM.SCPlayerLevelInfoMessage,typeof(CSNetTeam)},
		/*----------------instance----------------*/
		{(int)ECM.ResBuyInstanceTimesMessage,typeof(CSNetInstance)},
		{(int)ECM.ResInstanceInfoMessage,typeof(CSNetInstance)},
		{(int)ECM.SCEnterInstanceMessage,typeof(CSNetInstance)},
		{(int)ECM.SCInstanceFinishMessage,typeof(CSNetInstance)},
		{(int)ECM.SCLeaveInstanceMessage,typeof(CSNetInstance)},
		{(int)ECM.ResQuickInstanceMessage,typeof(CSNetInstance)},
		{(int)ECM.ResInstanceCountMessage,typeof(CSNetInstance)},
		{(int)ECM.ResDiLaoInfoMessage,typeof(CSNetInstance)},
		{(int)ECM.SCUndergroundTreasureMessage,typeof(CSNetInstance)},
		{(int)ECM.SCBossChallengeMessage,typeof(CSNetInstance)},
		{(int)ECM.SCDropLimitMessage,typeof(CSNetInstance)},
		/*----------------storehouse----------------*/
		{(int)ECM.ResGetStorehouseInfoMessage,typeof(CSNetStorehouse)},
		{(int)ECM.ResStorehouseItemChangedMessage,typeof(CSNetStorehouse)},
		{(int)ECM.ResSortStorehouseMessage,typeof(CSNetStorehouse)},
		{(int)ECM.ResExchangeItemMessage,typeof(CSNetStorehouse)},
		{(int)ECM.ResStorehouseToBagMessage,typeof(CSNetStorehouse)},
		{(int)ECM.ResAddStorehouseCountMessage,typeof(CSNetStorehouse)},
		/*----------------wolong----------------*/
		{(int)ECM.SCWoLongInfoMessage,typeof(CSNetWolong)},
		{(int)ECM.SCWoLongLevelUpMessage,typeof(CSNetWolong)},
		{(int)ECM.SCSkillGroupInfoMessage,typeof(CSNetWolong)},
		{(int)ECM.SCWoLongXiLianMessage,typeof(CSNetWolong)},
		{(int)ECM.SCWoLongXiLianSelectMessage,typeof(CSNetWolong)},
		{(int)ECM.SCSoldierSoulInfoAwakenMessage,typeof(CSNetWolong)},
		{(int)ECM.SCSoldierSoulInfoMessage,typeof(CSNetWolong)},
		/*----------------task----------------*/
		{(int)ECM.ResTaskListMessage,typeof(CSNetTask)},
		{(int)ECM.ResTaskStateChangedMessage,typeof(CSNetTask)},
		{(int)ECM.ResTaskGoalUpdatedMessage,typeof(CSNetTask)},
		{(int)ECM.ResAcceptTaskMessage,typeof(CSNetTask)},
		{(int)ECM.ResSubmitTaskMessage,typeof(CSNetTask)},
		{(int)ECM.SCCycTaskMessage,typeof(CSNetTask)},
		{(int)ECM.ResFlyToGoalMessage,typeof(CSNetTask)},
		{(int)ECM.SCNewTaskMessage,typeof(CSNetTask)},
		/*----------------energy----------------*/
		{(int)ECM.SCEnergyInfoMessage,typeof(CSNetEnergy)},
		{(int)ECM.SCEnergyFreeGetInfoMessage,typeof(CSNetEnergy)},
		{(int)ECM.SCGetFreeEnergyMessage,typeof(CSNetEnergy)},
		{(int)ECM.SCNotifyEnergyChangeMessage,typeof(CSNetEnergy)},
		{(int)ECM.SCEnergyExchangeInfoMessage,typeof(CSNetEnergy)},
		/*----------------systemcontroller----------------*/
		{(int)ECM.SystemFunctionStateNtfMessage,typeof(CSNetSystemcontroller)},
		{(int)ECM.SCRoleFunctionDataMessage,typeof(CSNetSystemcontroller)},
		/*----------------fengyin----------------*/
		{(int)ECM.SCFengYinOpenMessage,typeof(CSNetFengyin)},
		{(int)ECM.SCFengYinTimeShortenMessage,typeof(CSNetFengyin)},
		{(int)ECM.SCFengYinCloseMessage,typeof(CSNetFengyin)},
		{(int)ECM.SCHuanJingOpenMessage,typeof(CSNetFengyin)},
		{(int)ECM.SCHuanJingCloseMessage,typeof(CSNetFengyin)},
		{(int)ECM.SCHuanJingChangeMessage,typeof(CSNetFengyin)},
		{(int)ECM.ResWorldLevelMessage,typeof(CSNetFengyin)},
		/*----------------social----------------*/
		{(int)ECM.ResSocialInfoMessage,typeof(CSNetSocial)},
		{(int)ECM.ResAddRelationMessage,typeof(CSNetSocial)},
		{(int)ECM.ResFindPlayerByNameMessage,typeof(CSNetSocial)},
		{(int)ECM.ApplyFriendNotifyMessage,typeof(CSNetSocial)},
		{(int)ECM.ResDeleteRelationMessage,typeof(CSNetSocial)},
		{(int)ECM.ResGetAllSocialInfoMessage,typeof(CSNetSocial)},
		{(int)ECM.RejectSingleAckMessage,typeof(CSNetSocial)},
		{(int)ECM.QueryLatelyTouchAckMessage,typeof(CSNetSocial)},
		{(int)ECM.SCSettingMessage,typeof(CSNetSocial)},
		/*----------------shop----------------*/
		{(int)ECM.SCShopBuyInfoMessage,typeof(CSNetShop)},
		{(int)ECM.SCShopBuyMessage,typeof(CSNetShop)},
		{(int)ECM.SCShopInfoMessage,typeof(CSNetShop)},
		{(int)ECM.SCDailyRmbInfoMessage,typeof(CSNetShop)},
		{(int)ECM.SCDuiHuanShopInfoMessage,typeof(CSNetShop)},
		{(int)ECM.SCDuiHuanShopInfoByIdMessage,typeof(CSNetShop)},
		/*----------------luck----------------*/
		/*----------------tujian----------------*/
		{(int)ECM.SCTujianInfoMessage,typeof(CSNetTujian)},
		{(int)ECM.SCTujianUpLevelMessage,typeof(CSNetTujian)},
		{(int)ECM.SCTujianUpQualityMessage,typeof(CSNetTujian)},
		{(int)ECM.SCTujianInlayMessage,typeof(CSNetTujian)},
		{(int)ECM.SCActivateSlotWingMessage,typeof(CSNetTujian)},
		{(int)ECM.SCTujianAddMessage,typeof(CSNetTujian)},
		{(int)ECM.SCTujianRemoveMessage,typeof(CSNetTujian)},
		/*----------------worldboss----------------*/
		{(int)ECM.SCWorldBossActivityInfoResponseMessage,typeof(CSNetWorldboss)},
		{(int)ECM.SCJoinWorldBossActivityResponseMessage,typeof(CSNetWorldboss)},
		{(int)ECM.SCNotifyWorldBossRankInfoMessage,typeof(CSNetWorldboss)},
		{(int)ECM.SCWorldBossBlessInfoMessage,typeof(CSNetWorldboss)},
		{(int)ECM.SCWorldBossBossInfoMessage,typeof(CSNetWorldboss)},
		/*----------------activity----------------*/
		{(int)ECM.ResActivityDataMessage,typeof(CSNetActivity)},
		{(int)ECM.SCCollectActivityDataMessage,typeof(CSNetActivity)},
		{(int)ECM.ResActiveRewardMessage,typeof(CSNetActivity)},
		{(int)ECM.ResActiveMessage,typeof(CSNetActivity)},
		{(int)ECM.ResFengYinDataMessage,typeof(CSNetActivity)},
		{(int)ECM.ResSpecialActivityDataMessage,typeof(CSNetActivity)},
		{(int)ECM.SCBossFirstKillDatasMessage,typeof(CSNetActivity)},
		{(int)ECM.SCEquipXuanShangMessage,typeof(CSNetActivity)},
		{(int)ECM.ResSevenDayDataMessage,typeof(CSNetActivity)},
		{(int)ECM.ResEquipCompetitionMessage,typeof(CSNetActivity)},
		{(int)ECM.SCBossKuangHuanMessage,typeof(CSNetActivity)},
		{(int)ECM.SCKillDemonMessage,typeof(CSNetActivity)},
		{(int)ECM.SCSpecialActivityOpenInfoMessage,typeof(CSNetActivity)},
		{(int)ECM.SCSevenLoginMessage,typeof(CSNetActivity)},
		/*----------------ultimate----------------*/
		{(int)ECM.SCUltimateInfoMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCResetUltimateMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCSelectAdditionEffectMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCRankInfoMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCSelectAdditionIndexMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCResponseCardMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCOpenCardInfoMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCSelectCardIndexMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCGodBlessMessage,typeof(CSNetUltimate)},
		{(int)ECM.SCUltimatePassInfoMessage,typeof(CSNetUltimate)},
		/*----------------boss----------------*/
		{(int)ECM.SCBossInfoMessage,typeof(CSNetBoss)},
		/*----------------fashion----------------*/
		{(int)ECM.SCAllFashionInfoMessage,typeof(CSNetFashion)},
		{(int)ECM.SCEquipFashionMessage,typeof(CSNetFashion)},
		{(int)ECM.SCFashionStarLevelUpMessage,typeof(CSNetFashion)},
		{(int)ECM.SCAddFashionMessage,typeof(CSNetFashion)},
		{(int)ECM.SCRemoveFashionMessage,typeof(CSNetFashion)},
		{(int)ECM.SCUnEquipFashionMessage,typeof(CSNetFashion)},
		/*----------------wing----------------*/
		{(int)ECM.SCWingInfoMessage,typeof(CSNetWing)},
		{(int)ECM.SCWingUpStarMessage,typeof(CSNetWing)},
		{(int)ECM.SCWingAdvanceMessage,typeof(CSNetWing)},
		{(int)ECM.SCDressColorWingMessage,typeof(CSNetWing)},
		{(int)ECM.SCWingExpItemUseMessage,typeof(CSNetWing)},
		{(int)ECM.SCColorWingChangeMessage,typeof(CSNetWing)},
		{(int)ECM.SCYuLingInfoMessage,typeof(CSNetWing)},
		/*----------------lianti----------------*/
		{(int)ECM.SCLianTiInfoMessage,typeof(CSNetLianti)},
		{(int)ECM.SCLianTiFieldMessage,typeof(CSNetLianti)},
		{(int)ECM.SCLianTiUpLevelMessage,typeof(CSNetLianti)},
		/*----------------auction----------------*/
		{(int)ECM.ResGetAuctionItemsMessage,typeof(CSNetAuction)},
		{(int)ECM.ResGetAuctionShelfMessage,typeof(CSNetAuction)},
		{(int)ECM.ResAddToShelfMessage,typeof(CSNetAuction)},
		{(int)ECM.ResReAddToShelfMessage,typeof(CSNetAuction)},
		{(int)ECM.ResRemoveFromShelfMessage,typeof(CSNetAuction)},
		{(int)ECM.ResBuyAuctionItemMessage,typeof(CSNetAuction)},
		{(int)ECM.ResUnlockAuctionShelveMessage,typeof(CSNetAuction)},
		{(int)ECM.ResAttentionAuctionMessage,typeof(CSNetAuction)},
		/*----------------union----------------*/
		{(int)ECM.SCGetUnionInfoMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionListMessage,typeof(CSNetUnion)},
		{(int)ECM.SCCreateUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCInviteUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionPositionChangedMessage,typeof(CSNetUnion)},
		{(int)ECM.SCLeaveUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionDonateGoldMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionDonateEquipMessage,typeof(CSNetUnion)},
		{(int)ECM.SCJoinUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionExchangeEquipMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionDeclareWarMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionWarTimeoutMessage,typeof(CSNetUnion)},
		{(int)ECM.SCGetUnionTabMessage,typeof(CSNetUnion)},
		{(int)ECM.SCGetSouvenirWealthMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionInfoUpdatedMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionJoinInfoMessage,typeof(CSNetUnion)},
		{(int)ECM.SCImpeachementMessage,typeof(CSNetUnion)},
		{(int)ECM.SCCanApplyUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCChangeLeaderMessage,typeof(CSNetUnion)},
		{(int)ECM.SCQueryCombineUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionBulletinAckMessage,typeof(CSNetUnion)},
		{(int)ECM.SCImpeachmentAckMessage,typeof(CSNetUnion)},
		{(int)ECM.SCImpeachmentEndNtfMessage,typeof(CSNetUnion)},
		{(int)ECM.SCRoleApplyUnionMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionRecommendAckMessage,typeof(CSNetUnion)},
		{(int)ECM.SCImproveInfosMessage,typeof(CSNetUnion)},
		{(int)ECM.SCImproveMessage,typeof(CSNetUnion)},
		{(int)ECM.SCUnionDestroyItemMessage,typeof(CSNetUnion)},
		{(int)ECM.SCSplitYuanbaoMessage,typeof(CSNetUnion)},
		{(int)ECM.ResUpdateSpeakLimitsMessage,typeof(CSNetUnion)},
		{(int)ECM.ResUnionCallInfoMessage,typeof(CSNetUnion)},
		/*----------------intensify----------------*/
		{(int)ECM.SCIntensifyMessage,typeof(CSNetIntensify)},
		{(int)ECM.SCIntensifyInfoMessage,typeof(CSNetIntensify)},
		{(int)ECM.SCIntensifySuitInfoMessage,typeof(CSNetIntensify)},
		/*----------------baozhu----------------*/
		{(int)ECM.SCLevelUpBaoZhuMessage,typeof(CSNetBaozhu)},
		{(int)ECM.SCGradeUpBaoZhuMessage,typeof(CSNetBaozhu)},
		{(int)ECM.SCBaoZhuSkillsMessage,typeof(CSNetBaozhu)},
		{(int)ECM.SCChoseBaoZhuSkillMessage,typeof(CSNetBaozhu)},
		{(int)ECM.SCBaoZhuBossCountChangeMessage,typeof(CSNetBaozhu)},
		{(int)ECM.SCBaoZhuSlotSkillsMessage,typeof(CSNetBaozhu)},
		/*----------------sabac----------------*/
		{(int)ECM.SCSabacDataInfoMessage,typeof(CSNetSabac)},
		{(int)ECM.SCNotifySabacStateMessage,typeof(CSNetSabac)},
		{(int)ECM.SCSabacRankInfoMessage,typeof(CSNetSabac)},
		{(int)ECM.SCSabacResultMessage,typeof(CSNetSabac)},
		{(int)ECM.SCSabacTransDoorInfoMessage,typeof(CSNetSabac)},
		/*----------------vip----------------*/
		{(int)ECM.ResVipMessage,typeof(CSNetVip)},
		{(int)ECM.ResVipTasteCardMessage,typeof(CSNetVip)},
		{(int)ECM.SCVipTasteCardMessage,typeof(CSNetVip)},
		{(int)ECM.SCFirstRechargeInfoMessage,typeof(CSNetVip)},
		{(int)ECM.FirstRechargeNtfMessage,typeof(CSNetVip)},
		{(int)ECM.AccumulatedRechargeInfoMessage,typeof(CSNetVip)},
		{(int)ECM.SCMonthRechargeInfoMessage,typeof(CSNetVip)},
		{(int)ECM.SCMonthRechargeInfoByIdMessage,typeof(CSNetVip)},
		/*----------------gem----------------*/
		{(int)ECM.SCPosGemChangeMessage,typeof(CSNetGem)},
		{(int)ECM.SCPosGemInfoMessage,typeof(CSNetGem)},
		{(int)ECM.SCUnlockGemPositionMessage,typeof(CSNetGem)},
		{(int)ECM.SCGemSuitMessage,typeof(CSNetGem)},
		{(int)ECM.SCGemBossCountChangeMessage,typeof(CSNetGem)},
		/*----------------treasurehunt----------------*/
		{(int)ECM.ResServerHistoryMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResTreasureItemChangedMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResTreasureStorehouseMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResTreasureEndMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResUseTreasureExpMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResTreasureIdMessage,typeof(CSNetTreasurehunt)},
		{(int)ECM.ResHuntCallBackMessage,typeof(CSNetTreasurehunt)},
		/*----------------stonetreasure----------------*/
		{(int)ECM.SCFloorInfoMessage,typeof(CSNetStonetreasure)},
		{(int)ECM.SCStoneLocationMessage,typeof(CSNetStonetreasure)},
		{(int)ECM.SCDownLocationMessage,typeof(CSNetStonetreasure)},
		{(int)ECM.SCGetNormalAndDownMessage,typeof(CSNetStonetreasure)},
		/*----------------sign----------------*/
		{(int)ECM.ResCardInfoMessage,typeof(CSNetSign)},
		{(int)ECM.ResSignInfoMessage,typeof(CSNetSign)},
		{(int)ECM.ResCardChangeMessage,typeof(CSNetSign)},
		{(int)ECM.ResFinalSignRewardMessage,typeof(CSNetSign)},
		{(int)ECM.ResFragmentChangeMessage,typeof(CSNetSign)},
		{(int)ECM.ResLockCardMessage,typeof(CSNetSign)},
		{(int)ECM.ResCollectionChangeMessage,typeof(CSNetSign)},
		{(int)ECM.ResHonorChangeMessage,typeof(CSNetSign)},
		/*----------------wildadventure----------------*/
		{(int)ECM.SCWildAdventrueMessage,typeof(CSNetWildadventure)},
		{(int)ECM.SCTakeOutItemMessage,typeof(CSNetWildadventure)},
		{(int)ECM.SCBossItemMessage,typeof(CSNetWildadventure)},
		/*----------------rank----------------*/
		{(int)ECM.ResRankInfoMessage,typeof(CSNetRank)},
		/*----------------daycharge----------------*/
		{(int)ECM.SCDayChargeInfoMessage,typeof(CSNetDaycharge)},
		{(int)ECM.SCDayChargeRewardGetMessage,typeof(CSNetDaycharge)},
		/*----------------combine----------------*/
		{(int)ECM.SCCombineItemMessage,typeof(CSNetCombine)},
		/*----------------monthcard----------------*/
		{(int)ECM.SCBuyMonthCardMessage,typeof(CSNetMonthcard)},
		{(int)ECM.SCMonthCardInfoMessage,typeof(CSNetMonthcard)},
		{(int)ECM.SCReceiveMonthCardRewardMessage,typeof(CSNetMonthcard)},
		/*----------------dailypurchase----------------*/
		{(int)ECM.SCDailyPurchaseInfoMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCDailyPurchaseBuyMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCEquipCompetitionPurchaseInfoMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCZhiGouInfoMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCZhiGouOrderMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCGiftBagInfoMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCGiftBagOpenMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCGiftBagCloseMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCLookGiftMessage,typeof(CSNetDailypurchase)},
		{(int)ECM.SCLookPositionMessage,typeof(CSNetDailypurchase)},
		/*----------------lifelongfund----------------*/
		{(int)ECM.SCReceiveFundRewardMessage,typeof(CSNetLifelongfund)},
		{(int)ECM.SCLifelongFundInfoMessage,typeof(CSNetLifelongfund)},
		{(int)ECM.SCFundTaskInfoChangeMessage,typeof(CSNetLifelongfund)},
		/*----------------rankalonetable----------------*/
		{(int)ECM.SCRoleRankInfoMessage,typeof(CSNetRankalonetable)},
		{(int)ECM.SCUnionRankInfoMessage,typeof(CSNetRankalonetable)},
		/*----------------gameversion----------------*/
		{(int)ECM.ClientVersionNtfMessage,typeof(CSNetGameversion)},
		{(int)ECM.ClientUpdateNtfMessage,typeof(CSNetGameversion)},
		/*----------------pet----------------*/
		{(int)ECM.SCPetHpMessage,typeof(CSNetPet)},
		{(int)ECM.SCWoLongPetActiveMessage,typeof(CSNetPet)},
		{(int)ECM.SCNotifyWoLongPetStateMessage,typeof(CSNetPet)},
		{(int)ECM.SCWoLongPetInfoMessage,typeof(CSNetPet)},
		{(int)ECM.SCPlayerWoLongViewInfoMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetInfoMessage,typeof(CSNetPet)},
		{(int)ECM.SCItemCallBackInfoMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetTianFuInfoMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetSkillUpgradeMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetTianFuPassiveSkillMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetTianFuRandomPassiveSkillMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetTianFuChosePassiveSkillMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetTianFuChangeMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetHejiPointMessage,typeof(CSNetPet)},
		{(int)ECM.SCPetActivePvpMessage,typeof(CSNetPet)},
		{(int)ECM.SCCallBackSettingMessage,typeof(CSNetPet)},
		/*----------------athleticsactivity----------------*/
		{(int)ECM.SCAthleticsActivityInfoMessage,typeof(CSNetAthleticsactivity)},
		{(int)ECM.SCReceiveAthleticsActivityRewardMessage,typeof(CSNetAthleticsactivity)},
		{(int)ECM.ActivityRewardInfoChangeNotify,typeof(CSNetAthleticsactivity)},
		/*----------------mafa----------------*/
		{(int)ECM.ResMafaInfoMessage,typeof(CSNetMafa)},
		{(int)ECM.ResMafaExpChangeMessage,typeof(CSNetMafa)},
		{(int)ECM.ResMafaLayerChangeMessage,typeof(CSNetMafa)},
		{(int)ECM.ResMafaSuperLayerUnlockMessage,typeof(CSNetMafa)},
		{(int)ECM.ResMafaBoxRewardMessage,typeof(CSNetMafa)},
		/*----------------code----------------*/
		{(int)ECM.SCCodeRewardMessage,typeof(CSNetCode)},
		/*----------------memory----------------*/
		{(int)ECM.ResMemoryInstanceInfoMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryBagMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryEquipInfoMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryAddMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryEquipChangeMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryEquipSuitMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryEquipGeziChangeMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemoryRemoveMessage,typeof(CSNetMemory)},
		{(int)ECM.ResDiscardMemoryEquipMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemorySummonTeamMessage,typeof(CSNetMemory)},
		{(int)ECM.ResMemorySummonTeamCdMessage,typeof(CSNetMemory)},
		/*----------------unionbattle----------------*/
		{(int)ECM.SCUnionActivityInfoMessage,typeof(CSNetUnionbattle)},
		{(int)ECM.SCUnionActivityInfoChangeMessage,typeof(CSNetUnionbattle)},
		{(int)ECM.SCLastUnionRankMessage,typeof(CSNetUnionbattle)},
		{(int)ECM.SCUnionActivityRewardMessage,typeof(CSNetUnionbattle)},
		/*----------------yuanbao----------------*/
		{(int)ECM.ResYuanBaoInfoMessage,typeof(CSNetYuanbao)},
	};
}

