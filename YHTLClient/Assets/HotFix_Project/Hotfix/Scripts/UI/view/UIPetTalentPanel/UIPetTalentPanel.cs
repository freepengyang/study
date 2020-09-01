using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIPetTalentPanel : UIBasePanel
{

    string unlockClientStr;
    string curMaxLvClientStr;
    string equipLvClientStr;

    int curShowPage;

    FastArrayElementFromPool<CSTalentData> curShowList;
    FastArrayElementFromPool<UITalentStarBinder> starUIList;

    CSBetterLisHot<CSTalentAttrData> rightList;

    readonly int starPerPage = 9;
    readonly float lightDuration = 0.05f;

    int lastFakeLv;
    int curLightStar;


    CSTalentCore curBigCore;//当前大星的天赋核心


    public override void Init()
	{
		base.Init();


        CSEffectPlayMgr.Instance.ShowUITexture(mobj_abBg1, "warpet_talent1");
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_abBg2, "warpet_talent2");

        //mClientEvent.AddEvent(CEvent.PetTalentLvChange, PetTalentLvChange);

        mClientEvent.AddEvent(CEvent.PetTalentStarSelect, TalentStarSelectEvent);

        mbtn_help.onClick = HelpBtnClick;
        mbtn_talentPreview.onClick = TalentPreviewClick;
        UIEventListener.Get(mlb_hint.gameObject).onClick = GoToUpgradeClick;

        UIEventListener.Get(mlb_nextBigDes.gameObject).onClick = SkillPreviewClick;

        mscroll_right.SetDynamicArrowVertical(msp_scrollArrow);


        unlockClientStr = CSPetTalentInfo.Instance.unlockClientStr;
        curMaxLvClientStr = CSPetTalentInfo.Instance.curMaxLvClientStr;
        equipLvClientStr = CSPetTalentInfo.Instance.equipLvClientStr;


        InitLeftStarUIBinder();
    }


    void InitLeftStarUIBinder()
    {
        if (mGrid_left.controlList.Count != starPerPage)
        {
            return;
        }

        if (starUIList == null)
        {
            starUIList = mPoolHandleManager.CreateGeneratePool<UITalentStarBinder>();
        }
        else
        {
            for (int i = 0; i < starUIList.Count; i++)
            {
                starUIList[i]?.Destroy();
            }
            starUIList.Clear();
        }

        for (int i = 0; i < mGrid_left.controlList.Count; i++)
        {
            UITalentStarBinder binder = starUIList.Append();
            var handle = UIEventListener.Get(mGrid_left.controlList[i]);
            binder.Setup(handle);
        }
    }
       
	
	public override void Show()
	{
		base.Show();

        //CheckCanAvtivePoint();

        lastFakeLv = CSPetTalentInfo.Instance.LastFakeLvInPanel;

        RefreshUI();
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
    }

    protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(mobj_abBg1);
        CSEffectPlayMgr.Instance.Recycle(mobj_abBg2);

        mGrid_right.UnBind<UITalentRightPropertyBinder>();
        curShowList = null;
        if (starUIList != null)
        {
            for (int i = 0; i < starUIList.Count; i++)
            {
                starUIList[i].Destroy();
            }
            starUIList.Clear();
            starUIList = null;
        }

        rightList?.Clear();
        rightList = null;

        CSPetTalentInfo.Instance.LastFakeLvInPanel = lastFakeLv;

        base.OnDestroy();
	}
    

    /// <summary>
    /// 检查当前应该显示第几页天赋数据
    /// </summary>
    void CheckShowPage()
    {
        int backSpacePoint = CSPetTalentInfo.Instance.BackspacePoint;
        int activePage = CSPetTalentInfo.Instance.ActivatedPage;
        int activeLv = CSPetTalentInfo.Instance.CurActivatedPoint;
        //if (/*activeLv > 0 && activeLv % starPerPage == 0 && backSpacePoint < 0 && */activePage < CSPetTalentInfo.Instance.MaxPage)//只有该情况下显示下一页天赋
        //{
        //    curShowPage = activePage + 1;
        //}
        //else curShowPage = activePage;
        curShowPage = activePage;
        curShowList = CSPetTalentInfo.Instance.GetOnePageList(curShowPage);

        //if (curShowList == null) return;
        //if (rightList == null) rightList = new CSBetterLisHot<CSTalentData>();
        //else rightList.Clear();

        //for (int i = 0; i < curShowList.Count; i++)
        //{
        //    var data = curShowList[i];
        //    if (data == null || data.config == null) continue;
        //    if (data.config.type == 1) rightList.Add(data);
        //}
    }



    void RefreshUI()
    {
        CheckShowPage();
        string totalLvStr = CSString.Format(curMaxLvClientStr, CSPetTalentInfo.Instance.CurActivatedPoint);
        string equipLvStr = CSString.Format(equipLvClientStr, CSPetTalentInfo.Instance.PointFromEquip);
        mlb_title.text = $"{totalLvStr}{equipLvStr}";

        bool isMax = CSPetTalentInfo.Instance.CurActivatedPoint >= CSPetTalentInfo.Instance.MaxLevel;
        mlb_max.gameObject.CustomActive(isMax);
        mlb_nextBig.gameObject.CustomActive(!isMax);
        mlb_nextBigDes.gameObject.CustomActive(!isMax);

        curBigCore = null;
        if (curShowList == null) return;
        if (curShowList.Count >= starPerPage)
        {
            var bigData = curShowList[starPerPage - 1];
            if (bigData != null)
            {
                mlb_nextBig.text = CSString.Format(unlockClientStr, bigData.Id);
                if (string.IsNullOrEmpty(bigData.desc))
                {
                    bigData.SetDesc();
                }
                if (!string.IsNullOrEmpty(bigData.desc))
                {
                    string newStr = bigData.desc.Replace("[u=skill]", "[u]");
                    mlb_nextBigDes.text = newStr;
                }
                else
                {
                    mlb_nextBigDes.text = "";
                }
                curBigCore = bigData.linkedCore;
            }
        }

        RefreshRightUI();
        RefreshStarBinder();
    }


    void RefreshRightUI()
    {
        if (rightList == null) rightList = new CSBetterLisHot<CSTalentAttrData>();
        else rightList.Clear();

        var dic = CSPetTalentInfo.Instance.AllAttrDic;
        if (dic == null || dic.Count < 1) return;

        for (var it = dic.GetEnumerator(); it.MoveNext();)
        {
            var attr = it.Current.Value;
            if (attr == null) continue;
            rightList.Add(attr);
        }

        rightList.Sort((a, b) =>
        {
            return a.tipId - b.tipId;
        });

        mGrid_right.Bind<CSTalentAttrData, UITalentRightPropertyBinder>(rightList, mPoolHandleManager);
    }


    void RefreshStarBinder()
    {
        if (curShowList == null || starUIList == null) return;
        if (curShowList.Count != starUIList.Count) return;


        int realLv = CSPetTalentInfo.Instance.CurActivatedPoint;
        for (int i = 0; i < starUIList.Count; i++)
        {
            starUIList[i].Bind(curShowList[i]);
        }

        if (lastFakeLv < starUIList[0].thisLevel)
        {
            lastFakeLv = starUIList[0].thisLevel - 1;
        }

        int needLightCount = 0;
        for (int i = 0; i < starUIList.Count; i++)
        {
            if (starUIList[i].thisLevel > lastFakeLv && starUIList[i].thisLevel <= realLv)
            {
                needLightCount++;
            }
            else if (starUIList[i].thisLevel <= lastFakeLv)
            {
                starUIList[i].ForceRefresh();
            }
        }

        if (needLightCount > 0)
        {
            curLightStar = lastFakeLv % starPerPage;
            ScriptBinder.InvokeRepeating(0.5f, needLightCount * lightDuration, LightStar);
        }
    }


    void LightStar()
    {
        int realLv = CSPetTalentInfo.Instance.CurActivatedPoint;
        if (starUIList == null || starUIList.Count < starPerPage || lastFakeLv >= realLv)
        {
            ScriptBinder.StopInvokeRepeating();
            CSPetTalentInfo.Instance.LastFakeLvInPanel = lastFakeLv;
            return;
        }
        if (starUIList[curLightStar].thisLevel > realLv)
        {
            ScriptBinder.StopInvokeRepeating();
            CSPetTalentInfo.Instance.LastFakeLvInPanel = lastFakeLv;
            return;
        }
        starUIList[curLightStar].ForceRefresh();
        lastFakeLv++;
        curLightStar++;
    }


    //void PetTalentLvChange(uint id, object param)
    //{
    //    RefreshUI();
    //}


    void HelpBtnClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.PetTalent);
    }


    void TalentPreviewClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIPetTalentPreviewPanel>();
    }


    void GoToUpgradeClick(GameObject go)
    {

        //UtilityPanel.JumpToPanel(27101);
        CSPetLevelUpInfo.Instance.JudgeOpenPetLevelUpPanel(() => { UIManager.Instance.ClosePanel<UIWarPetCombinedPanel>(); });
    }


    void SkillPreviewClick(GameObject go)
    {
        if (curBigCore == null || curBigCore.config == null) return;
        TABLE.CHONGWUHEXIN cfg = curBigCore.config;

        if(cfg.talenttype == 3)
        {
            UIManager.Instance.CreatePanel<UIPetTalentSkillPreviewPanel>((f) =>
            {
                (f as UIPetTalentSkillPreviewPanel).Refresh(cfg);
            });
        }       
    }


    void TalentStarSelectEvent(uint id, object param)
    {
        CSTalentData data = param as CSTalentData;
        if (data == null) return;
        UIManager.Instance.CreatePanel<UIPetTalentTipsPanel>((f) =>
        {
            (f as UIPetTalentTipsPanel).RefreshUI(data);
        });
    }

}


