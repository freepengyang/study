using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIActivityCombinedPanel : UIBasePanel
{

    private enum ActivityPanelType { Daily = 1, Activities,  SignIn, Weekly,  }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }



    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);

        mbtn_close.onClick = CloseBtnClick;

        RegChildPanel<UIDailyActivityPanel>((int)ActivityPanelType.Daily, mobj_dailyActivityPanel, mbtn_daily);
        RegChildPanel<UIActivityHallPanel>((int)ActivityPanelType.Activities, mobj_activityHallPanel, mbtn_activity);
        RegChildPanel<UIDailySignInCombinedPanel>((int)ActivityPanelType.SignIn, mobj_signInPanel, mbtn_card);
        RegChildPanel<UIWeeklyCalendarPanel>((int)ActivityPanelType.Weekly, mobj_weeklyCalendarPanel, mbtn_weekly);

        FuncOpenBind();

        SetMoneyIds(1, 4);

        RegisterRed(mbtn_daily.transform.Find("redpoint").gameObject, RedPointType.DailyBtn);
        RegisterRedList(mbtn_card.transform.Find("redpoint").gameObject, RedPointType.SignIn, RedPointType.SignInAchivement);
    }

    void FuncOpenBind()
    {
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_signIn, mbtn_card.gameObject);
    }


    void FunctionOpenStateChange(uint id, object data)
    {
        if (mgrid_tgGroup != null) mgrid_tgGroup.Reposition();
    }


    public override void Show()
    {
        base.Show();

        FunctionOpenStateChange(0, null);

    }


    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIActivityCombinedPanel>();
    }
    

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }
    
}
