public partial class UIRenamePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_confirm;
	protected UIInput mlb_name;
	protected UnityEngine.GameObject mbtn_random;
	protected UnityEngine.GameObject mobj_bg;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_confirm = ScriptBinder.GetObject("btn_confirm") as UnityEngine.GameObject;
		mlb_name = ScriptBinder.GetObject("lb_name") as UIInput;
		mbtn_random = ScriptBinder.GetObject("btn_random") as UnityEngine.GameObject;
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
	}
}
