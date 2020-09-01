public partial class UIRankingCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mtog_rank;
	protected UnityEngine.GameObject mUILeaderboardPanel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtog_rank = ScriptBinder.GetObject("tog_rank") as UIToggle;
		mUILeaderboardPanel = ScriptBinder.GetObject("UILeaderboardPanel") as UnityEngine.GameObject;
	}
}
