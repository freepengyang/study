public partial class UIWelfareGiftBagPanel : UIBasePanel
{
	protected UIScrollView mScrollView_giftbag;
	protected UIWrapContent mWrapContent;
	protected UIGridContainer mgrid_item1;
	protected UIGridContainer mgrid_item2;
	protected UnityEngine.GameObject msp_right_scroll;
	protected UnityEngine.GameObject msp_left_scroll;
	protected UnityEngine.GameObject mtex_bg;
	protected UIGrid mgrid_btns;
	protected UIToggle mtoggle_tab1;
	protected UIToggle mtoggle_tab2;
	protected UIToggle mtoggle_tab3;
	protected UnityEngine.GameObject msp_lock2;
	protected UnityEngine.GameObject msp_lock3;
	protected UISprite msp_tab2;
	protected UISprite msp_checkmark2;
	protected UnityEngine.GameObject mmaskPanel;
	protected UIDragScrollView mDragScrollView;
	protected UISprite msp_checkmark3;
	protected UISprite msp_tab3;
	protected UIEventListener mbtn_locktab2;
	protected UIEventListener mbtn_locktab3;
	protected UnityEngine.GameObject mredpoint_tab1;
	protected UnityEngine.GameObject mredpoint_tab2;
	protected UnityEngine.GameObject mredpoint_tab3;
	protected UnityEngine.GameObject mbanner25;
	protected UILabel mlb_tab2;
	protected UILabel mlb_tab3;
	protected override void _InitScriptBinder()
	{
		mScrollView_giftbag = ScriptBinder.GetObject("ScrollView_giftbag") as UIScrollView;
		mWrapContent = ScriptBinder.GetObject("WrapContent") as UIWrapContent;
		mgrid_item1 = ScriptBinder.GetObject("grid_item1") as UIGridContainer;
		mgrid_item2 = ScriptBinder.GetObject("grid_item2") as UIGridContainer;
		msp_right_scroll = ScriptBinder.GetObject("sp_right_scroll") as UnityEngine.GameObject;
		msp_left_scroll = ScriptBinder.GetObject("sp_left_scroll") as UnityEngine.GameObject;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mgrid_btns = ScriptBinder.GetObject("grid_btns") as UIGrid;
		mtoggle_tab1 = ScriptBinder.GetObject("toggle_tab1") as UIToggle;
		mtoggle_tab2 = ScriptBinder.GetObject("toggle_tab2") as UIToggle;
		mtoggle_tab3 = ScriptBinder.GetObject("toggle_tab3") as UIToggle;
		msp_lock2 = ScriptBinder.GetObject("sp_lock2") as UnityEngine.GameObject;
		msp_lock3 = ScriptBinder.GetObject("sp_lock3") as UnityEngine.GameObject;
		msp_tab2 = ScriptBinder.GetObject("sp_tab2") as UISprite;
		msp_checkmark2 = ScriptBinder.GetObject("sp_checkmark2") as UISprite;
		mmaskPanel = ScriptBinder.GetObject("maskPanel") as UnityEngine.GameObject;
		mDragScrollView = ScriptBinder.GetObject("DragScrollView") as UIDragScrollView;
		msp_checkmark3 = ScriptBinder.GetObject("sp_checkmark3") as UISprite;
		msp_tab3 = ScriptBinder.GetObject("sp_tab3") as UISprite;
		mbtn_locktab2 = ScriptBinder.GetObject("btn_locktab2") as UIEventListener;
		mbtn_locktab3 = ScriptBinder.GetObject("btn_locktab3") as UIEventListener;
		mredpoint_tab1 = ScriptBinder.GetObject("redpoint_tab1") as UnityEngine.GameObject;
		mredpoint_tab2 = ScriptBinder.GetObject("redpoint_tab2") as UnityEngine.GameObject;
		mredpoint_tab3 = ScriptBinder.GetObject("redpoint_tab3") as UnityEngine.GameObject;
		mbanner25 = ScriptBinder.GetObject("banner25") as UnityEngine.GameObject;
		mlb_tab2 = ScriptBinder.GetObject("lb_tab2") as UILabel;
		mlb_tab3 = ScriptBinder.GetObject("lb_tab3") as UILabel;
	}
}
