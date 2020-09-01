public partial class UIStorehousePanel : UIBasePanel
{
	protected UIScrollView msc_Scroll;
	protected UIGrid mgrid_items;
	protected UnityEngine.GameObject mbtn_sort;
	protected UnityEngine.GameObject mbtn_help;
	protected UILabel mlb_Maxcount;
	protected UILabel mlb_pageNum;
	protected UIWrapContent mwrap;
	protected UnityEngine.GameObject mobj_mask;
	protected UISprite msp_btnSort;
	protected UILabel mlb_btnSort;
	protected override void _InitScriptBinder()
	{
		msc_Scroll = ScriptBinder.GetObject("sc_Scroll") as UIScrollView;
		mgrid_items = ScriptBinder.GetObject("grid_items") as UIGrid;
		mbtn_sort = ScriptBinder.GetObject("btn_sort") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mlb_Maxcount = ScriptBinder.GetObject("lb_Maxcount") as UILabel;
		mlb_pageNum = ScriptBinder.GetObject("lb_pageNum") as UILabel;
		mwrap = ScriptBinder.GetObject("wrap") as UIWrapContent;
		mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
		msp_btnSort = ScriptBinder.GetObject("sp_btnSort") as UISprite;
		mlb_btnSort = ScriptBinder.GetObject("lb_btnSort") as UILabel;
	}
}
