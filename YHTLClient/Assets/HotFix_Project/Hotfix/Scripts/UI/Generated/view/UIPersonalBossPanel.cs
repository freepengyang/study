public partial class UIPersonalBossPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject mobj_leftarrow;
	protected UnityEngine.GameObject mobj_rightarrow;
	protected UILabel mlb_count;
	protected UIGridContainer mitemPar;
	protected UILabel mlb_costNum;
	protected UnityEngine.GameObject mbtn_buy;
	protected UISprite msp_costIcon;
	protected UIScrollView msc_Scroll;
	protected UnityEngine.GameObject mobj_arrowLeft;
	protected UnityEngine.GameObject mobj_arrowRight;
	protected UIScrollBar mobj_scrollBar;
	protected UnityEngine.GameObject mbtn_BuyCount;
	protected override void _InitScriptBinder()
	{
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mobj_leftarrow = ScriptBinder.GetObject("obj_leftarrow") as UnityEngine.GameObject;
		mobj_rightarrow = ScriptBinder.GetObject("obj_rightarrow") as UnityEngine.GameObject;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mitemPar = ScriptBinder.GetObject("itemPar") as UIGridContainer;
		mlb_costNum = ScriptBinder.GetObject("lb_costNum") as UILabel;
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		msp_costIcon = ScriptBinder.GetObject("sp_costIcon") as UISprite;
		msc_Scroll = ScriptBinder.GetObject("sc_Scroll") as UIScrollView;
		mobj_arrowLeft = ScriptBinder.GetObject("obj_arrowLeft") as UnityEngine.GameObject;
		mobj_arrowRight = ScriptBinder.GetObject("obj_arrowRight") as UnityEngine.GameObject;
		mobj_scrollBar = ScriptBinder.GetObject("obj_scrollBar") as UIScrollBar;
		mbtn_BuyCount = ScriptBinder.GetObject("btn_BuyCount") as UnityEngine.GameObject;
	}
}
