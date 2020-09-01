using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine.Assertions.Must;

public partial class UIJewelryBoxPanel : UIBasePanel
{
    private CSNostalgiaEquipInfo Info;
    EndLessList<NostalgiaBagItem, NostalgiaBagItemData> endLessList; //背包格子的bind
    private int curPage;
    public float scrollequipx;
    //public int maxPage = 8;
    
    public int CurPage
    {
        get => curPage;
        set
        {
            curPage = value;
        }
    }
    
    public override void Init()
    {
        base.Init();
        Info = CSNostalgiaEquipInfo.Instance;
         endLessList = new EndLessList<NostalgiaBagItem, NostalgiaBagItemData>
             (SortType.Horizen, mwrap_page, mPoolHandleManager, 2, ScriptBinder);
        //items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mequipGrid.transform);
        //SuitUiClasses = mPoolHandleManager.GetSystemClass<Dictionary<int, SuitUIClassBinder>>();
        //SuitUiClasses.Clear();
        
        mClientEvent.AddEvent(CEvent.NostalgiaBagChange,RefreshBag);
        mClientEvent.AddEvent(CEvent.NostalgiaEquipChange,RefreshUI);
        mClientEvent.AddEvent(CEvent.NostalGeziChange,RefreshEquip);
        mClientEvent.AddEvent(CEvent.NostalsuitChange,RefreshUI);
        mClientEvent.AddEvent(CEvent.NostalgiaRemove,RefreshUI);
        mscollbar.onChange.Add(new EventDelegate(OnChangePage));
        scrollequipx = mscrollItems.transform.localPosition.x;
        UIEventListener.Get(mbtn_get).onClick = OnGetClick;
    }

    public override void Show()
    {
        base.Show();
        RefreshUI();
        CurPage = 1;
        mlb_page.text = CSString.Format(1704, CurPage);
        mscrollItems.ResetPosition();
        SpringPanel.Begin(mscrollItems.gameObject, new Vector3(-8,-8,0), 50);
    }

    protected override void OnDestroy()
    {
       
        mSuit.UnBind<SuitUIClassBinder>();
        mgcontain_equip.UnBind<equipItemClassBinder>();
        endLessList?.Destroy();
        endLessList = null;
        mClientEvent.RemoveEvent(CEvent.NostalgiaBagChange,RefreshBag);
        mClientEvent.RemoveEvent(CEvent.NostalgiaEquipChange,RefreshUI);
        mClientEvent.RemoveEvent(CEvent.NostalGeziChange,RefreshEquip);
        mClientEvent.RemoveEvent(CEvent.NostalgiaRemove,RefreshUI);
        mClientEvent.RemoveEvent(CEvent.NostalsuitChange,RefreshUI);
        base.OnDestroy();
    }

    /// <summary>    
    /// 切换分页
    /// </summary>
    private void OnChangePage()
    {
        float x = mscrollItems.transform.localPosition.x;

        int page = UtilityMath.GetRoundingInt((scrollequipx - x) / mwrap_page.itemSize) + 1;
        if (page <= 0) page = 1;
        if (page >= Info.PageNum) page = Info.PageNum;
        //UnityEngine.Debug.Log(scrollequipx+"||" + x  + "||" + mwrap_page.itemSize + "||" +page);
        if (page != CurPage)
        {
            CurPage = page;
            mlb_page.text = CSString.Format(1704, CurPage);
        }
    }

