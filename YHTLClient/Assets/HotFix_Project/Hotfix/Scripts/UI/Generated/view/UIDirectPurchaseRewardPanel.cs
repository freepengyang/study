public partial class UIDirectPurchaseRewardPanel : UIBasePanel
{
	protected UIGridContainer mgrid_reward;
	protected UIEventListener mbtn_close;
	protected UIScrollView mScrollView;
	protected override void _InitScriptBinder()
	{
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
	}
}
