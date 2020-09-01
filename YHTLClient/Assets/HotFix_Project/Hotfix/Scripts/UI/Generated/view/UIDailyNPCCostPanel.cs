public partial class UIDailyNPCCostPanel : UIBasePanel
{
	protected UILabel mlb_title;
	protected UILabel mlb_say;
	protected UIEventListener mbtnClose;
	protected UIEventListener mbtn_task;
	protected UILabel mlb_count;
	protected UnityEngine.Transform mcostroot;
	protected override void _InitScriptBinder()
	{
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mlb_say = ScriptBinder.GetObject("lb_say") as UILabel;
		mbtnClose = ScriptBinder.GetObject("btnClose") as UIEventListener;
		mbtn_task = ScriptBinder.GetObject("btn_task") as UIEventListener;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mcostroot = ScriptBinder.GetObject("costroot") as UnityEngine.Transform;
	}
}
