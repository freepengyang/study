public partial class UIDungeonInstacePanel : UIBasePanel
{
	protected UILabel mlb_title;
	protected UILabel mlb_killMonster;
	protected UIGrid mGrid;
	protected UnityEngine.GameObject mChallengeResult;
	protected UnityEngine.GameObject mbgtitle;
	protected UnityEngine.GameObject maward;
	protected UILabel mmissionfix;
	protected override void _InitScriptBinder()
	{
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mlb_killMonster = ScriptBinder.GetObject("lb_killMonster") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGrid;
		mChallengeResult = ScriptBinder.GetObject("ChallengeResult") as UnityEngine.GameObject;
		mbgtitle = ScriptBinder.GetObject("bgtitle") as UnityEngine.GameObject;
		maward = ScriptBinder.GetObject("award") as UnityEngine.GameObject;
		mmissionfix = ScriptBinder.GetObject("missionfix") as UILabel;
	}
}
