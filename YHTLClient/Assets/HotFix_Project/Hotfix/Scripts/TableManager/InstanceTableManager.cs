using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class InstanceTableManager : TableManager<TABLE.INSTANCEARRAY, TABLE.INSTANCE, int, InstanceTableManager>
{
    CSBetterLisHot<TABLE.INSTANCE> list = new CSBetterLisHot<TABLE.INSTANCE>();
    public CSBetterLisHot<TABLE.INSTANCE> GetTableDataByType(int _type)
    {
        list.Clear();
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == _type)
            {
                list.Add(item);
            }
        }
        list.Sort((a, b) => { return (a.id < b.id ? -1 : 1); });
        return list;
    }
    public int GetWorldBossOpenLevel()
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 3)
            {
                return item.openLevel;
            }
        }
        return 0;
    }
    public int GetWorldBossOpenId()
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 3)
            {
                return item.id;
            }
        }
        return 0;
    }
    public int GetWorldBossTotalTime()
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 3)
            {
                return item.totalTime;
            }
        }
        return 0;
    }
    public int GetSpringPaoDianOpenId()
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 10)
            {
                return item.id;
            }
        }
        return 0;
    }
    public int GetSpringPaoDianTotalTime()
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 10)
            {
                return item.totalTime;
            }
        }
        return 0;
    }

    public int GetInstanceIdByType(int _type, int level)
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == _type && item.level == level)
                return item.id;
        }
        return 0;
    }

    public int GetInstanceCountByType(int _type)
    {
        int count = 0;
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == _type)
            {
                count++;
            }
        }
        return count;
    }

    public int GetHonorChanllengeMaxLevel()
    {
        int i = 0;
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.INSTANCE;
            if (item.type == 20)
            {
                i++;
            }
        }
        return i;
    }
}
