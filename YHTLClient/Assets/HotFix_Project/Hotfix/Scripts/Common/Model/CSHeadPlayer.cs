using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHeadPlayer : CSHead
{
    private CSLabel _lb_guild_name;

    public CSLabel lb_guild_name
    {
        get
        {
            return _lb_guild_name ?? (_lb_guild_name = UtilityObj.GetOrAdd<CSLabel>(transform, "root/lb_guild_name"));
        }
    }

    private CSFillSprite _spr_ep;

    public CSFillSprite spr_ep
    {
        get { return _spr_ep ?? (_spr_ep = UtilityObj.GetOrAdd<CSFillSprite>(transform, "root/obj_hp/spr_ep")); }
    }

    private CSLabel _lb_dilaoskill;

    public CSLabel lb_dilaoskill
    {
        get
        {
            return _lb_dilaoskill ?? (_lb_dilaoskill = UtilityObj.GetOrAdd<CSLabel>(transform, "root/lb_dilaoskill"));
        }
    }

    /// <summary>
    /// 时装称号
    /// </summary>
    private Transform _sp_title;

    private Transform sp_title
    {
        get { return _sp_title ?? (_sp_title = UtilityObj.GetObject<Transform>(transform, "root/sp_title")); }
    }

    /// <summary>
    /// 玩家头顶名称得位置
    /// </summary>
    private Vector3 namePos;

    public override void Init(CSAvatar avatar, bool isVisible = true,bool isHideModel = false, bool isHideAllName = false)
    {
        base.Init(avatar, isVisible, isHideModel, isHideAllName);

        if (list_lbs == null)
            list_lbs = new List<CSLabel>();

        //添加需要管理的节点
        SetListV(lb_guild_name);
        SetListV(lb_dilaoskill);
        //设置尺寸
        lb_guild_name.fontSize = 16;
        lb_dilaoskill.fontSize = 16;

        namePos = lb_actorName.transform.localPosition;
        if (effectTitle!=null)
        {
            CSSceneEffectMgr.Instance.Destroy(effectTitle);
            effectTitle = null;
        }
        RefreshPlayerName();
        // if (avatar.ID != CSMainPlayerInfo.Instance.GuildId)
        // {
        //     SetHeadActive(!CSConfigInfo.Instance.GetBool(ConfigOption.HideAllName));
        // }
        // else
        // {
        //     SetGuildNameActive(true);
        // }

        if (avatar.BaseInfo is CSPlayerInfo playerInfo)
        {
            SetPosition(playerInfo.HasShield ? 4 : 0);
            SetShieldAttr(playerInfo.HasShield, playerInfo.Shield * 1f / playerInfo.MaxShield);
        }

        var eventHandler = avatar.BaseInfo.EventHandler;
        eventHandler.AddEvent(CEvent.Player_TitleChange, OnPlayerTitleChanged);
        eventHandler.AddEvent(CEvent.player_GuildNameChange, OnPlayerGuildNameChanged);
        eventHandler.AddEvent(CEvent.player_NameChange, OnPlayerNameChagned);
        eventHandler.AddEvent(CEvent.player_GuildIdChange, OnPlayerNameChagned);
        eventHandler.AddEvent(CEvent.player_pkValueChange, OnPlayerNameChagned);
        eventHandler.AddEvent(CEvent.player_greyStateChange, OnPlayerNameChagned);
        eventHandler.AddEvent(CEvent.ActiveShield, OnActiveShield);
        HotManager.Instance.EventHandler.AddEvent(CEvent.OnGuildFightStateChanged, OnGuildFightStateChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.Scene_EnterSceneAfter, OnGuildFightStateChanged);
        eventHandler.AddEvent(CEvent.ChangeHeadSkillInfo, OnSetDiLaoSkill);
        //HotManager.Instance.EventHandler.AddEvent();
    }

    public override void Destroy()
    {
        var eventHandler = mAvatar.BaseInfo.EventHandler;
        eventHandler.RemoveEvent(CEvent.Player_TitleChange, OnPlayerTitleChanged);
        eventHandler.RemoveEvent(CEvent.player_GuildNameChange, OnPlayerGuildNameChanged);
        eventHandler.RemoveEvent(CEvent.player_NameChange, OnPlayerNameChagned);
        eventHandler.RemoveEvent(CEvent.player_GuildIdChange, OnPlayerNameChagned);
        eventHandler.RemoveEvent(CEvent.player_pkValueChange, OnPlayerNameChagned);
        eventHandler.RemoveEvent(CEvent.player_greyStateChange, OnPlayerNameChagned);
        eventHandler.RemoveEvent(CEvent.ActiveShield, OnActiveShield);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OnGuildFightStateChanged, OnGuildFightStateChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.Scene_EnterSceneAfter, OnGuildFightStateChanged);
        eventHandler.RemoveEvent(CEvent.ChangeHeadSkillInfo, OnSetDiLaoSkill);
        list_lbs.Clear();
        base.Destroy();
    }

    public override void Release()
    {
        base.Release();
    }

    public override void Show()
    {
        base.Show();
    }

    /// <summary>
    /// 激活护盾设置高度
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void OnActiveShield(uint id, object data)
    {
        SetPosition(4);
    }

    protected void OnPlayerNameChagned(uint id, object argv)
    {
        if (mAvatar.BaseInfo is CSPlayerInfo)
        {
            RefreshPlayerName();
        }
    }

    protected void OnPlayerGuildNameChanged(uint id, object argv)
    {
        if (mAvatar.BaseInfo is CSPlayerInfo)
        {
            RefreshPlayerName();
        }
    }

    protected void OnPlayerTitleChanged(uint id, object argv)
    {
        int titleId = (int) argv;
        if (titleId <= 0)
        {
            CSSceneEffectMgr.Instance.Destroy(effectTitle);
            effectTitle = null;
        }
        else
        {
            if (mAvatar.BaseInfo is CSPlayerInfo)
            {
                SetFashionTitle(true);
                SetListPos();
            }
        }
    }

    protected void OnGuildFightStateChanged(uint id, object argv)
    {
        if (mAvatar.BaseInfo is CSPlayerInfo)
        {
            RefreshPlayerName();
        }
    }

    protected virtual void SetGuildName()
    {
        if (null == lb_guild_name)
            return;

        if (!(mAvatar.BaseInfo is CSPlayerInfo playerInfo) || playerInfo.GuildId == 0)
        {
            lb_guild_name.text = string.Empty;
            return;
        }

        if (Utility.IsInMap(ESpecialMap.DiXiaXunBao))
        {
            lb_guild_name.text = string.Empty;
            return;
        }


        //是否正在进行沙城争霸
        bool isFighting = CSGuildFightManager.Instance.IsActivityOpened;
        bool sameGuildWithMainPlayer = playerInfo.GuildId == CSMainPlayerInfo.Instance.GuildId;
        bool isSabakeMember = CSGuildFightManager.Instance.IsSabacMember(playerInfo.GuildId);



        //行会战处理
        if (CSGuildActivityInfo.Instance.IsActivityOpen((int)GuildActivityType.COMBAT) 
            && CSGuildActivityInfo.Instance.IsOnTheInstance((int)GuildActivityType.COMBAT))
        {
            string name = isSabakeMember ? $"{playerInfo.GuildName}{CSString.Format(1046)}" : playerInfo.GuildName;
            if (sameGuildWithMainPlayer)
                lb_guild_name.text = name.BBCode(ColorType.SabacTeam);
            else
                lb_guild_name.text = name.BBCode(ColorType.Orange);

            return;
        }


        //通常情况下
        if (!isFighting)
        {
            if (!isSabakeMember)
            {
                lb_guild_name.text = $"{playerInfo.GuildName}".BBCode(GetPlayerNameColor(playerInfo));
            }
            else
            {
                lb_guild_name.text =
                    $"{playerInfo.GuildName}{CSString.Format(1046)}".BBCode(GetPlayerNameColor(playerInfo));
            }

            return;
        }

        if (!CSGuildFightManager.Instance.IsInActivityMap)
        {
            if (!isSabakeMember)
            {
                lb_guild_name.text = $"{playerInfo.GuildName}".BBCode(GetPlayerNameColor(playerInfo));
            }
            else
            {
                lb_guild_name.text =
                    $"{playerInfo.GuildName}{CSString.Format(1046)}".BBCode(GetPlayerNameColor(playerInfo));
            }

            return;
        }

        if (!isSabakeMember)
        {
            if (sameGuildWithMainPlayer)
                lb_guild_name.text = $"{playerInfo.GuildName}".BBCode(ColorType.SabacTeam);
            else
                lb_guild_name.text = $"{playerInfo.GuildName}".BBCode(ColorType.Orange);
        }
        else
        {
            if (sameGuildWithMainPlayer)
                lb_guild_name.text = $"{playerInfo.GuildName}{CSString.Format(1046)}".BBCode(ColorType.SabacTeam);
            else
                lb_guild_name.text = $"{playerInfo.GuildName}{CSString.Format(1046)}".BBCode(ColorType.Orange);
        }
    }

    protected void OnSetDiLaoSkill(uint id, object argv)
    {
        if (argv is string str)
        {
            lb_dilaoskill.text = str;
            RefreshPlayerName();
        }
    }

    public static ColorType GetPlayerNameColor(CSPlayerInfo playerInfo)
    {
        if (null == playerInfo)
            return ColorType.White;

        if (playerInfo.PkValue >= UtilityFight.RedPlayerPkValue)
            return ColorType.Red;

        if (playerInfo.GreyName)
            return ColorType.WeakText;

        if (playerInfo.PkValue <= 0)
            return ColorType.White;

        return ColorType.Yellow;
    }

    protected void SetGuildNameActive(bool isActive)
    {
        if ((lb_guild_name != null) && (lb_guild_name.gameObject.activeSelf != isActive))
        {
            lb_guild_name.gameObject.SetActive(isActive);
        }
    }

    protected void SetTitleActive(bool isActive)
    {
        if ((sp_title != null) && (sp_title.gameObject.activeSelf != isActive))
        {
            sp_title.gameObject.SetActive(isActive);
        }
    }

    protected override void SetName()
    {
        if (null == lb_actorName)
            return;

        if (!(mAvatar.BaseInfo is CSPlayerInfo playerInfo))
        {
            base.SetName();
            return;
        }

        if (Utility.IsInMap(ESpecialMap.DiXiaXunBao))
        {
            lb_actorName.text = ClientTipsTableManager.Instance.GetClientTipsContext(1712);
            return;
        }

        //是否正在进行沙城争霸
        bool isFighting = CSGuildFightManager.Instance.IsActivityOpened;
        bool sameGuildWithMainPlayer = playerInfo.GuildId == CSMainPlayerInfo.Instance.GuildId;
        bool isSabakeMember = CSGuildFightManager.Instance.IsSabacMember(playerInfo.GuildId);

        //无公会
        if (playerInfo.GuildId == 0)
        {
            if (!isFighting)
                lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            else
            {
                if (CSGuildFightManager.Instance.IsInActivityMap)
                    lb_actorName.text = playerInfo.Name.BBCode(ColorType.Green);
                else
                    lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            }

            return;
        }

        //行会战处理
        if (CSGuildActivityInfo.Instance.IsActivityOpen((int)GuildActivityType.COMBAT)
            && CSGuildActivityInfo.Instance.IsOnTheInstance((int)GuildActivityType.COMBAT))
        {
            if (sameGuildWithMainPlayer)
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.SabacTeam);
            else
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.Orange);

            return;
        }

        //通常情况下
        if (!isFighting)
        {
            if (!isSabakeMember)
            {
                lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            }
            else
            {
                lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            }

            return;
        }

        if (!CSGuildFightManager.Instance.IsInActivityMap)
        {
            if (!isSabakeMember)
            {
                lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            }
            else
            {
                lb_actorName.text = playerInfo.Name.BBCode(GetPlayerNameColor(playerInfo));
            }

            return;
        }

        //行会争霸情况下
        if (!isSabakeMember)
        {
            if (sameGuildWithMainPlayer)
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.SabacTeam);
            else
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.Orange);
        }
        else
        {
            if (sameGuildWithMainPlayer)
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.SabacTeam);
            else
                lb_actorName.text = $"{playerInfo.Name}".BBCode(ColorType.Orange);
        }
    }


    /// <summary>
    /// 设置护盾条
    /// </summary>
    public virtual void SetShieldAttr(bool need_ep, float amount)
    {
        if (need_ep)
        {
            if (null != spr_ep)
                spr_ep.fillAmount = amount;
        }

        spr_ep.CustomActive(need_ep);
    }


    public override void SetHeadActive(bool active)
    {
        base.SetHeadActive(active);
        SetGuildNameActive(active);
        SetTitleActive(active);
    }

    protected void RefreshPlayerName()
    {
        SetName();
        SetGuildName();
        SetFashionTitle();
        SetListPos();
    }


    public CSSceneEffect effectTitle;

    /// <summary>
    /// 设置时装称号
    /// </summary>
    void SetFashionTitle(bool isChange = false)
    {
        if (null == sp_title) return;
        if (mAvatar.BaseInfo is CSPlayerInfo playerInfo)
        {
            if (playerInfo != null && playerInfo.TitleId > 0 )
            {
                if (isChange || (!isChange && effectTitle == null))
                {
                    if (effectTitle!=null)
                    {
                        CSSceneEffectMgr.Instance.Destroy(effectTitle);
                        effectTitle = null;
                    }
                    if (FashionTableManager.Instance.TryGetValue(playerInfo.TitleId, out TABLE.FASHION fashion))
                        effectTitle = CSSceneEffectMgr.Instance.Create(sp_title, fashion.titleModel, Vector3.zero);
                }
            }

            // effectTitle = CSSceneEffectMgr.Instance.Create(sp_title, 17912, Vector3.zero);
        }
    }

    protected void SetListPos()
    {
        if (effectTitle != null)
            sp_title.localPosition = new Vector3(0, 50, 0);
        int index = 1;
        for (int i = 0; i < list_lbs.Count; i++)
        {
            if (!(string.IsNullOrEmpty(list_lbs[i].text)))
            {
                Vector3 vec = namePos + new Vector3(0, index * 22, 0);
                list_lbs[i].transform.localPosition = vec;
                if (effectTitle != null)
                    sp_title.localPosition = vec + new Vector3(0, 35, 0);
                index++;
            }
        }
    }

    static Vector3 curVector3 = new Vector3(0, 102, -100000);

    public override void SetHeadPosition()
    {
        base.SetHeadPosition();
        //Debug.Log("curVector3 :" + curVector3);
        transform.localPosition = curVector3;
    }

    public void SetPosition(float offset)
    {
        if (offset != 0)
            transform.localPosition += Vector3.up * offset;
    }
}