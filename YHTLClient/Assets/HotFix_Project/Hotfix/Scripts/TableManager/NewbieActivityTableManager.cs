using System;
using System.Collections.Generic;
using TABLE;

public partial class NewbieActivityTableManager
{
    Map<int,NEWBIEACTIVITY> mapDatas = new Map<int,NEWBIEACTIVITY>();
    public Map<int, NEWBIEACTIVITY> GetDatabyGroup(int group) {
        //var iter = dic.GetEnumerator();
        mapDatas.Clear();
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var value = arr[k].Value as TABLE.NEWBIEACTIVITY;
            if (value.group == group)
            {
                mapDatas.Add(value.id,value);
            }
        }
        return mapDatas;
    }

    Dictionary<int, int> rewardsMap = new Dictionary<int, int>();

    public Dictionary<int, int> GetitemMap(int id)
    {
        rewardsMap.Clear();
        string[] rewards = (array.gItem.id2offset[id].Value as TABLE.NEWBIEACTIVITY).rewards.Split('&');
        for (int i = 0; i < rewards.Length; i++)
        {
            string[] data = rewards[i].Split('#');
            rewardsMap.Add(int.Parse(data[0]),int.Parse(data[1]));
        }

        return rewardsMap;
    }

}