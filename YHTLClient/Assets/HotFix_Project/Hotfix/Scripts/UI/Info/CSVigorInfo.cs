using energy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSVigorInfo : CSInfo<CSVigorInfo>
{
    double energyExp = 0;
    int maxEnergyExp = 0;
    Dictionary<int, TimerState> idsDic = new Dictionary<int, TimerState>();
    int maxExchangeTime = 0;
    int doneExchangeTimes = 0;
    /// <summary>
    /// 是否拥有充值领取
    /// </summary>
    public bool hasChargeEnergy = false;
    /// <summary>
    /// 是否领取过
    /// </summary>
    public bool isGetChargeEnergy = false;
    public void Init()
    {
        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor))
        {
            Net.CSEnergyInfoMessage();
            Net.CSEnergyFreeGetInfoMessage();
            Net.CSGetEnergeExchangeInfoMessage();
        }
        else
        {
            mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, GetFunctionOpen);
        }
    }

    public void SetVigorInfo(EnergyInfoResponse _msg)
    {
        energyExp = _msg.energy;
        maxEnergyExp = EnergyStorageTableManager.Instance.GetEnergyStorageMaxEneNum(CSMainPlayerInfo.Instance.Level);
    }
    public void SetVigorFreeIds(EnergyFreeGetInfoResponse _msg)
    {
        hasChargeEnergy = _msg.hasChargeEnergy;
        isGetChargeEnergy = _msg.isGetChargeEnergy;
        idsDic.Clear();
        for (int i = 0; i < _msg.timers.Count; i++)
        {
            idsDic.Add(_msg.timers[i].timerId, _msg.timers[i]);
        }
    }
    public void SetVigorValue(double _msg)
    {
        energyExp = _msg;
    }
    public void GetExchangeVigor(int _id)
    {
        if (idsDic.ContainsKey(_id))
        {
            idsDic[_id].state = 3;
        }
    }
    public void SetEnergyExchangeInfo(EnergyExchangeInfo _msg)
    {
        maxExchangeTime = _msg.maxExchangeTime.value;
        doneExchangeTimes = _msg.doneExchangeTimes.value;
    }


    public double GetCurrentVigorExp()
    {
        return energyExp;
    }
    public void GetOpenIds(out Dictionary<int, TimerState> _list)
    {
        _list = idsDic;
    }
    public int GetMaxChangeCount()
    {
        return maxEnergyExp;
    }
    public int GetTodayTotalCount()
    {
        return maxExchangeTime;
    }
    public int GetDoneChangeCount()
    {
        return doneExchangeTimes;
    }
    public override void Dispose()
    {

    }

    #region 红点判断
    void GetFunctionOpen(uint id, object data)
    {
        FunctionType type = (FunctionType)data;
        if (type == FunctionType.funcP_vigor)
        {
            mClientEvent.UnReg(CEvent.FunctionOpenStateChange, GetFunctionOpen);
            Net.CSEnergyInfoMessage();
            Net.CSEnergyFreeGetInfoMessage();
            Net.CSGetEnergeExchangeInfoMessage();
        }
    }
    public bool GetVigorRedState()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor))
        {
            return false;
        }
        if (energyExp > 0)
        {
            return false;
        }
        //var iter = idsDic.GetEnumerator();
        //while (iter.MoveNext())
        //{
        //    if (iter.Current.Value.state == 2)
        //    {
        //        return true;
        //    }
        //}
        //兑换前判断是否超上限
        if (energyExp >= maxEnergyExp)
        {
            return false;
        }
        else
        {
            if (doneExchangeTimes < maxExchangeTime)
            {
                TABLE.ENERGYEXCHANGE msg = EnergyExchangeTableManager.Instance.GetSingleCfgByTimes(doneExchangeTimes + 1);
                int cfgId = (msg.costType1 == 0) ? msg.costType2 : msg.costType1;
                int num = (msg.costType1 == 0) ? msg.costNum2 : msg.costNum1;
                if (num <= CSItemCountManager.Instance.GetItemCount(cfgId))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    #region 精力值已满判断
    public bool IsVigorFull()
    {
        return energyExp >= maxEnergyExp;
    }
    #endregion
}
