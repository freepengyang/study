public partial class UIGuildFightPanel : UIBasePanel
{
	protected UILabel mlb_stage_desc;
	protected UILabel mlb_fixed_desc;
	protected UIEventListener mbtn_help;
	protected UIEventListener mbtn_go_fight;
	protected UnityEngine.GameObject mtex_bg;
	protected UnityEngine.GameObject mbigItem;
	protected UnityEngine.GameObject mminItem;
	protected override void _InitScriptBinder()
	{
		mlb_stage_desc = ScriptBinder.GetObject("lb_stage_desc") as UILabel;
		mlb_fixed_desc = ScriptBinder.GetObject("lb_fixed_desc") as UILabel;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mbtn_go_fight = ScriptBinder.GetObject("btn_go_fight") as UIEventListener;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mbigItem = ScriptBinder.GetObject("bigItem") as UnityEngine.GameObject;
		mminItem = ScriptBinder.GetObject("minItem") as UnityEngine.GameObject;
	}
}
