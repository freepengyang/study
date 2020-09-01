using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UITimeExpCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    
    public enum TimeExpPanelType
    {
        TEPT_SHENFU = 1,
    }

    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = this.Close;
        RegChildPanel<UITimeExpPanel>((int)TimeExpPanelType.TEPT_SHENFU,mShenFuPanel.gameObject, mTogShenFu);
        RegisterRed(mShenFuRedpoint,RedPointType.TimeExp);

        mClientEvent.AddEvent(CEvent.OpenPanel, OnAlphaZero);
        mClientEvent.AddEvent(CEvent.ClosePanel, OnWelfareActivityPanelClosed);
        SetMoneyIds(1,4);
    }

    protected void OnAlphaZero(uint id,object argv)
    {
        if (argv is UIWelfareActivityPanel welfareActivityPanel)
        {
            Panel.alpha = 0.0f;
        }
    }

    protected void OnWelfareActivityPanelClosed(uint id,object argv)
    {
        if(argv is UIWelfareActivityPanel welfareActivityPanel)
        {
            Panel.alpha = 1.0f;
        }
    }
}