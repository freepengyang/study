public partial class UICheckInfoCombinePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mtog_role;
	protected UIToggle mtog_wolong;
	protected UnityEngine.GameObject mUICheckAttrPanel;
	protected UnityEngine.GameObject mUICheckDragonPanel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtog_role = ScriptBinder.GetObject("tog_role") as UIToggle;
		mtog_wolong = ScriptBinder.GetObject("tog_wolong") as UIToggle;
		mUICheckAttrPanel = ScriptBinder.GetObject("UICheckAttrPanel") as UnityEngine.GameObject;
		mUICheckDragonPanel = ScriptBinder.GetObject("UICheckDragonPanel") as UnityEngine.GameObject;
	}
}
