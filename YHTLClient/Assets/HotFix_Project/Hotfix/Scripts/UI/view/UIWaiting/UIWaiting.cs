using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ShowState
{
    Disconnected, //断线
    ConnectFailed, //连接失败
    Kickoff, //踢下线
}

public class UIWaiting : UIBase
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    private long ConnectTimes = 0;
    private bool isClose;
    private GameObject _btn;

    private GameObject btn
    {
        get { return _btn ?? (_btn = Get<GameObject>("btn")); }
    }

    private CSInvoke _Invoke;

    private CSInvoke Invoke
    {
        get { return _Invoke ?? (_Invoke = Get<CSInvoke>("bg")); }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Connect; }
    }

    public override void Init()
    {
        base.Init();
        isClose = false;
        if (CSGame.Sington.mCurState != GameState.MainScene)
        {
            btn.SetActive(false);
        }
        else
        {
            btn.SetActive(true);
        }

        UIEventListener.Get(btn).onClick = f => { CSHotNetWork.Instance.OnReturn(); };
    }

    public override void Show()
    {
        //同一帧打开并且关闭，，会导致 调用 UIPrefab 报空
        if (!isClose) base.Show();
    }

    public void Show(bool isshow = true)
    {
        if (isshow) StartCountDown();
        else
        {
            Invoke.StopInvoke();
        }
    }

    private void OnDisable()
    {
        ConnectTimes = 0;
        Invoke.StopInvoke();
    }

    protected override void OnDestroy()
    {
        isClose = true;
        base.OnDestroy();
    }

    private void TimeLimit()
    {
        ConnectTimes++;
        CSNetwork.Instance.Reconection();

        if (ConnectTimes >= 10)
        {
            Invoke.StopInvoke();

            if (CSGame.Sington.mCurState == GameState.MainScene)
            {
                CSHotNetWork.Instance.OnReturn();
            }
            else
            {
                UIManager.Instance.ClosePanel<UIWaiting>();
                UtilityTips.ShowRedTips(105543);
            }
        }
    }

    private void StartCountDown()
    {
        Invoke.InvokeRepeating(3, 3, TimeLimit);
    }
}