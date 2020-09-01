using UnityEngine;

public class UITipsViewLeftDown : UITipsViewBase
{
	public override UILayerType PanelLayerType
	{
		get { return UILayerType.Base; }
	}

	public override UITipsViewPool.TipsViewType TipPos
	{
		get { return UITipsViewPool.TipsViewType.LeftDown; }
	}

	//public long time;
	public override void ShowTips(string content, float timer/*, Color color*/)
	{
		base.ShowTips(content,timer/*,color*/);
		if (!UIPrefab.activeSelf)
			UIPrefab.SetActive(true);
		if (msp_bg.gameObject.activeSelf)
			msp_bg.gameObject.SetActive(false);
        UIPrefabTrans.localPosition = Vector3.zero;
		_vector3.Set(ChildGoTrans.localPosition.x, -260, 0);
		ChildGoTrans.localPosition = _vector3;
		mlb_tips.text = content;
		ScriptBinder.Invoke(timer, TipsTimeClose);
	}

	private void TipsTimeClose()
	{
		TA.SetOnFinished(() =>
		{
			TP.enabled = false;
			UITipsViewPool.Instance.PushUITipPanel(this);
		});
		TweenAlpha.Begin(UIPrefab, 1, 0);
	}

	
	public void MoveUpLeftDown(int index)
	{
		_vector3.Set(-560, (-85 + 31 * index), 0);
		TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
	}

	public override void Move(int index)
	{
		if(index == 0) return;
		base.Move(index);
		//Debug.Log("g::::" + gameObjectPos.y + "||" + 22 * index + "||" + UIPrefab.transform.localPosition);
		_vector3.Set(gameObjectPos.x, gameObjectPos.y + 22 * index, 0);
		TweenPosition.Begin(UIPrefab, 0.3f, _vector3);
	}

	public override void OnRecycle()
	{
		base.OnRecycle();
		//因为频繁的刷新tip 导致tip还没有来的及隐藏就直接从池子里取出 ,所以需要在回收的时候手动取消定时器
		ScriptBinder.StopInvoke();
		//mlb_tips.pivot = UIWidget.Pivot.Center;
	}
}