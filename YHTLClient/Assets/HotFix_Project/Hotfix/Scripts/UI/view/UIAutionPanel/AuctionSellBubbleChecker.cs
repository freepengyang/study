using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuctionSellBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get
        {
            return (int)FunctionPromptType.AuctionSell;
        }
    }

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return UIAuctionInfo.Instance.HasCanSellItems();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_Auction))
        {
            return;
        }
        UIManager.Instance.CreatePanel<UIAuctionPanel>(p =>
        {
            (p as UIAuctionPanel).SelectChildPanel(2);
        });
    }
}