public partial class UIRecycleGetPanel
{
	protected UIGridContainer mAwardGrids;
	protected override void _InitScriptBinder()
	{
		mAwardGrids = ScriptBinder.GetObject("AwardGrids") as UIGridContainer;
	}
}
