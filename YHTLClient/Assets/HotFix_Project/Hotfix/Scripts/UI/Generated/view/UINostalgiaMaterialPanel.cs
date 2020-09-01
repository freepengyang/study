public partial class UINostalgiaMaterialPanel : UIBasePanel
{
	protected UIGridContainer mgrid;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mlb_go;
	protected UnityEngine.GameObject mFull;
	protected override void _InitScriptBinder()
	{
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mlb_go = ScriptBinder.GetObject("lb_go") as UnityEngine.GameObject;
		mFull = ScriptBinder.GetObject("Full") as UnityEngine.GameObject;
	}
}
