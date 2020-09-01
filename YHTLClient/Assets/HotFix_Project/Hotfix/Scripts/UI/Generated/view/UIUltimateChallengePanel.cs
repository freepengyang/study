public partial class UIUltimateChallengePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UIGrid mToggleGroup;
	protected ScriptBinder mUIChallengePanel;
	protected UIToggle mto_challenge;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mUIChallengePanel = ScriptBinder.GetObject("UIChallengePanel") as ScriptBinder;
		mto_challenge = ScriptBinder.GetObject("to_challenge") as UIToggle;
	}
}
