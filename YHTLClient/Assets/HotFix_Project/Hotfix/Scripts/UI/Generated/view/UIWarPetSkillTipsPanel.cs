public partial class UIWarPetSkillTipsPanel : UIBasePanel
{
	protected UISprite msp_bg;
	protected UITexture mtx_quality;
	protected UISprite msp_skill_icon;
	protected UILabel mlb_skill_name;
	protected UILabel mlb_skill_lv;
	protected UILabel mlb_skill_desc;
	protected UILabel mlb_skill_cd_time;
	protected UILabel mlb_talentUnlock;
	protected UILabel mlb_skill_name_beidong;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mtx_quality = ScriptBinder.GetObject("tx_quality") as UITexture;
		msp_skill_icon = ScriptBinder.GetObject("sp_skill_icon") as UISprite;
		mlb_skill_name = ScriptBinder.GetObject("lb_skill_name") as UILabel;
		mlb_skill_lv = ScriptBinder.GetObject("lb_skill_lv") as UILabel;
		mlb_skill_desc = ScriptBinder.GetObject("lb_skill_desc") as UILabel;
		mlb_skill_cd_time = ScriptBinder.GetObject("lb_skill_cd_time") as UILabel;
		mlb_talentUnlock = ScriptBinder.GetObject("lb_talentUnlock") as UILabel;
		mlb_skill_name_beidong = ScriptBinder.GetObject("lb_skill_name_beidong") as UILabel;
	}
}
