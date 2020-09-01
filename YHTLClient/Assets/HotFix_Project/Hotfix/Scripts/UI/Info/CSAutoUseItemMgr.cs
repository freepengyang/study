using FlyBirds.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAutoUseItemMgr : CSInfo<CSAutoUseItemMgr>
{
    //Schedule autoTakeDrugSch;
    TimerEventHandle autoHpSch;
    TimerEventHandle hpBufferSch;
    int takeHpTimer = 0;

    //Schedule autoTakeMpSch;
    TimerEventHandle autoMpSch;
    TimerEventHandle mpBufferSch;
    int takeMpTimer = 0;

    TimerEventHandle autoInstantSch;
    int takeInstantTimer = 0;

    TimerEventHandle returnOrTransferSch;
    TimerEventHandle returnCDSch;
    int returnCDTimer = 0;
    TimerEventHandle transferCDSch;
    int transferCDTimer = 0;

    /// <summary> 自动按顺序吃红药id </summary>
    ILBetterList<TABLE.ITEM> autoDrugHpByOrderIds = new ILBetterList<TABLE.ITEM>(8);
    /// <summary> 自动按顺序吃蓝药id </summary>
    ILBetterList<TABLE.ITEM> autoDrugMpByOrderIds = new ILBetterList<TABLE.ITEM>(8);
    /// <summary> 自动按顺序吃瞬回药id </summary>
    ILBetterList<TABLE.ITEM> autoInstantDrugByOrderIds = new ILBetterList<TABLE.ITEM>(8);


    List<int> banReturnIds = new List<int>();

    TABLE.ITEM returnItem;
    TABLE.ITEM transferItem;


    public override void Dispose()
    {
        CancelTimers();
        autoDrugHpByOrderIds?.Clear();
        autoDrugHpByOrderIds = null;
        autoDrugMpByOrderIds?.Clear();
        autoDrugMpByOrderIds = null;
        autoInstantDrugByOrderIds?.Clear();
        autoInstantDrugByOrderIds = null;

        returnItem = null;
        transferItem = null;
    }


    public void Init()
    {
        string sharpStr = SundryTableManager.Instance.GetSundryEffect(1032);
        if (!string.IsNullOrEmpty(sharpStr))
        {
            autoDrugHpByOrderIds.Clear();
            var arr = sharpStr.Split('#');
            for (int i = 0; i < arr.Length; i++)
            {
                int id = 0;
                TABLE.ITEM item;
                if (int.TryParse(arr[i], out id) && ItemTableManager.Instance.TryGetValue(id, out item))
                {
                    autoDrugHpByOrderIds.Add(item);
                }
            }
        }

        sharpStr = SundryTableManager.Instance.GetSundryEffect(1033);
        if (!string.IsNullOrEmpty(sharpStr))
        {
            autoDrugMpByOrderIds.Clear();
            var arr = sharpStr.Split('#');
            for (int i = 0; i < arr.Length; i++)
            {
                int id = 0;
                TABLE.ITEM item;
                if (int.TryParse(arr[i], out id) && ItemTableManager.Instance.TryGetValue(id, out item))
                {
                    autoDrugMpByOrderIds.Add(item);
                }
            }
        }

        sharpStr = SundryTableManager.Instance.GetSundryEffect(1145);
        if (!string.IsNullOrEmpty(sharpStr))
        {
            autoInstantDrugByOrderIds.Clear();
            var arr = sharpStr.Split('#');
            for (int i = 0; i < arr.Length; i++)
            {
                int id = 0;
                TABLE.ITEM item;
                if (int.TryParse(arr[i], out id) && ItemTableManager.Instance.TryGetValue(id, out item))
                {
                    autoInstantDrugByOrderIds.Add(item);
                }
            }
        }

        string idstr = SundryTableManager.Instance.GetSundryEffect(1144);
        int cfgId = 0;
        if (int.TryParse(idstr, out cfgId))
        {
            returnItem = ItemTableManager.Instance.GetItemCfg(cfgId);
        }

        idstr = SundryTableManager.Instance.GetSundryEffect(1143);
        if (int.TryParse(idstr, out cfgId))
        {
            transferItem = ItemTableManager.Instance.GetItemCfg(cfgId);
        }

        banReturnIds = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(1150));

        HpDrugEvent(0, null);
        MpDrugEvent(0, null);
        InstantDrugEvent(0, null);
        UseReturnOrTransferEvent(0, null);
    }


    #region AutoTakeDrugs

    void TryToTakeDrug()
    {
        //Debug.LogError("吃药检测定时器：" + takeHpTimer);
        if (takeHpTimer > 0) return;

        if (!IsMapCanRecover())
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        if (!GetBool(ConfigOption.AutoTakeDrugSwitch))
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        //Debug.LogError("@@@@@吃药");
        int curHp = CSMainPlayerInfo.Instance.HP;
        int maxHp = CSMainPlayerInfo.Instance.MaxHP;
        var rate = ((float)curHp / maxHp) * 100;
        if (rate >= GetInt(ConfigOption.AutoTakeDrugHp))
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }

        int configid = GetInt(ConfigOption.AutoTakeDrug);

        int drugId = 0;

        if (configid > 0)//正常的药品id
        {
            if (configid.GetItemCount() <= 0) return;
            drugId = configid;
        }
        else if (configid == -1)//按从大到小顺序吃药
        {
            if (autoDrugHpByOrderIds == null || autoDrugHpByOrderIds.Count < 1)
            {
                //Timer.Instance.CancelInvoke(autoTakeDrugSch);
                return;
            }

            for (int i = 0; i < autoDrugHpByOrderIds.Count; i++)
            {
                var item = autoDrugHpByOrderIds[i];
                if (CSMainPlayerInfo.Instance.Level >= item.level && item.id.GetItemCount() > 0)
                {
                    drugId = item.id;
                    break;
                }
            }
        }

        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(drugId);
        if (cfg == null) return;
        if (ItemCDManager.Instance.InGroupCD(cfg.group)) return;

        var buffer = cfg.bufferParam;
        int bufferId = 0;
        if (!int.TryParse(buffer, out bufferId)) return;
        float bufferCd = BufferTableManager.Instance.GetBufferDispelParam(bufferId) / 1000f;//毫秒

        bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
        CSBagInfo.Instance.UseItem(drug);

        //CancelHpBufferTimer();
        takeHpTimer = (int)bufferCd;
        if (hpBufferSch == null)
            hpBufferSch = CSTimer.Instance.InvokeRepeating(0, 1f, HpBufferTimer);
        //if (takeHpTimer >= bufferCd) takeHpTimer = 0;

        //if (takeHpTimer == 0)
        //{
        //    bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
        //    CSBagInfo.Instance.UseItem(drug);
        //}       

        //takeHpTimer += 1;
    }

    void HpBufferTimer()
    {
        if (takeHpTimer > 0)
            takeHpTimer -= 1;
        //Debug.LogError("HpBufferTimer：" + takeHpTimer);
        //if (takeHpTimer <= 0)
        //{
        //    CancelHpBufferTimer();
        //}
    }



    void TryToTakeMp()
    {
        //Debug.LogError("吃蓝定时器:" + takeMpTimer);
        if (takeMpTimer > 0) return;

        if (!IsMapCanRecover())
        {
            //Timer.Instance.CancelInvoke(autoTakeMpSch);
            return;
        }
        if (!GetBool(ConfigOption.AutoTakeMpDrugSwitch))
        {
            //Timer.Instance.CancelInvoke(autoTakeMpSch);
            return;
        }

        int curMp = CSMainPlayerInfo.Instance.MP;
        int maxMp = CSMainPlayerInfo.Instance.MaxMP;
        var rate = ((float)curMp / maxMp) * 100;
        if (rate >= GetInt(ConfigOption.AutoTakeDrugMp))
        {
            //Timer.Instance.CancelInvoke(autoTakeMpSch);
            return;
        }

        int configid = GetInt(ConfigOption.AutoTakeMpDrug);

        int drugId = 0;
        if (configid > 0)//正常的药品id
        {
            //FNDebug.LogErrorFormat("药品id:{0}， 数量：{1}", configid, configid.GetItemCount());
            if (configid.GetItemCount() <= 0) return;
            drugId = configid;
        }
        else if (configid == -1)//按从大到小顺序吃药
        {
            if (autoDrugMpByOrderIds == null || autoDrugMpByOrderIds.Count < 1)
            {
                //Timer.Instance.CancelInvoke(autoTakeMpSch);
                return;
            }

            for (int i = 0; i < autoDrugMpByOrderIds.Count; i++)
            {
                var item = autoDrugMpByOrderIds[i];
                if (CSMainPlayerInfo.Instance.Level >= item.level && item.id.GetItemCount() > 0)
                {
                    drugId = item.id;
                    break;
                }
            }
        }

        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(drugId);
        if (cfg == null) return;
        if (ItemCDManager.Instance.InGroupCD(cfg.group)) return;

        var buffer = cfg.bufferParam;
        int bufferId = 0;
        if (!int.TryParse(buffer, out bufferId)) return;
        float bufferCd = BufferTableManager.Instance.GetBufferDispelParam(bufferId) / 1000f;//毫秒

        bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
        CSBagInfo.Instance.UseItem(drug);

        //CancelMpBufferTimer();
        takeMpTimer = (int)bufferCd;
        if (mpBufferSch == null)
            mpBufferSch = CSTimer.Instance.InvokeRepeating(0, 1f, MpBufferTimer);

        //if (takeMpTimer >= bufferCd) takeMpTimer = 0;

        //if (takeMpTimer == 0)
        //{
        //    bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
        //    CSBagInfo.Instance.UseItem(drug);
        //}

        //takeMpTimer += 1;
    }

    void MpBufferTimer()
    {
        if (takeMpTimer > 0)
            takeMpTimer -= 1;
        //if (takeMpTimer <= 0)
        //{
        //    CancelMpBufferTimer();
        //}
    }


    void TryToTakeInstantDrug()
    {
        if (!IsMapCanRecover())
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        if (!GetBool(ConfigOption.AutoInstantDrugSwitch))
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        //Debug.LogError("@@@@@吃药");
        int curHp = CSMainPlayerInfo.Instance.HP;
        int maxHp = CSMainPlayerInfo.Instance.MaxHP;
        var rate = ((float)curHp / maxHp) * 100;
        if (rate >= GetInt(ConfigOption.AutoInstantDrugHp))
        {
            //Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        int configid = GetInt(ConfigOption.AutoInstantDrug);

        int drugId = 0;

        if (configid > 0)//正常的药品id
        {
            if (configid.GetItemCount() <= 0) return;
            drugId = configid;
        }
        else if (configid == -1)//按从大到小顺序吃药
        {
            if (autoInstantDrugByOrderIds == null || autoInstantDrugByOrderIds.Count < 1)
            {
                //Timer.Instance.CancelInvoke(autoTakeDrugSch);
                return;
            }

            for (int i = 0; i < autoInstantDrugByOrderIds.Count; i++)
            {
                var item = autoInstantDrugByOrderIds[i];
                if (CSMainPlayerInfo.Instance.Level >= item.level && item.id.GetItemCount() > 0)
                {
                    drugId = item.id;
                    break;
                }
            }
        }

        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(drugId);
        if (cfg == null) return;
        if (ItemCDManager.Instance.InGroupCD(cfg.group)) return;

        bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
        CSBagInfo.Instance.UseItem(drug);
    }

    #endregion

    #region AutoReturnOrTransfer


    bool CanUseReturn()
    {
        if (returnCDTimer > 0)
            return false;

        if (!GetBool(ConfigOption.AutoGoBackSwitch))
            return false;

        int mapId = CSMainPlayerInfo.Instance.MapID;
        if (banReturnIds != null && banReturnIds.Contains(mapId))
            return false;

        if (IsSafeArea())
            return false;

        if (returnItem == null)
            return false;

        if (returnItem.id.GetItemCount() < 1)
            return false;

        if (ItemCDManager.Instance.InGroupCD(returnItem.group))
            return false;

        int curHp = CSMainPlayerInfo.Instance.HP;
        int maxHp = CSMainPlayerInfo.Instance.MaxHP;
        var rate = ((float)curHp / maxHp) * 100;
        if (rate >= GetInt(ConfigOption.AutoGoBackHp))
            return false;

        return true;
    }


    bool CanUseTransfer()
    {
        if (transferCDTimer > 0)
            return false;

        if (!GetBool(ConfigOption.AutoRandomDeliverySwitch))
            return false;

        int mapId = CSMainPlayerInfo.Instance.MapID;
        int ban = MapInfoTableManager.Instance.GetMapInfoBanTransfer(mapId);
        if (ban == 1 || ban == 3)
            return false;

        if (IsSafeArea())
            return false;

        if (transferItem == null)
            return false;

        if (transferItem.id.GetItemCount() < 1)
            return false;

        if (ItemCDManager.Instance.InGroupCD(transferItem.group))
            return false;

        int curHp = CSMainPlayerInfo.Instance.HP;
        int maxHp = CSMainPlayerInfo.Instance.MaxHP;
        var rate = ((float)curHp / maxHp) * 100;
        if (rate >= GetInt(ConfigOption.AutoRandomDeliveryHp))
            return false;


        return true;
    }
    
    /// <summary>
    /// 是否在安全区
    /// </summary>
    /// <returns></returns>
    bool IsSafeArea()
    {
        var avatar = CSAvatarManager.MainPlayer;
        if (avatar == null)
            return false;

        var cell = avatar.OldCell;
        if (cell == null)
            return false;

        //var node = cell.node;
        //if (node == null)
        //    return false;

        return cell.isAttributes(MapEditor.CellType.Protect);
    }


    //传送和回城两个条件都满足时，优先回城
    void TryToReturnOrTransfer()
    {
        if (CanUseReturn())
        {
            if (!DoUseReturn())
            {
                DoUseTransfer();
            }
            return;
        }

        if(CanUseTransfer()) DoUseTransfer();
    }


    bool DoUseReturn()
    {
        if (returnItem != null)
        {
            //Debug.LogError("使用回城");
            //bag.BagItemInfo _info = CSBagInfo.Instance.GetItemInfoByCfgId(returnItem.id);
            //if (_info == null) return false;
            Net.CSBackCityMessage();
            //CancelReturnCDTimer();
            if (returnCDSch == null)
                returnCDSch = CSTimer.Instance.InvokeRepeating(0, 1, ReturnCDTimer);
            return true;
        }

        return false;
    }

    bool DoUseTransfer()
    {
        if (transferItem != null)
        {
            //Debug.LogError("使用随机");
            bag.BagItemInfo _info = CSBagInfo.Instance.GetItemInfoByCfgId(transferItem.id);
            if (_info == null) return false;
            Net.ReqUseItemMessage(_info.bagIndex, 1, false, 0, _info.id);
            //CancelTransferCDTimer();
            if(transferCDSch == null)
                transferCDSch = CSTimer.Instance.InvokeRepeating(0, 1, TransferCDTimer);
            return true;
        }

        return false;
    }



    void ReturnCDTimer()
    {
        returnCDTimer += 1;
        if (returnCDTimer >= CSConfigInfo.Instance.GetAutoReturnTime())
        {
            returnCDTimer = 0;
            //CancelReturnCDTimer();
        }
    }

    void TransferCDTimer()
    {
        transferCDTimer += 1;
        if (transferCDTimer >= CSConfigInfo.Instance.GetAutoTransferTime())
        {
            transferCDTimer = 0;
            //CancelTransferCDTimer();
        }
    }

    #endregion

    #region OtherFunc
    bool GetBool(ConfigOption configOption)
    {
        return CSConfigInfo.Instance.GetInt(configOption) == 1;
    }


    int GetInt(ConfigOption configOption)
    {
        return CSConfigInfo.Instance.GetInt(configOption);
    }


    bool IsMapCanRecover()
    {
        var info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info != null)
        {
            var id = info.instanceId;
            int ban = MapBanTableManager.Instance.GetMapBanRecover(id);
            return ban == 0;
        }

        return true;
    }


    void HpDrugEvent(uint id, object data)
    {
        //if (!Timer.Instance.IsInvoking(autoTakeDrugSch))
        //{
        //    takeHpTimer = 0;
        //    autoTakeDrugSch = Timer.Instance.InvokeRepeating(0, 1, AutoTakeDrugTimer);
        //}
        if (autoHpSch == null)
        {
            autoHpSch = CSTimer.Instance.InvokeRepeating(0, 1f, TryToTakeDrug);
        }
    }


    void MpDrugEvent(uint id, object data)
    {
        //if (!Timer.Instance.IsInvoking(autoTakeMpSch))
        //{
        //    takeMpTimer = 0;
        //    autoTakeMpSch = Timer.Instance.InvokeRepeating(0, 1, AutoTakeMpTimer);
        //}
        if (autoMpSch == null)
        {
            autoMpSch = CSTimer.Instance.InvokeRepeating(0, 1f, TryToTakeMp);
        }
    }


    void InstantDrugEvent(uint id, object data)
    {
        if (autoInstantSch == null)
        {
            autoInstantSch = CSTimer.Instance.InvokeRepeating(0, 1f, TryToTakeInstantDrug);
        }
    }


    void UseReturnOrTransferEvent(uint id, object data)
    {
        if (returnOrTransferSch == null)
        {
            returnOrTransferSch = CSTimer.Instance.InvokeRepeating(0, 1f, TryToReturnOrTransfer);
        }
    }
    #endregion

    #region CancelTimers
    void CancelTimers()
    {
        //Timer.Instance.CancelInvoke(autoTakeDrugSch);
        //Timer.Instance.CancelInvoke(autoTakeMpSch);
        if (autoHpSch != null)
        {
            CSTimer.Instance.remove_timer(autoHpSch);
            autoHpSch = null;
        }


        if (autoMpSch != null)
        {
            CSTimer.Instance.remove_timer(autoMpSch);
            autoMpSch = null;
        }


        if (autoInstantSch != null)
        {
            CSTimer.Instance.remove_timer(autoInstantSch);
            autoInstantSch = null;
        }

        CancelHpBufferTimer();
        CancelMpBufferTimer();
        CancelReturnCDTimer();
        CancelTransferCDTimer();
        if (returnOrTransferSch != null)
        {
            CSTimer.Instance.remove_timer(returnOrTransferSch);
            returnOrTransferSch = null;
        }
    }

    void CancelHpBufferTimer()
    {
        if (hpBufferSch != null)
        {
            CSTimer.Instance.remove_timer(hpBufferSch);
            hpBufferSch = null;
        }
    }

    void CancelMpBufferTimer()
    {
        if (mpBufferSch != null)
        {
            CSTimer.Instance.remove_timer(mpBufferSch);
            mpBufferSch = null;
        }
    }


    void CancelReturnCDTimer()
    {
        if (returnCDSch != null)
        {
            CSTimer.Instance.remove_timer(returnCDSch);
            returnCDSch = null;
        }
    }

    void CancelTransferCDTimer()
    {
        if (transferCDSch != null)
        {
            CSTimer.Instance.remove_timer(transferCDSch);
            transferCDSch = null;
        }
    }

    #endregion
}
