using storehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetStorehouse : CSNetBase
{
    void ECM_ResGetStorehouseInfoMessage(NetInfo _info)
    {
        StorehouseInfo msg = Network.Deserialize<StorehouseInfo>(_info);
        CSStorehouseInfo.Instance.GetStorehouseData(msg);
    }
    void ECM_ResStorehouseItemChangedMessage(NetInfo _info)
    {
        StorehouseItemChangeList msg = Network.Deserialize<StorehouseItemChangeList>(_info);
    }
    void ECM_ResSortStorehouseMessage(NetInfo _info)
    {
        StorehouseItemChangeList msg = Network.Deserialize<StorehouseItemChangeList>(_info);
        CSStorehouseInfo.Instance.GetSrotData(msg);
    }
    void ECM_ResExchangeItemMessage(NetInfo _info)
    {
        ExchangeItemMsg msg = Network.Deserialize<ExchangeItemMsg>(_info);
    }
    void ECM_ResStorehouseToBagMessage(NetInfo _info)
    {
        StorehouseToBagResponse msg = Network.Deserialize<StorehouseToBagResponse>(_info);
        //CSBagInfo.Instance.GetWarehouseToBag(msg.addItem);
        CSStorehouseInfo.Instance.GetWarehouseToBag(msg.removeItem);
    }
    void ECM_ResAddStorehouseCountMessage(NetInfo _info)
    {
        AddStorehouseCount msg = Network.Deserialize<AddStorehouseCount>(_info);
        CSStorehouseInfo.Instance.GetCountChange(msg.storehouseCount);
    }
}
