public partial class UIWelfareActivationCodePanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_bg;
	protected UIInput minput;
	protected UnityEngine.GameObject mbtn_get;
	protected override void _InitScriptBinder()
	{
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		minput = ScriptBinder.GetObject("input") as UIInput;
		mbtn_get = ScriptBinder.GetObject("btn_get") as UnityEngine.GameObject;
	}
}
