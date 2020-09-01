using storehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Net
{
    //获取仓库信息
    public static void ReqGetStorehouseInfoMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetStorehouseInfoMessage, null);
    }
    //整理仓库请求
    public static void ReqSortStorehouseMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqSortStorehouseMessage, null);
    }
    //交换物品格子请求
    public static void ReqExchangeItemMessage(int _formIndex,int _toIndex)
    {
        ExchangeItemMsg data = CSProtoManager.Get<ExchangeItemMsg>();
        data.fromIndex = _formIndex;
        data.toIndex = _toIndex;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqExchangeItemMessage, data);
    }
    //仓库到背包请求
    public static void ReqStorehouseToBagMessage(int _index)
    {
        //Debug.Log(_index);
        StorehouseToBagRequest data = CSProtoManager.Get<StorehouseToBagRequest>();
        data.storehouseIndex = _index;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqStorehouseToBagMessage, data);
    }
    //扩大仓库请求
    public static void ReqAddStorehouseCountMessage(int _addNum)
    {
        AddStorehouseRequest data = CSProtoManager.Get<AddStorehouseRequest>();
        data.num = _addNum;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqAddStorehouseCountMessage, data);
    }
    
}
