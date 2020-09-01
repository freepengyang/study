using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ZhuFuRedCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("祝福红点状态  " + CSBagInfo.Instance.GetBagRedPointState());
        RefreshRed(RedPointType.ZhuFu, CSBagInfo.Instance.ZhuFuRedPointState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
    }
}
