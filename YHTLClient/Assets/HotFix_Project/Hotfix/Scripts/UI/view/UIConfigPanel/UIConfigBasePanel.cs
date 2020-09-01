using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIConfigBasePanel : UIBasePanel
{
    private CSConfigInfo mConfigInfo;

    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();

        mConfigInfo = CSConfigInfo.Instance;

        UIEventListener.Get(mbtn_resetAll).onClick = RestoreDefaultSettingBtn;
        UIEventListener.Get(mbtn_selectRole).onClick = BackToRoleSelectBtn;
        UIEventListener.Get(mbtn_login).onClick = BackToLoginBtn;

        AddSliderEvent(mslider_bgm, ConfigOption.BgMusicSlider);
        AddSliderEvent(mslider_effect, ConfigOption.EffectSoundSlider);
        AddSliderEvent(mslider_voice, ConfigOption.VoiceSoundSlider);
        AddToggleEvent(mtg_bgm, ConfigOption.BgMusic);
        AddToggleEvent(mtg_effect, ConfigOption.EffectSound);
        AddToggleEvent(mtg_voice, ConfigOption.VoiceSound);

        AddToggleEvent(mtg_fixJoystick, ConfigOption.FixJoystick);
        AddToggleEvent(mtg_pushAct, ConfigOption.PushActivity);
        AddToggleEvent(mtg_forbidSociety, ConfigOption.ForbidGuild);
        AddToggleEvent(mtg_forbidFriend, ConfigOption.ForbidFriend);
        AddToggleEvent(mtg_rejectStranger, ConfigOption.ForbidStranger);
    }

    public override void Show()
    {
        base.Show();

        RefreshUI();
    }


    void RefreshUI()
    {
        mslider_bgm.value = mConfigInfo.GetFloat(ConfigOption.BgMusicSlider) / 100f;
        mslider_effect.value = mConfigInfo.GetFloat(ConfigOption.EffectSoundSlider) / 100f;
        mslider_voice.value = mConfigInfo.GetFloat(ConfigOption.VoiceSoundSlider) / 100f;
        mtg_bgm.value = mConfigInfo.GetBool(ConfigOption.BgMusic);
        mtg_effect.value = mConfigInfo.GetBool(ConfigOption.EffectSound);
        mtg_voice.value = mConfigInfo.GetBool(ConfigOption.VoiceSound);

        mtg_fixJoystick.value = mConfigInfo.GetBool(ConfigOption.FixJoystick);

        mtg_pushAct.value = mConfigInfo.GetBool(ConfigOption.PushActivity);
        mtg_forbidSociety.value = mConfigInfo.GetBool(ConfigOption.ForbidGuild);
        mtg_forbidFriend.value = mConfigInfo.GetBool(ConfigOption.ForbidFriend);
        mtg_rejectStranger.value = mConfigInfo.GetBool(ConfigOption.ForbidStranger);
    }


    void AddToggleEvent(UIToggle toggle, ConfigOption configOption)
    {
        EventDelegate.Add(toggle.onChange, () =>
        {
            mConfigInfo.SetBool(configOption, toggle.value);
        });
    }

    void AddSliderEvent(UISlider slider, ConfigOption configOption)
    {
        EventDelegate.Add(slider.onChange, () =>
        {
            mConfigInfo.SetFloat(configOption, slider.value);
        });
    }


    void RestoreDefaultSettingBtn(GameObject go)
    {
        mConfigInfo.RestoreDefault();
        RefreshUI();
    }


    void BackToRoleSelectBtn(GameObject go)
    {
        //UIManager.Instance.ClosePanel<UIConfigPanel>();
        CSHotNetWork.Instance.OnReturnChooseRole();
    }


    void BackToLoginBtn(GameObject go)
    {
        //UIManager.Instance.ClosePanel<UIConfigPanel>();
        CSHotNetWork.Instance.OnReturn();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
