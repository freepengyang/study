public partial class UIChatVoiceMenuPanel : UIBasePanel
{
	protected UIEventListener mBtnVoice;
	protected UIEventListener mBtnVoiceWorld;
	protected UIEventListener mBtnVoiceGuild;
	protected UISprite mMin;
	protected UISprite mAdd;
	protected TweenScale mOption;
	protected UITable mOptionTable;
	protected UISprite mArrow;
	protected UISprite mVoiceSprite;
	protected UIEventListener mbtn_voice_Team;
	protected UIEventListener mbtn_voice_Neighbouring;
	protected override void _InitScriptBinder()
	{
		mBtnVoice = ScriptBinder.GetObject("BtnVoice") as UIEventListener;
		mBtnVoiceWorld = ScriptBinder.GetObject("BtnVoiceWorld") as UIEventListener;
		mBtnVoiceGuild = ScriptBinder.GetObject("BtnVoiceGuild") as UIEventListener;
		mMin = ScriptBinder.GetObject("Min") as UISprite;
		mAdd = ScriptBinder.GetObject("Add") as UISprite;
		mOption = ScriptBinder.GetObject("Option") as TweenScale;
		mOptionTable = ScriptBinder.GetObject("OptionTable") as UITable;
		mArrow = ScriptBinder.GetObject("Arrow") as UISprite;
		mVoiceSprite = ScriptBinder.GetObject("VoiceSprite") as UISprite;
		mbtn_voice_Team = ScriptBinder.GetObject("btn_voice_Team") as UIEventListener;
		mbtn_voice_Neighbouring = ScriptBinder.GetObject("btn_voice_Neighbouring") as UIEventListener;
	}
}
