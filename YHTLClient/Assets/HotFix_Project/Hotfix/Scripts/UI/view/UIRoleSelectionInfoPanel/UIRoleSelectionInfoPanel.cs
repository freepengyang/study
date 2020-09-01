using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using fight;
using UnityEngine;

public partial class UIRoleSelectionInfoPanel : UIBasePanel
{
    MenuInfo menuInfo;
    private CSPlayerInfo playerInfo;
    UISelectionMenuPanel uiSelectionMenuPanel;
    Action<GameObject> closeSelectionPanel;

    /// <summary>
    /// 是否在地下寻宝副本地图
    /// </summary>
    private bool isDiXiaXunBao = false;

    public override bool ShowGaussianBlur => false;

    public override UILayerType PanelLayerType => UILayerType.Resident;

    /// <summary>
    /// 主页面活动面板是否打开
    /// </summary>
    private bool isActivesPanel = false;

    public override void Init()
    {
        base.Init();
        isDiXiaXunBao = Utility.IsInMap(ESpecialMap.DiXiaXunBao);
        mClientEvent.Reg((uint) CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
        mClientEvent.Reg((uint) CEvent.ShowRoleSelectPanel, ShowRoleSelectPanel);
        mClientEvent.Reg((uint) CEvent.HideRoleSelectPanel, HideRoleSelectPanel);
        mClientEvent.AddEvent(CEvent.ReqChoicedTeamPlayer, OnReqChoicedTeamPlayer);
        HotManager.Instance.MainEventHandler.AddEvent(MainEvent.CloseSelectionPanel, CloseRoleSelectionPanel);
        mbtn_buff_info.onClick = OnClickBuffPanel;
        mbtn_close.onClick = OnClose;
        if (!isDiXiaXunBao)
            mbtn_detailed_info.onClick = OnShowButtons;
        EventDelegate.Add(mslider_hp.onChange, OnHpSliderChangeValue);
        EventDelegate.Add(mslider_mp.onChange, OnMpSliderChangeValue);
        UIManager.Instance.ClosePanel<UIMonsterSelectionInfoPanel>();
        SetIsActivesPanel();
    }

    void SetIsActivesPanel()
    {
        UIMainSceneManager mainSceneManager = UIManager.Instance.GetPanel<UIMainSceneManager>();
        if (null != mainSceneManager)
        {
            UIBase uiBase;
            if (mainSceneManager._RegisterPanel.TryGetValue(typeof(UIActivitiesPanel), out uiBase))
            {
                UIActivitiesPanel uiActivitiesPanel = uiBase as UIActivitiesPanel;
                isActivesPanel = uiActivitiesPanel.IsShowActivities;
            }
        }
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUIEffect(meffectHp, 17913);
        CSEffectPlayMgr.Instance.ShowUIEffect(meffectMp, 17914);
        meffectHp.SetActive(mslider_hp.value < 1);
        meffectMp.SetActive(mslider_mp.value < 1);

        SetIsActivesPanel();
        if (isActivesPanel)
            Utility.ShowOrHideUIActivitiesPanel(false);

        Panel.alpha = CSScenePanelPosManager.Instance.IsHaveMove() ? 0 : 1;
    }

    void OnHpSliderChangeValue()
    {
        meffectHp.SetActive(mslider_hp.value < 1);
    }

    void OnMpSliderChangeValue()
    {
        meffectMp.SetActive(mslider_mp.value < 1);
    }

    public void ShowSelectData(CSAvatarInfo info)
    {
        if (info == null) return;
        if (playerInfo != null)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.NoSelectLastMyTeamPlayer, playerInfo.ID);
            playerInfo.EventHandler.RemoveEvent(CEvent.HP_Change, UpdateHp);
            playerInfo.EventHandler.RemoveEvent(CEvent.MP_Change, UpdateMp);
            playerInfo.EventHandler.RemoveEvent(CEvent.player_levelChange, UpdateLevel);
            playerInfo.EventHandler.RemoveEvent(CEvent.Buff_Add_OtherPlayer, RefreshData);
            playerInfo.EventHandler.RemoveEvent(CEvent.Buff_Remove_OtherPlayer, RefreshData);
        }

