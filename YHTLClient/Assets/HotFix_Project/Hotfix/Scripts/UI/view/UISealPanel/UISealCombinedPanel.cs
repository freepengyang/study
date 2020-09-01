using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UISealCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType { get => PrefabTweenType.FirstPanel; }
    
    public enum SealPanelTye
    {
        SealGrade = 1,//等级封印
        Dreamland = 2,//幻境
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
        RegChildPanel<UISealGradePanel>((int)SealPanelTye.SealGrade, mSealGradePanel.gameObject, mtog_seal_grade);
        RegChildPanel<UIDreamLandPanel>((int)SealPanelTye.Dreamland, mDreamLandPanel.gameObject, mtog_dreamland);
        
        RegisterRed(mredpoint_dreamland, RedPointType.DreamLand);

        SetMoneyIds(1, 4);
    }

    public void SetActiveTogSealGrade(bool isActive)
    {
        mtog_seal_grade.gameObject.SetActive(isActive);
        mgrid_tab.repositionNow = true;
        mgrid_tab.Reposition();
    }
}
