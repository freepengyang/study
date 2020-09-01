using System;
using UnityEngine;

public partial class UISkillPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        if(null != mBtnUpGrade)
            mBtnUpGrade.onClick = OnClickSkillUpgrade;

        if(null != mbtn_help)
            mbtn_help.onClick = OnClickHelper;

        mClientEvent.AddEvent(CEvent.SkillUpgradeSucceed, OnSkillUpgradeSucceed);
        mClientEvent.AddEvent(CEvent.AttachedSkillModified, OnAttachedSkillModified);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnItemChange);
        mClientEvent.AddEvent(CEvent.OnSkillSlotChanged, OnSkillSlotChanged);
        mClientEvent.AddEvent(CEvent.OnSkillAutoPlayChanged, OnSkillAutoPlayChanged);
        mClientEvent.AddEvent(CEvent.OnSkillAdded, OnItemChange);
        mClientEvent.AddEvent(CEvent.OnSkillRemoved, OnItemChange);

        mKeepedSVPosition = mSkillScrollView.transform.localPosition;

        mCurDesc.SetupLink();
        mNextDesc.SetupLink();
    }

    protected void OnSkillUpgradeSucceed(uint eventId,object argv)
    {
        //技能升级替换
        SkillInfoData newSkill = argv as SkillInfoData;
        if (null != newSkill && null != mData && null != mData.item && mData.item.skillGroup == newSkill.item.skillGroup)
        {
            OnSkillSelected(newSkill);
        }
    }

    protected void OnAttachedSkillModified(uint eventId, object argv)
    {
        if(null != mData && argv is int skillGroup && mData.SkillGroup == skillGroup)
        {
            OnSkillSelected(mData);
        }
    }

    protected void OnItemChange(uint eventId, object argv)
    {
        InitSkills();
    }

    protected void OnSkillSlotChanged(uint eventId, object argv)
    {
        InitSkills();
    }

    protected void OnSkillAutoPlayChanged(uint eventId, object argv)
    {
        UpdateSkillAutoPlayState();
    }

    UISkillCombinedPanel.SkillPanelType mMode = UISkillCombinedPanel.SkillPanelType.SPT_SKILL;
    public override void OnShow(int typeId)
    {
        this.mMode = (UISkillCombinedPanel.SkillPanelType)typeId;
        UISkillBinder.Mode = this.mMode;
        if (this.mMode == UISkillCombinedPanel.SkillPanelType.SPT_CONFIG)
            ScriptBinder?._SetAction(CONST_CONFIG_MODE);
        else
            ScriptBinder?._SetAction(CONST_SKILL_MODE);

        CSEffectPlayMgr.Instance.ShowUITexture(mBGL.gameObject, "skill_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mBGR.gameObject, "skill_bg");

        OnModeChanged();
    }

    protected UnityEngine.Vector3 mKeepedSVPosition = new UnityEngine.Vector3(-339, 0, 0);
    protected UISkillConfigPanel mConfigPanel = null;
    protected void OnModeChanged()
    {
        InitSkills();

        mSkillScrollView.panel.clipOffset = UnityEngine.Vector2.zero;
        mSkillScrollView.transform.localPosition = mKeepedSVPosition;

        if (this.mMode == UISkillCombinedPanel.SkillPanelType.SPT_CONFIG)
        {
            if(null == mConfigPanel)
            {
                mConfigPanel = new UISkillConfigPanel
                {
                    UIPrefab = mSkillConfigHandle,
                };
                mConfigPanel.Init();
            }
            mConfigPanel.Show();
            mConfigPanel.OnShow((int)UISkillCombinedPanel.SkillPanelType.SPT_CONFIG);
        }
    }

    void InitSkills()
    {
        var datas = CSSkillInfo.Instance.GetJobRelativeSkills();
        for(int i = 0; i < datas.Count; ++i)
        {
            if (this.mMode == UISkillCombinedPanel.SkillPanelType.SPT_SKILL)
                datas[i].onSkillClicked = this.OnSkillSelected;
            else
            {
                datas[i].onSkillClicked = null;
            }
        }
        mGridSkills.Bind<UISkillBinder, SkillInfoData>(datas);

        if (this.mMode == UISkillCombinedPanel.SkillPanelType.SPT_SKILL)
            SetDefault();
        else
            mClientEvent.SendEvent(CEvent.SetSkillSelectedEffect, 0);
    }

    protected void SetDefault()
    {
        var datas = CSSkillInfo.Instance.GetJobRelativeSkills();
        if (datas.Count > 0)
        {
            if(null != mData)
            {
                int skillGroupId = mData.item.skillGroup;
                mData = null;
                for (int i = 0; i < datas.Count; ++i)
                {
                    if (datas[i].item.skillGroup == skillGroupId)
                    {
                        if(null != datas[i] && null != datas[i].item)
                            mClientEvent.SendEvent(CEvent.SetSkillSelectedEffect, datas[i].item.id);
                        OnSkillSelected(datas[i]);
                        break;
                    }
                }
            }

            if (null == mData)
            {
                if (null != datas[0] && null != datas[0].item)
                    mClientEvent.SendEvent(CEvent.SetSkillSelectedEffect, datas[0].item.id);
                OnSkillSelected(datas[0]);
            }
        }
    }

    SkillInfoData mData;
    const string CONST_SKILL_LEARNED = "Learned";
    const string CONST_SKILL_UNLEARNED = "UnLearned";
    const string CONST_SKILL_LEVELFULL = "LevelFull";
    const string CONST_SKILL_LevelNotFull = "LevelNotFull";
    const string CONST_SKILL_MODE = "SkillMode";
    const string CONST_CONFIG_MODE = "ConfigMode";
    const string CONST_SWITCH_OFF = "SwitchOff";
    const string CONST_SWITCH_ON = "SwitchOn";

    protected void OnSkillSelected(SkillInfoData data)
    {
        mData = data;
        if (null == mData)
            return;

        mSkillRedPoint?.SetActive(mData.canUpgrade);

        //技能名称
        var currentValidItem = this.mData.currentValidItem;
        if (null != mSkillName)
        {
            mSkillName.text = currentValidItem.name;
        }

        bool levelFull = mData.isLevelFull;

        //技能等级
        if (null != mCurLevel && null != mData.item)
        {
            if(!this.mData.learned)
            {
                mCurLevel.text = CSString.Format(552,0);
                mbtn_help.CustomActive(false);
            }
            else
            {
                int attachedSkillLv = currentValidItem.level - this.mData.item.level;
                mbtn_help.CustomActive(attachedSkillLv > 0);
                if (attachedSkillLv > 0)
                {
                    mCurLevel.text = CSString.Format(1254,currentValidItem.level,mData.item.level,attachedSkillLv);
                }
                else
                {
                    mCurLevel.text = CSString.Format(552,mData.item.level);
                }
            }
        }
        else
        {
            mbtn_help.CustomActive(false);
        }

        //技能描述
        if (null != mCurDesc && this.mData.learned)
        {
            mCurDesc.text = currentValidItem.Desc();
        }

        //本级技能状态设置
        if (null != ScriptBinder)
            ScriptBinder._SetAction(mData.learned ? CONST_SKILL_LEARNED: CONST_SKILL_UNLEARNED);

        //下一技能等级
        var nextValidItem = mData.nextValidItem;
        if (null != mNextLevel && null != nextValidItem)
        {
            if (!levelFull)
            {
                int attachedSkillLv = nextValidItem.level - this.mData.nextItem.level;
                if (attachedSkillLv > 0)
                {
                    mNextLevel.text = CSString.Format(1254, nextValidItem.level, mData.nextItem.level, attachedSkillLv);
                }
                else
                {
                    mNextLevel.text = CSString.Format(552, mData.nextValidItem.level);
                }
            }
            else
                mNextLevel.text = CSString.Format(545);
        }

        //下一级技能状态设置
        if (null != ScriptBinder)
            ScriptBinder._SetAction(levelFull ? CONST_SKILL_LEVELFULL : CONST_SKILL_LevelNotFull);

        //下一技能描述
        if (null != mNextDesc && !levelFull)
            mNextDesc.text = nextValidItem.NextDesc(currentValidItem);

        if (!levelFull)
        {
            //质跃标志
            mQualityFly.CustomActive(mData.QualityFly);

            //特殊效果显示
            mQualityFlyFrame.CustomActive(mData.hasSpecialEffect);

            //高级特效文字说明
            if (null != mHighEffectHint)
            {
                mHighEffectHint.CustomActive(mData.hasSpecialEffect);
                if(mData.hasSpecialEffect)
                    mHighEffectHint.text = string.Format(mHighEffectHint.FormatStr, mData.nextHighEffectDistance);
            }

            //高阶特效查看链接回调
            if(null != mBtnHighSkillEffect)
            {
                mBtnHighSkillEffect.gameObject.SetActive(mData.hasSpecialEffect);
                mBtnHighSkillEffect.onClick = null;
                if (mData.hasSpecialEffect)
                    mBtnHighSkillEffect.onClick = OnClickHighEffect;
            }

            //高阶特效显微镜
            if (null != mMicroScope)
                mMicroScope.gameObject.SetActive(mData.hasSpecialEffect);
        }

        //显示高阶特效显微镜[这个是满级后才会有的]
        mQualityFlyFull.CustomActive(levelFull);
        //高阶特效查看链接回调
        if (null != mBtnLookUp)
        {
            if(levelFull)
                mBtnLookUp.onClick = OnClickHighEffect;
            else
                mBtnLookUp.onClick = null;
        }

        //技能提升需要玩家等级
        if (null != mUpgradeNeedPlayerLevel && !levelFull && null != mData && null != mData.nextItem)
            mUpgradeNeedPlayerLevel.text = CSString.Format(mData.nextItem.studyLevel <= CSMainPlayerInfo.Instance.Level ? 540 : 541,mData.nextItem.studyLevel);

        //自动配置
        UpdateSkillAutoPlayState();

        //技能升级消耗
        if (!levelFull)
        {
            var itemBarDatas = mData.nextItem.cost.GetItemBarDatas(mPoolHandleManager, mClientEvent);
            UIItemBarManager.Instance.Bind(mUpgradeCosts, itemBarDatas);
            itemBarDatas.Clear();
            mPoolHandleManager.Recycle(itemBarDatas);
        }
        else
        {
            mUpgradeCosts.MaxCount = 0;
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
        //适配技能高度 (暂时取消技能适配 因为技能描述加了滚动)
        //mCurSkill.height = Math.Max(130, mCurSkillHead.height + mCurDesc.height);
        //int qualityFlyHeight = null != mQualityFlyFrame && mQualityFlyFrame.gameObject.activeInHierarchy ? mQualityFlyFrame.height : 0;

        //mNextSkill.transform.localPosition = mCurSkill.transform.localPosition + new UnityEngine.Vector3(0, -mCurSkill.height + 12, 0);
        //mNextSkill.height = Math.Max(147, mNextSkillHead.height + mNextDesc.height) + qualityFlyHeight;

        //var bottomPos = mNextSkill.transform.localPosition + new UnityEngine.Vector3(0, -mNextSkill.height - 10, 0);
        //bottomPos.y = Mathf.Min(-139, bottomPos.y);
        //mBottom.transform.localPosition = bottomPos;
    }

    protected void UpdateSkillAutoPlayState()
    {
        if (null == mData || null == mData.item || !mData.learned)
        {
            mToggleSwitch.CustomActive(false);
            return;
        }

        bool autoPlay = CSSkillInfo.Instance.GetSkillAutoPlay(mData.item.id);
        if (null != mToggleSwitch)
        {
            mToggleSwitch.CustomActive(mData.item.automatically != 0);
            mToggleSwitch.onChange.Clear();
            mToggleSwitch.value = autoPlay;
        }

        ScriptBinder?._SetAction(autoPlay ? CONST_SWITCH_ON : CONST_SWITCH_OFF);

        if (null != mToggleSwitch)
            EventDelegate.Add(mToggleSwitch.onChange, OnToggleSwitchChanged);
    }

    protected void OnToggleSwitchChanged()
    {
        if (null != mToggleSwitch && null != mData)
        {
            Net.ReqSetSkillAutoStateMessage(mData.item.id, mToggleSwitch.value);
        }
    }

    protected void OnClickHighEffect(UnityEngine.GameObject go)
    {
        if (null == mData)
            return;

        UIManager.Instance.CreatePanel<UISkillUpgradeEffectPanel>(f =>
        {
            (f as UISkillUpgradeEffectPanel).Show(mData);
        });
    }

    protected void OnClickSkillUpgrade(UnityEngine.GameObject go)
    {
        if (null == mData || !mData.callUpgrade)
            return;
        Net.ReqUpgradeSkill(mData.item.id);
    }

    protected void OnClickHelper(UnityEngine.GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.SkillHelper);
    }

    protected override void OnDestroy()
    {
        if (destroyed)
            return;

        mClientEvent.RemoveEvent(CEvent.SkillUpgradeSucceed, OnSkillUpgradeSucceed);
        mClientEvent.RemoveEvent(CEvent.AttachedSkillModified, OnAttachedSkillModified);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.OnSkillSlotChanged, OnSkillSlotChanged);
        mClientEvent.RemoveEvent(CEvent.OnSkillAutoPlayChanged, OnSkillAutoPlayChanged);
        mClientEvent.RemoveEvent(CEvent.OnSkillAdded, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.OnSkillRemoved, OnItemChange);

        if (null != mConfigPanel)
        {
            mConfigPanel.Destroy();
            mConfigPanel = null;
        }
        UIItemBarManager.Instance.UnBind(mUpgradeCosts);
        mUpgradeCosts = null;
        UISkillBinder.Clear();
        mGridSkills.UnBind<UISkillBinder>();
        mGridSkills = null;
        base.OnDestroy();
    }
}