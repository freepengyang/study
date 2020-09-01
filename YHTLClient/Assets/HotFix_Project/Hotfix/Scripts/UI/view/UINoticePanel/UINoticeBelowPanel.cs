using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UINoticeBelowPanel : UINoticeBase
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    protected UISprite _bg;
    protected virtual UISprite mbg
    {
        get
        {
            if (_bg == null) _bg = Get<UISprite>("Root/window/background");
            return _bg;
        }
    }
    protected override NoticeType _NoticeType
    {
        get { return NoticeType.Below; }
    }
    protected override float waitTime
    {
        get { return 0.3f; }
    }
    public override float MoveSpeed
    {
        get { return 70; }
    }

    private tip.BulletinResponse NoticeS;


    public override void Init()
    {
        base.Init();
        mTweenPosition.onFinished.Add(new EventDelegate(this.OnPlayOver));
    }

    public override void Show()
    {
        base.Show();
        if (!UIPrefab.activeSelf) UIPrefab.SetActive(true);


        if (!isBeginPlay)
        {
            isBeginPlay = true;
            NoticeS = NoticeStr;

            if (string.IsNullOrEmpty(NoticeS.msg)){
                UIPrefab.SetActive(false);
            }

            mDescriptionLb.text = string.Empty;
            OnPlayMove();
        }
    }

    //开始播放
    protected override void OnPlayMove()
    {
        if (NoticeCount > 0)
        {
            ShowBg();
            if (string.IsNullOrEmpty(mDescriptionLb.text))
            {
                ShowUIEffect(Vector3.zero, Vector3.one);
            }
            CSNoticeManager.Instance.NoticeDequeue(_NoticeType);

            mDescriptionLb.alpha = 1;
            mDescriptionLb.text = NoticeS.msg;

            float lenght = GetLabelWidth();
            float clipwidth = 344;
            float timer = (lenght + clipwidth) / MoveSpeed;
            mTweenPosition.duration = timer;
            Vector3 from = Vector3.zero;
            from.Set(200, 0, 0);
            Vector3 to = Vector3.zero;
            float x = from.x - clipwidth - lenght;
            to.Set(x, 0, 0);
            mTweenPosition.from = from;
            mTweenPosition.to = to;
            mTweenPosition.ResetToBeginning();
            mTweenPosition.PlayForward();
        }
    }

    //播放完毕
    protected override void OnPlayOver()
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
    
    private void ShowBg()
    {
        if (NoticeS.bulletinId == 284 || NoticeS.bulletinId == 248 || NoticeS.bulletinId == 340)
        {
            mbg.spriteName = "bg_noticefl";
        }else
        {
            mbg.spriteName = "bg_noticefl2";
        }
        mbg.gameObject.SetActive(true);
    }
}