public class UITalentRightPropertyBinder : UIBinder
{
    UILabel lb_name;
    UILabel lb_value;
    UILabel lb_nextName;
    UILabel lb_nextValue;
    GameObject obj_arrow;

    GameObject obj_item;

    CSTalentAttrData mData;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_value = Get<UILabel>("lb_value");
        lb_nextName = Get<UILabel>("lb_nextName");
        lb_nextValue = Get<UILabel>("lb_nextValue");
        obj_arrow = Get<GameObject>("sp_arrow");
        obj_item = handle.gameObject;

        lb_value.text = "";
        lb_nextValue.text = "";
    }

    public override void Bind(object data)
    {
        mData = data as CSTalentAttrData;
        if (mData == null) return;
        obj_arrow.CustomActive(false);

        string key = mData.tipStr;
        string value = "";
        if (mData.tipId != mData.tipId2)
        {
            value = $"{mData.value}-{mData.value2}";
        }
        else
        {
            value = CSPetBasePropInfo.Instance.GetDealWithValue(mData.tipId, mData.value);
        }
        
        lb_name.text = $"{UtilityColor.SubTitle}{key}";
        lb_nextName.text = $"{UtilityColor.MainText}{value}";

        //if (mData == null ||mData.config == null) return;

        //TABLE.CHONGWUTIANFU cfg = mData.config;
        //obj_item.CustomActive(cfg.starrating % 9 != 0);

        //if (cfg.starrating % 9 == 0) return;
        //obj_arrow.CustomActive(false);

        //if (string.IsNullOrEmpty(mData.desc))
        //{
        //    mData.SetDesc();
        //}

        //RefreshColor();
    }


    //void RefreshColor(bool hightLight = false)
    //{
    //    if (lb_name == null || lb_nextName == null) return;
    //    if (mData == null || mData.config == null) return;
    //    TABLE.CHONGWUTIANFU cfg = mData.config;

    //    int activePaging = CSPetTalentInfo.Instance.ActivatedPage;
    //    int activeStar = CSPetTalentInfo.Instance.ActivatedLvInCurPage;
    //    bool isActive = cfg.paging <= activePaging && cfg.starrating <= activeStar;
        

    //    if (cfg.type == 1)
    //    {
    //        int realValue = CSPetBasePropInfo.Instance.GetNetAttValue(cfg.tip);
    //        int fakeValue = isActive ? realValue - cfg.value : realValue + cfg.value;
    //        fakeValue = fakeValue < 0 ? 0 : fakeValue;
    //        string realStr = CSPetBasePropInfo.Instance.GetDealWithValue(cfg.tip, realValue);
    //        string fakeStr = CSPetBasePropInfo.Instance.GetDealWithValue(cfg.tip, fakeValue);

    //        string leftStr = /*isActive ? fakeStr : */realStr;
    //        string rightStr = isActive ? realStr : fakeStr;

    //        string leftColorKey = hightLight ? UtilityColor.Red : isActive ? UtilityColor.SubTitle : UtilityColor.WeakText;
    //        string leftColorValue = hightLight ? UtilityColor.Red : isActive ? UtilityColor.MainText : UtilityColor.WeakText;

    //        string rightColorKey = hightLight ? UtilityColor.Red : isActive ? UtilityColor.SubTitle : UtilityColor.WeakText;
    //        string rightColorValue = hightLight ? UtilityColor.Red : isActive ? UtilityColor.Green : UtilityColor.WeakText;

    //        lb_name.text = $"{leftColorKey}{mData.desc}：{leftColorValue}{realStr}";
    //        //lb_nextName.text = $"{rightColorKey}{mData.desc}：{rightColorValue}{rightStr}";
    //        lb_nextName.text = "";

    //    }
    //    //else if (cfg.type == 2)
    //    //{
    //    //    string newStr = mData.desc.Replace("[u=skill]", "");
    //    //    lb_name.text = newStr.BBCode(hightLight ? ColorType.Red : isActive ? ColorType.SubTitleColor : ColorType.WeakText);
    //    //    lb_nextName.text = "";
    //    //}


    //}


    public override void OnDestroy()
    {
        mData = null;

        lb_name = null;
        lb_value = null;
        lb_nextName = null;
        lb_nextValue = null;
        obj_arrow = null;
        obj_item = null;
    }
}



