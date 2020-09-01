public partial class UIHonorInstancePanel : UIBasePanel
{
	protected UnityEngine.GameObject mlb_bossName;
	protected override void _InitScriptBinder()
	{
		mlb_bossName = ScriptBinder.GetObject("lb_bossName") as UnityEngine.GameObject;
	}
}
