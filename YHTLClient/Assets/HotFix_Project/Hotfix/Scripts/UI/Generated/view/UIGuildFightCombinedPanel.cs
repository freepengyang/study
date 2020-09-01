public partial class UIGuildFightCombinedPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_calendar;
	protected UIToggle mTogCalendar;
	protected UIToggle mTogAward;
	protected UIToggle mTogFightRank;
	protected UnityEngine.GameObject mbtn_award;
	protected UnityEngine.GameObject mbtn_fight_rank;
	protected UIGrid mToggleGroup;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mGuildFightPanel;
	protected UnityEngine.GameObject mGuildAwardPanel;
	protected UnityEngine.GameObject mGuildRankPanel;
	protected UnityEngine.GameObject mGuildTreasureCabinet;
	protected UnityEngine.GameObject mbtn_TreasureCabinet;
	protected UIToggle mTogTreasureCabinet;
	protected override void _InitScriptBinder()
	{
		mbtn_calendar = ScriptBinder.GetObject("btn_calendar") as UnityEngine.GameObject;
		mTogCalendar = ScriptBinder.GetObject("TogCalendar") as UIToggle;
		mTogAward = ScriptBinder.GetObject("TogAward") as UIToggle;
		mTogFightRank = ScriptBinder.GetObject("TogFightRank") as UIToggle;
		mbtn_award = ScriptBinder.GetObject("btn_award") as UnityEngine.GameObject;
		mbtn_fight_rank = ScriptBinder.GetObject("btn_fight_rank") as UnityEngine.GameObject;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mGuildFightPanel = ScriptBinder.GetObject("GuildFightPanel") as UnityEngine.GameObject;
		mGuildAwardPanel = ScriptBinder.GetObject("GuildAwardPanel") as UnityEngine.GameObject;
		mGuildRankPanel = ScriptBinder.GetObject("GuildRankPanel") as UnityEngine.GameObject;
		mGuildTreasureCabinet = ScriptBinder.GetObject("GuildTreasureCabinet") as UnityEngine.GameObject;
		mbtn_TreasureCabinet = ScriptBinder.GetObject("btn_TreasureCabinet") as UnityEngine.GameObject;
		mTogTreasureCabinet = ScriptBinder.GetObject("TogTreasureCabinet") as UIToggle;
	}
}
