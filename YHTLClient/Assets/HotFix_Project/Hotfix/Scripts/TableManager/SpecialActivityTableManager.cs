using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class SpecialActivityTableManager : TableManager<TABLE.SPECIALACTIVITYARRAY, TABLE.SPECIALACTIVITY, int, SpecialActivityTableManager>
{
    
    public List<int> GetOpenAcIdList()
    {
        List<int> ids = new List<int>();
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            //if (iter.Current.Value.openLevel <= CSMainPlayerInfo.Instance.Level && iter.Current.Value.starttime <= CSMainPlayerInfo.Instance.ServerOpenDay
            //    && CSMainPlayerInfo.Instance.ServerOpenDay <= (iter.Current.Value.starttime + iter.Current.Value.eventLast))
            //{
            ids.Add(arr[k].key);
            //}
        }
        return ids;
    }
    public string GetTabName(int _id)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVITY;
            if (item.eventId == _id)
            {
                return item.eventName;
            }
        }
        return "";
    }

    /// <summary>
    /// 根据eventId获取数据
    /// </summary>
    /// <param name="_eventId"></param>
    /// <returns></returns>
    public CSBetterLisHot<TABLE.SPECIALACTIVITY> GetTableDataByEventId(int _eventId)
    {
        //list.Clear();
        CSBetterLisHot<TABLE.SPECIALACTIVITY> list = new CSBetterLisHot<TABLE.SPECIALACTIVITY>();
        int i = 0;
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVITY;
            if (item.eventId == _eventId)
            {
                list.Add(item);
            }
            i++;
        }
        list.Sort((a, b) => { return (a.id < b.id ? -1 : 1); });
        return list;
    }


    public string GetAcTabsName(int _order)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SPECIALACTIVITY;
            if (item.order == _order)
            {
                return item.eventName;
            }
        }
        return "";
    }

}
