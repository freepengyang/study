using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBinder : UIBinder
{
    UISprite sp_skill_icon;
    UILabel lb_skill_name;
    GameObject go_skill_lv;
    UILabel lb_skill_lv;
    UISprite sp_skill_up_arrow;
    UISprite sp_skill_installed;
    UISprite sp_choiced;
    UIEventListener btn_skill;
    UIDragDropItem btn_skill_drag;
    public override void Init(UIEventListener handle)
    {
        sp_skill_icon = Get<UISprite>("sp_skill_icon");
        btn_skill_drag = Get<UIDragDropItem>("sp_skill_icon");
        lb_skill_name = Get<UILabel>("sp_skill_name/lb_skill_name");
        btn_skill = UIEventListener.Get(sp_skill_icon.gameObject,null);
        go_skill_lv = handle.transform.Find("sp_skill_lv").gameObject;
        lb_skill_lv = Get<UILabel>("sp_skill_lv/lb_skill_lv");
        sp_skill_up_arrow = Get<UISprite>("sp_skill_up_arrow");
        sp_skill_installed = Get<UISprite>("sp_skill_installed");
        sp_choiced = Get<UISprite>("sp_choiced");

        HotManager.Instance.EventHandler.AddEvent(CEvent.SkillUpgradeSucceed, OnSkillUpgradeSucceed);
        HotManager.Instance.EventHandler.AddEvent(CEvent.SetSkillSelectedEffect, OnSetSkillEffect);
        HotManager.Instance.EventHandler.AddEvent(CEvent.AttachedSkillModified, OnAttachedSkillModified);
    }

    protected SkillInfoData data;
    protected static UISkillBinder ms_selected;
    public static UISkillCombinedPanel.SkillPanelType Mode { get; set; }

    protected void SetSelected(bool selected)
    {
        sp_choiced.gameObject.SetActive(selected);
        CSEffectPlayMgr.Instance.ShowUIEffect(sp_choiced.gameObject,"effect_skillsel_add", 10, true, false);
    }

    protected void OnSkillSelected(GameObject go)
    {
        if (ms_selected != this)
            ms_selected?.SetSelected(false);
        ms_selected = this;
        ms_selected?.SetSelected(true);
        if (null != go)
        {
            data?.onSkillClicked?.Invoke(data);
        }
    }

    protected void OnDragStart(GameObject go)
    {
        if (null != go && null != data)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillDragStart, this.data);
        }
    }

    public override void Bind(object data)
    {
        this.data = data as SkillInfoData;
        if(null != data)
        {
            if(null != go_skill_lv)
                go_skill_lv.gameObject.SetActive(this.data.learned);

            //图标点击回调
            if(null != btn_skill)
            {
                btn_skill.parameter = this.data;
                btn_skill.onClick = null;
                btn_skill.onDragStart = null;
                btn_skill_drag.enabled = Mode == UISkillCombinedPanel.SkillPanelType.SPT_CONFIG && this.data.canBeDraged;
                if (Mode == UISkillCombinedPanel.SkillPanelType.SPT_SKILL)
                    btn_skill.onClick = OnSkillSelected;
                if (Mode == UISkillCombinedPanel.SkillPanelType.SPT_CONFIG && this.data.canBeDraged)
                    btn_skill.onDragStart = OnDragStart;
            }

            //技能名称
            if (null != lb_skill_name)
                lb_skill_name.text = $"{this.data.item.name.BBCode(ColorType.SecondaryText)}";

            //技能等级
            if(null != lb_skill_lv)
            {
                lb_skill_lv.text = $"{this.data.currentValidItem.level}".BBCode(ColorType.MainText);
            }

            //技能图标
            if (null != sp_skill_icon)
            {
                sp_skill_icon.spriteName = this.data.item.icon;
                sp_skill_icon.color = this.data.learned ? Color.white : Color.black;
            }

            //可以升级
            sp_skill_up_arrow.CustomActive(this.data.canUpgrade && Mode == UISkillCombinedPanel.SkillPanelType.SPT_SKILL);

            //已经装备
            if(null != sp_skill_installed)
                sp_skill_installed.gameObject.SetActive(CSSkillInfo.Instance.GetSkillInstalled(this.data.item.id));
        }
    }

    protected void OnSkillUpgradeSucceed(uint id,object argv)
    {
        SkillInfoData newSkill = argv as SkillInfoData;
        if (null != newSkill && null != this.data && null != this.data.item && this.data.item.skillGroup == newSkill.item.skillGroup)
        {
            Bind(newSkill);
        }
    }

    protected void OnAttachedSkillModified(uint id,object argv)
    {
        if(null != this.data && argv is int skillGroup && skillGroup == this.data.SkillGroup)
        {
            Bind(this.data);
        }
    }

    protected void OnSetSkillEffect(uint id, object argv)
    {
        int skillId = (int)argv;
        if(skillId == 0)
        {
            SetSelected(false);
        }
        else
        {
            if(null != this.data && null != this.data.item && this.data.item.id == skillId)
            {
                if (ms_selected != this)
                    ms_selected?.SetSelected(false);
                ms_selected = this;
                ms_selected?.SetSelected(true);
            }
        }
    }

    public static void Clear()
    {
        ms_selected = null;
    }

    public override void OnDestroy()
    {
        if (null != sp_choiced)
        {
            CSEffectPlayMgr.Instance.Recycle(sp_choiced.gameObject);
        }
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.AttachedSkillModified, OnAttachedSkillModified);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.SetSkillSelectedEffect, OnSetSkillEffect);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.SkillUpgradeSucceed, OnSkillUpgradeSucceed);
        sp_skill_icon = null;
        btn_skill_drag = null;
        lb_skill_name = null;
        btn_skill = null;
        go_skill_lv = null;
        lb_skill_lv = null;
        sp_skill_up_arrow = null;
        sp_skill_installed = null;
        sp_choiced = null;
    }
}
