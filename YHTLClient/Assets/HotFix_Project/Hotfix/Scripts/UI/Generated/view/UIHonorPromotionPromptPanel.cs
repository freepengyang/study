public partial class UIHonorPromotionPromptPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_bg;
	protected UILabel mlb_des1;
	protected UILabel mlb_des2;
	protected UnityEngine.GameObject mobj_effect;
	protected override void _InitScriptBinder()
	{
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		mlb_des1 = ScriptBinder.GetObject("lb_des1") as UILabel;
		mlb_des2 = ScriptBinder.GetObject("lb_des2") as UILabel;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
	}
}
