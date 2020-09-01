using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bag;
using Google.Protobuf.Collections;
using TABLE;

public partial class UINostalgiaUpLevelPanel : UIBasePanel
{

    //EndLessKeepHandleList<NostalgiaUpLevelItem, NostalgiaBase> endLessList;
    //EndLessKeepHandleList<NostalgiaUpLevelItem, NostalgiaBase> endLessequipList;
    private NostalgiaSelectType CurType;
    private CSNostalgiaEquipInfo Info;
    private ILBetterList<NostalgiaUpLevelCenter> ceters;
    private UIGridContainer curgrid;
    private NostalgiaBase curBagClass;
    private ILBetterList<NostalgiaBagClass> CurSortList = new ILBetterList<NostalgiaBagClass>();
    //private ILBetterList<NostalgiaBagClass> equipSortList = new ILBetterList<NostalgiaBagClass>();
    
    public override void Init()
    {
        base.Init();
        Info = CSNostalgiaEquipInfo.Instance;

        ceters = mPoolHandleManager.GetSystemClass<ILBetterList<NostalgiaUpLevelCenter>>();
        ceters.Clear();
        UIEventListener.Get(mbtn_equip).onClick = OnChangeMode;
        UIEventListener.Get(mbtn_bag).onClick = OnChangeMode;
        UIEventListener.Get(mbtn_up).onClick = OnUpClick;
        UIEventListener.Get(mstar).onClick = OnShowSuit;
        UIEventListener.Get(mlb_empty).onClick = ShowTip;
        
        mClientEvent.AddEvent(CEvent.NostalgiaChoose, OnChooseChange);
        mClientEvent.AddEvent(CEvent.NostalgiaSelectItem, OnAddItem);
        mClientEvent.AddEvent(CEvent.NostalgiaBagChange, RefreshUI);
        mClientEvent.AddEvent(CEvent.NostalgiaEquipChange, RefreshUI);
        mClientEvent.AddEvent(CEvent.NostalgiaRefreshHint, RefreshHint);
        CSEffectPlayMgr.Instance.ShowUITexture(mnostalgia_bg, "nostalgia_bg");

        Info.curClickIndex = 0;
        
    }

    public override void Show()
    {
        base.Show();
    }
    
    #region 点击事件
    
    /// <summary>
    /// 切换模式
    /// </summary>
    /// <param name="obj"></param>
    private void OnChangeMode(GameObject obj = null)
    {
        if (obj == mbtn_equip)
            CurType = NostalgiaSelectType.EQUIP;
        if (obj == mbtn_bag)
            CurType = NostalgiaSelectType.BAG;
        SortList();
        RefreshItemList();
    }

