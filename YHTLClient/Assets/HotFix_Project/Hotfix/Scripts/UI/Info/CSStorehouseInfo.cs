using bag;
using Google.Protobuf.Collections;
using storehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSStorehouseInfo : CSInfo<CSStorehouseInfo>
{


    Dictionary<long, bag.BagItemInfo> mItemData;
    Dictionary<int, bag.BagItemInfo> mItemIndexData;
    StorehouseInfo msg;
    int curItemNum;
    int maxCount;
    public void GetStorehouseData(StorehouseInfo _msg)
    {
        msg = _msg;
        curItemNum = msg.itemInfo.Count;
        maxCount = msg.maxCount;
        mItemData = new Dictionary<long, BagItemInfo>();
        mItemIndexData = new Dictionary<int, BagItemInfo>();
        for (int i = 0; i < msg.itemInfo.Count; i++)
        {
            mItemData.Add(msg.itemInfo[i].id, msg.itemInfo[i]);
        }
        for (int i = 0; i < msg.itemInfo.Count; i++)
        {
            mItemIndexData.Add(msg.itemInfo[i].bagIndex, msg.itemInfo[i]);
        }
        mClientEvent.SendEvent(CEvent.GetWarehouseData);
    }
    public void GetBagToWarehouse(RepeatedField<bag.BagItemInfo> _repeat)
    {
        for (int i = 0; i < _repeat.Count; i++)
        {
            //Debug.Log(_repeat[i].id +"   "+_repeat[i].count);
            if (mItemData.ContainsKey(_repeat[i].id))
            {
                mItemData[_repeat[i].id] = _repeat[i];
            }
            else
            {
                mItemData.Add(_repeat[i].id, _repeat[i]);
            }
        }
        for (int i = 0; i < _repeat.Count; i++)
        {
            //Debug.Log(" 背包放入仓库 " + _repeat[i].bagIndex);
            if (mItemIndexData.ContainsKey(_repeat[i].bagIndex))
            {
                mItemIndexData[_repeat[i].bagIndex] = _repeat[i];
            }
            else
            {
                mItemIndexData.Add(_repeat[i].bagIndex, _repeat[i]);
            }
        }
        curItemNum = mItemData.Count;
        mClientEvent.SendEvent(CEvent.GetWarehouseItemsChange);
    }
    public void GetWarehouseToBag(RepeatedField<bag.BagItemInfo> _repeat)
    {
        for (int i = 0; i < _repeat.Count; i++)
        {
            //Debug.Log(_repeat[i].id + "   " + _repeat[i].count);
            mItemData.Remove(_repeat[i].id);
        }
        for (int i = 0; i < _repeat.Count; i++)
        {
            //Debug.Log("   仓库取出到背包" + _repeat[i].bagIndex);
            mItemIndexData.Remove(_repeat[i].bagIndex);
        }
        curItemNum = mItemData.Count;
        mClientEvent.SendEvent(CEvent.GetWarehouseItemsChange);
    }
    public void GetSrotData(StorehouseItemChangeList _msg)
    {
        mItemData.Clear();
        mItemIndexData.Clear();
        for (int i = 0; i < _msg.changeList.Count; i++)
        {
            mItemData.Add(_msg.changeList[i].id, _msg.changeList[i]);
        }
        for (int i = 0; i < _msg.changeList.Count; i++)
        {
            mItemIndexData.Add(_msg.changeList[i].bagIndex, _msg.changeList[i]);
        }
        mClientEvent.SendEvent(CEvent.GetWarehouseSort);
    }
    public void GetCountChange(int _num)
    {
        maxCount = _num;
        mClientEvent.SendEvent(CEvent.GetWarehouseItemsChange);
    }
    public void GetDataByPage(int _page, Dictionary<long, bag.BagItemInfo> _dic)
    {
        _dic.Clear();
        int startNum = 25 * (_page - 1) + 1;
        int endNum = startNum + 24;
        var iter = mItemData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (startNum <= iter.Current.Value.bagIndex && iter.Current.Value.bagIndex <= endNum)
            {
                _dic.Add(iter.Current.Key, iter.Current.Value);
            }
        }
    }
    public void GetDataByPage(int _page, Dictionary<int, bag.BagItemInfo> _dic)
    {
        _dic.Clear();
        int startNum = 25 * (_page - 1) + 1;
        int endNum = startNum + 24;
        var iter = mItemIndexData.GetEnumerator();
        while (iter.MoveNext())
        {
            if (startNum <= iter.Current.Value.bagIndex && iter.Current.Value.bagIndex <= endNum)
            {
                _dic.Add(iter.Current.Key, iter.Current.Value);
            }
        }
    }
    public bag.BagItemInfo GetIndexIsNil(int _bagindex)
    {
        if (null != mItemIndexData && mItemIndexData.ContainsKey(_bagindex))
            return mItemIndexData[_bagindex];
        return null;
    }
    public int GetMaxCount()
    {
        return maxCount;
    }
    public int GetCurCount()
    {
        return curItemNum;
    }
    //public int GetLocked
    public override void Dispose()
    {

    }

    #region 排序倒计时
    int sec = 10;
    Schedule sortSchedule;
    bool canSort = true;

    public int Sec
    {
        get => sec;
    }

    public Schedule SortSchedule
    {
        get => sortSchedule;
        set => sortSchedule = value;
    }

    public bool CanSort
    {
        get => canSort;
        set => canSort = value;
    }

    private UIBagPanel uiBagPanel;
    public void SortCountDown(Schedule schedule)
    {
        sec--;
        canSort = sec <= 0 ? true : false;
        mClientEvent.SendEvent(CEvent.WarehouseCountDown);
        if (sec <= 0)
        {
            sec = 10;
            Timer.Instance.CancelInvoke(SortSchedule);
        }
    }

    #endregion
}
