using System.Collections.Generic;
using UnityEngine;
using System;


public partial class UICompleteWayPanel : UIBasePanel
{
    ILBetterList<GetWayData> getWayList;

    public override void Init()
    {
        base.Init();

        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);
        mClientEvent.AddEvent(CEvent.FastAccessTransferNpc, CloseEvent);

        mbtn_bg.onClick = (go) => { Close(); };
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        mgrid_btn.UnBind<GetWayBtn>();
        getWayList?.Clear();
        getWayList = null;

        base.OnDestroy();
    }

    void CloseEvent(uint id, object param)
    {
        Close();
    }


    public void OpenPanel(string idStr, Vector2 worldPos, AnchorType anchor, int titleType)
    {
        mtrans_view.CustomActive(false);
        ShowBtns(idStr);

        Vector2 pos = mtrans_viewParent.InverseTransformPoint(worldPos);
        SetViewPos(pos, (int)anchor, Vector2.zero);

        mtrans_view.CustomActive(true);
    }


    public void OpenPanelSelfAdapt<T>(string idStr, T target, AnchorType precedenceAnchor = AnchorType.TopCenter, int _titleType = 0) where T : UIWidget
    {
        //titleType=0  完成途径  =1获取途径
        if (_titleType == 0)
        {
            mlb_title.text = ClientTipsTableManager.Instance.GetClientTipsContext(2035);
        }
        else if (_titleType == 1)
        {
            mlb_title.text = ClientTipsTableManager.Instance.GetClientTipsContext(2036);
        }
        mtrans_view.CustomActive(false);
        ShowBtns(idStr);
        var guiCamera = NGUITools.FindCameraForLayer(UIPrefab.layer);

        int precedenceX = (int)precedenceAnchor / 10;
        int precedenceY = (int)precedenceAnchor % 10;

        var targetWorldPos = target.transform.position;
        Vector2 localInitPos = mtrans_viewParent.InverseTransformPoint(targetWorldPos);

        float offsetX = 0f;
        float offsetY = 0f;

        if (precedenceX == 1)//锚点在左，方向则是朝右
        {
            var rightBorder = guiCamera.ViewportToWorldPoint(new Vector3(1, 0)).x;
            var viewRight = mtrans_viewParent.TransformPoint(localInitPos + new Vector2(target.width * 0.5f + msp_bg.width, 0f)).x;
            precedenceX = viewRight >= rightBorder ? 3 : 1;
            offsetX = viewRight >= rightBorder ? -target.width * 0.5f : target.width * 0.5f;
        }
        else if (precedenceX == 3)//反之
        {
            var leftBorder = guiCamera.ViewportToWorldPoint(new Vector3(0, 0)).x;
            var viewLeft = mtrans_viewParent.TransformPoint(localInitPos + new Vector2(-target.width * 0.5f - msp_bg.width, 0f)).x;
            precedenceX = viewLeft <= leftBorder ? 1 : 3;
            offsetX = viewLeft <= leftBorder ? target.width * 0.5f : -target.width * 0.5f;
        }

        if (precedenceY == 1)//锚点在上，方向则是朝下
        {
            var bottomBorder = guiCamera.ViewportToWorldPoint(new Vector3(0, 0)).y;
            var viewBottom = mtrans_viewParent.TransformPoint(localInitPos + new Vector2(0f, -target.height * 0.5f - msp_bg.height)).y;
            precedenceY = viewBottom <= bottomBorder ? 3 : 1;
            offsetY = viewBottom <= bottomBorder ? target.height * 0.5f : -target.height * 0.5f;
        }
        else if (precedenceY == 3)//反之
        {
            var topBorder = guiCamera.ViewportToWorldPoint(new Vector3(0, 1)).y;
            var viewTop = mtrans_viewParent.TransformPoint(localInitPos + new Vector2(0f, target.height * 0.5f + msp_bg.height)).y;
            precedenceY = viewTop >= topBorder ? 1 : 3;
            offsetY = viewTop >= topBorder ? -target.height * 0.5f : target.height * 0.5f;
        }

        SetViewPos(localInitPos, precedenceX * 10 + precedenceY, new Vector2(offsetX, offsetY));

        mtrans_view.CustomActive(true);
    }


    void ShowBtns(string idStr)
    {
        if (getWayList == null) getWayList = new ILBetterList<GetWayData>();
        else getWayList.Clear();
        CSGetWayInfo.Instance.GetGetWays(idStr, ref getWayList);
        mgrid_btn.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);

        int cellHeight = (int)mgrid_btn.CellHeight * mgrid_btn.MaxCount;
        msp_bg.height = 48 + cellHeight;
        msp_bgButton.height = 4 + cellHeight;

    }


    void SetViewPos(Vector2 localPos, int anchor, Vector2 offset)
    {
        int anchorX = (int)anchor / 10;
        int anchorY = (int)anchor % 10;

        float selfOffsetX = anchorX < 2 ? msp_bg.width * 0.5f : anchorX > 2 ? -msp_bg.width * 0.5f : 0;
        float selfOffsetY = anchorY < 2 ? 0 : anchorY > 2 ? msp_bg.height * 1f : msp_bg.height * 0.5f;

        mtrans_view.localPosition = localPos + new Vector2(selfOffsetX, selfOffsetY) + offset;
    }

}