    private void ShowTip(GameObject obj)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1063));
    }

    private void OnShowSuit(GameObject obj)
    {
        int type = curBagClass.Huaijiusuit.type;
        if (Info.suitInfos.ContainsKey(type))
        {
            var suit = Info.suitInfos[type];
            if (suit != null)
            {
                UIManager.Instance.CreatePanel<UINostalgiaSuitTipPanel>((f) =>
                {
                    (f as UINostalgiaSuitTipPanel).OpenPanel(suit);
                });
            }
        }
    }

    private void OnUpClick(GameObject obj)
    {
        var curSelectlist = Info.curSelectlist;
        if (curSelectlist.Count == 0)
            return;
        if (curSelectlist.Count < 3)
        {
            //判断背包内是否有满足条件的装备
            int itemid = curSelectlist[0].item.id;
            if (Info.bagStackList.ContainsKey(itemid))
            {
                var data = Info.bagStackList[itemid];
                int num = data.Count;
                if (!data.Contains(curBagClass as NostalgiaBagClass))
                    num++;
                if (num >= 3)
                {
                    UtilityTips.ShowPromptWordTips(96, () =>
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (!curSelectlist.Contains(data[i]))
                                curSelectlist.Add(data[i]);
                            if (curSelectlist.Count >= 3)
                                break;
                        }

                        SendNet();
                    });
                    return;
                }

            }

            if (curSelectlist.Count < 3)
            {
                Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1063));
                return;
            }
        }

        SendNet();

    }
    private void SendNet()
    {
        var curSelectlist = Info.curSelectlist;
        if (curSelectlist.Count < 3)
            return;
        RepeatedField<long> para = mPoolHandleManager.GetSystemClass<RepeatedField<long>>();
        para.Clear();
        para.Add(curSelectlist[1].bagiteminfo.id);
        para.Add(curSelectlist[2].bagiteminfo.id);
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17087);
        
        Net.ReqMemoryEquipLevelUpMessage(curSelectlist[0].bagiteminfo.id, para);
        curSelectlist.Clear();
        mPoolHandleManager.Recycle(para);
    }
    
    
    #endregion

    #region 消息回调
    private void RefreshHint(uint uievtid, object data)
    {
        ShowHint();
    }

    private void RefreshUI(uint uievtid, object data)
    {
        long nostalid = 0;
        SortList();
        for (int i = 0; i < CurSortList.Count; i++)
        {
            int num = 0;
            var id = CurSortList[i].item.id;
            if (Info.bagStackList.ContainsKey(id))
            {
                for (int j = 0; j < Info.bagStackList[id].Count; j++)
                {
                    if ( Info.bagStackList[id][j] ==  CurSortList[i])
                        continue;
                    num++;
                    if (num>= Info.NostalgialevelUpNum)
                    {
                        nostalid = CurSortList[i].bagiteminfo.id;
                        break;
                    }
                }
            }
            if (nostalid != 0)
                break;
        }
        RefreshItemList(nostalid);
    }

    private void OnAddItem(uint uievtid, object data)
    {
        NostalgiaBagClass bagClass = data as NostalgiaBagClass;
        
        var gp = mItemList.transform.GetChild(Info.curClickIndex)?.gameObject;
        if (gp == null)
            return;
        
        var eventHandle = UIEventListener.Get(gp);
        var Binder = eventHandle.parameter as NostalgiaUpLevelCenter;
        //Binder.Bind(Info.curClickIndex);
        Binder.AddItem(bagClass);

        Info.curSelectlist.Add(bagClass);
        ShowHint();
    }
    
    private void OnChooseChange(uint uievtid = 0, object data = null)
    {
        RefreshRight((long)data);
    }

    #endregion
    
    
    private void RefreshRight(long nostalid)
    {
        
        NostalgiaUpLevelCenter Binder;

        var list = NostalgiaSelectType.EQUIP == CurType ? Info.EquipList : Info.BagList;
        //如果传入空值 ,那么认为当前列表没有怀旧装备
        var curSelectlist = Info.curSelectlist;
        //刷新 中间格子和套装
        curSelectlist.Clear();
        
        bool isShow = list.ContainsKey(nostalid);
        mstar.SetActive(isShow);
        if (isShow)
        {
            curBagClass = list[nostalid];
            
            int type = curBagClass.Huaijiusuit.type;
            bool IsShow = Info.suitInfos.ContainsKey(type);
            if (IsShow)
                ShowSuit(Info.suitInfos[type]);
            curSelectlist.Add(curBagClass);
            int id = curBagClass.item.id;
            if (Info.bagStackList.ContainsKey(id))
            {
                var bagStackList = Info.bagStackList[id];
                for (int j = 0; j < bagStackList.Count; j++)
                {
                    if (curSelectlist.Count >= 3)
                        break;
                    if (bagStackList[j] != curBagClass)
                        curSelectlist.Add(bagStackList[j]);
                }
            }
        }
    
        //刷新列表
        for (int i = 0; i < 3; i++)
        {
            var gp = mItemList.transform.GetChild(i).gameObject;
            var eventHandle = UIEventListener.Get(gp);
            
            if (eventHandle.parameter == null)
            {
                Binder = new NostalgiaUpLevelCenter();
                Binder.Setup(eventHandle);
            }
            else
                Binder = eventHandle.parameter as NostalgiaUpLevelCenter;
            Binder.Bind(i);
        }
        ShowHint();
     
       
        //刷新下方hint 
        
    }

    private void ShowSuit(NostalgiaSuit suit)
    {
        int num = suit.equips.Count;
        int max = suit.maxNum;
        string temp = CSString.Format(1042, num, max);
        string strnum = num >= max ? temp.BBCode(ColorType.Green) : temp.BBCode(ColorType.Red);
        string name = suit.Huaijiusuit.name.BBCode(ColorType.Yellow);
        mlb_name.text = $"{name}{strnum}";
        int type = suit.Huaijiusuit.type;
        msp_suit.spriteName = $"nostalgia_btn{type}";
    }

    private void ChangType(NostalgiaSelectType type)
    {
        CurType = type;
        if (type == NostalgiaSelectType.BAG)
            mbtn_bag.GetComponent<UIToggle>().value = true;
        else
            mbtn_equip.GetComponent<UIToggle>().value = true;
    }

    public void OpenPanel(long id = 0)
    {
        if (Info.EquipList.ContainsKey(id))
            ChangType(NostalgiaSelectType.EQUIP);
        else if (Info.BagList.ContainsKey(id))
            ChangType(NostalgiaSelectType.BAG);
        SortList();
        RefreshItemList(id);
    }


    protected override void OnDestroy()
    {
        for (int i = 0; i < ceters.Count; i++)
        {
            mPoolHandleManager.Recycle(ceters[i]);
        }

        mPoolHandleManager.Recycle(ceters);
        ceters = null;
        mwrap_equip.UnBind<NostalgiaUpLevelItem>();
        mwrap_page.UnBind<NostalgiaUpLevelItem>();
        CSEffectPlayMgr.Instance.Recycle(meffect.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mnostalgia_bg);
        // endLessList?.Clear();
        // endLessequipList?.Clear();
        // endLessequipList = null;
        // endLessList = null;
        base.OnDestroy();
    }

    public void RefreshItemList(long nostalid = 0)
    {
        //var List = mPoolHandleManager.GetSystemClass<ILBetterList<NostalgiaBase>>();
        curgrid = CurType == NostalgiaSelectType.EQUIP ? mwrap_equip : mwrap_page;
        int curIndex = 0;
        
        for (int i = 0; i < CurSortList.Count; i++)
        {
            if (nostalid == CurSortList[i].bagiteminfo.id)
                curIndex = i;
        }
        curgrid.MaxCount = CurSortList.Count;
        curgrid.Bind<NostalgiaBagClass,NostalgiaUpLevelItem>(CurSortList,mPoolHandleManager);
        bool isShowList = CurSortList.Count != 0;
        //bool isShowList = endless.Count != 0;
        mEquipList.gameObject.SetActive(isShowList &&CurType == NostalgiaSelectType.EQUIP);
        mBagEquip.gameObject.SetActive(isShowList &&CurType == NostalgiaSelectType.BAG);
        mmoreEquip.SetActive(!isShowList);
        if (isShowList)
        {
            
            if (nostalid == 0 && CurSortList.Count > 0)
                nostalid = CurSortList[0].bagiteminfo.id;
            
            // for (int i = 0; i < curgrid.MaxCount; i++)
            // {
            //     UIEventListener.Get(curgrid.controlList[i]).onClick = OnClickSelect;
            // }
            //endless.Bind(curIndex,6);
            //endless.Bind();
            //FNDebug.Log($"发送事件 {nostalgiaBase.bagiteminfo.id}");
            ScriptBinder.Invoke(0.02f, () =>
            {
                mClientEvent.SendEvent(CEvent.NostalgiaChoose, nostalid);
                mscollbar.value = (float)curIndex / (CurSortList.Count - 5.2f);
            });
        }
        else
            RefreshRight(0);
        
    }

    private void SortList()
    {
        CurSortList = Info.GetSortList(CurType);
        //bagSortList.Clear();
        //equipSortList.Clear();
        // if (CurType == NostalgiaSelectType.EQUIP)
        // {
        //     
        //     // for (var it = Info.EquipList.GetEnumerator(); it.MoveNext();)
        //     // {
        //     //     CurSortList.Add(it.Current.Value);
        //     // }    
        //     // CurSortList.Sort(Info.SortList);
        //
        // }
        // if (CurType == NostalgiaSelectType.BAG)
        // {
        //     for (var it = Info.BagList.GetEnumerator(); it.MoveNext();)
        //     {
        //         CurSortList.Add(it.Current.Value);
        //     }
        // }
    }

    public void ShowHint()
    {
        bool isMax = false;
        if (curBagClass != null)
            isMax = curBagClass.Huaijiusuit.nextID == 0;
        bool isCount = false;
        if (CurType == NostalgiaSelectType.BAG)
            isCount = Info.BagList.Count > 0;
        if (CurType == NostalgiaSelectType.EQUIP)
            isCount = Info.EquipList.Count > 0;

        mlb_hint.gameObject.SetActive(!isMax && isCount);
        mbtn_up.SetActive(!isMax && isCount);
        mlb_max.SetActive(isMax);
        mlb_empty.SetActive(!isCount);

        string str = "";
        if (Info.curSelectlist.Count == 0)
        {
            str = CSString.Format(1981);
        }
        //if (Info.curSelectlist.Count = 2)
        else if (Info.curSelectlist.Count == 3)
        {
            ITEM item;
            int id = Info.curSelectlist[0].item.data;
            if (ItemTableManager.Instance.TryGetValue(id,out item))
            {
                var para = item.name.BBCode(UtilityColor.GetColorTypeByQuality(item.quality));
                
                str = CSString.Format(1982,item.levClass,para);
            }
        }
        else 
        {
            int count = 3 - Info.curSelectlist.Count;
            var item = Info.curSelectlist[0].item;
            string name =item.name.BBCode(UtilityColor.GetColorTypeByQuality(item.quality));
            str = CSString.Format(1983,count,item.levClass,name);
        }
        mlb_hint.text = str;
    }

}

