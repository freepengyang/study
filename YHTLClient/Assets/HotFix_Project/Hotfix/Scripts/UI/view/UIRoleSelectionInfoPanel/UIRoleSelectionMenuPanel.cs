using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIRoleSelectionMenuPanel : UIBasePanel
{

    MenuInfo menuInfo = null;
    UISelectionMenuPanel uiSelectionMenuPanel;

    public override bool ShowGaussianBlur { get => false; }
    
    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
    }

    
    public override void Show()
    {
        base.Show();
    }
    
    
    public void ShowSelectData(MenuInfo info)
    {
        if (info == null) return;
        menuInfo = info;
        if (uiSelectionMenuPanel == null)
        {
            uiSelectionMenuPanel = new UISelectionMenuPanel();
            uiSelectionMenuPanel.UIPrefab = mSelectionMenuPanel;
            uiSelectionMenuPanel.Init();
            uiSelectionMenuPanel.Show();
            uiSelectionMenuPanel.ShowBtnsData(info, mbg_head);
        }
        else
        {
            uiSelectionMenuPanel.Show();
            uiSelectionMenuPanel.ShowBtnsData(info, mbg_head);
        }
        uiSelectionMenuPanel.closeSelectionPanel = Close;
        InitRoleData();
    }

    /// <summary>
    /// 初始化聊天框选中人物信息
    /// </summary>
    void InitRoleData()
    {
        mlb_role_name.text = menuInfo.roleName;
        CSStringBuilder.Clear();
        mlb_role_level.text = menuInfo.lv.ToString();
        msp_role_head.spriteName = Utility.GetPlayerIcon(menuInfo.sex, menuInfo.career);
        mlb_career.text = Utility.GetCareerName(menuInfo.career);
    }

}
