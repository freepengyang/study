using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using rank;
using UnityEngine;

public partial class UISealGradePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    SealGradeData mySealData;
    private RankInfo rankInfo;
    long remainingTime = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.ShortenSeal, ShortenSeal);
        mClientEvent.Reg((uint) CEvent.CloseSeal, CloseSeal);
        mClientEvent.Reg((uint) CEvent.RankInfo, ShowRankInfo);
        UIEventListener.Get(mbtn_reward1.gameObject, 1).onClick = OnClickReward;
        UIEventListener.Get(mbtn_reward2.gameObject, 2).onClick = OnClickReward;
        UIEventListener.Get(mbtn_reward3.gameObject, 3).onClick = OnClickReward;
        UIEventListener.Get(mroleModel1.gameObject, 0).onClick = OnClickRoleModel;
        UIEventListener.Get(mroleModel2.gameObject, 1).onClick = OnClickRoleModel;
        UIEventListener.Get(mroleModel3.gameObject, 2).onClick = OnClickRoleModel;
        mbtn_check_heartlaw.onClick = OnClickCheckHeartlaw;
        mbtn_rule.onClick = OnClickRule;
        mbtn_addition.onClick = OnClickAddition;
        //排行榜每10分钟请求一次
        ScriptBinder.InvokeRepeating2(0f, 60f, OnRankInfo);
    }

    void OnClickRoleModel(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        if (rankInfo != null && rankInfo.ranks.Count > index)
        {
            RankData rankData = rankInfo.ranks[index];
            Net.ReqGetOtherPlayerInfoMessage(rankData.rid);
        }
    }

    /// <summary>
    /// 显示排行榜
    /// </summary>
    void ShowRankInfo(uint id, object data)
    {
        // Debug.Log("------------------------------显示排行榜");
        if (data == null) return;
        rankInfo = (RankInfo) data;
        ShowRoleModelAndInfo();
    }


    void ShowRoleModelAndInfo()
    {
        mobj_noplayer1.SetActive(true);
        mroleModel1.gameObject.SetActive(false);
        mobj_noplayer2.SetActive(true);
        mroleModel2.gameObject.SetActive(false);
        mobj_noplayer3.SetActive(true);
        mroleModel3.gameObject.SetActive(false);
        for (int i = 0; i < rankInfo.ranks.Count; i++)
        {
            RankData rankData = rankInfo.ranks[i];
            switch (i)
            {
                case 0:
                    mobj_noplayer1.SetActive(false);
                    mroleModel1.gameObject.SetActive(true);
                    if (rankData.paramList.Count == 7)
                    {
                        RepeatedField<int> paramList = rankData.paramList;
                        AvatarModelHelper.LoadAvatarModel(msp_model1, msp_weapen1, /*msp_title1*/null,
                            paramList[0], paramList[1], paramList[2],
                            paramList[3], paramList[4] /*18010*/, paramList[5], paramList[6]);
                    }

                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_title1, 17922);
                    mlb_name1.text = rankData.name;
                    mlb_heartlaw_level1.text =
                        CSString.Format(1859, LianTiTableManager.Instance.GetLianTiName(rankData.value),
                            rankData.value);
                    break;
                case 1:
                    mobj_noplayer2.SetActive(false);
                    mroleModel2.gameObject.SetActive(true);
                    if (rankData.paramList.Count == 7)
                    {
                        RepeatedField<int> paramList = rankData.paramList;
                        AvatarModelHelper.LoadAvatarModel(msp_model2, msp_weapen2, /*msp_title2*/null,
                            paramList[0], paramList[1], paramList[2],
                            paramList[3], paramList[4], paramList[5], paramList[6]);
                    }

                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_title2, 17920);
                    mlb_name2.text = rankData.name;
                    mlb_heartlaw_level2.text =
                        CSString.Format(1859, LianTiTableManager.Instance.GetLianTiName(rankData.value),
                            rankData.value);
                    break;
                case 2:
                    mobj_noplayer3.SetActive(false);
                    mroleModel3.gameObject.SetActive(true);
                    if (rankData.paramList.Count == 7)
                    {
                        RepeatedField<int> paramList = rankData.paramList;
                        AvatarModelHelper.LoadAvatarModel(msp_model3, msp_weapen3, /*msp_title3*/null,
                            paramList[0], paramList[1], paramList[2],
                            paramList[3], paramList[4], paramList[5], paramList[6]);
                    }

                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_title3, 17921);
                    mlb_name3.text = rankData.name;
                    mlb_heartlaw_level3.text =
                        CSString.Format(1859, LianTiTableManager.Instance.GetLianTiName(rankData.value),
                            rankData.value);
                    break;
            }
        }
    }

    /// <summary>
    /// 接收缩短封印时间广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void ShortenSeal(uint id, object data)
    {
        if (data == null) return;
        ScriptBinder.StopInvokeRepeating();
        InitData();
    }

    /// <summary>
    /// 接收关闭封印广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void CloseSeal(uint id, object data)
    {
        if (data == null) return;
        UIManager.Instance.ClosePanel<UISealCombinedPanel>();
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_title, "seal_time");
        CSEffectPlayMgr.Instance.ShowUIEffect(msp_title_effect, 17090);
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_bg, "seal_bg");
        InitData();
    }

    void InitData()
    {
        mySealData = CSSealGradeInfo.Instance.MySealData;
        if (mySealData == null) return;
        rankInfo = CSRankInfo.Instance.GetRankByType((int) RankType.SealGrade);

        TABLE.LEVELSEAL tableLevelSeal;
        if (!LevelSealTableManager.Instance.TryGetValue(mySealData.level, out tableLevelSeal)) return;

        //封印时间(倒计时)
        remainingTime = mySealData.endTime / 1000 - CSServerTime.Instance.TotalSeconds; //剩余时间（秒）

        ScriptBinder.InvokeRepeating(0f, 1f, OnDesParticle);

        //加速天数
        mlb_speed_up.text = CSString.Format(577, mySealData.speedupTime);
        //特效
        msp_title_effect.SetActive(mySealData.speedupTime > 0);
        //封印等级
        mlb_seal_level.text = CSString.Format(578, mySealData.level);
        //触发封印玩家
        mlb_trigger_seal_name.text = CSString.Format(554, mySealData.roleName);
        //解封加速条件
        mlb_unseal_speedup_condition.text = CSString.Format(555,
            LianTiTableManager.Instance.GetLianTiName(tableLevelSeal.xinFaLevel), tableLevelSeal.xinFaLevel,
            tableLevelSeal.shortenTime);
        //解封加速人数
        string tepNum =
            $"{mySealData.shortenCount}/{tableLevelSeal.xinFaNumber}".BBCode(mySealData.shortenCount < tableLevelSeal.xinFaNumber
                ? ColorType.Red
                : ColorType.Green);

        mlb_unseal_speedup_num.text = CSString.Format(556,
            /*LianTiTableManager.Instance.GetLianTiName(tableLevelSeal.xinFaLevel),*/ tepNum);

        //我的心法等级
        int myXinFaLevel = CSLianTiInfo.Instance.LianTiID;
        mlb_myheartlaw_level.text =
            CSString.Format(579, LianTiTableManager.Instance.GetLianTiName(myXinFaLevel), myXinFaLevel);

        //显示排行榜
        if (rankInfo != null)
            ShowRoleModelAndInfo();
    }


    void OnRankInfo()
    {
        long time = CSServerTime.Instance.TotalSeconds;
        if (time - CSSealGradeInfo.Instance.ReqTime >= 60f)
        {
            Net.ReqRankInfoMessage((int) RankType.SealGrade);
            CSSealGradeInfo.Instance.ReqTime = time;
        }
    }

    void OnDesParticle()
    {
        mlb_count_down.text = CSServerTime.Instance.FormatLongToTimeStr(remainingTime, 3);
        remainingTime--;
    }

    void OnClickRule(GameObject go)
    {
        //查看规则(调用统一接口)
        if (go == null) return;
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Seal);
    }

    void OnClickAddition(GameObject go)
    {
        if (go == null) return;
        UIManager.Instance.CreatePanel<UISealAddtionPanel>();
    }

    void OnClickCheckHeartlaw(GameObject go)
    {
        UtilityPanel.JumpToPanel(12910);
    }

    void OnClickReward(GameObject go)
    {
        if (go == null) return;
        if (mySealData == null) return;
        int rank = (int) UIEventListener.Get(go).parameter;
        int levelSealRankId = mySealData.level * 100 + rank;
        TABLE.LEVELSEALRANK tableLevelSealRank;
        if (LevelSealRankTableManager.Instance.TryGetValue(levelSealRankId, out tableLevelSealRank))
        {
            string str = tableLevelSealRank.rankReward;
            UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f => { (f as UIUnsealRewardPanel).Show(str); });
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.StopInvokeRepeating2();
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_title);
        CSEffectPlayMgr.Instance.Recycle(msp_title_effect);
        CSEffectPlayMgr.Instance.Recycle(mobj_bg);
        CSEffectPlayMgr.Instance.Recycle(msp_title1);
        CSEffectPlayMgr.Instance.Recycle(msp_title2);
        CSEffectPlayMgr.Instance.Recycle(msp_title3);
        CSEffectPlayMgr.Instance.Recycle(msp_model1);
        CSEffectPlayMgr.Instance.Recycle(msp_model2);
        CSEffectPlayMgr.Instance.Recycle(msp_model3);
        CSEffectPlayMgr.Instance.Recycle(msp_weapen1);
        CSEffectPlayMgr.Instance.Recycle(msp_weapen2);
        CSEffectPlayMgr.Instance.Recycle(msp_weapen3);
        base.OnDestroy();
    }
}