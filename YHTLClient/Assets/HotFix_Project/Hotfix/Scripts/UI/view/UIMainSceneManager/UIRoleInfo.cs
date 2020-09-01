using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIRoleInfo : UIBase
{
    #region forms

    //角色界面外显红点
    GameObject _redP_role;

    GameObject redP_role
    {
        get { return _redP_role ?? (_redP_role = Get("roleInfo/blood/redpoint").gameObject); }
    }

    UILabel _lb_levelSpr;

    UILabel lb_levelSpr
    {
        get { return _lb_levelSpr ?? (_lb_levelSpr = Get<UILabel>("roleInfo/levelSpr/lb_level")); }
    }

    #region 红蓝

    UISprite _sp_hp;

    UISprite sp_hp
    {
        get { return _sp_hp ?? (_sp_hp = Get<UISprite>("roleInfo/blood/hpPanel/hp")); }
    }

    GameObject _obj_hpEff;

    GameObject obj_hpEff
    {
        get { return _obj_hpEff ?? (_obj_hpEff = Get("roleInfo/blood/hpPanel/hp/effect").gameObject); }
    }

    UISprite _obj_hpEff1;

    UISprite obj_hpEff1
    {
        get { return _obj_hpEff1 ?? (_obj_hpEff1 = Get<UISprite>("roleInfo/blood/hpPanel/hp/effect1")); }
    }

    UISprite _sp_mp;

    UISprite sp_mp
    {
        get { return _sp_mp ?? (_sp_mp = Get<UISprite>("roleInfo/blood/mpPanel/mp")); }
    }

    GameObject _obj_mpEff;

    GameObject obj_mpEff
    {
        get { return _obj_mpEff ?? (_obj_mpEff = Get("roleInfo/blood/mpPanel/mp/effect").gameObject); }
    }

    UISprite _obj_mpEff1;

    UISprite obj_mpEff1
    {
        get { return _obj_mpEff1 ?? (_obj_mpEff1 = Get<UISprite>("roleInfo/blood/mpPanel/mp/effect1")); }
    }

    #endregion

    #region 经验

    private UIProgressBar _sp_Exp;

    private UIProgressBar sp_Exp
    {
        get { return _sp_Exp ?? (_sp_Exp = Get<UIProgressBar>("Exp/roleexp")); }
    }

    private UILabel _lb_Expnum;

    private UILabel lb_Expnum
    {
        get { return _lb_Expnum ?? (_lb_Expnum = Get<UILabel>("Exp/lb_roleexp")); }
    }

    UIProgressBar _pro_NowVigor;

    UIProgressBar pro_NowVigor
    {
        get { return _pro_NowVigor ?? (_pro_NowVigor = Get<UIProgressBar>("Exp/Nowvigor")); }
    }

    UISprite _sp_Nowvigor;

    UISprite sp_Nowvigor
    {
        get { return _sp_Nowvigor ?? (_sp_Nowvigor = Get<UISprite>("Exp/Nowvigor/bar/expSliderEff")); }
    }

    UIProgressBar _pro_ExpectVigor;

    UIProgressBar pro_ExpectVigor
    {
        get { return _pro_ExpectVigor ?? (_pro_ExpectVigor = Get<UIProgressBar>("Exp/Expectvigor")); }
    }

    private GameObject _effect1;

    private GameObject effect1
    {
        get { return _effect1 ?? (_effect1 = Get("Exp/roleexp/effectParent/effect").gameObject); }
    }

    private GameObject _effect2;

    private GameObject effect2
    {
        get { return _effect2 ?? (_effect2 = Get("Exp/Nowvigor/effectParent/effect").gameObject); }
    }

    GameObject _btn_addExp;

    GameObject btn_addExp
    {
        get { return _btn_addExp ?? (_btn_addExp = Get("btn_add").gameObject); }
    }

    #endregion

    #region 背包按钮

    private GameObject _btn_bagBtn;

    public GameObject btn_bagBtn
    {
        get { return _btn_bagBtn ? _btn_bagBtn : (_btn_bagBtn = Get<GameObject>("btn_bag")); }
    }

    private GameObject _btn_bagBtnRed;

    public GameObject btn_bagBtnRed
    {
        get { return _btn_bagBtnRed ? _btn_bagBtnRed : (_btn_bagBtnRed = Get<GameObject>("btn_bag/redpoint")); }
    }

    private GameObject _btn_bagBtnFull;

    public GameObject btn_bagBtnFull
    {
        get { return _btn_bagBtnFull ? _btn_bagBtnFull : (_btn_bagBtnFull = Get<GameObject>("btn_bag/sp_full")); }
    }

    private GameObject _obj_bagFull;

    public GameObject obj_bagFull
    {
        get { return _obj_bagFull ? _obj_bagFull : (_obj_bagFull = Get<GameObject>("btn_bag/fullBubble")); }
    }

    private UILabel _lb_bagFull;

    public UILabel lb_bagFull
    {
        get { return _lb_bagFull ? _lb_bagFull : (_lb_bagFull = Get<UILabel>("btn_bag/fullBubble/Label")); }
    }

    #endregion

    #region 获得真气气泡

    GameObject _obj_ZhenQiBubble;

    GameObject obj_ZhenQiBubble
    {
        get { return _obj_ZhenQiBubble ?? (_obj_ZhenQiBubble = Get("ZhenQiPanel", UIPrefabTrans.parent).gameObject); }
    }

    GameObject _btn_zqBubble;

    GameObject btn_zqBubble
    {
        get { return _btn_zqBubble ?? (_btn_zqBubble = Get("bubble", obj_ZhenQiBubble.transform).gameObject); }
    }

    UILabel _lb_zqBubble;

    UILabel lb_zqBubble
    {
        get { return _lb_zqBubble ?? (_lb_zqBubble = Get<UILabel>("Label", btn_zqBubble.transform)); }
    }

    #endregion

    #endregion

    public override void Init()
    {
        base.Init();
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.GetExp, GetExpChange);
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, GetUpgrade);
        mClientEvent.AddEvent(CEvent.SCEnergyInfoMessage, GetExpChange);
        mClientEvent.AddEvent(CEvent.SCEnergyFreeGetInfoMessage, GetExpChange);
        mClientEvent.AddEvent(CEvent.SCGetFreeEnergyMessage, GetExpChange);
        mClientEvent.AddEvent(CEvent.SCEnergyExchangeInfoMessage, GetExpChange);

        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.HP_Change, GetHpChange);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MP_Change, GetMpChange);
        CSRedPointManager.Instance.RegisterRedPoint(redP_role, RedPointType.LianTi, RedPointType.WolongUpGrade,
            RedPointType.Nostalgia);

        UIEventListener.Get(btn_bagBtn).onClick = BagBtnClick;
        CSRedPointManager.Instance.RegisterRedPoint(btn_bagBtnRed, RedPointType.Bag, RedPointType.Nostalgia);

        CSEffectPlayMgr.Instance.ShowUIEffect(effect1, 17916);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect2, 17915);
        CSEffectPlayMgr.Instance.ShowUIEffect(sp_Nowvigor.gameObject, 17931);

        effect1.SetActive(sp_Exp.value < 1 && sp_Exp.value > 0);
        effect2.SetActive(pro_NowVigor.value < 1 && pro_NowVigor.value > 0);
        EventDelegate.Add(sp_Exp.onChange, OnExp1SliderChangeValue);
        EventDelegate.Add(pro_NowVigor.onChange, OnExp2SliderChangeValue);

        //背包已满状态提示
        UIEventListener.Get(obj_bagFull).onClick = BagFullBubbleClick;
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemListChange);
        mClientEvent.AddEvent(CEvent.BagInit, GetItemListChange);
        lb_bagFull.text = ClientTipsTableManager.Instance.GetClientTipsContext(1626);


        CSEffectPlayMgr.Instance.ShowUIEffect(obj_hpEff, 17053);
        CSEffectPlayMgr.Instance.ShowUIEffect(obj_hpEff1.gameObject, 17005);
        CSEffectPlayMgr.Instance.ShowUIEffect(obj_mpEff, 17054);
        CSEffectPlayMgr.Instance.ShowUIEffect(obj_mpEff1.gameObject, 17008);


        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_vigor, btn_addExp);
        UIEventListener.Get(btn_addExp).onClick = AddExpClick;

        ZhenQiBubbleInit();
    }

    public override void Show()
    {
        base.Show();
        ShowExp();
        lb_levelSpr.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1974),
            CSMainPlayerInfo.Instance.Level);
        ShowHp();
        ShowMp();
    }

    protected override void OnDestroy()
    {
        CSRedPointManager.Instance.Recycle(btn_bagBtnRed);
        CSRedPointManager.Instance.Recycle(redP_role);
        CSEffectPlayMgr.Instance.Recycle(effect1);
        CSEffectPlayMgr.Instance.Recycle(effect2);
        CSEffectPlayMgr.Instance.Recycle(sp_Nowvigor.gameObject);
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.GetExp, GetExpChange);
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.MainPlayer_LevelChange, GetUpgrade);
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.HP_Change, GetHpChange);
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MP_Change, GetMpChange);
        base.OnDestroy();
    }

    #region 按钮事件

    void OnExp1SliderChangeValue()
    {
        effect1.SetActive(sp_Exp.value < 1 && sp_Exp.value > 0);
    }

    void OnExp2SliderChangeValue()
    {
        effect2.SetActive(pro_NowVigor.value < 1 && pro_NowVigor.value > 0);
        sp_Nowvigor.fillAmount = pro_NowVigor.value;
    }

    void BagBtnClick(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIBagPanel>();
    }

    void AddExpClick(GameObject _go)
    {
        if (UICheckManager.Instance.DoCheckButtonClick(FunctionType.funcP_vigor))
        {
            UIManager.Instance.CreatePanel<UIFastAccessVigorPanel>();
        }
    }

    int RecyclefuncId = 37;
    int needVipLv = 0;

    void BagFullBubbleClick(GameObject _go)
    {
        if (needVipLv == 0)
        {
            needVipLv = int.Parse(SundryTableManager.Instance.GetSundryEffect(731));
        }

        obj_bagFull.SetActive(false);
        TABLE.FUNCOPEN funcopenItem;
        if (!FuncOpenTableManager.Instance.TryGetValue(RecyclefuncId, out funcopenItem))
        {
            return;
        }

        if (CSMainPlayerInfo.Instance.Level < funcopenItem.needLevel)
        {
            UtilityTips.ShowRedTips(106, funcopenItem.needLevel, funcopenItem.functionName);
            return;
        }

        if (CSMainPlayerInfo.Instance.VipLevel < needVipLv)
        {
            UtilityTips.ShowPromptWordTips(75, TransMitToOpenRecyclePanel, ToVipPanel);
            return;
        }

        CSPetLevelUpInfo.Instance.JudgeOpenPetLevelUpPanel();
    }

    void TransMitToOpenRecyclePanel()
    {
        UtilityPath.FindWithDeliverId(1047);
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }

    void ToVipPanel()
    {
        UIManager.Instance.CreatePanel<UIVIPPanel>();
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }

    #endregion

    #region 经验变动刷新

    void GetExpChange(uint eventId, object args)
    {
        ShowExp();
        lb_levelSpr.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1974),
            CSMainPlayerInfo.Instance.Level);
    }

    void GetUpgrade(uint eventId, object args)
    {
        ShowExp();
        RefershSeal();
        lb_levelSpr.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1974),
            CSMainPlayerInfo.Instance.Level);
    }

    const long wan = 10000;
    const long shiwan = 100000;
    const long baiwan = 1000000;
    const long qianwan = 10000000;
    const long yi = 100000000;
    const long shiyi = 1000000000;
    const long baiyi = 10000000000;
    const long qianyi = 100000000000;
    const long wanyi = 1000000000000;

    void ShowExp()
    {
        //1 显示无精力值   2显示有精力值
        int state = 0;
        double curVigor = CSVigorInfo.Instance.GetCurrentVigorExp();
        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor))
        {
            if (CSSealGradeInfo.Instance.MySealData != null) //封印期间
            {
                if (CSMainPlayerInfo.Instance.Level > CSSealGradeInfo.Instance.MySealLevel)
                {
                    //state = 1;
                    state = (curVigor > 0) ? 2 : 1;
                }
                else
                {
                    state = (curVigor > 0) ? 2 : 1;
                }
            }
            else
            {
                state = (curVigor > 0) ? 2 : 1;
            }
        }
        else
        {
            state = 1;
        }

        if (state == 2)
        {
            //Debug.Log("显示有精力值经验条");
            pro_NowVigor.gameObject.SetActive(true);
            pro_ExpectVigor.gameObject.SetActive(true);
            sp_Exp.gameObject.SetActive(false);
            long cur_exp = CSMainPlayerInfo.Instance.Exp;
            long max_exp = CSMainPlayerInfo.Instance.GetCurLevelMaxExp();
            float value = (max_exp == 0) ? 0 : (float)cur_exp / max_exp;
            pro_NowVigor.value = value;
            if (value == 0f || value == 1f)
            {
                pro_ExpectVigor.value = value;
            }
            else
            {
                pro_ExpectVigor.value = 0.96f * value + 0.02f;
            }

            int ex = EnergyStorageTableManager.Instance.GetEnergyStorageEnergyExp(CSMainPlayerInfo.Instance.Level);
            //long expectVigorExp = cur_exp + ex * curVigor;
            pro_ExpectVigor.value = (float)(cur_exp + ex * curVigor) / max_exp;
            //Debug.Log($"{cur_exp}   {ex}    {curVigor}    {max_exp}");
            string curValue = "";
            string maxValue = "";
            lb_Expnum.text = $"{GetExpResultStr(cur_exp, curValue)}/{GetExpResultStr(max_exp, maxValue)}";
        }
        else if (state == 1)
        {
            pro_NowVigor.gameObject.SetActive(false);
            pro_ExpectVigor.gameObject.SetActive(false);
            sp_Exp.gameObject.SetActive(true);
            //Debug.Log("显示无精力值经验条");
            long cur_exp = CSMainPlayerInfo.Instance.Exp;
            long max_exp = CSMainPlayerInfo.Instance.GetCurLevelMaxExp();
            float value = (max_exp == 0) ? 0 : (float)cur_exp / max_exp;
            sp_Exp.value = value;
            //固定显示位数为4位，万位显示万单位，亿位显示亿单位，参考：9.999万 99.99万，999.0万，9999万，1.000亿
            string curValue = "";
            string maxValue = "";
            lb_Expnum.text = $"{GetExpResultStr(cur_exp, curValue)}/{GetExpResultStr(max_exp, maxValue)}";
        }
    }

    string GetExpResultStr(long _num, string _result)
    {
        if (_num < wan)
        {
            _result = _num.ToString();
        }
        else if (wan <= _num && _num < shiwan)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / wan), 3, MidpointRounding.AwayFromZero)}万";
        }
        else if (shiwan <= _num && _num < baiwan)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / wan), 2, MidpointRounding.AwayFromZero)}万";
        }
        else if (baiwan <= _num && _num < qianwan)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / wan), 1, MidpointRounding.AwayFromZero)}万";
        }
        else if (qianwan <= _num && _num < yi)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / wan), 0, MidpointRounding.AwayFromZero)}万";
        }
        else if (yi <= _num && _num < shiyi) //亿
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / yi), 3, MidpointRounding.AwayFromZero)}亿";
        }
        else if (shiyi <= _num && _num < baiyi)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / yi), 2, MidpointRounding.AwayFromZero)}亿";
        }
        else if (baiyi <= _num && _num < qianyi)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / yi), 1, MidpointRounding.AwayFromZero)}亿";
        }
        else if (qianyi <= _num && _num < wanyi)
        {
            _result = $"{Math.Round(Convert.ToDecimal(_num * 1f / yi), 0, MidpointRounding.AwayFromZero)}亿";
        }

        return _result;
    }

    #endregion

    #region 背包已满状态刷新

    void GetItemListChange(uint id, object data)
    {
        int maxCount = CSBagInfo.Instance.GetMaxCount();
        int curCount = CSBagInfo.Instance.GetCurMaxCount();
        if ((maxCount - curCount) > 5)
        {
            obj_bagFull.SetActive(false);
            btn_bagBtnFull.SetActive(false);
        }
        else
        {
            obj_bagFull.SetActive(true);
            lb_bagFull.text = ClientTipsTableManager.Instance.GetClientTipsContext(1626);
            if (maxCount == curCount)
            {
                btn_bagBtnFull.SetActive(true);
                lb_bagFull.text = ClientTipsTableManager.Instance.GetClientTipsContext(1710);
            }
            else
            {
                btn_bagBtnFull.SetActive(false);
            }
        }
    }

    #endregion

    #region 红蓝变化

    void ShowHp()
    {
        sp_hp.fillAmount = CSMainPlayerInfo.Instance.HP * 1f / CSMainPlayerInfo.Instance.MaxHP;
        obj_hpEff1.fillAmount = CSMainPlayerInfo.Instance.HP * 1f / CSMainPlayerInfo.Instance.MaxHP;
        obj_hpEff.transform.localPosition = new Vector3(0, -35 + (70) * sp_hp.fillAmount, 0);
    }

    void ShowMp()
    {
        sp_mp.fillAmount = CSMainPlayerInfo.Instance.MP * 1f / CSMainPlayerInfo.Instance.MaxMP;
        obj_mpEff1.fillAmount = CSMainPlayerInfo.Instance.MP * 1f / CSMainPlayerInfo.Instance.MaxMP;
        obj_mpEff.transform.localPosition = new Vector3(0, -35 + (70) * sp_mp.fillAmount, 0);
    }

    void GetHpChange(uint id, object data)
    {
        ShowHp();
    }

    void GetMpChange(uint id, object data)
    {
        ShowMp();
    }

    #endregion

    #region 获得真气气泡

    // bool isSealed = false;
    bool isShowZQBubble = false;
    string zqQiPaoSetting = "";
    void ZhenQiBubbleInit()
    {
        mClientEvent.AddEvent(CEvent.OpenSeal, GetLevelSealOpen);
        mClientEvent.AddEvent(CEvent.CloseSeal, GetLevelSealClose);
        // mClientEvent.AddEvent(CEvent.ResDayPassedMessage, GetPassedDay);
        mClientEvent.AddEvent(CEvent.ZhenQiAdd, GetZhenQiAdd);
        RefershSeal();
        lb_zqBubble.text = ClientTipsTableManager.Instance.GetClientTipsContext(2020);
        UIEventListener.Get(btn_zqBubble).onClick = ZhenQiBubbleClick;
        zqQiPaoSetting = $"{CSMainPlayerInfo.Instance.ID}FirstGetZhenQiCurrentDay";
    }

    void GetLevelSealOpen(uint id, object data)
    {
        RefershSeal();
    }

    void GetLevelSealClose(uint id, object data)
    {
        RefershSeal();
    }

    void RefershSeal()
    {
        RefreshZQBubbleState();
    }

    // void GetPassedDay(uint id, object data)
    // {
    //     RefreshZQBubbleState();
    // }
    void GetZhenQiAdd(uint id, object data)
    {
        RefreshZQBubbleState();
    }

    void RefreshZQBubbleState()
    {
        if (CSSealGradeInfo.Instance.MySealData != null &&
            CSMainPlayerInfo.Instance.Level >= CSSealGradeInfo.Instance.MySealLevel &&
            CSMainPlayerInfo.Instance.Exp >= CSMainPlayerInfo.Instance.GetCurLevelMaxExp())
        {
            if (!PlayerPrefs.HasKey(zqQiPaoSetting))
            {
                obj_ZhenQiBubble.SetActive(true);
                PlayerPrefs.SetString(zqQiPaoSetting, CSServerTime.Instance.TotalMillisecond.ToString());
            }
            else
            {
                string time = PlayerPrefs.GetString(zqQiPaoSetting);
                if (Int64.TryParse(time, out long lastTime))
                {
                    if (CSServerTime.Instance.GetDayByMinusCurTime(lastTime) > 0)
                    {
                        obj_ZhenQiBubble.SetActive(true);
                        PlayerPrefs.SetString(zqQiPaoSetting,CSServerTime.Instance.TotalMillisecond.ToString());
                    }
                }
                else
                    obj_ZhenQiBubble.SetActive(false);
            }
        }
        else
            obj_ZhenQiBubble.SetActive(false);

    }

    void ZhenQiBubbleClick(GameObject _go)
    {
        obj_ZhenQiBubble.SetActive(false);
    }

    #endregion
}