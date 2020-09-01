public partial class UIServerActivityDownloadPanel : UIBasePanel
{
	protected UIEventListener mbtn_get;
	protected UISlider mslider_progress;
	protected UILabel mlb_progress;
	protected UIGridContainer mGrid_rewards;
	protected UnityEngine.GameObject mbanner17;
	protected UnityEngine.GameObject mobj_redpoint;
	protected override void _InitScriptBinder()
	{
		mbtn_get = ScriptBinder.GetObject("btn_get") as UIEventListener;
		mslider_progress = ScriptBinder.GetObject("slider_progress") as UISlider;
		mlb_progress = ScriptBinder.GetObject("lb_progress") as UILabel;
		mGrid_rewards = ScriptBinder.GetObject("Grid_rewards") as UIGridContainer;
		mbanner17 = ScriptBinder.GetObject("banner17") as UnityEngine.GameObject;
		mobj_redpoint = ScriptBinder.GetObject("obj_redpoint") as UnityEngine.GameObject;
	}
}
