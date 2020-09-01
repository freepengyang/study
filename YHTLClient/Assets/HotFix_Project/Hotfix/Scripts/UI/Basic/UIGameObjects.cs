using System;
using System.Collections.Generic;
using UnityEngine;
public class UIGameObjects
{
    private static UIGameObjects mInstance;
    public static UIGameObjects Instance
    {
        get
        {
            if (mInstance == null) mInstance = new UIGameObjects();
            return mInstance;
        }
        set { mInstance = value; }
    }
    public UIGameObjects()
    {
        Init();
    }
    [System.Serializable]
    public class Entry
    {
        public Type type;
        public string typeName;
        public string path;
        public override string ToString()
        {
            return typeName + " " + path;
        }
    }
    [SerializeField]
    [HideInInspector]
    public List<Entry> gos = new List<Entry>();

    void AddEntry(Type t, string path)
    {
        Entry en = new Entry { type = t, typeName = t.Name, path = path };
        gos.Add(en);
    }

    public bool isfirstInit = true;

    public void addHotFixPanel(Type t, string path)
    {
        for (int i = 0; i < gos.Count; i++)
        {
            if (gos[i].typeName == path)
            {
                return;
            }
        }
        Entry en = new Entry { type = t, typeName = t.Name, path = path };
        gos.Add(en);
    }

