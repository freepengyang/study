public partial class UIAgreementPanel : UIBasePanel
{
	protected UILabel mlb_content;
	protected UIEventListener mbtn_know;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_hook;
	protected UnityEngine.GameObject mobj_hook;
	protected override void _InitScriptBinder()
	{
		mlb_content = ScriptBinder.GetObject("lb_content") as UILabel;
		mbtn_know = ScriptBinder.GetObject("btn_know") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_hook = ScriptBinder.GetObject("btn_hook") as UIEventListener;
		mobj_hook = ScriptBinder.GetObject("obj_hook") as UnityEngine.GameObject;
	}
}
