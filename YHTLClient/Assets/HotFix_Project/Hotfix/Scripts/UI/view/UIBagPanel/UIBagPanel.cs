using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum TipsOpenType
{
    Bag,
    RoleEquip,
    Normal,
    OtherPlayerInfo,
    BagWarehouse, //背包中点击（显示放入）
    WarehouseBag,//仓库中点击（显示取出）
    Bag2Recycle,//背包到回收（显示放入）
    Recycle2Bag,//回收到背包（显示取出）
    GuildWareHouseDonate,//公会仓库捐赠
    GuildWareHouseExchange,//公会仓库兑换
    WildAdventureRewardReceive,//野外探险奖励提取
    //怀旧装备的tip情况
    HuaijiuBag,
    HuaijiuEquip,
    HuaijiuNormal,
}
public enum BagShowType
{
    All,
    Equip,
    Props,
    Medicine,
}

public class UIBagPanel : UIBasePanel
{
    enum panelType
    {
        None = 0,           //默认值
        Bag = 1,            //背包
        Warehouse,      //仓库
        Store,          //商店
        Recycle,        //回收
        Synthesis,      //合成
    }
    enum RecycleMode
    {
        RM_NORMAL = 0,
        RM_NEIGONG = 1,
    }



    #region forms
    //页签按钮
    private GameObject _btn_close;
    private GameObject btn_close { get { return _btn_close ?? (_btn_close = Get("center/event/btn_close").gameObject); } }
    private GameObject _btn_bag;
    private GameObject btn_bag { get { return _btn_bag ?? (_btn_bag = Get("center/event/btn_bag").gameObject); } }
    private GameObject _btn_baghl;
    private GameObject btn_baghl { get { return _btn_baghl ?? (_btn_baghl = Get("Background/Checkmark", btn_bag.transform).gameObject); } }
    private GameObject _btn_storehouse;
    private GameObject btn_storehouse { get { return _btn_storehouse ?? (_btn_storehouse = Get("center/event/btn_storehouse").gameObject); } }
    private GameObject _btn_storehousehl;
    private GameObject btn_storehousehl { get { return _btn_storehousehl ?? (_btn_storehousehl = Get("Background/Checkmark", btn_storehouse.transform).gameObject); } }
    private GameObject _btn_shop;
    private GameObject btn_shop { get { return _btn_shop ?? (_btn_shop = Get("center/event/btn_shop").gameObject); } }
    private GameObject _btn_shophl;
    private GameObject btn_shophl { get { return _btn_shophl ?? (_btn_shophl = Get("Background/Checkmark", btn_shop.transform).gameObject); } }
    private GameObject _btn_combine;
    private GameObject btn_combine { get { return _btn_combine ?? (_btn_combine = Get("center/event/btn_combine").gameObject); } }
    private GameObject _btn_recycle;
    private GameObject btn_recycle { get { return _btn_recycle ?? (_btn_recycle = Get("center/event/btn_recycle").gameObject); } }
    private GameObject _btn_trim;
    private GameObject btn_trim { get { return _btn_trim ?? (_btn_trim = Get("center/event/btn_trim").gameObject); } }

    private UILabel _lb_trim;
    private UILabel lb_trim { get { return _lb_trim ?? (_lb_trim = Get<UILabel>("center/event/btn_trim/Label")); } }

    private UISprite _sp_btnSort;
    private UISprite sp_btnSort { get { return _sp_btnSort ?? (_sp_btnSort = Get<UISprite>("center/event/btn_trim/Background")); } }

    private UILabel _lb_recycle;
    private UILabel lb_recycle { get { return _lb_recycle ?? (_lb_recycle = Get<UILabel>("Background/Label", _btn_recycle.transform)); } }


    //子panel
    private GameObject _obj_rolequip;
    private GameObject obj_roleequip { get { return _obj_rolequip ?? (_obj_rolequip = Get("center/view/UIRoleShowPanel").gameObject); } }
    private GameObject _obj_warehouse;
    private GameObject obj_warehouse { get { return _obj_warehouse ?? (_obj_warehouse = Get("center/view/UIStorehousePanel").gameObject); } }
    private GameObject _obj_store;
    private GameObject obj_store { get { return _obj_store ?? (_obj_store = Get("center/view/UIKeepshopPanel").gameObject); } }
    private GameObject _obj_recycle;
    private GameObject obj_recycle { get { return _obj_recycle ?? (_obj_recycle = Get("center/view/UIRecyclePanel").gameObject); } }
    //bag
    private UILabel _lb_page;
    private UILabel lb_page { get { return _lb_page ?? (_lb_page = Get<UILabel>("center/view/lb_page")); } }
    private UIScrollView _mscroll;
    private UIScrollView mscroll { get { return _mscroll ?? (_mscroll = Get<UIScrollView>("center/bagitems/Scroll View")); } }
    private GameObject _obj_mask;
    private GameObject obj_mask { get { return _obj_mask ?? (_obj_mask = Get("center/bagitems/maskPanel").gameObject); } }
    private BagWrapContent _wrap;
    private BagWrapContent wrap { get { return _wrap ?? (_wrap = Get<BagWrapContent>("center/bagitems/Scroll View/UIGrid")); } }

