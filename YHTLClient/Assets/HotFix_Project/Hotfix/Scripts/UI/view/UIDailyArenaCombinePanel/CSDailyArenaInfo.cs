using athleticsactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSDailyArenaInfo : CSInfo<CSDailyArenaInfo>
{
    List<DailyArenaData> resDic = new List<DailyArenaData>();
    public bool showBubble = true;
    public override void Dispose()
    {

    }
    public void Init()
    {
        Net.CSAthleticsActivityInfoMessage();
    }
    public void SCAthleticsActivityInfoMessage(AthleticsActivityInfoResponse _mes)
    {
        resDic.Clear();
        for (int i = 0; i < _mes.athleticsActivityInfos.Count; i++)
        {
            //UnityEngine.Debug.Log($" 页签  {_mes.athleticsActivityInfos[i].id}   {_mes.athleticsActivityInfos[i].activityRewardInfos.Count}");
            resDic.Add(new DailyArenaData(_mes.athleticsActivityInfos[i]));
        }
    }
    public void SCReceiveAthleticsActivityRewardMessage(ActivityRewardInfo _mes)
    {
        for (int i = 0; i < resDic.Count; i++)
        {
            if (resDic[i].dic.ContainsKey(_mes.id))
            {
                resDic[i].dic[_mes.id].ResetData(_mes);
            }
        }
    }
    public void ECM_ActivityRewardInfoChangeNotify(ActivityRewardInfoChange msg)
    {
        for (int i = 0; i < resDic.Count; i++)
        {
            for (int j = 0; j < msg.activityRewardInfos.Count; j++)
            {
                if (resDic[i].dic.ContainsKey(msg.activityRewardInfos[j].id))
                {
                    resDic[i].dic[msg.activityRewardInfos[j].id].ResetData(msg.activityRewardInfos[j]);
                }
            }
        }
    }

    ILBetterList<DailyArenaData> SelectresDic = new ILBetterList<DailyArenaData>();
    public ILBetterList<DailyArenaData> GetDailyInfo()
    {
        SelectresDic.Clear();
        for (int i = 0; i < resDic.Count; i++)
        {
            TABLE.JINGJIHUODONG huodongCfg;
            if (JingjiHuodongTableManager.Instance.TryGetValue(resDic[i].id, out huodongCfg))
            {
                if (resDic[i].Acstate == 0)
                {
                    SelectresDic.Add(resDic[i]);
                }
                else
                {
                    if (huodongCfg.open2 != 0 && huodongCfg.close2 != 0)
                    {
                        if (huodongCfg.open2 <= CSMainPlayerInfo.Instance.ServerOpenDay && CSMainPlayerInfo.Instance.ServerOpenDay < huodongCfg.close2)
                        {
                            SelectresDic.Add(resDic[i]);
                        }
                    }
                }
            }
        }
        SelectresDic.Sort((a,b)=> 
        {
            return a.id - b.id;
        });
        return SelectresDic;
    }

    public bool DailyAranaRedState()
    {
        for (int i = 0; i < resDic.Count; i++)
        {
            var iter = resDic[i].dic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.num > 0 && iter.Current.Value.state == 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
public class DailyArenaData
{
    public int id;
    /// <summary>
    /// 页签活动状态：0进行中，1未开启，2已结束
    /// </summary>
    public int Acstate;
    public long remainTime;
    public Dictionary<int, DailyArenaRewardData> dic = new Dictionary<int, DailyArenaRewardData>();
    public DailyArenaData(AthleticsActivityInfo _info)
    {
        id = _info.id;
        remainTime = _info.endTime;
        Acstate = _info.state;
        dic.Clear();
        for (int i = 0; i < _info.activityRewardInfos.Count; i++)
        {
            dic.Add(_info.activityRewardInfos[i].id, new DailyArenaRewardData(_info.activityRewardInfos[i]));
        }
    }
}
public class DailyArenaRewardData
{
    public int id;
    public int pro;
    public int num;
    /// <summary>
    /// 单个活动奖励状态：0进行中，1可领取，2已完成
    /// </summary>
    public int state;
    public DailyArenaRewardData(ActivityRewardInfo _info)
    {
        ResetData(_info);
    }
    public void ResetData(ActivityRewardInfo _info)
    {
        //UnityEngine.Debug.Log($" 奖励ID  {_info.id}   {_info.curProcess}   {_info.surplusNum}  {_info.state}");
        id = _info.id;
        pro = _info.curProcess;
        num = _info.surplusNum;
        state = _info.state;
    }
}
