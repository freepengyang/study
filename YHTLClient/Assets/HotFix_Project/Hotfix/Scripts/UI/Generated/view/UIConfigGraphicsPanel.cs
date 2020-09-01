public partial class UIConfigGraphicsPanel : UIBasePanel
{
	protected UIToggle mtg_PopupTips;
	protected UIToggle mtg_DefaultMode;
	protected UIToggle mtg_FluencyMode;
	protected UIToggle mtg_SpeedMode;
	protected UIToggle mtg_HidePlayers;
	protected UIToggle mtg_HideOwnPlayers;
	protected UIToggle mtg_HideMonsters;
	protected UIToggle mtg_HideAllSkillEffects;
	protected UnityEngine.GameObject mobj_config1;
	protected UnityEngine.GameObject mobj_config2;
	protected UnityEngine.GameObject mobj_config3;
	protected UIToggle mtg_HidePet;
	protected UIToggle mtg_HideWarPet;
	protected UIToggle mtg_HideAllName;
	protected override void _InitScriptBinder()
	{
		mtg_PopupTips = ScriptBinder.GetObject("tg_PopupTips") as UIToggle;
		mtg_DefaultMode = ScriptBinder.GetObject("tg_DefaultMode") as UIToggle;
		mtg_FluencyMode = ScriptBinder.GetObject("tg_FluencyMode") as UIToggle;
		mtg_SpeedMode = ScriptBinder.GetObject("tg_SpeedMode") as UIToggle;
		mtg_HidePlayers = ScriptBinder.GetObject("tg_HidePlayers") as UIToggle;
		mtg_HideOwnPlayers = ScriptBinder.GetObject("tg_HideOwnPlayers") as UIToggle;
		mtg_HideMonsters = ScriptBinder.GetObject("tg_HideMonsters") as UIToggle;
		mtg_HideAllSkillEffects = ScriptBinder.GetObject("tg_HideAllSkillEffects") as UIToggle;
		mobj_config1 = ScriptBinder.GetObject("obj_config1") as UnityEngine.GameObject;
		mobj_config2 = ScriptBinder.GetObject("obj_config2") as UnityEngine.GameObject;
		mobj_config3 = ScriptBinder.GetObject("obj_config3") as UnityEngine.GameObject;
		mtg_HidePet = ScriptBinder.GetObject("tg_HidePet") as UIToggle;
		mtg_HideWarPet = ScriptBinder.GetObject("tg_HideWarPet") as UIToggle;
		mtg_HideAllName = ScriptBinder.GetObject("tg_HideAllName") as UIToggle;
	}
}
