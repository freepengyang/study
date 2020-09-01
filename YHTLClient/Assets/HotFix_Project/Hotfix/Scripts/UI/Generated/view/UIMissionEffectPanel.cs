public partial class UIMissionEffectPanel : UIBasePanel
{
	protected UnityEngine.GameObject mmissionEffect;
	protected override void _InitScriptBinder()
	{
		mmissionEffect = ScriptBinder.GetObject("missionEffect") as UnityEngine.GameObject;
	}
}
