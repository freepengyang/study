public partial class UIStrengthenCombinedPanel : UIBasePanel
{
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	public enum ChildType
	{
		CPT_STRONG = 1,
	}

	public override void Init()
	{
		base.Init();
		mBtnClose.onClick = Close;
		RegChildPanel<UIStrengthenPanel>((int)ChildType.CPT_STRONG, mUStrengthenPanel, mTogStrong);
        SetMoneyIds(1, 4);
    }
	public override void Show()
	{
		base.Show();
		//FunctionOpenStateChange(0, null);
	}
	private void FunctionOpenStateChange(uint id, object data)
	{
		if (mgrid_Group != null) mgrid_Group.Reposition();
	}
	public override void SelectChildPanel(int type, int subType)
	{
		if (type == (int)ChildType.CPT_STRONG)
		{
			UIStrengthenPanel p = OpenChildPanel(type) as UIStrengthenPanel;
			p.SetItemClick(subType);
		}
		else
			OpenChildPanel(type);
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();	
	}
}