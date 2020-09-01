public partial class UIAuctionSellPanel : UIBasePanel
{
	protected UIGridContainer mgrid_sellItems;
	protected UnityEngine.GameObject mbtn_help;
	protected UILabel mlb_myStall;
	protected UIScrollView mscr_bagScroll;
	protected UIGrid mgrid_bagItems;
	protected override void _InitScriptBinder()
	{
		mgrid_sellItems = ScriptBinder.GetObject("grid_sellItems") as UIGridContainer;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mlb_myStall = ScriptBinder.GetObject("lb_myStall") as UILabel;
		mscr_bagScroll = ScriptBinder.GetObject("scr_bagScroll") as UIScrollView;
		mgrid_bagItems = ScriptBinder.GetObject("grid_bagItems") as UIGrid;
	}
}
