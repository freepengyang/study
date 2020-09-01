public partial class UIServerActivitySealPanel : UIBasePanel
{
	protected UILabel mlb_countdown;
	protected UnityEngine.GameObject mtex_title;
	protected UILabel mlb_lvLimit;
	protected UIGrid mgrid_allreward;
	protected UISprite mbtn_getReward;
	protected UILabel mlb_getReward;
	protected UIGrid mgrid_rank;
	protected UnityEngine.GameObject mobj_allRewardGet;
	protected override void _InitScriptBinder()
	{
		mlb_countdown = ScriptBinder.GetObject("lb_countdown") as UILabel;
		mtex_title = ScriptBinder.GetObject("tex_title") as UnityEngine.GameObject;
		mlb_lvLimit = ScriptBinder.GetObject("lb_lvLimit") as UILabel;
		mgrid_allreward = ScriptBinder.GetObject("grid_allreward") as UIGrid;
		mbtn_getReward = ScriptBinder.GetObject("btn_getReward") as UISprite;
		mlb_getReward = ScriptBinder.GetObject("lb_getReward") as UILabel;
		mgrid_rank = ScriptBinder.GetObject("grid_rank") as UIGrid;
		mobj_allRewardGet = ScriptBinder.GetObject("obj_allRewardGet") as UnityEngine.GameObject;
	}
}
