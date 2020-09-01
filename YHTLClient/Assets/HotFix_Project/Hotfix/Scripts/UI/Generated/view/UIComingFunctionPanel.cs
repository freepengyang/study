public partial class UIComingFunctionPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UISprite mFuncIcon;
	protected UILabel mFuncTitle;
	protected UILabel mFuncDesc;
	protected UILabel mFuncOpenLevel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mFuncIcon = ScriptBinder.GetObject("FuncIcon") as UISprite;
		mFuncTitle = ScriptBinder.GetObject("FuncTitle") as UILabel;
		mFuncDesc = ScriptBinder.GetObject("FuncDesc") as UILabel;
		mFuncOpenLevel = ScriptBinder.GetObject("FuncOpenLevel") as UILabel;
	}
}
