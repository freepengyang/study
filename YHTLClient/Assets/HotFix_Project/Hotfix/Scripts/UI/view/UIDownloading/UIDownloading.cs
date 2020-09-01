using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Main_Project.Script.Update;
using UnityEngine.Networking;

public class UIDownloading : UIBase
{
    private GameObject _checkUpdatePanel;

    private GameObject checkUpdatePanel
    {
        get { return _checkUpdatePanel ? _checkUpdatePanel : (_checkUpdatePanel = Get("root/CheckingUpdate").gameObject); }
    }

    private UILabel _tipsLabel;

    private UILabel tipsLabel
    {
        get { return _tipsLabel ? _tipsLabel : (_tipsLabel = Get<UILabel>("root/CheckingUpdate/Tips")); }
    }

    private GameObject _updateResource;

    private GameObject UpdateResource
    {
        get { return _updateResource ? _updateResource : (_updateResource = Get("root/UpdateResource").gameObject); }
    }

    private GameObject _btn_update;

    private GameObject btn_update
    {
        get { return _btn_update ? _btn_update : (_btn_update = Get("root/UpdateResource/confirm").gameObject); }
    }

    private UISlider _updateProgress;

    private UISlider updateProgress
    {
        get { return _updateProgress ? _updateProgress : (_updateProgress = Get<UISlider>("root/CheckingUpdate/updateslidernew")); }
    }

    private Transform _loadingEffect;

    private Transform loadingEffect
    {
        get { return _loadingEffect ? _loadingEffect : (_loadingEffect = Get("root/Loading")); }
    }

    private UITexture _logo;

    private UITexture logo
    {
        get { return _logo ? _logo : (_logo = Get<UITexture>("root/CheckingUpdate/logo")); }
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
    private DownloadUIType type;

    //bool unRarFinish = false;
    bool needUpdate = false;

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(btn_update).onClick = OnUpdateConfirmClick;
        LoadingEffect(true);
        RefreshTips(CSStringTip.CHECK_RESOURCE_UPDATE);
        CSPreDownLoadManger.Instance.onDownloadProgress = RefreshDownloadProgress;
        CSPreDownLoadManger.Instance.onDownloadError = PreDownloadError;
        if (logo != null) ReadLogo();
    }

    public override void Show()
    {
        base.Show();
    }

    private void ReadLogo()
    {
        logo.gameObject.SetActive(true);
        //孙砚说这个去掉
        //CoroutineManager.DoCoroutine(LoadLog());
    }

