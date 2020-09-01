public partial class UITimeExpCombinedPanel : UIBasePanel
{
	protected UIEventListener mBtnShenFu;
	protected ScriptBinder mShenFuPanel;
	protected UIEventListener mBtnClose;
	protected UIToggle mTogShenFu;
	protected UIGrid mToggleGroup;
	protected UnityEngine.GameObject mShenFuRedpoint;
	protected override void _InitScriptBinder()
	{
		mBtnShenFu = ScriptBinder.GetObject("BtnShenFu") as UIEventListener;
		mShenFuPanel = ScriptBinder.GetObject("ShenFuPanel") as ScriptBinder;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogShenFu = ScriptBinder.GetObject("TogShenFu") as UIToggle;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mShenFuRedpoint = ScriptBinder.GetObject("ShenFuRedpoint") as UnityEngine.GameObject;
	}
}
