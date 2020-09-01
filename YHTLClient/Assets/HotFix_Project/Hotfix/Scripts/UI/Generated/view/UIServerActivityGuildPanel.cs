public partial class UIServerActivityGuildPanel : UIBasePanel
{
	protected UIEventListener mbtn_go;
	protected UnityEngine.GameObject mobj_banner16;
	protected override void _InitScriptBinder()
	{
		mbtn_go = ScriptBinder.GetObject("btn_go") as UIEventListener;
		mobj_banner16 = ScriptBinder.GetObject("obj_banner16") as UnityEngine.GameObject;
	}
}
