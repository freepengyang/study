using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWarPetRefineBinder : UIBinder
{
    public bool isSelect = false;
    public Action<int> actionItem;
    private GameObject gp;
    private GameObject skill;
    private UISprite sp_skill_icon;
    private UILabel lb_skillname;
    private GameObject lb_hint;
    private GameObject select;
    private GameObject redpoint;
    private WarPetSkillData warPetSkillData;

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        skill = Get<GameObject>("skill");
        sp_skill_icon = Get<UISprite>("sp_skill_icon", skill.transform);
        lb_skillname = Get<UILabel>("lb_skillname");
        lb_hint = Get<GameObject>("lb_hint");
        select = Get<GameObject>("select");
        redpoint = Get<GameObject>("redpoint");
        UIEventListener.Get(gp).onClick = OnClickItem;
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        warPetSkillData = (WarPetSkillData) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (warPetSkillData == null) return;
        select.SetActive(isSelect);
        redpoint.SetActive(warPetSkillData.ID == 0 && warPetSkillData.Special == 0 &&
                           CSWarPetRefineInfo.Instance.GetCurCost(0).IsItemsEnough());
        if (warPetSkillData.Special == 1)
        {
            lb_hint.SetActive(false);
            lb_skillname.text = warPetSkillData.CfgSkill?.name;
            lb_skillname.gameObject.SetActive(true);
            sp_skill_icon.spriteName = warPetSkillData.CfgSkill?.icon;
            sp_skill_icon.gameObject.SetActive(true);
            sp_skill_icon.color = warPetSkillData.ID == 0 ? Color.black : Color.white;
        }
        else
        {
            lb_hint.SetActive(warPetSkillData.ID == 0);
            lb_skillname.text = warPetSkillData.CfgSkill?.name;
            lb_skillname.gameObject.SetActive(warPetSkillData.ID > 0);
            sp_skill_icon.spriteName = warPetSkillData.CfgSkill?.icon;
            sp_skill_icon.gameObject.SetActive(warPetSkillData.ID > 0);
        }
    }

    void OnClickItem(GameObject go)
    {
        if (warPetSkillData.Special == 1 && warPetSkillData.ID == 0)
        {
            UtilityTips.ShowRedTips(1724);
            return;
        }

        actionItem?.Invoke(index);
    }

    public override void OnDestroy()
    {
        actionItem = null;
        gp = null;
        skill = null;
        sp_skill_icon = null;
        lb_skillname = null;
        lb_hint = null;
        select = null;
        warPetSkillData = null;
    }
}