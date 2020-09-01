public partial class UIHonorResultPromptPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_fail;
	protected UnityEngine.GameObject mobj_success;
	protected UnityEngine.GameObject mobj_bg;
	protected UIGrid mgrid_reward;
	protected UnityEngine.GameObject mobj_failDes;
	protected UnityEngine.GameObject mbtn_back;
	protected UnityEngine.GameObject mbtn_next;
	protected UILabel mlb_next;
	protected UnityEngine.GameObject mobj_effect;
	protected override void _InitScriptBinder()
	{
		mobj_fail = ScriptBinder.GetObject("obj_fail") as UnityEngine.GameObject;
		mobj_success = ScriptBinder.GetObject("obj_success") as UnityEngine.GameObject;
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGrid;
		mobj_failDes = ScriptBinder.GetObject("obj_failDes") as UnityEngine.GameObject;
		mbtn_back = ScriptBinder.GetObject("btn_back") as UnityEngine.GameObject;
		mbtn_next = ScriptBinder.GetObject("btn_next") as UnityEngine.GameObject;
		mlb_next = ScriptBinder.GetObject("lb_next") as UILabel;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
	}
}
