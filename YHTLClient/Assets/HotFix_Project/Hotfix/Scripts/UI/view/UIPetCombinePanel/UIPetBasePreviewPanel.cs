public partial class UIPetBasePreviewPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		AddCollider();
	}
	
	public override void Show()
	{
		base.Show();
		RefreshUI();
	}
	private void RefreshUI()
	{
		int value;
		string name;
		UILabel lb_name, lb_value;
		ScriptBinder tempBinder;
		mGrid.MaxCount = ZhanHunSuitTableManager.Instance.array.gItem.id2offset.Count;
		for(int i=0;i<mGrid.MaxCount;i++)
		{
			tempBinder = mGrid.controlList[i].GetComponent<ScriptBinder>();
			lb_name = tempBinder.GetObject("lb_name") as UILabel;
			lb_value = tempBinder.GetObject("lb_value") as UILabel;
			name = ZhanHunSuitTableManager.Instance.GetZhanHunSuitName(i+1);
			value = ZhanHunSuitTableManager.Instance.GetZhanHunSuitMaxLevel(i + 1);
			lb_name.text = name.BBCode(ColorType.SecondaryText);
			lb_value.text = CSString.Format(1744, value).BBCode(ColorType.SecondaryText);
		}
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
