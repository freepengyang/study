using UnityEngine;
using System.Collections;
using System;

public class UINoticeColoursPanel : UINoticeBase
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    private UILabel _name;
    private UILabel playerName { get { return  _name ?? (_name = Get<UILabel>("Root/view/name")); } }

    private AutoChangeColorLabel _colorLabel;
    private AutoChangeColorLabel colorLabe { get { return _colorLabel ?? (_colorLabel = Get<AutoChangeColorLabel>("Root/Scroll View/info")); } }

    private UIPanel _ScrollView;
    private UIPanel ScrollView { get { return _ScrollView ?? (_ScrollView = Get<UIPanel>("Root/Scroll View")); } }

    private GameObject _bg;
    private GameObject bg { get { return _bg ?? (_bg = Get<GameObject>("Root/window/btn_coloursWorld/background")); } }

    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Resident;
        }
    }

    protected override NoticeType _NoticeType
    {
        get { return NoticeType.ColoursWorld; }
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

    private int playTime = 0;
    private float scrollViewWeight = 0;
    Vector4 rect = new Vector4();
    private int labelLimit = 50;

    public override void Init()
    {
        base.Init();
        mTweenPosition.onFinished.Add(new EventDelegate(OnPlayerOnce));

        UIEventListener.Get(bg).onClick = OnOpenChatPanelClick;

        rect = ScrollView.baseClipRegion;
        SetLabelLimit();
    }

    public override void Show()
    {
        base.Show();
        if (!UIPrefab.activeSelf) UIPrefab.SetActive(true);


        InitializeColor();

        mTweenPosition.ResetToBeginning();
        playTime = 0;
        NoticeS = NoticeStr;

        if (string.IsNullOrEmpty(NoticeS.msg))
        {
            UIPrefab.SetActive(false);
        }

        mDescriptionLb.text = string.Empty;

        ScriptBinder.InvokeRepeating(0, 1, TimeClose);
        OnPlayMove();
    }

    //开始播放
    protected override void OnPlayMove()
    {
        if (NoticeCount > 0)
        {
            if (string.IsNullOrEmpty(mDescriptionLb.text))
            {
                ShowUIEffect(Vector3.zero, Vector3.one);
            }
            mDescriptionLb.alpha = 1;
            CSNoticeManager.Instance.NoticeDequeue(_NoticeType);

            string[] message = NoticeS.msg.Split('#');
            playerName.text = "[eee5c3]" + message[0] +"：";

            CalculateSrollView();

            if (message.Length < 2) return;
            if (message[1].Length > labelLimit)
            {
                message[1] = message[1].Substring(0, labelLimit);
            }
            mDescriptionLb.text = message[1];

            float lenght = GetLabelWidth();
            float timer = (lenght + scrollViewWeight) / MoveSpeed;
            mTweenPosition.duration = timer;
            Vector3 from = Vector3.zero;
            from.Set(140, 0, 0);
            Vector3 to = Vector3.zero;
            float x = from.x - scrollViewWeight - lenght;
            to.Set(x, 0, 0);
            mTweenPosition.from = from;
            mTweenPosition.to = to;
            mTweenPosition.ResetToBeginning();
            mTweenPosition.PlayForward();
        }
    }

    private void OnPlayerOnce()
    {
        mTweenPosition.ResetToBeginning();
        mTweenPosition.PlayForward();
    }
    private void TimeClose()
    {
        playTime += 1;
        if (playTime >= 60)
        {
            OnPlayOver();
            playTime = 0;
            ScriptBinder.StopInvokeRepeating();
        }
    }
    //播放完毕
    protected override void OnPlayOver()
    {
        if (NoticeCount > 0)
        {
            OnPlayMove();
        }
        else
        {
            ShowUIEffect(Vector3.one, Vector3.zero);
            WaitCloseUI();
            colorLabe.gameObject.SetActive(false);
        }
    }

    private void InitializeColor()
    {
        TABLE.SUNDRY sundry;
        if(SundryTableManager.Instance.TryGetValue(60034,out sundry))
        {
            string[] color = sundry.effect.Split('#');

            try
            {
                Color c1 = Color.white;

                if (color.Length >= 1)
                {
                    ColorUtility.TryParseHtmlString("#" + color[0], out c1);
                    colorLabe.StartColor = c1;
                }

                if (color.Length >= 2)
                {
                    ColorUtility.TryParseHtmlString("#" + color[1], out c1);
                    colorLabe.MiddleColor = c1;
                }
                if (color.Length >= 3)
                {
                    ColorUtility.TryParseHtmlString("#" + color[2], out c1);
                    colorLabe.EndColor = c1;
                }
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }

            colorLabe.gameObject.SetActive(true);

        }
    }
    private void CalculateSrollView()
    {
        float size = playerName.GetComponent<UIWidget>().localSize.x;

        Vector4 newRect = new Vector4(rect.x + size / 2, rect.y, rect.z - size, rect.w);
        ScrollView.baseClipRegion = newRect;
        ScrollView.transform.GetComponent<UIScrollView>().InvalidateBounds();

        scrollViewWeight = newRect.z;
    }

    private void SetLabelLimit()
    {
        TABLE.SUNDRY sundry;
        if(SundryTableManager.Instance.TryGetValue(60032,out sundry))
        {
            labelLimit = Convert.ToInt32(sundry.effect);
        }
    }

    private void OnOpenChatPanelClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIChatPanel>(f => { (f as UIChatPanel).Show(ChatType.CT_WORLD); });
    }
}
