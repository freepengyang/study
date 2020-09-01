using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Text;

public class UIChatSettingPanel : UIBasePanel
{
    UIToggle mVoiceToggleWorld;
    UIToggle mVoiceToggleGuild;
    UIToggle mVoiceToggleColorWorld;
    UIToggle mVoiceTogglePrivate;
    UIToggle mVoiceToggleNearby;
    UIToggle mVoiceToggleTeam;
    UIToggle mToggleWorld;
    UIToggle mToggleTeam;
    UIToggle mToggleGuild;
    UIToggle mToggleNearby;
    UIToggle mTogglePrivate;
    UIToggle mToggleVipLevel;
    UIEventListener mBtnClose;
    UIEventListener mBtnCloseExtend;

    protected override void _InitScriptBinder()
    {
        mVoiceToggleWorld = ScriptBinder.GetObject("VoiceToggleWorld") as UIToggle;
        mVoiceToggleGuild = ScriptBinder.GetObject("VoiceToggleGuild") as UIToggle;
        mVoiceToggleColorWorld = ScriptBinder.GetObject("VoiceToggleColorWorld") as UIToggle;
        mVoiceTogglePrivate = ScriptBinder.GetObject("VoiceTogglePrivate") as UIToggle;
        mVoiceToggleNearby = ScriptBinder.GetObject("VoiceToggleNearby") as UIToggle;
        mVoiceToggleTeam = ScriptBinder.GetObject("VoiceToggleTeam") as UIToggle;
        mToggleWorld = ScriptBinder.GetObject("ToggleWorld") as UIToggle;
        mToggleTeam = ScriptBinder.GetObject("ToggleTeam") as UIToggle;
        mToggleGuild = ScriptBinder.GetObject("ToggleGuild") as UIToggle;
        mToggleNearby = ScriptBinder.GetObject("ToggleNearby") as UIToggle;
        mTogglePrivate = ScriptBinder.GetObject("TogglePrivate") as UIToggle;
        mToggleVipLevel = ScriptBinder.GetObject("ToggleVipLevel") as UIToggle;
        mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
        mBtnCloseExtend = ScriptBinder.GetObject("btn_close") as UIEventListener;
    }

    protected void AddToggleEvent(UIToggle toggle,ConfigOption configOption)
    {
        EventDelegate.Add(toggle.onChange, () =>
         {
             OnValueChanged(configOption,toggle.value);
         });
    }

    protected void OnValueChanged(ConfigOption configOption,bool value)
    {
        //Debug.LogFormat("OnValueChanged {0} = {1}", configOption, value);
        CSConfigInfo.Instance.SetBool(configOption, value);
    }

    private void InitVoiceSetting()
    {
        mVoiceToggleGuild.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayGuildAudio);
        mVoiceToggleTeam.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayTeamAudio);
        mVoiceToggleWorld.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayWorldAudio);
        mVoiceTogglePrivate.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayPrivateAudio);
        mVoiceToggleNearby.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayFuJinAudio);
        mVoiceToggleColorWorld.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayColorWorldAudio);

        mToggleWorld.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayWorldText);
        mToggleTeam.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayTeamText);
        mToggleGuild.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayGuildText);
        mToggleNearby.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayNearbyText);
        mTogglePrivate.value = CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayPrivateText);
        mToggleVipLevel.value = CSConfigInfo.Instance.GetBool(ConfigOption.MakeVipLevelVisible);
    }

    public override void Init()
    {
        base.Init();
        
        AddToggleEvent(mVoiceToggleGuild, ConfigOption.AutoPlayGuildAudio);
        AddToggleEvent(mVoiceToggleTeam, ConfigOption.AutoPlayTeamAudio);
        AddToggleEvent(mVoiceToggleWorld, ConfigOption.AutoPlayWorldAudio);
        AddToggleEvent(mVoiceTogglePrivate, ConfigOption.AutoPlayPrivateAudio);
        AddToggleEvent(mVoiceToggleNearby, ConfigOption.AutoPlayFuJinAudio);
        AddToggleEvent(mVoiceToggleColorWorld, ConfigOption.AutoPlayColorWorldAudio);

        AddToggleEvent(mToggleWorld, ConfigOption.AutoPlayWorldText);
        AddToggleEvent(mToggleTeam, ConfigOption.AutoPlayTeamText);
        AddToggleEvent(mToggleGuild, ConfigOption.AutoPlayGuildText);
        AddToggleEvent(mToggleNearby, ConfigOption.AutoPlayNearbyText);
        AddToggleEvent(mTogglePrivate, ConfigOption.AutoPlayPrivateText);
        AddToggleEvent(mToggleVipLevel, ConfigOption.MakeVipLevelVisible);

        mBtnClose.onClick = this.Hide;
        mBtnCloseExtend.onClick = this.Hide;

        InitVoiceSetting();
    }
}