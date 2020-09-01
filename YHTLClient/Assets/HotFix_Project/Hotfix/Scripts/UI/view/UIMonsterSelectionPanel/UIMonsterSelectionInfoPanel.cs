using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerType
{
    None, //无归属
    Firstknife, //首刀
    Endknife, //末刀
    Output, //输出
}

public enum SelectType
{
    Monster, //怪物
    Pet, //召唤物
}

public partial class UIMonsterSelectionInfoPanel : UIBasePanel
{
    private CSAvatarInfo avatarInfo;

    private SelectType selectType = SelectType.Monster;

    public override bool ShowGaussianBlur => false;

    public override UILayerType PanelLayerType => UILayerType.Resident;

    /// <summary>
    /// 主页面活动面板是否打开
    /// </summary>
    private bool isActivesPanel = false;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.OpenSeal, SealGradeChange);
        mClientEvent.Reg((uint) CEvent.MainPlayer_LevelChange, MainPlayerLevelChange);
        mClientEvent.Reg((uint) CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
        mClientEvent.Reg((uint) CEvent.ShowMonsterSelectPanel, ShowMonsterSelectPanel);
        mClientEvent.Reg((uint) CEvent.HideMonsterSelectPanel, HideMonsterSelectPanel);
        mClientEvent.AddEvent(CEvent.ReqChoicedMonster, OnReqChoicedMonster);
        HotManager.Instance.MainEventHandler.AddEvent(MainEvent.CloseSelectionPanel, CloseMonsterSelectionPanel);

        mbtn_detailed_info.onClick = OnDetailedInfo;
        mbtn_close.onClick = OnClose;
        EventDelegate.Add(mslider_hp.onChange, OnHpSliderChangeValue);
        UIManager.Instance.ClosePanel<UIRoleSelectionInfoPanel>();
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

    void ShowMonsterSelectPanel(uint id, object data)
    {
        Panel.alpha = 1;
    }

    void HideMonsterSelectPanel(uint id, object data)
    {
        Panel.alpha = 0;
    }

    void OnReqChoicedMonster(uint id, object data)
    {
        if(null != avatarInfo)
        {
            mClientEvent.SendEvent(CEvent.SelectMonster, avatarInfo.ID);
        }
    }

    private void MoveUIMainScenePanel(uint id, object data)
    {
        if (data == null) return;
        bool isHide = (bool) data;
        SetIsActivesPanel();
        Panel.alpha = isHide ? 0 : isActivesPanel ? 0 : 1;
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUIEffect(meffectHp, 17913);
        meffectHp.SetActive(mslider_hp.value < 1);

        SetIsActivesPanel();
        if (isActivesPanel)
            Utility.ShowOrHideUIActivitiesPanel(false);

        Panel.alpha = CSScenePanelPosManager.Instance.IsHaveMove() ? 0 : 1;
    }

    void OnHpSliderChangeValue()
    {
        meffectHp.SetActive(mslider_hp.value < 1);
    }

    public void ShowSelectData(CSAvatarInfo info)
    {
        if (info == null) return;
        if (avatarInfo != null)
        {
            avatarInfo.EventHandler.RemoveEvent(CEvent.HP_Change, UpdateHp);
            avatarInfo.EventHandler.RemoveEvent(CEvent.player_levelChange, UpdateLevel);
            avatarInfo.EventHandler.RemoveEvent(CEvent.MonsterOwner_Change, UpdateRoleNameAscription);
            mClientEvent.SendEvent(CEvent.NoSelectMonster, avatarInfo.ID);
        }

        avatarInfo = info;
        TABLE.MONSTERINFO monsterinfo;
        if (MonsterInfoTableManager.Instance.TryGetValue((int) info.ConfigId, out monsterinfo))
        {
            switch (monsterinfo.type)
            {
                case 4:
                case 7:
                case 8:
                    selectType = SelectType.Pet;
                    break;
                default:
                    selectType = SelectType.Monster;
                    break;
            }
        }

        mClientEvent.SendEvent(CEvent.SelectMonster, avatarInfo.ID);
        avatarInfo.EventHandler.AddEvent(CEvent.HP_Change, UpdateHp);
        avatarInfo.EventHandler.AddEvent(CEvent.player_levelChange, UpdateLevel);
        avatarInfo.EventHandler.AddEvent(CEvent.MonsterOwner_Change, UpdateRoleNameAscription);
        if (selectType == SelectType.Monster)
            InitGrid();
        else if (selectType == SelectType.Pet)
            mgrid_dropEquip.MaxCount = 0;
        InitMonsterData();
    }

    /// <summary>
    /// 初始化选中怪物信息
    /// </summary>
    void InitMonsterData()
    {
        if (avatarInfo == null) return;
        mlb_monster_name.text = avatarInfo.Name;
        switch (MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int) avatarInfo.ConfigId))
        {
            case 1: //显示世界等级
                mlb_monster_level.text = $"{CSSealGradeInfo.Instance.GetNowWorldLevel()}{mlb_monster_level.FormatStr}";
                break;
            case 3: //显示玩家等级
                mlb_monster_level.text = $"{CSMainPlayerInfo.Instance.Level}{mlb_monster_level.FormatStr}";
                break;
            default: //显示怪物等级
                mlb_monster_level.text = $"{avatarInfo.Level}{mlb_monster_level.FormatStr}";
                break;
        }

        mlb_hp.text = $"{avatarInfo.HP}/{avatarInfo.MaxHP}";
        mtable.repositionNow = true;
        mtable.Reposition();
        float value = (float) avatarInfo.HP / avatarInfo.MaxHP;
        mslider_hp.value = value;
        mlb_role_name_ascription.gameObject.SetActive(false);
        mlb_type_ascription.gameObject.SetActive(false);

        if (selectType == SelectType.Monster)
        {
            CSMonsterInfo monsterInfo = avatarInfo as CSMonsterInfo;
            switch ((OwnerType) MonsterInfoTableManager.Instance.GetMonsterInfoOwnerType((int) monsterInfo.ConfigId))
            {
                case OwnerType.None:
                    mlb_type_ascription.gameObject.SetActive(false);
                    break;
                case OwnerType.Firstknife:
                    mlb_type_ascription.text = CSString.Format(872);
                    mlb_type_ascription.gameObject.SetActive(true);
                    break;
                case OwnerType.Endknife:
                    mlb_type_ascription.text = CSString.Format(873);
                    mlb_type_ascription.gameObject.SetActive(true);
                    break;
                case OwnerType.Output:
                    mlb_type_ascription.text = CSString.Format(874);
                    mlb_type_ascription.gameObject.SetActive(true);
                    break;
            }

            mlb_role_name_ascription.text = monsterInfo.MonsterOwner;
            mlb_role_name_ascription.gameObject.SetActive(!string.IsNullOrEmpty(monsterInfo.MonsterOwner));
        }

        //怪物头像
        if (MonsterInfoTableManager.Instance.GetMonsterInfoHead((int) avatarInfo.ConfigId) != 0)
        {
            msp_monster_head.spriteName = MonsterInfoTableManager.Instance
                .GetMonsterInfoHead((int) avatarInfo.ConfigId).ToString();
        }
        else
        {
            msp_monster_head.gameObject.SetActive(false);
        }
    }

    List<UIItemBase> listItemBases = new List<UIItemBase>();
    private UIItemBase uiItemBase;

    void InitGrid()
    {
        //奖励重新定规则
        int pro = MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int) avatarInfo.ConfigId);
        CSBetterLisHot<int> listInfos = new CSBetterLisHot<int>();
        MDropItemsTableManager.Instance.GetDropItemsByMonsterId((int) avatarInfo.ConfigId, listInfos, 3);
        if (listInfos != null)
        {
            mgrid_dropEquip.MaxCount = listInfos.Count;
            GameObject gp;
            for (int i = 0; i < mgrid_dropEquip.MaxCount; i++)
            {
                gp = mgrid_dropEquip.controlList[i];
                if (listItemBases.Count <= i)
                    listItemBases.Add(
                        UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size26));
                uiItemBase = listItemBases[i];
                TABLE.ITEM Item;
                if (ItemTableManager.Instance.TryGetValue(listInfos[i], out Item))
                {
                    uiItemBase.IsShowWoLongSeal = false;
                    uiItemBase.IsHuaijiuIcon = false;
                    uiItemBase.isShowNormalSuit = false;
                    uiItemBase.Refresh(Item, needTips: false);
                }
            }
        }
    }

    /// <summary>
    /// 更新等级
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdateLevel(uint id, object data)
    {
        CSStringBuilder.Clear();
        mlb_monster_level.text = CSStringBuilder.Append(avatarInfo.Level, mlb_monster_level.FormatStr).ToString();
        mtable.repositionNow = true;
        mtable.Reposition();
    }

    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdateHp(uint id, object data)
    {
        // if (data == null) return;
        CSStringBuilder.Clear();
        mlb_hp.text = CSStringBuilder.Append(avatarInfo.HP, '/', avatarInfo.MaxHP).ToString();
        float value = (float) avatarInfo.HP / avatarInfo.MaxHP;
        mslider_hp.value = value;
    }

    /// <summary>
    /// 更新怪物归属玩家名字
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdateRoleNameAscription(uint id, object data)
    {
        // if (data == null) return;
        if (selectType == SelectType.Monster)
        {
            CSMonsterInfo monsterInfo = avatarInfo as CSMonsterInfo;
            mlb_role_name_ascription.text = monsterInfo.MonsterOwner;
            mlb_role_name_ascription.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 打开查看选中怪物详细信息界面
    /// </summary>
    /// <param name="go"></param>
    void OnDetailedInfo(GameObject go)
    {
        if (selectType == SelectType.Monster)
        {
            UIManager.Instance.CreatePanel<UIMonsterSelectionDetailedInfoPanel>(
                (f) => { (f as UIMonsterSelectionDetailedInfoPanel).ShowMonsterDetailedData(avatarInfo); });
        }
    }

    /// <summary>
    /// 关闭怪物选中面板
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void CloseMonsterSelectionPanel(uint id, object data)
    {
        Close();
        SetIsActivesPanel();
        Utility.ShowOrHideUIActivitiesPanel(isActivesPanel);
    }

    void SealGradeChange(uint id, object data)
    {
        if (avatarInfo != null)
        {
            if (MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int) avatarInfo.ConfigId) == 1)
                mlb_monster_level.text = $"{CSSealGradeInfo.Instance.GetNowWorldLevel()}{mlb_monster_level.FormatStr}";
        }
    }

    void MainPlayerLevelChange(uint id, object data)
    {
        if (avatarInfo != null)
        {
            if (MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int) avatarInfo.ConfigId) == 3)
                mlb_monster_level.text = $"{CSMainPlayerInfo.Instance.Level}{mlb_monster_level.FormatStr}";
        }
    }

    void OnClose(GameObject go)
    {
        Close();
        SetIsActivesPanel();
        Utility.ShowOrHideUIActivitiesPanel(isActivesPanel);
        if (avatarInfo != null)
        {
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(avatarInfo.ID);
            if (avatar != null)
            {
                avatar.DestroyBottom();
            }
        }
    }

    protected override void OnDestroy()
    {
        if(null != avatarInfo)
            mClientEvent.SendEvent(CEvent.NoSelectMonster, avatarInfo.ID);

        CSEffectPlayMgr.Instance.Recycle(meffectHp);
        HotManager.Instance.MainEventHandler.UnReg((uint) MainEvent.CloseSelectionPanel, CloseMonsterSelectionPanel);
        if (avatarInfo != null)
        {
            avatarInfo.EventHandler.RemoveEvent(CEvent.HP_Change, UpdateHp);
            avatarInfo.EventHandler.RemoveEvent(CEvent.player_levelChange, UpdateLevel);
            avatarInfo.EventHandler.RemoveEvent(CEvent.MonsterOwner_Change, UpdateRoleNameAscription);
        }
        
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
        base.OnDestroy();
    }
}