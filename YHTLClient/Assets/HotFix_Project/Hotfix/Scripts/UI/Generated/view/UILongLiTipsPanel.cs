public partial class UILongLiTipsPanel : UIBasePanel
{
	protected UIScrollView mscr_base;
	protected UIGridContainer mgrid_base;
	protected UnityEngine.GameObject mobj_arrow;
	protected UILabel mlb_intenDes;
	protected UITable mtable_des;
	protected UIScrollView mscr_des;
	protected UIScrollBar mscrBar;
	protected override void _InitScriptBinder()
	{
		mscr_base = ScriptBinder.GetObject("scr_base") as UIScrollView;
		mgrid_base = ScriptBinder.GetObject("grid_base") as UIGridContainer;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mlb_intenDes = ScriptBinder.GetObject("lb_intenDes") as UILabel;
		mtable_des = ScriptBinder.GetObject("table_des") as UITable;
		mscr_des = ScriptBinder.GetObject("scr_des") as UIScrollView;
		mscrBar = ScriptBinder.GetObject("scrBar") as UIScrollBar;
	}
}
