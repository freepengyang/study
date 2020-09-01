public partial class UIWoLongPromptPanel : UIBasePanel
{
	protected UIGridContainer mgrid;
	protected override void _InitScriptBinder()
	{
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
	}
}
