using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bag;
public class GemRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemChange,OnCheckRedPointByItem);
        mClientEvent.AddEvent(CEvent.CSUnlockGemPositionMessage,OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.CSGemRefresh,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }


    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Gem, CSGemInfo.Instance.GemRedCheck());
    }

    protected void OnCheckRedPointByItem(uint id, object argv)
    {
        if (argv is EventData data)
        {
            if (data.arg1 is BagItemInfo bagitemInfo &&data.arg2 is ItemChangeType itemChangeType)
            {
                TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(bagitemInfo.configId);
                //判断是否是宝石,或者是否是溶剂
                if (cfg.type == 9||(cfg.subType == 4&&cfg.type == 7))
                {
                    RefreshRed(RedPointType.Gem, CSGemInfo.Instance.GemRedCheck(data));
                }
            }
        }
    }

    
    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemChange,OnCheckRedPointByItem);
        mClientEvent.RemoveEvent(CEvent.CSUnlockGemPositionMessage,OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.CSGemRefresh,OnCheckRedPoint);
    }
    
    
}
