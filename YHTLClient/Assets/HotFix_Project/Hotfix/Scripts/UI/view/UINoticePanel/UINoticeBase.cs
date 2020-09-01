using UnityEngine;
using System.Collections;

public class UINoticeBase : UIBase
{
    #region   UI控件
    protected TweenPosition _mTweenPosition;
    protected virtual TweenPosition mTweenPosition
    {
        get
        {
            if (_mTweenPosition == null) _mTweenPosition = mDescriptionLb.GetComponent<TweenPosition>();
            return _mTweenPosition;
        }
    }
    protected UILabel _mDescriptionLb;
    protected virtual UILabel mDescriptionLb
    {
        get
        {
            if (_mDescriptionLb == null) _mDescriptionLb = Get<UILabel>("Root/Scroll View/info");
            return _mDescriptionLb;
        }
    }

    protected UIPanel _mUIPanel;
    protected virtual UIPanel mUIPanel
    {
        get
        {
            if (_mUIPanel == null) _mUIPanel = Panel;
            return _mUIPanel;
        }
    }

    protected TweenScale _mTweenScale;
    protected virtual TweenScale mTweenScale
    {
        get
        {
            if (_mTweenScale == null) _mTweenScale = Get<TweenScale>("Root/window/background");
            return _mTweenScale;
        }
    }

    #endregion


    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.TopWindow;
        }
    }

    protected float mMoveSpeed = 140f;

    public virtual float MoveSpeed
    {
        get { return mMoveSpeed; }
    }

    protected NoticeType mNoticeType;
    
    protected virtual NoticeType _NoticeType
    {
        get { return mNoticeType; }
        set { mNoticeType = value; }
    }

    public tip.BulletinResponse NoticeStr
    {
        get {
            return CSNoticeManager.Instance.NoticePeek(_NoticeType);
        }
    }

    protected float mwaitTime = 0.3f;
    protected virtual float waitTime
    {
        get { return mwaitTime; }
        set { mwaitTime = value; }
    }

    protected int NoticeCount
    {
        get { return CSNoticeManager.Instance.NoticeCount(_NoticeType); }
    }

    protected bool isBeginPlay;
    
    /// <summary>
    /// 开始播放
    /// </summary>
    protected virtual void OnPlayMove()
    {

    }

    /// <summary>
    /// 播放完毕
    /// </summary>
    protected virtual void OnPlayOver()
    {

    }

    protected virtual float GetLabelWidth()
    {
        return mDescriptionLb.localSize.x * mDescriptionLb.cachedTransform.localScale.x;
    }


    protected virtual void ShowUIEffect(Vector3 from, Vector3 to)
    {
        if (mTweenScale == null) return;
        mTweenScale.from = from;
        mTweenScale.to = to;
        mTweenScale.duration = 0.2f;
        mTweenScale.ResetToBeginning();
        mTweenScale.PlayForward();
    }

    protected void WaitCloseUI()
    {
        mDescriptionLb.text = "";
        ScriptBinder.Invoke(waitTime, HideGameObject);
    }

    private void HideGameObject()
    {
        mUIPanel.gameObject.SetActive(false);
    }

}
