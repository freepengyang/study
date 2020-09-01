public partial class UIUndergroundTreasureInstancePanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UnityEngine.GameObject mobj_time;
	protected UnityEngine.GameObject mobj_Finish;
	protected UnityEngine.GameObject mobj_protectTime;
	protected UILabel mlb_protectTime;
	protected UnityEngine.GameObject mlb_hint;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mobj_time = ScriptBinder.GetObject("obj_time") as UnityEngine.GameObject;
		mobj_Finish = ScriptBinder.GetObject("obj_Finish") as UnityEngine.GameObject;
		mobj_protectTime = ScriptBinder.GetObject("obj_protectTime") as UnityEngine.GameObject;
		mlb_protectTime = ScriptBinder.GetObject("lb_protectTime") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UnityEngine.GameObject;
	}
}
