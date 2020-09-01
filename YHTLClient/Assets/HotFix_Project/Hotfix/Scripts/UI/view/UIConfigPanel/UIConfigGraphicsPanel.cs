using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIConfigGraphicsPanel : UIBasePanel
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


        CSEffectPlayMgr.Instance.ShowUITexture(mobj_config1, "config1");
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_config2, "config2");
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_config3, "config3");

        AddToggleEvent(mtg_PopupTips, ConfigOption.PopGraphicsModeTips);
        AddToggleEvent(mtg_HidePlayers, ConfigOption.HideAllPlayers);
        AddToggleEvent(mtg_HideOwnPlayers, ConfigOption.HideMyGuildPlayers);
        AddToggleEvent(mtg_HideMonsters, ConfigOption.HideMonsters);
        AddToggleEvent(mtg_HideAllSkillEffects, ConfigOption.HideSkillEffect);

        AddToggleEvent(mtg_HidePet, ConfigOption.HideTaoistMonster);
        AddToggleEvent(mtg_HideWarPet, ConfigOption.HideWarPet);
        AddToggleEvent(mtg_HideAllName, ConfigOption.HideAllName);

        UIEventListener.Get(mtg_DefaultMode.gameObject, 0).onClick = SwitchModeClick;
        UIEventListener.Get(mtg_FluencyMode.gameObject, 1).onClick = SwitchModeClick;
        UIEventListener.Get(mtg_SpeedMode.gameObject, 2).onClick = SwitchModeClick;
    }

    public override void Show()
    {
        base.Show();

        RefreshUI();
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_config1);
        CSEffectPlayMgr.Instance.Recycle(mobj_config2);
        CSEffectPlayMgr.Instance.Recycle(mobj_config3);
        base.OnDestroy();
    }


    void RefreshUI()
    {
        RefreshToggleUI(mtg_PopupTips, ConfigOption.PopGraphicsModeTips);
        RefreshToggleUI(mtg_HidePlayers, ConfigOption.HideAllPlayers);
        RefreshToggleUI(mtg_HideOwnPlayers, ConfigOption.HideMyGuildPlayers);
        RefreshToggleUI(mtg_HideMonsters, ConfigOption.HideMonsters);
        RefreshToggleUI(mtg_HideAllSkillEffects, ConfigOption.HideSkillEffect);

        RefreshToggleUI(mtg_HidePet, ConfigOption.HideTaoistMonster);
        RefreshToggleUI(mtg_HideWarPet, ConfigOption.HideWarPet);
        RefreshToggleUI(mtg_HideAllName, ConfigOption.HideAllName);

        mtg_DefaultMode.value = mConfigInfo.GetInt(ConfigOption.GraphicsMode) == 0;
        mtg_FluencyMode.value = mConfigInfo.GetInt(ConfigOption.GraphicsMode) == 1;
        mtg_SpeedMode.value = mConfigInfo.GetInt(ConfigOption.GraphicsMode) == 2;
    }


    void AddToggleEvent(UIToggle toggle, ConfigOption configOption)
    {
        EventDelegate.Add(toggle.onChange, () =>
        {
            mConfigInfo.SetBool(configOption, toggle.value);
        });
    }

    void RefreshToggleUI(UIToggle toggle, ConfigOption configOption)
    {
        toggle.value = mConfigInfo.GetBool(configOption);
    }

    void SwitchModeClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        mConfigInfo.SetInt(ConfigOption.GraphicsMode, param);
    }
}
