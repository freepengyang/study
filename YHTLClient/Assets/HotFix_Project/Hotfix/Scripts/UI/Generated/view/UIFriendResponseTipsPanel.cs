public partial class UIFriendResponseTipsPanel : UIBasePanel
{
	protected UILabel mtitle;
	protected UILabel mtip;
	protected UIGridContainer mfriendItemList;
	protected UIWidget mcontainer;
	protected UIEventListener mBtnClose;
	protected UIEventListener mBtnRejectAll;
	protected UIEventListener mBtnAddAll;
	protected override void _InitScriptBinder()
	{
		mtitle = ScriptBinder.GetObject("title") as UILabel;
		mtip = ScriptBinder.GetObject("tip") as UILabel;
		mfriendItemList = ScriptBinder.GetObject("friendItemList") as UIGridContainer;
		mcontainer = ScriptBinder.GetObject("container") as UIWidget;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBtnRejectAll = ScriptBinder.GetObject("BtnRejectAll") as UIEventListener;
		mBtnAddAll = ScriptBinder.GetObject("BtnAddAll") as UIEventListener;
	}
}
