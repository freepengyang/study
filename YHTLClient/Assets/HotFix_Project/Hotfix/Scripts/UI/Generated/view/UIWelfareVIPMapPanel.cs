public partial class UIWelfareVIPMapPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbanner23;
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject mbtn_vip;
	protected UIGrid mGrid;
	protected override void _InitScriptBinder()
	{
		mbanner23 = ScriptBinder.GetObject("banner23") as UnityEngine.GameObject;
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mbtn_vip = ScriptBinder.GetObject("btn_vip") as UnityEngine.GameObject;
		mGrid = ScriptBinder.GetObject("Grid") as UIGrid;
	}
}
