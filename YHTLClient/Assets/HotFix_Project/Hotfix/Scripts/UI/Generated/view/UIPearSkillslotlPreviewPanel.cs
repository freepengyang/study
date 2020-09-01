public partial class UIPearSkillslotlPreviewPanel : UIBasePanel
{
	protected UIEventListener mbtn_bg;
	protected UIEventListener mbtn_close;
	protected UIGridContainer mgrid_catalog;
	protected UIGridContainer mgrid_skill;
	protected override void _InitScriptBinder()
	{
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mgrid_catalog = ScriptBinder.GetObject("grid_catalog") as UIGridContainer;
		mgrid_skill = ScriptBinder.GetObject("grid_skill") as UIGridContainer;
	}
}
