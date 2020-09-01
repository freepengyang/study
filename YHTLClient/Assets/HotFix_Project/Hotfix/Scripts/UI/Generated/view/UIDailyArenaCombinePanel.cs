public partial class UIDailyArenaCombinePanel : UIBasePanel
{
	protected UIScrollView mscr_tabs;
	protected UIGridContainer mgrid_tabs;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mobj_arena;
	protected UILabel mlb_des1;
	protected UILabel mlb_des2;
	protected UnityEngine.GameObject mtex_titleBg;
	protected UIScrollView mscr_tasks;
	protected UIWrapContent mwrap_tasks;
	protected UnityEngine.GameObject mobj_arrow;
	protected UnityEngine.GameObject mbtn_gift;
	protected UILabel mlb_gift;
	protected UnityEngine.GameObject msp_bubble;
	protected UnityEngine.GameObject mobj_eff;
	protected UnityEngine.GameObject mobj_bubble;
	protected UnityEngine.GameObject mbtn_introduce;
	protected override void _InitScriptBinder()
	{
		mscr_tabs = ScriptBinder.GetObject("scr_tabs") as UIScrollView;
		mgrid_tabs = ScriptBinder.GetObject("grid_tabs") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mobj_arena = ScriptBinder.GetObject("obj_arena") as UnityEngine.GameObject;
		mlb_des1 = ScriptBinder.GetObject("lb_des1") as UILabel;
		mlb_des2 = ScriptBinder.GetObject("lb_des2") as UILabel;
		mtex_titleBg = ScriptBinder.GetObject("tex_titleBg") as UnityEngine.GameObject;
		mscr_tasks = ScriptBinder.GetObject("scr_tasks") as UIScrollView;
		mwrap_tasks = ScriptBinder.GetObject("wrap_tasks") as UIWrapContent;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mbtn_gift = ScriptBinder.GetObject("btn_gift") as UnityEngine.GameObject;
		mlb_gift = ScriptBinder.GetObject("lb_gift") as UILabel;
		msp_bubble = ScriptBinder.GetObject("sp_bubble") as UnityEngine.GameObject;
		mobj_eff = ScriptBinder.GetObject("obj_eff") as UnityEngine.GameObject;
		mobj_bubble = ScriptBinder.GetObject("obj_bubble") as UnityEngine.GameObject;
		mbtn_introduce = ScriptBinder.GetObject("btn_introduce") as UnityEngine.GameObject;
	}
}
