public partial class UIUltimateChallengStorePanel
{
	protected UnityEngine.GameObject mbg;
	protected UIScrollView mScrollView;
	protected UIGridContainer mGrid;
	protected UnityEngine.GameObject mscrollViewDrag;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mbg = ScriptBinder.GetObject("bg") as UnityEngine.GameObject;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mscrollViewDrag = ScriptBinder.GetObject("scrollViewDrag") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
