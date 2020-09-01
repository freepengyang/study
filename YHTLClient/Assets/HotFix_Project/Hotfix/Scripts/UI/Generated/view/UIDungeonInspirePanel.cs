public partial class UIDungeonInspirePanel : UIBasePanel
{
	protected UIGridContainer mgird_skill;
	protected UILabel mlb_value;
	protected UnityEngine.GameObject mbtn_add;
	protected UISprite msp_icon;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_refresh;
	protected UnityEngine.GameObject mobj_view;
	protected UnityEngine.GameObject mobj_down;
	protected UITexture mtex_bg;
	protected UILabel mlb_hint;
	protected override void _InitScriptBinder()
	{
		mgird_skill = ScriptBinder.GetObject("gird_skill") as UIGridContainer;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_refresh = ScriptBinder.GetObject("btn_refresh") as UnityEngine.GameObject;
		mobj_view = ScriptBinder.GetObject("obj_view") as UnityEngine.GameObject;
		mobj_down = ScriptBinder.GetObject("obj_down") as UnityEngine.GameObject;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UITexture;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
	}
}
