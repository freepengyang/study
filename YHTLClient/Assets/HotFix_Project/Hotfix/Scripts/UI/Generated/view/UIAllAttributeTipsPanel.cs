public partial class UIAllAttributeTipsPanel : UIBasePanel
{
	protected UILabel mlb_title;
	protected UIGridContainer mgrid_attributes;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_bg;
	protected UnityEngine.GameObject mobj_nontips;
	protected UnityEngine.GameObject mfix;
	protected override void _InitScriptBinder()
	{
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mgrid_attributes = ScriptBinder.GetObject("grid_attributes") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mobj_nontips = ScriptBinder.GetObject("obj_nontips") as UnityEngine.GameObject;
		mfix = ScriptBinder.GetObject("fix") as UnityEngine.GameObject;
	}
}
