public partial class UIPetBasePreviewPanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
	}
}
