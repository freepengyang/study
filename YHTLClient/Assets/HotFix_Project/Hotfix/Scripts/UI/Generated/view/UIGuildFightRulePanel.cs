public partial class UIGuildFightRulePanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_bg;
	protected UnityEngine.GameObject mlb_level;
	protected UnityEngine.GameObject mlb_time;
	protected UnityEngine.GameObject mlb_des;
	protected UIGridContainer mgrid_rewards;
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject msand_bg;
	protected UnityEngine.GameObject msand_line;
	protected UnityEngine.GameObject msand;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject msandfight_road;
	protected override void _InitScriptBinder()
	{
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mlb_level = ScriptBinder.GetObject("lb_level") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UnityEngine.GameObject;
		mlb_des = ScriptBinder.GetObject("lb_des") as UnityEngine.GameObject;
		mgrid_rewards = ScriptBinder.GetObject("grid_rewards") as UIGridContainer;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		msand_bg = ScriptBinder.GetObject("sand_bg") as UnityEngine.GameObject;
		msand_line = ScriptBinder.GetObject("sand_line") as UnityEngine.GameObject;
		msand = ScriptBinder.GetObject("sand") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		msandfight_road = ScriptBinder.GetObject("sandfight_road") as UnityEngine.GameObject;
	}
}