        playerInfo = info as CSPlayerInfo;
        HotManager.Instance.EventHandler.SendEvent(CEvent.SelectLastMyTeamPlayer, playerInfo.ID);
        //局部监听
        playerInfo.EventHandler.AddEvent(CEvent.HP_Change, UpdateHp);
        playerInfo.EventHandler.AddEvent(CEvent.MP_Change, UpdateMp);
        playerInfo.EventHandler.AddEvent(CEvent.player_levelChange, UpdateLevel);
        playerInfo.EventHandler.AddEvent(CEvent.Buff_Add_OtherPlayer, RefreshData);
        playerInfo.EventHandler.AddEvent(CEvent.Buff_Remove_OtherPlayer, RefreshData);
        if (uiSelectionMenuPanel == null)
        {
            uiSelectionMenuPanel = new UISelectionMenuPanel();
            uiSelectionMenuPanel.UIPrefab = mUISelectionMenuPanel;
            uiSelectionMenuPanel.Init();
            uiSelectionMenuPanel.Show();
            uiSelectionMenuPanel.ShowBtnsData(menuInfo);
        }
        else
        {
            uiSelectionMenuPanel.Show();
            uiSelectionMenuPanel.ShowBtnsData(menuInfo);
        }

        uiSelectionMenuPanel.closeSelectionPanel = OnHideButtons;
        mUISelectionMenuPanel.SetActive(false);
        mUIBuffPanel.SetActive(false);
        mgrid_dropEquip.gameObject.SetActive(true);
        InitRoleData();
        InitBufferData();
    }

    void RefreshData(uint id, object data)
    {
        InitBufferData();
    }

    private CSBuffInfo buffInfo;
    private ILBetterList<BufferInfo> listBuffInfo = new ILBetterList<BufferInfo>();
    ILBetterList<UIItemBuff> listItemBuff = new ILBetterList<UIItemBuff>();
    ILBetterList<Schedule> listSchedule = new ILBetterList<Schedule>();

    void InitBufferData()
    {
        if (playerInfo == null) return;
        buffInfo = playerInfo.BuffInfo;
        listBuffInfo.Clear();

        if (buffInfo.buffIdList != null)
        {
            for (int i = 0; i < buffInfo.buffIdList.Count; ++i)
            {
                if (BufferTableManager.Instance.GetBufferShowOrHide(buffInfo.buffIdList[i]) == 1)
                {
                    BufferInfo info = buffInfo.GetBuff(buffInfo.buffIdList[i]);
                    if (info != null)
                        listBuffInfo.Add(info);
                }
            }
        }

        mgrid_dropEquip.MaxCount = Mathf.Min(listBuffInfo.Count, 6);
        GameObject go;
        for (int i = 0, max = mgrid_dropEquip.MaxCount; i < max; i++)
        {
            go = mgrid_dropEquip.controlList[i];
            var eventHandle = UIEventListener.Get(go);
            UIRoleSelectionBufferBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIRoleSelectionBufferBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIRoleSelectionBufferBinder;
            }

            Binder.Bind(listBuffInfo[i]);
        }

        if (listBuffInfo.Count == 0)
        {
            mobj_nonbuff.SetActive(true);
            mgrid_buff.gameObject.SetActive(false);
            msp_bg.height = 177;
            mgrid_dropEquip.MaxCount = 0;
        }
        else
        {
            mobj_nonbuff.SetActive(false);
            mgrid_buff.gameObject.SetActive(true);
            mgrid_buff.MaxCount = listBuffInfo.Count;
            GameObject gp;
            int height = 0;
            int allheight = 0;
            Vector3 initPosition = Vector3.zero;
            UIItemBuff itemBuff;
            for (int i = 0; i < listItemBuff.Count; i++)
            {
                if (Timer.Instance.IsInvoking(listItemBuff[i].itemSchedule))
                    Timer.Instance.CancelInvoke(listItemBuff[i].itemSchedule);
            }

            listItemBuff.Clear();
            for (int i = 0; i < mgrid_buff.MaxCount; i++)
            {
                gp = mgrid_buff.controlList[i];

                if (listItemBuff.Count <= i)
                {
                    itemBuff = new UIItemBuff(gp);
                    listItemBuff.Add(itemBuff);
                }

                listItemBuff[i].Refresh(listBuffInfo[i]);
                listItemBuff[i].ShowOrHideLine(!(i == mgrid_buff.MaxCount - 1));

                if (listItemBuff[i].itemSchedule != null)
                {
                    listSchedule.Add(listItemBuff[i].itemSchedule);
                }

                //自适应高度
                if (i != 0)
                {
                    gp.transform.localPosition = initPosition - Vector3.up * height;
                }

                initPosition = gp.transform.localPosition;
                height = listItemBuff[i].Height;
                allheight += height;
                allheight += 2;
            }

            int difference = 0;
            if (allheight <= 177)
            {
                // difference = allheight - msp_bg.height;
                msp_bg.height = allheight;
                // mgrid_buff.transform.localPosition += difference * Vector3.up;
            }
            else
            {
                // difference = 177 - msp_bg.height;
                msp_bg.height = 177;
                // mgrid_buff.transform.localPosition += difference * Vector3.up;
            }

            CSGame.Sington.StartCoroutine(SetScrollValue());
        }
    }

    IEnumerator SetScrollValue()
    {
        yield return null;
        mScrollView_buff.ResetPosition();
    }

    void OpenSelectionMenuPanel()
    {
        if (uiSelectionMenuPanel != null)
        {
            menuInfo = new MenuInfo();
            menuInfo.sundryId = 391;
            menuInfo.roleName = playerInfo.Name;
            menuInfo.roleId = playerInfo.ID;
            menuInfo.hp = playerInfo.HP;
            menuInfo.maxHp = playerInfo.MaxHP;
            menuInfo.maxMp = playerInfo.MaxMP;
            menuInfo.mp = playerInfo.MP;
            menuInfo.career = playerInfo.Career;
            menuInfo.sex = playerInfo.Sex;
            menuInfo.lv = playerInfo.Level;
            menuInfo.teamId = playerInfo.TeamId;

            uiSelectionMenuPanel.ShowBtnsData(menuInfo);
        }
    }

    void ShowRoleSelectPanel(uint id, object data)
    {
        Panel.alpha = 1;
    }

    void HideRoleSelectPanel(uint id, object data)
    {
        Panel.alpha = 0;
    }

    void OnReqChoicedTeamPlayer(uint id, object data)
    {
        if(null != playerInfo)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.SelectLastMyTeamPlayer, playerInfo.ID);
        }
    }

    /// <summary>
    /// 更新等级
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdateLevel(uint id, object data)
    {
        // if (data == null) return;
        //等级
        CSStringBuilder.Clear();
        mlb_role_level.text = CSStringBuilder.Append(playerInfo.Level, mlb_role_level.FormatStr).ToString();
    }

    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdateHp(uint id, object data)
    {
        // if (data == null) return;
        //血条
        CSStringBuilder.Clear();
        mlb_hp.text = CSStringBuilder.Append(playerInfo.HP, '/', playerInfo.MaxHP).ToString();
        mslider_hp.value = (float) playerInfo.HP / playerInfo.MaxHP;
    }

    void UpdateMp(uint id, object data)
    {
        // if (data == null) return;
        //蓝条
        CSStringBuilder.Clear();
        mlb_mp.text = CSStringBuilder.Append(playerInfo.MP, '/', playerInfo.MaxMP).ToString();
        mslider_mp.value = (float) playerInfo.MP / playerInfo.MaxMP;
    }

    void OnHideButtons(GameObject go)
    {
        mUISelectionMenuPanel.SetActive(false);
        mgrid_dropEquip.gameObject.SetActive(true);
        mUIBuffPanel.SetActive(false);
    }

    void OnShowButtons(GameObject go)
    {
        OpenSelectionMenuPanel();
        mUISelectionMenuPanel.SetActive(!mUISelectionMenuPanel.activeSelf);
        mgrid_dropEquip.gameObject.SetActive(!mUISelectionMenuPanel.activeSelf);
        mUIBuffPanel.SetActive(false);
    }

    /// <summary>
    /// 初始化选中人物信息
    /// </summary>
    void InitRoleData()
    {
        //头像
        msp_role_head.spriteName = Utility.GetPlayerIcon(playerInfo.Sex, playerInfo.Career);
        //名字
        mlb_role_name.text = isDiXiaXunBao ? CSString.Format(1712) : playerInfo.Name;
        //等级
        mlb_role_level.gameObject.SetActive(!isDiXiaXunBao);
        CSStringBuilder.Clear();
        mlb_role_level.text = CSStringBuilder.Append(playerInfo.Level, mlb_role_level.FormatStr).ToString();
        //血条
        mlb_hp.gameObject.SetActive(!isDiXiaXunBao);
        CSStringBuilder.Clear();
        mlb_hp.text = CSStringBuilder.Append(playerInfo.HP, '/', playerInfo.MaxHP).ToString();
        mslider_hp.value = (float) playerInfo.HP / playerInfo.MaxHP;
        //蓝条
        mlb_mp.gameObject.SetActive(!isDiXiaXunBao);
        CSStringBuilder.Clear();
        mlb_mp.text = CSStringBuilder.Append(playerInfo.MP, '/', playerInfo.MaxMP).ToString();
        mslider_mp.value = (float) playerInfo.MP / playerInfo.MaxMP;
    }

    private void MoveUIMainScenePanel(uint id, object data)
    {
        if (data == null) return;
        bool isHide = (bool) data;
        SetIsActivesPanel();
        Panel.alpha = isHide ? 0 : isActivesPanel ? 0 : 1;
    }

    /// <summary>
    /// 关闭人物选中面板
    /// </summary>
    void CloseRoleSelectionPanel(uint id, object data)
    {
        Close();
        SetIsActivesPanel();
        Utility.ShowOrHideUIActivitiesPanel(isActivesPanel);
    }

    void OnClickBuffPanel(GameObject go)
    {
        mUIBuffPanel.SetActive(!mUIBuffPanel.activeSelf);
    }

    void OnClose(GameObject go)
    {
        Close();
        SetIsActivesPanel();
        Utility.ShowOrHideUIActivitiesPanel(isActivesPanel);

        if (playerInfo != null)
        {
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(playerInfo.ID);
            if (avatar != null)
            {
                avatar.DestroyBottom();
            }
        }
    }


    protected override void OnDestroy()
    {
        HotManager.Instance.MainEventHandler.UnReg((uint) MainEvent.CloseSelectionPanel, CloseRoleSelectionPanel);
        if (playerInfo != null)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.NoSelectLastMyTeamPlayer, playerInfo.ID);
            playerInfo.EventHandler.RemoveEvent(CEvent.HP_Change, UpdateHp);
            playerInfo.EventHandler.RemoveEvent(CEvent.MP_Change, UpdateMp);
            playerInfo.EventHandler.RemoveEvent(CEvent.player_levelChange, UpdateLevel);
            playerInfo.EventHandler.RemoveEvent(CEvent.Buff_Add_OtherPlayer, RefreshData);
            playerInfo.EventHandler.RemoveEvent(CEvent.Buff_Remove_OtherPlayer, RefreshData);
        }
        mgrid_dropEquip.UnBind<UIRoleSelectionBufferBinder>();
        CSEffectPlayMgr.Instance.Recycle(meffectHp);
        CSEffectPlayMgr.Instance.Recycle(meffectMp);
        for (int i = 0; i < listItemBuff.Count; i++)
        {
            if (Timer.Instance.IsInvoking(listItemBuff[i].itemSchedule))
                Timer.Instance.CancelInvoke(listItemBuff[i].itemSchedule);
        }
        listItemBuff.Clear();
        base.OnDestroy();
    }
}

public class UIRoleSelectionBufferBinder : UIBinder
{
    private UISprite sp_buff;
    private BufferInfo bufferInfo;

    public override void Init(UIEventListener handle)
    {
        sp_buff = handle.GetComponent<UISprite>();
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        bufferInfo = (BufferInfo) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        sp_buff.spriteName = BufferTableManager.Instance.GetBufferIcon(bufferInfo.bufferId);
    }

    public override void OnDestroy()
    {
        sp_buff = null;
        bufferInfo = null;
    }
}