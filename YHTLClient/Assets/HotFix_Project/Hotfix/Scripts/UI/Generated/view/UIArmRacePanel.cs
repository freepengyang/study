public partial class UIArmRacePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UIScrollView mscr_armScr;
	protected UIGridContainer mgrid_armGrid;
	protected UnityEngine.GameObject mobj_con;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mobj_helpRedpoint;
	protected UnityEngine.GameObject mbtn_gift;
	protected UnityEngine.GameObject mobj_giftRedPoint;
	protected UIScrollView mscr_taskScr;
	protected UIGridContainer mgrid_taskGrid;
	protected UILabel mlb_help;
	protected UnityEngine.GameObject mobj_arrow;
	protected UIScrollBar mscrollBar;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mscr_armScr = ScriptBinder.GetObject("scr_armScr") as UIScrollView;
		mgrid_armGrid = ScriptBinder.GetObject("grid_armGrid") as UIGridContainer;
		mobj_con = ScriptBinder.GetObject("obj_con") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mobj_helpRedpoint = ScriptBinder.GetObject("obj_helpRedpoint") as UnityEngine.GameObject;
		mbtn_gift = ScriptBinder.GetObject("btn_gift") as UnityEngine.GameObject;
		mobj_giftRedPoint = ScriptBinder.GetObject("obj_giftRedPoint") as UnityEngine.GameObject;
		mscr_taskScr = ScriptBinder.GetObject("scr_taskScr") as UIScrollView;
		mgrid_taskGrid = ScriptBinder.GetObject("grid_taskGrid") as UIGridContainer;
		mlb_help = ScriptBinder.GetObject("lb_help") as UILabel;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
	}
}
