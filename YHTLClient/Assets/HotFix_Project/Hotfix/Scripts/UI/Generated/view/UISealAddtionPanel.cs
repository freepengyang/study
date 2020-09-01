public partial class UISealAddtionPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIGridContainer mgrid_seal_addtion;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mgrid_seal_addtion = ScriptBinder.GetObject("grid_seal_addtion") as UIGridContainer;
	}
}
