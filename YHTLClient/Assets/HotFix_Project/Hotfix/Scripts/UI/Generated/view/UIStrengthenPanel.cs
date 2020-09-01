public partial class UIStrengthenPanel : UIBasePanel
{
	protected UIGridContainer mGridTab;
	protected UIGridContainer mGridInfo;
	protected UIScrollView mInfoScrollView;
	protected override void _InitScriptBinder()
	{
		mGridTab = ScriptBinder.GetObject("GridTab") as UIGridContainer;
		mGridInfo = ScriptBinder.GetObject("GridInfo") as UIGridContainer;
		mInfoScrollView = ScriptBinder.GetObject("InfoScrollView") as UIScrollView;
	}
}
