public partial class UIGiftBagPreviewPanel : UIBasePanel
{
	protected UIGridContainer mgrid_gift;
	protected UILabel mlb_name;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mgrid_gift = ScriptBinder.GetObject("grid_gift") as UIGridContainer;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
