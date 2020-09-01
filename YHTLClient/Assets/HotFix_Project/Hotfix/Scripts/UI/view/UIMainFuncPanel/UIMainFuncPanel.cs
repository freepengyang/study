using System;
using UnityEngine;

public partial class UIMainFuncPanel : UIBasePanel
{
    #region Component

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }

    private bool _isHide;

    public bool IsHide
    {
        get { return _isHide; }
        set
        {
            if (_isHide == value) return;
            mRoot.from = _NormalPosition;
            mRoot.to = _DragEndPosition;
            mRoot.Play(value);
            _isHide = value;
        }
    }

    private bool _isPackUp;

    public bool IsPackUp
    {
        get { return _isPackUp; }
        set
        {
            if (_isPackUp == value) return;
            if (_isHide)
                mRoot.from = _DragEndPosition;
            else
                mRoot.from = _NormalPosition;
            mRoot.to = _HidePosition;
            mRoot.Play(value);
            _isPackUp = value;

            if (_isPackUp)
                mClientEvent.SendEvent(CEvent.ResetMainTeamSelect);
        }
    }

    #endregion

    private const float DRAG_ANGLE_RATE = 0.2f;
    private const int MOVE_DISTANCE = 30;

    UIMainTeamPanel uiMainTeamPanel;

    private Vector3 _NormalPosition;
    private Vector3 _DragEndPosition;
    private Vector3 _HidePosition;

    public override void Init()
    {
        base.Init();
        EventDelegate.Add(mbtn_func.onChange, FuncToggleClick);
        EventDelegate.Add(mbtn_team.onChange, TeamToggleClick);

        mbtn_rotate.onClick = OnClickChangeMode;

        mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
        mClientEvent.AddEvent(CEvent.MainFuncModeChanged, OnMainFuncModeChanged);

        _NormalPosition = mRoot.transform.localPosition;
        _DragEndPosition = _NormalPosition + Vector3.right * 41;
        _HidePosition = _NormalPosition - Vector3.right * 400;

        mRoot.AddOnFinished(new EventDelegate(OnFinishMove));
    }

    protected void OnClickChangeMode(GameObject go)
    {
        CSMainFuncManager.Instance.Init(CSMainFuncManager.Instance.Mode == CSMainFuncManager.PM_MODE ?
            CSMainFuncManager.MT_MODE : CSMainFuncManager.PM_MODE);
    }

    private void OnFinishMove()
    {
        mClientEvent.SendEvent(CEvent.MainFuncPanelTweenFinish);
    }


    private void MoveUIMainScenePanel(uint id, object data)
    {
        if (data == null) return;
        IsPackUp = (bool)data;
        //ApplyModeChange();
    }

    private void ApplyModeChange(bool autoAdaptMap)
    {
        bool SetUpTabAsDefault = true;
        if (autoAdaptMap && CSMainFuncManager.Instance.Mode == CSMainFuncManager.PM_MODE)
        {
            int mapId = CSMainPlayerInfo.Instance.MapID;
            SetUpTabAsDefault = CSMainFuncManager.Instance.ShowPlayerInfo(mapId);
        }

        if (SetUpTabAsDefault)
        {
            EventDelegate.Remove(mbtn_func.onChange, FuncToggleClick);
            mbtn_func.Set(true);
            FuncClick();
            EventDelegate.Add(mbtn_func.onChange, FuncToggleClick);
        }
        else
        {
            EventDelegate.Remove(mbtn_team.onChange, TeamToggleClick);
            mbtn_team.Set(true);
            TeamClick();
            EventDelegate.Add(mbtn_team.onChange, TeamToggleClick);
        }
    }

    private void OnMainFuncModeChanged(uint id, object data)
    {
        if(data is bool autoAdaptMap)
        {
            ApplyModeChange(autoAdaptMap);
        }
    }

    public void InitPanel(Transform go, MainFucShowName showName)
    {
        go.parent = mUIFuncPanel.transform;
        go.localPosition = Vector3.zero;
        ShowButtonName(showName);
    }

    void FuncClick()
    {
        CSMainFuncManager.Instance.InitUpPanel();
    }

    void TeamClick()
    {
        CSMainFuncManager.Instance.InitDownPanel();
    }

    void TeamToggleClick()
    {
        if (mbtn_team.value)
        {
            TeamClick();
        }
    }
    void FuncToggleClick()
    {
        if (mbtn_func.value)
        {
            FuncClick();
        }
    }

    void InitDownTab()
    {
        var mode = CSMainFuncManager.Instance.Mode;
        if(mode == CSMainFuncManager.MT_MODE)
        {
            int mapId = CSMainPlayerInfo.Instance.MapID;
            int type = InstanceTableManager.Instance.GetInstanceType(mapId);
            if (type == (int)ECopyType.WorldBoss)
            {
                msp_team.spriteName = "btn_activity1";
                msp_teamlight.spriteName = "btn_activity2";
            }
            else
            {
                msp_team.spriteName = "btn_team1";
                msp_teamlight.spriteName = "btn_team2";
            }
        }
        else
        {
            msp_team.spriteName = "btn_monster_1";
            msp_teamlight.spriteName = "btn_monster_2";
        }
    }

    private void ShowButtonName(MainFucShowName showName)
    {
        switch (showName)
        {
            case MainFucShowName.Mission:
                msp_func.spriteName = "btn_task1";
                msp_funclight.spriteName = "btn_task2";
                mlb_func.text = CSString.Format(520);
                InitDownTab();
                break;
            case MainFucShowName.Activity:
                msp_func.spriteName = "btn_activity1";
                msp_funclight.spriteName = "btn_activity2";
                mlb_func.text = CSString.Format(521);
                InitDownTab();
                break;
            case MainFucShowName.Instance:
                msp_func.spriteName = "btn_copy1";
                msp_funclight.spriteName = "btn_copy2";
                mlb_func.text = CSString.Format(522);
                InitDownTab();
                break;
            case MainFucShowName.PlayerInfo:
                msp_func.spriteName = "btn_play1";
                msp_funclight.spriteName = "btn_play2";
                mlb_func.text = CSString.Format(520);
                InitDownTab();
                break;
            case MainFucShowName.MonsterInfo:
                msp_func.spriteName = "btn_play1";
                msp_funclight.spriteName = "btn_play2";
                mlb_func.text = CSString.Format(520);
                InitDownTab();
                break;
            case MainFucShowName.Team:
                InitDownTab();
                break;
        }
    }

    protected override void OnDestroy()
    {
        if (uiMainTeamPanel != null)
            uiMainTeamPanel.Destroy();
        base.OnDestroy();
    }
}