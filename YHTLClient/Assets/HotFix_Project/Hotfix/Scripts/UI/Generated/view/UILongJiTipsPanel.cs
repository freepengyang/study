public partial class UILongJiTipsPanel : UIBasePanel
{
	protected UIScrollView mscr_longji;
	protected UIGridContainer mgrid_longji;
	protected UILabel mlb_noLongJi;
	protected override void _InitScriptBinder()
	{
		mscr_longji = ScriptBinder.GetObject("scr_longji") as UIScrollView;
		mgrid_longji = ScriptBinder.GetObject("grid_longji") as UIGridContainer;
		mlb_noLongJi = ScriptBinder.GetObject("lb_noLongJi") as UILabel;
	}
}
