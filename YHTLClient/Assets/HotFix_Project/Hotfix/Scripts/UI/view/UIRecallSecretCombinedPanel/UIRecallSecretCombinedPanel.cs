public partial class UIRecallSecretCombinedPanel : UIBasePanel
{
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	public enum ChildType
	{
		CPT_RECALL = 1,//ªÿ“‰√ÿæ≥
	}

	public override void Init()
	{
		base.Init();
		mBtnClose.onClick = Close;
		RegChildPanel<UISecretAreaPanel>((int)ChildType.CPT_RECALL, mUISecretAreaPanel, mTogSecret);
		SetMoneyIds(1, 4);
	}
	public override void Show()
	{
		base.Show();
		FunctionOpenStateChange(0, null);
	}
	private void FunctionOpenStateChange(uint id, object data)
	{
		if (mgrid_Group != null) mgrid_Group.Reposition();
	}
	//public override void SelectChildPanel(int type, int subType)
	//{
	//	if (type == (int)ChildType.CPT_RECALL)
	//	{
	//		UISecretAreaPanel p = OpenChildPanel(type) as UISecretAreaPanel;
	//		p.SetItemClick(subType);
	//	}
	//	else
	//		OpenChildPanel(type);
	//}
	protected override void OnDestroy()
	{
		base.OnDestroy();	
	}
}