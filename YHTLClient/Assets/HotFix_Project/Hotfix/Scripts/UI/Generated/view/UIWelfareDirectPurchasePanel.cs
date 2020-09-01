public partial class UIWelfareDirectPurchasePanel : UIBasePanel
{
	protected UnityEngine.GameObject mtx_bg;
	protected UIScrollView mScrollView_gift;
	protected UIGridContainer mgrid_gift;
	protected UIEventListener mbtn_repay;
	protected UnityEngine.GameObject mredpoint_repay;
	protected UnityEngine.GameObject meffect;
	protected UILabel mlb_time;
	protected override void _InitScriptBinder()
	{
		mtx_bg = ScriptBinder.GetObject("tx_bg") as UnityEngine.GameObject;
		mScrollView_gift = ScriptBinder.GetObject("ScrollView_gift") as UIScrollView;
		mgrid_gift = ScriptBinder.GetObject("grid_gift") as UIGridContainer;
		mbtn_repay = ScriptBinder.GetObject("btn_repay") as UIEventListener;
		mredpoint_repay = ScriptBinder.GetObject("redpoint_repay") as UnityEngine.GameObject;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
	}
}
