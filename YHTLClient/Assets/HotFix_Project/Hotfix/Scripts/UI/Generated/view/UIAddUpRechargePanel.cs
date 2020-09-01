public partial class UIAddUpRechargePanel : UIBasePanel
{
	protected UIGridContainer mgrid_days;
	protected UILabel mlb_day;
	protected UILabel mlb_hint;
	protected UITexture mbanner18;
	protected UIScrollBar mscollbar;
	protected UIScrollView mscrollview_days;
	protected UnityEngine.GameObject mbtn_recharge;
	protected UIGridContainer mButtonGroup;
	protected UnityEngine.GameObject mbtn_Tips;
	protected override void _InitScriptBinder()
	{
		mgrid_days = ScriptBinder.GetObject("grid_days") as UIGridContainer;
		mlb_day = ScriptBinder.GetObject("lb_day") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mbanner18 = ScriptBinder.GetObject("banner18") as UITexture;
		mscollbar = ScriptBinder.GetObject("scollbar") as UIScrollBar;
		mscrollview_days = ScriptBinder.GetObject("scrollview_days") as UIScrollView;
		mbtn_recharge = ScriptBinder.GetObject("btn_recharge") as UnityEngine.GameObject;
		mButtonGroup = ScriptBinder.GetObject("ButtonGroup") as UIGridContainer;
		mbtn_Tips = ScriptBinder.GetObject("btn_Tips") as UnityEngine.GameObject;
	}
}
