using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandBookCardBinder : UIBinder
{
    protected UISprite sp_icon;
    protected UISprite sp_quality_frame;
    protected UILabel lb_city_name;
    protected UILabel lb_camp_name;
    protected UILabel lb_grade_name;//精英，普通，首领
    protected UILabel lb_level;
    protected UISprite sp_add;
    protected UISprite sp_lock;
    protected UISprite sp_bg_setup_mode;
    protected UISprite sp_bg_general_mode;
    protected GameObject go_check;
    protected UISprite sp_setuped;
    protected UISprite sp_can_quality_up;
    protected Transform ts_can_quality_up;
    protected GameObject go_card;
    protected GameObject go_slot;
    protected Transform go_red;
    protected UISprite mask_bg;
    protected Transform go_effect;
    protected Transform go_resident_effect;
    protected Transform go_unlock_effect;
    protected Transform go_upgrade_effect;
    protected Transform go_main_effect;
    protected Transform go_second_effect;
    protected UISpriteAnimation sp_unlock_effect;
    protected UISpriteAnimation sp_resident_effect;
    protected Vector3 setupedPos = Vector3.zero;
    protected Vector3 qualityUpPos = Vector3.zero;
    public override void Init(UIEventListener handle)
    {
        var ScriptBinder = handle.GetComponent<ScriptBinder>();
        go_red = ScriptBinder.GetObject("go_red") as UnityEngine.Transform;
        go_card = ScriptBinder.GetObject("go_card") as UnityEngine.GameObject;
        go_slot = ScriptBinder.GetObject("go_slot") as UnityEngine.GameObject;
        var cardTransform = ScriptBinder.GetObject("cardTransform") as UnityEngine.Transform;
        sp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
        sp_quality_frame = ScriptBinder.GetObject("sp_quality_frame") as UISprite;
        lb_city_name = ScriptBinder.GetObject("lb_city_name") as UILabel;
        lb_camp_name = ScriptBinder.GetObject("lb_camp_name") as UILabel;
        lb_grade_name = ScriptBinder.GetObject("lb_grade_name") as UILabel;
        lb_level = ScriptBinder.GetObject("lb_level") as UILabel;
        go_check = ScriptBinder.GetObject("go_check") as UnityEngine.GameObject;
        sp_setuped = ScriptBinder.GetObject("sp_setuped") as UISprite;
        go_resident_effect = ScriptBinder.GetObject("go_resident_effect") as UnityEngine.Transform;
        sp_resident_effect = ScriptBinder.GetObject("sp_resident_effect") as UISpriteAnimation;
        //var slotTransform = ScriptBinder.GetObject("slotTransform") as UnityEngine.Transform;
        //go_unlock_effect = ScriptBinder.GetObject("go_unlock_effect") as UnityEngine.GameObject;
        sp_unlock_effect = ScriptBinder.GetObject("sp_unlock_effect") as UISpriteAnimation;
        sp_bg_setup_mode = ScriptBinder.GetObject("sp_bg_setup_mode") as UISprite;
        sp_bg_general_mode = ScriptBinder.GetObject("sp_bg_general_mode") as UISprite;
        sp_add = ScriptBinder.GetObject("sp_add") as UISprite;
        sp_lock = ScriptBinder.GetObject("sp_lock") as UISprite;

        //go_red = CachedTransform.Find("redpoint");
        //go_card = CachedTransform.Find("card").gameObject;
        //var cardTransform = go_card.transform;
        //sp_icon = Get<UISprite>("icon", cardTransform);
        //sp_quality_frame = Get<UISprite>("sp_quality_frame", cardTransform);
        //lb_city_name = Get<UILabel>("lb_city_name", cardTransform);
        //lb_camp_name = Get<UILabel>("lb_camp_name", cardTransform);
        //lb_grade_name = Get<UILabel>("lb_grade_name", cardTransform);
        //lb_level = Get<UILabel>("lb_level", cardTransform);
        //go_check = cardTransform.Find("go_check").gameObject;
        //sp_setuped = Get<UISprite>("sp_setuped", cardTransform);
        if (null != sp_setuped)
            setupedPos = sp_setuped.transform.localPosition;

        sp_can_quality_up = ScriptBinder.GetObject("sp_can_quality_up") as UISprite;
        ts_can_quality_up = ScriptBinder.GetObject("ts_can_quality_up") as UnityEngine.Transform;
        if (null != ts_can_quality_up)
            qualityUpPos = ts_can_quality_up.localPosition;

        mask_bg = Get<UISprite>("mask_bg", cardTransform);

        go_effect = ScriptBinder.GetObject("go_effect") as UnityEngine.Transform;
        go_upgrade_effect = ScriptBinder.GetObject("go_upgrade_effect") as UnityEngine.Transform;

        go_main_effect = ScriptBinder.GetObject("go_merge_main") as UnityEngine.Transform;
        go_second_effect = ScriptBinder.GetObject("go_merge_second") as UnityEngine.Transform;
        //go_second_effect = CachedTransform.Find("go_merge_second");

        //go_slot = CachedTransform.Find("slot").gameObject;
        //var slotTransform = go_slot.transform;
        //go_unlock_effect = slotTransform.Find("go_unlock_effect");
        //if (null != go_unlock_effect)
            //sp_unlock_effect = go_unlock_effect.GetComponent<UISpriteAnimation>();
        //sp_bg_setup_mode = Get<UISprite>("bg_setup", slotTransform);
        //sp_bg_general_mode = Get<UISprite>("bg_general", slotTransform);
        //sp_add = Get<UISprite>("sp_add", slotTransform);
        //sp_lock = Get<UISprite>("sp_lock", slotTransform);
        if (null != handle)
        {
            handle.onClick = this.OnClick;
            handle.onKeepPress = this.OnPress;
        }

        if (null != sp_unlock_effect)
            sp_unlock_effect.OnFinish = OnUnLockEffectPlayFinished;

        HotManager.Instance.EventHandler.AddEvent(CEvent.RemoveSelectedFlag, OnSelectedChanged);
    }

    protected void OnPress(GameObject go)
    {
        if(null != this.data)
        {
            this.data.onKeepPressed?.Invoke(this.data);
        }
    }

    protected void OnUnLockEffectPlayFinished()
    {
        go_unlock_effect.CustomActive(false);
    }

    public void PlayUpgradeQualityEffect(bool main = false)
    {
        if(null != go_main_effect && main)
        {
            go_main_effect.gameObject.PlayEffect("effect_handbook_merge_add",12);
            //CSEffectPlayMgr.Instance.ShowUIEffect(go_main_effect.gameObject, "effect_handbook_merge_add", 10, false);
        }

        if (null != go_second_effect && !main)
        {
            go_second_effect.gameObject.PlayEffect("effect_handbook_mergecailiao_add",12);
            //CSEffectPlayMgr.Instance.ShowUIEffect(go_second_effect.gameObject, "effect_handbook_mergecailiao_add", 10, false);
        }
    }

    public void PlayUpgradeLevelEffect()
    {
        if(null != go_upgrade_effect)
        {
            go_upgrade_effect.gameObject.PlayEffect("effect_handbook_upgrade_add",12);
            //CSEffectPlayMgr.Instance.ShowUIEffect(go_upgrade_effect.gameObject, "effect_handbook_upgrade_add", 10, false);
        }
    }

    protected void OnSelectedChanged(uint id,object argv)
    {
        if(null != this.data && this.data.Guid == (long)argv)
        {
            UpdateCheckVisible();
        }
    }

    protected void UpdateCheckVisible()
    {
        if (null != go_check)
            go_check.gameObject.SetActive(data.HasFlag(HandBookSlotData.CardFlag.CF_SELECTED | HandBookSlotData.CardFlag.CF_ENABLE_SELECTED));
    }

    protected HandBookSlotData data;

    public override void Bind(object value)
    {
        data = value as HandBookSlotData;
        if (null == data)
            return;

        if(null != data.HandBook)
        {
            if (null != sp_icon)
                sp_icon.spriteName = data.GetBgSprite();

            if (null != sp_bg_setup_mode)
                sp_bg_setup_mode.gameObject.SetActive(false);

            if (null != sp_bg_general_mode)
                sp_bg_general_mode.gameObject.SetActive(false);

            if (null != sp_quality_frame)
                sp_quality_frame.spriteName = data.GetQualitySprite();

            if (null != lb_camp_name)
                lb_camp_name.text = data.GetCampShortName();

            if (null != lb_city_name)
                lb_city_name.text = data.GetMapShortName();

            if(null != lb_grade_name)
            {
                lb_grade_name.text = data.GetPositionShortName();
            }

            if (null != lb_level)
                lb_level.text = data.GetLevel();
            
            sp_setuped.CustomActive(data.Setuped);

            if (data.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FLAG))
                sp_can_quality_up.CustomActive(data.CanUpgradeQuality);
            else if (data.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_LEVEL_FLAG))
                sp_can_quality_up.CustomActive(data.CanUpgrade);
            else
                sp_can_quality_up.CustomActive(false);

            if (null != sp_can_quality_up)
            {
                if (data.HasFlag(HandBookSlotData.CardFlag.CF_UPGRADE_LEVEL))
                {
                    sp_can_quality_up.spriteName = "yisji";
                }
                else
                {
                    sp_can_quality_up.spriteName = "yispin";
                }
                ts_can_quality_up.localPosition = data.Setuped ? qualityUpPos : setupedPos;
            }

            if (null != mask_bg)
                mask_bg.gameObject.SetActive(data.HasFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE));

            if(null != go_resident_effect)
            {
                int quality = this.data.HandBook.Quality;
                if (quality == 4)
                {
                    go_resident_effect.gameObject.PlayEffect(17302, 10, true);
                }
                else if (quality == 5)
                {
                    go_resident_effect.gameObject.PlayEffect(17303, 10, true);
                }
                else
                    go_resident_effect.gameObject.StopEffect();
                //CSEffectPlayMgr.Instance.ShowUIEffect(go_resident_effect.gameObject,this.data.HandBook.QualityEffect(), 10, true);
                //if(null != sp_resident_effect && !sp_resident_effect.enabled)
                //{
                //    sp_resident_effect.enabled = true;
                //}
            }

            if(null != go_effect)
            {
                if(this.data.HasFlag(HandBookSlotData.CardFlag.CF_UPGRADE_LEVEL))
                {
                    bool canUpgrade = this.data.CanUpgrade;
                    if (canUpgrade)
                    {
                        //TODO:
                        CSEffectPlayMgr.Instance.ShowUIEffect(go_effect.gameObject, "effect_star_add", 10, false);
                    }
                    if(go_effect.gameObject.activeSelf != canUpgrade)
                        go_effect.gameObject.SetActive(canUpgrade);
                }
                else if (this.data.HasFlag(HandBookSlotData.CardFlag.CF_UPGRADE_QUALITY))
                {
                    bool canUpgradeQuality = this.data.CanUpgradeQuality;
                    if (canUpgradeQuality)
                    {
                        //TODO:
                        CSEffectPlayMgr.Instance.ShowUIEffect(go_effect.gameObject, "effect_star_add", 10, false);
                    }
                    if (go_effect.gameObject.activeSelf != canUpgradeQuality)
                        go_effect.gameObject.SetActive(canUpgradeQuality);
                }
                else
                {
                    if(go_effect.gameObject.activeSelf)
                        go_effect.gameObject.SetActive(false);
                }
            }

            UpdateCheckVisible();

            if (null != go_card)
                go_card.gameObject.SetActive(true);

            if (null != go_slot)
                go_slot.gameObject.SetActive(false);

            go_unlock_effect.CustomActive(false);
        }
        else
        {
            if (null != go_resident_effect)
            {
                go_resident_effect.gameObject.StopEffect();
                //CSEffectPlayMgr.Instance.Recycle(go_resident_effect.gameObject);
                //sp_resident_effect.enabled = false;
            }

            sp_can_quality_up.CustomActive(false);

            sp_bg_setup_mode.CustomActive(data.HasFlag(HandBookSlotData.CardFlag.CF_SETUP_MODE));

            sp_bg_general_mode.CustomActive(!data.HasFlag(HandBookSlotData.CardFlag.CF_SETUP_MODE));

            sp_lock.CustomActive(data.HasFlag(HandBookSlotData.CardFlag.CF_LOCKED));

            sp_add.CustomActive(data.HasFlag(HandBookSlotData.CardFlag.CF_ADDED) && !data.HasFlag(HandBookSlotData.CardFlag.CF_LOCKED));

            go_card.CustomActive(false);

            go_slot.CustomActive(true);

            if (this.data.HasFlag(HandBookSlotData.CardFlag.CF_UNLOCK_EFFECT))
            {
                this.data.RemoveFlag(HandBookSlotData.CardFlag.CF_UNLOCK_EFFECT);
                FNDebug.LogFormat($"play effect PlaySlotUnLockEffect");
                if (null != go_unlock_effect)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(go_unlock_effect.gameObject, "effect_star_add", 10, false);
                    go_unlock_effect.CustomActive(true);
                }
                else
                {
                    go_unlock_effect.CustomActive(false);
                }
            }
            else
            {
                go_unlock_effect.CustomActive(false);
            }
        }

        go_red.CustomActive(data.HasFlag(HandBookSlotData.CardFlag.CF_REDPOINT));
    }

    protected void OnClick(GameObject go)
    {
        if (null == this.data || !this.data.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED))
            return;

        if(this.data.HasFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE))
        {
            this.data.onClicked?.Invoke(this.data);
            return;
        }

        if(this.data.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED))
        {
            if(this.data.HasFlag(HandBookSlotData.CardFlag.CF_SELECTED))
            {
                if (this.data.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED))
                {
                    this.data.RemoveFlag(HandBookSlotData.CardFlag.CF_SELECTED);
                    UpdateCheckVisible();

                    this.data.onChoiceChanged?.Invoke(this.data, false);
                }
            }
            else
            {
                if(null == this.data.onChoiceFilter || !this.data.onChoiceFilter.Invoke())
                {
                    this.data.AddFlag(HandBookSlotData.CardFlag.CF_SELECTED);
                    UpdateCheckVisible();
                    this.data.onChoiceChanged?.Invoke(this.data, true);
                }
            }
        }
        else
        {
            this.data.onClicked?.Invoke(this.data);
        }
    }

    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.UnReg((int)CEvent.RemoveSelectedFlag, OnSelectedChanged);
        if(null != go_upgrade_effect)
        {
            CSEffectPlayMgr.Instance.Recycle(go_upgrade_effect.gameObject);
            go_upgrade_effect = null;
        }
        if (null != go_effect)
        {
            CSEffectPlayMgr.Instance.Recycle(go_effect.gameObject);
            go_effect = null;
        }
        if (null != go_unlock_effect)
        {
            CSEffectPlayMgr.Instance.Recycle(go_unlock_effect.gameObject);
            go_unlock_effect = null;
        }
        if (null != go_resident_effect)
        {
            CSEffectPlayMgr.Instance.Recycle(go_resident_effect.gameObject);
            go_resident_effect = null;
        }

        //不要在这里RESET这里的数据是由CSHandBookManager管理的
        //this.data.Reset();
        if (null != this.data)
        {
            this.data.onClicked = null;
            this.data.onChoiceChanged = null;
            this.data = null;
        }
        sp_bg_setup_mode = null;
        sp_bg_general_mode = null;
        sp_icon = null;
        sp_quality_frame = null;
        lb_camp_name = null;
        lb_city_name = null;
        lb_level = null;
        sp_add = null;
        sp_lock = null;
        go_check = null;
        sp_setuped = null;
        go_card = null;
        go_slot = null;
    }
}
