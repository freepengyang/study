using UnityEngine;
using System.Collections;

public class UINoticeBottomPanel : UINoticeBase {

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    #region UI
    TweenAlpha _mTweenAlpha;
    protected TweenAlpha mTweenAlpha
    {
        get
        {
            if (_mTweenAlpha == null) _mTweenAlpha = mDescriptionLb.GetComponent<TweenAlpha>();
            return _mTweenAlpha;
        }
    }
    #endregion

    protected override NoticeType _NoticeType
    {
        get { return NoticeType.Bottom; }
    }

    protected override float waitTime
    {
        get { return 1.1f; }
    }

    public override float MoveSpeed
    {
        get { return 1; }
    }

    private tip.BulletinResponse NoticeS;

    public override void Init()
    {
        base.Init();
        mTweenAlpha.onFinished.Add(new EventDelegate(OnPlayAlphaOver));
    }

    public override void Show()
    {
        base.Show();
        if (!UIPrefab.activeSelf)
            UIPrefab.SetActive(true);

        if (!isBeginPlay)
        {
            isBeginPlay = true;
            NoticeS = NoticeStr;

            if (string.IsNullOrEmpty(NoticeS.msg))
            {
                UIPrefab.SetActive(false);
            }

            mDescriptionLb.text = string.Empty;
            OnPlayMove();
        }
    }

    protected override void OnPlayMove()
    {
        if (NoticeCount > 0)
        {
            if (string.IsNullOrEmpty(mDescriptionLb.text))
            {
                ShowUIEffect(Vector3.zero, Vector3.one);
            }
            CSNoticeManager.Instance.NoticeDequeue(_NoticeType);

            mDescriptionLb.alpha = 1;

            mDescriptionLb.text = NoticeS.msg;
            mTweenPosition.duration = 1f;

            Vector3 from = Vector3.zero;
            from.Set(0, -40, 0);
            Vector3 to = Vector3.zero;
            to.Set(0, 0, 0);
            mTweenPosition.from = from;
            mTweenPosition.to = to;

            mTweenPosition.ResetToBeginning();
            mTweenPosition.PlayForward();

            mTweenAlpha.delay = 4;
            mTweenAlpha.duration = 1;
            mTweenAlpha.ResetToBeginning();
            mTweenAlpha.PlayForward();
        }
    }

    protected void OnPlayAlphaOver()
    {
        //CSNoticeManager.Instance.NoticeDequeue(_NoticeType);

        if (NoticeCount > 0)
        {
            NoticeS = NoticeStr;
            OnPlayMove();
        }
        else
        {
            isBeginPlay = false;

            ShowUIEffect(Vector3.one, Vector3.zero);
            WaitCloseUI();
        }
    }
}
