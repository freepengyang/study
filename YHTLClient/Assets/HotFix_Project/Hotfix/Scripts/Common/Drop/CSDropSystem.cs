using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PickItemLimit
{
    public int serverOpenDay;
    public int roleLevel;
    public int roleReincarnation;
    public int itemLevel;
    public int itemReincarnation;
    public float autoIntervalTime;
    public float intervalTime;
    public float standDetectIntervalTime;
}

public class CSDropSystem : CSInfo<CSDropSystem>
{
    private float NextAutoPickItemTime { get; set; }

    private float NextPickItemTime { get; set; }

    private float NextStandPickItemTime { get; set; }

    private PickItemLimit mPickItemLimit = new PickItemLimit();

    /// <summary>
    /// <pickUpType, priority>
    /// </summary>
    public Dictionary<int, int> PickUpTypeProrityDic = new Dictionary<int, int>()
    {
        { PickUpType.WoLongEquip,90},
        { PickUpType.NormalEuip,70},
        { PickUpType.Materials,50},
        { PickUpType.Currency,30},
        { PickUpType.Other,10},
        { PickUpType.RecoverDrug,1},
    };

    public float PickItemIntervalTime
    {
        get
        {
            return mPickItemLimit.intervalTime;
        }
    }

    public CSDropSystem()
    {
        mClientEvent.AddEvent(CEvent.OnPickupItemMessage,OnPickupItemMessage);
        InitData();
    }

