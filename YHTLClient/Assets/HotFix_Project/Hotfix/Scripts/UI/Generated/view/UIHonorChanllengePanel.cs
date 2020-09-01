public partial class UIHonorChanllengePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_enter;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.Transform mtran_bossPar;
	protected UnityEngine.GameObject mobj_eff;
	protected UnityEngine.GameObject mtex_bg;
	protected UILabel mlb_leftTime;
	protected UILabel mlb_rankTitle;
	protected UILabel mlb_myRank;
	protected UnityEngine.GameObject mobj_moreRankMes;
	protected UnityEngine.GameObject mobj_rankReward;
	protected UIGrid mgrid_reward;
	protected UnityEngine.GameObject mtran_rankPar;
	protected UILabel mlb_killDes;
	protected UnityEngine.GameObject mobj_max;
	protected override void _InitScriptBinder()
	{
		mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mtran_bossPar = ScriptBinder.GetObject("tran_bossPar") as UnityEngine.Transform;
		mobj_eff = ScriptBinder.GetObject("obj_eff") as UnityEngine.GameObject;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mlb_leftTime = ScriptBinder.GetObject("lb_leftTime") as UILabel;
		mlb_rankTitle = ScriptBinder.GetObject("lb_rankTitle") as UILabel;
		mlb_myRank = ScriptBinder.GetObject("lb_myRank") as UILabel;
		mobj_moreRankMes = ScriptBinder.GetObject("obj_moreRankMes") as UnityEngine.GameObject;
		mobj_rankReward = ScriptBinder.GetObject("obj_rankReward") as UnityEngine.GameObject;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGrid;
		mtran_rankPar = ScriptBinder.GetObject("tran_rankPar") as UnityEngine.GameObject;
		mlb_killDes = ScriptBinder.GetObject("lb_killDes") as UILabel;
		mobj_max = ScriptBinder.GetObject("obj_max") as UnityEngine.GameObject;
	}
}
