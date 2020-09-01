public partial class UIArmRacePromptPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_bg;
	protected UISprite msp_title;
	protected UnityEngine.GameObject mobj_reward1;
	protected UnityEngine.GameObject mobj_reward2;
	protected UILabel mlb_des1;
	protected UIGrid mgrid_reward1;
	protected UnityEngine.GameObject mbtn_recharge1;
	protected UILabel mlb_num1;
	protected UnityEngine.GameObject mobj_complete1;
	protected UnityEngine.GameObject mbtn_get1;
	protected override void _InitScriptBinder()
	{
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UnityEngine.GameObject;
		msp_title = ScriptBinder.GetObject("sp_title") as UISprite;
		mobj_reward1 = ScriptBinder.GetObject("obj_reward1") as UnityEngine.GameObject;
		mobj_reward2 = ScriptBinder.GetObject("obj_reward2") as UnityEngine.GameObject;
		mlb_des1 = ScriptBinder.GetObject("lb_des1") as UILabel;
		mgrid_reward1 = ScriptBinder.GetObject("grid_reward1") as UIGrid;
		mbtn_recharge1 = ScriptBinder.GetObject("btn_recharge1") as UnityEngine.GameObject;
		mlb_num1 = ScriptBinder.GetObject("lb_num1") as UILabel;
		mobj_complete1 = ScriptBinder.GetObject("obj_complete1") as UnityEngine.GameObject;
		mbtn_get1 = ScriptBinder.GetObject("btn_get1") as UnityEngine.GameObject;
	}
}
