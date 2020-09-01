using UnityEngine;

public partial class UIAgreementTipsPanel : UIBasePanel
{
	public override bool ShowGaussianBlur { get => false; }
	
	public override void Init()
	{
		base.Init();
		AddCollider();
		mbtn_close.onClick = Close;
		mbtn_knew.onClick = OnClickKnew;
	}
	
	public override void Show()
	{
		base.Show();
	}

	void OnClickKnew(GameObject go)
	{
		if (go == null) return;
		HotManager.Instance.EventHandler.SendEvent(CEvent.UserAgreementRead);
		Close();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
