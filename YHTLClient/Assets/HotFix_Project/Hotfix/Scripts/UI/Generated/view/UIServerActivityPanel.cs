public partial class UIServerActivityPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_bg;
	protected UnityEngine.Transform mtrans_parent;
	protected UIScrollView mscr_tabs;
	protected UIGridContainer mgrid_tabs;
	protected UnityEngine.GameObject mbtn_close;
	protected override void _InitScriptBinder()
	{
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mtrans_parent = ScriptBinder.GetObject("trans_parent") as UnityEngine.Transform;
		mscr_tabs = ScriptBinder.GetObject("scr_tabs") as UIScrollView;
		mgrid_tabs = ScriptBinder.GetObject("grid_tabs") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
	}
}
