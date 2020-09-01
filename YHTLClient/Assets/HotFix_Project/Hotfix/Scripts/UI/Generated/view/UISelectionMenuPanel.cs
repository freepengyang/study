public partial class UISelectionMenuPanel : UIBase
{
	protected UISprite msp_bg;
	protected UIGridContainer mgrid_btns;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mgrid_btns = ScriptBinder.GetObject("grid_btns") as UIGridContainer;
	}
}
