public partial class UIRoleSelectionMenuPanel : UIBasePanel
{
	protected UILabel mlb_role_name;
	protected UILabel mlb_role_level;
	protected UIEventListener mbtn_close;
	protected UISprite mbg_head;
	protected UISprite msp_role_head;
	protected UnityEngine.GameObject mSelectionMenuPanel;
	protected UISprite mbg;
	protected UILabel mlb_career;
	protected override void _InitScriptBinder()
	{
		mlb_role_name = ScriptBinder.GetObject("lb_role_name") as UILabel;
		mlb_role_level = ScriptBinder.GetObject("lb_role_level") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbg_head = ScriptBinder.GetObject("bg_head") as UISprite;
		msp_role_head = ScriptBinder.GetObject("sp_role_head") as UISprite;
		mSelectionMenuPanel = ScriptBinder.GetObject("SelectionMenuPanel") as UnityEngine.GameObject;
		mbg = ScriptBinder.GetObject("bg") as UISprite;
		mlb_career = ScriptBinder.GetObject("lb_career") as UILabel;
	}
}
