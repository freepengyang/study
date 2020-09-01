public partial class UISealCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mtog_seal_grade;
	protected UIToggle mtog_dreamland;
	protected ScriptBinder mSealGradePanel;
	protected ScriptBinder mDreamLandPanel;
	protected UIGrid mgrid_tab;
	protected UnityEngine.GameObject mredpoint_dreamland;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtog_seal_grade = ScriptBinder.GetObject("tog_seal_grade") as UIToggle;
		mtog_dreamland = ScriptBinder.GetObject("tog_dreamland") as UIToggle;
		mSealGradePanel = ScriptBinder.GetObject("SealGradePanel") as ScriptBinder;
		mDreamLandPanel = ScriptBinder.GetObject("DreamLandPanel") as ScriptBinder;
		mgrid_tab = ScriptBinder.GetObject("grid_tab") as UIGrid;
		mredpoint_dreamland = ScriptBinder.GetObject("redpoint_dreamland") as UnityEngine.GameObject;
	}
}
