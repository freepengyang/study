public partial class UIAuctionBuyPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_mainTabs;
	protected UnityEngine.GameObject mobj_check;
	protected UnityEngine.GameObject mobj_subTabs;
	protected UIScrollView mscr_tabScroll;
	protected UITable mgrid_tabGrid;
	protected UIScrollView mscr_goodsScr;
	protected UIGridContainer mgrid_goodsPar;
	protected UILabel mlb_page;
	protected UnityEngine.GameObject mobj_noGoods;
	protected UnityEngine.GameObject mbtn_price;
	protected UnityEngine.GameObject mobj_arrow;
	protected UIGrid mgrid_limitPar;
	protected UnityEngine.GameObject mobj_limitCareer;
	protected UnityEngine.GameObject mobj_lvNormalEquip;
	protected UnityEngine.GameObject mobj_lvWoLongEquip;
	protected UnityEngine.GameObject mgrid_limitCareer;
	protected UnityEngine.GameObject mgrid_limitNormalLv;
	protected UnityEngine.GameObject mgrid_limitWolongLv;
	protected UIInput minput_search;
	protected UnityEngine.GameObject mbtn_search;
	protected UnityEngine.GameObject mobj_Scrdrag;
	protected UnityEngine.GameObject mobj_shield;
	protected override void _InitScriptBinder()
	{
		mobj_mainTabs = ScriptBinder.GetObject("obj_mainTabs") as UnityEngine.GameObject;
		mobj_check = ScriptBinder.GetObject("obj_check") as UnityEngine.GameObject;
		mobj_subTabs = ScriptBinder.GetObject("obj_subTabs") as UnityEngine.GameObject;
		mscr_tabScroll = ScriptBinder.GetObject("scr_tabScroll") as UIScrollView;
		mgrid_tabGrid = ScriptBinder.GetObject("grid_tabGrid") as UITable;
		mscr_goodsScr = ScriptBinder.GetObject("scr_goodsScr") as UIScrollView;
		mgrid_goodsPar = ScriptBinder.GetObject("grid_goodsPar") as UIGridContainer;
		mlb_page = ScriptBinder.GetObject("lb_page") as UILabel;
		mobj_noGoods = ScriptBinder.GetObject("obj_noGoods") as UnityEngine.GameObject;
		mbtn_price = ScriptBinder.GetObject("btn_price") as UnityEngine.GameObject;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mgrid_limitPar = ScriptBinder.GetObject("grid_limitPar") as UIGrid;
		mobj_limitCareer = ScriptBinder.GetObject("obj_limitCareer") as UnityEngine.GameObject;
		mobj_lvNormalEquip = ScriptBinder.GetObject("obj_lvNormalEquip") as UnityEngine.GameObject;
		mobj_lvWoLongEquip = ScriptBinder.GetObject("obj_lvWoLongEquip") as UnityEngine.GameObject;
		mgrid_limitCareer = ScriptBinder.GetObject("grid_limitCareer") as UnityEngine.GameObject;
		mgrid_limitNormalLv = ScriptBinder.GetObject("grid_limitNormalLv") as UnityEngine.GameObject;
		mgrid_limitWolongLv = ScriptBinder.GetObject("grid_limitWolongLv") as UnityEngine.GameObject;
		minput_search = ScriptBinder.GetObject("input_search") as UIInput;
		mbtn_search = ScriptBinder.GetObject("btn_search") as UnityEngine.GameObject;
		mobj_Scrdrag = ScriptBinder.GetObject("obj_Scrdrag") as UnityEngine.GameObject;
		mobj_shield = ScriptBinder.GetObject("obj_shield") as UnityEngine.GameObject;
	}
}