    private GameObject _btn_All;
    private GameObject btn_All { get { return _btn_All ?? (_btn_All = Get("center/event/btn_allItem").gameObject); } }
    private GameObject _btn_Equip;
    private GameObject btn_Equip { get { return _btn_Equip ?? (_btn_Equip = Get("center/event/btn_onlyEquip").gameObject); } }
    private GameObject _btn_EquipRed;
    private GameObject btn_EquipRed { get { return _btn_EquipRed ?? (_btn_EquipRed = Get("center/event/btn_onlyEquip/redpoint").gameObject); } }
    private GameObject _btn_Props;
    private GameObject btn_Props { get { return _btn_Props ?? (_btn_Props = Get("center/event/btn_onlyItem").gameObject); } }
    private GameObject _btn_PropsRed;
    private GameObject btn_PropsRed { get { return _btn_PropsRed ?? (_btn_PropsRed = Get("center/event/btn_onlyItem/redpoint").gameObject); } }
    private GameObject _btn_Medicine;
    private GameObject btn_Medicine { get { return _btn_Medicine ?? (_btn_Medicine = Get("center/event/btn_onlyDrug").gameObject); } }

    private UIToggle _toggle_All;
    private UIToggle toggle_All { get { return _toggle_All ?? (_toggle_All = Get<UIToggle>("center/event/btn_allItem")); } }

    private UIToggle _toggle_Equip;
    private UIToggle toggle_Equip { get { return _toggle_Equip ?? (_toggle_Equip = Get<UIToggle>("center/event/btn_onlyEquip")); } }
    private UIToggle _toggle_Item;
    private UIToggle toggle_Item { get { return _toggle_Item ?? (_toggle_Item = Get<UIToggle>("center/event/btn_onlyItem")); } }
    private UIToggle _toggle_Drug;
    private UIToggle toggle_Drug { get { return _toggle_Drug ?? (_toggle_Drug = Get<UIToggle>("center/event/btn_onlyDrug")); } }

    private Transform _red_recyle;
    private Transform red_recyle { get { return _red_recyle ?? (_red_recyle = Get<Transform>("center/event/btn_recycle/redpoint")); } }
    #endregion

