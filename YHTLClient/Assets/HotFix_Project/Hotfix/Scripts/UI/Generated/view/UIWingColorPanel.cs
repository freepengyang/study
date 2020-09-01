public partial class UIWingColorPanel : UIBasePanel
{
	protected UIEventListener mbtn_attribute;
	protected UILabel mlb_title;
	protected UnityEngine.GameObject msp_stage_effect_bg;
	protected UISprite msp_stage_effect_icon;
	protected UIGridContainer mgrid_effects;
	protected UIGridContainer mgrid_wing_color;
	protected UnityEngine.GameObject meffect_wing_idle_add;
	protected override void _InitScriptBinder()
	{
		mbtn_attribute = ScriptBinder.GetObject("btn_attribute") as UIEventListener;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		msp_stage_effect_bg = ScriptBinder.GetObject("sp_stage_effect_bg") as UnityEngine.GameObject;
		msp_stage_effect_icon = ScriptBinder.GetObject("sp_stage_effect_icon") as UISprite;
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mgrid_wing_color = ScriptBinder.GetObject("grid_wing_color") as UIGridContainer;
		meffect_wing_idle_add = ScriptBinder.GetObject("effect_wing_idle_add") as UnityEngine.GameObject;
	}
}
