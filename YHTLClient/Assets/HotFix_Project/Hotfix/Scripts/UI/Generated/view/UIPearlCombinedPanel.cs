public partial class UIPearlCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mUIPearlUpgradePanel;
	protected UnityEngine.GameObject mUIPearlEvolutionPanel;
	protected UnityEngine.GameObject mUIPearlSkillslotPanel;
	protected UIToggle mtog_upgrade;
	protected UIToggle mtog_evolution;
	protected UIToggle mtog_skillslot;
	protected UnityEngine.GameObject mbtn_skillslot;
	protected UnityEngine.GameObject mred_evolution;
	protected UnityEngine.GameObject mred_skillslot;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mUIPearlUpgradePanel = ScriptBinder.GetObject("UIPearlUpgradePanel") as UnityEngine.GameObject;
		mUIPearlEvolutionPanel = ScriptBinder.GetObject("UIPearlEvolutionPanel") as UnityEngine.GameObject;
		mUIPearlSkillslotPanel = ScriptBinder.GetObject("UIPearlSkillslotPanel") as UnityEngine.GameObject;
		mtog_upgrade = ScriptBinder.GetObject("tog_upgrade") as UIToggle;
		mtog_evolution = ScriptBinder.GetObject("tog_evolution") as UIToggle;
		mtog_skillslot = ScriptBinder.GetObject("tog_skillslot") as UIToggle;
		mbtn_skillslot = ScriptBinder.GetObject("btn_skillslot") as UnityEngine.GameObject;
		mred_evolution = ScriptBinder.GetObject("red_evolution") as UnityEngine.GameObject;
		mred_skillslot = ScriptBinder.GetObject("red_skillslot") as UnityEngine.GameObject;
	}
}