public class NostalgiaUpLevelItem : UIBinder
{
    private NostalgiaBase _data;
    private UILabel name;
    private UILabel levelUp;
    private Transform Item;
    private UIItemBase item;
    private CSNostalgiaEquipInfo Info;
    private Transform choose;
    public override void Init(UIEventListener handle)
    {
        Info = CSNostalgiaEquipInfo.Instance;
        Transform trans = handle.transform;
        name = Get<UILabel>("name", trans);
        levelUp = Get<UILabel>("levelUp", trans);
        Item = Get<Transform>("Item", trans);
        choose = Get<Transform>("choose", trans);
        HotManager.Instance.EventHandler.AddEvent(CEvent.NostalgiaChoose,OnChoose);
        UIEventListener.Get(handle.gameObject).onClick = OnClickBtn;
    }

    private void OnClickBtn(GameObject obj)
    {
        if (_data != null)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaChoose,_data.bagiteminfo.id);
        }
        
    }
    private void OnChoose(uint uievtid, object data)
    {
        if (choose == null)
            return;
        
        if (_data != null)
        {
             long nostid = (long)data;
             choose.gameObject.SetActive(nostid == _data.bagiteminfo.id);
             Info.curSelectid = nostid;
        }
    }

    public override void Bind(object data)
    {
        
        _data = (NostalgiaBase)data;
        int id = _data.item.id;
        choose.gameObject.SetActive(_data.bagiteminfo.id == Info.curSelectid);
        if (item == null)
            item = Utility.GetItemByInfo(id,Item);
        else
            item.Refresh(id);
        
        int quality = _data.item.quality;
        name.text = _data.item.name.BBCode(UtilityColor.GetColorTypeByQuality(quality));
        levelUp.gameObject.SetActive(false);
        if (Info.bagStackList.ContainsKey(id))
        {
            var list = Info.bagStackList[id];
            int num = list.Count;
            for (int i = 0; i < num; i++)
            {
                if (_data == list[i])
                    num--;
            }
            levelUp.gameObject.SetActive(num >= Info.NostalgialevelUpNum && _data.Huaijiusuit.nextID != 0);
        }
    }

    public override void OnDestroy()
    {
        
        UIItemManager.Instance.RecycleSingleItem(item);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.NostalgiaChoose);
        
        _data = null;
        name = null;
        levelUp = null;
        Item = null;
        item = null;
    }
}

