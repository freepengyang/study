using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetBag : CSNetBase
{
    void ECM_ResGetBagInfoMessage(NetInfo info)
    {
        bag.BagInfo msg = Network.Deserialize<bag.BagInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagInfo");
            return;
        }
        if (msg.maxCount > CSBagInfo.Instance.GetMaxCount())
        {
            CSBagInfo.Instance.GetBagMaxCountChange(msg.maxCount);
            HotManager.Instance.EventHandler.SendEvent(CEvent.BagMaxCountChange, msg);
        }
    }
    void ECM_ResBagItemChangedMessage(NetInfo info)
    {
        bag.BagItemChangeList msg = Network.Deserialize<bag.BagItemChangeList>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagItemChangeList");
            return;
        }
        CSBagInfo.Instance.ItemsChangeList(msg);
    }
    void ECM_ResWealthAmountChangeMessage(NetInfo info)
    {
        bag.WealthAmountChangeResponse msg = Network.Deserialize<bag.WealthAmountChangeResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.WealthAmountChangeResponse");
            return;
        }
        CSBagInfo.Instance.MoneyChange(msg);
    }
    void ECM_ResEquipItemMessage(NetInfo info)
    {
        bag.EquipItemMsg msg = Network.Deserialize<bag.EquipItemMsg>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.EquipItemMsg");
            return;
        }
        CSBagInfo.Instance.ResEquipWear(msg);
    }
    void ECM_ResSortItemsMessage(NetInfo info)
    {
        bag.BagItemChangeList msg = Network.Deserialize<bag.BagItemChangeList>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagItemChangeList");
            return;
        }
        CSBagInfo.Instance.BagSrot(msg.changeList);
    }
    void ECM_ResSwapItemMessage(NetInfo info)
    {
        bag.SwapItemMsg msg = Network.Deserialize<bag.SwapItemMsg>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.SwapItemMsg");
            return;
        }
    }
    void ECM_ResUnEquipItemMessage(NetInfo info)
    {
        bag.UnEquipItemResponse msg = Network.Deserialize<bag.UnEquipItemResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.UnEquipItemResponse");
            return;
        }
        CSBagInfo.Instance.ResEquipUnWear(msg);
    }
    void ECM_ResCallBackItemMessage(NetInfo info)
    {
        bag.CallbackItemResponse msg = Network.Deserialize<bag.CallbackItemResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.CallbackItemResponse");
            return;
        }
        // if (null != msg)
        // {
        //     UIManager.Instance.CreatePanel<UIRecycleGetPanel>(f =>
        //     {
        //         (f as UIRecycleGetPanel).Show(msg);
        //     });
        // }
    }
    void ECM_ResPickupItemMessage(NetInfo info)
    {
        bag.PickupMsg msg = Network.Deserialize<bag.PickupMsg>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.PickupMsg");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnPickupItemMessage, msg);
    }
    void ECM_ResBagIsFullMessage(NetInfo info)
    {
        bag.BagIsFull msg = Network.Deserialize<bag.BagIsFull>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagIsFull");
            return;
        }
    }
    void ECM_ResBagToStorehouseMessage(NetInfo info)
    {
        bag.BagToStorehouseResponse msg = Network.Deserialize<bag.BagToStorehouseResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagToStorehouseResponse");
            return;
        }
        CSStorehouseInfo.Instance.GetBagToWarehouse(msg.addItem);
    }
    void ECM_ResChangeBagCountMessage(NetInfo info)
    {
        bag.ChangeBagCount msg = Network.Deserialize<bag.ChangeBagCount>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.ChangeBagCount");
            return;
        }

    }
    void ECM_ItemUseLimitNtfMessage(NetInfo info)
    {
        bag.ItemUseLimitNtf msg = Network.Deserialize<bag.ItemUseLimitNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.ItemUseLimitNtf");
            return;
        }
    }
    void ECM_LimitItemInfoNtfMessage(NetInfo info)
    {
        baglimit.LimitItemInfoNtf msg = Network.Deserialize<baglimit.LimitItemInfoNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.LimitItemInfoNtf");
            return;
        }
    }
    void ECM_EquipItemModifyNBValueNtfMessage(NetInfo info)
    {
        baglimit.EquipItemModifyNBValueNtf msg = Network.Deserialize<baglimit.EquipItemModifyNBValueNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.EquipItemModifyNBValueNtf");
            return;
        }
    }
    void ECM_EquipRebuildNtfMessage(NetInfo info)
    {
        bag.EquipRebuildNtf msg = Network.Deserialize<bag.EquipRebuildNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.EquipRebuildNtf");
            return;
        }
        CSBagInfo.Instance.GetEuqipRecastRes(msg.equip);
        HotManager.Instance.EventHandler.SendEvent(CEvent.EquipRebuildNtfMessage, msg);
    }
    void ECM_EquipXiLianNtfMessage(NetInfo info)
    {
        bag.SCEquipRandomsNtf msg = Network.Deserialize<bag.SCEquipRandomsNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.SCEquipRandomsNtf");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.EquipXiLianNtfMessage, msg);
    }
    void ECM_SCChooseXiLianResultNtfMessage(NetInfo info)
    {
        bag.SCChooseXiLianResultNtf msg = Network.Deserialize<bag.SCChooseXiLianResultNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.SCChooseXiLianResultNtf");
            return;
        }
        CSBagInfo.Instance.GetEquipRefineRes(msg.result);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCChooseXiLianResultNtf, msg.result);
    }
    void ECM_ResOpenDebugMsgMessage(NetInfo info)
    {

    }
	void ECM_SCNotifyBagItemCdInfoMessage(NetInfo info)
	{
		bag.BagItemCdInfo msg = Network.Deserialize<bag.BagItemCdInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbag.BagItemCdInfo");
			return;
		}

        if (CSScene.IsLanuchMainPlayer)
            ItemCDManager.Instance.ResetItemCD(msg.cds);
        else
        {
            OnceEventTrigger.Instance.Register(OnceEvent.OnLogginTrigger, () =>
            {
                ItemCDManager.Instance.ResetItemCD(msg.cds);
            });
        }
    }
	void ECM_SCItemUsedDailyMessage(NetInfo info)
	{
		bag.ResItemUsedDaily msg = Network.Deserialize<bag.ResItemUsedDaily>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forbag.ResItemUsedDaily");
			return;
		}
        CSBagInfo.Instance.GetItemUseCountChangeMes(msg);
    }
	void ECM_SCItemUsedDailyTotalMessage(NetInfo info)
	{
		bag.ResItemUsedDailyTotal msg = Network.Deserialize<bag.ResItemUsedDailyTotal>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forbag.ResItemUsedDailyTotal");
			return;
		}
        CSBagInfo.Instance.GetAllItemUseCountMes(msg);

    }
}