public class UITalentStarBinder : UIBinder
{
    private enum StarState { Locked, Actived, CanUnlock, Special}

    UISprite sp_star;
    GameObject obj_special;
    UISprite sp_lock;
    GameObject obj_lineBase;
    GameObject obj_lineLight;
    UISprite sp_lineLight;
    UILabel lb_name;

    GameObject obj_normalEffect;
    GameObject obj_lineEffect;

    CSTalentData mData;
    
    StarState curState;

    public int thisLevel;

    string talentLvStr;

    public override void Init(UIEventListener handle)
    {
        sp_star = Get<UISprite>("sp_normal");
        obj_special = Get<GameObject>("sp_special");
        sp_lock = Get<UISprite>("sp_lock");
        obj_lineBase = Get<GameObject>("sp_line1");
        obj_lineLight = Get<GameObject>("sp_line1/sp_line2");
        sp_lineLight = Get<UISprite>("sp_line1/sp_line2");
        lb_name = Get<UILabel>("lb_name");
        obj_normalEffect = Get<GameObject>("effect_normal");
        obj_lineEffect = Get<GameObject>("sp_line1/effect_line");

        talentLvStr = CSPetTalentInfo.Instance.talentLvStr;
    }

    public override void Bind(object data)
    {
        mData = data as CSTalentData;
        if (mData == null) return;
        thisLevel = mData.Id;
        curState = StarState.Locked;
        RefreshUI();
    }


