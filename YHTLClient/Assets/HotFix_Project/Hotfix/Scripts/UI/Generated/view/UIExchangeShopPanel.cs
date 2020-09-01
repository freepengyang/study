public partial class UIExchangeShopPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_hint;
	protected UISprite msp_ticketIcon;
	protected UIEventListener mbtn_ticketAdd;
	protected UILabel mlb_ticketNum;
	protected UIWrapContent mwrap;
	protected UIGridContainer mgrid_toggles;
	protected UIScrollView mScrollView;
	protected UnityEngine.GameObject mobj_arrowR;
	protected UnityEngine.GameObject mobj_arrowL;
	protected override void _InitScriptBinder()
	{
		mobj_hint = ScriptBinder.GetObject("obj_hint") as UnityEngine.GameObject;
		msp_ticketIcon = ScriptBinder.GetObject("sp_ticketIcon") as UISprite;
		mbtn_ticketAdd = ScriptBinder.GetObject("btn_ticketAdd") as UIEventListener;
		mlb_ticketNum = ScriptBinder.GetObject("lb_ticketNum") as UILabel;
		mwrap = ScriptBinder.GetObject("wrap") as UIWrapContent;
		mgrid_toggles = ScriptBinder.GetObject("grid_toggles") as UIGridContainer;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mobj_arrowR = ScriptBinder.GetObject("obj_arrowR") as UnityEngine.GameObject;
		mobj_arrowL = ScriptBinder.GetObject("obj_arrowL") as UnityEngine.GameObject;
	}
}
