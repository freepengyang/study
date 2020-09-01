public partial class UIGuildActivityPanel : UIBasePanel
{
	protected UnityEngine.GameObject mUIGuildCombatPanel;
	protected UnityEngine.GameObject mUIGuildBossPanel;
	protected UIToggle mtg_fight;
	protected UIToggle mtg_boss;
	protected UnityEngine.GameObject mbtn_fight;
	protected UnityEngine.GameObject mbtn_boss;
	protected override void _InitScriptBinder()
	{
		mUIGuildCombatPanel = ScriptBinder.GetObject("UIGuildCombatPanel") as UnityEngine.GameObject;
		mUIGuildBossPanel = ScriptBinder.GetObject("UIGuildBossPanel") as UnityEngine.GameObject;
		mtg_fight = ScriptBinder.GetObject("tg_fight") as UIToggle;
		mtg_boss = ScriptBinder.GetObject("tg_boss") as UIToggle;
		mbtn_fight = ScriptBinder.GetObject("btn_fight") as UnityEngine.GameObject;
		mbtn_boss = ScriptBinder.GetObject("btn_boss") as UnityEngine.GameObject;
	}
}
