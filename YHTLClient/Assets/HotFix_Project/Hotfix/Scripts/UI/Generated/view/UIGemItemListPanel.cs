public partial class UIGemItemListPanel : UIBasePanel
{
	protected UIGridContainer mgrid;
	protected UnityEngine.GameObject mbtn_close;
	protected override void _InitScriptBinder()
	{
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
	}
}
