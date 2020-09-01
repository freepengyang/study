public partial class UIMapCombinePanel
{
	protected UIEventListener mbtn_map;
	protected UIEventListener mbtn_deliver;
	protected UIGrid mToggleGroup;
	protected UIToggle mmapToggle;
	protected UIToggle mdeliverToggle;
	protected UnityEngine.GameObject mUIMapPanel;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mUIMapTransferPanel;
	protected override void _InitScriptBinder()
	{
		mbtn_map = ScriptBinder.GetObject("btn_map") as UIEventListener;
		mbtn_deliver = ScriptBinder.GetObject("btn_deliver") as UIEventListener;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mmapToggle = ScriptBinder.GetObject("mapToggle") as UIToggle;
		mdeliverToggle = ScriptBinder.GetObject("deliverToggle") as UIToggle;
		mUIMapPanel = ScriptBinder.GetObject("UIMapPanel") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mUIMapTransferPanel = ScriptBinder.GetObject("UIMapTransferPanel") as UnityEngine.GameObject;
	}
}