    #region variable
    UIRoleEquipPanel roleequip;
    UIStorehousePanel warehouse;
    UIBagStorePanel store;
    UIEquipRecyclePanel recycle;
    panelType lastPanelType = panelType.None;
    BagShowType lastBagShowType;
    private bool _is_recycle_open = false;
    private bool IsRecycleOpen
    {
        get
        {
            return _is_recycle_open;
        }
        set
        {
            _is_recycle_open = value;
            if (null != lb_recycle)
            {
                lb_recycle.text = value ? CONST_RECYCLE_RETURN : CONST_RECYCLE_NORMAL;
            }
        }
    }
    private panelType _curpanelType;
    private panelType curpanelType
    {
        get
        {
            return _curpanelType;
        }
        set
        {
            if (_curpanelType != value)
            {
                _curpanelType = value;
                if (_curpanelType != panelType.Recycle)
                {
                    CSItemRecycleInfo.Instance.recycleMode = CSItemRecycleInfo.RecycleMode.RM_NONE;
                }
            }
        }
    }
    private BagShowType _curShowType;
    private BagShowType curShowType
    {
        get
        {
            return _curShowType;
        }
        set
        {
            if (_curShowType != value)
            {
                _curShowType = value;
            }
        }
    }
    private GameObject curChild;
    private GameObject curTab;
    private int curPageNum = 1;
    Dictionary<GameObject, BagGridData> itemsDic;
    protected string CONST_RECYCLE_NORMAL;
    protected string CONST_RECYCLE_RETURN;
    int recycOpenVipLv = 0;
    int warehouseVipLv = 0;
    #endregion
    public override void Init()
    {
        CONST_RECYCLE_NORMAL = ClientTipsTableManager.Instance.GetClientTipsContext(1630);
        CONST_RECYCLE_RETURN = ClientTipsTableManager.Instance.GetClientTipsContext(1629);

        //监听消息  1.货币变动 2.道具增删改   3.整理背包   
        base.Init();
        mClientEvent.Reg((uint)CEvent.BagItemsDrag, GetItemsDrag);
        //mClientEvent.Reg((uint)CEvent.ItemChange, GetItemsChange);
        mClientEvent.Reg((uint)CEvent.ItemListChange, GetItemListChange);
        mClientEvent.Reg((uint)CEvent.BagSort, GetSort);
        mClientEvent.Reg((uint)CEvent.ChangeEquipShow, GetChangeEquipShow);
        mClientEvent.Reg((uint)CEvent.EquipRebuildNtfMessage, EquipChange);
        mClientEvent.Reg((uint)CEvent.BagMaxCountChange, GetBagMaxCountChange);
        mClientEvent.Reg((uint)CEvent.WearEquip, GetBagMaxCountChange);
        mClientEvent.Reg((uint)CEvent.UnWeatEquip, GetBagMaxCountChange);
        mClientEvent.AddEvent(CEvent.BagBoxItemClick, GetItemsChange);
        UIEventListener.Get(btn_close).onClick = CloseBtnClick;
        UIEventListener.Get(btn_bag, panelType.Bag).onClick = ClickEvent;
        UIEventListener.Get(btn_storehouse, panelType.Warehouse).onClick = ClickEvent;
        UIEventListener.Get(btn_shop, panelType.Store).onClick = ClickEvent;
        UIEventListener.Get(btn_combine, panelType.Synthesis).onClick = ClickEvent;
        UIEventListener.Get(btn_recycle, panelType.Recycle).onClick = ClickEvent;
        UIEventListener.Get(btn_All, BagShowType.All).onClick = BagShowTypeChange;
        UIEventListener.Get(btn_Equip, BagShowType.Equip).onClick = BagShowTypeChange;
        UIEventListener.Get(btn_Props, BagShowType.Props).onClick = BagShowTypeChange;
        UIEventListener.Get(btn_Medicine, BagShowType.Medicine).onClick = BagShowTypeChange;
        UIEventListener.Get(btn_trim).onClick = SortBtnClick;
        SetMoneyIds(1, 4);
        IsRecycleOpen = false;
        btn_baghl.SetActive(false);
        btn_storehousehl.SetActive(false);
        btn_shophl.SetActive(false);
        mClientEvent.AddEvent(CEvent.OnRecycleModeChanged, OnRecycleModeChanged);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1137), out recycOpenVipLv);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1136), out warehouseVipLv);
    }

    protected void OnRecycleModeChanged(uint id, object argv)
    {
        if (null != updateObj && realIndexDic.ContainsKey(updateObj) && curShowType == BagShowType.Equip)
        {
            var it = realIndexDic.GetEnumerator();
            while (it.MoveNext())
            {
                int idx = it.Current.Value;
                if (!itemsDic.ContainsKey(it.Current.Key))
                {
                    continue;
                }
                var bagGridData = itemsDic[it.Current.Key];
                CSBagInfo.Instance.GetBagItemInfosByPageNum(idx, curShowType, bagGridData.infoList);
                bagGridData.SetInfos(idx, curShowType);
            }
        }
    }

    protected void setDefaultBagShowType(BagShowType eType)
    {
        this.curShowType = eType;
        switch (eType)
        {
            case BagShowType.All:
                toggle_All.value = true;
                break;
            case BagShowType.Equip:
                toggle_Equip.value = true;
                break;
            case BagShowType.Medicine:
                toggle_Drug.value = true;
                break;
            case BagShowType.Props:
                toggle_Item.value = true;
                break;
        }
        OnShowTypeChanged();
    }
    public override void Show()
    {
        base.Show();
        InitGrid();
        InitWrap();

        ChangePanelState(panelType.Bag);
        setDefaultBagShowType(BagShowType.All);
        btn_PropsRed.SetActive(CSBagInfo.Instance.GetBagBoxRedPointState());
        btn_EquipRed.SetActive(CSBagInfo.Instance.GetBagEquipRedPointState());
        RegisterRed(red_recyle.gameObject, RedPointType.PetLevelUp);

        InitSortTime();
    }

    /// <summary>
    /// 初始化整理按钮
    /// </summary>
    void InitSortTime()
    {
        if (curShowType != BagShowType.All)
        {
            return;
        }
        if (CSBagInfo.Instance.Sec > 0 && CSBagInfo.Instance.Sec < 10)
        {
            lb_trim.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1309), CSBagInfo.Instance.Sec);
            sp_btnSort.spriteName = "btn_samll3";
            btn_trim.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnRecycleModeChanged, OnRecycleModeChanged);
        if (itemsDic != null)
        {
            var iter = itemsDic.GetEnumerator();
            while (iter.MoveNext())
            {
                UIItemManager.Instance.RecycleItemsFormMediator(iter.Current.Value.itemList);
            }
        }
        curChild = null;
        curTab = null;
        if (roleequip != null) { roleequip.Destroy(); roleequip = null; }
        if (warehouse != null) { warehouse.Destroy(); warehouse = null; }
        if (store != null) { store.Destroy(); store = null; }
        if (recycle != null) { recycle.OnDestroy(); recycle = null; }
        base.OnDestroy();
    }
    void InitGrid()
    {
        if (itemsDic == null)
        {
            itemsDic = new Dictionary<GameObject, BagGridData>();
            for (int i = 0; i < wrap.transform.childCount; i++)
            {
                GameObject gridObj = wrap.transform.GetChild(i).Find("UIGridItem").gameObject;
                itemsDic.Add(wrap.transform.GetChild(i).gameObject, new BagGridData(gridObj, ItemClick, DoubleItemClick));
            }
        }
    }
    bool warehouseOpenMode = true;
    public void SetWarehouseOpenMode(bool _judgeVip)
    {
        warehouseOpenMode = _judgeVip;
    }
    bool openRecycleFromNpc = false;
    public void SetRecycleOpenMode(bool fromNpc)
    {
        openRecycleFromNpc = fromNpc;
    }
    public override void SelectChildPanel(int type = 1)
    {
        ChangePanelState((panelType)type);
    }

    public override void SelectChildPanel(int type, int subType)
    {

    }
    void InitWrap()
    {
        wrap.onInitializeItem = OnUpdateItem;
        wrap.maxIndex = 4;
        wrap.minIndex = 0;
    }
    void GetBagInfo()
    {
        OnUpdateItem(wrap.transform.GetChild(0).gameObject, 1);
        OnUpdateItem(wrap.transform.GetChild(1).gameObject, 2);
        mscroll.ResetPosition();
    }
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }
    //页签点击
    void ClickEvent(GameObject _go)
    {
        panelType temp_type = (panelType)UIEventListener.Get(_go).parameter;
        if (temp_type == panelType.Warehouse)
        {
            if (warehouseOpenMode && CSMainPlayerInfo.Instance.VipLevel < warehouseVipLv)
            {
                ChooseWarehouseNpc();
                TABLE.NPC npcCfg = warehouseNpcInfo;
                if (Utility.IsNearPlayerInMap(npcCfg.sceneId, npcCfg.bornX, npcCfg.bornY, 9))
                {
                    UtilityPath.FindPath(new CSMisc.Dot2(npcCfg.bornX, npcCfg.bornY), npcCfg.sceneId, npcCfg.id, () =>
                      {
                          UIManager.Instance.CreatePanel<UIBagPanel>(p =>
                          {
                              (p as UIBagPanel).SetRecycleOpenMode(true);
                              (p as UIBagPanel).SelectChildPanel(2);
                          });
                      });
                }
                else
                {
                    UtilityTips.ShowPromptWordTips(4, TransMit, ToVipPanel);
                }
                return;
            }
        }
        if (temp_type == panelType.Recycle)
        {
            int funcId = 37;
            TABLE.FUNCOPEN funcopenItem;
            if (!FuncOpenTableManager.Instance.TryGetValue(funcId, out funcopenItem))
            {
                return;
            }
            if (CSMainPlayerInfo.Instance.Level < funcopenItem.needLevel)
            {
                UtilityTips.ShowRedTips(106, funcopenItem.needLevel, funcopenItem.functionName);
                return;
            }

            TABLE.NPC npcCfg;
            NpcTableManager.Instance.TryGetValue(1047, out npcCfg);

            if (!openRecycleFromNpc && CSMainPlayerInfo.Instance.VipLevel < recycOpenVipLv)
            {
                if (!Utility.IsNearPlayerInMap(npcCfg.sceneId, npcCfg.bornX, npcCfg.bornY, 9))
                {
                    UtilityTips.ShowPromptWordTips(75, TransMitToOpenRecyclePanel, ToVipPanel);
                    return;
                }

            }
            if (!IsRecycleOpen)
            {
                lastBagShowType = curShowType;
                lastPanelType = curpanelType;
            }
        }
        ChangePanelState(temp_type);
    }
    TABLE.NPC warehouseNpcInfo;
    void ChooseWarehouseNpc()
    {
        TABLE.NPC npcInfo1;
        NpcTableManager.Instance.TryGetValue(414, out npcInfo1);
        TABLE.NPC npcInfo2;
        NpcTableManager.Instance.TryGetValue(415, out npcInfo2);

        if (npcInfo1 == null && npcInfo2 != null)
        {
            warehouseNpcInfo = npcInfo2;
        }
        if (npcInfo1 != null && npcInfo2 == null)
        {
            warehouseNpcInfo = npcInfo1;
        }
        if (npcInfo1 != null && npcInfo2 != null)
        {
            int mapId = CSMainPlayerInfo.Instance.MapID;
            if (mapId == npcInfo1.sceneId)
            {
                warehouseNpcInfo = npcInfo1;
            }
            else if (mapId == npcInfo2.sceneId)
            {
                warehouseNpcInfo = npcInfo2;
            }
            else
            {
                warehouseNpcInfo = npcInfo1;
            }
        }
    }
    void ChangePanelState(panelType eType)
    {
        curpanelType = eType;
        OnChangePanelState();
    }

    //切换子物体状态
    void OnChangePanelState()
    {
        if (curpanelType != panelType.Recycle)
        {
            if (null != recycle && IsRecycleOpen)
            {
                IsRecycleOpen = false;
                recycle.Hide();
                ChangePanelState(curpanelType);
                setDefaultBagShowType(curShowType);
            }
        }
        if (curChild != null)
        {
            curChild.SetActive(false);
        }
        if (curTab != null)
        {
            curTab.SetActive(false);
        }
        switch (curpanelType)
        {
            case panelType.Bag:
                if (roleequip == null)
                {
                    roleequip = new UIRoleEquipPanel();
                    roleequip.UIPrefab = obj_roleequip;
                    roleequip.Init();
                }
                curTab = btn_baghl;
                curChild = roleequip.UIPrefab;
                if (itemsDic != null)
                {
                    var iter = itemsDic.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        for (int i = 0; i < iter.Current.Value.itemList.Count; i++)
                        {
                            iter.Current.Value.itemList[i].ShowSelect(false);
                        }
                    }
                }
                roleequip.Show();
                break;
            case panelType.Warehouse:
                //vip2已以上可以直接打开，否则弹窗
                if (warehouse == null)
                {
                    warehouse = new UIStorehousePanel();
                    warehouse.SetGO(obj_warehouse);
                    warehouse.Init();
                }
                curTab = btn_storehousehl;
                curChild = warehouse.UIPrefab;
                warehouse.Show();
                break;
            case panelType.Store:
                if (store == null)
                {
                    store = new UIBagStorePanel();
                    store.UIPrefab = obj_store;
                    store.Init();
                }
                curTab = btn_shophl;
                curChild = store.UIPrefab;
                store.Show();
                break;
            case panelType.Synthesis:
                break;
            case panelType.Recycle:
                UIManager.Instance.CreatePanel<UIPetLevelUpPanel>((f) =>
                {
                    if (f != null)
                    {
                        (f as UIPetLevelUpPanel).CloseAction = () =>
                        {
                            UIManager.Instance.CreatePanel<UIBagPanel>();

                        };
                    }
                });
                Close();
                break;
        }
        curChild.SetActive(true);
        curTab.SetActive(true);
    }
    void TransMit()
    {
        UtilityPath.FindWithDeliverId(115);
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }
    void TransMitToOpenRecyclePanel()
    {
        UtilityPath.FindWithDeliverId(1047);
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }
    void ToVipPanel()
    {
        UIManager.Instance.CreatePanel<UIVIPPanel>();
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }

    protected void TryCloseRecyclePanel()
    {
        if (curShowType != BagShowType.Equip || curpanelType != panelType.Recycle)
        {
            if (null != recycle && IsRecycleOpen)
            {
                IsRecycleOpen = false;
                recycle.Hide();
                ChangePanelState(curpanelType);
                setDefaultBagShowType(curShowType);
            }
        }
    }

    GameObject updateObj;
    Dictionary<GameObject, int> realIndexDic = new Dictionary<GameObject, int>();
    void OnUpdateItem(GameObject _go, int _realindex)
    {
        updateObj = _go;
        if (realIndexDic.ContainsKey(_go))
        {
            realIndexDic[_go] = _realindex;
        }
        else
        {
            realIndexDic.Add(_go, _realindex);
        }
        //返回的index是 1~5   根据这个拿到背包格子的 25*（_realindex - 1）数据
        CSBagInfo.Instance.GetBagItemInfosByPageNum(_realindex, curShowType, itemsDic[_go].infoList);
        itemsDic[_go].SetInfos(_realindex, curShowType);
    }
    void BagShowTypeChange(GameObject _go)
    {
        BagShowType temp_type = (BagShowType)UIEventListener.Get(_go).parameter;
        curShowType = temp_type;
        OnShowTypeChanged();
    }


    void SortBtnClick(GameObject _go)
    {
        //策划需求所有页签都可以点击整理，并跳转到全部页签
        //if (curShowType != BagShowType.All)
        //{
        //    return;
        //}
        if (CSBagInfo.Instance.CanSort && !Timer.Instance.IsInvoking(CSBagInfo.Instance.SortSchedule))
        {
            CSBagInfo.Instance.SortSchedule = Timer.Instance.InvokeRepeating(0f, 1f, CSBagInfo.Instance.SortCountDown);
            CSBagInfo.Instance.CanSort = false;
            Net.ReqSortItemsMessage();
            sp_btnSort.spriteName = "btn_samll3";
            btn_trim.GetComponent<BoxCollider>().enabled = false;


        }
    }
    public void CountDown()
    {
        if (CSBagInfo.Instance.Sec > 0)
        {
            lb_trim.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1309), CSBagInfo.Instance.Sec);
        }
        else
        {
            sp_btnSort.spriteName = "btn_samll1";
            btn_trim.GetComponent<BoxCollider>().enabled = true;
            lb_trim.text = ClientTipsTableManager.Instance.GetClientTipsContext(1310);
        }
    }

    protected void OnShowTypeChanged()
    {
        if (curShowType != BagShowType.Equip)
        {
            if (null != recycle && IsRecycleOpen)
            {
                IsRecycleOpen = false;
                recycle.Hide();
                if (curpanelType == panelType.Recycle)
                {
                    curpanelType = panelType.Bag;
                    ChangePanelState(curpanelType);
                }
            }
        }
        mscroll.transform.localPosition = new Vector3(0, 35f, 0);
        mscroll.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        curPageNum = 1;
        lb_page.text = "<  " + curPageNum + "/" + "5  >";
        GetBagInfo();
    }

    void GetSort(uint id, object data)
    {
        curShowType = BagShowType.All;
        setDefaultBagShowType(curShowType);
        //OnShowTypeChanged();
    }
    void GetBagMaxCountChange(uint id, object data)
    {
        btn_EquipRed.SetActive(CSBagInfo.Instance.GetBagEquipRedPointState());
        var iter = realIndexDic.GetEnumerator();
        while (iter.MoveNext())
        {
            //Debug.Log($"{iter.Current.Value}   {iter.Current.Key.name}   ");
            CSBagInfo.Instance.GetBagItemInfosByPageNum(iter.Current.Value, curShowType, itemsDic[iter.Current.Key].infoList);
            itemsDic[iter.Current.Key].SetInfos(iter.Current.Value, curShowType);
        }
    }
    void EquipChange(uint id, object data)
    {
        bag.EquipInfo res = ((bag.EquipRebuildNtf)data).equip;
        if (res.position < 0) return;
        bag.BagItemInfo info = res.equip;
        ItemChangeType type = ItemChangeType.NumAdd;
        if (curShowType == BagShowType.All)
        {
            var iter = realIndexDic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value == curPageNum)
                {
                    BagGridData grid = itemsDic[iter.Current.Key];
                    grid.RefreshSingleItem(type, info);
                    return;
                }
            }
        }
        else
        {
            if (itemsDic != null)
            {
                var iter = itemsDic.GetEnumerator();
                while (iter.MoveNext())
                {
                    iter.Current.Value.GetInfo();
                }
            }
        }
    }

    void GetItemListChange(uint id, object data)
    {
        btn_EquipRed.SetActive(CSBagInfo.Instance.GetBagEquipRedPointState());
        var iter = realIndexDic.GetEnumerator();
        while (iter.MoveNext())
        {
            //Debug.Log($"{iter.Current.Value}   {iter.Current.Key.name}   ");
            CSBagInfo.Instance.GetBagItemInfosByPageNum(iter.Current.Value, curShowType, itemsDic[iter.Current.Key].infoList);
            itemsDic[iter.Current.Key].SetInfos(iter.Current.Value, curShowType);
        }
    }

    void GetItemsChange(uint id, object data)
    {
        if (data == null) return;
        btn_PropsRed.SetActive(CSBagInfo.Instance.GetBagBoxRedPointState());
        EventData eventData = (EventData)data;
        bag.BagItemInfo info = (bag.BagItemInfo)eventData.arg1;
        ItemChangeType type = (ItemChangeType)eventData.arg2;
        if (curShowType == BagShowType.All)
        {
            var iter = realIndexDic.GetEnumerator();
            int pageNum = Math.Abs((info.bagIndex - 1) / 25) + 1;
            while (iter.MoveNext())
            {
                //Debug.Log(info.bagIndex + "     " + pageNum + "   " + iter.Current.Value);
                if (iter.Current.Value == pageNum)
                {
                    BagGridData grid = itemsDic[iter.Current.Key];
                    grid.RefreshSingleItem(type, info);
                    return;
                }
            }
        }
        else
        {
            if (itemsDic != null)
            {
                var iter = itemsDic.GetEnumerator();
                while (iter.MoveNext())
                {
                    iter.Current.Value.GetInfo();
                }
            }
        }
    }
    void GetChangeEquipShow(uint id, object data)
    {
        UIRoleEquipPanel.equipType etype = (UIRoleEquipPanel.equipType)data;
        if (etype == UIRoleEquipPanel.equipType.Normal)
        {
            if (roleequip != null)
            {
                roleequip.SetEquipType(UIRoleEquipPanel.equipType.WoLong);
            }
        }
        else if (etype == UIRoleEquipPanel.equipType.WoLong)
        {
            if (roleequip != null)
            {
                roleequip.SetEquipType(UIRoleEquipPanel.equipType.Normal);
            }
        }
    }
    int IsCurrentPage(int _index)
    {
        int result = (int)Math.Ceiling(_index / 25f);
        result = (result < 1) ? 1 : result;
        result = (result > 5) ? 5 : result;
        return result;
    }
    Vector2 vector;
    Vector3 startPos;
    void GetItemsDrag(uint id, object data)
    {
        if (data == null) return;
        ItemBaseDragPara eventData = (ItemBaseDragPara)data;
        if (eventData.mtype != PropItemType.Bag) { return; }
        vector = eventData.mvector;
        startPos = eventData.mstartPos;
        CalScrollPosDragEnd();
    }

    void CalScrollPosDragEnd()
    {
        Vector3 vec = new Vector3(-420 * (curPageNum - 1), 35, 0);
        if (vector.x >= 205)
        {
            vec.x = vec.x + 420;
            curPageNum--;
            curPageNum = (curPageNum < 1) ? 1 : curPageNum;
        }
        else if (vector.x <= -205)
        {
            vec.x = vec.x - 420;
            curPageNum++;
            curPageNum = (curPageNum > 5) ? 5 : curPageNum;
        }
        vec = new Vector3(-420 * (curPageNum - 1), 35, 0);
        obj_mask.SetActive(true);
        SpringPanel.Begin(mscroll.gameObject, vec, 40f).onFinished = ScrollFinish;
    }
    UIPanel mpanel;
    void ScrollFinish()
    {
        obj_mask.SetActive(false);
        vector = Vector2.zero;
        //Debug.Log("结束重置  " + vector);
        lb_page.text = "<  " + curPageNum + "/" + "5  >";
    }
    void ItemClick(UIItemBase _item)
    {
        if (_item != null)
        {
            if (curpanelType == panelType.Recycle)
            {
                if (IsRecycleOpen)
                {
                    if (CSItemRecycleInfo.Instance.recycleMode == CSItemRecycleInfo.RecycleMode.RM_NORMAL)
                    {
                        UITipsManager.Instance.CreateTips(TipsOpenType.Bag2Recycle, _item.itemCfg, _item.infos);
                    }
                    else if (CSItemRecycleInfo.Instance.recycleMode == CSItemRecycleInfo.RecycleMode.RM_NEIGONG)
                    {
                        UITipsManager.Instance.CreateTips(TipsOpenType.Bag2Recycle, _item.itemCfg, _item.infos);
                    }
                    else
                        UITipsManager.Instance.CreateTips(TipsOpenType.Bag, _item.itemCfg, _item.infos);
                }
                else
                {
                    UITipsManager.Instance.CreateTips(TipsOpenType.Bag, _item.itemCfg, _item.infos);
                }
            }
            else if (curpanelType == panelType.Warehouse)
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.BagWarehouse, _item.itemCfg, _item.infos);
            }
            else if (curpanelType == panelType.Bag || curpanelType == panelType.Store)
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.Bag, _item.itemCfg, _item.infos);
            }
        }
    }

    void DoubleItemClick(UIItemBase _item)
    {
        if (curpanelType == panelType.Recycle)
        {
            if (IsRecycleOpen)
            {
                if (null != _item.infos)
                {
                    HotManager.Instance.EventHandler.SendEvent(CEvent.BagItemDBClicked, _item.infos);
                }
            }
        }
        else if (curpanelType == panelType.Warehouse)
        {
            Net.ReqBagToStorehouseMessage(_item.infos.bagIndex);
        }
        else if (curpanelType == panelType.Bag)
        {
            if (_item.itemCfg.type == 2)
            {
                Net.ReqEquipItemMessage(_item.infos.bagIndex, CSBagInfo.Instance.GetEquipWearPos(_item.itemCfg), 0, _item.infos);
            }
            else
            {
                if (ItemOperateTableManager.Instance.GetOperaTypeForBatchUse(_item.itemCfg.type, _item.itemCfg.subType, _item.itemCfg.Operationtype) == 1)
                {
                    CSBagInfo.Instance.UseItem(_item.infos, _item.infos.count, false);
                }
                else
                {
                    CSBagInfo.Instance.UseItem(_item.infos, 1, false);
                }
            }
        }
    }
    public void SelectWeaponZhuFu()
    {
        roleequip.SelectWeapon();
    }
    public void SelectEquipState(int type = 0)
    {
        roleequip.SetEquipType(type);
    }
    public void ReplaceBetterEquip()
    {
        CSItemCountManager.Instance.RepalceBestEquip();
    }
}
public class BagGridData
{
    public GameObject go;
    public List<UIItemBase> itemList = new List<UIItemBase>();
    public Dictionary<long, bag.BagItemInfo> infoList = new Dictionary<long, bag.BagItemInfo>();
    public Action<UIItemBase> onClick;
    PoolHandleManager mPoolHandleManager;
    Action<UIItemBase> onDoubleClick;
    Dictionary<int, bag.BagItemInfo> indexList = new Dictionary<int, bag.BagItemInfo>();
    public int startNum = 0;
    BagShowType curShowType;
    int curpageNum;
    public BagGridData(GameObject obj, Action<UIItemBase> _onClick = null, Action<UIItemBase> _onDoubleClick = null)
    {
        mPoolHandleManager = new PoolHandleManager();
        go = obj;
        onClick = _onClick;
        onDoubleClick = _onDoubleClick;
        itemList = UIItemManager.Instance.GetUIItems(25, PropItemType.Bag, go.transform);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            itemList[i].HasCD = true;
            itemList[i].ListenDrag();
            itemList[i].SetClick();
            itemList[i].SetItemDBClickedCB(onDoubleClick);
            itemList[i].SetMaskJudgeState(true);
        }
        go.GetComponent<UIGrid>().repositionNow = true;
        go.GetComponent<UIGrid>().Reposition();
    }
    public void SetInfos(int _realIndex, BagShowType _curShowType)
    {

        curShowType = _curShowType;
        curpageNum = _realIndex;
        startNum = 25 * (curpageNum - 1) + 1;
        indexList.Clear();
        var iter = infoList.GetEnumerator();
        while (iter.MoveNext())
        {
            indexList.Add(iter.Current.Value.bagIndex, iter.Current.Value);
        }
        //Debug.Log($"{startNum}    {curpageNum}       {infoList.Count}       {indexList.Count} ");
        RefreshItems();

    }
    public void RefreshItems()
    {

        if (curShowType == BagShowType.All)
        {
            int t_key = startNum;
            for (int j = 0; j < itemList.Count; j++)
            {
                //Debug.Log($"{startNum}    {t_key}    {j}    {CSBagInfo.Instance.GetMaxCount()} ");
                if (t_key > CSBagInfo.Instance.GetMaxCount())
                {
                    itemList[j].UnInit();
                    itemList[j].ShowUnlock(t_key);
                }
                else
                {
                    if (indexList.ContainsKey(t_key))
                    {
                        itemList[j].HasCD = true;
                        itemList[j].SetItemDBClickedCB(onDoubleClick);
                        itemList[j].SetMaskJudgeState(true);
                        itemList[j].Refresh(indexList[t_key], onClick);
                    }
                    else
                    {
                        itemList[j].UnInit();
                    }
                }
                t_key++;
            }
        }
        else
        {
            //装备药品其他的显示  
            for (int j = 0; j < itemList.Count; j++)
            {
                itemList[j].UnInit();
            }
            int i = 0;
            var iter = infoList.GetEnumerator();
            while (iter.MoveNext())
            {
                itemList[i].HasCD = true;
                itemList[i].SetItemDBClickedCB(onDoubleClick);
                itemList[i].SetMaskJudgeState(true);
                itemList[i].Refresh(iter.Current.Value, onClick);
                i++;
            }
        }
    }
    public void RefreshSingleItem(ItemChangeType _type, bag.BagItemInfo _singleInfo)
    {
        int pos = (_singleInfo.bagIndex % 25) - 1;
        pos = (pos < 0) ? 24 : pos;
        //Debug.Log(infoList.Count+"  "+ startNum  +"  "+ pos);
        switch (_type)
        {
            case ItemChangeType.Add:
                infoList.Add(_singleInfo.id, _singleInfo);
                itemList[pos].HasCD = true;
                itemList[pos].SetItemDBClickedCB(onDoubleClick);
                itemList[pos].SetMaskJudgeState(true);
                itemList[pos].Refresh(_singleInfo, onClick);
                break;
            case ItemChangeType.Remove:
                infoList.Remove(_singleInfo.id);
                UIEventListener.Get(itemList[pos].obj).parameter = null;
                itemList[pos].UnInit();
                break;
            case ItemChangeType.NumAdd:
                infoList[_singleInfo.id] = _singleInfo;
                itemList[pos].HasCD = true;
                itemList[pos].SetItemDBClickedCB(onDoubleClick);
                itemList[pos].SetMaskJudgeState(true);
                itemList[pos].Refresh(_singleInfo, onClick);
                break;
            case ItemChangeType.NumReduce:
                infoList[_singleInfo.id] = _singleInfo;
                itemList[pos].HasCD = true;
                itemList[pos].SetItemDBClickedCB(onDoubleClick);
                itemList[pos].SetMaskJudgeState(true);
                itemList[pos].Refresh(_singleInfo, onClick);
                break;
        }

    }
    public void GetInfo()
    {
        CSBagInfo.Instance.GetBagItemInfosByPageNum(curpageNum, curShowType, infoList);
        RefreshItems();
    }

}
public class TipClickParams : IDispose
{
    public void Dispose()
    {
        info = null;
        cfg = null;
    }
    public TipsOpenType clickType;
    public bag.BagItemInfo info;
    public TABLE.ITEM cfg;
    public TipClickParams()
    {

    }
    public TipClickParams(TipsOpenType _clickType, bag.BagItemInfo _info, TABLE.ITEM _cfg)
    {
        _clickType = clickType;
        info = _info;
        cfg = _cfg;
    }
    public void SetParams(TipsOpenType _clickType, bag.BagItemInfo _info, TABLE.ITEM _cfg)
    {
        _clickType = clickType;
        info = _info;
        cfg = _cfg;
    }
    public void UnInit()
    {
        clickType = TipsOpenType.Bag;
        info = null;
        cfg = null;
    }
}
