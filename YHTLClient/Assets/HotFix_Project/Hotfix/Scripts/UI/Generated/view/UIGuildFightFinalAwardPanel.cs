public partial class UIGuildFightFinalAwardPanel : UIBasePanel
{
	protected UIGrid mgrid_reward;
	protected UIEventListener mbtn_close;
	protected UILabel mtitle;
	protected override void _InitScriptBinder()
	{
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGrid;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtitle = ScriptBinder.GetObject("title") as UILabel;
	}
}
