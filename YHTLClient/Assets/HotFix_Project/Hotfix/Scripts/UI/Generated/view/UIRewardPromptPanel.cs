public partial class UIRewardPromptPanel : UIBase
{
	protected UIScrollView mScroll_reward;
	protected UIGrid mGrid_reward;
	protected UnityEngine.GameObject mbtn_back;
	protected UnityEngine.GameObject mobj_effect;
	protected UILabel mlb_time;
	protected override void _InitScriptBinder()
	{
		mScroll_reward = ScriptBinder.GetObject("Scroll_reward") as UIScrollView;
		mGrid_reward = ScriptBinder.GetObject("Grid_reward") as UIGrid;
		mbtn_back = ScriptBinder.GetObject("btn_back") as UnityEngine.GameObject;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
	}
}
