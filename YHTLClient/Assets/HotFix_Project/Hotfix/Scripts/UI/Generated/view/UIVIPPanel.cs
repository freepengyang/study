public partial class UIVIPPanel : UIBasePanel
{
	protected UILabel mlb_level;
	protected UILabel mlb_levelbox;
	protected UILabel mlb_tips;
	protected UIGridContainer mgrid_vips;
	protected UILabel mlb_hint;
	protected UISlider mslider_exp;
	protected UILabel mlb_exp;
	protected UnityEngine.GameObject mbtn_upgrade;
	protected UnityEngine.GameObject mbtn_get;
	protected UnityEngine.GameObject mbtn_close;
	protected UIGrid mgrid_items;
	protected UnityEngine.GameObject mlb_Received;
	protected UIScrollView mscrollview_vip;
	protected UnityEngine.GameObject msp_up;
	protected UnityEngine.GameObject msp_down;
	protected UnityEngine.GameObject mvip_bg;
	protected UILabel mlb_levelLeft;
	protected UnityEngine.GameObject mvip_effect;
	protected UnityEngine.GameObject mobj_topLevel;
	protected UnityEngine.GameObject mobj_upLevel;
	protected UnityEngine.GameObject mbtngeteffect;
	protected UnityEngine.GameObject msp_unreach;
	protected UILabel mlb_time;
	protected override void _InitScriptBinder()
	{
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlb_levelbox = ScriptBinder.GetObject("lb_levelbox") as UILabel;
		mlb_tips = ScriptBinder.GetObject("lb_tips") as UILabel;
		mgrid_vips = ScriptBinder.GetObject("grid_vips") as UIGridContainer;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mslider_exp = ScriptBinder.GetObject("slider_exp") as UISlider;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mbtn_upgrade = ScriptBinder.GetObject("btn_upgrade") as UnityEngine.GameObject;
		mbtn_get = ScriptBinder.GetObject("btn_get") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mgrid_items = ScriptBinder.GetObject("grid_items") as UIGrid;
		mlb_Received = ScriptBinder.GetObject("lb_Received") as UnityEngine.GameObject;
		mscrollview_vip = ScriptBinder.GetObject("scrollview_vip") as UIScrollView;
		msp_up = ScriptBinder.GetObject("sp_up") as UnityEngine.GameObject;
		msp_down = ScriptBinder.GetObject("sp_down") as UnityEngine.GameObject;
		mvip_bg = ScriptBinder.GetObject("vip_bg") as UnityEngine.GameObject;
		mlb_levelLeft = ScriptBinder.GetObject("lb_levelLeft") as UILabel;
		mvip_effect = ScriptBinder.GetObject("vip_effect") as UnityEngine.GameObject;
		mobj_topLevel = ScriptBinder.GetObject("obj_topLevel") as UnityEngine.GameObject;
		mobj_upLevel = ScriptBinder.GetObject("obj_upLevel") as UnityEngine.GameObject;
		mbtngeteffect = ScriptBinder.GetObject("btngeteffect") as UnityEngine.GameObject;
		msp_unreach = ScriptBinder.GetObject("sp_unreach") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
	}
}
