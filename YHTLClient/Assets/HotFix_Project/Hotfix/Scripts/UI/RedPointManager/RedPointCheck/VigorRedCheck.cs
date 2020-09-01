using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VigorRedCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SCEnergyFreeGetInfoMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SCGetFreeEnergyMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SCEnergyExchangeInfoMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Vigor, CSVigorInfo.Instance.GetVigorRedState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.SCEnergyFreeGetInfoMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SCGetFreeEnergyMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SCEnergyExchangeInfoMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);

    }
}
