public partial class UIWingCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mtog_wing;
	protected UIToggle mtog_color;
	protected ScriptBinder mUIWingPanel;
	protected ScriptBinder mUIWingColorPanel;
	protected UnityEngine.GameObject mred_wing;
	protected UnityEngine.GameObject mred_color;
	protected UIToggle mtog_wingspirit;
	protected UnityEngine.GameObject mred_wingspirit;
	protected ScriptBinder mUIWingSpiritPanel;
	protected UIGrid mgrid_Group;
	protected UnityEngine.GameObject mbtn_wingspirit;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtog_wing = ScriptBinder.GetObject("tog_wing") as UIToggle;
		mtog_color = ScriptBinder.GetObject("tog_color") as UIToggle;
		mUIWingPanel = ScriptBinder.GetObject("UIWingPanel") as ScriptBinder;
		mUIWingColorPanel = ScriptBinder.GetObject("UIWingColorPanel") as ScriptBinder;
		mred_wing = ScriptBinder.GetObject("red_wing") as UnityEngine.GameObject;
		mred_color = ScriptBinder.GetObject("red_color") as UnityEngine.GameObject;
		mtog_wingspirit = ScriptBinder.GetObject("tog_wingspirit") as UIToggle;
		mred_wingspirit = ScriptBinder.GetObject("red_wingspirit") as UnityEngine.GameObject;
		mUIWingSpiritPanel = ScriptBinder.GetObject("UIWingSpiritPanel") as ScriptBinder;
		mgrid_Group = ScriptBinder.GetObject("grid_Group") as UIGrid;
		mbtn_wingspirit = ScriptBinder.GetObject("btn_wingspirit") as UnityEngine.GameObject;
	}
}
