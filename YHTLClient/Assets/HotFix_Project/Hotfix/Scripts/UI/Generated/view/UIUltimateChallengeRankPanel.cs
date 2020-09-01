public partial class UIUltimateChallengeRankPanel : UIBasePanel
{
	protected UnityEngine.GameObject maward;
	protected UnityEngine.GameObject mrank;
	protected UIScrollView mawardScrollView;
	protected UIScrollView mrankScrollView;
	protected UIGridContainer mawardGrid;
	protected UIGridContainer mrankGrid;
	protected UnityEngine.GameObject mrankFix;
	protected UILabel mlb_myRank;
	protected UILabel mlb_myLevel;
	protected UIEventListener mbtn_battleRank;
	protected UIEventListener mbtn_rankAward;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mbtn_next;
	protected override void _InitScriptBinder()
	{
		maward = ScriptBinder.GetObject("award") as UnityEngine.GameObject;
		mrank = ScriptBinder.GetObject("rank") as UnityEngine.GameObject;
		mawardScrollView = ScriptBinder.GetObject("awardScrollView") as UIScrollView;
		mrankScrollView = ScriptBinder.GetObject("rankScrollView") as UIScrollView;
		mawardGrid = ScriptBinder.GetObject("awardGrid") as UIGridContainer;
		mrankGrid = ScriptBinder.GetObject("rankGrid") as UIGridContainer;
		mrankFix = ScriptBinder.GetObject("rankFix") as UnityEngine.GameObject;
		mlb_myRank = ScriptBinder.GetObject("lb_myRank") as UILabel;
		mlb_myLevel = ScriptBinder.GetObject("lb_myLevel") as UILabel;
		mbtn_battleRank = ScriptBinder.GetObject("btn_battleRank") as UIEventListener;
		mbtn_rankAward = ScriptBinder.GetObject("btn_rankAward") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_next = ScriptBinder.GetObject("btn_next") as UnityEngine.GameObject;
	}
}
