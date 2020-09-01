public partial class UISpringPaoDianPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_bg;
	protected UnityEngine.GameObject mlb_level;
	protected UnityEngine.GameObject mlb_time;
	protected UnityEngine.GameObject mlb_des;
	protected UIGrid mgrid_rewards;
	protected UnityEngine.GameObject mbtn_enter;
	protected UILabel mlb_Acstate;
	protected override void _InitScriptBinder()
	{
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mlb_level = ScriptBinder.GetObject("lb_level") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UnityEngine.GameObject;
		mlb_des = ScriptBinder.GetObject("lb_des") as UnityEngine.GameObject;
		mgrid_rewards = ScriptBinder.GetObject("grid_rewards") as UIGrid;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mlb_Acstate = ScriptBinder.GetObject("lb_Acstate") as UILabel;
	}
}