    private void OnGetClick(GameObject obj)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1063));
    }
    
    #region 刷新事件

    /// <summary>
    /// 刷新整个界面
    /// </summary>
    private void RefreshUI(uint uievtid = 0, object data = null)
    {
        RefreshEquip();
        RefreshBag();
        //RefreshSuit();
    }

    /// <summary>
    /// 刷新怀旧背包
    /// </summary>
    private void RefreshBag(uint uievtid = 0, object data = null)
    {
        endLessList.Clear();
        Info.GetSortList(NostalgiaSelectType.BAG,false);
        
        int PageNum = Info.PageNum;
        
        for (int i = 0; i < PageNum; i++)
        {
            //var nostalgiaBagItemData = mPoolHandleManager.GetSystemClass<NostalgiaBagItemData>();
            //nostalgiaBagItemData.page = i + 1;
            var tempdata = endLessList.Append();
            tempdata.page = i + 1;
        }
        endLessList.Bind();
    }

    /// <summary>
    /// 刷新装备格子
    /// </summary>
    private void RefreshEquip(uint uievtid = 0, object data = null)
    {
        // if (equipItems == null)
        // {
        //     equipItems = mPoolHandleManager.GetSystemClass<ILBetterList<equipItemClass>>();
        //     equipItems.Clear();
        // }
        var slots = Info.GetSlots();
        //var equiplist = Info.EquipList;
        // var list = mPoolHandleManager.GetSystemClass<ILBetterList<NostalgiaBagClass>>();
        // list.Clear();
        // for (var it = equiplist.GetEnumerator(); it.MoveNext();)
        // {
        //     list.Add(it.Current.Value);
        // }
        //list.Sort(Info.SortList);
        
        var list = Info.GetSortList(NostalgiaSelectType.EQUIP, false);
        
        mgcontain_equip.MaxCount = slots.Count;
        for (int i = 0; i < mgcontain_equip.MaxCount; i++)
        {
            var gp = mgcontain_equip.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            equipItemClassBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new equipItemClassBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as equipItemClassBinder;
            }
            Binder._index = i;
            var bagClass = list.Count > i ? list[i] : null;
            Binder.Bind(bagClass);
            
            // Transform trans = mgcontain_equip.controlList[i].transform;
            //
            // UIItemBase item;
            // if (i >= items.Count)
            // {
            //     item = items.Append();
            // }
            // else
            //     item = items[i];
            //
            // equipItemClass equipItem;
            // if (i >= equipItems.Count)
            // {
            //     equipItem = mPoolHandleManager.GetSystemClass<equipItemClass>();
            //     equipItem.Init(trans,i,item);
            //     equipItems.Add(equipItem);
            // }
            // else
            //     equipItem = equipItems[i];
            // equipItem.Refresh(list);
            
        }
        //mPoolHandleManager.Recycle(list);
        RefreshSuit();
    }

    /// <summary>
    /// 刷新套装
    /// </summary>
    private void RefreshSuit(uint uievtid = 0, object data = null)
    {
        for (int i = 0; i < mSuit.MaxCount; i++)
        {
            var gp = mSuit.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            SuitUIClassBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new SuitUIClassBinder();
                Binder.Setup(eventHandle);
            }
            else
                Binder = eventHandle.parameter as SuitUIClassBinder;
            
            if (Info.suitInfos.ContainsKey(i+1))
                Binder.Bind(Info.suitInfos[i+1]);  
            
            //UIEventListener.Get(mSuit.controlList[i],suitInfo).onClick = OnClickSuit;
            
            // SuitUIClassBinder suitUIClass;
            // if (!SuitUiClasses.ContainsKey(i))
            // {
            //     suitUIClass = mPoolHandleManager.GetSystemClass<SuitUIClassBinder>();
            //     suitUIClass.Init(mSuit.controlList[i-1].transform);
            //     SuitUiClasses.Add(i,suitUIClass);
            // }
            // else
            //     suitUIClass = SuitUiClasses[i];
            //
            // if (Info.suitInfos.ContainsKey(i))
            // {
            //     var suitInfo = Info.suitInfos[i]; 
            //     int max = suitInfo.maxNum;
            //         int curnum = suitInfo.Count;
            //     var Huaijiusuit = suitInfo.Huaijiusuit;
            //     suitUIClass.Sprite.spriteName = Huaijiusuit.pic;
            //     //suitUIClass.Sprite.spriteName = Info.lowSuits[i].pic;
            //     UIEventListener.Get(mSuit.controlList[i - 1],suitInfo).onClick = OnClickSuit;
            //     string str = CSString.Format(1952, curnum, max);
            //     suitUIClass.lb_num.text = curnum >= max ? str.BBCode(ColorType.Green) : str.BBCode(ColorType.Red);
            // }
            
        }
    }

    #endregion
    
}

