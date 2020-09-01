using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class BossTableManager : TableManager<TABLE.BOSSARRAY, TABLE.BOSS, int, BossTableManager>
{
    Dictionary<int, int> monsterDic = new Dictionary<int, int>();
    /// <summary>
    /// 野外、炼体boss可预览
    /// </summary>
    /// <param name="bossType"></param>
    /// <returns></returns>
    public Dictionary<int, int> GetPreviewBossMes(int bossType)
    {
        monsterDic.Clear();
        var arr = array.gItem.handles;
        int groupId = 0;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (!monsterDic.ContainsKey(item.monsterid) &&
                item.bossType == bossType &&
                item.group != groupId)
            {
                monsterDic.Add(item.monsterid, item.id);
                groupId = item.group;

            }
        }
        return monsterDic;
    }

    Dictionary<int, List<int>> WildmonsterDic = new Dictionary<int, List<int>>();
    public Dictionary<int, List<int>> GetBossPreviewMes(int bossType)
    {
        WildmonsterDic.Clear();
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.bossType == bossType)
            {
                if (!WildmonsterDic.ContainsKey(item.group))
                {
                    WildmonsterDic.Add(item.group,new List<int>(5));
                }
                WildmonsterDic[item.group].Add(item.id);
            }
        }
        return WildmonsterDic;
    }

    public string GetBossRefreshTimeByMonsterId(int _monsterId)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.monsterid == _monsterId)
            {
                return item.time;
            }
        }
        return "";
    }
    public List<int> GetMapIdsByMonsterId(int _monsterId)
    {
        List<int> list = new List<int>();
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.monsterid == _monsterId)
            {
                list.Add(item.mapId);
            }
        }
        return list;
    }
    public List<int> GetIdsByGroupsToId(int _id)
    {
        List<int> list = new List<int>();
        var arr = array.gItem.handles;
        int groupId = GetBossGroup(_id);
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.group == groupId)
            {
                list.Add(item.id);
            }
        }
        return list;
    }
    int mapId = 0;
    int deliverId = 0;
    int paneld = 0;
    public void GetMapDic(List<int> idList, Dictionary<int, BossMapBtnData> mapDic, List<int> needScreenMapIds)
    {
        mapDic.Clear();

        for (int i = 0; i < idList.Count; i++)
        {
            mapId = 0;
            deliverId = 0;
            paneld = 0;

            mapId = GetBossMapId(idList[i]);
            if (needScreenMapIds.Contains(mapId))
            {
                TABLE.INSTANCE insCfg;
                if (InstanceTableManager.Instance.TryGetValue(mapId, out insCfg))
                {
                    if (insCfg.openLevel > CSMainPlayerInfo.Instance.Level || CSMainPlayerInfo.Instance.Level > insCfg.openLevelMax)
                    {
                        continue;
                    }
                }
            }
            deliverId = GetBossDeliver(idList[i]);
            paneld = GetBossGameModel(idList[i]);
            if (!mapDic.ContainsKey(mapId))
            {
                mapDic.Add(mapId, new BossMapBtnData());
                mapDic[mapId].bossId = idList[i];
                mapDic[mapId].mapId = mapId;
            }
            if (paneld != 0)
            {
                mapDic[mapId].type = 1;
                mapDic[mapId].id = paneld;
            }
            else
            {
                mapDic[mapId].type = 2;
                mapDic[mapId].id = deliverId;
            }
        }
    }
    public void GetMapList(List<int> idList, ILBetterList<BossMapBtnData> _mapList)
    {
        _mapList.Clear();

        for (int i = 0; i < idList.Count; i++)
        {
            mapId = 0;
            deliverId = 0;
            paneld = 0;

            mapId = GetBossMapId(idList[i]);
            deliverId = GetBossDeliver(idList[i]);
            paneld = GetBossGameModel(idList[i]);
            BossMapBtnData data = new BossMapBtnData();
            data.bossId = idList[i];
            data.mapId = GetBossMapId(idList[i]);
            data.monsterId = GetBossMonsterid(idList[i]);
            if (paneld != 0)
            {
                data.type = 1;
                data.id = paneld;
            }
            else
            {
                data.type = 2;
                data.id = deliverId;
            }
            _mapList.Add(data);
        }
    }
    public void GetDeliverIdsByMonsterId(int _monsterId, List<int> list)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.monsterid == _monsterId)
            {
                list.Add(item.deliver);
            }
        }
    }

    public void GetMapsByMonsterId(int _monsterId, List<int> list)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.monsterid == _monsterId)
            {
                list.Add(item.mapId);
            }
        }
    }
    public void GetMapsByGroupId(int _groupId, List<int> list)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSS;
            if (item.group == _groupId)
            {
                list.Add(item.id);
            }
        }
    }
}
