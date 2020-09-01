public partial class UIDungeonPanel : UIBasePanel
{
	protected UITexture mtex_bg;
	protected UILabel mlb_time;
	protected UILabel mlb_condition;
	protected UILabel mlb_activityEntrance;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_go;
	protected UnityEngine.GameObject mgrid_reward;
	protected UISlider mslider_award;
	protected UILabel mlb_remain;
	protected UIGridContainer mgrid_label;
	protected override void _InitScriptBinder()
	{
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UITexture;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_condition = ScriptBinder.GetObject("lb_condition") as UILabel;
		mlb_activityEntrance = ScriptBinder.GetObject("lb_activityEntrance") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_go = ScriptBinder.GetObject("btn_go") as UnityEngine.GameObject;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UnityEngine.GameObject;
		mslider_award = ScriptBinder.GetObject("slider_award") as UISlider;
		mlb_remain = ScriptBinder.GetObject("lb_remain") as UILabel;
		mgrid_label = ScriptBinder.GetObject("grid_label") as UIGridContainer;
	}
}
