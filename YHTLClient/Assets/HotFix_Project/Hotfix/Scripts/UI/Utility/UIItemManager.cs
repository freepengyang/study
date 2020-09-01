using System;
using System.Collections.Generic;
using AssetBundles;
using UnityEngine;
using Object = UnityEngine.Object;

//格子类的管理
public enum PropItemType
{
    Normal,
    EquipShow,
    Bag,
    Recycle,
    Warehouse,
    GuildWarehouse,
    GuildBag,
}
public enum itemSize
{
    Default = 74,//74*74
    Size72 = 72,//66*66
    Size66 = 66,//66*66
    Size50 = 50,// 50* 50
    Size64 = 64,
    Size54 = 54,
    Size58 = 58,
    Size60 = 60,
    Size70 = 70,
    Size68 = 68,
    Size26 = 26,
    Size32 = 32,
    Size40 = 40,
    Size44 = 44,
}
public enum PropItemState
{
    Selected,
    Normal,
    Unlock,
}
public class UIItemManager : CSInfo<UIItemManager>
{
    GameObject obj = null;

    public override void Dispose()
    {
        if (obj != null)
        {
            AssetBundleManager.UnloadAssetBundle(itemPath);
            obj = null;
        }
        for (int i = 0; i < spareItems.Count; i++)
        {
            GameObject.Destroy(spareItems[i].obj);
        }
        spareItems.Clear();
        GameObject.DestroyImmediate(obj_itemRoot);
    }
    private GameObject _obj_itemRoot;
    public GameObject obj_itemRoot
    {
        get
        {
            if (_obj_itemRoot == null)
            {
                _obj_itemRoot = new GameObject("ItemRoot");
                GameObject.DontDestroyOnLoad(_obj_itemRoot);
            }
            return _obj_itemRoot;
        }
    }
    Transform _transRoot;
    public Transform TransRoot
    {
        get
        {
            if (_transRoot == null) { _transRoot = obj_itemRoot.transform; }
            return _transRoot;
        }
    }
    string itemPath = "ItemBase";

    private List<UIItemBase> spareItems = new List<UIItemBase>();
    public void Init()
    {
        TransRoot.SetParent(GameObject.Find("UI Root").transform);
        TransRoot.localScale = Vector3.one;
        TransRoot.localPosition = new Vector3(10000, 10000, 10000);
    }
    public UIItemBase GetItem(PropItemType _type, Transform _parent, itemSize size = itemSize.Default)
    {
        if (_parent == null)
        {
            return null;
        }
        UIItemBase item = GetISpareItem(_type, _parent, size);
        return item;
    }
    //考虑预生成一部分数量的格子类对象（如果某些界面要求打开速度更快）
    public List<UIItemBase> GetUIItems(int _num, PropItemType _type, Transform _parent, itemSize size = itemSize.Default)
    {
        if (_parent == null)
        {
            return null;
        }
        if (_num <= 0) return null;
        List<UIItemBase> list = new List<UIItemBase>();

        for (int i = 1; i <= _num; i++)
        {
            UIItemBase item = GetISpareItem(_type, _parent, size);
            list.Add(item);
        }
        return list;
    }
    public void RecycleSingleItem(UIItemBase _item)
    {
        if (_item == null) return;
        if (spareItems.Contains(_item))
        {
            FNDebug.Log("重复回收   界面检查修改");
            return;
        }
        _item.obj.transform.SetParent(TransRoot);
        _item.obj.transform.localPosition = Vector3.zero;
        _item.obj.transform.localScale = Vector3.one;
        _item.UnInit();
        _item.CancelDrag();
        _item.obj.SetActive(false);
        ChangeItemSize(_item, itemSize.Default);
        spareItems.Add(_item);
        //Debug.Log("回收单个   "+ spareItems.Count);
    }
    public void RecycleItemsFormMediator(List<UIItemBase> _items)
    {
        if (_items == null) return;
        for (int i = 0; i < _items.Count; i++)
        {
            if (spareItems.Contains(_items[i]))
            {
                FNDebug.Log("重复回收   界面检查修改");
                continue;
            }
            _items[i].obj.transform.SetParent(TransRoot);
            _items[i].obj.transform.localPosition = Vector3.zero;
            _items[i].obj.transform.localScale = Vector3.one;
            _items[i].UnInit();
            _items[i].CancelDrag();
            _items[i].obj.SetActive(false);
            ChangeItemSize(_items[i], itemSize.Default);
            spareItems.Add(_items[i]);
        }
        _items.Clear();
        _items = null;
        //Debug.Log("回收list   " + spareItems.Count);
    }
    UIItemBase GetISpareItem(PropItemType _type, Transform _parent, itemSize _size)
    {
        UIItemBase item;
        if (spareItems.Count > 0)
        {
            item = GetSpareItems();
            item.SetParent(_parent);
            ChangeItemSize(item, _size);
            item.obj.SetActive(true);
            item.ChangeType(_type);
        }
        else
        {
            item = GetNewItemObj(_type, _parent);
            ChangeItemSize(item, _size);
        }
        return item;
    }
    UIItemBase GetNewItemObj(PropItemType _type, Transform _parent)
    {
        return new UIItemBase(loadUI(itemPath, _parent), _type);
    }
    UIItemBase GetSpareItems()
    {
        UIItemBase item = spareItems[0];
        spareItems.RemoveAt(0);
        return item;
    }
    void ChangeItemSize(UIItemBase _item, itemSize _size)
    {
        _item.SetSize(_size);
    }
    GameObject loadUI(string _name, Transform parent)
    {
        if (obj == null)
        {
            bool isMiniLoad = CSGame.Sington.IsMiniApp && !UIManager.Instance.ignoreABList.Contains(_name);
            obj = AssetBundleLoad.LoadUIAsset(_name, isMiniLoad);
        }
        if (obj != null)
        {
            GameObject inst = Object.Instantiate(obj);
            if (inst != null)
            {
                inst.SetActive(true);
                NGUITools.SetParent(parent, inst);
                return inst;
            }
        }
        return null;
    }
}
