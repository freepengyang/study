public partial class UIGuildBossPanel : UIBasePanel
{
	protected UIEventListener mbtn_go;
	protected UILabel mlb_activityEntrance;
	protected UILabel mlb_time;
	protected UILabel mlb_guild;
	protected UILabel mlb_openTimer;
	protected UIGridContainer mGrid;
	protected UnityEngine.GameObject mtex_bg;
	protected override void _InitScriptBinder()
	{
		mbtn_go = ScriptBinder.GetObject("btn_go") as UIEventListener;
		mlb_activityEntrance = ScriptBinder.GetObject("lb_activityEntrance") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_guild = ScriptBinder.GetObject("lb_guild") as UILabel;
		mlb_openTimer = ScriptBinder.GetObject("lb_openTimer") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
	}
}
