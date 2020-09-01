using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIMissionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override bool Cached
    {
        get { return true; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }

    private UIMissionInfo _mLastSelectMis;
    private readonly Vector4 scrollViewPos1 = new Vector4(0, 0, 216, 178);
    private readonly Vector4 scrollViewPos_ShowRecom = new Vector4(0, -17, 216, 143);
    private UIGridContainerHot<UIMissionInfo> gridContainerMis;

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.OpenFuncRecommond, OnOpenFuncRecommond);
        mClientEvent.AddEvent(CEvent.Task_GoalUpdate, RefreshMissionList);
        mClientEvent.AddEvent(CEvent.SetSelectMissionState, SetSelectMissionState);

        mClientEvent.AddEvent(CEvent.ShowMissionUnAccept, ShowMissionUnAccept);
        mClientEvent.AddEvent(CEvent.HideMissionTips, HideMissionTips);
        
        mClientEvent.AddEvent(CEvent.MainFuncPanelTweenFinish, MainFuncPanelTweenFinish);
        mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);

        UIEventListener.Get(mTips).onClick = OnTipsClick;
        InitGridContainer();

        ScriptBinder.InvokeRepeating(1.0f, 1.0f, CheckDailyGuiding);
    }


    void MoveUIMainScenePanel(uint id, object data)
    {
        if (data == null || mTweenAlpha == null) return;
        bool isHide = (bool)data;
        if (isHide) mTweenAlpha.PlayForward();
        else mTweenAlpha.PlayReverse();
    }

    protected void CheckDailyGuiding()
    {
        if (CSGuideManager.Instance.IsGuiding)
            return;

        for (int i = 0, max = gridContainerMis.MaxCount; i < max; i++)
        {
            var missionInfo = gridContainerMis.controlList[i];
            if (null == missionInfo)
                continue;

            if (missionInfo.IsDailyTaskAndFinished())
            {
                break;
            }
        }
    }

    private void InitGridContainer()
    {
        if (gridContainerMis == null)
        {
            gridContainerMis = new UIGridContainerHot<UIMissionInfo>();
        }

        gridContainerMis.SetArrangement(UIGridContainerHot<UIMissionInfo>.Arrangement.Vertical).SetCellHeight(1)
            .SetGameObject(mMsiionList, mmission);
    }

    public override void Show()
    {
        base.Show();
        OnOpenFuncRecommond(0, null);
        ShowMissionList();
    }

    //Vector3 pos = new Vector3(0, 80, 0);
    //private int index = 0;
    private void MainFuncPanelTweenFinish(uint id, object data)
    {
        mlistScrollView.panel.alpha = 0.95f;
        ScriptBinder.Invoke(0.1f, ShowPanel);
    }
    
    private void ShowPanel()
    {
        mlistScrollView.panel.alpha = 1f;
    }

    #region 功能预告显示

    private void ShowOpenRecommend(bool islevelUp, FUNCOPEN data)
    {
        if (data == null)
        {
            data = UICheckManager.Instance.GetMainOpenRecommendTab();
            if (data == null)
            {
                mrecommend.SetActive(false);
                mlistScrollViewPanel.baseClipRegion = scrollViewPos1;
                //mlistScrollViewPanel.clipOffset = Vector2.zero;
                //mlistScrollView.ResetPosition();
            }
            else
            {
                RefreshOpenRecommendTips(data);
            }
        }
        else
        {
            RefreshOpenRecommendTips(data);
        }

        mlistScrollView.ResetPosition();
    }

    FUNCOPEN mCachedData;

    private void RefreshOpenRecommendTips(FUNCOPEN data)
    {
        mCachedData = data;
        if (data == null) return;
        mrecommend.SetActive(true);
        mlistScrollViewPanel.baseClipRegion = scrollViewPos_ShowRecom;
        //mlistScrollViewPanel.clipOffset = Vector2.zero;
        mlb_title.text = data.functionName;
        mlb_level.text = CSString.Format(data.needLevel > CSMainPlayerInfo.Instance.Level, 523, 524, data.needLevel);
        mspr_icon.spriteName = data.icon1.ToString();
        mbtn_recommand.onClick = this.OnClickRecommand;
        //mlistScrollView.ResetPosition();
    }

    void OnClickRecommand(GameObject go)
    {
        if (null != mCachedData)
        {
            CSNewFunctionUnlockManager.Instance.PreviewFuncOpen(mCachedData.id);
        }
    }

    #endregion

    private void OnMissionClick(UIMissionInfo go)
    {
        if (go == null) return;

        if (_mLastSelectMis == go) return;

        if (_mLastSelectMis != null && _mLastSelectMis.gameObject.activeSelf)
            _mLastSelectMis.ResetChoose();
        go.ChooseObj();
        _mLastSelectMis = go;
    }

    #region 协议刷新

    private void RefreshMissionList(uint id, object obj)
    {
        ShowMissionList();
    }

    /// <summary>
    /// 任务面板刷新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="obj"></param>
    private void ShowMissionList()
    {
        //任务更新的时候停止日常任务引导
        bool isDailyGuiding = false;
        bool existedLockedMission = false;
        if (CSGuideManager.Instance.CurrentGuideId == 100001000)
        {
            isDailyGuiding = true;
            CSGuideManager.Instance.ResetGuideStep(false);
            CSGuideManager.Instance.CurrentGuideId = -1;
        }

        var _showMissionList = CSMissionManager.Instance.GetMission();

        gridContainerMis.MaxCount = _showMissionList.Count;
        _mLastSelectMis = null;
        int height = 0;
        UIMissionInfo info;
        for (int i = 0; i < gridContainerMis.MaxCount; i++)
        {
            var missionBase = _showMissionList[i];
            if (isDailyGuiding && CSGuideManager.Instance.LastGuideDailyTaskId == missionBase.TaskId)
            {
                existedLockedMission = true;
                if (missionBase.TaskState != TaskState.Completed)
                {
                    isDailyGuiding = false;
                }
            }

            info = gridContainerMis.controlList[i];
            info.Show(missionBase);
            info.OnClick = OnMissionClick;
            if (i != 0)
                info.gameObject.transform.localPosition = Vector3.down * (gridContainerMis.CellHeight * i + height);
            height += info.height;

            if (CSMissionManager.Instance.CurSelectMission != null &&
                CSMissionManager.Instance.CurSelectMission.TaskId == missionBase.TaskId)
            {
                info.ChooseObj();
                _mLastSelectMis = info;
            }
        }

        if (_showMissionList.Count <= 3)
        {
            mlistScrollView.ResetPosition();
        }

        if (isDailyGuiding && existedLockedMission)
        {
            FNDebug.LogFormat("<color=#00ff00>[重启任务引导]:{0}</color>", CSGuideManager.Instance.LastGuideDailyTaskId);
            CSGuideManager.Instance.ResidentDailyTrigger(100001000, CSGuideManager.Instance.LastGuideDailyTaskId);
        }
        else
        {
            CSGuideManager.Instance.LastGuideDailyTaskId = 0;
        }

        mlistScrollView.UpdateScrollbars();
        //mSpecialTipGroup.SetActive(_showMissionList.Count == 0);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnTaskGuideNameChanged);
    }

    private void SetSelectMissionState(uint id, object obj)
    {
        if (_mLastSelectMis == null) return;
        int taskId = (int) obj;
        if (taskId == 0 || !_mLastSelectMis.IsTask(taskId))
        {
            _mLastSelectMis.ResetChoose();
            _mLastSelectMis = null;
        }
    }

    private void ShowMissionUnAccept(uint id, object data)
    {
        if (mTips.activeSelf)
        {
            mTips.SetActive(false);
            return;
        }

        if (data == null) return;
        int taskId = (int) data;
        string comWay = TasksTableManager.Instance.GetTasksCompleteWay(taskId);
        if (string.IsNullOrEmpty(comWay)) return;
        List<int> wayList = UtilityMainMath.SplitStringToIntList(comWay);
        SelectList(ref wayList);

        wayList = Utility.DealWithFirstRecharge(wayList);
        mButtonGroup.Bind<int, UIMissionTaskWayItem>(wayList);
        mButtonGroup.MaxCount = wayList.Count;
        mtipBg.height = wayList.Count * (int) mButtonGroup.CellHeight + 40;
        mTips.SetActive(true);
    }

    private void SelectList(ref List<int> wayList)
    {
        for (int i = 0; i < wayList.Count; i++)
        {
            if (wayList[i] == 716)
            {
                if (!CSOpenServerACInfo.Instance.GetOpenAcState(10106))
                {
                    wayList.Remove(wayList[i]);
                    i--;
                }
            }
        }
    }


    private void HideMissionTips(uint id, object obj)
    {
        OnTipsClick(null);
    }

    private void OnOpenFuncRecommond(uint id, object obj)
    {
        EventData eventData = obj as EventData;
        if (eventData == null)
        {
            ShowOpenRecommend(false, null);
            return;
        }

        FUNCOPEN funcopens = eventData.arg2 as FUNCOPEN;
        ShowOpenRecommend((bool) eventData.arg1, funcopens);
    }

    #endregion

    private void OnTipsClick(GameObject go)
    {
        mTips.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        gridContainerMis.Dispose();
        gridContainerMis = null;
        _mLastSelectMis = null;
        UIManager.Instance.ClosePanel<UIMissionGuildPanel>();
    }
}