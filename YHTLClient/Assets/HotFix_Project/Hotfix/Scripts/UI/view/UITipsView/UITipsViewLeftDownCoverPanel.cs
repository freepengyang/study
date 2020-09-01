using UnityEngine;
public class UITipsViewLeftDownCoverPanel : UITipsViewBase
{
    public override UITipsViewPool.TipsViewType TipPos
    {
        get { return UITipsViewPool.TipsViewType.LeftDownCoverPanel; }
    }
    public override void ShowTips(string content, float timer/*, Color color*/)
    {
        base.ShowTips(content, timer/*, color*/);
        if (!UIPrefab.activeSelf)
            UIPrefab.SetActive(true);
        if (!msp_bg.gameObject.activeSelf)
            msp_bg.gameObject.SetActive(true);
        //msp_bg.width = 230;
        gameObjectPos.Set(0, 50, 0);
        UIPrefabTrans.localPosition = gameObjectPos;
        _vector3.Set(0, 50, 0);
        ChildGoTrans.localPosition = _vector3;
        mlb_tips.text = content;
        //mlb_tips.color = color;
        ScriptBinder.Invoke(1, CloseCenterTips);
    }

    private void CloseCenterTips()
    {
       TA.SetOnFinished(() =>
		{
			TP.enabled = false;
			UITipsViewPool.Instance.PushUITipPanel(this);
		});
		TweenAlpha.Begin(UIPrefab, 1, 0);
    }

    public override void Move(int index)
    {
        if (index == 0) return;
        base.Move(index);
        _vector3.Set(gameObjectPos.x, gameObjectPos.y + 31 * index, 0);
        TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
    }


}