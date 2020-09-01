public partial class UIRecallSecretCombinedPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UIToggle mTogSecret;
	protected UnityEngine.GameObject mUISecretAreaPanel;
	protected UIGrid mgrid_Group;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogSecret = ScriptBinder.GetObject("TogSecret") as UIToggle;
		mUISecretAreaPanel = ScriptBinder.GetObject("UISecretAreaPanel") as UnityEngine.GameObject;
		mgrid_Group = ScriptBinder.GetObject("grid_Group") as UIGrid;
	}
}
