public partial class UIDreamInstancePanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
	}
}
