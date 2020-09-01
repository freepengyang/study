public partial class UIWingSoulDetailedAttrsPanel : UIBasePanel
{
	protected UIGridContainer mgrid_effects;
	protected UISprite mbg;
	protected UILabel mlb_title;
	protected override void _InitScriptBinder()
	{
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mbg = ScriptBinder.GetObject("bg") as UISprite;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
	}
}
