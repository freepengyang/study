public partial class UISeekTreasureWarehousePanel : UIBasePanel
{
	protected UIGridContainer mgrid_item;
	protected UIGridContainer mgrid_page;
	protected UIEventListener mbtn_useExp;
	protected UIEventListener mbtn_recEquip;
	protected UIEventListener mbtn_get;
	protected UIScrollView mScrollView;
	protected UIEventListener mDragScrollView;
	protected override void _InitScriptBinder()
	{
		mgrid_item = ScriptBinder.GetObject("grid_item") as UIGridContainer;
		mgrid_page = ScriptBinder.GetObject("grid_page") as UIGridContainer;
		mbtn_useExp = ScriptBinder.GetObject("btn_useExp") as UIEventListener;
		mbtn_recEquip = ScriptBinder.GetObject("btn_recEquip") as UIEventListener;
		mbtn_get = ScriptBinder.GetObject("btn_get") as UIEventListener;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mDragScrollView = ScriptBinder.GetObject("DragScrollView") as UIEventListener;
	}
}
