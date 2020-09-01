public partial class UIServerActivityRankPanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UIGridContainer mgrid_task;
	protected UIGridContainer mgrid_person;
	protected UnityEngine.GameObject mbtn_right;
	protected UnityEngine.GameObject mbtn_left;
	protected UIGridBinderContainer mgrid_rank;
	protected UILabel mlb_rank;
	protected UILabel mlb_point;
	protected UnityEngine.GameObject mbtn_down;
	protected UIScrollView mscrollview_person;
	protected UIScrollView mscrollview_rank;
	protected UITexture mtex_banner9;
	protected UIProgressBar mprogressv;
	protected CSInvoke mCSInvoke;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mgrid_task = ScriptBinder.GetObject("grid_task") as UIGridContainer;
		mgrid_person = ScriptBinder.GetObject("grid_person") as UIGridContainer;
		mbtn_right = ScriptBinder.GetObject("btn_right") as UnityEngine.GameObject;
		mbtn_left = ScriptBinder.GetObject("btn_left") as UnityEngine.GameObject;
		mgrid_rank = ScriptBinder.GetObject("grid_rank") as UIGridBinderContainer;
		mlb_rank = ScriptBinder.GetObject("lb_rank") as UILabel;
		mlb_point = ScriptBinder.GetObject("lb_point") as UILabel;
		mbtn_down = ScriptBinder.GetObject("btn_down") as UnityEngine.GameObject;
		mscrollview_person = ScriptBinder.GetObject("scrollview_person") as UIScrollView;
		mscrollview_rank = ScriptBinder.GetObject("scrollview_rank") as UIScrollView;
		mtex_banner9 = ScriptBinder.GetObject("tex_banner9") as UITexture;
		mprogressv = ScriptBinder.GetObject("progressv") as UIProgressBar;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
	}
}
