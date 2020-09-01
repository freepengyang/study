//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_deliver.proto
using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class DeliverTableManager : TableManager<TABLE.DELIVERARRAY, TABLE.DELIVER, int, DeliverTableManager>
{
    public int GetDeliverIdByNpcId(int npcId)
    {
        var arr = array.gItem.handles;
        TABLE.DELIVER value;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            value = arr[i].Value as TABLE.DELIVER;
            if ((DeliverType)value.DeliverType == DeliverType.DT_NPC && value.deliverParameter == npcId)
            {
                return value.id;
            }
        }

        return 0;
    }

    ILBetterList<SuggestDeliverData> deliverList;
    public void InitLvDeliver()
    {
        if (deliverList == null)
        {
            deliverList = new ILBetterList<SuggestDeliverData>(15);
            var arr = array.gItem.handles;
            TABLE.DELIVER value;
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                value = arr[i].Value as TABLE.DELIVER;
                if (!string.IsNullOrEmpty(value.level3))
                {
                    SuggestDeliverData data = new SuggestDeliverData();
                    data.id = value.id;
                    data.lvs = UtilityMainMath.SplitStringToIntList(value.level3);
                    deliverList.Add(data);
                }
            }
        }
    }

    public int GetSuggestDeliverId(int _lv)
    {
        int deliverId = 0;
        InitLvDeliver();
        if (deliverList != null)
        {
            for (int i = 0; i < deliverList.Count; i++)
            {
                if (deliverList[i].EqualsLv(_lv))
                {
                    deliverId = deliverList[i].id;
                    continue;
                }
            }
        }
        return deliverId;
    }
}
class SuggestDeliverData
{
    public int id;
    public List<int> lvs;
    public bool EqualsLv(int _lv)
    {
        if (lvs[0] <= _lv && _lv < lvs[1])
        {
            return true;
        }
        return false;
    }
}