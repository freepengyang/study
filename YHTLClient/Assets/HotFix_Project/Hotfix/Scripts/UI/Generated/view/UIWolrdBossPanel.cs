public partial class UIWolrdBossPanel : UIBasePanel
{
	protected UILabel mlb_bossName;
	protected UnityEngine.GameObject mobj_model;
	protected UILabel mlb_time;
	protected UILabel mlb_lv;
	protected UILabel mlb_des;
	protected UIGridContainer mgrid_itemsPar;
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject mlb_remainTime;
	protected UnityEngine.GameObject mobj_arrow;
	protected UnityEngine.GameObject mobj_bg;
	protected UIScrollBar mobj_scrollBar;
	protected UILabel mlb_notOpen;
	protected UILabel mlb_activityEnd;
	protected UnityEngine.GameObject mbtn_help;
	protected UIScrollView mscr_reward;
	protected UIPanel mpanel_reward;
	protected override void _InitScriptBinder()
	{
		mlb_bossName = ScriptBinder.GetObject("lb_bossName") as UILabel;
		mobj_model = ScriptBinder.GetObject("obj_model") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_lv = ScriptBinder.GetObject("lb_lv") as UILabel;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
		mgrid_itemsPar = ScriptBinder.GetObject("grid_itemsPar") as UIGridContainer;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mlb_remainTime = ScriptBinder.GetObject("lb_remainTime") as UnityEngine.GameObject;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mobj_scrollBar = ScriptBinder.GetObject("obj_scrollBar") as UIScrollBar;
		mlb_notOpen = ScriptBinder.GetObject("lb_notOpen") as UILabel;
		mlb_activityEnd = ScriptBinder.GetObject("lb_activityEnd") as UILabel;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mscr_reward = ScriptBinder.GetObject("scr_reward") as UIScrollView;
		mpanel_reward = ScriptBinder.GetObject("panel_reward") as UIPanel;
	}
}
