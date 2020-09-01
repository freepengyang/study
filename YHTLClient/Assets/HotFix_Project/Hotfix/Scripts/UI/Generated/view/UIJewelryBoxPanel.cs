public partial class UIJewelryBoxPanel : UIBasePanel
{
	protected UIGridContainer mSuit;
	protected UIGrid mequipGrid;
	protected UIWrapContent mwrap_page;
	protected UILabel mlb_page;
	protected UnityEngine.GameObject mbtn_get;
	protected UIScrollView mscrollItems;
	protected UIScrollBar mscollbar;
	protected UIGridContainer mgcontain_equip;
	protected override void _InitScriptBinder()
	{
		mSuit = ScriptBinder.GetObject("Suit") as UIGridContainer;
		mequipGrid = ScriptBinder.GetObject("equipGrid") as UIGrid;
		mwrap_page = ScriptBinder.GetObject("wrap_page") as UIWrapContent;
		mlb_page = ScriptBinder.GetObject("lb_page") as UILabel;
		mbtn_get = ScriptBinder.GetObject("btn_get") as UnityEngine.GameObject;
		mscrollItems = ScriptBinder.GetObject("scrollItems") as UIScrollView;
		mscollbar = ScriptBinder.GetObject("scollbar") as UIScrollBar;
		mgcontain_equip = ScriptBinder.GetObject("gcontain_equip") as UIGridContainer;
	}
}
