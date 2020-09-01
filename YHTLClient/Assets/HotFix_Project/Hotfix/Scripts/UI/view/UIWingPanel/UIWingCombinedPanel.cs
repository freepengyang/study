using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWingCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType => PrefabTweenType.FirstPanel;
    private int curTab = 1;
    private int lastTab = 1;

    public enum ChildPanelType
    {
        CPT_WING = 1,
        CPT_WINGCOLOR = 2,
        CPT_WINGSOUL = 3,
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
        RegChildPanel<UIWingPanel>((int) ChildPanelType.CPT_WING, mUIWingPanel.gameObject, mtog_wing);
        RegChildPanel<UIWingColorPanel>((int) ChildPanelType.CPT_WINGCOLOR, mUIWingColorPanel.gameObject, mtog_color);
        RegChildPanel<UIWingSpiritPanel>((int) ChildPanelType.CPT_WINGSOUL, mUIWingSpiritPanel.gameObject,
            mtog_wingspirit, null, () => UICheckManager.Instance.DoCheckButtonClick(FunctionType.funcP_WingSoul) );
        FuncOpenBind();
        RegisterRed(mred_wing, RedPointType.Wing);
        RegisterRed(mred_color, RedPointType.WingColor);
        RegisterRed(mred_wingspirit, RedPointType.WingSpirit);

        if (mgrid_Group != null)
        {
            mgrid_Group.repositionNow = true;
            mgrid_Group.Reposition();
        }

        SetMoneyIds(1, 4);
    }

    public override void Show()
    {
        base.Show();
        FunctionOpenStateChange(0, null);
    }

    // public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    // {
    //     mtog_wing.Set(false);
    //     mtog_color.Set(false);
    //     mtog_wingspirit.Set(false);
    //
    //     if (type != curTab)
    //     {
    //         lastTab = curTab;
    //         curTab = type;    
    //     }
    //     
    //     switch (type)
    //     {
    //         case 1:
    //             mtog_wing.value = true;
    //             break;
    //         case 2:
    //             mtog_color.value = true;
    //             break;
    //         case 3:
    //             if (!UICheckManager.Instance.DoCheckButtonClick(FunctionType.funcP_WingSoul))
    //             {
    //                 switch (lastTab)
    //                 {
    //                     case 1:
    //                         mtog_wing.value = true;
    //                         break;
    //                     case 2:
    //                         mtog_color.value = true;
    //                         break;
    //                 }
    //                 return this;    
    //             }
    //             mtog_wingspirit.value = true;
    //             break;
    //     }
    //
    //     return this;
    // }

    void FuncOpenBind()
    {
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_WingSoul, mbtn_wingspirit);
    }


    void FunctionOpenStateChange(uint id, object data)
    {
        if (mgrid_Group != null)
        {
            mgrid_Group.repositionNow = true;
            mgrid_Group.Reposition();
        }
    }
}