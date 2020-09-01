public partial class UIVigorPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_introduce;
	protected UILabel mlb_levelExp;
	protected UILabel mlb_levelValue;
	protected UnityEngine.GameObject mobj_bg;
	protected UnityEngine.GameObject mobj_bg2;
	protected UnityEngine.GameObject mobj_free1;
	protected UILabel mlb_todayNum;
	protected UnityEngine.GameObject mobj_exchange;
	protected UISprite mlb_pro;
	protected UILabel mlb_getVigor;
	protected TweenAlpha mtween_getVigor;
	protected UnityEngine.GameObject mobj_lb1;
	protected UnityEngine.GameObject mobj_tex1;
	protected UnityEngine.GameObject mobj_tex2;
	protected UILabel mlb_nowLv;
	protected UILabel mlb_expectedLv;
	protected UILabel mlb_des;
	protected UnityEngine.GameObject mobj_red;
	protected UILabel mlb_des2;
	protected UISprite msp_buffIcon;
	protected UILabel mlb_buff;
	protected UIGridContainer mgrid_getway;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_introduce = ScriptBinder.GetObject("btn_introduce") as UnityEngine.GameObject;
		mlb_levelExp = ScriptBinder.GetObject("lb_levelExp") as UILabel;
		mlb_levelValue = ScriptBinder.GetObject("lb_levelValue") as UILabel;
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mobj_bg2 = ScriptBinder.GetObject("obj_bg2") as UnityEngine.GameObject;
		mobj_free1 = ScriptBinder.GetObject("obj_free1") as UnityEngine.GameObject;
		mlb_todayNum = ScriptBinder.GetObject("lb_todayNum") as UILabel;
		mobj_exchange = ScriptBinder.GetObject("obj_exchange") as UnityEngine.GameObject;
		mlb_pro = ScriptBinder.GetObject("lb_pro") as UISprite;
		mlb_getVigor = ScriptBinder.GetObject("lb_getVigor") as UILabel;
		mtween_getVigor = ScriptBinder.GetObject("tween_getVigor") as TweenAlpha;
		mobj_lb1 = ScriptBinder.GetObject("obj_lb1") as UnityEngine.GameObject;
		mobj_tex1 = ScriptBinder.GetObject("obj_tex1") as UnityEngine.GameObject;
		mobj_tex2 = ScriptBinder.GetObject("obj_tex2") as UnityEngine.GameObject;
		mlb_nowLv = ScriptBinder.GetObject("lb_nowLv") as UILabel;
		mlb_expectedLv = ScriptBinder.GetObject("lb_expectedLv") as UILabel;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
		mobj_red = ScriptBinder.GetObject("obj_red") as UnityEngine.GameObject;
		mlb_des2 = ScriptBinder.GetObject("lb_des2") as UILabel;
		msp_buffIcon = ScriptBinder.GetObject("sp_buffIcon") as UISprite;
		mlb_buff = ScriptBinder.GetObject("lb_buff") as UILabel;
		mgrid_getway = ScriptBinder.GetObject("grid_getway") as UIGridContainer;
	}
}
