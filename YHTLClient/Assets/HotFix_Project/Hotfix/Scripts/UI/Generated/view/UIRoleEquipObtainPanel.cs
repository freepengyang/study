public partial class UIRoleEquipObtainPanel : UIBasePanel
{
	protected UISprite msp_bg;
	protected UIGridContainer mgrid_btn;
	protected UIEventListener mbtn_bg;
	protected UISprite msp_bgButton;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mgrid_btn = ScriptBinder.GetObject("grid_btn") as UIGridContainer;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		msp_bgButton = ScriptBinder.GetObject("sp_bgButton") as UISprite;
	}
}
