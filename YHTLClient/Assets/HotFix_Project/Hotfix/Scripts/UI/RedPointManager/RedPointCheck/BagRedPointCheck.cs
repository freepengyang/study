
public class BagRedPointCheck : RedPointCheckBase
{
    //背包红点显示相关
    //  1.背包已满    2.新宝箱类道具   3.存在有比身上装备好的装备（策划确认不影响外边红点，只在界面内显示）
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.BagBoxItemClick, OnCheckRedPoint);

    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("背包红点状态  " + CSBagInfo.Instance.GetBagRedPointState());
        RefreshRed(RedPointType.Bag, CSBagInfo.Instance.GetBagRedPointState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.BagBoxItemClick, OnCheckRedPoint);
    }
}
