public partial class UIGuildEndActivityPanel : UIBasePanel
{
	protected UnityEngine.GameObject meffect;
	protected UILabel mlb_rank;
	protected UILabel mlb_score;
	protected UIGridContainer mGrid;
	protected override void _InitScriptBinder()
	{
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		mlb_rank = ScriptBinder.GetObject("lb_rank") as UILabel;
		mlb_score = ScriptBinder.GetObject("lb_score") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
	}
}
