public partial class UIDailyActivityRewardPanel : UIBasePanel
{
	protected UIGridContainer mgrid_reward;
	protected UnityEngine.GameObject mbtn_ok;
	protected override void _InitScriptBinder()
	{
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGridContainer;
		mbtn_ok = ScriptBinder.GetObject("btn_ok") as UnityEngine.GameObject;
	}
}
