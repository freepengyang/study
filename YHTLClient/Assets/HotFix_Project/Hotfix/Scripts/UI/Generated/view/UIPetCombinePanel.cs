public partial class UIPetCombinePanel : UIBasePanel
{
	protected UIEventListener mCloseBtn;
	protected UIGrid mGrid;
	protected UnityEngine.GameObject mView;
	protected override void _InitScriptBinder()
	{
		mCloseBtn = ScriptBinder.GetObject("CloseBtn") as UIEventListener;
		mGrid = ScriptBinder.GetObject("Grid") as UIGrid;
		mView = ScriptBinder.GetObject("View") as UnityEngine.GameObject;
	}
}
