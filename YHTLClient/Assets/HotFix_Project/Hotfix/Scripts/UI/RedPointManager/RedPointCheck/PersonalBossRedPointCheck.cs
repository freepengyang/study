using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PersonalBossRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ResInstanceCountMessage, OnCheckRedPoint);

    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PersonalBoss, CSInstanceInfo.Instance.GetPersonalBossRedState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ResInstanceCountMessage, OnCheckRedPoint);
    }
}
