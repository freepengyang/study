public partial class UIStrengthenCombinedPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UIToggle mTogStrong;
	protected UnityEngine.GameObject mUStrengthenPanel;
	protected UIGrid mgrid_Group;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogStrong = ScriptBinder.GetObject("TogStrong") as UIToggle;
		mUStrengthenPanel = ScriptBinder.GetObject("UStrengthenPanel") as UnityEngine.GameObject;
		mgrid_Group = ScriptBinder.GetObject("grid_Group") as UIGrid;
	}
}
