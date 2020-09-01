public partial class UIGuildBossInstancePanel : UIBasePanel
{
	protected UILabel mlb_count;
	protected UIGridContainer mGrid;
	protected UILabel mlb_title;
	protected override void _InitScriptBinder()
	{
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
	}
}
