public partial class UIGuildRankInstancePanel : UIBasePanel
{
	protected UIGridContainer mgrid_ranks;
	protected UILabel mlb_rank;
	protected UILabel mlb_name;
	protected UILabel mlb_num;
	protected override void _InitScriptBinder()
	{
		mgrid_ranks = ScriptBinder.GetObject("grid_ranks") as UIGridContainer;
		mlb_rank = ScriptBinder.GetObject("lb_rank") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_num = ScriptBinder.GetObject("lb_num") as UILabel;
	}
}
