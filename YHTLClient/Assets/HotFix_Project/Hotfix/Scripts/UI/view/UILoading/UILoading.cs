using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Main_Project.Script.Update;

public class UILoading : UIBase
{
    private UILabel _mLoadLb;

    private UILabel mLoadLb
    {
        get { return _mLoadLb ?? (_mLoadLb = Get<UILabel>("root/loadlb")); }
    }

    private UISlider _mLoadSlider_;

    private UISlider mLoadSlider_
    {
        get { return _mLoadSlider_ ?? (_mLoadSlider_ = Get<UISlider>("root/loadslidernew")); }
    }

    private Transform _mheader;

    private Transform mheader
    {
        get { return _mheader ?? (_mheader = Get<Transform>("root/loadslider/header")); }
    }

    private UILabel _lb_version;

    private UILabel lb_version
    {
        get { return _lb_version ?? (_lb_version = Get<UILabel>("root/lb_version")); }
    }

    private UILabel _lb_tips;

    private UILabel lb_tips
    {
        get { return _lb_tips ?? (_lb_tips = Get<UILabel>("root/Tips")); }
    }

    GameObject _slider1;

    GameObject slider1
    {
        get { return _slider1 ?? (_slider1 = Get<GameObject>("root/loadslidernew/login_slider1")); }
    }

    GameObject _slider2;

    GameObject slider2
    {
        get { return _slider2 ?? (_slider2 = Get<GameObject>("root/loadslidernew/login_slider2")); }
    }

    GameObject _thumb;

    GameObject thumb
    {
        get { return _thumb ?? (_thumb = Get<GameObject>("root/loadslidernew/thumb/17919")); }
    }
    
    GameObject _loadingTex;

    GameObject loadingTex
    {
        get { return _loadingTex ?? (_loadingTex = Get<GameObject>("window/Texture")); }
    }
    
    GameObject _loadingTex2;

    GameObject loadingTex2
    {
        get { return _loadingTex2 ?? (_loadingTex2 = Get<GameObject>("window/Texture2")); }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Guide; }
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public Vector3 initPos = new Vector3();

    public override void Init()
    {
        initPos = new Vector3();
        mLoadSlider_.gameObject.SetActive(true);
        mLoadSlider_.value = 0;
        if (mheader != null)
        {
            initPos = mheader.localPosition;
        }

        if (!CSGame.Sington.IsLoadLocalRes)
        {
            if (lb_version != null) lb_version.text = "V" + CSVersionManager.Instance.Version;
        }

        if (mLoadSlider_ != null)
        {
            mLoadSlider_.gameObject.SetActive(false);
        }

        if (mLoadLb != null)
        {
            mLoadLb.gameObject.SetActive(false);
        }
    }


    public override void Show()
    {
        base.Show();
        if (mLoadLb != null)
            mLoadLb.text = CSString.Format(408);
        if (mLoadSlider_ != null)
            mLoadSlider_.value = 0;
    }
    
    public void ShowLoginScene()
    {
        CSMainParameterManager.LoadingComplete = false;
        loadingTex.SetActive(false);
        mLoadSlider_.value = 0;
        ScriptBinder.InvokeRepeating(0, 0.02f, RepeatSetLoadin);
    }
    
    private void RepeatSetLoadin()
    {
        if(mLoadSlider_.value < 1)
        {
            SetLoading(mLoadSlider_.value + 0.05f);
        }
        else
        {
            CSMainParameterManager.LoadingComplete = true;
        }
    }

    public void StartLoadTable()
    {
        if (CSConstant.ServerType == CSConstant.LastServerType)
        {
            LoadComplate();
            return;
        }
        loadingTex.SetActive(true);
        ScriptBinder.StartCoroutine(InitTable());
        ScriptBinder.StartCoroutine(LoadTable());
    }

    private IEnumerator InitTable()
    {
        //yield return ExtendTableLoader.Instance.AddTableParser();
        CSPreLoadResourceRes.PreLoad();
        ExtendTableLoader.Instance.LoadTables();
        yield return ExtendTableLoader.Instance.OnLoadTable();

        ExtendTableLoader.Instance.CastLoadComplate();
    }

    IEnumerator LoadTable()
    {
        int index = 1;
        float toProgress = 0;
        while (!ExtendTableLoader.Instance.IsStartCalculate)
        {
            toProgress = 0.004f * index++;
            if (toProgress >= 0.9f)
                toProgress = 0.9f;
            SetLoading(toProgress);
            yield return null;
        }

        float lastProc = toProgress;
        float leftProc = 1 - toProgress;
        Vector2 targetStep = Vector2.zero;
        Vector2 lastProgressValue = Vector2.zero;
        if (!Mathf.Approximately(leftProc, 0))
        {
            while (lastProgressValue.x + 0.01f < 1) //误差
            {
                float f = ExtendTableLoader.Instance.CurLoadProgress - lastProc;
                toProgress = f / leftProc;
                targetStep.x = toProgress;
                lastProgressValue = Vector2.Lerp(lastProgressValue, targetStep, Time.deltaTime * 5);

                SetLoading(lastProgressValue.x);

                yield return null;
            }
        }
        else
        {
            while (lastProgressValue.x + 0.01f < toProgress) //误差
            {
                float f = 0;
                f = 1 - lastProc;
                f = f / leftProc;

                toProgress = f;
                toProgress = Mathf.Min(toProgress, toProgress);
                targetStep.Set(toProgress, 0);
                lastProgressValue = Vector2.Lerp(lastProgressValue, targetStep, Time.deltaTime * 5);
                SetLoading(lastProgressValue.x);
                yield return null;
            }
        }

        yield return null;

        while (!ExtendTableLoader.Instance.IsLoadedAll)
        {
            yield return null;
        }

        LoadComplate();
    }

    private void LoadComplate()
    {
        CSConstant.LastServerType = CSConstant.ServerType;
        UIManager.Instance.CreatePanel<UILoginRolePanel>();
        UIManager.Instance.ClosePanel<UILoading>();
    }

    public override void OnRecycle()
    {
        if (mheader != null)
        {
            mheader.localPosition = initPos;
        }

        base.OnRecycle();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _mLoadLb = null;
        _mLoadSlider_ = null;
    }

    public void SetLoading(float displayProgress)
    {
        if (mLoadSlider_ != null && !mLoadSlider_.gameObject.activeSelf)
        {
            mLoadSlider_.gameObject.SetActive(true);
        }

        if (mLoadLb != null && !mLoadLb.gameObject.activeSelf)
        {
            mLoadLb.gameObject.SetActive(true);
        }

        if (mLoadLb != null)
        {
            mLoadLb.text = string.Format(CSStringTip.LOAD_TABLE_TEXT, (int) (displayProgress * 100));
        }

        if (mLoadSlider_ != null && mLoadSlider_.value < displayProgress)
        {
            mLoadSlider_.value = displayProgress;
            if (mheader != null)
            {
                mheader.localPosition = new Vector3((initPos.x + displayProgress * 790), initPos.y, initPos.z);
            }
        }
    }
}