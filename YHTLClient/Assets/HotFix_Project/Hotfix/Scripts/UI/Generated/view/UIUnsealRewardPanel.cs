public partial class UIUnsealRewardPanel : UIBasePanel
{
	protected UIGrid mgrid_reward;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_determine;
	protected override void _InitScriptBinder()
	{
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGrid;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_determine = ScriptBinder.GetObject("btn_determine") as UIEventListener;
	}
}
