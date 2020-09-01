using UnityEngine;
using System.Collections.Generic;
using System;

public class UITipsViewDown : UITipsViewBase
{
	public override UITipsViewPool.TipsViewType TipPos
	{
		get { return UITipsViewPool.TipsViewType.Down; }
	}

	//显示tips
	public override void ShowTips(string content, float timer/*, Color color*/)
	{
		base.ShowTips(content,timer/*,color*/);
		if (!UIPrefab.activeSelf)
			UIPrefab.SetActive(true);
		UIPrefabTrans.localPosition = Vector3.zero;
		gameObjectPos = Vector3.zero;
		if (!msp_bg.gameObject.activeSelf)
			msp_bg.gameObject.SetActive(true);
		//msp_bg.width = 540;
		_vector3.Set(0, -122, 0);
		ChildGoTrans.localPosition = _vector3;

		mlb_tips.text = content;
		//mlb_tips.color = color;
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

	public void MoveUp(int index)
	{
		_vector3.Set(0, (31 * index), 0);
		TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
	}

	public override void Move(int index)
	{
		if(index == 0) return;
		_vector3.Set(gameObjectPos.x, gameObjectPos.y + 31 * index, 0);
		TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
	}
}