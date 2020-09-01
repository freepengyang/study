using gem;
using instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class EInstanceType
{
    /// <summary>
    /// 行会首领副本
    /// </summary>
    public const int groupId_300010 = 300010;
}

public class CSInstanceInfo : CSInfo<CSInstanceInfo>
{
    Dictionary<int, OneInstanceCount> insLeftCount = new Dictionary<int, OneInstanceCount>();
    InstanceInfo info;

    UndergroundTreasureInstanceInfo _undergroundInfo;
    public UndergroundTreasureInstanceInfo UndergroundInfo { get => _undergroundInfo; set => _undergroundInfo = value; }

    public bool isUnionInstance = false;

    public override void Dispose()
    {

        mClientEvent.UnRegAll();
    }

    InstanceCount mInfo;


    public void Init()
    {
        mClientEvent.Reg((uint)CEvent.Scene_EnterSceneAfter, GetChangeMap);
    }
    void GetChangeMap(uint id, object data)
    {
        //从个人boss退出时，如果剩余次数>0 则打开个人boss面板
        if (IsPersonalBossInstance(CSMainPlayerInfo.Instance.LastMapID) && GetPersonalLeftCount() > 0)
        {
            UIManager.Instance.CreatePanel<UIBossCombinePanel>(p =>
            {
                (p as UIBossCombinePanel).SelectChildPanel(3);
            });
        }

        isUnionInstance = false;
        TABLE.INSTANCE tblInstance = null;
        if(InstanceTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out tblInstance))
        {
            isUnionInstance = (tblInstance.groupid == EInstanceType.groupId_300010);
        }
    }
    public void GetInstanceCountInfo(InstanceCount _mInfo)
    {
        for (int i = 0; i < _mInfo.countList.Count; i++)
        {
            //Debug.Log(_mInfo.countList[i].groupId+"副本次数变动  " + _mInfo.countList[i].leftCount);
            if (insLeftCount.ContainsKey(_mInfo.countList[i].groupId))
            {
                insLeftCount[_mInfo.countList[i].groupId] = _mInfo.countList[i];
            }
            else
            {
                insLeftCount.Add(_mInfo.countList[i].groupId, _mInfo.countList[i]);
            }
        }
    }
    public int GetPersonalLeftCount()
    {
        OneInstanceCount count;
        if (insLeftCount.ContainsKey(13001))
        {
            count = insLeftCount[13001];
            return count.leftCount;
        }
        return 0;
    }
    public OneInstanceCount GetCountDataByType(int _id)
    {
        OneInstanceCount count;
        if (insLeftCount.ContainsKey(_id))
        {
            count = insLeftCount[_id];
            return count;
        }
        return null;
    }

    public int GetInstanceCount(int mapid)
    {
        OneInstanceCount count;
        if (insLeftCount.ContainsKey(mapid))
        {
            count = insLeftCount[mapid];
            return count.leftCount;
        }
        return 0;
    }


    public bool IsPersonalBossInstance(int _mapId)
    {
        return (InstanceTableManager.Instance.GetInstanceType(_mapId) == 13) ? true : false;
    }
    public void GetInstanceInfo(InstanceInfo _info)
    {
        info = _info;

        //Debug.Log(info.state   +"   "+info.usedTime +"   "+info.endTime);

        CSDreamLandInfo.Instance.EnterDreamLand(_info);
    }
    public void GetLeaveInstance()
    {
        CSDreamLandInfo.Instance.ExitDreamLand();
        info = null;
    }
    public InstanceInfo GetInstanceInfo()
    {
        return info;
    }

    public DiLaoInfo _diLaoInfo;

    public DiLaoInfo DiLaoInfo
    {
        get => _diLaoInfo;
        set
        {
            _diLaoInfo = value;
        }
    }

    public bool isGuwu(int skillGroup)
    {
        if (DiLaoInfo == null)
            return false;
        
        return DiLaoInfo.skillId == skillGroup;
    }

    public void ShowDiLaoInfo(DiLaoInfo info)
    {

        _diLaoInfo = info;
        mClientEvent.SendEvent(CEvent.DungeonInfo, info);
    }

    public void ShowUndergroundTreasureInfo(UndergroundTreasureInstanceInfo info)
    {

        //Debug.Log("ShowUndergroundTreasureInfo");
        UndergroundInfo = info;
        mClientEvent.SendEvent(CEvent.UndergroundTreasure, info);
    }

    //个人boss红点判断
    public bool GetPersonalBossRedState()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funP_personalBoss))
        {
            return false;
        }
        CSBetterLisHot<TABLE.INSTANCE> dataList = InstanceTableManager.Instance.GetTableDataByType(13);
        for (int i = 0; i < dataList.Count; i++)
        {
            //等级和卧龙等级满足
            bool lvState = false;
            if (dataList[i].reincarnation > 0)
            {
                if (CSWoLongInfo.Instance.GetWoLongLevel() >= dataList[i].reincarnation)
                {
                    lvState = true;
                }
            }
            else
            {
                if (dataList[i].openLevel <= CSMainPlayerInfo.Instance.Level)
                {
                    lvState = true;
                }
            }
            if (lvState)
            {
                //消耗品数量
                string[] cost = dataList[i].requireItems.Split('#');
                int costId = int.Parse(cost[0]);
                long num = CSItemCountManager.Instance.GetItemCount(costId);
                if (num >= int.Parse(cost[1]))
                {
                    //当天副本进入次数
                    if (GetCountDataByType(13001).leftCount > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public void DetectInstanceState(int instanceId, int state)
    {
        if (state == (int)EInstanceState.Started)
        {
            if (CSAutoFightManager.Instance.IsAutoFight == false)
            {
                TABLE.INSTANCE tblInstance = null;
                if (InstanceTableManager.Instance.TryGetValue(instanceId, out tblInstance))
                {
                    if (tblInstance.autoFight > 0)
                    {
                        CSAutoFightManager.Instance.IsAutoFight = true;
                    }
                }
            }
        }
    }

    public void DetectInstanceAutofight(int instanceId)
    {
        TABLE.INSTANCE tblInstance = null;
        if (InstanceTableManager.Instance.TryGetValue(instanceId, out tblInstance) && (tblInstance.autoFight > 0))
        {
            CSAutoFightManager.Instance.IsAutoFight = true;
        }
        else
        {
            CSAutoFightManager.Instance.IsAutoFight = false;
        }

    }

    public bool IsUnionInstanceFinish()
    {
        if(isUnionInstance)
        {
            if(info != null && info.killedBoss > 0)
            {
                return true;
            }
        }
        return false;
    }

    #region 世界boss鼓舞信息
    worldboss.BlessInfo worldBossBlessInfo;
    public void SetWorldBossBlessInfo(worldboss.BlessInfo _info)
    {
        worldBossBlessInfo = _info;
    }
    public worldboss.BlessInfo GetWorldBossBlessInfo()
    {
        return worldBossBlessInfo;
    }

    worldboss.BossInfo worldBossinfo;
    public void SetWorldBossBuffInfo(worldboss.BossInfo _info)
    {
        worldBossinfo = _info;
    }
    public worldboss.BossInfo GetWorldBossBuffInfo()
    {
        return worldBossinfo;
    }
    #endregion
}
