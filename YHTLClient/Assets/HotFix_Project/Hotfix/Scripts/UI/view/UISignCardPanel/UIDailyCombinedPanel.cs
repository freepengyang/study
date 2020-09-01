using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailyCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    private enum DailyPanelType
    {
        SignIn,
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
        mbtn_close.onClick = CloseBtnClick;

        RegChildPanel<UIDailySignInCombinedPanel>((int)DailyPanelType.SignIn, mobj_signInPanel, mtg_signIn);
    }

    void CloseBtnClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIDailyCombinedPanel>();
    }
}
