using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMyTeamBinder : UIBinder
{
    public bool isAdd; //是否是加号
    public Action<team.TeamMember, bool> actionItem;
    GameObject gp;
    GameObject obj_isleader;
    UILabel lb_name;
    UILabel lb_level;
    GameObject bottom;

    GameObject sp_add;

    // UISprite sp_head;
    GameObject sp_model;
    GameObject sp_weapen;
    team.TeamMember teamMember;

    private UISprite sp_career;
    private Map<int, string> careerIconName;

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        obj_isleader = Get<GameObject>("obj_isleader");
        lb_name = Get<UILabel>("lb_name");
        lb_level = Get<UILabel>("lb_level");
        bottom = Get<GameObject>("bottom");
        sp_add = Get<GameObject>("sp_add");
        // sp_head = gp.GetComponent<UISprite>();
        sp_model = Get<GameObject>("sp_model");
        sp_weapen = Get<GameObject>("sp_weapen");
        sp_career = Get<UISprite>("sp_career", lb_level.transform);
        UIEventListener.Get(gp).onClick = OnClickItem;
        
        if (null==careerIconName)
            careerIconName = new Map<int, string>();
        careerIconName.Add(1, "mission_zhan");
        careerIconName.Add(2, "mission_fa");
        careerIconName.Add(3, "mission_dao");
    }

    void OnClickItem(GameObject go)
    {
        actionItem?.Invoke(teamMember, isAdd);
    }

    public override void Bind(object data)
    {
        if (data != null)
        {
            teamMember = (team.TeamMember) data;
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        if (isAdd)
        {
            //最后一个是“+”号
            obj_isleader.SetActive(false);
            lb_name.gameObject.SetActive(false);
            lb_level.gameObject.SetActive(false);
            bottom.SetActive(false);
            sp_add.SetActive(true);
            sp_model.SetActive(false);
            sp_weapen.SetActive(false);
            // sp_head.enabled = false;
        }
        else
        {
            obj_isleader.SetActive(index == 0); //队长标志
            lb_name.gameObject.SetActive(true);
            lb_level.gameObject.SetActive(true);
            bottom.SetActive(true);
            sp_add.SetActive(false);
            sp_add.SetActive(false);
            sp_model.SetActive(true);
            sp_weapen.SetActive(true);
            sp_career.spriteName = careerIconName[teamMember.career];
            lb_name.text = teamMember.name;
            lb_level.text = $"{lb_level.FormatStr}{teamMember.level}";
            AvatarModelHelper.LoadAvatarModel(sp_model, sp_weapen, teamMember.clothes,
                teamMember.weapon, teamMember.fashionClothesId, teamMember.fashionWeaponId, teamMember.suitId,
                teamMember.sex);
        }
    }

    public override void OnDestroy()
    {
        gp = null;
        obj_isleader = null;
        lb_name = null;
        lb_level = null;
        bottom = null;
        sp_add = null;
        // sp_head = null;
        sp_model = null;
        sp_weapen = null;
        actionItem = null;
        teamMember = null;
        sp_career = null;
        careerIconName = null;
    }
}