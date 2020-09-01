public partial class UIServerActivityBossPanel : UIBasePanel
{
	protected UnityEngine.GameObject mBG;
	protected UILabel mLbTime;
	protected UnityEngine.GameObject mUIItemBarPrefab;
	protected UIEventListener mBtnMove;
	protected UIGridContainer mGrid;
	protected UIEventListener mBtnHelp;
	protected UnityEngine.GameObject mDownIcon;
	protected UIScrollBar mScrollBar;
	protected CSInvoke mCSInvoke;
	protected override void _InitScriptBinder()
	{
		mBG = ScriptBinder.GetObject("BG") as UnityEngine.GameObject;
		mLbTime = ScriptBinder.GetObject("LbTime") as UILabel;
		mUIItemBarPrefab = ScriptBinder.GetObject("UIItemBarPrefab") as UnityEngine.GameObject;
		mBtnMove = ScriptBinder.GetObject("BtnMove") as UIEventListener;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mDownIcon = ScriptBinder.GetObject("DownIcon") as UnityEngine.GameObject;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
	}
}
