public partial class UIDailySignInAwardPanel : UIBasePanel
{
	protected UILabel mlb_name;
	protected UILabel mlb_cardSlot;
	protected UnityEngine.Transform mtrans_scrollView;
	protected UIGrid mGrid_reward;
	protected UnityEngine.GameObject mobj_arrow;
	protected UIScrollView mscroll_reward;
	protected UISprite msp_bg;
	protected override void _InitScriptBinder()
	{
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_cardSlot = ScriptBinder.GetObject("lb_cardSlot") as UILabel;
		mtrans_scrollView = ScriptBinder.GetObject("trans_scrollView") as UnityEngine.Transform;
		mGrid_reward = ScriptBinder.GetObject("Grid_reward") as UIGrid;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mscroll_reward = ScriptBinder.GetObject("scroll_reward") as UIScrollView;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
	}
}
