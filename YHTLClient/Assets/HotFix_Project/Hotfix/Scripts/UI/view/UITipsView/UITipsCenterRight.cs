using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UITipsCenterRight : UITipsViewBase
{
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Base; }
    }

    public override UITipsViewPool.TipsViewType TipPos
    {
        get { return UITipsViewPool.TipsViewType.CenterRight; }
    }
    UILabel _vigorExp;
    UILabel vigorExp
    {
        get { return _vigorExp ?? (_vigorExp = Get<UILabel>("GameObject/lb_tipsVigor")); }
    }
    int type = 0;
    
    public override void ShowTips(string content, float timer/*, Color color*/)
    {
        base.ShowTips(content, timer/*, color*/);
        if (!UIPrefab.activeSelf)
            UIPrefab.SetActive(true);
        if (msp_bg.gameObject.activeSelf)
            msp_bg.gameObject.SetActive(false);
        gameObjectPos.Set(150, 110, 0);
        UIPrefabTrans.localPosition = gameObjectPos;

        _vector3.Set(0, -124, 0);
        ChildGoTrans.localPosition = _vector3;
        if (type == 1)
        {
            mlb_tips.text = content;
            //mlb_tips.color = color;
            vigorExp.text = "";
        }
        else
        {
            mlb_tips.text = "";
            vigorExp.text = content;
            //vigorExp.color = color;
        }
        ScriptBinder.Invoke(timer, TipsTimeClose);
    }

    public override void SetExpType(int _type, string content, float timer/*, Color color*/)
    {
        type = _type;
        ShowTips(content, timer/*, color*/);
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
        if (index == 0) return;
        base.Move(index);
        _vector3.Set(gameObjectPos.x, gameObjectPos.y + 31 * index, 0);
        TweenPosition.Begin(UIPrefab, 0.2f, _vector3);
    }
}
