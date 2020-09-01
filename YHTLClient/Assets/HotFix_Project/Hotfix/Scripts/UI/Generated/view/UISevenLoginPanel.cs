public partial class UISevenLoginPanel : UIBasePanel
{
	protected UITable mgrid_hints;
	protected UIScrollView mScrollView;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_extract;
	protected UnityEngine.Transform mgrid_award_parent;
	protected UnityEngine.Transform mgrid_day_labels;
	protected UnityEngine.Transform mchild;
	protected UILabel mlb_status;
	protected UILabel mlb_theme;
	protected UnityEngine.Transform mgrid_hints_transform;
	protected UnityEngine.GameObject mgo_can_acquired;
	protected UnityEngine.GameObject mbanner26;
	protected override void _InitScriptBinder()
	{
		mgrid_hints = ScriptBinder.GetObject("grid_hints") as UITable;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_extract = ScriptBinder.GetObject("btn_extract") as UIEventListener;
		mgrid_award_parent = ScriptBinder.GetObject("grid_award_parent") as UnityEngine.Transform;
		mgrid_day_labels = ScriptBinder.GetObject("grid_day_labels") as UnityEngine.Transform;
		mchild = ScriptBinder.GetObject("child") as UnityEngine.Transform;
		mlb_status = ScriptBinder.GetObject("lb_status") as UILabel;
		mlb_theme = ScriptBinder.GetObject("lb_theme") as UILabel;
		mgrid_hints_transform = ScriptBinder.GetObject("grid_hints_transform") as UnityEngine.Transform;
		mgo_can_acquired = ScriptBinder.GetObject("go_can_acquired") as UnityEngine.GameObject;
		mbanner26 = ScriptBinder.GetObject("banner26") as UnityEngine.GameObject;
	}
}