    public void ForceRefresh()
    {
        CheckStarState();
        RefreshUI();
    }


    void RefreshUI()
    {
        if (mData == null || mData.config == null) return;
        if (lb_name != null)
        {
            lb_name.text = CSString.Format(talentLvStr, mData.Id);
        }
        
        obj_special?.CustomActive(false);

        obj_lineBase?.CustomActive(mData.config.starrating != 1);
        obj_lineLight?.CustomActive(curState != StarState.Locked);

        if (sp_lock != null)
        {
            sp_lock.CustomActive(curState == StarState.Locked || curState == StarState.CanUnlock);
            sp_lock.color = curState == StarState.Locked ? Color.black : CSColor.white;
        }
        if (sp_lineLight != null)
        {
            sp_lineLight.color = curState == StarState.Locked || curState == StarState.Special ? Color.black : CSColor.white;
        }

        if (sp_star != null)
        {
            sp_star.gameObject.CustomActive(true);
            sp_star.color = curState == StarState.Locked || curState == StarState.Special ? Color.black : CSColor.white;
            
        }

        if (obj_normalEffect != null)
        {
            if (curState == StarState.Locked || curState == StarState.Special)
            {
                CSEffectPlayMgr.Instance.Recycle(obj_normalEffect);
            }
            else
            {
                int id = mData.config.starrating != 9 ? 17810 : 17811;
                CSEffectPlayMgr.Instance.ShowUIEffect(obj_normalEffect, id);
            }
        }

        if (obj_lineEffect != null && mData.config.starrating != 1)
        {
            obj_lineEffect.CustomActive(curState != StarState.Locked && curState != StarState.Special);
            if (curState == StarState.Locked || curState == StarState.Special)
            {
                CSEffectPlayMgr.Instance.Recycle(obj_lineEffect);
            }
            else CSEffectPlayMgr.Instance.ShowUIEffect(obj_lineEffect, 17812);
        }

        Handle.onClick = StarOnClick;
    }