public class NostalgiaUpLevelCenter :UIBinder
{
    private Transform btn_add;
    private Transform btn_min;
    private NostalgiaBase _nostalgiaBase;
    private UIItemBase item;
    private Transform _transform;
    public int _geziIndex;
    private CSNostalgiaEquipInfo Info;
    
    public override void Init(UIEventListener handle)
    {
        if (item != null)
            item = null;
        Info = CSNostalgiaEquipInfo.Instance;
        //_geziIndex = geziIndex;
        _transform = handle.transform;
        btn_add = UtilityObj.Get<Transform>(_transform, "btn_add");
        btn_min = UtilityObj.Get<Transform>(_transform, "btn_min");
        UIEventListener.Get(btn_add.gameObject).onClick = onClickAdd;
        UIEventListener.Get(btn_min.gameObject).onClick = onClickMin;
    }

    public override void Bind(object data)
    {
        _geziIndex = (int) data;
        if (item == null) item = UIItemManager.Instance.GetItem(PropItemType.Normal, _transform);
        
        if (Info.curSelectlist.Count>_geziIndex)
        {
            _nostalgiaBase = Info.curSelectlist[_geziIndex];
            bool isShow = _nostalgiaBase != null;
            if (_geziIndex != 0)
            {
                btn_min.gameObject.SetActive(isShow);
                btn_add.gameObject.SetActive(!isShow);
            }
            else
            {
                btn_min.gameObject.SetActive(false);
                btn_add.gameObject.SetActive(false);
            }
           
            if (isShow)
                item.Refresh(_nostalgiaBase.item);
            else
                item.UnInit();
        }
        else
        {
            btn_add.gameObject.SetActive(true);
            btn_min.gameObject.SetActive(false);
            item.UnInit();
        }

    }

    public void AddItem(NostalgiaBagClass nostalgiadata)
    {
        if (nostalgiadata.item != null)
        {
            item.Refresh(nostalgiadata.item);
            btn_min.gameObject.SetActive(true);
            btn_add.gameObject.SetActive(false);
            _nostalgiaBase = nostalgiadata;
        }
    }


    private void onClickAdd(GameObject obj)
    {
        if (Info.curSelectlist.Count > 0)
        {
            Info.curClickIndex = _geziIndex;
            UIManager.Instance.CreatePanel<UINostalgiaMaterialPanel>();
        }
        
    }

    private void onClickMin(GameObject obj)
    {
        if (Info.curSelectlist.Contains(_nostalgiaBase))
            Info.curSelectlist.Remove(_nostalgiaBase); 
        
        _nostalgiaBase = null;
        item.UnInit();
        btn_min.gameObject.SetActive(false);
        btn_add.gameObject.SetActive(true);
        HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaRefreshHint);
        
    }

    public override void OnDestroy()
    {
        UIItemManager.Instance.RecycleSingleItem(item);
        item = null;
        btn_add = null;
        btn_min = null;
        _nostalgiaBase = null;
    }
}
