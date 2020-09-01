public partial class UISkillTipsPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_mask;
	protected UISprite msp_bg;
	protected UILabel mlb_des;
	protected UILabel mlb_name;
	protected override void _InitScriptBinder()
	{
		mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
	}
}
