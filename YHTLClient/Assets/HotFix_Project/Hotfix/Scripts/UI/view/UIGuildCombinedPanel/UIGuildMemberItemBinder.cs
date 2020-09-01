using UnityEngine;

public class GuildMemberItemData
{
    public union.UnionMember member;
    public int index;
}

public class UIGuildMemberItemBinder : UIBinder
{
    UILabel lb_name;
    UILabel lb_level;
    UILabel lb_career;
    UILabel lb_offline;
    UILabel lb_duty;
    UILabel lb_contribute;
    UISprite sprite_bg;
    UIToggle tg_bg;
    UISprite btn_select;
    UISprite Privilege;
    UISprite YvSpeakIcon;
    UILabel lb_power;
    public static long curSelectedRoleId
    {
        private set;
        get;
    }

    public static void Clear()
    {
        curSelectedRoleId = 0;
    }

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_level = Get<UILabel>("lb_level");
        lb_career = Get<UILabel>("lb_career");
        lb_offline = Get<UILabel>("lb_offline");
        lb_duty = Get<UILabel>("lb_duty");
        lb_contribute = Get<UILabel>("lb_contribute");
        sprite_bg = Get<UISprite>("btn_bg");
        tg_bg = Get<UIToggle>("btn_bg");
        btn_select = Get<UISprite>("btn_select");
        Privilege = Get<UISprite>("Privilege");
        YvSpeakIcon = Get<UISprite>("YvSpeakIcon");
        lb_power = Get<UILabel>("lb_power");
        if(null != sprite_bg)
            UIEventListener.Get(sprite_bg.gameObject).onClick = OnMemberClick;
    }

    private void OnMemberClick(GameObject button)
    {
        var tabInfo = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionMemberInfo);
        if (null == tabInfo || null == tabInfo.unionInfo)
            return;
        var unionInfo = tabInfo.unionInfo;
        if (null == memberData || null == memberData.member)
            return;
        long roleId = memberData.member.roleId;
        if (roleId == CSMainPlayerInfo.Instance.ID)
        {
            UtilityTips.ShowRedTips(813);
            return;
        }
        curSelectedRoleId = roleId;
        union.UnionMember info = null;
        for (int i = 0; i < unionInfo.members.Count; i++)
        {
            if (unionInfo.members[i].roleId == roleId)
            {
                info = unionInfo.members[i];
                break;
            }
        }
        if (info != null)
        {
            MenuInfo data = new MenuInfo();
            bool canSpeak = info.canSpeak;
            bool isBaby = false;//info.isBaby;
            data.SetFamilyTips(info.roleId, info.name, info.teamId, info.position, info.contribute, info.isOnline, canSpeak, isBaby, info.sex, 0, info.level, info.career);
            data.sundryId = (int)PanelSelcetType.GuildPanel;
            data.myPosition = CSMainPlayerInfo.Instance.GuildPos;
            UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
            {
                (f as UIRoleSelectionMenuPanel).ShowSelectData(data);
            });
        }
    }

    GuildMemberItemData memberData;
    public override void Bind(object data)
    {
        memberData = data as GuildMemberItemData;
        if (null == memberData)
        {
            return;
        }

        var member = memberData.member;
        var color = UtilityColor.GetColorString(member.isOnline ? ColorType.MainText : ColorType.WeakText);

        if (null != lb_name)
            lb_name.text = color + member.name;

        if (null != lb_level)
            lb_level.text = color + member.level;

        if (null != lb_career)
            lb_career.text = color + Utility.GetCareerName(member.career);

        if (lb_power != null)
            lb_power.text = color + member.fighting;

        if (lb_contribute != null)
            lb_contribute.text = color + member.contribute;

        YvSpeakIcon.CustomActive(false);//�����˵��Ȩ��,��ʾ

        SetOffLineInfo(color);

        if (null != lb_duty)
            lb_duty.text = $"{color}{CSGuildInfo.Instance.GetGuildPos(member.position)}";

        if(null != tg_bg)
        {
            tg_bg.enabled = CSMainPlayerInfo.Instance.ID != member.roleId;
            tg_bg.value = curSelectedRoleId == member.roleId;
        }

        if (null != btn_select)
            btn_select.alpha = curSelectedRoleId == member.roleId ? 1 : 0;

        if(null != sprite_bg)
            sprite_bg.spriteName = memberData.index % 2 == 0 ? "list_subbg1" : "list_subbg2";
    }

    protected void SetOffLineInfo(string color)
    {
        if (lb_offline != null)
        {
            var member = memberData.member;
            if (!member.isOnline)
            {
                string strDesc = "";
                long destime = CSServerTime.DateTimeToStamp(CSServerTime.Now) - member.lastLogoutTime / 1000;
                if (member.lastLogoutTime == 0)
                {
                    strDesc = CSString.Format(799, string.Empty);
                }
                else if (destime < 60)
                {
                    strDesc = CSString.Format(800);
                }
                else if (destime < 3600)
                {
                    strDesc = CSString.Format(801, destime / 60);
                }
                else if (destime < 3600 * 24)
                {
                    strDesc = CSString.Format(802, destime / 3600);
                }
                else if (destime < 3600 * 24 * 7)
                {
                    strDesc = CSString.Format(803, destime / (3600 * 24));
                }
                else
                {
                    strDesc = CSString.Format(804, destime / (3600 * 24 * 7));
                }

                lb_offline.text = strDesc.BBCode(ColorType.WeakText);
            }
            else
            {
                /*if (false /*member.curServerType == 3#1#)
                {
                    lb_offline.text = CSString.Format(805, color);
                }
                else
                {*/
                    lb_offline.text = CSString.Format(806,string.Empty).BBCode(ColorType.Green);
                //}
            }
        }
    }


    public override void OnDestroy()
    {
        lb_name = null;
        lb_level = null;
        lb_career = null;
        lb_offline = null;
        lb_duty = null;
        lb_contribute = null;
        sprite_bg = null;
        btn_select = null;
        Privilege = null;
        YvSpeakIcon = null;
        lb_power = null;
    }
}