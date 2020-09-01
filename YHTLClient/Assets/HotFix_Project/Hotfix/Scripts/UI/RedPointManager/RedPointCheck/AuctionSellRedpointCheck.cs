using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuctionSellRedpointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ECM_ResGetAuctionShelfMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ECM_ResAddToShelfMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ECM_ResRemoveFromShelfMessage, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.AuctionSell, UIAuctionInfo.Instance.AuctionSellRedpoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ECM_ResGetAuctionShelfMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ECM_ResAddToShelfMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ECM_ResRemoveFromShelfMessage, OnCheckRedPoint);
    }
}
