public partial class UIGuildDevidePanel : UIBasePanel
{
	protected UIInput mInput;
	protected UIEventListener mBtnClose;
	protected UIEventListener mBtnSearch;
	protected UIGridContainer mGridList;
	protected UIEventListener mBtnDevide;
	protected UILabel mYuanBao;
	protected override void _InitScriptBinder()
	{
		mInput = ScriptBinder.GetObject("Input") as UIInput;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBtnSearch = ScriptBinder.GetObject("BtnSearch") as UIEventListener;
		mGridList = ScriptBinder.GetObject("GridList") as UIGridContainer;
		mBtnDevide = ScriptBinder.GetObject("BtnDevide") as UIEventListener;
		mYuanBao = ScriptBinder.GetObject("YuanBao") as UILabel;
	}
}
