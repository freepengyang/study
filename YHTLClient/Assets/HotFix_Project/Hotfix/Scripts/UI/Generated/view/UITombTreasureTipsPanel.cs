public partial class UITombTreasureTipsPanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected UIScrollBar mScrollBar;
	protected UnityEngine.GameObject mDirection;
	protected UnityEngine.GameObject mBG;
	protected UnityEngine.GameObject mView;
	protected UnityEngine.GameObject mPosition;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mDirection = ScriptBinder.GetObject("Direction") as UnityEngine.GameObject;
		mBG = ScriptBinder.GetObject("BG") as UnityEngine.GameObject;
		mView = ScriptBinder.GetObject("View") as UnityEngine.GameObject;
		mPosition = ScriptBinder.GetObject("Position") as UnityEngine.GameObject;
	}
}
