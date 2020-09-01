public partial class UIPkModePanel : UIBasePanel
{
	protected UISprite mbtn_showModeList;
	protected UILabel mlb_mode;
	protected UnityEngine.GameObject mobj_lock;
	protected UnityEngine.GameObject mobj_ModeList;
	protected UnityEngine.GameObject mobj_shield;
	protected UIGrid mgrid_modes;
	protected override void _InitScriptBinder()
	{
		mbtn_showModeList = ScriptBinder.GetObject("btn_showModeList") as UISprite;
		mlb_mode = ScriptBinder.GetObject("lb_mode") as UILabel;
		mobj_lock = ScriptBinder.GetObject("obj_lock") as UnityEngine.GameObject;
		mobj_ModeList = ScriptBinder.GetObject("obj_ModeList") as UnityEngine.GameObject;
		mobj_shield = ScriptBinder.GetObject("obj_shield") as UnityEngine.GameObject;
		mgrid_modes = ScriptBinder.GetObject("grid_modes") as UIGrid;
	}
}
