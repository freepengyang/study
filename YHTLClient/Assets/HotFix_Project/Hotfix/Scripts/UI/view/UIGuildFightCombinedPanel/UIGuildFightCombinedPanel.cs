public partial class UIGuildFightCombinedPanel : UIBasePanel
{
	public enum ChildPanelType
	{
		CPT_FIGHT = 1,
		CPT_TreasureCabinet = 2,
		CPT_AWARD = 3,
		CPT_RANK_LIST = 4,
	}

    public override void Init()
	{
		base.Init();

		PanelTweenType = PrefabTweenType.FirstPanel;
		mbtn_close.onClick = this.Close;
		RegChildPanel<UIGuildFightPanel>((int)ChildPanelType.CPT_FIGHT, mGuildFightPanel, mTogCalendar);
		RegChildPanel<UIGuildTreasureCabinetPanel>((int)ChildPanelType.CPT_TreasureCabinet, mGuildTreasureCabinet, mTogTreasureCabinet);
		RegChildPanel<UIGuildAwardPanel>((int)ChildPanelType.CPT_AWARD, mGuildAwardPanel, mTogAward);
		RegChildPanel<UIGuildRankPanel>((int)ChildPanelType.CPT_RANK_LIST, mGuildRankPanel, mTogFightRank);
	}
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
