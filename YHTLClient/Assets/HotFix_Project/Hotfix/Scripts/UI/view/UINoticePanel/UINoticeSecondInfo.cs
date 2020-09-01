using UnityEngine;
using System.Collections;

public class UINoticeSecondInfo : GridContainerBase {

    private TweenAlpha _alpha;
    private TweenAlpha alpha
    {
        get { return _alpha ?? ( _alpha = gameObject.transform.GetComponent<TweenAlpha>()) ; }     
    }

    private UILabel _msg;
    public UILabel msgLab
    {
        get { return _msg ?? (_msg = gameObject.transform.GetComponent<UILabel>()); }
    }

    private TweenPosition _textScale;
    private TweenPosition textScale
    {
        get { return _textScale ?? (_textScale = gameObject.transform.GetComponent<TweenPosition>()); }
    }

    private int index;

    private int showTime;
    private int vanishTime;

    public bool isShow = false;

    public override void Init()
    {
        index = System.Convert.ToInt32(gameObject.name);
        GetShowTime();
        alpha.onFinished.Add(new EventDelegate(OnFinishShow));
        UIEventListener.Get(msgLab.gameObject).onClick = OnOpenUrlClick;
    }

    public void ShowNotice(string msg)
    {
        msgLab.text = msg;
        msgLab.alpha = 1;
        isShow = true;
        alpha.ResetToBeginning();
        alpha.PlayForward();

        if(index ==1 && textScale != null)
        {
            textScale.ResetToBeginning();
            textScale.PlayForward();
        }
    }

    private void StartTime()
    {
       

    }

    private void OnFinishShow()
    {
        isShow = false;
        msgLab.text = "";
    }

    private void GetShowTime()
    {
        switch (index)
        {
            case 1:
                showTime = 5;
                vanishTime = 3;
                break;
            case 2:
                showTime = 2;
                vanishTime = 2;
                break;
            case 3:
                showTime = 2;
                vanishTime = 1;
                break;
        }
        alpha.delay = showTime;
        alpha.duration = vanishTime;
    }

    private void OnOpenUrlClick(GameObject go)
    {
        msgLab.SetupLink();
    }

    public override void Dispose()
    {
    }
}
