public partial class UINewSkillPanel : UIBasePanel
{
	protected UISprite mSkillIcon;
	protected TweenPosition mtweenPosition;
	protected TweenScale mtweenScale;
	protected UILabel mSkillName;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mSkillIcon = ScriptBinder.GetObject("SkillIcon") as UISprite;
		mtweenPosition = ScriptBinder.GetObject("tweenPosition") as TweenPosition;
		mtweenScale = ScriptBinder.GetObject("tweenScale") as TweenScale;
		mSkillName = ScriptBinder.GetObject("SkillName") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
