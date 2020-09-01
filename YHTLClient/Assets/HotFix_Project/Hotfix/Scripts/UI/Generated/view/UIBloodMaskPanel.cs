public partial class UIBloodMaskPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_mask;
	protected override void _InitScriptBinder()
	{
		mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
	}
}
