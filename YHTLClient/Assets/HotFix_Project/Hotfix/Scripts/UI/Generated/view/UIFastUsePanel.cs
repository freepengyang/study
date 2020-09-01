public partial class UIFastUsePanel : UIBasePanel
{
	protected UIScrollView mscr_items;
	protected UIGridContainer mgrid_items;
	protected UISprite msp_bg;
	protected UnityEngine.GameObject mobj_shield;
	protected override void _InitScriptBinder()
	{
		mscr_items = ScriptBinder.GetObject("scr_items") as UIScrollView;
		mgrid_items = ScriptBinder.GetObject("grid_items") as UIGridContainer;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mobj_shield = ScriptBinder.GetObject("obj_shield") as UnityEngine.GameObject;
	}
}