    /// <summary>
    /// 注册UI界面
    /// </summary>
    public void Init()
    {
        AddEntry(typeof(UILoading), "UILoading"); //加载
        AddEntry(typeof(UIWaiting), "UIWaiting");
        AddEntry(typeof(UILogin), "UILogin"); //登录
        AddEntry(typeof(UIDownloading), "UIDownloading");
        AddEntry(typeof(UIDebugPanel), "UIDebugPanel");
        AddEntry(typeof(UIServerListPanel), "UIServerListPanel");
        AddEntry(typeof(UILoginRolePanel), "UILoginRolePanel");
        AddEntry(typeof(UIMainSceneManager), "UIMainScenePanel");
        AddEntry(typeof(UIPromptPanel), "UIPromptPanel");
        AddEntry(typeof(UIBagPanel), "UIBagPanel");
        AddEntry(typeof(UIMailPanel), "UIMailPanel");
        AddEntry(typeof(UIEquipTipPanel), "UIEquipTipPanel");
        AddEntry(typeof(UIEquipRecyclePanel), "UIEquipRecyclePanel");
        AddEntry(typeof(UIRolePanel), "UIRolePanel");
        AddEntry(typeof(UIRecycleConfirmPanel), "UIRecycleConfirmPanel");
        AddEntry(typeof(UIRecycleGetPanel), "UIRecycleGetPanel");
        AddEntry(typeof(UIHelpTipsPanel), "UIHelpTipsPanel");
        AddEntry(typeof(UITimeExpPanel), "UITimeExpPanel");
        AddEntry(typeof(UITimeExpStageUpPanel), "UITimeExpStageUpPanel");
        AddEntry(typeof(UIEquipRefinePanel), "UIEquipRefinePanel");
        AddEntry(typeof(UIWaitingAB), "UIWaitingAB");
        AddEntry(typeof(UIRefineResultPanel), "UIRefineResultPanel");
        AddEntry(typeof(UIEquipRecastPanel), "UIEquipRecastPanel");
        AddEntry(typeof(UITimeExpHighStageEffectPanel), "UITimeExpHighStageEffectPanel");
        AddEntry(typeof(UIFunctionPanel), "UIFunctionPanel");
        AddEntry(typeof(UIGMMenuPanel), "UIGMMenuPanel");
        AddEntry(typeof(UITimeExpCombinedPanel), "UITimeExpCombinedPanel");
        AddEntry(typeof(UIChatPanel), "UIChatPanel");
        AddEntry(typeof(UIChatAddPanel), "UIChatAddPanel");
        AddEntry(typeof(UIEquipCombinePanel), "UIEquipCombinePanel");
        AddEntry(typeof(UIBossCombinePanel), "UIBossCombinePanel");
        AddEntry(typeof(UIChatSettingPanel), "ChatSettingPanel");
        AddEntry(typeof(UIChatVoiceTemplate), "UIChatVoiceTemplate");
        AddEntry(typeof(UIChatVoiceListPanel), "UIChatVoiceListPanel");
        AddEntry(typeof(UIColoursWorldPanel), "UIColoursWorldPanel");

        AddEntry(typeof(UIRoleSelectionInfoPanel), "UIRoleSelectionInfoPanel");
        AddEntry(typeof(UIRoleSelectionMenuPanel), "UIRoleSelectionMenuPanel");
        AddEntry(typeof(UIMonsterSelectionDetailedInfoPanel), "UIMonsterSelectionDetailedInfoPanel");
        AddEntry(typeof(UIMonsterSelectionInfoPanel), "UIMonsterSelectionInfoPanel");

        //AddEntry(typeof(UIRelationPanel), "UIRelationPanel");
        AddEntry(typeof(UITeamInvitationPanel), "UITeamInvitationPanel");
        AddEntry(typeof(UISkillTipsPanel), "UISkillTipsPanel");
        AddEntry(typeof(UIMoneyPanel), "UIMoneyPanel");
        AddEntry(typeof(UIMissionPanel), "UIMissionPanel");
        AddEntry(typeof(UIMainFuncPanel), "UIMainFuncPanel");
        AddEntry(typeof(UISkillCombinedPanel), "UISkillCombinedPanel");
        AddEntry(typeof(UISkillPanel), "UISkillPanel");
        AddEntry(typeof(UIPropItemPanel), "UIPropItemPanel");
        AddEntry(typeof(UIVigorPanel), "UIVigorPanel");
        AddEntry(typeof(UISkillUpgradeEffectPanel), "UISkillUpgradeEffectPanel");
        //AddEntry(typeof(UISealPanel), "UISealPanel");
        AddEntry(typeof(UIUnsealRewardPanel), "UIUnsealRewardPanel");
        AddEntry(typeof(UIRoleDragonPanel), "UIRoleDragonPanel");
        AddEntry(typeof(UIRoleAttrInfoPanel), "UIRoleAttrInfoPanel");
        AddEntry(typeof(UIBuffPanel), "UIBuffPanel");
        AddEntry(typeof(UINPCDialogPanel), "UINPCDialogPanel");
        AddEntry(typeof(UIRelationCombinedPanel), "UIRelationCombinedPanel");
        AddEntry(typeof(UIFriendResponseTipsPanel), "UIFriendResponseTipsPanel");
        AddEntry(typeof(UISealCombinedPanel), "UISealCombinedPanel");
        AddEntry(typeof(UISealGradePanel), "UISealGradePanel");
        AddEntry(typeof(UIRelivePanel), "UIRelivePanel");
        AddEntry(typeof(UIConfirmTeamInaitation), "UIConfirmTeamInaitation");
        AddEntry(typeof(UIEquipZhuFuPanel), "UIEquipZhuFuPanel");
        AddEntry(typeof(UIDreamLandPanel), "UIDreamLandPanel");
        AddEntry(typeof(UIWorldBossInstancePanel), "UIWorldBossInstancePanel");
        AddEntry(typeof(UIFastAccessPanel), "UIFastAccessPanel");
        AddEntry(typeof(UIBuyConfirmPanel), "UIBuyConfirmPanel");
        AddEntry(typeof(UIHandBookCombinedPanel), "UIHandBookCombinedPanel");
        AddEntry(typeof(UIHandBookCardSelectPanel), "UIHandBookCardSelectPanel");
        AddEntry(typeof(UIWorldInspirePanel), "UIWorldInspirePanel");
        AddEntry(typeof(UIFashionPanel), "UIFashionPanel");
        AddEntry(typeof(UIDailyNPCCostPanel), "UIDailyNPCCostPanel");

        AddEntry(typeof(UIDealPkValuePanel), "UIDealPkValuePanel");
        AddEntry(typeof(UISpringPaoDianInstacePanel), "UISpringPaoDianInstacePanel");
        AddEntry(typeof(UISpringPaoDianPanel), "UISpringPaoDianPanel");
        AddEntry(typeof(UIHandBookChoicePanel), "UIHandBookChoicePanel");
        AddEntry(typeof(UIShopPanel), "UIShopPanel");
        AddEntry(typeof(UIAllAttributeTipsPanel), "UIAllAttributeTipsPanel");
        AddEntry(typeof(UIFashionLevelUPPanel), "UIFashionLevelUPPanel");
        AddEntry(typeof(UIActivityCombinedPanel), "UIActivityCombinedPanel");
        AddEntry(typeof(UIDailyActivityTipsPanel), "UIDailyActivityTipsPanel");
        AddEntry(typeof(UIActivityHalllTipsPanel), "UIActivityHalllTipsPanel");
        AddEntry(typeof(UIAuctionPanel), "UIAuctionPanel");
        AddEntry(typeof(UILianTiPanel), "UILianTiPanel");
        AddEntry(typeof(UIUltimateChallengePanel), "UIUltimateChallengePanel");
        AddEntry(typeof(UIWingCombinedPanel), "UIWingCombinedPanel");
        AddEntry(typeof(UIWingAdvanceSuccessPanel), "UIWingAdvanceSuccessPanel");
        AddEntry(typeof(UIHandBookGroupTipsPanel), "UIHandBookGroupTipsPanel");
        AddEntry(typeof(UIHandBookTipsPanel), "UIHandBookTipsPanel");
        AddEntry(typeof(UIUltimateChallengeRankPanel), "UIUltimateChallengeRankPanel");
        AddEntry(typeof(UIAuctionOperationPanel), "UIAuctionOperationPanel");
        AddEntry(typeof(UILianTiLandPanel), "UILianTiLandPanel");
        AddEntry(typeof(UIDeadGrayPanel), "UIDeadGrayPanel");
        AddEntry(typeof(UIDungeonInstacePanel), "UIDungeonInstacePanel");
        AddEntry(typeof(UIUltimateChallengeAttributePanel), "UIUltimateChallengeAttributePanel");
        AddEntry(typeof(UIWoLongActivityPanel), "UIWoLongActivityPanel");
        AddEntry(typeof(UICostPromptPanel), "UICostPromptPanel");
        AddEntry(typeof(UIUltimateChallengeGainEffectPanel), "UIUltimateChallengeGainEffectPanel");
        AddEntry(typeof(UIUltimateChallengStorePanel), "UIUltimateChallengStorePanel");
        AddEntry(typeof(UIUltimateChallengCardPanel), "UIUltimateChallengCardPanel");
        AddEntry(typeof(UIUltimateChallengDialogPanel), "UIUltimateChallengDialogPanel");
        AddEntry(typeof(UINoticePanel), "UINoticePanel");
        AddEntry(typeof(UINoticeSecondPanel), "UINoticeSecondPanel");
        AddEntry(typeof(UINoticeBelowPanel), "UINoticeBelowPanel");
        AddEntry(typeof(UINoticeBottomPanel), "UINoticeBottomPanel");
        AddEntry(typeof(UINoticeColoursPanel), "UINoticeColoursPanel");
        AddEntry(typeof(UIDungeonPanel), "UIDungeonPanel");
        AddEntry(typeof(UIDungeonInspirePanel), "UIDungeonInspirePanel");
        AddEntry(typeof(UIDungeonSkillTipsPanel), "UIDungeonSkillTipsPanel");
        AddEntry(typeof(UIGuildCombinedPanel), "UIGuildCombinedPanel");
        AddEntry(typeof(UIGuildInfoPanel), "UIGuildInfoPanel");
        AddEntry(typeof(UIGuildMemberListPanel), "UIGuildMemberListPanel");
        AddEntry(typeof(UIGuildBagPanel), "UIGuildBagPanel");
        AddEntry(typeof(UIGuildPracticePanel), "UIGuildPracticePanel");
        AddEntry(typeof(UIGuildWealPanel), "UIGuildWealPanel");
        AddEntry(typeof(UIGuildListPanel), "UIGuildListPanel");
        AddEntry(typeof(UICreateGuildPanel), "UICreateGuildPanel");
        AddEntry(typeof(UIPromptItemSplitPanel), "UIPromptItemSplitPanel");
        AddEntry(typeof(UIDreamInstancePanel), "UIDreamInstancePanel");
        AddEntry(typeof(UIGuildDonatePanel), "UIGuildDonatePanel");
        AddEntry(typeof(UISealAddtionPanel), "UISealAddtionPanel");
        AddEntry(typeof(UIRenameGuildPanel), "UIRenameGuildPanel");
        AddEntry(typeof(UIDungeonSiegeInstancePanel), "UIDungeonSiegeInstancePanel");
        AddEntry(typeof(UIUndergroundTreasureInstancePanel), "UIUndergroundTreasureInstancePanel");
        AddEntry(typeof(UIAccuseChiefPanel), "UIAccuseChiefPanel");
        AddEntry(typeof(UIPetActivePanel), "UIPetActivePanel");
        AddEntry(typeof(UIMapCombinePanel), "UIMapCombinePanel");
        AddEntry(typeof(UIUndergroundTreasurePanel), "UIUndergroundTreasurePanel");

        AddEntry(typeof(UIRewardPromptPanel), "UIRewardPromptPanel");
        AddEntry(typeof(UIAttributeChangePanel), "UIAttributeChangePanel");
        AddEntry(typeof(UIGuilApplyListPanel), "UIGuilApplyListPanel");
        AddEntry(typeof(UIPearlCombinedPanel), "UIPearlCombinedPanel");
        AddEntry(typeof(UIPearSkillslotlPreviewPanel), "UIPearSkillslotlPreviewPanel");
        AddEntry(typeof(UIEnhanceResultPanel), "UIEnhanceResultPanel");
        AddEntry(typeof(UIGuildListGroupPanel), "UIGuildListGroupPanel");
		AddEntry(typeof(UIRandomThingPanel), "UIRandomThingPanel");
        AddEntry(typeof(UINewPlayerPlacePanel), "UINewPlayerPlacePanel");
        AddEntry(typeof(UIConfigPanel), "UIConfigPanel");
        AddEntry(typeof(UIGuildBagDonatePanel), "UIGuildBagDonatePanel");
        AddEntry(typeof(UIGemPanel), "UIGemPanel");
        AddEntry(typeof(UIChooseGuildPanel), "UIChooseGuildPanel");
		AddEntry(typeof(UIRandomThingInstancePanel), "UIRandomThingInstancePanel");
		AddEntry(typeof(UIRandomBtnPanel), "UIRandomBtnPanel");
        AddEntry(typeof(UIGemItemListPanel), "UIGemItemListPanel");
        AddEntry(typeof(UIGemStarSuitTipPanel), "UIGemStarSuitTipPanel");
        AddEntry(typeof(UIGuildPositionPanel), "UIGuildPositionPanel");
        AddEntry(typeof(UIWoLongXiLianCombinePanel), "UIWoLongXiLianCombinePanel");
        AddEntry(typeof(UIGemLevelUpPanel), "UIGemLevelUpPanel");
        AddEntry(typeof(UIForceIncreasePanel), "UIForceIncreasePanel");
		AddEntry(typeof(UITombTreasurePanel), "UITombTreasurePanel");
        AddEntry(typeof(UIGuildFightCombinedPanel), "UIGuildFightCombinedPanel");
        AddEntry(typeof(UISeekTreasureCombinedPanel), "UISeekTreasureCombinedPanel");
	
        AddEntry(typeof(UIDailyActivityRewardPanel), "UIDailyActivityRewardPanel");
        AddEntry(typeof(UIDailyCombinedPanel), "UIDailyCombinedPanel");
        AddEntry(typeof(UIDailySignInPromptPanel), "UIDailySignInPromptPanel");
        AddEntry(typeof(UIDailySignInCardTipsPanel), "UIDailySignInCardTipsPanel");
        AddEntry(typeof(UIDailySignInCombinedPanel), "UIDailySignInCombinedPanel");
        AddEntry(typeof(UIDailySignInPreviewPanel), "UIDailySignInPreviewPanel");
        AddEntry(typeof(UIDailySignInTipsPanel), "UIDailySignInTipsPanel");
        AddEntry(typeof(UIDailySignInAwardPanel), "UIDailySignInAwardPanel");
        AddEntry(typeof(UITombTreasureTipsPanel), "UITombTreasureTipsPanel");
        AddEntry(typeof(UIGuildFightFinalAwardPanel), "UIGuildFightFinalAwardPanel");
        AddEntry(typeof(UIGuildScoreListPanel), "UIGuildScoreListPanel");
        AddEntry(typeof(UIGuildFightRulePanel), "UIGuildFightRulePanel");
        AddEntry(typeof(UIRechargeFirstPanel), "UIRechargeFirstPanel");
        AddEntry(typeof(UISummonPanel), "UISummonPanel");
        AddEntry(typeof(UIGuildTreasureCabinetPanel), "UIGuildTreasureCabinetPanel");
        AddEntry(typeof(UIGuildDevidePanel), "UIGuildDevidePanel");
        AddEntry(typeof(UIServerActivityPanel), "UIServerActivityPanel");
        AddEntry(typeof(UIServerActivityBossKillPanel), "UIServerActivityBossKillPanel");
        AddEntry(typeof(UIVIPPanel), "UIVIPPanel");
        AddEntry(typeof(UIServerActivitySealPanel), "UIServerActivitySealPanel");
        AddEntry(typeof(UIVIPExperiencePanel), "UIVIPExperiencePanel");
        AddEntry(typeof(UILifeTimeFundPanel), "UILifeTimeFundPanel");
        AddEntry(typeof(UIGuildFightStatusPanel), "UIGuildFightStatusPanel");
        AddEntry(typeof(UIServerActivityGiftPanel), "UIServerActivityGiftPanel");
        AddEntry(typeof(UIWildEscapadePanel), "UIWildEscapadePanel");
        AddEntry(typeof(UIGuidePanel), "UIGuidePanel");
        AddEntry(typeof(UIWildEscapadeAwardPanel), "UIWildEscapadeAwardPanel");
        AddEntry(typeof(UIServerActivityEquipPanel), "UIServerActivityEquipPanel");
        AddEntry(typeof(UIServerActivityRankPanel), "UIServerActivityRankPanel");
        AddEntry(typeof(UISevenDayTrialPanel), "UISevenDayTrialPanel");
        AddEntry(typeof(UIWelfareActivityPanel), "UIWelfareActivityPanel");
		AddEntry(typeof(UIServerActivityRechargePanel), "UIServerActivityRechargePanel");
        AddEntry(typeof(UIServerActivityMonthCardPanel), "UIServerActivityMonthCardPanel");
        // AddEntry(typeof(UICompoundPanel), "UICompoundPanel");
        AddEntry(typeof(UICompoundPrompPanel), "UICompoundPrompPanel");
        AddEntry(typeof(UIHonorChanllengeCombinePanel), "UIHonorChanllengeCombinePanel");
        AddEntry(typeof(UIHonorChanllengeRankPanel), "UIHonorChanllengeRankPanel");
        AddEntry(typeof(UIMonthCardGiftPanel), "UIMonthCardGiftPanel");
		AddEntry(typeof(UIServerActivityEquipRewardsPanel), "UIServerActivityEquipRewardsPanel");
        AddEntry(typeof(UIHonorResultPromptPanel), "UIHonorResultPromptPanel");
		AddEntry(typeof(UIServerActivityGiftBagPanel), "UIServerActivityGiftBagPanel");
		AddEntry(typeof(UIHonorPromotionPromptPanel), "UIHonorPromotionPromptPanel");

		AddEntry(typeof(UIServerActivityBossPanel), "UIServerActivityBossPanel");
        AddEntry(typeof(UIGiftPromptPanel), "UIGiftPromptPanel");
        AddEntry(typeof(UIServerActivitySlayPanel), "UIServerActivitySlayPanel");
		AddEntry(typeof(UIBuyBossKuangHuanPanel), "UIBuyBossKuangHuanPanel");
        AddEntry(typeof(UIArmRacePanel), "UIArmRacePanel");
        AddEntry(typeof(UIRankingCombinedPanel), "UIRankingCombinedPanel");
		AddEntry(typeof(UIServerActivityMapPanel), "UIServerActivityMapPanel");
        AddEntry(typeof(UIArmRacePromptPanel), "UIArmRacePromptPanel");
        AddEntry(typeof(UINewFunctionPanel), "UINewFunctionPanel");
        AddEntry(typeof(UIComingFunctionPanel), "UIComingFunctionPanel");
        AddEntry(typeof(UIGetItemPanel), "UIGetItemPanel");
        AddEntry(typeof(UICheckInfoCombinePanel), "UICheckInfoCombinePanel");
		AddEntry(typeof(UINPCDayChargeMapPanel), "UINPCDayChargeMapPanel");
        AddEntry(typeof(UINewSkillPanel), "UINewSkillPanel");
        AddEntry(typeof(UIUltimateInstacePanel), "UIUltimateInstacePanel");
		AddEntry(typeof(UIRewardFlyEffectPanel), "UIRewardFlyEffectPanel");
		AddEntry(typeof(UIMissionGuildPanel), "UIMissionGuildPanel");
        AddEntry(typeof(UIRenamePanel), "UIRenamePanel");
        AddEntry(typeof(UISkillDescriptionPanel), "UISkillDescriptionPanel");
        AddEntry(typeof(UIOfficialNoticePanel), "UIOfficialNoticePanel");
        AddEntry(typeof(UIServerActivityDownloadPanel), "UIServerActivityDownloadPanel");
        AddEntry(typeof(UIAgreementPanel), "UIAgreementPanel");
		AddEntry(typeof(UIAgreementTipsPanel), "UIAgreementTipsPanel");
		AddEntry(typeof(UIRoleEquipObtainPanel), "UIRoleEquipObtainPanel");
		AddEntry(typeof(UIUpdateExitGamePanel), "UIUpdateExitGamePanel");
        AddEntry(typeof(UIShopCombinePanel), "UIShopCombinePanel");
        AddEntry(typeof(UIServerActivityGuildPanel), "UIServerActivityGuildPanel");

		AddEntry(typeof(UIFastAccessTwoPanel), "UIFastAccessTwoPanel");
        AddEntry(typeof(UIFastUsePanel), "UIFastUsePanel");
        AddEntry(typeof(UIWarPetCombinedPanel), "UIWarPetCombinedPanel");
        AddEntry(typeof(UIWarPetSkillPanel), "UIWarPetSkillPanel");
        AddEntry(typeof(UIExitInstanceCountDownPanel), "UIExitInstanceCountDownPanel");
        AddEntry(typeof(UICountDownLeavePanel), "UICountDownLeavePanel");
        AddEntry(typeof(UIWarPetSkillPromptPanel), "UIWarPetSkillPromptPanel");
        AddEntry(typeof(UIPetLevelUpSetPanel), "UIPetLevelUpSetPanel");
        AddEntry(typeof(UIPetLevelUpPanel), "UIPetLevelUpPanel");
        AddEntry(typeof(UIWarPetSkillTipsPanel), "UIWarPetSkillTipsPanel");
		AddEntry(typeof(UIWarSoulPropDescPanel), "UIWarSoulPropDescPanel");
		AddEntry(typeof(UIPetSuitTipPanel), "UIPetSuitTipPanel");
		AddEntry(typeof(UIPetBaseEquipGetWayPanel), "UIPetBaseEquipGetWayPanel");
        AddEntry(typeof(UIWoLongRefineResultPanel), "UIWoLongRefineResultPanel");
        AddEntry(typeof(UIMissionEffectPanel), "UIMissionEffectPanel");
        AddEntry(typeof(UIWoLongSkillTipsPanel), "UIWoLongSkillTipsPanel");
        AddEntry(typeof(UIPetLevelUpPromptPanel), "UIPetLevelUpPromptPanel");
        AddEntry(typeof(UIPetTalentPreviewPanel), "UIPetTalentPreviewPanel");
        AddEntry(typeof(UIBloodMaskPanel), "UIBloodMaskPanel");
        AddEntry(typeof(UITotemPanel), "UITotemPanel");
		AddEntry(typeof(UIPetBasePreviewPanel), "UIPetBasePreviewPanel");
        AddEntry(typeof(UIPetTalentTipsPanel), "UIPetTalentTipsPanel");
        AddEntry(typeof(UIStrengthenCombinedPanel), "UIStrengthenCombinedPanel");
        AddEntry(typeof(UIMoneyTipsPanel), "UIMoneyTipsPanel");
        AddEntry(typeof(UIAddUpRechargePanel), "UIAddUpRechargePanel");
        AddEntry(typeof(UIWelfareDirectPurchasePanel), "UIWelfareDirectPurchasePanel");
        AddEntry(typeof(UIDirectPurchaseRewardPanel), "UIDirectPurchaseRewardPanel");
		AddEntry(typeof(UIMaFaPanel), "UIMaFaPanel");
        AddEntry(typeof(UIRedNameNPCDialogPanel), "UIRedNameNPCDialogPanel");
        AddEntry(typeof(UISpecialShopCombinePanel), "UISpecialShopCombinePanel");
        AddEntry(typeof(UIRechargeShopPanel), "UIRechargeShopPanel");
        AddEntry(typeof(UIWelfareGiftBagPanel), "UIWelfareGiftBagPanel");
        AddEntry(typeof(UIGiftBagPreviewPanel), "UIGiftBagPreviewPanel");
        AddEntry(typeof(UIBuyVigorPanel), "UIBuyVigorPanel");
		AddEntry(typeof(UIMaFaGetWayPanel), "UIMaFaGetWayPanel");
        AddEntry(typeof(UIPromptForcePanel), "UIPromptForcePanel");
        AddEntry(typeof(UIFastExchangePanel), "UIFastExchangePanel");
        AddEntry(typeof(UIWelfareMonthMapPanel), "UIWelfareMonthMapPanel");
        AddEntry(typeof(UIDailyArenaCombinePanel), "UIDailyArenaCombinePanel");
        AddEntry(typeof(UIWelfareVIPMapPanel), "UIWelfareVIPMapPanel");
		AddEntry(typeof(UIMaFaPromptPanel), "UIMaFaPromptPanel");
		AddEntry(typeof(UIVIPMapPanel), "UIVIPMapPanel");
		AddEntry(typeof(UIMapInstancePanel), "UIMapInstancePanel");
        AddEntry(typeof(UIPetTalentSkillPreviewPanel), "UIPetTalentSkillPreviewPanel");
        AddEntry(typeof(UILongLiTipsPanel), "UILongLiTipsPanel");
        AddEntry(typeof(UILongJiTipsPanel), "UILongJiTipsPanel");
        AddEntry(typeof(UIWoLongPromptPanel), "UIWoLongPromptPanel");
        AddEntry(typeof(UIWelfareActivationCodePanel), "UIWelfareActivationCodePanel");
        AddEntry(typeof(UIBillboardPanel), "UIBillboardPanel");
		AddEntry(typeof(UIFastAccessWoLongPanel), "UIFastAccessWoLongPanel");
        AddEntry(typeof(UILiquidSettingPanel), "UILiquidSettingPanel");
        AddEntry(typeof(UIHandBookPackagePanel), "UIHandBookPackagePanel");

		AddEntry(typeof(UIRecallSecretCombinedPanel), "UIRecallSecretCombinedPanel");
        AddEntry(typeof(UISevenLoginPanel), "UISevenLoginPanel");
		AddEntry(typeof(UIGiveMeIngotPanel), "UIGiveMeIngotPanel");
		AddEntry(typeof(UINostalgiaEquipPanel), "UINostalgiaEquipPanel");
        AddEntry(typeof(UIGuildRankInstancePanel), "UIGuildRankInstancePanel");
        AddEntry(typeof(UIGuildBossInstancePanel), "UIGuildBossInstancePanel");
        AddEntry(typeof(UIGuildEndActivityPanel), "UIGuildEndActivityPanel");
        
        AddEntry(typeof(UIWingSoulDetailedAttrsPanel), "UIWingSoulDetailedAttrsPanel");
        AddEntry(typeof(UIWingSoulSeeDetailsPanel), "UIWingSoulSeeDetailsPanel");
        AddEntry(typeof(UIWingSoulChangeMosaicPanel), "UIWingSoulChangeMosaicPanel");
        AddEntry(typeof(UIWingSoulPromotePanel), "UIWingSoulPromotePanel");
        AddEntry(typeof(UINostalgiaSuitTipPanel), "UINostalgiaSuitTipPanel");
        AddEntry(typeof(UIFastAccessVigorPanel), "UIFastAccessVigorPanel");
        AddEntry(typeof(UINostalgiaTipPanel), "UINostalgiaTipPanel");
        AddEntry(typeof(UINostalgiaPromptPanel), "UINostalgiaPromptPanel");
        AddEntry(typeof(UINostalgiaMaterialPanel), "UINostalgiaMaterialPanel");
        AddEntry(typeof(UICompleteWayPanel), "UICompleteWayPanel");
        AddEntry(typeof(UIPlayerInfoPanel), "UIPlayerInfoPanel");
        AddEntry(typeof(UIMonsterInfoPanel), "UIMonsterInfoPanel");
        AddEntry(typeof(UIMainTeamPanel), "UIMainTeamPanel");
        AddEntry(typeof(UIEquipRewardShowPanel), "UIEquipRewardShowPanel");
        AddEntry(typeof(UIRecoverDialogPanel), "UIRecoverDialogPanel");
    }
}