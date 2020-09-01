public partial class UINewFunctionPanel : UIBasePanel
{
	protected UILabel mFuncName;
	protected UISprite mFuncIcon;
	protected UIEventListener mbtn_close;
	protected TweenPosition mtweenPosition;
	protected TweenScale mtweenScale;
	protected override void _InitScriptBinder()
	{
		mFuncName = ScriptBinder.GetObject("FuncName") as UILabel;
		mFuncIcon = ScriptBinder.GetObject("FuncIcon") as UISprite;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtweenPosition = ScriptBinder.GetObject("tweenPosition") as TweenPosition;
		mtweenScale = ScriptBinder.GetObject("tweenScale") as TweenScale;
	}
}
