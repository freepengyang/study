public partial class UIFastAccessWoLongPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UIGridContainer mTextGrid;
	protected UILabel mLbTips1;
	protected UILabel mLbTips2;
	protected UIEventListener mTips2Btn;
	protected UILabel mTitle;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTextGrid = ScriptBinder.GetObject("TextGrid") as UIGridContainer;
		mLbTips1 = ScriptBinder.GetObject("LbTips1") as UILabel;
		mLbTips2 = ScriptBinder.GetObject("LbTips2") as UILabel;
		mTips2Btn = ScriptBinder.GetObject("Tips2Btn") as UIEventListener;
		mTitle = ScriptBinder.GetObject("Title") as UILabel;
	}
}
