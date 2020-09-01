using UnityEngine;

public class UITipsViewCenter : UITipsViewBase
{
	public override UITipsViewPool.TipsViewType TipPos
	{
		get { return UITipsViewPool.TipsViewType.Center; }
	}

	/// <summary>显示在中间的提示框 </summary>
	public override void ShowTips(string content, float timer/*, Color color*/)
	{
		base.ShowTips(content,timer/*,color*/);
		//TODO:
		if (!UIPrefab.activeSelf)
			UIPrefab.SetActive(true);
		gameObjectPos.Set(0, 200, 0);
		UIPrefabTrans.localPosition = gameObjectPos;
		_vector3.Set(0, -205, 0);
		ChildGoTrans.localPosition = _vector3;
		ChildGoTrans.localScale = Vector3.one;
		if (!msp_bg.gameObject.activeSelf)
			msp_bg.gameObject.SetActive(true);
		mlb_tips.text = content;
		//mlb_tips.color = color;

		ScriptBinder.Invoke(timer, CloseTips);
	}

	private void CloseTips()
	{
		TweenAlpha.Begin(UIPrefab, 1, 0);
		TA.SetOnFinished(() =>
		{
			TP.enabled = false;
			UITipsViewPool.Instance.PushUITipPanel(this);
		});
	}
}