    void CheckStarState()
    {
        curState = StarState.Locked;
        if (mData == null) return;
        int thisLv = mData.Id;
        int backSpacePoint = CSPetTalentInfo.Instance.BackspacePoint;
        int activePoint = CSPetTalentInfo.Instance.CurActivatedPoint;


        if (backSpacePoint > 0)
        {
            if (thisLv > activePoint - backSpacePoint && thisLv <= activePoint)
            {
                curState = StarState.Special;
            }
            else if (thisLv <= activePoint - backSpacePoint)
            {
                curState = StarState.Actived;
            }
            return;
        }

        if (backSpacePoint <= 0)
        {
            if (thisLv == activePoint + 1 && backSpacePoint < 0)
            {
                curState = StarState.CanUnlock;
                return;
            }

            if(thisLv <= activePoint)
            {
                curState = StarState.Actived;
                return;
            }
        }

        curState = StarState.Locked;
    }

    


    void StarOnClick(GameObject go)
    {
        if (mData == null || mData.config == null) return;
        if (curState == StarState.CanUnlock)
        {
            Net.CSUnlockPetTianFuMessage(mData.config.paging, mData.config.starrating);
        }
        //else if(curState == StarState.Locked)
        //{
        //    UtilityTips.ShowRedTips(1727, mData.Id);
        //}
        else
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.PetTalentStarSelect, mData);
        }
    }



    public override void OnDestroy()
    {
        if (obj_normalEffect != null)
        {
            CSEffectPlayMgr.Instance.Recycle(obj_normalEffect);
        }
        obj_normalEffect = null;
        if (obj_lineEffect != null)
        {
            CSEffectPlayMgr.Instance.Recycle(obj_lineEffect);
        }
        obj_lineEffect = null;
        mData = null;
        sp_star = null;
        obj_special = null;
        sp_lock = null;
        obj_lineBase = null;
        obj_lineLight = null;
        sp_lineLight = null;
        lb_name = null;
    }
}
