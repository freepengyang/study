//# generated by tools , Do Not Edit It ...
using System.Collections.Generic;
using System;
using Google.Protobuf;

public static class NetMsgParser
{
	public static void InitNetMsg()
	{
		Dictionary<int, MessageParser> hotDic = NetMsgMain.Instance.mNetInfoDicHot;
		/*----------------heart----------------*/
		hotDic.Add((int)ECM.ResHeartbeatMessage,heart.Heartbeat.Parser);
		/*----------------user----------------*/
		hotDic.Add((int)ECM.ResLoginMessage,user.LoginResponse.Parser);
		hotDic.Add((int)ECM.ResRandomRoleNameMessage,user.RandomRoleNameResponse.Parser);
		hotDic.Add((int)ECM.ResPlayerInfoMessage,user.PlayerInfo.Parser);
		hotDic.Add((int)ECM.ResDeleteRoleMessage,user.RoleIdMsg.Parser);
		hotDic.Add((int)ECM.ResPlayAttributeChangedMessage,user.PlayerAttribute.Parser);
		hotDic.Add((int)ECM.ResPlayerEquipChangedMessage,user.RoleBrief.Parser);
		hotDic.Add((int)ECM.ResDisconnectMessage,user.DisconnectResponse.Parser);
		hotDic.Add((int)ECM.ResPushMessageMessage,user.PushMessageResponse.Parser);
		hotDic.Add((int)ECM.ResExtractInvitationCodeMessage,user.ExtractInvitationCodeResponse.Parser);
		hotDic.Add((int)ECM.CheckCreateRoleArgsVaildAckMessage,user.CheckCreateRoleArgsVaildAck.Parser);
		hotDic.Add((int)ECM.ServerTimeNtfMessage,user.ServerTimeNtf.Parser);
		hotDic.Add((int)ECM.ServerLoadNtfMessage,user.ServerLoadNtf.Parser);
		hotDic.Add((int)ECM.ServerBusyNtfMessage,user.ServerBusyNtf.Parser);
		hotDic.Add((int)ECM.CreateRoleNtfMessage,user.CreateRoleNtf.Parser);
		hotDic.Add((int)ECM.SCPlayerMoveSpeedMessage,user.PlayerMoveSpeed.Parser);
		hotDic.Add((int)ECM.SCRoleListMessage,user.LoginResponse.Parser);
		/*----------------map----------------*/
		hotDic.Add((int)ECM.ResEnterMapMessage,map.EnterMapResponse.Parser);
		hotDic.Add((int)ECM.ResUpdateViewMessage,map.UpdateViewResponse.Parser);
		hotDic.Add((int)ECM.ResObjectExitViewMessage,map.ObjectExitViewResponse.Parser);
		hotDic.Add((int)ECM.ResPlayerEnterViewMessage,map.RoundPlayer.Parser);
		hotDic.Add((int)ECM.ResMonsterEnterViewMessage,map.RoundMonster.Parser);
		hotDic.Add((int)ECM.ResObjectMoveMessage,map.ObjectMoveResponse.Parser);
		hotDic.Add((int)ECM.ResItemEnterViewMessage,map.RoundItem.Parser);
		hotDic.Add((int)ECM.ResNPCEnterViewMessage,map.RoundNPC.Parser);
		hotDic.Add((int)ECM.ResPositionChangeMessage,map.PositionChangeResponse.Parser);
		hotDic.Add((int)ECM.ResChangeMapMessage,map.EnterMapResponse.Parser);
		hotDic.Add((int)ECM.ResAdjustPositionMessage,map.PositionChangeResponse.Parser);
		hotDic.Add((int)ECM.ResReliveMessage,map.ReliveResponse.Parser);
		hotDic.Add((int)ECM.ResPlayerHPChangedMessage,map.PlayerHPChanged.Parser);
		hotDic.Add((int)ECM.ResBufferEnterViewMessage,map.RoundBuffer.Parser);
		hotDic.Add((int)ECM.ResItemsDropMessage,map.ItemsDropResponse.Parser);
		hotDic.Add((int)ECM.ResItemOwnerChangedMessage,map.RoundItem.Parser);
		hotDic.Add((int)ECM.ResPetEnterViewMessage,map.RoundPet.Parser);
		hotDic.Add((int)ECM.ResGuardEnterViewMessage,map.RoundGuard.Parser);
		hotDic.Add((int)ECM.ResPetStateChangedMessage,map.PetStateChanged.Parser);
		hotDic.Add((int)ECM.ResTriggerEnterViewMessage,map.RoundTrigger.Parser);
		hotDic.Add((int)ECM.ResObjectChangePositionMessage,map.ObjectMoveResponse.Parser);
		hotDic.Add((int)ECM.ResBossOwnerChangedMessage,map.RoundMonster.Parser);
		hotDic.Add((int)ECM.ResBoxEnterViewMessage,map.RoundBox.Parser);
		hotDic.Add((int)ECM.ResSafeAreaCoordEnterViewMessage,map.RoundSafeAreaCoord.Parser);
		hotDic.Add((int)ECM.ResMapBossMessage,map.MapBossInfo.Parser);
		hotDic.Add((int)ECM.ResWeatherChangeMessage,map.WeatherChangeResponse.Parser);
		hotDic.Add((int)ECM.SmallViewTeammateNtfMessage,map.SmallViewTeammateNtf.Parser);
		hotDic.Add((int)ECM.MonsterAppearanceChangedNtfMessage,map.RoundMonster.Parser);
		hotDic.Add((int)ECM.NpcsStatNtfMessage,map.NpcsStatNtf.Parser);
		hotDic.Add((int)ECM.PlayerStateNtfMessage,map.PlayerStateNtf.Parser);
		hotDic.Add((int)ECM.ResPetShapeChangeNtf,map.RoundPet.Parser);
		hotDic.Add((int)ECM.SCMapDetailsMessage,map.MapDetails.Parser);
		hotDic.Add((int)ECM.SCGoldKeyPickUpItemMessage,map.GoldKeyPickUpItems.Parser);
		hotDic.Add((int)ECM.SCMainTaskTransmitEventMessage,map.MainTaskTransmitEvent.Parser);
		hotDic.Add((int)ECM.SCAddMainTaskTransmitEventMessage,map.AddMainTaskTransmitEvent.Parser);
		hotDic.Add((int)ECM.SCRemoveMainTaskTransmitEventMessage,map.RemoveMainTaskTransmitEvent.Parser);
		/*----------------bag----------------*/
		hotDic.Add((int)ECM.ResGetBagInfoMessage,bag.BagInfo.Parser);
		hotDic.Add((int)ECM.ResBagItemChangedMessage,bag.BagItemChangeList.Parser);
		hotDic.Add((int)ECM.ResWealthAmountChangeMessage,bag.WealthAmountChangeResponse.Parser);
		hotDic.Add((int)ECM.ResEquipItemMessage,bag.EquipItemMsg.Parser);
		hotDic.Add((int)ECM.ResSortItemsMessage,bag.BagItemChangeList.Parser);
		hotDic.Add((int)ECM.ResSwapItemMessage,bag.SwapItemMsg.Parser);
		hotDic.Add((int)ECM.ResUnEquipItemMessage,bag.UnEquipItemResponse.Parser);
		hotDic.Add((int)ECM.ResCallBackItemMessage,bag.CallbackItemResponse.Parser);
		hotDic.Add((int)ECM.ResPickupItemMessage,bag.PickupMsg.Parser);
		hotDic.Add((int)ECM.ResBagIsFullMessage,bag.BagIsFull.Parser);
		hotDic.Add((int)ECM.ResBagToStorehouseMessage,bag.BagToStorehouseResponse.Parser);
		hotDic.Add((int)ECM.ResChangeBagCountMessage,bag.ChangeBagCount.Parser);
		hotDic.Add((int)ECM.ItemUseLimitNtfMessage,bag.ItemUseLimitNtf.Parser);
		hotDic.Add((int)ECM.LimitItemInfoNtfMessage,baglimit.LimitItemInfoNtf.Parser);
		hotDic.Add((int)ECM.EquipItemModifyNBValueNtfMessage,baglimit.EquipItemModifyNBValueNtf.Parser);
		hotDic.Add((int)ECM.EquipRebuildNtfMessage,bag.EquipRebuildNtf.Parser);
		hotDic.Add((int)ECM.EquipXiLianNtfMessage,bag.SCEquipRandomsNtf.Parser);
		hotDic.Add((int)ECM.SCChooseXiLianResultNtfMessage,bag.SCChooseXiLianResultNtf.Parser);
		hotDic.Add((int)ECM.SCNotifyBagItemCdInfoMessage,bag.BagItemCdInfo.Parser);
		hotDic.Add((int)ECM.SCItemUsedDailyMessage,bag.ResItemUsedDaily.Parser);
		hotDic.Add((int)ECM.SCItemUsedDailyTotalMessage,bag.ResItemUsedDailyTotal.Parser);
		/*----------------tip----------------*/
		hotDic.Add((int)ECM.ResTipMessage,tip.TipResponse.Parser);
		hotDic.Add((int)ECM.ResBulletinMessage,tip.BulletinResponse.Parser);
		hotDic.Add((int)ECM.TipNtfMessage,tip.TipNtf.Parser);
		hotDic.Add((int)ECM.SCNotifyNoteInfoMessage,tip.NotifyNoteInfo.Parser);
		/*----------------fight----------------*/
		hotDic.Add((int)ECM.ResSkillEffectMessage,fight.SkillEffect.Parser);
		hotDic.Add((int)ECM.ResAddBufferMessage,fight.BufferChanged.Parser);
		hotDic.Add((int)ECM.ResRemoveBufferMessage,fight.BufferChanged.Parser);
		hotDic.Add((int)ECM.ResBufferDeltaHPMessage,fight.BufferDeltaHP.Parser);
		hotDic.Add((int)ECM.SCAddSkillMessage,fight.SkillIdInfo.Parser);
		hotDic.Add((int)ECM.SCRemoveSkillMessage,fight.SkillIdInfo.Parser);
		hotDic.Add((int)ECM.SCSkillShortCutMessage,fight.SaveSkillShortCutRequest.Parser);
		hotDic.Add((int)ECM.ResBufferInfoMessage,fight.BufferChanged.Parser);
		hotDic.Add((int)ECM.ResRemoveSkillMessage,fight.RemoveSkillMsg.Parser);
		hotDic.Add((int)ECM.PkModeChangedNtfMessage,fight.PkModeChangedNtf.Parser);
		hotDic.Add((int)ECM.SCUpgradeSkillMessage,fight.SCUpgradeSkillInfo.Parser);
		hotDic.Add((int)ECM.BufferRemoveRemindNtfMessage,fight.BufferRemoveRemindNtf.Parser);
		hotDic.Add((int)ECM.ClearSkillCDNtfMessage,fight.ClearSkillCDNtf.Parser);
		hotDic.Add((int)ECM.SCSetSkillAutoStateMessage,fight.SkillAutoState.Parser);
		hotDic.Add((int)ECM.SCUpdateSkillRefixMessage,user.SkillRefixInfo.Parser);
		hotDic.Add((int)ECM.SCPlayerFsmStateMessage,fight.PlayerFsmState.Parser);
		/*----------------gm----------------*/
		hotDic.Add((int)ECM.ResCloseServerMessage,user.CloseServerResponse.Parser);
		hotDic.Add((int)ECM.ResReloadScriptMessage,user.ReloadScriptResponse.Parser);
		/*----------------mail----------------*/
		hotDic.Add((int)ECM.ResMailListMessage,mail.MailList.Parser);
		hotDic.Add((int)ECM.ResNewMailMessage,mail.MailInfo.Parser);
		hotDic.Add((int)ECM.ResGetMailItemMessage,mail.MailIdMsg.Parser);
		hotDic.Add((int)ECM.ResDeleteMailMessage,mail.MailIdMsg.Parser);
		/*----------------player----------------*/
		hotDic.Add((int)ECM.ResRoleExpUpdatedMessage,player.RoleExpUpdated.Parser);
		hotDic.Add((int)ECM.ResRoleUpgradeMessage,player.RoleUpgrade.Parser);
		hotDic.Add((int)ECM.ResRoleExtraValuesMessage,player.RoleExtraValues.Parser);
		hotDic.Add((int)ECM.ResDayPassedMessage,player.DayPassed.Parser);
		hotDic.Add((int)ECM.ResOtherPlayerInfoMessage,user.OtherPlayerInfo.Parser);
		hotDic.Add((int)ECM.ResCommonMessage,player.CommonInfo.Parser);
		hotDic.Add((int)ECM.ResOtherPlayerCommonInfoMessage,user.OtherPlayerCommonInfo.Parser);
		hotDic.Add((int)ECM.RoleAttrNtfMessage,player.RoleAttrNtf.Parser);
		hotDic.Add((int)ECM.ResPlayerDieMessage,player.PlayerDie.Parser);
		hotDic.Add((int)ECM.ResPkValueUpdateMessage,player.PkValueUpdate.Parser);
		hotDic.Add((int)ECM.SCRoleBriefMessage,player.ResRoleBrief.Parser);
		hotDic.Add((int)ECM.SCPkValueChangeMessage,player.PkValueChange.Parser);
		hotDic.Add((int)ECM.SCPkGreyNameStateMessage,player.PkGreyNameState.Parser);
		/*----------------paodian----------------*/
		hotDic.Add((int)ECM.ResUPaoDianChangeMessage,paodian.PaoDianChange.Parser);
		hotDic.Add((int)ECM.SCPaoDianInfoMessage,paodian.PaoDianChange.Parser);
		hotDic.Add((int)ECM.SCRandomPaoDianMessage,paodian.RandomPaoDian.Parser);
		hotDic.Add((int)ECM.SCPaoDianExpMessage,paodian.PaoDianExp.Parser);
		/*----------------chat----------------*/
		hotDic.Add((int)ECM.ResChatMessage,chat.ChatMessage.Parser);
		hotDic.Add((int)ECM.LeftCornerTipNtfMessage,chat.LeftCornerTipNtf.Parser);
		hotDic.Add((int)ECM.ReleaseNtfMessage,chat.ReleaseNtf.Parser);
		hotDic.Add((int)ECM.VoiceRoomNtfMessage,chat.VoiceRoomNtf.Parser);
		hotDic.Add((int)ECM.RoleDetailNtfMessage,chat.RoleDetailNtf.Parser);
		hotDic.Add((int)ECM.ForbidChatNtfMessage,chat.ForbidChatNtf.Parser);
		hotDic.Add((int)ECM.BigExpressionNtfMessage,chat.BigExpressionNtf.Parser);
		/*----------------team----------------*/
		hotDic.Add((int)ECM.ResTeamInfoMessage,team.TeamInfo.Parser);
		hotDic.Add((int)ECM.ResApplyTeamMessage,team.TeamMember.Parser);
		hotDic.Add((int)ECM.ResInviteTeamMessage,team.InviteTeamMsg.Parser);
		hotDic.Add((int)ECM.ResJoinTeamMessage,team.JoinTeamResponse.Parser);
		hotDic.Add((int)ECM.ResLeaveTeamMessage,team.LeaveTeamResponse.Parser);
		hotDic.Add((int)ECM.ResTeamLeaderChangedMessage,team.TeamLeaderChanged.Parser);
		hotDic.Add((int)ECM.ResGetTeamInfoMessage,team.GetTeamInfoResponse.Parser);
		hotDic.Add((int)ECM.ResGetTeamTabMessage,team.TeamTabInfo.Parser);
		hotDic.Add((int)ECM.ResTeamCallBackMessage,team.CallBack.Parser);
		hotDic.Add((int)ECM.TeamTargetAckMessage,team.TeamTargetAck.Parser);
		hotDic.Add((int)ECM.TeamCallBackAckMessage,team.TeamCallBackAck.Parser);
		hotDic.Add((int)ECM.SCSetTeamModeMessage,team.SetTeamModeRequest.Parser);
		hotDic.Add((int)ECM.SCPlayerHpMpInfoMessage,team.PlayerHpMpInfo.Parser);
		hotDic.Add((int)ECM.SCPlayerLevelInfoMessage,team.PlayerLevelInfo.Parser);
		/*----------------instance----------------*/
		hotDic.Add((int)ECM.ResBuyInstanceTimesMessage,player.RoleExtraValues.Parser);
		hotDic.Add((int)ECM.ResInstanceInfoMessage,instance.InstanceInfo.Parser);
		hotDic.Add((int)ECM.SCEnterInstanceMessage,instance.InstanceInfo.Parser);
		hotDic.Add((int)ECM.SCInstanceFinishMessage,instance.InstanceInfo.Parser);
		hotDic.Add((int)ECM.SCLeaveInstanceMessage,instance.InstanceInfo.Parser);
		hotDic.Add((int)ECM.ResQuickInstanceMessage,instance.QuickInstanceInfoMsg.Parser);
		hotDic.Add((int)ECM.ResInstanceCountMessage,instance.InstanceCount.Parser);
		hotDic.Add((int)ECM.ResDiLaoInfoMessage,instance.DiLaoInfo.Parser);
		hotDic.Add((int)ECM.SCUndergroundTreasureMessage,instance.UndergroundTreasureInstanceInfo.Parser);
		hotDic.Add((int)ECM.SCBossChallengeMessage,instance.BossChallengeInfo.Parser);
		hotDic.Add((int)ECM.SCDropLimitMessage,instance.ResDropLimit.Parser);
		/*----------------storehouse----------------*/
		hotDic.Add((int)ECM.ResGetStorehouseInfoMessage,storehouse.StorehouseInfo.Parser);
		hotDic.Add((int)ECM.ResStorehouseItemChangedMessage,storehouse.StorehouseItemChangeList.Parser);
		hotDic.Add((int)ECM.ResSortStorehouseMessage,storehouse.StorehouseItemChangeList.Parser);
		hotDic.Add((int)ECM.ResExchangeItemMessage,storehouse.ExchangeItemMsg.Parser);
		hotDic.Add((int)ECM.ResStorehouseToBagMessage,storehouse.StorehouseToBagResponse.Parser);
		hotDic.Add((int)ECM.ResAddStorehouseCountMessage,storehouse.AddStorehouseCount.Parser);
		/*----------------wolong----------------*/
		hotDic.Add((int)ECM.SCWoLongInfoMessage,wolong.WoLongInfo.Parser);
		hotDic.Add((int)ECM.SCWoLongLevelUpMessage,wolong.WoLongLevelUpgradeResponse.Parser);
		hotDic.Add((int)ECM.SCSkillGroupInfoMessage,wolong.SkillGroupInfoResponse.Parser);
		hotDic.Add((int)ECM.SCWoLongXiLianMessage,wolong.WoLongXiLianResponse.Parser);
		hotDic.Add((int)ECM.SCWoLongXiLianSelectMessage,wolong.WoLongXiLianSelectResponse.Parser);
		hotDic.Add((int)ECM.SCSoldierSoulInfoAwakenMessage,wolong.SoldierSoulInfo.Parser);
		hotDic.Add((int)ECM.SCSoldierSoulInfoMessage,wolong.SoldierSoulInfoResponse.Parser);
		/*----------------task----------------*/
		hotDic.Add((int)ECM.ResTaskListMessage,task.TaskList.Parser);
		hotDic.Add((int)ECM.ResTaskStateChangedMessage,task.TaskInfo.Parser);
		hotDic.Add((int)ECM.ResTaskGoalUpdatedMessage,task.TaskGoalUpdateResponse.Parser);
		hotDic.Add((int)ECM.ResAcceptTaskMessage,task.TaskInfo.Parser);
		hotDic.Add((int)ECM.ResSubmitTaskMessage,task.SubmitTaskResponse.Parser);
		hotDic.Add((int)ECM.SCCycTaskMessage,task.TaskList.Parser);
		hotDic.Add((int)ECM.SCNewTaskMessage,task.NewTaskResponse.Parser);
		/*----------------energy----------------*/
		hotDic.Add((int)ECM.SCEnergyInfoMessage,energy.EnergyInfoResponse.Parser);
		hotDic.Add((int)ECM.SCEnergyFreeGetInfoMessage,energy.EnergyFreeGetInfoResponse.Parser);
		hotDic.Add((int)ECM.SCGetFreeEnergyMessage,energy.GetFreeEnergyRequest.Parser);
		hotDic.Add((int)ECM.SCNotifyEnergyChangeMessage,energy.NotifyEnergyChangeResponse.Parser);
		hotDic.Add((int)ECM.SCEnergyExchangeInfoMessage,energy.EnergyExchangeInfo.Parser);
		/*----------------systemcontroller----------------*/
		hotDic.Add((int)ECM.SystemFunctionStateNtfMessage,systemcontroller.SystemFunctionStateNtf.Parser);
		hotDic.Add((int)ECM.SCRoleFunctionDataMessage,systemcontroller.RoleFunctionData.Parser);
		/*----------------fengyin----------------*/
		hotDic.Add((int)ECM.SCFengYinOpenMessage,fengyin.FengYinOpen.Parser);
		hotDic.Add((int)ECM.SCFengYinTimeShortenMessage,fengyin.FengYinOpen.Parser);
		hotDic.Add((int)ECM.SCFengYinCloseMessage,fengyin.FengYinClose.Parser);
		hotDic.Add((int)ECM.SCHuanJingOpenMessage,fengyin.HuanJingOpen.Parser);
		hotDic.Add((int)ECM.SCHuanJingCloseMessage,fengyin.HuanJingClose.Parser);
		hotDic.Add((int)ECM.SCHuanJingChangeMessage,fengyin.HuanJingChange.Parser);
		hotDic.Add((int)ECM.ResWorldLevelMessage,fengyin.WorldLevel.Parser);
		/*----------------social----------------*/
		hotDic.Add((int)ECM.ResSocialInfoMessage,social.SocialInfo.Parser);
		hotDic.Add((int)ECM.ResAddRelationMessage,social.AddRelationResponse.Parser);
		hotDic.Add((int)ECM.ResFindPlayerByNameMessage,social.FindPlayerByNameResponse.Parser);
		hotDic.Add((int)ECM.ApplyFriendNotifyMessage,social.ApplyFriendList.Parser);
		hotDic.Add((int)ECM.ResDeleteRelationMessage,social.DeleteRelationRequest.Parser);
		hotDic.Add((int)ECM.ResGetAllSocialInfoMessage,social.RelationAllResponse.Parser);
		hotDic.Add((int)ECM.RejectSingleAckMessage,social.RejectSingleAck.Parser);
		hotDic.Add((int)ECM.QueryLatelyTouchAckMessage,social.QueryLatelyTouchAck.Parser);
		hotDic.Add((int)ECM.SCSettingMessage,social.ResSetting.Parser);
		/*----------------shop----------------*/
		hotDic.Add((int)ECM.SCShopBuyInfoMessage,shop.ShopBuyInfoResponse.Parser);
		hotDic.Add((int)ECM.SCShopBuyMessage,shop.ShopBuyResponse.Parser);
		hotDic.Add((int)ECM.SCShopInfoMessage,shop.ShopInfoResponse.Parser);
		hotDic.Add((int)ECM.SCDailyRmbInfoMessage,shop.ResDailyRmbInfo.Parser);
		hotDic.Add((int)ECM.SCDuiHuanShopInfoMessage,shop.ResDuiHuanShopInfo.Parser);
		hotDic.Add((int)ECM.SCDuiHuanShopInfoByIdMessage,shop.DuiHuanShopInfo.Parser);
		/*----------------luck----------------*/
		/*----------------tujian----------------*/
		hotDic.Add((int)ECM.SCTujianInfoMessage,tujian.TujianInfoResponse.Parser);
		hotDic.Add((int)ECM.SCTujianUpLevelMessage,tujian.TujianUpLevelResponse.Parser);
		hotDic.Add((int)ECM.SCTujianUpQualityMessage,tujian.TujianUpQualityResponse.Parser);
		hotDic.Add((int)ECM.SCTujianInlayMessage,tujian.TujianInlayResponse.Parser);
		hotDic.Add((int)ECM.SCActivateSlotWingMessage,tujian.ActivateSlotResponse.Parser);
		hotDic.Add((int)ECM.SCTujianAddMessage,tujian.TujianAddResponse.Parser);
		hotDic.Add((int)ECM.SCTujianRemoveMessage,tujian.TujianRemoveResponse.Parser);
		/*----------------worldboss----------------*/
		hotDic.Add((int)ECM.SCWorldBossActivityInfoResponseMessage,worldboss.ActivityInfo.Parser);
		hotDic.Add((int)ECM.SCJoinWorldBossActivityResponseMessage,worldboss.JoinActivityResponse.Parser);
		hotDic.Add((int)ECM.SCNotifyWorldBossRankInfoMessage,worldboss.DamageRank.Parser);
		hotDic.Add((int)ECM.SCWorldBossBlessInfoMessage,worldboss.BlessInfo.Parser);
		hotDic.Add((int)ECM.SCWorldBossBossInfoMessage,worldboss.BossInfo.Parser);
		/*----------------activity----------------*/
		hotDic.Add((int)ECM.ResActivityDataMessage,activity.ActivityInfo.Parser);
		hotDic.Add((int)ECM.SCCollectActivityDataMessage,activity.CollectActivityDatas.Parser);
		hotDic.Add((int)ECM.ResActiveRewardMessage,activity.ResActiveReward.Parser);
		hotDic.Add((int)ECM.ResActiveMessage,activity.ResActive.Parser);
		hotDic.Add((int)ECM.ResFengYinDataMessage,activity.ResFengYinData.Parser);
		hotDic.Add((int)ECM.ResSpecialActivityDataMessage,activity.SpecialActivityData.Parser);
		hotDic.Add((int)ECM.SCBossFirstKillDatasMessage,activity.BossFirstKillDatasResponse.Parser);
		hotDic.Add((int)ECM.SCEquipXuanShangMessage,activity.ResEquipXuanShang.Parser);
		hotDic.Add((int)ECM.ResSevenDayDataMessage,activity.SevenDayData.Parser);
		hotDic.Add((int)ECM.ResEquipCompetitionMessage,activity.EquipCompetition.Parser);
		hotDic.Add((int)ECM.SCBossKuangHuanMessage,activity.ResBossKuangHuan.Parser);
		hotDic.Add((int)ECM.SCKillDemonMessage,activity.ResKillDemon.Parser);
		hotDic.Add((int)ECM.SCSpecialActivityOpenInfoMessage,activity.SCSpecialActivityOpenInfo.Parser);
		hotDic.Add((int)ECM.SCSevenLoginMessage,activity.ResSevenLogin.Parser);
		/*----------------ultimate----------------*/
		hotDic.Add((int)ECM.SCUltimateInfoMessage,ultimate.RoleUltimateData.Parser);
		hotDic.Add((int)ECM.SCResetUltimateMessage,ultimate.RoleUltimateData.Parser);
		hotDic.Add((int)ECM.SCSelectAdditionEffectMessage,ultimate.SelectAdditionEffect.Parser);
		hotDic.Add((int)ECM.SCRankInfoMessage,ultimate.ResponseRankInfo.Parser);
		hotDic.Add((int)ECM.SCSelectAdditionIndexMessage,ultimate.OpState.Parser);
		hotDic.Add((int)ECM.SCResponseCardMessage,ultimate.SelectAdditionEffect.Parser);
		hotDic.Add((int)ECM.SCOpenCardInfoMessage,ultimate.OpState.Parser);
		hotDic.Add((int)ECM.SCSelectCardIndexMessage,ultimate.OpState.Parser);
		hotDic.Add((int)ECM.SCGodBlessMessage,ultimate.AddHp.Parser);
		hotDic.Add((int)ECM.SCUltimatePassInfoMessage,ultimate.UltimatePassInfo.Parser);
		/*----------------boss----------------*/
		hotDic.Add((int)ECM.SCBossInfoMessage,boss.ChallengeBossInfoResponse.Parser);
		/*----------------fashion----------------*/
		hotDic.Add((int)ECM.SCAllFashionInfoMessage,fashion.AllFashionInfo.Parser);
		hotDic.Add((int)ECM.SCEquipFashionMessage,fashion.FashionIdList.Parser);
		hotDic.Add((int)ECM.SCFashionStarLevelUpMessage,fashion.FashionInfo.Parser);
		hotDic.Add((int)ECM.SCAddFashionMessage,fashion.FashionInfo.Parser);
		hotDic.Add((int)ECM.SCRemoveFashionMessage,fashion.FashionIdList.Parser);
		hotDic.Add((int)ECM.SCUnEquipFashionMessage,fashion.FashionId.Parser);
		/*----------------wing----------------*/
		hotDic.Add((int)ECM.SCWingInfoMessage,wing.WingInfoResponse.Parser);
		hotDic.Add((int)ECM.SCWingUpStarMessage,wing.WingUpStarResponse.Parser);
		hotDic.Add((int)ECM.SCWingAdvanceMessage,wing.WingAdvanceResponse.Parser);
		hotDic.Add((int)ECM.SCDressColorWingMessage,wing.DressColorWingResponse.Parser);
		hotDic.Add((int)ECM.SCWingExpItemUseMessage,wing.WingExpItemUseResponse.Parser);
		hotDic.Add((int)ECM.SCColorWingChangeMessage,wing.WingColorChange.Parser);
		hotDic.Add((int)ECM.SCYuLingInfoMessage,wing.ResYuLingInfo.Parser);
		/*----------------lianti----------------*/
		hotDic.Add((int)ECM.SCLianTiInfoMessage,lianti.LianTiInfoResponse.Parser);
		hotDic.Add((int)ECM.SCLianTiFieldMessage,lianti.LianTiFieldResponse.Parser);
		hotDic.Add((int)ECM.SCLianTiUpLevelMessage,lianti.LianTiInfoResponse.Parser);
		/*----------------auction----------------*/
		hotDic.Add((int)ECM.ResGetAuctionItemsMessage,auction.AllAuctionItems.Parser);
		hotDic.Add((int)ECM.ResGetAuctionShelfMessage,auction.SelfAuctionItems.Parser);
		hotDic.Add((int)ECM.ResAddToShelfMessage,auction.AddToShelfResponse.Parser);
		hotDic.Add((int)ECM.ResReAddToShelfMessage,auction.ResReAddToShelf.Parser);
		hotDic.Add((int)ECM.ResRemoveFromShelfMessage,auction.ItemIdMsg.Parser);
		hotDic.Add((int)ECM.ResBuyAuctionItemMessage,auction.ItemIdMsg.Parser);
		hotDic.Add((int)ECM.ResUnlockAuctionShelveMessage,auction.UnlockAuctionShelve.Parser);
		hotDic.Add((int)ECM.ResAttentionAuctionMessage,auction.ItemIdList.Parser);
		/*----------------union----------------*/
		hotDic.Add((int)ECM.SCGetUnionInfoMessage,union.UnionInfo.Parser);
		hotDic.Add((int)ECM.SCUnionListMessage,union.UnionList.Parser);
		hotDic.Add((int)ECM.SCCreateUnionMessage,union.UnionInfo.Parser);
		hotDic.Add((int)ECM.SCInviteUnionMessage,union.InviteUnionMsg.Parser);
		hotDic.Add((int)ECM.SCUnionPositionChangedMessage,union.ChangePositionMsg.Parser);
		hotDic.Add((int)ECM.SCLeaveUnionMessage,union.LeaveUnionResponse.Parser);
		hotDic.Add((int)ECM.SCUnionDonateGoldMessage,union.UnionDonateResponse.Parser);
		hotDic.Add((int)ECM.SCUnionDonateEquipMessage,union.UnionDonateResponse.Parser);
		hotDic.Add((int)ECM.SCJoinUnionMessage,union.UnionInfo.Parser);
		hotDic.Add((int)ECM.SCUnionExchangeEquipMessage,union.UnionExchangeEquipResponse.Parser);
		hotDic.Add((int)ECM.SCUnionDeclareWarMessage,union.DeclareWarResponse.Parser);
		hotDic.Add((int)ECM.SCUnionWarTimeoutMessage,union.WarTimeout.Parser);
		hotDic.Add((int)ECM.SCGetUnionTabMessage,union.GetUnionTabResponse.Parser);
		hotDic.Add((int)ECM.SCGetSouvenirWealthMessage,union.GetSouvenirWealthResponse.Parser);
		hotDic.Add((int)ECM.SCUnionInfoUpdatedMessage,union.UnionInfoUpdate.Parser);
		hotDic.Add((int)ECM.SCUnionJoinInfoMessage,union.JoinList.Parser);
		hotDic.Add((int)ECM.SCImpeachementMessage,union.ImpeachementMsg.Parser);
		hotDic.Add((int)ECM.SCCanApplyUnionMessage,union.canApplyUnions.Parser);
		hotDic.Add((int)ECM.SCChangeLeaderMessage,union.ApplyChangeLeader.Parser);
		hotDic.Add((int)ECM.SCQueryCombineUnionMessage,union.combineUnion.Parser);
		hotDic.Add((int)ECM.SCUnionBulletinAckMessage,union.UnionBulletinAck.Parser);
		hotDic.Add((int)ECM.SCImpeachmentEndNtfMessage,union.ImpeachmentEndNtf.Parser);
		hotDic.Add((int)ECM.SCRoleApplyUnionMessage,union.ApplyUnionListResponse.Parser);
		hotDic.Add((int)ECM.SCImproveInfosMessage,union.ImproveInfos.Parser);
		hotDic.Add((int)ECM.SCImproveMessage,union.ImproveResponse.Parser);
		hotDic.Add((int)ECM.SCUnionDestroyItemMessage,union.UnionDestroyItemResponse.Parser);
		hotDic.Add((int)ECM.SCSplitYuanbaoMessage,union.SplitYuanbaoResponse.Parser);
		hotDic.Add((int)ECM.ResUpdateSpeakLimitsMessage,union.UpdateCanSpeakMsg.Parser);
		hotDic.Add((int)ECM.ResUnionCallInfoMessage,union.UnionCallInfo.Parser);
		/*----------------intensify----------------*/
		hotDic.Add((int)ECM.SCIntensifyMessage,intensify.IntensifyResponse.Parser);
		hotDic.Add((int)ECM.SCIntensifyInfoMessage,intensify.IntensifyInfoResponse.Parser);
		hotDic.Add((int)ECM.SCIntensifySuitInfoMessage,intensify.IntensifySuitInfoResponse.Parser);
		/*----------------baozhu----------------*/
		hotDic.Add((int)ECM.SCLevelUpBaoZhuMessage,baozhu.ResLevelUpBaoZhu.Parser);
		hotDic.Add((int)ECM.SCGradeUpBaoZhuMessage,baozhu.ResGradeUpBaoZhu.Parser);
		hotDic.Add((int)ECM.SCBaoZhuSkillsMessage,baozhu.BaoZhuSkills.Parser);
		hotDic.Add((int)ECM.SCChoseBaoZhuSkillMessage,baozhu.BaoZhuSkills.Parser);
		hotDic.Add((int)ECM.SCBaoZhuBossCountChangeMessage,bag.EquipInfo.Parser);
		hotDic.Add((int)ECM.SCBaoZhuSlotSkillsMessage,baozhu.BaoZhuSkills.Parser);
		/*----------------sabac----------------*/
		hotDic.Add((int)ECM.SCSabacDataInfoMessage,sabac.SabacDataResponse.Parser);
		hotDic.Add((int)ECM.SCNotifySabacStateMessage,sabac.SabacStateResponse.Parser);
		hotDic.Add((int)ECM.SCSabacRankInfoMessage,sabac.ResponseRankInfo.Parser);
		hotDic.Add((int)ECM.SCSabacResultMessage,sabac.SabacResultResponse.Parser);
		hotDic.Add((int)ECM.SCSabacTransDoorInfoMessage,map.RoundTrigger.Parser);
		/*----------------vip----------------*/
		hotDic.Add((int)ECM.ResVipMessage,vip.VipInfo.Parser);
		hotDic.Add((int)ECM.ResVipTasteCardMessage,vip.ExperienceCardInfo.Parser);
		hotDic.Add((int)ECM.SCVipTasteCardMessage,vip.ExperienceCardInfo.Parser);
		hotDic.Add((int)ECM.SCFirstRechargeInfoMessage,vip.FirstRechargeInfoResponse.Parser);
		hotDic.Add((int)ECM.FirstRechargeNtfMessage,vip.FirstRechargeNtf.Parser);
		hotDic.Add((int)ECM.AccumulatedRechargeInfoMessage,vip.AccumulatedRechargeInfo.Parser);
		hotDic.Add((int)ECM.SCMonthRechargeInfoMessage,vip.ResMonthRechargeInfo.Parser);
		hotDic.Add((int)ECM.SCMonthRechargeInfoByIdMessage,vip.MonthRechargeInfo.Parser);
		/*----------------gem----------------*/
		hotDic.Add((int)ECM.SCPosGemChangeMessage,gem.PosGemChange.Parser);
		hotDic.Add((int)ECM.SCPosGemInfoMessage,gem.PosGemInfo.Parser);
		hotDic.Add((int)ECM.SCUnlockGemPositionMessage,gem.UnlockGemPosition.Parser);
		hotDic.Add((int)ECM.SCGemSuitMessage,gem.GemSuit.Parser);
		hotDic.Add((int)ECM.SCGemBossCountChangeMessage,gem.GemBossCountChange.Parser);
		/*----------------treasurehunt----------------*/
		hotDic.Add((int)ECM.ResServerHistoryMessage,treasurehunt.ServerHistory.Parser);
		hotDic.Add((int)ECM.ResTreasureItemChangedMessage,treasurehunt.TreasureItemChangeList.Parser);
		hotDic.Add((int)ECM.ResTreasureStorehouseMessage,treasurehunt.TreasureHuntInfo.Parser);
		hotDic.Add((int)ECM.ResTreasureEndMessage,treasurehunt.TreasureEndResponse.Parser);
		hotDic.Add((int)ECM.ResUseTreasureExpMessage,treasurehunt.ExpUseRequest.Parser);
		hotDic.Add((int)ECM.ResTreasureIdMessage,treasurehunt.TreasureIdResponse.Parser);
		hotDic.Add((int)ECM.ResHuntCallBackMessage,treasurehunt.HuntCallbackResponse.Parser);
		/*----------------stonetreasure----------------*/
		hotDic.Add((int)ECM.SCFloorInfoMessage,stonetreasure.FloorInfoResponse.Parser);
		hotDic.Add((int)ECM.SCStoneLocationMessage,stonetreasure.StoneLocationResponse.Parser);
		hotDic.Add((int)ECM.SCDownLocationMessage,stonetreasure.DownLocationResponse.Parser);
		hotDic.Add((int)ECM.SCGetNormalAndDownMessage,stonetreasure.GetNormalAndDownResponse.Parser);
		/*----------------sign----------------*/
		hotDic.Add((int)ECM.ResCardInfoMessage,sign.CardInfo.Parser);
		hotDic.Add((int)ECM.ResSignInfoMessage,sign.SignInfo.Parser);
		hotDic.Add((int)ECM.ResCardChangeMessage,sign.CardChange.Parser);
		hotDic.Add((int)ECM.ResFinalSignRewardMessage,sign.ResFinalSignReward.Parser);
		hotDic.Add((int)ECM.ResFragmentChangeMessage,sign.FragmentChange.Parser);
		hotDic.Add((int)ECM.ResLockCardMessage,sign.LockCard.Parser);
		hotDic.Add((int)ECM.ResCollectionChangeMessage,sign.CollectionChange.Parser);
		hotDic.Add((int)ECM.ResHonorChangeMessage,sign.HonorChange.Parser);
		/*----------------wildadventure----------------*/
		hotDic.Add((int)ECM.SCWildAdventrueMessage,wildadventure.WildAdventureInfo.Parser);
		hotDic.Add((int)ECM.SCTakeOutItemMessage,wildadventure.TakeOutItemResponse.Parser);
		hotDic.Add((int)ECM.SCBossItemMessage,wildadventure.BossItemNotify.Parser);
		/*----------------rank----------------*/
		hotDic.Add((int)ECM.ResRankInfoMessage,rank.RankInfo.Parser);
		/*----------------daycharge----------------*/
		hotDic.Add((int)ECM.SCDayChargeInfoMessage,daycharge.DayChargeResponse.Parser);
		hotDic.Add((int)ECM.SCDayChargeRewardGetMessage,daycharge.GetRewardResponse.Parser);
		/*----------------combine----------------*/
		hotDic.Add((int)ECM.SCCombineItemMessage,combine.CombineItemResponse.Parser);
		/*----------------monthcard----------------*/
		hotDic.Add((int)ECM.SCBuyMonthCardMessage,monthcard.BuyMonthCardResponse.Parser);
		hotDic.Add((int)ECM.SCMonthCardInfoMessage,monthcard.MonthCardInfoResponse.Parser);
		hotDic.Add((int)ECM.SCReceiveMonthCardRewardMessage,monthcard.ReceiveMonthCardRewardResponse.Parser);
		/*----------------dailypurchase----------------*/
		hotDic.Add((int)ECM.SCDailyPurchaseInfoMessage,dailypurchase.DailyPurchaseInfoResponse.Parser);
		hotDic.Add((int)ECM.SCDailyPurchaseBuyMessage,dailypurchase.DailyPurchaseBuyResponse.Parser);
		hotDic.Add((int)ECM.SCEquipCompetitionPurchaseInfoMessage,dailypurchase.DailyPurchaseInfoResponse.Parser);
		hotDic.Add((int)ECM.SCZhiGouInfoMessage,dailypurchase.ZhiGouInfo.Parser);
		hotDic.Add((int)ECM.SCZhiGouOrderMessage,dailypurchase.ZhiGouOrder.Parser);
		hotDic.Add((int)ECM.SCGiftBagInfoMessage,dailypurchase.GiftBagList.Parser);
		hotDic.Add((int)ECM.SCGiftBagOpenMessage,dailypurchase.GiftBagList.Parser);
		hotDic.Add((int)ECM.SCGiftBagCloseMessage,dailypurchase.GiftBagList.Parser);
		hotDic.Add((int)ECM.SCLookGiftMessage,dailypurchase.DailyPurchaseInfoResponse.Parser);
		hotDic.Add((int)ECM.SCLookPositionMessage,dailypurchase.LookPositionInfo.Parser);
		/*----------------lifelongfund----------------*/
		hotDic.Add((int)ECM.SCReceiveFundRewardMessage,lifelongfund.ReceiveFundRewardResponse.Parser);
		hotDic.Add((int)ECM.SCLifelongFundInfoMessage,lifelongfund.LifelongFundInfo.Parser);
		hotDic.Add((int)ECM.SCFundTaskInfoChangeMessage,lifelongfund.FundTaskInfoChange.Parser);
		/*----------------rankalonetable----------------*/
		hotDic.Add((int)ECM.SCRoleRankInfoMessage,rankalonetable.RoleRankInfoResponse.Parser);
		hotDic.Add((int)ECM.SCUnionRankInfoMessage,rankalonetable.UnionRankInfoResponse.Parser);
		/*----------------gameversion----------------*/
		hotDic.Add((int)ECM.ClientVersionNtfMessage,gameversion.ClientVersionNtf.Parser);
		hotDic.Add((int)ECM.ClientUpdateNtfMessage,gameversion.UpdateState.Parser);
		/*----------------pet----------------*/
		hotDic.Add((int)ECM.SCPetHpMessage,pet.PetHpInfo.Parser);
		hotDic.Add((int)ECM.SCWoLongPetActiveMessage,pet.WoLongPetInfo.Parser);
		hotDic.Add((int)ECM.SCNotifyWoLongPetStateMessage,pet.WoLongPetState.Parser);
		hotDic.Add((int)ECM.SCWoLongPetInfoMessage,pet.WoLongPetInfo.Parser);
		hotDic.Add((int)ECM.SCPlayerWoLongViewInfoMessage,pet.PlayerPetViewInfo.Parser);
		hotDic.Add((int)ECM.SCPetInfoMessage,pet.ResPetInfo.Parser);
		hotDic.Add((int)ECM.SCItemCallBackInfoMessage,pet.ResItemCallBackInfo.Parser);
		hotDic.Add((int)ECM.SCPetTianFuInfoMessage,pet.PetTianFuInfo.Parser);
		hotDic.Add((int)ECM.SCPetSkillUpgradeMessage,pet.ResPetSkillUpgrade.Parser);
		hotDic.Add((int)ECM.SCPetTianFuPassiveSkillMessage,pet.PetTianFuPassiveSkillList.Parser);
		hotDic.Add((int)ECM.SCPetTianFuRandomPassiveSkillMessage,pet.PetTianFuPassiveSkill.Parser);
		hotDic.Add((int)ECM.SCPetTianFuChosePassiveSkillMessage,pet.PetTianFuPassiveSkill.Parser);
		hotDic.Add((int)ECM.SCPetTianFuChangeMessage,pet.PetTianFuChange.Parser);
		hotDic.Add((int)ECM.SCPetHejiPointMessage,pet.PetHejiPointChange.Parser);
		hotDic.Add((int)ECM.SCPetActivePvpMessage,pet.ActivePvpInfo.Parser);
		hotDic.Add((int)ECM.SCCallBackSettingMessage,pet.CallBackSetting.Parser);
		/*----------------athleticsactivity----------------*/
		hotDic.Add((int)ECM.SCAthleticsActivityInfoMessage,athleticsactivity.AthleticsActivityInfoResponse.Parser);
		hotDic.Add((int)ECM.SCReceiveAthleticsActivityRewardMessage,athleticsactivity.ActivityRewardInfo.Parser);
		hotDic.Add((int)ECM.ActivityRewardInfoChangeNotify,athleticsactivity.ActivityRewardInfoChange.Parser);
		/*----------------mafa----------------*/
		hotDic.Add((int)ECM.ResMafaInfoMessage,mafa.MafaInfo.Parser);
		hotDic.Add((int)ECM.ResMafaExpChangeMessage,mafa.MafaExpChange.Parser);
		hotDic.Add((int)ECM.ResMafaLayerChangeMessage,mafa.MafaLayerList.Parser);
		hotDic.Add((int)ECM.ResMafaSuperLayerUnlockMessage,mafa.MafaSuperLayerUnlock.Parser);
		hotDic.Add((int)ECM.ResMafaBoxRewardMessage,mafa.MafaBoxReward.Parser);
		/*----------------code----------------*/
		hotDic.Add((int)ECM.SCCodeRewardMessage,code.ResCodeReward.Parser);
		/*----------------memory----------------*/
		hotDic.Add((int)ECM.ResMemoryInstanceInfoMessage,memory.MemoryInstanceInfo.Parser);
		hotDic.Add((int)ECM.ResMemoryBagMessage,memory.MemoryBag.Parser);
		hotDic.Add((int)ECM.ResMemoryEquipInfoMessage,memory.MemoryEquipInfo.Parser);
		hotDic.Add((int)ECM.ResMemoryAddMessage,memory.MemoryAdd.Parser);
		hotDic.Add((int)ECM.ResMemoryEquipChangeMessage,memory.MemoryEquipChange.Parser);
		hotDic.Add((int)ECM.ResMemoryEquipSuitMessage,memory.MemoryEquipSuit.Parser);
		hotDic.Add((int)ECM.ResMemoryEquipGeziChangeMessage,memory.MemoryEquipGeziChange.Parser);
		hotDic.Add((int)ECM.ResMemoryRemoveMessage,memory.MemoryRemove.Parser);
		hotDic.Add((int)ECM.ResDiscardMemoryEquipMessage,memory.MemoryEquipId.Parser);
		hotDic.Add((int)ECM.ResMemorySummonTeamMessage,memory.MemorySummonTeam.Parser);
		hotDic.Add((int)ECM.ResMemorySummonTeamCdMessage,memory.MemorySummonTeamCd.Parser);
		/*----------------unionbattle----------------*/
		hotDic.Add((int)ECM.SCUnionActivityInfoMessage,unionbattle.UnionActivityInfoResponse.Parser);
		hotDic.Add((int)ECM.SCUnionActivityInfoChangeMessage,unionbattle.UnionActivityInfo.Parser);
		hotDic.Add((int)ECM.SCLastUnionRankMessage,rank.RankInfo.Parser);
		hotDic.Add((int)ECM.SCUnionActivityRewardMessage,unionbattle.UnionActivityReward.Parser);
		/*----------------yuanbao----------------*/
		hotDic.Add((int)ECM.ResYuanBaoInfoMessage,yuanbao.YuanBaoInfo.Parser);
	}
	private static CSNetBase cSNetBase;
	public static void ProcessNetwork(NetInfo netinfo)
	{
		cSNetBase = CSNetFactory.Get(netinfo.msgId);
		if (cSNetBase != null)
			cSNetBase.NetCallback((ECM)netinfo.msgId, netinfo);
	}
}

