using UnityEngine;

public class UITipsViewMoveUp : UITipsViewBase
{
	private TweenPosition mChildTP;

	public TweenPosition ChildTP
	{
		get
		{
			if (mChildTP == null)
			{
				mChildTP = ChildGo.GetComponent<TweenPosition>();
				if (mChildTP == null) mChildTP = ChildGo.AddComponent<TweenPosition>();
			}

			return mChildTP;
		}
	}

	private TweenScale mChildTS;

	public TweenScale ChildTS
	{
		get
		{
			if (mChildTS == null)
			{
				mChildTS = ChildGo.GetComponent<TweenScale>();
				if (mChildTS == null) mChildTS = ChildGo.AddComponent<TweenScale>();
			}

			return mChildTS;
		}
	}

	public override UITipsViewPool.TipsViewType TipPos
	{
		get { return UITipsViewPool.TipsViewType.MoveUp; }
	}


	/// <summary>显示在中间向上飘的提示框 </summary>
	public override void ShowTips(string content, float timer/*, Color color*/)
	{
		base.ShowTips(content,timer/*,color*/);
		if (!UIPrefab.activeSelf)
			UIPrefab.SetActive(true);
		gameObjectPos = Vector3.zero;
		UIPrefabTrans.localPosition = gameObjectPos;

		_vector3.Set(0, 80, 0);
		ChildGoTrans.localPosition = _vector3;
		if (!msp_bg.gameObject.activeSelf)
			msp_bg.gameObject.SetActive(true);
		

		//msp_bg.width = 230;

		_vector3.Set(0.5f, 0.5f, 0.5f);
		ChildGoTrans.localScale = _vector3;

		TweenScale.Begin(ChildGo, 0.1f, Vector3.one);
		_vector3.Set(0, ChildGoTrans.localPosition.y + 20f, 0);
		TweenPosition.Begin(ChildGo, 0.1f, _vector3);

        mlb_tips.text = content;
		//mlb_tips.color = color;
		ScriptBinder.Invoke(1, CloseCenterTips);
	}

	private void CloseCenterTips()
	{
		_vector3.Set(0, 120, 0);
		TweenPosition.Begin(ChildGo, 1f, _vector3);
		TweenAlpha.Begin(UIPrefab, 1, 0);
		TA.SetOnFinished(() =>
		{
			ChildTP.enabled = false;
			ChildTS.enabled = false;
			TP.enabled = false;
			UITipsViewPool.Instance.PushUITipPanel(this);
		});
	}

	public override void Move(int index)
	{
		if(index == 0) return;
		_vector3.Set(0, gameObjectPos.y + 31 * index, 0);
		TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
	}

	public override void Dispose()
	{
		base.Dispose();
		ChildTP.ResetToBeginning();
		ChildTP.enabled = false;
		ChildTS.ResetToBeginning();
		ChildTS.enabled = false;
	}
}