using System.Collections.Generic;
using UnityEngine;

public partial class UIRedNameNPCDialogPanel : UIBasePanel
{

    int leaveLimit;

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.NpcDialog;
    }
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mobj_buy).onClick = BuyBtnClick;
        UIEventListener.Get(mbtn_leave).onClick = LeaveBtnClick;
        List<int> list = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(107));
        leaveLimit = list[0];
        mlb_des.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(663), CSMainPlayerInfo.Instance.PkValue, list[0]);
    }

    public override void Show()
    {
        base.Show();

        bool canLeave = CSMainPlayerInfo.Instance.PkValue < leaveLimit;

        msp_btnGo.spriteName = canLeave ? "btn_nbig1" : "btn_nbig3";
        mlb_btnGo.color = UtilityColor.HexToColor(canLeave ? "#b0bbcf" : "#888580");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    void BuyBtnClick(GameObject _go)
    {
        UtilityPanel.JumpToPanel(12301);
        UIManager.Instance.ClosePanel<UIRedNameNPCDialogPanel>();
    }
    void LeaveBtnClick(GameObject _go)
    {
        if (CSMainPlayerInfo.Instance.PkValue >= leaveLimit)
        {
            UtilityTips.ShowRedTips(1837);
            return;
        }
        Net.CSBackCityMessage();
        UIManager.Instance.ClosePanel<UIRedNameNPCDialogPanel>();
    }
}
