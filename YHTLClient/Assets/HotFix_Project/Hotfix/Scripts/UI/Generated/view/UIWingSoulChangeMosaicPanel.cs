public partial class UIWingSoulChangeMosaicPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIGridContainer mgrid_wingSoul;
	protected UILabel mlb_title;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mgrid_wingSoul = ScriptBinder.GetObject("grid_wingSoul") as UIGridContainer;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
	}
}
