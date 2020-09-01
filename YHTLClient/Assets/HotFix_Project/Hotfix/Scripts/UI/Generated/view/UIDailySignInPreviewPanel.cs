public partial class UIDailySignInPreviewPanel : UIBasePanel
{
	protected UIGridContainer mGrid_left;
	protected UISprite msp_icon;
	protected UISlider mslider_reached;
	protected UILabel mlb_slider;
	protected UnityEngine.GameObject mobj_common;
	protected UnityEngine.GameObject mobj_ult;
	protected UIGridContainer mGrid_common;
	protected UIGridContainer mGrid_ult;
	protected UnityEngine.Transform mtrans_select;
	protected UnityEngine.GameObject mobj_giftFx;
	protected UIScrollView mscroll_left;
	protected UIScrollBar mscrollBar;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mobj_boxCheck;
	protected override void _InitScriptBinder()
	{
		mGrid_left = ScriptBinder.GetObject("Grid_left") as UIGridContainer;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mslider_reached = ScriptBinder.GetObject("slider_reached") as UISlider;
		mlb_slider = ScriptBinder.GetObject("lb_slider") as UILabel;
		mobj_common = ScriptBinder.GetObject("obj_common") as UnityEngine.GameObject;
		mobj_ult = ScriptBinder.GetObject("obj_ult") as UnityEngine.GameObject;
		mGrid_common = ScriptBinder.GetObject("Grid_common") as UIGridContainer;
		mGrid_ult = ScriptBinder.GetObject("Grid_ult") as UIGridContainer;
		mtrans_select = ScriptBinder.GetObject("trans_select") as UnityEngine.Transform;
		mobj_giftFx = ScriptBinder.GetObject("obj_giftFx") as UnityEngine.GameObject;
		mscroll_left = ScriptBinder.GetObject("scroll_left") as UIScrollView;
		mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mobj_boxCheck = ScriptBinder.GetObject("obj_boxCheck") as UnityEngine.GameObject;
	}
}
