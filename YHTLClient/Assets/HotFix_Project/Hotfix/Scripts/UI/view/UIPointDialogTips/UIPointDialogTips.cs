using System;
using AssetBundles;
using UnityEngine;

public class UIPointDialogTips : UIBase
{
    private TweenScale _tweenScale;

    private TweenScale TweenScale
    {
        get { return _tweenScale ? _tweenScale : (_tweenScale = Get<TweenScale>("tipPanel")); }
    }

    private UILabel _lb_tips;

    private UILabel Lb_tips
    {
        get { return _lb_tips ? _lb_tips : (_lb_tips = Get<UILabel>("tipPanel/lb_tips")); }
    }

    private GameObject _btn_close;

    private GameObject Btn_close
    {
        get { return _btn_close ? _btn_close : (_btn_close = Get<GameObject>("tipPanel/lb_tips/btn_close")); }
    }

    private UISprite _spr_bg;

    private UISprite Spr_bg
    {
        get { return _spr_bg ? _spr_bg : (_spr_bg = Get<UISprite>("tipPanel/lb_tips/bg")); }
    }


    private int mAllCount = 0;
    private int mCount = 0;
    private Action<int> mSecondCountDownCall = null;
    private System.Action mLabCall = null;
    private System.Action mCloseCall = null;
    private Schedule _schedule;

    public override void Init()
    {
        base.Init();
        if (Btn_close != null) UIEventListener.Get(Btn_close).onClick = OnClosePanel;
        if (Lb_tips != null) UIEventListener.Get(Lb_tips.gameObject).onClick = OnTipsClick;
        TweenScale.gameObject.SetActive(false);
    }

    public void Show(string text, System.Action _closeAction, System.Action _clickAction = null)
    {
        mCloseCall = _closeAction;
        mLabCall = _clickAction;
        SetLabel(text);
        TweenScale.gameObject.SetActive(true);
        PlayTween();
    }
    
    /// <summary>
    /// 设置层级
    /// </summary>
    public void SetDepth(int depth)
    {
        if(Panel != null) Panel.depth = depth;
    }

    #region 倒计时回调

    public void StartCountDown(int countTime, Action<int> secondCallback)
    {
        if (countTime == 0) return;
        mCount = 0;
        mSecondCountDownCall = secondCallback;
        mAllCount = countTime;

        if (Timer.Instance.IsInvoking(_schedule))
        {
            Timer.Instance.CancelInvoke(_schedule);
        }

        _schedule = Timer.Instance.InvokeRepeating(0, 1, TimeCountDown);
    }

    public void CancelCountDown()
    {
        mCount = 0;
        Timer.Instance.CancelInvoke(_schedule);
    }

  #endregion

    private void SetLabel(string text)
    {
        if(Lb_tips == null) return;
        Lb_tips.text = text;
    }

    private void PlayTween()
    {
        if(TweenScale == null) return;
        TweenScale.ResetToBeginning();
        TweenScale.PlayForward();
    }
    

    private void OnClosePanel(GameObject go)
    {
        if (mCloseCall != null) mCloseCall();
    }
    private void OnTipsClick(GameObject go)
    {
        if (mLabCall != null) mLabCall();
    }
    private void TimeCountDown(Schedule schedule)
    {
        if (mCount >= mAllCount)
        {
            mCount = 0;
            Timer.Instance.CancelInvoke(_schedule);
            if (mSecondCountDownCall != null)
                mSecondCountDownCall(0);
            return;
        }

        if (mSecondCountDownCall != null)
            mSecondCountDownCall(mAllCount - mCount);
        mCount++;
    }

    public void OnReset()
    {
        if (Timer.Instance.IsInvoking(_schedule))
        {
            Timer.Instance.CancelInvoke(_schedule);
        }

        mLabCall = null;
        mCloseCall = null;
        mSecondCountDownCall = null;
    }

    protected override void OnDestroy()
    {
        OnReset();
        _tweenScale = null;
        _lb_tips = null;
        _btn_close = null;
        _spr_bg = null;
        base.OnDestroy();

    }
}