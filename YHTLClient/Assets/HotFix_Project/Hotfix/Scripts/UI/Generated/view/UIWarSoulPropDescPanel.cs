public partial class UIWarSoulPropDescPanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected UIScrollView mScrollView;
	protected UnityEngine.GameObject mDownIcon;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mDownIcon = ScriptBinder.GetObject("DownIcon") as UnityEngine.GameObject;
	}
}