    private IEnumerator LoadLog()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(AppUrl.GameLogoUrl);

        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Texture tex = DownloadHandlerTexture.GetContent(www);
            if (tex != null && logo != null)
            {
                logo.mainTexture = tex;
                logo.MakePixelPerfect();
            }
        }else
        {
            FNDebug.LogErrorFormat("UIDownloading LoadLog error  {0}  code: {1}    url:{2}", www.error,
                www.responseCode, www.uri);
        }
        www.Dispose();
    }

    private void PreDownloadError(bool b)
    {
        if (UpdateResource != null)
        {
            UpdateResource.SetActive(true);
        }
    }

    private void PreDownloadOrEnterGame()
    {
        LoadingEffect(false);

        UnityEngine.Debug.Log($"OnCheckFinish PreDownloadOrEnterGame  needUpdate: {needUpdate}  ");
        if (needUpdate)
        {
            RefreshTips(CSStringTip.CHECK_RESOURCE_UPDATE);
            UIManager.Instance.ClosePanel<UIWaiting>();
            CSResUpdateManager.Instance.StartPreDownload(OnPreDownloadComplete);
        }
        else
        {
            OnPreloadTableComplete();
            //InitializeUIAndTable();
        }
    }

    /*private void InitializeUIAndTable()
    {
        ExtendTableLoader.Instance.PreLoadTables(OnPreloadTableComplete);
        ExtendTableLoader.Instance.LoadTables();
        CSPreLoadResourceRes.PreLoad();
        //资源修复功能暂不处理
        //CSResourceRepairMgr.Instance.DownLoadPreDownComplete();
    }*/

    private void OnInitFinish(bool complete)
    {
        this.needUpdate = false;
        if(complete)
        {
            PreDownloadOrEnterGame();
        }else
        {
            UtilityTips.ShowRedTips("版本文件异常，请重启游戏");
        }
    }
    
    private void OnCheckFinish(bool needUpdate)
    {
        this.needUpdate = needUpdate;

        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                PreDownloadOrEnterGame();
                break;
            case PlatformType.ANDROID:
                PreDownloadOrEnterGame();
                break;
            case PlatformType.IOS:
                PreDownloadOrEnterGame();
                break;
        }
    }

    private void OnPreDownloadComplete(bool isComplete)
    {
        QuDaoInterface.Instance.RestartGame();
    }

    private void OnPreloadTableComplete()
    {
        UIManager.Instance.ClosePanel<UIDownloading>();
        UIManager.Instance.CreatePanel<UILogin>();
    }

    protected override void OnDestroy()
    {
        CSPreDownLoadManger.Instance.onDownloadProgress = null;
        CSPreDownLoadManger.Instance.onDownloadError = null;

        base.OnDestroy();
    }

    public void RefreshTips(string tips)
    {
        if (tipsLabel != null)
        {
            tipsLabel.text = tips;
        }
    }

    public void RefreshMovingrogress(float percent)
    {
        tipsLabel.text = string.Format(CSStringTip.PREPARE_GAME_RESOURCE, (percent * 100).ToString("F2"));
    }

    public void RefreshUnRarProgress(float progress)
    {
        if (tipsLabel == null) return;

        if (progress == 0)
        {
            tipsLabel.text = CSStringTip.INIT_RESOURCE;
        }
        else if (progress < 0)
        {
            tipsLabel.text = CSStringTip.INIT_RESOURCE_FAIL;
        }
        else if (progress < 1)
        {
            tipsLabel.text = string.Format(CSStringTip.INIT_GAME_RESOURCE, (progress * 100).ToString("F2"));
        }
        else
        {
            tipsLabel.text = string.Format(CSStringTip.INIT_GAME_RESOURCE, 100);
        }
    }

    public void RefreshDownloadProgress(int curByteNum, int totalByteNum)
    {
        if (totalByteNum > 0)
        {
            if (!updateProgress.gameObject.activeInHierarchy) updateProgress.gameObject.SetActive(true);
            float value = (float) curByteNum / (float) totalByteNum;
            updateProgress.value = value;
            tipsLabel.text = string.Format(CSStringTip.UPDATE_GAME, ((float) curByteNum / 1024f / 1024f).ToString("F2"),
                ((float) totalByteNum / 1024f / 1024f).ToString("F2"));
        }
    }

    public override void RefreshData(params object[] obj)
    {
        base.RefreshData(obj);
        type = (DownloadUIType) obj[0];

        switch (type)
        {
            case DownloadUIType.CheckUpdate:
                checkUpdatePanel.SetActive(true);
                CSVersionManager.Instance.Init(OnInitFinish);
                RefreshTips(CSStringTip.INIT_CONFIG_JSON);
                break;
            case DownloadUIType.Download:
                OnCheckFinish(true);
                break;
        }
    }

    private void LoadingEffect(bool open)
    {
        if (loadingEffect != null && loadingEffect.gameObject != null)
        {
            loadingEffect.gameObject.SetActive(open);
        }
    }

    private void OnUpdateConfirmClick(GameObject go)
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.ANDROID:
                QuDaoInterface.Instance.FinishGame();
                break;
            case PlatformType.EDITOR:
            case PlatformType.IOS:
                Application.Quit();
                break;
        }
    }
}