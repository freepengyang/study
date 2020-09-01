public partial class UIGuildCombatPanel : UIBasePanel
{
	protected UIEventListener mbtn_go;
	protected UILabel mlb_time;
	protected UILabel mlb_openTimer;
	protected UIScrollView mscroll;
	protected UnityEngine.GameObject msp_scroll;
	protected UILabel mlb_activityEntrance;
	protected UIGridContainer mGrid_rewards;
	protected UnityEngine.GameObject mtx_bg;
	protected override void _InitScriptBinder()
	{
		mbtn_go = ScriptBinder.GetObject("btn_go") as UIEventListener;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_openTimer = ScriptBinder.GetObject("lb_openTimer") as UILabel;
		mscroll = ScriptBinder.GetObject("scroll") as UIScrollView;
		msp_scroll = ScriptBinder.GetObject("sp_scroll") as UnityEngine.GameObject;
		mlb_activityEntrance = ScriptBinder.GetObject("lb_activityEntrance") as UILabel;
		mGrid_rewards = ScriptBinder.GetObject("Grid_rewards") as UIGridContainer;
		mtx_bg = ScriptBinder.GetObject("tx_bg") as UnityEngine.GameObject;
	}
}
