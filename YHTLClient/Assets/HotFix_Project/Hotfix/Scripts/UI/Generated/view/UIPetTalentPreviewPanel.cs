public partial class UIPetTalentPreviewPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_panelRight;
	protected UISprite msp_rightBg;
	protected UIWrapContent mwrap_left;
	protected UITable mTable_right;
	protected UnityEngine.GameObject mtableChild;
	protected UnityEngine.GameObject msp_scroll;
	protected UIScrollView mscroll_left;
	protected override void _InitScriptBinder()
	{
		mobj_panelRight = ScriptBinder.GetObject("obj_panelRight") as UnityEngine.GameObject;
		msp_rightBg = ScriptBinder.GetObject("sp_rightBg") as UISprite;
		mwrap_left = ScriptBinder.GetObject("wrap_left") as UIWrapContent;
		mTable_right = ScriptBinder.GetObject("Table_right") as UITable;
		mtableChild = ScriptBinder.GetObject("tableChild") as UnityEngine.GameObject;
		msp_scroll = ScriptBinder.GetObject("sp_scroll") as UnityEngine.GameObject;
		mscroll_left = ScriptBinder.GetObject("scroll_left") as UIScrollView;
	}
}
