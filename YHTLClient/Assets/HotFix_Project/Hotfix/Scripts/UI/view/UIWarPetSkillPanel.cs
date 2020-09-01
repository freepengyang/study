using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIWarPetSkillPanel : UIBasePanel
{
    const string CONST_SKILL_LEARNED = "Learned";
    const string CONST_SKILL_UNLEARNED = "UnLearned";
    const string CONST_SKILL_LEVELFULL = "LevelFull";
    const string CONST_SKILL_LevelNotFull = "LevelNotFull";

    public override void Init()
	{
		base.Init();

        mClientEvent.AddEvent(CEvent.PetSkillUpgradeSucceed, OnPetSkillUpgradeSucceed);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnZdjnChange);
        mClientEvent.AddEvent(CEvent.OnPetSkillAdded, OnZdjnChange);
        mClientEvent.AddEvent(CEvent.OnPetSkillRemoved, OnZdjnChange);
        mClientEvent.AddEvent(CEvent.OnPetJobChanged, OnZdjnChange);
        mClientEvent.AddEvent(CEvent.PetTalentLvChange, OnZdjnChange);
        mClientEvent.AddEvent(CEvent.GetWarPetBaseInfo, OnZdjnChange);

        mClientEvent.AddEvent(CEvent.OnPetJobChanged, OnBdjnChange);
        mClientEvent.AddEvent(CEvent.PetBdjnChanged, OnBdjnChange);

        mPetSkillDatas = mPoolHandleManager.CreateGeneratePool<PetAttrItemData>();

        if (null != mBtnUpGrade)
            mBtnUpGrade.onClick = OnClickSkillUpgrade;

        if (null != mbtn_help)
            mbtn_help.onClick = OnClickHelper;

        CSEffectPlayMgr.Instance.ShowUITexture(mpet_skill_bg, "pet_skill_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(mpet_bd_skill_bg, "pet_skill_bg");
    }

    protected void OnClickHelper(UnityEngine.GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.PetSkillHelper);
    }

    protected void OnClickSkillUpgrade(UnityEngine.GameObject go)
    {
        if (null == mData || !mData.callUpgrade)
            return;

        CSSkillInfo.Instance.ReqPetSkillUpgrade(mData.item.id);
    }

    protected void OnPetSkillUpgradeSucceed(uint eventId, object argv)
    {
        InitZDJN();
        //技能升级替换
        PetSkillItemData newSkill = argv as PetSkillItemData;
        if (null != newSkill && null != mData && null != mData.item && mData.item.skillGroup == newSkill.item.skillGroup)
        {
            OnSkillSelected(newSkill);
        }
    }

    protected void OnZdjnChange(uint eventId, object argv)
    {
        InitZDJN();
    }

    protected void OnBdjnChange(uint eventId, object argv)
    {
        InitBDJN();
    }

    protected void OnPetJobChanged(uint eventId, object argv)
    {
        InitZDJN();
        InitBDJN();
    }

    void InitZDJN()
    {
        var datas = CSSkillInfo.Instance.GetPetJobRelativeSkills();
        for (int i = 0; i < datas.Count; ++i)
        {
            datas[i].onSkillClicked = this.OnSkillSelected;
        }
        mZdjnGrid.Bind<PetSkillItemBinder,PetSkillItemData>(datas);

        if(!PetSkillItemBinder.HasChoiced() || PetSkillItemBinder.IsActived())
            SetDefaultForZdjn();
    }

    void InitBDJN()
    {
        int bdjnUnlockLevel = CSPetTalentInfo.Instance.GetPassiveSkillUnlockLv();
        int curTalentLevel = CSSkillInfo.Instance.GetPetTalentLevel();
        bool unloced = curTalentLevel >= bdjnUnlockLevel;
        mBdjnUnlockHint.CustomActive(!unloced);
        //被动技能解锁提示
        if (!unloced)
        {
            mBdjnUnlockHint.text = CSString.Format(1534, bdjnUnlockLevel);
            mBdjnUnlockHint.SetupLink();
        }
        else
        {
            mBdjnUnlockHint.CustomActive(false);
            bool isDeactiveChoiced = PetSkillItemBinder.HasChoiced() && !PetSkillItemBinder.IsActived();

            int choicedSkillPos = PetSkillItemBinder.ChoicedSkillPos;
            //被动技能
            var datas = CSSkillInfo.Instance.GetPetBDJNItems();
            PetSkillItemData choicedItem = null;
            for (int i = 0; i < datas.Count; ++i)
            {
                datas[i].onSkillClicked = this.OnSkillSelected;
                FNDebug.LogFormat("<color=#00ff00>[被动技能位置]:[{0}]</color>", datas[i].Pos);
                if (isDeactiveChoiced && null != datas[i] && datas[i].Pos == choicedSkillPos && choicedSkillPos >= 0)
                {
                    choicedItem = datas[i];
                }
            }
            mBdjnGrid.Bind<PetSkillItemBinder, PetSkillItemData>(datas);

            if (null != choicedItem)
            {
                mClientEvent.SendEvent(CEvent.PetSkillSelected, choicedItem.item.id);
                OnSkillSelected(choicedItem);
            }
        }
    }

    protected void SetDefaultForZdjn()
    {
        var datas = CSSkillInfo.Instance.GetPetJobRelativeSkills();
        if (datas.Count > 0)
        {
            if (null != mData && null != mData.item)
            {
                int skillGroupId = mData.item.skillGroup;
                mData = null;
                for (int i = 0; i < datas.Count; ++i)
                {
                    if (datas[i].item.skillGroup == skillGroupId)
                    {
                        if (null != datas[i] && null != datas[i].item)
                            mClientEvent.SendEvent(CEvent.PetSkillSelected, datas[i].item.id);
                        OnSkillSelected(datas[i]);
                        break;
                    }
                }
            }

            if (null == mData)
            {
                if (null != datas[0] && null != datas[0].item)
                {
                    mClientEvent.SendEvent(CEvent.PetSkillSelected, datas[0].item.id);
                    OnSkillSelected(datas[0]);
                }
            }
        }
    }

    PetSkillItemData mData;
    protected void OnSkillSelected(PetSkillItemData data)
    {
        if (null == data)
            return;

        if (data.IsActiveSkill)
        {
            ScriptBinder._SetAction("ZDJN");
            mData = data;
            OnActivedSkillSelected(mData);
        }
        else
        {
            //弹出红色TIPS
            if(!data.learned && data.HasFlag(PetSkillItemData.SkillFlag.SF_SPECIAL))
            {
                UtilityTips.ShowRedTips(1724);
                return;
            }
            if(null == data.item || !data.learned)
            {
                CSSkillInfo.Instance.Link2PetRefinePanel();
                return;
            }
            ScriptBinder._SetAction("BDJN");
            mData = data;
            OnDeActivedSkillSelected(mData);
        }
    }

    /// <summary>
    /// 主动技能被选中
    /// </summary>
    /// <param name="data"></param>
    protected void OnActivedSkillSelected(PetSkillItemData data)
    {
        bool levelFull = data.isLevelFull;

        //技能名称
        if (null != mSkillName && null != mData.item)
        {
            mSkillName.text = mData.item.name;
        }

        //技能等级
        if (null != mCurLevel && null != mData.item)
        {
            if (!this.mData.learned)
            {
                mCurLevel.text = CSString.Format(552, 0);
            }
            else
            {
                mCurLevel.text = CSString.Format(552, mData.item.level);
            }
        }

        //技能描述
        if (null != mCurDesc && data.learned)
        {
            mCurDesc.text = data.item.Desc();
        }

        //本级技能状态设置
        if (null != ScriptBinder)
            ScriptBinder._SetAction(data.learned ? CONST_SKILL_LEARNED : CONST_SKILL_UNLEARNED);

        //下一技能等级
        var nextItem = data.nextItem;
        if (null != mNextLevel && null != nextItem)
        {
            if (!levelFull)
            {
                mNextLevel.text = CSString.Format(552, nextItem.level);
            }
            else
                mNextLevel.text = CSString.Format(545);
        }

        //下一级技能状态设置
        if (null != ScriptBinder)
            ScriptBinder._SetAction(levelFull ? CONST_SKILL_LEVELFULL : CONST_SKILL_LevelNotFull);

        //下一技能描述
        if (null != mNextDesc && !levelFull)
            mNextDesc.text = nextItem.NextDesc(data.item);

        mQualityFly.CustomActive(false);
        mQualityFlyFull.CustomActive(false);
        mQualityFlyFrame.CustomActive(false);
        //if (!levelFull)
        //{
        //    //质跃标志
        //    mQualityFly.CustomActive(data.QualityFly);

        //    //特殊效果显示
        //    mQualityFlyFrame.CustomActive(data.hasSpecialEffect);

        //    //高级特效文字说明
        //    if (null != mHighEffectHint)
        //    {
        //        mHighEffectHint.CustomActive(data.hasSpecialEffect);
        //        if (data.hasSpecialEffect)
        //            mHighEffectHint.text = string.Format(mHighEffectHint.FormatStr, data.nextHighEffectDistance);
        //    }

        //    //高阶特效查看链接回调
        //    if (null != mBtnHighSkillEffect)
        //    {
        //        mBtnHighSkillEffect.gameObject.SetActive(data.hasSpecialEffect);
        //        mBtnHighSkillEffect.onClick = null;
        //        if (data.hasSpecialEffect)
        //            mBtnHighSkillEffect.onClick = OnClickHighEffect;
        //    }

        //    //高阶特效显微镜
        //    if (null != mMicroScope)
        //        mMicroScope.gameObject.SetActive(data.hasSpecialEffect);
        //}

        mspr_redpoint.CustomActive(data.learned && data.canUpgrade);

        //设置天赋链接或者升级需求
        mBtnUpGrade.CustomActive(data.learned && !levelFull);
        mUpgradeNeedPlayerLevel.CustomActive(data.learned && !levelFull);
        mUpgradeCosts.CustomActive(data.learned);
        mTalentUnlock.CustomActive(!data.learned);
        if(data.learned)
        {
            //技能提升需要宠物等级
            if (null != mUpgradeNeedPlayerLevel && !levelFull && null != data && null != data.nextItem)
                mUpgradeNeedPlayerLevel.text = CSString.Format(data.nextItem.studyLevel <= CSSkillInfo.Instance.GetPetLevel() ? 1531 : 1532, data.nextItem.studyLevel);

            //技能升级消耗
            if (!levelFull)
            {
                var itemBarDatas = mData.nextItem.cost.GetItemBarDatas(mPoolHandleManager, mClientEvent);
                for(int i = 0,max = itemBarDatas.Count;i<max;++i)
                {
                    var itemData = itemBarDatas[i];
                    itemData.flag |= (int)ItemBarData.ItemBarType.IBT_SHORT_EXPRESS;
                    itemData.flag |= (int)ItemBarData.ItemBarType.IBT_COST_GENERAL_COMPARE_RED_GREEN_ICON;
                }
                UIItemBarManager.Instance.Bind(mUpgradeCosts, itemBarDatas);
                itemBarDatas.Clear();
                mPoolHandleManager.Recycle(itemBarDatas);
            }
            else
            {
                mUpgradeCosts.MaxCount = 0;
            }
        }
        else
        {
            mUpgradeCosts.MaxCount = 0;
            int needTalentLevel = CSSkillInfo.Instance.GetStudySkillNeedTalentLevel(data.item.skillGroup);
            mTalentUnlock.text = CSString.Format(1533, needTalentLevel);
            mTalentUnlock.SetupLink();
        }

        Vector3 pos = mNextHead.transform.localPosition;
        pos.y = mCurDesc.transform.localPosition.y - mCurDesc.height;
        pos.y = Mathf.Min(-130, pos.y);
        mNextHead.transform.localPosition = pos;

        float y = pos.y;
        pos = mNextDesc.transform.localPosition;
        pos.y = y - mNextHead.height;
        mNextDesc.transform.localPosition = pos;

        mScrollView.ResetPosition();
        mScrollView.SetDragAmount(0, 0, true);
    }

    protected void OnClickHighEffect(UnityEngine.GameObject go)
    {
        if (null == mData)
            return;

        //UIManager.Instance.CreatePanel<UISkillUpgradeEffectPanel>(f =>
        //{
        //    (f as UISkillUpgradeEffectPanel).Show(mData);
        //});
    }

    /// <summary>
    /// 被动技能被选中
    /// </summary>
    /// <param name="data"></param>
    protected void OnDeActivedSkillSelected(PetSkillItemData data)
    {
        //技能名称
        if (null != mSkillName && null != mData.item)
        {
            mSkillName.text = mData.item.name;
        }

        //设置技能等级
        if (null != mCurLevel)
        {
            mCurLevel.text = CSString.Format(1539);
        }
        //设置技能描述
        if(null != mCurDesc)
        {
            if (null != data.warPetSkillData)
                mCurDesc.text = data.item.BdjnDesc(data.XilanId, data.warPetSkillData.AttrDisplays);
            else
                mCurDesc.text = string.Empty;
        }
        //初始化属性列表
        InitBdjnAttributes(data);
        //初始化属性链接
        mupgrade_talent_link.text = ClientTipsTableManager.Instance.GetClientTipsContext(1540);
        mupgrade_talent_link.SetupLink();
    }

    public class PetAttrItemData
    {
        public int idx;
        public string name;
        public string curValue;
        public string curMaxValue;
        public string nextMaxValue;
    }
    public class PetAttrItemBinder : UIBinder,IndexedItem
    {
        public int Index { get; set; }
        UISprite sp_bg;
        UILabel lb_name;
        UILabel lb_value;
        UILabel lb_nextValue;
        UILabel lb_nextMaxValue;
        public override void Init(UIEventListener handle)
        {
            sp_bg = Handle.GetComponent<UISprite>();
            lb_name = Get<UILabel>("lb_name");
            lb_value = Get<UILabel>("lb_value");
            lb_nextValue = Get<UILabel>("lb_nextValue");
            lb_nextMaxValue = Get<UILabel>("lb_nnv");
        }

        public PetAttrItemData Data { get; set; }
        public override void Bind(object data)
        {
            Data = data as PetAttrItemData;
            if (null == this.Data)
                return;

            sp_bg.enabled = (this.Data.idx % 2 == 0);
            if (null != lb_name)
                lb_name.text = this.Data.name;
            if (null != lb_value)
                lb_value.text = this.Data.curValue;
            if (null != lb_nextValue)
                lb_nextValue.text = this.Data.curMaxValue;
            if (null != lb_nextMaxValue)
                lb_nextMaxValue.text = this.Data.nextMaxValue;
        }

        public override void OnDestroy()
        {
            this.Data = null;
            sp_bg = null;
            lb_name = null;
            lb_nextValue = null;
            lb_value = null;
            lb_nextMaxValue = null;
        }
    }

    FastArrayElementFromPool<PetAttrItemData> mPetSkillDatas;
    void GetPetAttrItems(PetSkillItemData data)
    {
        mPetSkillDatas.Clear();
        TABLE.CHONGWUXILIAN xiulianItem = null;
        if (!ChongwuXilianTableManager.Instance.TryGetValue(data.XilanId, out xiulianItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:修炼ID = {0} 无法再修炼表中找到", data.XilanId);
            return;
        }
        
        TABLE.CHONGWUXILIAN nextXiulianStageItem = null;
        int nextXiulianID = (xiulianItem.petLevClass + 1) * 10000 + xiulianItem.getlntimes;
        if (!ChongwuXilianTableManager.Instance.TryGetValue(nextXiulianID, out nextXiulianStageItem))
        {
            nextXiulianStageItem = xiulianItem;
        }

        TABLE.CHONGWUXILIANSHUJUKU dbItem = null;
        int dbid = xiulianItem.library + data.item.skillGroup * 1000;
        if (!ChongwuXilianShujukuTableManager.Instance.TryGetValue(dbid, out dbItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:[library:{0}][skillGroup:{1}][dbId:{2}]无法在技能数据库中被找到", xiulianItem.library, data.item.skillGroup, dbid);
            return;
        }

        TABLE.CHONGWUXILIANSHUJUKU dbNextItem = null;
        int dbNextId = nextXiulianStageItem.library + data.item.skillGroup * 1000;
        if (!ChongwuXilianShujukuTableManager.Instance.TryGetValue(dbNextId, out dbNextItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:[library:{0}][skillGroup:{1}][dbId:{2}]无法在技能数据库中被找到", nextXiulianStageItem.library, data.item.skillGroup, dbNextId);
            return;
        }

        if (data.PetSkillAttrs.Count < 1)
        {
            FNDebug.LogError("[被动技能]:技能属性条目为空");
            return;
        }

        string curValues = string.Empty;
        string curMaxValues = string.Empty;
        string nextMaxValues = string.Empty;
        if (data.PetSkillAttrs.Count == 1)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc());
            if (dbItem.value1.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description, Utility.GetPetSkillAttr(dbItem.type,dbItem.parameter1, dbItem.value1[1],2,2));
            }
            if (dbNextItem.value1.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description, Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1],1, 2));
            }
        }
        else if (data.PetSkillAttrs.Count == 2)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description, 
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2));
            }
        }
        else if (data.PetSkillAttrs.Count == 3)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc(), data.PetSkillAttrs[2].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3 && dbItem.value3.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description,
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter3, dbItem.value3[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3 && dbItem.value3.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter3, dbNextItem.value3[1], 1, 2));
            }
        }
        else
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc(), data.PetSkillAttrs[2].BdjnPartDesc(), data.PetSkillAttrs[3].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3 && dbItem.value3.Count == 3 && dbItem.value4.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description,
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter3, dbItem.value3[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter4, dbItem.value4[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3 && dbItem.value3.Count == 3 && dbItem.value4.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter3, dbNextItem.value3[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter4, dbNextItem.value4[1], 1, 2));
            }
        }

        var curTokens = curValues.Split('&');
        var curMaxTokens = curMaxValues.Split('&');
        var nextMaxTokens = nextMaxValues.Split('&');
        if(curTokens.Length != curMaxTokens.Length || curTokens.Length != nextMaxTokens.Length)
        {
            FNDebug.LogErrorFormat("[被动技能]:属性条目数量不对等");
            return;
        }
        if(curTokens.Length != dbItem.attrIds.Count)
        {
            FNDebug.LogErrorFormat("[被动技能]:属性ID条目与属性描述条目不对等");
            return;
        }
        string[] attrNames = new string[curTokens.Length];
        for(int i = 0; i < dbItem.attrIds.Count; ++i)
        {
            var attrId = dbItem.attrIds[i];
            var attrName = string.Empty;
            if(dbItem.type == 2)
            {
                CHONGWUSHUXING shuxingItem = null;
                if (!ChongwuShuxingTableManager.Instance.TryGetValue(attrId, out shuxingItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:属性Id = {0} 无法在ChongwuShuxing表中被找到", attrId);
                    return;
                }
                attrName = shuxingItem.name;
            }
            else
            {
                CLIENTATTRIBUTE clientAttrItem = null;
                if (!ClientAttributeTableManager.Instance.TryGetValue(attrId, out clientAttrItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:属性Id = {0} 无法在ClientAttribute表中被找到", attrId);
                    return;
                }
                CLIENTTIPS tipItem = null;
                if (!ClientTipsTableManager.Instance.TryGetValue(clientAttrItem.tipID, out tipItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:TipId = {0} 无法在ClientTips表中被找到", clientAttrItem.tipID);
                    return;
                }
                attrName = tipItem.context;
            }
            attrNames[i] = attrName;
        }

        for(int i = 0; i < attrNames.Length; ++i)
        {
            var skillData = mPetSkillDatas.Append();
            skillData.idx = i;
            skillData.name = attrNames[i];
            skillData.curValue = curTokens[i];
            skillData.curMaxValue = curMaxTokens[i];
            skillData.nextMaxValue = nextMaxTokens[i];
        }
    }
    void GetPetAttrItemsForSpecific(PetSkillItemData data)
    {
        mPetSkillDatas.Clear();
        TABLE.CHONGWUHUSHENXILIAN xiulianItem = null;
        if (!ChongwuHushenXilianTableManager.Instance.TryGetValue(data.XilanId, out xiulianItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:修炼ID = {0} 无法再修炼表中找到", data.XilanId);
            return;
        }

        TABLE.CHONGWUHUSHENXILIAN nextXiulianStageItem = null;
        int nextXiulianID = (xiulianItem.petLevelClass + 1) * 10000 + xiulianItem.getlntimes;
        if (!ChongwuHushenXilianTableManager.Instance.TryGetValue(nextXiulianID, out nextXiulianStageItem))
        {
            nextXiulianStageItem = xiulianItem;
        }

        TABLE.CHONGWUHUSHENXILIANSHUJUKU dbItem = null;
        int dbid = xiulianItem.library + data.item.skillGroup * 1000;
        if (!ChongwuHushenXilianShujukuTableManager.Instance.TryGetValue(dbid, out dbItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:[library:{0}][skillGroup:{1}][dbId:{2}]无法在技能数据库中被找到", xiulianItem.library, data.item.skillGroup, dbid);
            return;
        }

        TABLE.CHONGWUHUSHENXILIANSHUJUKU dbNextItem = null;
        int dbNextId = nextXiulianStageItem.library + data.item.skillGroup * 1000;
        if (!ChongwuHushenXilianShujukuTableManager.Instance.TryGetValue(dbNextId, out dbNextItem))
        {
            FNDebug.LogErrorFormat("[被动技能]:[library:{0}][skillGroup:{1}][dbId:{2}]无法在技能数据库中被找到", nextXiulianStageItem.library, data.item.skillGroup, dbNextId);
            return;
        }

        if (data.PetSkillAttrs.Count < 1)
        {
            FNDebug.LogError("[被动技能]:技能属性条目为空");
            return;
        }

        string curValues = string.Empty;
        string curMaxValues = string.Empty;
        string nextMaxValues = string.Empty;
        if (data.PetSkillAttrs.Count == 1)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc());
            if (dbItem.value1.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description, Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description, Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2));
            }
        }
        else if (data.PetSkillAttrs.Count == 2)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description,
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2));
            }
        }
        else if (data.PetSkillAttrs.Count == 3)
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc(), data.PetSkillAttrs[2].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3 && dbItem.value3.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description,
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter3, dbItem.value3[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3 && dbItem.value3.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter3, dbNextItem.value3[1], 1, 2));
            }
        }
        else
        {
            curValues = string.Format(dbItem.description, data.PetSkillAttrs[0].BdjnPartDesc(), data.PetSkillAttrs[1].BdjnPartDesc(), data.PetSkillAttrs[2].BdjnPartDesc(), data.PetSkillAttrs[3].BdjnPartDesc());
            if (dbItem.value1.Count == 3 && dbItem.value2.Count == 3 && dbItem.value3.Count == 3 && dbItem.value4.Count == 3)
            {
                curMaxValues = string.Format(dbItem.description,
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter1, dbItem.value1[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter2, dbItem.value2[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter3, dbItem.value3[1], 2, 2),
                    Utility.GetPetSkillAttr(dbItem.type, dbItem.parameter4, dbItem.value4[1], 2, 2));
            }
            if (dbNextItem.value1.Count == 3 && dbNextItem.value2.Count == 3 && dbItem.value3.Count == 3 && dbItem.value4.Count == 3)
            {
                nextMaxValues = string.Format(dbNextItem.description,
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter1, dbNextItem.value1[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter2, dbNextItem.value2[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter3, dbNextItem.value3[1], 1, 2),
                    Utility.GetPetSkillAttr(dbNextItem.type, dbNextItem.parameter4, dbNextItem.value4[1], 1, 2));
            }
        }

        var curTokens = curValues.Split('&');
        var curMaxTokens = curMaxValues.Split('&');
        var nextMaxTokens = nextMaxValues.Split('&');
        if (curTokens.Length != curMaxTokens.Length || curTokens.Length != nextMaxTokens.Length)
        {
            FNDebug.LogErrorFormat("[被动技能]:属性条目数量不对等");
            return;
        }
        if (curTokens.Length != dbItem.attrIds.Count)
        {
            FNDebug.LogErrorFormat("[被动技能]:属性ID条目与属性描述条目不对等");
            return;
        }
        string[] attrNames = new string[curTokens.Length];
        for (int i = 0; i < dbItem.attrIds.Count; ++i)
        {
            var attrId = dbItem.attrIds[i];
            var attrName = string.Empty;
            if (dbItem.type == 2)
            {
                CHONGWUSHUXING shuxingItem = null;
                if (!ChongwuShuxingTableManager.Instance.TryGetValue(attrId, out shuxingItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:属性Id = {0} 无法在ChongwuShuxing表中被找到", attrId);
                    return;
                }
                attrName = shuxingItem.name;
            }
            else
            {
                CLIENTATTRIBUTE clientAttrItem = null;
                if (!ClientAttributeTableManager.Instance.TryGetValue(attrId, out clientAttrItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:属性Id = {0} 无法在ClientAttribute表中被找到", attrId);
                    return;
                }
                CLIENTTIPS tipItem = null;
                if (!ClientTipsTableManager.Instance.TryGetValue(clientAttrItem.tipID, out tipItem))
                {
                    FNDebug.LogErrorFormat("[被动技能]:TipId = {0} 无法在ClientTips表中被找到", clientAttrItem.tipID);
                    return;
                }
                attrName = tipItem.context;
            }
            attrNames[i] = attrName;
        }

        for (int i = 0; i < attrNames.Length; ++i)
        {
            var skillData = mPetSkillDatas.Append();
            skillData.idx = i;
            skillData.name = attrNames[i];
            skillData.curValue = curTokens[i];
            skillData.curMaxValue = curMaxTokens[i];
            skillData.nextMaxValue = nextMaxTokens[i];
        }
    }
    void InitBdjnAttributes(PetSkillItemData data)
    {
        if (!data.HasFlag(PetSkillItemData.SkillFlag.SF_SPECIAL))
            GetPetAttrItems(data);
        else
            GetPetAttrItemsForSpecific(data);
        mgrid_effects.Bind<PetAttrItemData,PetAttrItemBinder>(mPetSkillDatas,mPoolHandleManager);
    }

    public override void Show()
	{
		base.Show();

        InitZDJN();
        InitBDJN();
    }
	
	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(mpet_bd_skill_bg);
        CSEffectPlayMgr.Instance.Recycle(mpet_skill_bg);
        mClientEvent.RemoveEvent(CEvent.PetSkillUpgradeSucceed, OnPetSkillUpgradeSucceed);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnZdjnChange);
        mClientEvent.RemoveEvent(CEvent.OnPetSkillAdded, OnZdjnChange);
        mClientEvent.RemoveEvent(CEvent.OnPetSkillRemoved, OnZdjnChange);
        mClientEvent.RemoveEvent(CEvent.OnPetJobChanged, OnZdjnChange);
        //mClientEvent.RemoveEvent(CEvent.OnPetLevelChanged, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.PetTalentLvChange, OnZdjnChange);
        mClientEvent.RemoveEvent(CEvent.GetWarPetBaseInfo, OnZdjnChange);
        mClientEvent.RemoveEvent(CEvent.PetBdjnChanged, OnZdjnChange);
        PetSkillItemBinder.Clear();
        mBdjnGrid?.UnBind<PetSkillItemBinder>();
        mBdjnGrid = null;
        mZdjnGrid?.UnBind<PetSkillItemBinder>();
        mZdjnGrid = null;
        mgrid_effects?.UnBind<PetAttrItemBinder>();
        mgrid_effects = null;
        mPetSkillDatas?.Clear();
        mPetSkillDatas = null;
        base.OnDestroy();
	}
}
