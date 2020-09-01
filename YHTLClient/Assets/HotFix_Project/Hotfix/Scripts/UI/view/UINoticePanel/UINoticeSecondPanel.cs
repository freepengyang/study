using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// 二级公告显示栏
/// </summary>
public class UINoticeSecondPanel : UINoticeBase
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    private UISprite _background;
    private UISprite background { get { return _background ?? (_background = Get<UISprite>("Root/window/background")); } }

    protected Transform _mDescriptionInfoList;
    protected Transform mDescriptionInfoList
    {
        get
        {
            if (_mDescriptionInfoList == null) _mDescriptionInfoList = UIPrefabTrans.Find("Root/Scroll View");
            return _mDescriptionInfoList;
        }
    }

    protected override NoticeType _NoticeType
    {
        get { return NoticeType.CenterTop; }
    }

    private float mnotiecewaitTime;
    protected  float notieceWaitTime
    {
        get { return mnotiecewaitTime; }
        set { mnotiecewaitTime = value; }
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

    private UINoticeSecondInfo[] _noticeSecondInfos;

    public override void Init()
    {
        base.Init();
        InitNoticeInfo();
    }

    private void InitNoticeInfo()
    {
        if(_noticeSecondInfos != null) return;
        _noticeSecondInfos = new UINoticeSecondInfo[mDescriptionInfoList.childCount];
        for (int i = 0; i < _noticeSecondInfos.Length; i++)
        {
            if(_noticeSecondInfos[i] == null ) _noticeSecondInfos[i] = new UINoticeSecondInfo();
            _noticeSecondInfos[i].gameObject = mDescriptionInfoList.GetChild(i).gameObject;
            _noticeSecondInfos[i].Init();
        }
    }
    

    public override void Show()
    {
        base.Show();
        UIPrefabTrans.position = Vector3.zero;
        if (!UIPrefab.activeSelf) UIPrefab.SetActive(true);
        
        NoticeS = NoticeStr;
        CSNoticeManager.Instance.NoticeDequeue(_NoticeType);
        if (NoticeS == null || string.IsNullOrEmpty(NoticeS.msg))
            return;

        string nextMsg = NoticeS.msg;
        bool lastShow = true;
        for (int i = 0; i < _noticeSecondInfos.Length; i++)
        {
            if (_noticeSecondInfos[i].isShow)
            {
                nextMsg = _noticeSecondInfos[i].msgLab.text;
            }

            if (lastShow)
            {
                lastShow = _noticeSecondInfos[i].isShow;

                _noticeSecondInfos[i].ShowNotice(NoticeS.msg);
            }
            else
                break;

            NoticeS.msg = nextMsg;
        }
    }

    //开始播放
    protected override void OnPlayMove()
    {
    }

    //播放完毕
    protected override void OnPlayOver()
    {
    }

    protected override void OnDestroy()
    {
        if (_noticeSecondInfos != null)
        {
            for (var i = 0; i < _noticeSecondInfos.Length; i++)
            {
                _noticeSecondInfos[i]?.Dispose();
            }
        }
        base.OnDestroy();
    }
}
