using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonSkillBinder : UIBinder
{
    UISprite sp_skill_icon;
    UILabel lb_skill_name;
    GameObject go_skill_lv;
    UILabel lb_skill_lv;
    UISprite sp_skill_installed;
    UISprite sp_choiced;
    UIEventListener btn_skill;
    protected DilaoSkillInfo data;

    public UIDungeonSkillBinder uiBinder;
    EventHanlderManager mClientEvent;
    public override void Init(UIEventListener handle)
    {
        //Debug.Log("init");
        sp_skill_icon = Get<UISprite>("sp_skill_icon");
        lb_skill_name = Get<UILabel>("lb_skill_name");
        go_skill_lv = handle.transform.Find("sp_skill_lv").gameObject;
        lb_skill_lv = Get<UILabel>("sp_skill_lv/lb_skill_lv");
        sp_skill_installed = Get<UISprite>("sp_skill_installed");
        sp_choiced = handle.transform.Find("sp_choiced").gameObject.GetComponent<UISprite>();
        btn_skill = UIEventListener.Get(sp_skill_icon.gameObject, null);

        //HotManager.Instance.EventHandler.AddEvent(CEvent.SetSkillSelectedEffect, OnSetSkillEffect);
        mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
        HotManager.Instance.EventHandler.AddEvent(CEvent.GuWuSkill, OnGuWuSkill);
    }

    private void OnGuWuSkill(uint uiEvtID, object data)
    {
        if (data == null)
        {
            FNDebug.Log("skill id is null");
        }

        var skildata = this.data.skillInfoData;
        
        //Debug.Log(this.data.item.skillGroup + "||" + (int)data);
        bool isShow = skildata.item.skillGroup == (int) data;
        sp_skill_installed?.gameObject.SetActive(isShow);
        sp_choiced?.gameObject.SetActive(isShow);
    }

    // private void OnSetSkillEffect(uint uiEvtID, object argv)
    // {
    //     // int skillId = (int)argv;
    //     //
    //     // if (skillId == 0)
    //     // {
    //     //     SetSelected(false);
    //     // }
    //     // else
    //     // {
    //     //     
    //     //     if (null != this.data && null != this.data.item)
    //     //     {
    //     //         this.SetSelected(this.data.item.id == skillId);
    //     //     }
    //     // }
    // }

    public override void Bind(object data)
    {
        this.data = data as DilaoSkillInfo;
        if (data != null)
        {
            var skildata = this.data.skillInfoData;
            var validItem = skildata.currentValidItem;
            if (validItem != null)
            {
                sp_choiced.gameObject.SetActive(false);
                if (btn_skill !=null)
                {
                    btn_skill.onClick = null;
                    btn_skill.onClick = OnSkillSelected;
                }
                //技能名称
                if (null != lb_skill_name)
                    lb_skill_name.text = $"{validItem.name.BBCode(ColorType.SecondaryText)}";
                //技能等级
                if (null != lb_skill_lv)
                {
                    string str;
                    
                    if (validItem.skillGroup == this.data.skillgroup)
                    {
                        str = $"{validItem.level}".BBCode(ColorType.Green);
                    }
                    else
                    {
                        str = $"{validItem.level}".BBCode(ColorType.MainText);
                    }

                    lb_skill_lv.text = str;
                }
                
                //技能图标
                if (null != sp_skill_icon)
                {
                    sp_skill_icon.spriteName = validItem.icon;
                    //sp_skill_icon.color = this.data.learned ? Color.white : Color.black;
                }
                //是否鼓舞
                sp_skill_installed.gameObject.SetActive(false);
            }
        }

    }

    private void OnSkillSelected(GameObject obj)
    {
        Utility.OpenSkillTipPanel(data.skillInfoData.currentValidItem.id);
    }

    public override void OnDestroy()
    {
        sp_skill_icon = null;
        lb_skill_name = null;
        go_skill_lv = null;
        lb_skill_lv = null;
        sp_skill_installed = null;
        sp_choiced.gameObject.SetActive(true);
        sp_choiced = null;
    }
    
}
