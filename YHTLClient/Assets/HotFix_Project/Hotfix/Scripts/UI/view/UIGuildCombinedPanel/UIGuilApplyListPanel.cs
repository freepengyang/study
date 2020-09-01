using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGuilApplyListPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();

        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        CSGuildInfo.Instance.Tab = UnionTab.UnionApplyInfos;
        mapply_settings_options.onClick = OnClickStatus;
        mbtn_allagree.onClick = OnOneKeyAgree;
        mbtn_alldisagree.onClick = OnOneKeyOppose;
    }

    void OnGuildTabDataChanged(uint id,object argv)
    {
        if(argv is UnionTab tab && tab == UnionTab.UnionApplyInfos)
        {
            RefreshApplyList();
        }
    }

    public override void Show()
    {
        base.Show();

        Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
        RefreshApplyList();
    }

    protected void RefreshApplyList()
    {
        var applyInfos = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionApplyInfos);
        if (null != applyInfos)
        {
            mGrildList.MaxCount = applyInfos.applies.Count;
            InitApplyOptions(applyInfos.autoJionLevel);
            mapply_settings_options.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President || CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.VicePresident);

            for (int i = 0; i < mGrildList.MaxCount; i++)
            {
                var info = applyInfos.applies[i];

                var gp = mGrildList.controlList[i].gameObject;

                UILabel lb_name = NGUITools.FindGameObject<UILabel>("lb_name", gp);
                UILabel lb_career = NGUITools.FindGameObject<UILabel>("lb_career", gp);
                UILabel lb_level = NGUITools.FindGameObject<UILabel>("lb_level", gp);
                UILabel lb_time = NGUITools.FindGameObject<UILabel>("lb_time", gp);
                UISprite sp_bg = NGUITools.FindGameObject<UISprite>("bg", gp);

                var lb_agree = gp.transform.Find("lb_agree").gameObject;
                var lb_oppose = gp.transform.Find("lb_oppose").gameObject;

                UIEventListener.Get(lb_agree, info.roleId).onClick = OnAgreeOrIgnore;
                UIEventListener.Get(lb_oppose, info.roleId).onClick = OnAgreeOrIgnore;

                lb_name.text = info.name;
                lb_career.text = Utility.GetCareerName(info.career);
                lb_level.text = info.level.ToString();
                DateTime dt = CSServerTime.StampToDateTime(info.applyTime);
                lb_time.text = $"{dt.Year}.{dt.Month}.{dt.Day}";
                if (null != sp_bg)
                    sp_bg.spriteName = i % 2 == 0 ? "list_subbg1" : "list_subbg2";
            }
        }
    }

    /// <summary>
    /// 一键同意
    /// </summary>
    /// <param name="gp"></param>
    void OnOneKeyAgree(GameObject gp = null)
    {
        var roles = mPoolHandleManager.GetSystemClass<RepeatedField<long>>();
        roles.Clear();

        var applyInfos = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionApplyInfos);
        if(null != applyInfos)
        {
            for (int i = 0; i < applyInfos.applies.Count; i++)
                roles.Add(applyInfos.applies[i].roleId);
            applyInfos.applies.Clear();
        }

        Net.CSConfirmUnionApplyMessage(roles, true);
        roles.Clear();
        mPoolHandleManager.Recycle(roles);

        Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
    }

    /// <summary>
    /// 一键反对
    /// </summary>
    /// <param name="gp"></param>
    private void OnOneKeyOppose(GameObject gp = null)
    {
        var roles = mPoolHandleManager.GetSystemClass<RepeatedField<long>>();
        roles.Clear();

        var applyInfos = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionApplyInfos);
        if (null != applyInfos)
        {
            for (int i = 0; i < applyInfos.applies.Count; i++)
                roles.Add(applyInfos.applies[i].roleId);
            applyInfos.applies.Clear();
        }

        Net.CSConfirmUnionApplyMessage(roles, false);
        roles.Clear();
        mPoolHandleManager.Recycle(roles);

        Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
    }

    void InitApplyOptions(int autoJionLevel)
    {
        int sundryId = 439;
        TABLE.SUNDRY sundryItem;
        if(!SundryTableManager.Instance.TryGetValue(sundryId,out sundryItem))
        {
            return;
        }
        string[] options = sundryItem.effect.Split('#');
        if (Array.IndexOf(options, autoJionLevel.ToString()) == -1)
        {
            mapply_settings_text.text = $"[969696]{CSString.Format(886)}[-]";
        }
        else
        {
            mapply_settings_text.text = string.Format("[969696]≥{0}[-]", autoJionLevel);
        }
        mapply_settings_grid.MaxCount = options.Length + 1;
        for (int i = 0; i < options.Length; i++)
        {
            InitApplyItemInfo(mapply_settings_grid.controlList[i], options[i]);
        }
        InitApplyItemInfo(mapply_settings_grid.controlList[options.Length], "0", false);
    }

    string GetCloseLvString(int functionId = 8)
    {
        TABLE.FUNCOPEN funcItem = null;
        if(FuncOpenTableManager.Instance.TryGetValue(functionId,out funcItem))
        {
            return funcItem.needLevel.ToString();
        }
        return "45";
    }

    void InitApplyItemInfo(GameObject item, string level, bool islevel = true)
    {
        UILabel label = item.transform.Find("Text").GetComponent<UILabel>();
        GameObject select = item.transform.Find("Select").gameObject;
        bool choiced = label.text.Trim().Equals(mapply_settings_text.text.Trim());
        if(!choiced)
        {
            if (islevel)
            {
                label.text = string.Format("[969696]≥{0}[-]", level);
                item.name = level;
            }
            else
            {
                label.text = $"[969696]{CSString.Format(886)}[-]";
                item.name = GetCloseLvString();
            }
        }
        else
        {
            if (islevel)
            {
                label.text = string.Format("[c7c7c7]≥{0}[-]", level);
                item.name = level;
            }
            else
            {
                label.text = $"[c7c7c7]{CSString.Format(886)}[-]";
                item.name = GetCloseLvString();
            }
        }
        select.SetActive(choiced);
        UIEventListener.Get(item).onClick = OnChangeStatus;
    }

    bool onchoose = false;
    private void OnClickStatus(GameObject gobj)
    {
        onChange(onchoose);
        mapply_settings_grid.CustomActive(onchoose);
    }

    private void OnChangeStatus(GameObject btn)
    {
        int lv = 0;
        if (!int.TryParse(btn.name, out lv))
            return;
        onChange(true);
        mapply_settings_grid.CustomActive(false);
        Net.CSSetAutoJoinLevelMessage(lv);
    }

    private void onChange(bool choose)
    {
        onchoose = !onchoose;
        mapply_settings_arrow.transform.localEulerAngles = new Vector3(0, 0, onchoose ? 180 : 0);
        mapply_settings_grid.CustomActive(onchoose);
    }

    /// <summary>
    /// 同意或者忽略
    /// </summary>
    private void OnAgreeOrIgnore(GameObject gp)
    {
        long roleId = (long)UIEventListener.Get(gp).parameter;

        if (gp.name.Equals("lb_oppose"))
        {
            Net.CSConfirmUnionApplyMessage(roleId.ToGoogleList(), false);
        }
        else if (gp.name.Equals("lb_agree"))
        {
            Net.CSConfirmUnionApplyMessage(roleId.ToGoogleList(), true);
        }

        var applyInfos = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionApplyInfos);
        if (null != applyInfos)
        {
            union.UnionApplyInfo info = null;
            for (int i = 0; i < applyInfos.applies.Count; i++)
            {
                if (applyInfos.applies[i].roleId.Equals(roleId))
                {
                    info = applyInfos.applies[i];
                    break;
                }
            }
            if (info != null)
            {
                applyInfos.applies.Remove(info);
            }
        }

        Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnGuildInfoChanged, OnGuildTabDataChanged);
    }
}