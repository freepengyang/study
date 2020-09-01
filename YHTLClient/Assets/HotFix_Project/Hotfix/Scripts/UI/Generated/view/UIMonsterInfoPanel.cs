public partial class UIMonsterInfoPanel : UIBasePanel
{
	protected TweenAlpha mTweenAlpha;
	protected UIScrollView mScrollView;
	protected UIGridContainer mgrid_team_players;
	protected UnityEngine.Transform mteamList;
	protected override void _InitScriptBinder()
	{
		mTweenAlpha = ScriptBinder.GetObject("TweenAlpha") as TweenAlpha;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mgrid_team_players = ScriptBinder.GetObject("grid_team_players") as UIGridContainer;
		mteamList = ScriptBinder.GetObject("teamList") as UnityEngine.Transform;
	}
}
