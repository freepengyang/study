public partial class UIWelfareActivityPanel : UIBasePanel
{
	protected UnityEngine.Transform mtrans_panelParent;
	protected UIScrollView mscr_tabs;
	protected UIGridContainer mgrid_tabs;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mtrans_panelParent = ScriptBinder.GetObject("trans_panelParent") as UnityEngine.Transform;
		mscr_tabs = ScriptBinder.GetObject("scr_tabs") as UIScrollView;
		mgrid_tabs = ScriptBinder.GetObject("grid_tabs") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
