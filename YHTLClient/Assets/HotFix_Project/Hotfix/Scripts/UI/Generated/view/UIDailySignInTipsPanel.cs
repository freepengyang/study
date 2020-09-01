public partial class UIDailySignInTipsPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_view2;
	protected UIEventListener mbtn_award;
	protected UILabel mlb_desc;
	protected UILabel mlb_name;
	protected UIEventListener mbtn_lock;
	protected UISprite msp_icon;
	protected UIGridContainer mGrid_leftCards;
	protected UILabel mlb_hintRight;
	protected UILabel mlb_lock;
	protected UnityEngine.GameObject mbtn_scroll;
	protected UISprite msp_awardBtn;
	protected UnityEngine.Transform mtrans_rightScroll;
	protected UISprite msp_rightBg;
	protected UIScrollBar mscrollBar;
	protected UIScrollView mscroll_left;
	protected UILabel mlb_awardBtn;
	protected UIWrapContent mwrap_right;
	protected UnityEngine.GameObject mobj_curlock;
	protected UISprite msp_lockBtn;
	protected UISprite msp_leftBg;
	protected UnityEngine.GameObject mobj_line2;
	protected UIGridContainer mGrid_right;
	protected UISprite msp_quality;
	protected UIWrapContent mwrap_left;
	protected override void _InitScriptBinder()
	{
		mobj_view2 = ScriptBinder.GetObject("obj_view2") as UnityEngine.GameObject;
		mbtn_award = ScriptBinder.GetObject("btn_award") as UIEventListener;
		mlb_desc = ScriptBinder.GetObject("lb_desc") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mbtn_lock = ScriptBinder.GetObject("btn_lock") as UIEventListener;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mGrid_leftCards = ScriptBinder.GetObject("Grid_leftCards") as UIGridContainer;
		mlb_hintRight = ScriptBinder.GetObject("lb_hintRight") as UILabel;
		mlb_lock = ScriptBinder.GetObject("lb_lock") as UILabel;
		mbtn_scroll = ScriptBinder.GetObject("btn_scroll") as UnityEngine.GameObject;
		msp_awardBtn = ScriptBinder.GetObject("sp_awardBtn") as UISprite;
		mtrans_rightScroll = ScriptBinder.GetObject("trans_rightScroll") as UnityEngine.Transform;
		msp_rightBg = ScriptBinder.GetObject("sp_rightBg") as UISprite;
		mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
		mscroll_left = ScriptBinder.GetObject("scroll_left") as UIScrollView;
		mlb_awardBtn = ScriptBinder.GetObject("lb_awardBtn") as UILabel;
		mwrap_right = ScriptBinder.GetObject("wrap_right") as UIWrapContent;
		mobj_curlock = ScriptBinder.GetObject("obj_curlock") as UnityEngine.GameObject;
		msp_lockBtn = ScriptBinder.GetObject("sp_lockBtn") as UISprite;
		msp_leftBg = ScriptBinder.GetObject("sp_leftBg") as UISprite;
		mobj_line2 = ScriptBinder.GetObject("obj_line2") as UnityEngine.GameObject;
		mGrid_right = ScriptBinder.GetObject("Grid_right") as UIGridContainer;
		msp_quality = ScriptBinder.GetObject("sp_quality") as UISprite;
		mwrap_left = ScriptBinder.GetObject("wrap_left") as UIWrapContent;
	}
}
