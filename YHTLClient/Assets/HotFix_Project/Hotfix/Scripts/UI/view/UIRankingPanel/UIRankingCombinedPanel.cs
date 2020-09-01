public partial class UIRankingCombinedPanel : UIBasePanel
{
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	public enum RankPanelTye
	{
		CPT_RANK = 1,//排行榜
	}
	
	public override void Init()
	{
		base.Init();
		mbtn_close.onClick = Close;
		RegChildPanel<UIRankingPanel>((int)RankPanelTye.CPT_RANK, mUILeaderboardPanel, mtog_rank);
	}
}
