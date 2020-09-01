public partial class UIHonorChanllengeRewardPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
	}
}
