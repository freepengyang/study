using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIData : MonoBehaviour
{
    public int ID { get; set; }
    public int Type { get; set; }
    private int _itemId = 0;
    public int ItemId
    {
        get
        {
            return _itemId;
        }
        set
        {
            _itemId = value;
            //UnityEngine.Debug.LogError("itemId = " + ItemId);
        }
    }
    public int pos { get; set; }
    public object Data { get; set; }

    //public BagItemInfo bagiteminfo { get; set; }

    private int index = 0;
    public int mIndex
    {
        get { return index; }
        set
        {
            index = value;
        }
    }
    public List<int> list = null;
    //拷贝函数，并不完全拷贝，需要的自己补充
    public void Copy(UIData _data)
    {
        ID = _data.ID;
        Type = _data.Type;
        ItemId = _data.ItemId;
        pos = _data.pos;
        mIndex = _data.mIndex;
        list = new List<int>();
        list.AddRange(_data.list);
    }
}
