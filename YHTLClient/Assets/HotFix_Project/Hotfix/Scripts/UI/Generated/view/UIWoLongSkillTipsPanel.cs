public partial class UIWoLongSkillTipsPanel : UIBasePanel
{
	protected UISprite msp_bg;
	protected UITexture mtx_quality;
	protected UISprite msp_skill_icon;
	protected UILabel mlb_skill_name;
	protected UILabel mlb_skill_lv;
	protected UILabel mlb_skill_desc;
	protected UILabel mlb_skill_cd_time;
	protected UnityEngine.Transform mobj_line;
	protected UIWidget mobj_view;
	protected UILabel mlb_lock;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mtx_quality = ScriptBinder.GetObject("tx_quality") as UITexture;
		msp_skill_icon = ScriptBinder.GetObject("sp_skill_icon") as UISprite;
		mlb_skill_name = ScriptBinder.GetObject("lb_skill_name") as UILabel;
		mlb_skill_lv = ScriptBinder.GetObject("lb_skill_lv") as UILabel;
		mlb_skill_desc = ScriptBinder.GetObject("lb_skill_desc") as UILabel;
		mlb_skill_cd_time = ScriptBinder.GetObject("lb_skill_cd_time") as UILabel;
		mobj_line = ScriptBinder.GetObject("obj_line") as UnityEngine.Transform;
		mobj_view = ScriptBinder.GetObject("obj_view") as UIWidget;
		mlb_lock = ScriptBinder.GetObject("lb_lock") as UILabel;
	}
}
