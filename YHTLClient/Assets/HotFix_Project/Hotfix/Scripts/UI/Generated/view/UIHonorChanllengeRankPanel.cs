public partial class UIHonorChanllengeRankPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UILabel mlb_title;
	protected UIScrollView mscr_show;
	protected UIGridContainer mgrid_show;
	protected UnityEngine.Transform mtrans_tabsPar;
	protected UILabel mlb_myRank;
	protected UILabel mlb_myLv;
	protected UILabel mlb_goalName;
	protected UILabel mlb_goalDes;
	protected UnityEngine.GameObject mobj_rewardDes;
	protected UnityEngine.GameObject mobj_arrow;
	protected UIScrollBar mscrbar_scr;
	protected UILabel mlb_noRank;
	protected UnityEngine.GameObject mobj_desPar;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mscr_show = ScriptBinder.GetObject("scr_show") as UIScrollView;
		mgrid_show = ScriptBinder.GetObject("grid_show") as UIGridContainer;
		mtrans_tabsPar = ScriptBinder.GetObject("trans_tabsPar") as UnityEngine.Transform;
		mlb_myRank = ScriptBinder.GetObject("lb_myRank") as UILabel;
		mlb_myLv = ScriptBinder.GetObject("lb_myLv") as UILabel;
		mlb_goalName = ScriptBinder.GetObject("lb_goalName") as UILabel;
		mlb_goalDes = ScriptBinder.GetObject("lb_goalDes") as UILabel;
		mobj_rewardDes = ScriptBinder.GetObject("obj_rewardDes") as UnityEngine.GameObject;
		mobj_arrow = ScriptBinder.GetObject("obj_arrow") as UnityEngine.GameObject;
		mscrbar_scr = ScriptBinder.GetObject("scrbar_scr") as UIScrollBar;
		mlb_noRank = ScriptBinder.GetObject("lb_noRank") as UILabel;
		mobj_desPar = ScriptBinder.GetObject("obj_desPar") as UnityEngine.GameObject;
	}
}
