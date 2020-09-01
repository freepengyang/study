public partial class UIPetSuitTipPanel : UIBasePanel
{
	protected UnityEngine.GameObject mBG;
	protected UnityEngine.GameObject mNextInfo;
	protected UIScrollView mCurInfoScrollView;
	protected UIGridContainer mCurInfoGrid;
	protected UnityEngine.GameObject mCurInfoDownIcon;
	protected UIScrollView mNextInfoScrollView;
	protected UIGridContainer mNextInfoGrid;
	protected UnityEngine.GameObject mNextInfoDownIcon;
	protected UILabel mCurInfoSuitName;
	protected UILabel mCurInfoSuitSubName;
	protected UILabel mNextInfoSuitName;
	protected UILabel mNextInfoSuitSubName;
	protected UnityEngine.Transform mTip;
	protected override void _InitScriptBinder()
	{
		mBG = ScriptBinder.GetObject("BG") as UnityEngine.GameObject;
		mNextInfo = ScriptBinder.GetObject("NextInfo") as UnityEngine.GameObject;
		mCurInfoScrollView = ScriptBinder.GetObject("CurInfoScrollView") as UIScrollView;
		mCurInfoGrid = ScriptBinder.GetObject("CurInfoGrid") as UIGridContainer;
		mCurInfoDownIcon = ScriptBinder.GetObject("CurInfoDownIcon") as UnityEngine.GameObject;
		mNextInfoScrollView = ScriptBinder.GetObject("NextInfoScrollView") as UIScrollView;
		mNextInfoGrid = ScriptBinder.GetObject("NextInfoGrid") as UIGridContainer;
		mNextInfoDownIcon = ScriptBinder.GetObject("NextInfoDownIcon") as UnityEngine.GameObject;
		mCurInfoSuitName = ScriptBinder.GetObject("CurInfoSuitName") as UILabel;
		mCurInfoSuitSubName = ScriptBinder.GetObject("CurInfoSuitSubName") as UILabel;
		mNextInfoSuitName = ScriptBinder.GetObject("NextInfoSuitName") as UILabel;
		mNextInfoSuitSubName = ScriptBinder.GetObject("NextInfoSuitSubName") as UILabel;
		mTip = ScriptBinder.GetObject("Tip") as UnityEngine.Transform;
	}
}
