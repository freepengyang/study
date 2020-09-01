public partial class UIDreamLandPanel : UIBasePanel
{
	protected UnityEngine.GameObject mtexbg;
	protected UILabel mlb_time;
	protected UILabel mlb_condition;
	protected UILabel mlb_activityEntrance;
	protected UIGridContainer mgrid_reward;
	protected UISlider mslider_time;
	protected UILabel mlb_dreamtime;
	protected UIEventListener mbtn_add;
	protected UIEventListener mbtn_go;
	protected UILabel mlb_hint;
	protected UIEventListener mbtn_close;
	protected UILabel mlb_tips;
	protected UIEventListener mbtn_help;
	protected override void _InitScriptBinder()
	{
		mtexbg = ScriptBinder.GetObject("texbg") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_condition = ScriptBinder.GetObject("lb_condition") as UILabel;
		mlb_activityEntrance = ScriptBinder.GetObject("lb_activityEntrance") as UILabel;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGridContainer;
		mslider_time = ScriptBinder.GetObject("slider_time") as UISlider;
		mlb_dreamtime = ScriptBinder.GetObject("lb_dreamtime") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mbtn_go = ScriptBinder.GetObject("btn_go") as UIEventListener;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mlb_tips = ScriptBinder.GetObject("lb_tips") as UILabel;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
	}
}
