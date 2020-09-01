public partial class UIWarPetSkillPromptPanel : UIBasePanel
{
	protected UIGridContainer mgrid_skill;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject msp_scroll;
	protected UISprite msp_bg;
	protected UnityEngine.GameObject mlb_hint;
	protected UIScrollBar mbar;
	protected override void _InitScriptBinder()
	{
		mgrid_skill = ScriptBinder.GetObject("grid_skill") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		msp_scroll = ScriptBinder.GetObject("sp_scroll") as UnityEngine.GameObject;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UnityEngine.GameObject;
		mbar = ScriptBinder.GetObject("bar") as UIScrollBar;
	}
}
