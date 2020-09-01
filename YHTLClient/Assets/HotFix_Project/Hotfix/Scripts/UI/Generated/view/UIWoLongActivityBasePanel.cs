public partial class UIWoLongActivityBasePanel : UIBasePanel
{
	protected UILabel mlb_level;
	protected UILabel mlb_desc;
	protected UIGridContainer mgrid_rewards;
	protected UnityEngine.GameObject mbtn_buy;
	protected UISprite msp_icon;
	protected UILabel mlb_value;
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject mbtn_add;
	protected override void _InitScriptBinder()
	{
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlb_desc = ScriptBinder.GetObject("lb_desc") as UILabel;
		mgrid_rewards = ScriptBinder.GetObject("grid_rewards") as UIGridContainer;
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
	}
}