/// <summary>
/// 背包binder类
/// </summary>
public class NostalgiaBagItem : UIBinder
{
    private ILBetterList<UIItemBase> items;
    private NostalgiaBagItemData _data;
    private UIGridContainer gridContainer;
    private int itemCount;
    private CSNostalgiaEquipInfo Info;
    public override void Init(UIEventListener handle)
    {
        Info = CSNostalgiaEquipInfo.Instance;
        items = PoolHandle.GetSystemClass<ILBetterList<UIItemBase>>();
        items.Clear();
        gridContainer = handle.GetComponentInChildren<UIGridContainer>();
        itemCount = CSNostalgiaEquipInfo.Instance.PageMaxNum;
        
    }

    public override void Bind(object data)
    {
        _data = (NostalgiaBagItemData)data;
        int page = _data.page;
        //var itemsinfo = Info.BagList;
        
        //排序
        // ILBetterList<NostalgiaBagClass> list = PoolHandle.GetSystemClass<ILBetterList<NostalgiaBagClass>>();
        // list.Clear();
        // for (var it = itemsinfo.GetEnumerator();it.MoveNext();)
        // {
        //     list.Add(it.Current.Value);
        // }
        //var list =  list.Sort(Info.SortList);
        //var list = Info.GetSortList(NostalgiaSelectType.BAG,false);
        
        
        //获取排序列表
        var list = Info.bagSortList; 
        int count = list.Count;
        gridContainer.MaxCount = itemCount;
        
        for (int i = 0; i < gridContainer.MaxCount; i++)
        {
            int index = itemCount * (page - 1) + i;
            UIItemBase item;
            if (items.Count > i)
                item = items[i];
            else
            {
                var node = UtilityObj.Get<Transform>(gridContainer.controlList[i].transform, "node");
                var itembase = PoolHandle.CreateItemPool
                    (PropItemType.Bag, node, 8);
                var tempitem = itembase.Append();
                items.Add(tempitem);
                item = tempitem;
            }
            
            if (count > index)
            {
                try
                {
                    if (list[index]!= null)
                        item.Refresh(list[index].bagiteminfo,OnBagClick,false);
                }
                catch (Exception e)
                {
                    
                }
            }
            else
                item.UnInit();
        }
        
        //PoolHandle.Recycle(list);
    }

    void OnBagClick(UIItemBase _item)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.HuaijiuBag, _item.itemCfg, _item.infos);
    }
    
    public override void OnDestroy()
    {
        for (int i = 0; i < items.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(items[i]);
        }
        //UIItemManager.Instance.RecycleItemsFormMediator(items);
        items.Clear();
        PoolHandle.Recycle(items);
        items = null;
    }
}

public class NostalgiaBagItemData : IDispose
{
    public int page;
    public void Dispose()
    {
        
    }
}

public class SuitUIClassBinder : UIBinder
{
    public UISprite Sprite;
    public UILabel lb_num;
    public GameObject obj;
    private NostalgiaSuit suitInfo;

    public override void Init(UIEventListener handle)
    {
        var trans = handle.transform;
        Sprite = UtilityObj.Get<UISprite>(trans,"sp_name/Sprite");
        lb_num = UtilityObj.Get<UILabel>(trans,"lb_num");
        obj = handle.gameObject;
        
    }
    public override void Bind(object data)
    {
        suitInfo = data as NostalgiaSuit;
        if (suitInfo == null)
            return;
        int max = suitInfo.maxNum;
        int curnum = suitInfo.Count;
        var Huaijiusuit = suitInfo.Huaijiusuit;
        Sprite.spriteName = Huaijiusuit.pic;
        UIEventListener.Get(obj).onClick = OnClickSuit;
        string str = CSString.Format(1952, curnum, max);
        lb_num.text = curnum >= max ? str.BBCode(ColorType.Green) : str.BBCode(ColorType.Red);
    }

