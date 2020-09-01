public partial class UIMapTransferPanel
{
	protected UIGrid mleftToggleGroup;
	protected UIGridContainer mrightGroup;
	protected UIToggle mmainToggle;
	protected UIToggle mfieldToggle;
	protected UIToggle mspecialToggle;
	protected UIScrollView mScrollView;
	protected override void _InitScriptBinder()
	{
		mleftToggleGroup = ScriptBinder.GetObject("leftToggleGroup") as UIGrid;
		mrightGroup = ScriptBinder.GetObject("rightGroup") as UIGridContainer;
		mmainToggle = ScriptBinder.GetObject("mainToggle") as UIToggle;
		mfieldToggle = ScriptBinder.GetObject("fieldToggle") as UIToggle;
		mspecialToggle = ScriptBinder.GetObject("specialToggle") as UIToggle;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
	}
}
