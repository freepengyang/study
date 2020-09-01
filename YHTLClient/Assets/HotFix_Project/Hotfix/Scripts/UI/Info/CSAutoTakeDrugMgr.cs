using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAutoTakeDrugMgr : CSInfo<CSAutoTakeDrugMgr>
{
    Schedule autoTakeDrugSch;
    int takeHpTimer = 0;

    Schedule autoTakeMpSch;
    int takeMpTimer = 0;

    /// <summary> 自动按顺序吃红药id </summary>
    ILBetterList<TABLE.ITEM> autoDrugHpByOrderIds = new ILBetterList<TABLE.ITEM>(8);
    /// <summary> 自动按顺序吃蓝药id </summary>
    ILBetterList<TABLE.ITEM> autoDrugMpByOrderIds = new ILBetterList<TABLE.ITEM>(8);


    public override void Dispose()
    {
        CancelTimers();
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


        mClientEvent.AddEvent(CEvent.Setting_AutoTakeDrugSwitchChange, HpDrugEvent);
        mClientEvent.AddEvent(CEvent.Setting_AutoTakeDrugHpChange, HpDrugEvent);
        mClientEvent.AddEvent(CEvent.Setting_AutoTakeDrugChange, HpDrugEvent);
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, HpDrugEvent);
        mClientEvent.AddEvent(CEvent.LeaveInstance, HpDrugEvent);

        mClientEvent.AddEvent(CEvent.Setting_AutoTakeMpDrugSwitchChange, MpDrugEvent);
        mClientEvent.AddEvent(CEvent.Setting_AutoTakeDrugMpChange, MpDrugEvent);
        mClientEvent.AddEvent(CEvent.Setting_AutoTakeMpDrugChange, MpDrugEvent);
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, MpDrugEvent);
        mClientEvent.AddEvent(CEvent.LeaveInstance, MpDrugEvent);

        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.HP_Change, HpDrugEvent);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MP_Change, MpDrugEvent);

        HpDrugEvent(0, null);
        MpDrugEvent(0, null);
    }


    #region AutoTakeDrugs
    void AutoTakeDrugTimer(Schedule sch)
    {
        TryToTakeDrug();
    }

    void TryToTakeDrug()
    {
        if (!IsMapCanUseDrug())
        {
            Timer.Instance.CancelInvoke(autoTakeDrugSch);
            return;
        }
        if (!GetBool(ConfigOption.AutoTakeDrugSwitch))
        {
            Timer.Instance.CancelInvoke(autoTakeDrugSch);
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
        float cfgCd = BufferTableManager.Instance.GetBufferDispelParam(bufferId) / 1000f;//毫秒

        
        if (takeHpTimer >= cfgCd) takeHpTimer = 0;

        if (takeHpTimer == 0)
        {
            bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
            CSBagInfo.Instance.UseItem(drug);
        }       

        takeHpTimer += 1;
    }



    void AutoTakeMpTimer(Schedule sch)
    {
        TryToTakeMp();
    }


    void TryToTakeMp()
    {
        if (!IsMapCanUseDrug())
        {
            Timer.Instance.CancelInvoke(autoTakeMpSch);
            return;
        }
        if (!GetBool(ConfigOption.AutoTakeMpDrugSwitch))
        {
            Timer.Instance.CancelInvoke(autoTakeMpSch);
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
        float cfgCd = BufferTableManager.Instance.GetBufferDispelParam(bufferId) / 1000f;//毫秒


        
        if (takeMpTimer >= cfgCd) takeMpTimer = 0;

        if (takeMpTimer == 0)
        {
            bag.BagItemInfo drug = CSBagInfo.Instance.GetItemInfoByCfgId(drugId);
            CSBagInfo.Instance.UseItem(drug);
        }

        takeMpTimer += 1;
    }


    void CancelTimers()
    {
        Timer.Instance.CancelInvoke(autoTakeDrugSch);
        Timer.Instance.CancelInvoke(autoTakeMpSch);
    }

    #endregion


    bool GetBool(ConfigOption configOption)
    {
        return CSConfigInfo.Instance.GetBool(configOption);
    }


    int GetInt(ConfigOption configOption)
    {
        return CSConfigInfo.Instance.GetInt(configOption);
    }


    bool IsMapCanUseDrug()
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
        if (!Timer.Instance.IsInvoking(autoTakeDrugSch))
        {
            takeHpTimer = 0;
            autoTakeDrugSch = Timer.Instance.InvokeRepeating(0, 1, AutoTakeDrugTimer);
        }
    }


    void MpDrugEvent(uint id, object data)
    {
        if (!Timer.Instance.IsInvoking(autoTakeMpSch))
        {
            takeMpTimer = 0;
            autoTakeMpSch = Timer.Instance.InvokeRepeating(0, 1, AutoTakeMpTimer);
        }
    }
}
