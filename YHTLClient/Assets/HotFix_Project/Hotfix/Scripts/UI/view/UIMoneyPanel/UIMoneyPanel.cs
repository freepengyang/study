using System.Collections.Generic;

public partial class UIMoneyPanel : UIBasePanel
{
    protected FastArrayMeta<int> mMoneyIds;
    protected FastArrayElementKeepHandle<ItemBarData> mItemBarDatas;
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
    {
        base.Init();

        mMoneyIds = new FastArrayMeta<int>();
        mItemBarDatas = new FastArrayElementKeepHandle<ItemBarData>(8);

        mClientEvent.AddEvent(CEvent.OnMoneyStackChanged, OnMoneyStackChanged);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMoneyStackChanged, OnMoneyStackChanged);
        mMoneyIds.Clear();
        mMoneyIds = null;
        mItemBarDatas.Clear();
        mItemBarDatas = null;
        UIItemMoneyManager.Instance.UnBind(mMoneyGrids);
        mMoneyGrids = null;
        base.OnDestroy();
    }

    protected void OnMoneyStackChanged(uint id,object argv)
    {
        MoneyData moneyData = argv as MoneyData;
        if(null != moneyData)
            SetMoneyIds(moneyData.moneyIds);
    }

    protected void SetMoneyIds(FastArrayMeta<int> moneyIds)
    {
        mMoneyIds.Clear();
        mMoneyIds.AddRange(moneyIds);
        OnMoneyChanged();
    }

    protected void OnMoneyChanged()
    {
        mItemBarDatas.Clear();
        for(int i = 0; i < mMoneyIds.Count; ++i)
        {
            TABLE.MONEYTYPE item = null;
            if(!MoneyTypeTableManager.Instance.TryGetValue(mMoneyIds[i], out item))
            {
                continue;
            }
            var itemData = UIItemMoneyManager.Instance.Get();
            itemData.bgWidth = 184;
            itemData.cfgId = item.id;
            itemData.needed = 0;
            itemData.flag = (int)ItemBarData.ItemBarType.IBT_MONEYPANEL;
            itemData.owned = itemData.cfgId.GetItemCountByMoneyType();
            itemData.eventHandle = mClientEvent;
            itemData.desId = item.desid;
            mItemBarDatas.Append(itemData);
        }
        UIItemMoneyManager.Instance.Bind(mMoneyGrids,mItemBarDatas);
        mItemBarDatas.Clear();
        mtable_con.Reposition();
    }
}