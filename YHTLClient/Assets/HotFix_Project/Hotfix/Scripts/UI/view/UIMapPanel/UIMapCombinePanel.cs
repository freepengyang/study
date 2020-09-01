public partial class UIMapCombinePanel : UIBasePanel
{
	public enum MapPanelType
	{
		None = 0,
		Map = 1,
		Deliver = 2,
	}

	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}

	public override void Init()
	{
		base.Init();
        SetMoneyIds(1, 4);
        mbtn_close.onClick = this.Close;
		RegChildPanel<UIMapPanel>((int)MapPanelType.Map,mUIMapPanel.gameObject, mmapToggle);
		RegChildPanel<UIMapTransferPanel>((int)MapPanelType.Deliver,mUIMapTransferPanel.gameObject, mdeliverToggle);
		
	}
}
