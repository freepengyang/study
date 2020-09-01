public partial class UISevenDayTrialPanel : UIBasePanel
{
	protected UIGridBinderContainer mgrid_tab;
	protected UIGridContainer mgrid_task;
	protected UIGridBinderContainer mgrid_reward;
	protected UILabel mlb_time;
	protected UnityEngine.GameObject mbtn_down;
	protected UILabel mlb_point;
	protected UITexture mtex_7days;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mobj_select;
	protected UIScrollView mscrollview_task;
	protected override void _InitScriptBinder()
	{
		mgrid_tab = ScriptBinder.GetObject("grid_tab") as UIGridBinderContainer;
		mgrid_task = ScriptBinder.GetObject("grid_task") as UIGridContainer;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGridBinderContainer;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mbtn_down = ScriptBinder.GetObject("btn_down") as UnityEngine.GameObject;
		mlb_point = ScriptBinder.GetObject("lb_point") as UILabel;
		mtex_7days = ScriptBinder.GetObject("tex_7days") as UITexture;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mobj_select = ScriptBinder.GetObject("obj_select") as UnityEngine.GameObject;
		mscrollview_task = ScriptBinder.GetObject("scrollview_task") as UIScrollView;
	}
}
