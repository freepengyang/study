public partial class UITotemPanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_bg;
	protected override void _InitScriptBinder()
	{
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
	}
}
