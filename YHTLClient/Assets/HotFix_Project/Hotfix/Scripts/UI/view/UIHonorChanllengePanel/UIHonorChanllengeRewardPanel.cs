using UnityEngine;

public partial class UIHonorChanllengeRewardPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
	}
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
    #region µã»÷ÊÂ¼þ
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIHonorChanllengeRewardPanel>();
    }
    #endregion
}
