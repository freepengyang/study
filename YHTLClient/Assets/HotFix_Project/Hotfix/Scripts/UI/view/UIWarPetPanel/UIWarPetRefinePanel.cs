using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIWarPetRefinePanel : UIBasePanel
{
    private ILBetterList<WarPetSkillData> listWarPetSkillDatas;
    private int selectIndex = 0;
    private UIItemBarPrefab[] uiItemBarPrefabs;

    /// <summary>
    /// 当前所需装备列表
    /// </summary>
    private LongArray curCost;

    /// <summary>
    /// 是否注入成功
    /// </summary>
    /// <returns></returns>
    private bool isInjection = false;

    /// <summary>
    /// 当前选中技能信息
    /// </summary>
    private WarPetSkillData curWarPetSkillData;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.PetTianFuPassiveSkillMessage, RefreshData);
        mClientEvent.Reg((uint) CEvent.PetTianFuRandomPassiveSkill, PetTianFuRandomPassiveSkill);
        mClientEvent.Reg((uint) CEvent.PetTianFuChosePassiveSkill, TiHuanData);
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshItemChange);
        mClientEvent.Reg((uint) CEvent.MoneyChange, RefreshItemChange);
        mbtn_rule.onClick = OnClickRule;
        mbtn_preview.onClick = OnClickPreview;
        mbtn_add2.onClick = OnClickAdd2;
        mbtn_add1.onClick = OnClickAdd1;
        mbtn_replace.onClick = OnClickReplace;
        mbtn_refresh.onClick = OnClickReFresh;
        mbtn_inject.onClick = OnClickInject;
        mbtn_reinject.onClick = OnClickReInject;
        
        mTween_curSkill.onFinished.Add(new EventDelegate(OnFinished));
        
        uiItemBarPrefabs = new UIItemBarPrefab[2]
        {
            new UIItemBarPrefab(mUIItemBarPrefab1, msp_icon1, mlb_value1, mbtn_add1, mbtn_sp1),
            new UIItemBarPrefab(mUIItemBarPrefab2, msp_icon2, mlb_value2, mbtn_add2, mbtn_sp2),
        };
    }

    void OnFinished()
    {
        mcolInjection.SetActive(false);
        mcolRefresh.SetActive(true);
        mTween_curSkill.gameObject.transform.localPosition = mTween_curSkill.from;
        isInjection = false;
    }

    void RefreshData(uint id, object data)
    {
        InitData();
    }
    
    void TiHuanData(uint id, object data)
    {
        InitData(mcolInjection.activeSelf);
    }
    
    void PetTianFuRandomPassiveSkill(uint id, object data)
    {
        InitData();
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17362);
    }

    void RefreshItemChange(uint id, object data)
    {
        InitData();
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mbgtex0, "pearl_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mbgtex1, "pearl_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mbgtex2, "pearl_bg2");
        mScrollViewLeft.ResetPosition();
        InitData();
    }

    void InitData(bool isInjectionSuccess = false)
    {
        isInjection = isInjectionSuccess;
        listWarPetSkillDatas = CSWarPetRefineInfo.Instance.MyWarPetSkillDatas;
        if (listWarPetSkillDatas.Count > 0)
        {
            curWarPetSkillData = listWarPetSkillDatas[selectIndex];
            RefreshGrid();
            SetRightInfo();
        }
    }

    void RefreshGrid()
    {
        mgrid_skill.MaxCount = listWarPetSkillDatas.Count;
        GameObject gp;
        for (int i = 0; i < mgrid_skill.MaxCount; i++)
        {
            gp = mgrid_skill.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIWarPetRefineBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWarPetRefineBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWarPetRefineBinder;
            }

            WarPetSkillData warPetSkillData = listWarPetSkillDatas[i];
            Binder.isSelect = i == selectIndex;
            Binder.index = i;
            Binder.actionItem = OnClickItem;
            Binder.Bind(warPetSkillData);
        }
    }

    /// <summary>
    /// 点击单个技能Item
    /// </summary>
    /// <param name="index"></param>
    void OnClickItem(int index)
    {
        if (index == selectIndex || isInjection) return;
        selectIndex = index;
        curWarPetSkillData = listWarPetSkillDatas[selectIndex];
        RefreshGrid();
        SetRightInfo();
    }

    /// <summary>
    /// 设置右边信息
    /// </summary>
    void SetRightInfo()
    {
        //是否是空槽
        bool isEmpty = curWarPetSkillData.ID == 0;
        //注入成功播放动画
        if (isInjection)
        {
            minjection_sp_skill_icon.gameObject.SetActive(true);
            minjection_lb_skill_name.gameObject.SetActive(true);
            minjection_ScrollView.gameObject.SetActive(true);
            minjection_lb_nonSkill.gameObject.SetActive(false);
            minjection_sp_skill_icon.spriteName = curWarPetSkillData.CfgSkill?.icon;
            minjection_lb_skill_name.text = curWarPetSkillData.CfgSkill?.name;
            minjection_lb_content.text = CSString.Format(curWarPetSkillData.CfgSkill.clientDescription,
                curWarPetSkillData.AttrDisplays);
            minjection_lb_skillCD.text = CSString.Format(1740, curWarPetSkillData.CfgSkill?.cdTime / 1000);
            TweenPosition.Begin(mTween_curSkill.gameObject, 1f, mTween_curSkill.to);
        }
        else
        {
            minjection_sp_skill_icon.gameObject.SetActive(false);
            minjection_lb_skill_name.gameObject.SetActive(false);
            minjection_ScrollView.gameObject.SetActive(false);
            minjection_lb_nonSkill.gameObject.SetActive(true);
            mcolInjection.SetActive(isEmpty);
            mcolRefresh.SetActive(!isEmpty);
        }

        //设置花费
        SetCost();
        //红点
        mredpoint_inject.SetActive(isEmpty && curWarPetSkillData.Special==0 && curCost.IsItemsEnough());
        //非空槽时设置右边信息
        if (!isEmpty)
        {
            //设置现在的技能
            msp_skill_icon1.spriteName = curWarPetSkillData.CfgSkill?.icon;    
			mlb_skill_name1.text = curWarPetSkillData.CfgSkill?.name;
            mlb_content1.text = CSString.Format(curWarPetSkillData.CfgSkill.clientDescription,
                curWarPetSkillData.AttrDisplays);
            mlb_skillCD1.text = CSString.Format(1740, curWarPetSkillData.CfgSkill?.cdTime / 1000);

            //设置新技能
            bool isHasTmpSkill = curWarPetSkillData.TmpID > 0;
            mbtn_reinject.gameObject.SetActive(!isHasTmpSkill);
            mgrid_btns.gameObject.SetActive(isHasTmpSkill);
            mlb_hint.SetActive(!isHasTmpSkill);
            mlb_skill_name2.gameObject.SetActive(isHasTmpSkill);
            mlb_content2.gameObject.SetActive(isHasTmpSkill);
            // mlb_skillCD2.gameObject.SetActive(isHasTmpSkill);
            msp_skill_icon2.gameObject.SetActive(isHasTmpSkill);
            if (isHasTmpSkill) //当前有洗炼出来的新技能
            {
				msp_skill_icon2.spriteName = curWarPetSkillData.TmpCfgSkill?.icon;
				mlb_skill_name2.text = curWarPetSkillData.TmpCfgSkill?.name;
                mlb_content2.text = CSString.Format(curWarPetSkillData.TmpCfgSkill.clientDescription,
                    curWarPetSkillData.TmpAttrDisplays);
                mlb_skillCD2.text = CSString.Format(1740, curWarPetSkillData.TmpCfgSkill?.cdTime / 1000);
            }
        }
        
        mScrollView_colRefresh1.ResetPosition();
        mScrollView_colRefresh2.ResetPosition();
    }

    /// <summary>
    /// 设置花费
    /// </summary>
    void SetCost()
    {
        curCost = CSWarPetRefineInfo.Instance.GetCurCost(curWarPetSkillData.Special);
        //if (curCost == null) return;
        uiItemBarPrefabs[0].prefab.SetActive(false);
        uiItemBarPrefabs[1].prefab.SetActive(false);
        for (int i = 0; i < curCost.Count; i++)
        {
            ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(curCost[i].key(), out cfg))
            {
                UIItemBarPrefab uiItemBar = uiItemBarPrefabs[i];
                uiItemBar.sp_icon.spriteName = $"tubiao{cfg.icon}";
                long curNum = CSBagInfo.Instance.GetAllItemCount(cfg.id);
                long maxNum = curCost[i].value();
                string color;
                switch (cfg.type)
                {
                    case 1: //货币
                        color = curNum < maxNum ? "[ff0000]" : "[00ff0c]";
                        uiItemBar.lb_value.text = $"{color}{UtilityMath.GetDecimalTenThousandValue(maxNum)}[-]";
                        break;
                    default:
                        color = curNum < maxNum ? "[ff0000]" : "[00ff0c]";
                        uiItemBar.lb_value.text =
                            $"{color}{UtilityMath.GetDecimalTenThousandValue(curNum)}/{UtilityMath.GetDecimalTenThousandValue(maxNum)}[-]";
                        break;
                }

                uiItemBar.prefab.SetActive(true);
                UIEventListener.Get(uiItemBarPrefabs[i].btn_sp.gameObject).onClick = o =>
                {
                    UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfg.id);
                };
            }
        }
    }

    void OnClickAdd1(GameObject go)
    {
        if (curCost.Count > 0)
        {
            ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(curCost[0].key(), out cfg))
                Utility.ShowGetWay(cfg.id);
        }
    }

    void OnClickAdd2(GameObject go)
    {
        if (curCost.Count > 1)
        {
            ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(curCost[1].key(), out cfg))
                Utility.ShowGetWay(cfg.id);
        }
    }

    /// <summary>
    /// 注入
    /// </summary>
    /// <param name="go"></param>
    void OnClickInject(GameObject go)
    {
        if (!isInjection)
        {
            if (curCost.IsItemsEnough(showGetWay: true))
                Net.CSPetTianFuRandomPassiveSkillMessage(curWarPetSkillData.Pos, curWarPetSkillData.Special);
        }
    }

    /// <summary>
    /// 洗炼
    /// </summary>
    /// <param name="go"></param>
    void OnClickReInject(GameObject go)
    {
        if (curCost.IsItemsEnough(showGetWay: true))
            Net.CSPetTianFuRandomPassiveSkillMessage(curWarPetSkillData.Pos, curWarPetSkillData.Special);
    }

    /// <summary>
    /// 继续洗炼
    /// </summary>
    /// <param name="go"></param>
    void OnClickReFresh(GameObject go)
    {
        if (curCost.IsItemsEnough(showGetWay: true))
            Net.CSPetTianFuRandomPassiveSkillMessage(curWarPetSkillData.Pos, curWarPetSkillData.Special);
    }

    /// <summary>
    /// 替换
    /// </summary>
    /// <param name="go"></param>
    void OnClickReplace(GameObject go)
    {
        Net.CSPetTianFuChosePassiveSkillMessage(curWarPetSkillData.Pos, curWarPetSkillData.Special);
    }

    void OnClickRule(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WarPetRefine);
    }

    void OnClickPreview(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIWarPetSkillPromptPanel>();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mbgtex0);
        CSEffectPlayMgr.Instance.Recycle(mbgtex1);
        CSEffectPlayMgr.Instance.Recycle(mbgtex2);
        CSEffectPlayMgr.Instance.Recycle(meffect);
        mgrid_skill.UnBind<UIWarPetRefineBinder>();
        for (int i = 0; i < uiItemBarPrefabs.Length; i++)
        {
            uiItemBarPrefabs[i].Recycle();
        }
        uiItemBarPrefabs = null;
        base.OnDestroy();
    }
}