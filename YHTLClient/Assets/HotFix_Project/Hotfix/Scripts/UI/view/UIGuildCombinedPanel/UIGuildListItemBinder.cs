using UnityEngine;

public class GuildItemData
{
    public union.ApplyUnionListResponse applyUnionList;
    public union.UnionBrief brief;
    public int index;
}

public class UIGuildListItemBinder : UIBinder
{
    UILabel lb_name;
    UILabel lb_chairman;
    UILabel lb_grade;
    UILabel lb_member;
    UILabel lb_state;
    UILabel lb_time;
    UIEventListener btn_state;
    UISprite spr_state;
    UISprite sprite_bg;
    UISprite lb_select;
    UIEventListener btn_rebuild;
    UILabel lb_rebuild;
    UIEventListener btn_war;
    CSInvoke invoker;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_chairman = Get<UILabel>("lb_chairman");
        lb_grade = Get<UILabel>("lb_grade");
        lb_member = Get<UILabel>("lb_member");
        lb_state = Get<UILabel>("lb_state/Label");
        lb_time = Get<UILabel>("lb_time");
        btn_state = Get<UIEventListener>("lb_state");
        spr_state = Get<UISprite>("lb_state");
        sprite_bg = Get<UISprite>("bg");
        lb_select = Get<UISprite>("lb_select");
        btn_rebuild = Get<UIEventListener>("btn_rebuild");
        lb_rebuild = Get<UILabel>("btn_rebuild");
        btn_war = Get<UIEventListener>("btn_war");
        if (null != sprite_bg)
            UIEventListener.Get(sprite_bg.gameObject).onClick = OnGuildItemClicked;
        if (null != btn_state)
            btn_state.onClick = OnApplyGuild;
        lb_time.CustomActive(true);
        btn_war.onClick = OnGuildWar;
        invoker = handle.gameObject.AddComponent<CSInvoke>();
    }

    public static long curSelectedGuildId
    {
        private set;
        get;
    }

    public static void Clear()
    {
        curSelectedGuildId = 0;
    }

    private void OnGuildItemClicked(GameObject button)
    {
        curSelectedGuildId = guildData.brief.unionId;
    }

    void OnGuildWar(GameObject go)
    {
        if (null == this.guildData || null == this.guildData.brief)
            return;

        if (!CSGuildInfo.Instance.CanWar(this.guildData.brief))
            return;

        long id = this.guildData.brief.unionId;

        var need = CSGuildInfo.Instance.WarCost;
        long owned = CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao);
        bool enough = need <= owned;
        UtilityTips.ShowPromptWordTips(64, () =>
        {
            if(!enough)
            {
                Utility.ShowGetWay((int)MoneyType.yuanbao);
                UtilityTips.ShowRedTips(1067,need);
                return;
            }
            Net.CSUnionDeclareWarMessage(id);
            Net.CSGetUnionTabMessage((int)UnionTab.UnionsList);
        }, CSString.Format(1066,need).BBCode(enough ? ColorType.Green : ColorType.Red),this.guildData.brief.name);
    }

    /// <summary>
    /// 申请公会
    /// </summary>
    /// <param name="obj"></param>
    void OnApplyGuild(GameObject go)
    {
        if(null != this.guildData && null != this.guildData.brief)
        {
            long id = this.guildData.brief.unionId;
            string str = CSMainPlayerInfo.Instance.GuildName;
            var applyUnionList = this.guildData.applyUnionList;

            if(Utility.HasGuild())
            {
                return;
            }

            if (applyUnionList != null && applyUnionList.allUList.Contains(id))
            {
                //暂时去掉取消申请
                //Net.CSRemoveApplyUnionMessage(id);
            }
            else
            {
                Net.CSApplyUnionMessage(id);
            }
        }
    }

    public bool Selected
    {
        get
        {
            return null != guildData && null != guildData.brief && guildData.brief.unionId == curSelectedGuildId;
        }
    }

    protected void DeclareTimeInvoke()
    {
        if (null == guildData || null == guildData.brief)
            return;
        var brief = guildData.brief;
        long time = brief.declareWarTime / 1000 + 24 * 60 * 60 - CSServerTime.Instance.TotalSeconds;
        if (null != lb_time)
        {
            var h = time / 3600 % 24;
            var m = time / 60 % 60;
            var s = time % 60;
            lb_time.text = CSString.Format(867,h,m,s);
        }
    }

    GuildItemData guildData;
    public override void Bind(object data)
    {
        invoker.CancelInvoke();

        guildData = data as GuildItemData;
        if (null == guildData || null == guildData.brief)
        {
            return;
        }
        var brief = guildData.brief;
        var applyUnionList = guildData.applyUnionList;
        if(null == applyUnionList)
            return;

        if (null != sprite_bg)
            sprite_bg.spriteName = guildData.index % 2 == 0 ? "list_subbg1" : "list_subbg2";

        if (null != lb_select)
            lb_select.alpha = Selected ? 1 : 0;

        if (null != lb_name)
            lb_name.text = brief.leaderName;

        if (null != lb_chairman)
            lb_chairman.text = brief.name;

        if (null != lb_grade)
            lb_grade.text = brief.level.ToString();

        bool canWar = CSGuildInfo.Instance.CanWar(brief);
        btn_war.CustomActive(canWar);

        if (brief.declareWarTime <= 0)
        {
            if(null != lb_time)
                lb_time.text = string.Empty;

            bool applyInCD = CSGuildInfo.Instance.IsInApplyColdTime(applyUnionList.lastUnionTime);

            bool inApply = applyUnionList != null && applyUnionList.allUList != null && applyUnionList.allUList.Contains(brief.unionId);
            if(null != lb_state)
                lb_state.text = CSString.Format(inApply ? 865 : 866);

            spr_state.enabled = !inApply;

            btn_state.CustomActive(!Utility.HasGuild());
            spr_state.color = (applyUnionList.lastUnionTime == 0 || !applyInCD) ? Color.white : Color.black;
            //lb_state.color = (applyUnionList.lastUnionTime == 0 || !applyInCD) ? UtilityColor.GetColor(ColorType.CommonButtonGreen) : UtilityColor.GetColor(ColorType.CommonButtonGrey);

            if (!string.IsNullOrEmpty(CSMainPlayerInfo.Instance.GuildName) && CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President && CSGuildInfo.Instance.CanCombinedUnion(brief.unionId))
            {
                btn_rebuild.CustomActive(CSGuildInfo.Instance.CanRebuild()); //(开服时间大于表的天数)
                lb_rebuild.text = CSString.Format(870);// unionBrief.level >CSAvatarManager.MainPlayerInfo.UnionLevel ? "合 并" : "合 并"; //当前家族等级大于主角的家族等级
                btn_rebuild.onClick = OnRebuild;
                btn_rebuild.CustomActive(true);
            }
            else
            {
                btn_rebuild.onClick = null;
                btn_rebuild.CustomActive(false);
            }
        }
        else
        {
            btn_rebuild.onClick = null;
            btn_rebuild.CustomActive(false);
            btn_state.CustomActive(false);

            invoker.InvokeRepeating(0.0f, 0.50f, DeclareTimeInvoke);
        }

        if(null != lb_member)
        {
            TABLE.UNION tbl_union;
            if (UnionTableManager.Instance.TryGetValue(brief.level, out tbl_union))
            {
                bool isOpenAdd = false;
                int addNum = isOpenAdd ? 0 : 0;
                int max = tbl_union.maxPlayers + addNum;
                lb_member.text = $"{brief.size}/{max}";
            }
            else
                lb_member.text = string.Empty;
        }
    }

    protected void OnAgreeMergeType2()
    {
        if (null == this.guildData || null == this.guildData.brief)
            return;
        var brief = guildData.brief;
        Net.CSCombineUnionMessage(1, 2, brief.unionId, brief.name, CSMainPlayerInfo.Instance.GuildId, CSMainPlayerInfo.Instance.GuildName, true);
    }

    protected void OnAgreeMergeType1()
    {
        if (null == this.guildData || null == this.guildData.brief)
            return;
        var brief = guildData.brief;
        Net.CSCombineUnionMessage(1, 1, CSMainPlayerInfo.Instance.GuildId, CSMainPlayerInfo.Instance.GuildName, brief.unionId, brief.name, true);
    }

    private void OnRebuild(GameObject obj)
    {
        if (null == this.guildData || null == this.guildData.brief)
            return;
        var brief = guildData.brief;

        TABLE.UNION tbl_union;
        if (UnionTableManager.Instance.TryGetValue(brief.level, out tbl_union))
        {
            if (brief.size > tbl_union.maxPlayers)
            {
                UtilityTips.ShowRedTips(871);
                return;
            }
        }
        if (brief.level > CSMainPlayerInfo.Instance.GuildLevel)
        {
            UtilityTips.ShowPromptWordTips(39, OnAgreeMergeType2, brief.name);
        }
        else
        {
            UtilityTips.ShowPromptWordTips(37, OnAgreeMergeType1, brief.name);
        }
    }


    public override void OnDestroy()
    {
        lb_name = null;
        lb_chairman = null;
        lb_grade = null;
        lb_member = null;
        lb_state = null;
        lb_time = null;
        btn_state = null;
        spr_state = null;
        sprite_bg = null;
        lb_select = null;
        btn_rebuild.onClick = null;
        btn_rebuild = null;
        lb_rebuild = null;
        btn_war = null;
        if(null != invoker)
        {
            invoker.CancelInvoke();
            invoker = null;
        }
    }
}