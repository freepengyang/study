public partial class UINostalgiaPromptPanel : UIBasePanel
{
	protected UISprite msp_suit;
	protected UILabel mlb_effect;
	protected UILabel mlb_time;
	protected UILabel mlb_hint;
	protected UISprite msp_name;
	protected UISprite msp_num;
	protected UITexture mTexbg;
	protected override void _InitScriptBinder()
	{
		msp_suit = ScriptBinder.GetObject("sp_suit") as UISprite;
		mlb_effect = ScriptBinder.GetObject("lb_effect") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		msp_name = ScriptBinder.GetObject("sp_name") as UISprite;
		msp_num = ScriptBinder.GetObject("sp_num") as UISprite;
		mTexbg = ScriptBinder.GetObject("Texbg") as UITexture;
	}
}
