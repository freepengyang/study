public partial class UIMonthCardGiftPanel : UIBasePanel
{
	protected UITexture msp_texbg;
	protected UIScrollView mscroll_bot;
	protected UIGrid mGrid_top;
	protected UIGrid mGrid_bot;
	protected UILabel mlb_keeps;
	protected UIScrollView mscroll_top;
	protected UnityEngine.GameObject mobj_effect;
	protected override void _InitScriptBinder()
	{
		msp_texbg = ScriptBinder.GetObject("sp_texbg") as UITexture;
		mscroll_bot = ScriptBinder.GetObject("scroll_bot") as UIScrollView;
		mGrid_top = ScriptBinder.GetObject("Grid_top") as UIGrid;
		mGrid_bot = ScriptBinder.GetObject("Grid_bot") as UIGrid;
		mlb_keeps = ScriptBinder.GetObject("lb_keeps") as UILabel;
		mscroll_top = ScriptBinder.GetObject("scroll_top") as UIScrollView;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
	}
}
