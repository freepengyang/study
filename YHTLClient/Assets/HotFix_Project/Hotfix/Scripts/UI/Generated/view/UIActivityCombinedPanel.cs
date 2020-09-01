public partial class UIActivityCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mbtn_daily;
	protected UIToggle mbtn_activity;
	protected UIToggle mbtn_weekly;
	protected UnityEngine.GameObject mobj_dailyActivityPanel;
	protected UnityEngine.GameObject mobj_activityHallPanel;
	protected UnityEngine.GameObject mobj_weeklyCalendarPanel;
	protected UIToggle mbtn_card;
	protected UnityEngine.GameObject mobj_signInPanel;
	protected UIGrid mgrid_tgGroup;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_daily = ScriptBinder.GetObject("btn_daily") as UIToggle;
		mbtn_activity = ScriptBinder.GetObject("btn_activity") as UIToggle;
		mbtn_weekly = ScriptBinder.GetObject("btn_weekly") as UIToggle;
		mobj_dailyActivityPanel = ScriptBinder.GetObject("obj_dailyActivityPanel") as UnityEngine.GameObject;
		mobj_activityHallPanel = ScriptBinder.GetObject("obj_activityHallPanel") as UnityEngine.GameObject;
		mobj_weeklyCalendarPanel = ScriptBinder.GetObject("obj_weeklyCalendarPanel") as UnityEngine.GameObject;
		mbtn_card = ScriptBinder.GetObject("btn_card") as UIToggle;
		mobj_signInPanel = ScriptBinder.GetObject("obj_signInPanel") as UnityEngine.GameObject;
		mgrid_tgGroup = ScriptBinder.GetObject("grid_tgGroup") as UIGrid;
	}
}
