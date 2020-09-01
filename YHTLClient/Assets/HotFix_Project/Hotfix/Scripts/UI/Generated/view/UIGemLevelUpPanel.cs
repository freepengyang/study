public partial class UIGemLevelUpPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_send;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mobj_gem1;
	protected UnityEngine.GameObject mobj_gem2;
	protected UIGridContainer mgrid_glist;
	protected UIGridContainer mgird_items;
	protected UnityEngine.GameObject mobj_effect;
	protected UILabel mlb_bagnum;
	protected override void _InitScriptBinder()
	{
		mbtn_send = ScriptBinder.GetObject("btn_send") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mobj_gem1 = ScriptBinder.GetObject("obj_gem1") as UnityEngine.GameObject;
		mobj_gem2 = ScriptBinder.GetObject("obj_gem2") as UnityEngine.GameObject;
		mgrid_glist = ScriptBinder.GetObject("grid_glist") as UIGridContainer;
		mgird_items = ScriptBinder.GetObject("gird_items") as UIGridContainer;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
		mlb_bagnum = ScriptBinder.GetObject("lb_bagnum") as UILabel;
	}
}
