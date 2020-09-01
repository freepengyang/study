public partial class UIUpcomingActivitiesPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_arrow;
	protected UIGridContainer mgrid_Acs;
	protected UnityEngine.Transform mobj_arrow;
	protected UnityEngine.GameObject mobj_redpoint;
	protected UILabel mlb_count;
	protected override void _InitScriptBinder()
	{
		mbtn_arrow = ScriptBinder.GetObject("btn_arrow") as UnityEngine.GameObject;
		mgrid_Acs = ScriptBinder.GetObject("grid_Acs") as UIGridContainer;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.Transform;
		mobj_redpoint = ScriptBinder.GetObject("obj_redpoint") as UnityEngine.GameObject;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
	}
}