    public void Init()
    {
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MainPlayer_CellChangeTrigger, OnMainPlayerCellChangeTrigger);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerCellStopTrigger);
    }

    private void InitData()
    {
        List<int> list = CSSundryData.SplitIntToList(491);
        mPickItemLimit.autoIntervalTime = (list.Count > 0) ? list[0] * 0.001f : 0;
        mPickItemLimit.intervalTime = (list.Count > 1) ? list[1] * 0.001f : 0.5f;
        mPickItemLimit.standDetectIntervalTime = (list.Count > 2) ? list[2] * 0.001f : 1.0f;
        mPickItemLimit.roleLevel = 0;
        mPickItemLimit.itemLevel = 0;
        mPickItemLimit.itemReincarnation = 0;
        mPickItemLimit.serverOpenDay = 0;
        mPickItemLimit.roleReincarnation = 0;

        string str = SundryTableManager.Instance.GetSundryEffect(1154);
        PickUpTypeProrityDic = UtilityMainMath.SplitStringToIntDic(str);

        //list = CSSundryData.SplitIntToList(850);
        //if (list.Count > 0)
        //{
        //    mPickItemLimit.serverOpenDay = list[0];
        //}
        //if (list.Count > 1)
        //{
        //    mPickItemLimit.roleLevel = list[1] % 1000;
        //    mPickItemLimit.roleReincarnation = list[1] / 1000;
        //}
        //if(list.Count > 2)
        //{
        //    mPickItemLimit.itemLevel = list[2] % 1000;
        //    mPickItemLimit.itemReincarnation = list[2] / 1000;
        //}
    }

    #region 道具拾取

    public void DetectPickItemStand()
    {
        if(Time.time >= NextStandPickItemTime)
        {
            DetectPickItem();
            NextStandPickItemTime = Time.time + mPickItemLimit.standDetectIntervalTime;
        }
    }

    public void DetectPickItem()
    {
        if (CSScene.IsLanuchMainPlayer)
        {
            CSCell oldCell =CSAvatarManager.MainPlayer.OldCell;
            //CSMainPlayerInfo.Instance.EventHandler.SendEvent(CEvent.MainPlayer_CellChangeTrigger, oldCell);
            if(oldCell != null)
            {
                DetectPickItem(oldCell.Coord);
            }
        }
    }

    public void DetectPickItem(CSItem item)
    {
        if (CSScene.IsLanuchMainPlayer)
        {
            CSCell oldCell =CSAvatarManager.MainPlayer.OldCell;
            if (oldCell != null && oldCell.Coord.Equal(item.OldCell.Coord))
            {
                DetectPickItem(item.OldCell.Coord);
            }
        }
    }

    /// <summary>
    /// 经过一个格子或者在某个格子停止，或者点击了玩家自己脚下的格子时调用(在TouchEvent里面)
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="isOnlyNotice"></param>
    public void DetectPickItem(CSMisc.Dot2 coord)
    {
        //Debug.LogFormat("======> CSDropSystem: DetectPickItem coord = ({0},{1})",coord.x,coord.y);

        if(!IsInPickItemTime())
        {
            return;
        }
        SetNextStandPickItemTime();
        CSCharacter mainPlayer =CSAvatarManager.MainPlayer;
        if (mainPlayer.IsMoving)
        {
            return;
        }
        if (mainPlayer.IsDead)
        {
            return;
        }
        CSItem item = CSDropManager.Instance.GetItem(coord); ;
        if (item == null || item.itemTbl == null)
        {
            return;
        }

        if((item.itemTbl.type  != BagItemType.Property) && CSBagInfo.Instance.IsBagFilled())
        {
            //背包已满，不可拾取!
            UtilityTips.ShowTips(967,1.5f,ColorType.White);
            return;
        }

        if (!IsCanPickUpItemBySet(item.itemTbl.pickUpType, item.itemTbl))
        {
            return;
        }

        //if (!IsSmallPlayerCanPick(item.itemTbl))
        //{
        //    //当前等级不足，不可拾取!
        //    UtilityTips.ShowTips(966);
        //    return;
        //}

        //if (IsInProtectTime(item))
        //{
        //    //改装备处于保护状态，不可拾取
        //    UtilityTips.ShowTips(968);
        //    return;
        //}
        bool isAffiliation = CheckItemAffiliation(item);
        if (isAffiliation)
        {
            Net.ReqPickupItemMessage(item.BaseInfo.itemId);
            SetNextPickItemTime();
        }
        else
        {
            //没有拾取的权限
            UtilityTips.ShowTips(969);
        }

    }
    public bool IsInPickItemTime()
    {
        bool ret = (CSAutoFightManager.Instance.IsAutoFight) ? 
            (Time.time >= NextAutoPickItemTime) : (Time.time >= NextPickItemTime);
        return ret;
    }

    /// <summary>
    /// 检查道具归属
    /// </summary>
    private bool CheckItemAffiliation(CSItem item)
    {
        if(item.BaseInfo.unionId == 0)
        {
            if (item.BaseInfo.owner != 0 && item.BaseInfo.owner != CSMainPlayerInfo.Instance.ID)
            {
                if (item.BaseInfo.ownerTeamId == 0 || 
                    (item.BaseInfo.ownerTeamId != 0 && item.BaseInfo.ownerTeamId != CSMainPlayerInfo.Instance.TeamId))
                {
                    return false;
                }
            }
        }
        else if(item.BaseInfo.unionId != CSMainPlayerInfo.Instance.GuildId)
        {
            return false;
        }
        return true;
    }

    private bool IsSmallPlayerCanPick(TABLE.ITEM tblItem)
    {
        //if (!IsLanuchMainPlayer) return false;
        //if (IsDaTaoSha) return true;
        if (tblItem == null)
        {
            return true;
        }
        int serverOpenday = 0;
        int reincarnation = 0;
        if (serverOpenday <= mPickItemLimit.serverOpenDay)
        {
            return true;
        }

        bool isLv = (CSMainPlayerInfo.Instance.Level <= mPickItemLimit.roleLevel)||
            ((reincarnation <= mPickItemLimit.roleReincarnation) && (mPickItemLimit.roleReincarnation != 0));

        if(isLv)
        {
            int tblReinLv = 0;
            bool isItemLv = (tblItem.level >= mPickItemLimit.itemLevel) || 
                ((tblReinLv >= mPickItemLimit.itemReincarnation) && (mPickItemLimit.itemReincarnation != 0));
            if(isItemLv)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsInProtectTime(CSItem item)
    {
        if(item != null && item.BaseInfo != null)
        {
            if (CSServerTime.Instance.TotalMillisecond < item.BaseInfo.protectTime)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCanPickUpItemBySet(int type, TABLE.ITEM tbl_item)
    {
        switch (type)
        {
            case PickUpType.None:
                {
                    return false;
                }
            case PickUpType.WoLongEquip:
                {
                    bool b = CSConfigInfo.Instance.GetBool(ConfigOption.AllEquipPickUp);
                    int lv = CSConfigInfo.Instance.GetInt(ConfigOption.WLEquipPickUpLv);
                    return (b && tbl_item.level >= lv);
                }
            case PickUpType.NormalEuip:
                {
                    bool b = CSConfigInfo.Instance.GetBool(ConfigOption.AllEquipPickUp);
                    int q = CSConfigInfo.Instance.GetBenYuanEquipPickUpQuality();
                    int lv = CSConfigInfo.Instance.GetInt(ConfigOption.BYEquipPickUpLv);
                    return (b && tbl_item.quality >= q && tbl_item.level >= lv);
                }
            default:
                {
                    bool b = CSConfigInfo.Instance.GetPropPickUpBool(type);
                    if(!b && type == PickUpType.Currency && tbl_item.id != 1)
                    {
                        b = true;
                    }
                    return b;
                }
        }
    }

    public CSItem GetPriorityItemByType()
    {
        CSItem item = null;//优先武器
        item = GetClosestItem(PickUpType.WoLongEquip);
        if (item == null) item = GetClosestItem(PickUpType.NormalEuip);
        if (item == null) item = GetClosestItem(PickUpType.Currency, true);
        if (item == null) item = GetClosestItem(PickUpType.Currency);
        if (item == null) item = GetClosestItem(PickUpType.Materials);
        //if (item == null) item = GetClosestItem(PickUpType.Mount);
        if (item == null) item = GetClosestItem(PickUpType.RecoverDrug);
        if (item == null) item = GetClosestItem(PickUpType.Other);
        return item;
    }

    public CSItem GetPriorityItem()
    {
        CSItem item = null;
        bool isCanCrossScene = CSScene.IsCanCrossScene;
        float distance = float.MaxValue;
        int quality = 0;
        int priority = 0;
        int tempPriority = 0;

        ILBetterList<CSItem> itemList = CSDropManager.Instance.itemList;

        bool isBagFull = CSBagInfo.Instance.IsBagFilled();

        for(int i = 0; i < itemList.Count; ++i)
        {
            CSItem tempItem = itemList[i];
            if (tempItem == null || tempItem.itemTbl == null) continue;
            if (isBagFull && (tempItem.itemTbl.pickUpType != (int)PickUpType.Currency)) continue;

            TABLE.ITEM tblItem;
            if (!ItemTableManager.Instance.TryGetValue(tempItem.BaseInfo.itemConfigId, out tblItem))
            {
                continue;
            }
            if(tblItem.quality < quality)
            {
                continue;
            }
            if (tblItem.pickUpType == 0)
            {
                continue;
            }
            int pickUpType  = tblItem.pickUpType;
            if (tblItem.quality == quality)
            {
                if(PickUpTypeProrityDic.TryGetValue(pickUpType, out tempPriority))
                {
                    if(tempPriority < priority)
                    {
                        continue;
                    }
                }
            }
            CSCell cell = tempItem.OldCell;
            if (cell == null) continue;
            if (cell != null && !cell.node.isCanCrossNpc) continue;
            if (cell != null && !isCanCrossScene && !cell.node.isProtect && cell.node.avatarNum > 0) continue;
            if (cell != null && !UtilityMain.IsInViewRange(cell.Coord)) continue;

            if (CheckItemAffiliation(tempItem))
            {
                if (CSServerTime.Instance.TotalMillisecond >= tempItem.BaseInfo.endTime /*&& !IsDaTaoSha*/) continue;
                if (!IsCanPickUpItemBySet(pickUpType, tblItem)) continue;

                if (!IsSmallPlayerCanPick(tblItem)) continue;
                float x = Mathf.Abs(CSAvatarManager.MainPlayer.OldCell.Coord.x - tempItem.OldCell.Coord.x);
                float y = Mathf.Abs(CSAvatarManager.MainPlayer.OldCell.Coord.y - tempItem.OldCell.Coord.y);
                float dist = x * x + y * y;
                if((tblItem.quality > quality) || (tblItem.quality == quality && dist < distance))
                {
                    item = tempItem;
                    item.pickType = tblItem.pickUpType;
                    quality = tblItem.quality;
                    PickUpTypeProrityDic.TryGetValue(pickUpType, out priority);
                    distance = dist;
                }
            }
        }
        return item;
    }

    private CSItem GetClosestItem(int type, bool gold = false)
    {
        bool isCanCrossScene = CSScene.IsCanCrossScene;
        CSItem item = null;
        float distance = float.MaxValue;

        bool isBagFull = CSBagInfo.Instance.IsBagFilled();
        if (isBagFull && type != PickUpType.Currency)
        {
            return item;
        }
        ILBetterList<CSItem> itemList = CSDropManager.Instance.itemList;

        for (int i = 0; i < itemList.Count; ++i)
        {
            CSItem tempItem = itemList[i];
            if (tempItem == null || tempItem.itemTbl ==null) continue;
            if(isBagFull && (tempItem.itemTbl.pickUpType != (int)PickUpType.Currency)) continue;

            CSCell cell = tempItem.OldCell;
            if (cell == null) continue;

            if (cell != null && !cell.node.isCanCrossNpc) continue;
            if (cell != null && !isCanCrossScene && !cell.node.isProtect && cell.node.avatarNum > 0) continue;
            if (cell != null && !UtilityMain.IsInViewRange(cell.Coord)) continue;

            if (CheckItemAffiliation(tempItem))
            {
                if (CSServerTime.Instance.TotalMillisecond >= tempItem.BaseInfo.endTime /*&& !IsDaTaoSha*/) continue;

                TABLE.ITEM tbl;
                if (ItemTableManager.Instance.TryGetValue(tempItem.BaseInfo.itemConfigId, out tbl))
                {
                    if (!IsCanPickUpItemBySet(type, tbl)) continue;

                    if (!IsSmallPlayerCanPick(tbl)) continue;

                    if (gold && (tbl.id != 1 && tbl.id != 2)) continue;

                    if (tbl.pickUpType == type)
                    {
                        float x = Mathf.Abs(CSAvatarManager.MainPlayer.OldCell.Coord.x - tempItem.OldCell.Coord.x);
                        float y = Mathf.Abs(CSAvatarManager.MainPlayer.OldCell.Coord.y - tempItem.OldCell.Coord.y);
                        float dist = x * x + y * y;
                        if (dist < distance)
                        {
                            distance = dist;
                            item = tempItem;
                        }
                    }
                }
            }
        }

        return item;
    }

    private void SetNextPickItemTime()
    {
        NextPickItemTime = Time.time + mPickItemLimit.intervalTime;
        SetNextStandPickItemTime();
        if (CSAutoFightManager.Instance.IsAutoFight)
        {
            NextAutoPickItemTime = Time.time + mPickItemLimit.autoIntervalTime;
        }
    }

    private void SetNextStandPickItemTime()
    {
        NextStandPickItemTime = Time.time + mPickItemLimit.standDetectIntervalTime;
    }

    public void OnMainPlayerCellChangeTrigger(uint evtId, object obj)
    {
        CSCell cell = obj as CSCell;
        if(cell != null)
        {
            DetectPickItem(cell.Coord);
        }
    }

    public void OnMainPlayerCellStopTrigger(uint evtId, object obj)
    {
        CSCell cell = obj as CSCell;
        if (cell != null)
        {
            DetectPickItem(cell.Coord);
        }
    }

    public void OnPickupItemMessage(uint evtId, object obj)
    {
        bag.PickupMsg info = obj as bag.PickupMsg;
        if (info == null)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed info is Null");
            return;
        }

        mClientEvent.SendEvent(CEvent.OnPickupItemPlayEffect,info);
        CSDropManager.Instance.RemoveItem(info.id);
    }

    #endregion

    public override void Dispose()
    {
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_CellChangeTrigger,OnMainPlayerCellChangeTrigger);
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, OnMainPlayerCellStopTrigger);
    }
}
