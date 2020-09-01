using System;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using Unity.Collections;
using UnityEngine;

public partial class UIMonsterSelectionDetailedInfoPanel : UIBasePanel
{
    List<UIItemBase> itemList = new List<UIItemBase>();

    private CSAvatarInfo avatarInfo;

    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mtx_montser, "boss_level_2");
    }

    public void ShowMonsterDetailedData(CSAvatarInfo info)
    {
        if (info == null) return;
        avatarInfo = info;
        mlb_monster_name.text = $"[ff9000]{info.Name}[-]";
        switch (MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int)avatarInfo.ConfigId))
        {
            case 1://显示世界等级
                mlb_monster_level.text = CSString.Format(mlb_monster_level.FormatStr, CSSealGradeInfo.Instance.GetNowWorldLevel());
                break;
            case 3://显示玩家等级
                mlb_monster_level.text = CSString.Format(mlb_monster_level.FormatStr, CSMainPlayerInfo.Instance.Level);
                break;
            default://显示怪物等级
                mlb_monster_level.text = CSString.Format(mlb_monster_level.FormatStr, avatarInfo.Level);
                break;
        }
        if (MonsterInfoTableManager.Instance.GetMonsterInfoHead((int) avatarInfo.ConfigId) != 0)
        {
            msp_monster_head.spriteName = MonsterInfoTableManager.Instance.GetMonsterInfoHead((int) avatarInfo.ConfigId)
                .ToString();
        }
        else
        {
            msp_monster_head.gameObject.SetActive(false);
        }

        InitGrid();
    }
    
    
    List<UIItemBase> listItemBases = new List<UIItemBase>();
    private UIItemBase uiItemBase;
    /// <summary>
    /// 奖励
    /// </summary>
    void InitGrid()
    {
        int pro = MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit((int)avatarInfo.ConfigId);
        //CSBetterLisHot<int> listInfos =
        //    MDropItemsTableManager.Instance.GetItemsInfoByBossDropId((int)avatarInfo.ConfigId, pro, mPoolHandleManager);

        CSBetterLisHot<int> infoList = new CSBetterLisHot<int>();
        MDropItemsTableManager.Instance.GetDropItemsByMonsterId((int)avatarInfo.ConfigId, infoList);
        mgrid_dropEquip.MaxCount = infoList.Count;
        GameObject gp;
        for (int i = 0; i < mgrid_dropEquip.MaxCount; i++)
        {
            gp = mgrid_dropEquip.controlList[i];
            if (listItemBases.Count<=i)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size64));
            uiItemBase = listItemBases[i];
            TABLE.ITEM Item;
            if (ItemTableManager.Instance.TryGetValue(infoList[i], out Item))
            {
                uiItemBase.Refresh(Item);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        itemList.Clear();
        CSEffectPlayMgr.Instance.Recycle(mtx_montser);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
    }

    //奖励结构
    class SortReward
    {
        public int configId = 0; //物品配置Id
        public int equipTypeId = 0; // 1.卧龙道具 2.普通装备 3.道具
        public int quality = 0; //物品品质
        public int level = 0; //物品等级
    }
}