public partial class UIMainFuncPanel : UIBasePanel
{
	protected TweenPosition mRoot;
	protected UnityEngine.GameObject mUIFuncPanel;
	protected UIToggle mtg_func;
	protected UIToggle mtg_team;
	protected UIToggle mbtn_func;
	protected UIToggle mbtn_team;
	protected UILabel mlb_func;
	protected UISprite msp_func;
	protected UISprite msp_funclight;
	protected UnityEngine.GameObject mdrag;
	protected UISprite msp_team;
	protected UISprite msp_teamlight;
	protected UIEventListener mbtn_rotate;
	protected override void _InitScriptBinder()
	{
		mRoot = ScriptBinder.GetObject("Root") as TweenPosition;
		mUIFuncPanel = ScriptBinder.GetObject("UIFuncPanel") as UnityEngine.GameObject;
		mtg_func = ScriptBinder.GetObject("tg_func") as UIToggle;
		mtg_team = ScriptBinder.GetObject("tg_team") as UIToggle;
		mbtn_func = ScriptBinder.GetObject("btn_func") as UIToggle;
		mbtn_team = ScriptBinder.GetObject("btn_team") as UIToggle;
		mlb_func = ScriptBinder.GetObject("lb_func") as UILabel;
		msp_func = ScriptBinder.GetObject("sp_func") as UISprite;
		msp_funclight = ScriptBinder.GetObject("sp_funclight") as UISprite;
		mdrag = ScriptBinder.GetObject("drag") as UnityEngine.GameObject;
		msp_team = ScriptBinder.GetObject("sp_team") as UISprite;
		msp_teamlight = ScriptBinder.GetObject("sp_teamlight") as UISprite;
		mbtn_rotate = ScriptBinder.GetObject("btn_rotate") as UIEventListener;
	}
}
