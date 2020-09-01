public partial class UIDungeonSkillTipsPanel : UIBase
{
	protected UISprite msp_skill_icon;
	protected UILabel mlb_skill_name;
	protected UILabel mlb_skill_lv;
	protected UILabel mlb_skill_desc;
	protected UILabel mlb_skill_cd_time;
	protected override void _InitScriptBinder()
	{
		msp_skill_icon = ScriptBinder.GetObject("sp_skill_icon") as UISprite;
		mlb_skill_name = ScriptBinder.GetObject("lb_skill_name") as UILabel;
		mlb_skill_lv = ScriptBinder.GetObject("lb_skill_lv") as UILabel;
		mlb_skill_desc = ScriptBinder.GetObject("lb_skill_desc") as UILabel;
		mlb_skill_cd_time = ScriptBinder.GetObject("lb_skill_cd_time") as UILabel;
	}
}
