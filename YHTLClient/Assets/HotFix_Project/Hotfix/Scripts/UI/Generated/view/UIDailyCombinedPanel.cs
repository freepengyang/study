public partial class UIDailyCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mtg_signIn;
	protected UnityEngine.GameObject mobj_signInPanel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtg_signIn = ScriptBinder.GetObject("tg_signIn") as UIToggle;
		mobj_signInPanel = ScriptBinder.GetObject("obj_signInPanel") as UnityEngine.GameObject;
	}
}
