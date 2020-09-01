using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class SpecialActiveRewardTableManager : TableManager<TABLE.SPECIALACTIVEREWARDARRAY, TABLE.SPECIALACTIVEREWARD, int, SpecialActiveRewardTableManager>
{
    
    public int GetBossBoxId(int _id,int _rewardType)
    {
        int goalId = GetSpecialActiveRewardGoalId(_id);
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            if (item.goalId == goalId && item.rewardType == _rewardType)
            {
                return item.reward;
            }
        }
        return 0;
    }
    public int GetBoxId(int _acId, int _rewardType)
    {
        //Debug.Log("GetBoxId" + _rewardType);
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            //Debug.Log("iter.Current.Value.rewardType" + iter.Current.Value.rewardType);
            if (item.activityId == _acId && item.rewardType == _rewardType)
            {
                return item.reward;
            }
        }
        return 0;
    }
    List<TABLE.SPECIALACTIVEREWARD> list = new List<TABLE.SPECIALACTIVEREWARD>();
    public List<TABLE.SPECIALACTIVEREWARD> GetDataByIdAndType(int _acId, int _rewardType) {
        list.Clear();
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            //Debug.Log("iter.Current.Value.rewardType" + iter.Current.Value.rewardType);
            if (item.activityId == _acId && item.rewardType == _rewardType)
            {
                list.Add(item);
                //return iter.Current.Value.reward;
            }
        }

        return list;
    }
    

    public TABLE.SPECIALACTIVEREWARD GetDataByacId(int _acId) {
        //RewardData
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            if (item.activityId == _acId)
            {
                return item;
            }
        }

        return null;
    }

    public int GetSealTypeStartId(int _type)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVEREWARD;
            if (item.activityId == 10103 && item.rewardType == _type)
            {
                return item.id;
            }
        }
        return 0;
    }
}