    private void OnClickSuit(GameObject obj)
    {
        //var suit = UIEventListener.Get(obj).parameter as NostalgiaSuit;
        if (suitInfo != null)
        {
            UIManager.Instance.CreatePanel<UINostalgiaSuitTipPanel>((f) =>
            {
                (f as UINostalgiaSuitTipPanel).OpenPanel(suitInfo);
            });
        }
    }
    
    public override void OnDestroy()
    {
        Sprite = null;
        lb_num = null;
        obj = null;
    }
}

public class equipItemClassBinder : UIBinder
{
    public Transform mbg;
    public Transform btn_add;
    private UIItemBase item;
    public int _index;
    private CSNostalgiaEquipInfo Info;
    private Transform effect;
    private Transform weareffect;
    private Transform addeffect;
    private bool isFirst;
    private Transform trans;
    public override void Init(UIEventListener handle/*Transform trans,int index,UIItemBase item*/)
    {
        var trans = handle.transform;
        isFirst = false;
        Info = CSNostalgiaEquipInfo.Instance;
        //_index = index;
        mbg = UtilityObj.Get<Transform>(trans,"bg");
        btn_add = UtilityObj.Get<Transform>(trans,"btn_add");
        effect = UtilityObj.Get<Transform>(trans,"effect");
        weareffect = UtilityObj.Get<Transform>(trans, "Weareffect");
        addeffect = UtilityObj.Get<Transform>(trans, "addeffect");     
        if (item == null)
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, trans);
        //item.obj_trans.SetParent(trans);
        //item.obj_trans.localPosition = Vector3.zero;
        //this.item = item;
        UIEventListener.Get(btn_add.gameObject).onClick = OnAutoWear;
        CSEffectPlayMgr.Instance.ShowUIEffect(addeffect.gameObject, 17086);
    }

    public override void Bind(object data)
    {
        var mData = data as NostalgiaBagClass;
        
        //设置背景图
        mbg.gameObject.SetActive(_index % 4 == 0);
        btn_add.gameObject.SetActive(false);
        addeffect.gameObject.SetActive(false);
        var equiplist = Info.EquipList;
        effect.gameObject.SetActive(false);
        
        if (equiplist.Count > _index)
        {
            var bagClass = mData;
            if (null == mData)
                return;
            
            int configid = bagClass.bagiteminfo.configId;
            item.Refresh(bagClass.bagiteminfo,OnClickEquip,false);

            //显示穿戴特效
            if (item.itemCfg == null && bagClass.item != null && isFirst)
                CSEffectPlayMgr.Instance.ShowUIEffect(weareffect.gameObject, 17001);
            //显示红点
            if (Info.bagStackList.ContainsKey(configid))
                item.ShowRedPoint(Info.bagStackList[configid].Count >= Info.NostalgialevelUpNum && bagClass.Huaijiusuit.nextID != 0);
            //显示套装特效
            int type = bagClass.Huaijiusuit.type;
            int effectid = 17830 + type;
            if (Info.suitInfos.ContainsKey(type))
            {
                var equip = Info.suitInfos[type].equips;
                if (equip.ContainsKey(bagClass.item.subType) && equip.Count >= 4)
                {
                    long id = equip[bagClass.item.subType].bagiteminfo.id;
                    if (id == bagClass.bagiteminfo.id)
                    {
                        CSEffectPlayMgr.Instance.ShowUIEffect(effect.gameObject, effectid);
                        effect.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            item.UnInit();
            if (_index + 1 > Info.GetziIndex)
                item.ShowHuaiJiuLock(_index + 1,OnClickUnLock);
            else
            {
                if (Info.BagList.Count > 0)
                {
                    btn_add.gameObject.SetActive(true);
                    addeffect.gameObject.SetActive(true);
                }
            }
        }
        isFirst = true;
    }

    public override void OnDestroy()
    {
        if (item != null)
            UIItemManager.Instance.RecycleSingleItem(item);
        mbg = null;
        btn_add = null;
        Info = null;
        CSEffectPlayMgr.Instance.Recycle(addeffect.gameObject);
        CSEffectPlayMgr.Instance.Recycle(weareffect.gameObject);
        CSEffectPlayMgr.Instance.Recycle(effect.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list">list为排序后的装备列表</param>
    // public void Refresh(ILBetterList<NostalgiaBagClass> list)
    // {
    //     btn_add.gameObject.SetActive(false);
    //     addeffect.gameObject.SetActive(false);
    //     var equiplist = Info.EquipList;
    //     effect.gameObject.SetActive(false);
    //     if (equiplist.Count > _index)
    //     {
    //         var bagClass = list[_index];
    //         int configid = bagClass.bagiteminfo.configId;
    //         
    //         //显示穿戴特效
    //         if (item.itemCfg == null && bagClass.item != null && isFirst)
    //         {
    //             CSEffectPlayMgr.Instance.ShowUIEffect(weareffect.gameObject, 17001);
    //             //effect.gameObject.SetActive(true);
    //         }
    //         item.Refresh(bagClass.bagiteminfo,OnClickEquip,false);
    //         
    //         //显示红点
    //         if (Info.bagStackList.ContainsKey(configid))
    //         {
    //             item.ShowRedPoint(Info.bagStackList[configid].Count >= Info.NostalgialevelUpNum);
    //         }
    //         //显示套装特效
    //         int type = bagClass.Huaijiusuit.type;
    //         int effectid = 0;
    //         
    //         switch (type)
    //         {
    //            case 1 :
    //                effectid = 17831;
    //                break;
    //            case 2 :
    //                effectid = 17832;
    //                break;
    //            case 3 :
    //                effectid = 17833;
    //                break;
    //            default:
    //                effectid = 17831;
    //                break;
    //         }
    //         if (Info.suitInfos.ContainsKey(type))
    //         {
    //             var equip = Info.suitInfos[type].equips;
    //             if (equip.ContainsKey(bagClass.item.subType) && equip.Count >= 4)
    //             {
    //                 long id = equip[bagClass.item.subType].bagiteminfo.id;
    //                 if (id == bagClass.bagiteminfo.id)
    //                 {
    //                     CSEffectPlayMgr.Instance.ShowUIEffect(effect.gameObject, effectid);
    //                     effect.gameObject.SetActive(true);
    //                 }
    //             }
    //         }
    //     }
    //     else
    //     {
    //         item.UnInit();
    //         if (_index + 1 > Info.GetziIndex)
    //             item.ShowHuaiJiuLock(_index + 1,OnClickUnLock);
    //         else
    //         {
    //             if (Info.BagList.Count > 0)
    //             {
    //                 btn_add.gameObject.SetActive(true);
    //                 addeffect.gameObject.SetActive(true);
    //             }
    //         }
    //     }
    //
    //     isFirst = true;
    // }

   
    void OnClickUnLock(GameObject obj)
    {
        int lockNum = (int)UIEventListener.Get(obj).parameter;
        //FNDebug.Log("OnClickUnLock");
        int index = Info.GetziIndex+1; 
        if (index == lockNum )
        {
            var solts = Info.GetSlots();
            HUAIJIUSLOT slot;
            if (solts.Count >= index)
            {
                slot = solts[index - 1];
                if (slot.param.Count > 0)
                {
                    var param = slot.param[0];
                    UICostPromptPanel.Open(392, param.key(), param.value(), () =>
                    {
                        if (param.key().GetItemCount() < param.value())
                        {
                            Utility.ShowGetWay(param.key());
                            return false;
                        }
                        Net.ReqUnlockMemoryEquipGeziMessage();
                        return true;
                    });
                }
                else
                {
                    Net.ReqUnlockMemoryEquipGeziMessage();
                }
            }
        }

        if (lockNum > index)
        {
            UtilityTips.ShowRedTips(1970);
        }
        //_item.lockNum = 1;
    }
    
    void OnClickEquip(UIItemBase _item)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.HuaijiuEquip, _item.itemCfg, _item.infos);
    }
    
    private void OnAutoWear(GameObject obj)
    {
        if (Info.BagList.Count > 0)
        {
            //获取背包中的第一个
            for (var it = Info.BagList.GetEnumerator(); it.MoveNext();)
            {
                long id = AutoSelect();
                Net.ReqPutOnMemoryEquipMessage(id,0);
                break;
            }
        }
    }

    //private List<int> findidList = new List<int>();
    
    private long AutoSelect()
    {
        // 如果装备快满,优先获取
        NostalgiaBagClass bagClass = null;
        for (var it = Info.suitInfos.GetEnumerator(); it.MoveNext();)
        {
            if (bagClass != null)
                break;
            var v = it.Current.Value;
            if (v.equips.Count == 3)
                bagClass = GetBagClass(v);
        }

        if (bagClass != null)
            return bagClass.bagiteminfo.id;
        //等于 0 1 或者等于2的情况
        for (var it = Info.suitInfos.GetEnumerator(); it.MoveNext();)
        {
            if (bagClass != null)
                break;
            var v = it.Current.Value;
            if (v.equips.Count == 1 || v.equips.Count == 2 || v.equips.Count == 0)
                bagClass = GetBagClass(v);
            
        }
        
        if (bagClass != null)
            return bagClass.bagiteminfo.id;
        else{
            //返回第一种最高阶的
            if (Info.TopsuitsubTypeData.Count >0)
            {
                for (var it = Info.TopsuitsubTypeData.GetEnumerator(); it.MoveNext();)
                {
                    var v = it.Current.Value;
                    for (int i = 0; i <v.Count; i++)
                    {
                        if (bagClass == null)
                            bagClass = v[i];
                        else
                        {
                            if (bagClass.Huaijiusuit.rank < v[i].Huaijiusuit.rank)
                                bagClass = v[i];
                            
                        }

                    }

                    if (bagClass != null)
                        return bagClass.bagiteminfo.id;
                }
            }
           
        }

        //等于 4的情况
        // for (var it = Info.suitInfos.GetEnumerator(); it.MoveNext();)
        // {
        //     if (bagClass != null)
        //         break;
        //     var v = it.Current.Value;
        //     if (v.equips.Count == 4)
        //     {
        //     }
        //     //bagClass = GetBagClass(v);
        //         
        //         
        // }

        return 0;
    }

    private NostalgiaBagClass GetBagClass(NostalgiaSuit v)
    {
        NostalgiaBagClass bagClass = null;
        //获取缺少的subType
        int subType = Info.GetTopRemoveSubType(v);
        //查找是否有符合条件的id
        int suitsubTypeid = Info.GetsuitsubTypeid(v.Huaijiusuit.type,subType);
        if (Info.TopsuitsubTypeData.ContainsKey(suitsubTypeid))
        {
            var topData = Info.TopsuitsubTypeData[suitsubTypeid];
            for (int i = 0, max = topData.Count; i < max; i++)
            {
                if (bagClass == null)
                    bagClass = topData[i];
                else
                {
                    if (bagClass.Huaijiusuit.rank < topData[i].Huaijiusuit.rank)
                        bagClass = topData[i];
                }
            }
        }

        return bagClass;
    }


}
