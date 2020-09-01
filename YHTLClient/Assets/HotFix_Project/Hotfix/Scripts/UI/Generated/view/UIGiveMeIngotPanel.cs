public partial class UIGiveMeIngotPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UISprite mGetIcon;
	protected UILabel mLbGetNum;
	protected UISprite mLimitIcon;
	protected UILabel mLbLimitNum;
	protected UIGridContainer mRightGrid;
	protected UnityEngine.GameObject mIngotBg;
	protected UIGridContainer mTextGrid;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mGetIcon = ScriptBinder.GetObject("GetIcon") as UISprite;
		mLbGetNum = ScriptBinder.GetObject("LbGetNum") as UILabel;
		mLimitIcon = ScriptBinder.GetObject("LimitIcon") as UISprite;
		mLbLimitNum = ScriptBinder.GetObject("LbLimitNum") as UILabel;
		mRightGrid = ScriptBinder.GetObject("RightGrid") as UIGridContainer;
		mIngotBg = ScriptBinder.GetObject("IngotBg") as UnityEngine.GameObject;
		mTextGrid = ScriptBinder.GetObject("TextGrid") as UIGridContainer;
	}
}
