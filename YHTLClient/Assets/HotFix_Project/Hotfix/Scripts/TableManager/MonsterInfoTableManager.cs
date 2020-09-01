using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MonsterInfoTableManager : TableManager<TABLE.MONSTERINFOARRAY, TABLE.MONSTERINFO, int, MonsterInfoTableManager>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public int GetWorldBossModel()
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.MONSTERINFO;
            if (item.type == 5)
            {
                return item.model;
            }
        }
        return 0;
    }
    public string GetWorldBossName()
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.MONSTERINFO;
            if (item.type == 5)
            {
                return item.name;
            }
        }
        return "";
    }
    public TABLE.MONSTERINFO GetWorldBossCfg()
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.MONSTERINFO;
            if (item.type == 5)
            {
                return item;
            }
        }
        return null;
    }
}
