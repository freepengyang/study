using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIConfigPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
	private enum ConfigPanelType
    {
        Base,
        HangUp,
        Graphics,
        Feedback,
        Push,
    }
    

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = CloseBtnClick;

        RegChildPanel<UIConfigBasePanel>((int)ConfigPanelType.Base, mobj_basePanel, mtg_base);
        RegChildPanel<UIConfigHangUpPanel>((int)ConfigPanelType.HangUp, mobj_hangUpPanel, mtg_hangup);
        RegChildPanel<UIConfigGraphicsPanel>((int)ConfigPanelType.Graphics, mobj_graphicPanel, mtg_graphics);
        RegChildPanel<UIConfigFeedbackPanel>((int)ConfigPanelType.Feedback, mobj_feedbackPanel, mtg_feedback);
        RegChildPanel<UIConfigPushSetPanel>((int)ConfigPanelType.Push, mobj_pushSetPanel, mtg_push);
    }

    public override void Show()
    {
        base.Show();

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //CSConfigInfo.Instance.SaveConfig();
    }
         


    void CloseBtnClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIConfigPanel>();
    }

}
