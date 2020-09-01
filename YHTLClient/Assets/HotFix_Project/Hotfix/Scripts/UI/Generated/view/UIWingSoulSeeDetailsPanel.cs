public partial class UIWingSoulSeeDetailsPanel : UIBasePanel
{
	protected UIGridContainer mgrid_effects;
	protected UISprite mbg;
	protected override void _InitScriptBinder()
	{
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mbg = ScriptBinder.GetObject("bg") as UISprite;
	}
}
