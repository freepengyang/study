public partial class UIAddFriendPanel : UIBasePanel
{
	protected UIInput mInput;
	protected UIEventListener mBtnAddAll;
	protected UIEventListener mBtnAddClose;
	protected UIEventListener mBtnSearch;
	protected UIGridContainer mGridList;
	protected override void _InitScriptBinder()
	{
		mInput = ScriptBinder.GetObject("Input") as UIInput;
		mBtnAddAll = ScriptBinder.GetObject("BtnAddAll") as UIEventListener;
		mBtnAddClose = ScriptBinder.GetObject("BtnAddClose") as UIEventListener;
		mBtnSearch = ScriptBinder.GetObject("BtnSearch") as UIEventListener;
		mGridList = ScriptBinder.GetObject("GridList") as UIGridContainer;
	}
}
