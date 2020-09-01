using UnityEngine;
using System.Collections.Generic;
using bag;
using System.Collections;
using auction;

public class ItemContainer
{
    public ItemContainer(PoolHandleManager poolHandleManager, GameObject parent, PropItemType propItemType = PropItemType.Bag, int capacity = 25)
    {
        mPoolHandleManager = poolHandleManager;
        mParent = parent;
        mPropItemType = propItemType;
        mItems = new FastArrayElementFromPool<UIItemBase>(capacity, OnCreate, OnRecycle);
        mItems.Count = capacity;
    }

    UIItemBase OnCreate()
    {
        return UIItemManager.Instance.GetItem(mPropItemType, mParent.transform);
    }

    void OnRecycle(UIItemBase item)
    {
        UIItemManager.Instance.RecycleSingleItem(item);
    }

    public void OnDestroy()
    {
        mPropItemType = PropItemType.Bag;
        mParent = null;
        mItems?.Clear();
        mItems = null;
        mPoolHandleManager = null;
    }

    PoolHandleManager mPoolHandleManager;
    PropItemType mPropItemType = PropItemType.Bag;
    GameObject mParent;

    FastArrayElementFromPool<UIItemBase> mItems;

    public void Bind(FastArrayElementKeepHandle<BagItemInfo> datas, System.Action<UIItemBase, BagItemInfo> onItemVisible)
    {
        for (int i = 0; i < mItems.Count; ++i)
        {
            onItemVisible?.Invoke(mItems[i], i < datas.Count ? datas[i] : null);
        }
    }
}

public partial class UIGuildBagPanel : UIBasePanel
{
    UIGrid bagList = null;
    UIGrid BagList
    {
        get
        {
            return bagList;
        }
        set
        {
            bagList = value;
            mBagContainer = UIEventListener.Get(bagList.gameObject).parameter as ItemContainer;
        }
    }

    UIGrid warehouseList = null;
    UIGrid WarehouseList
    {
        get
        {
            return warehouseList;
        }
        set
        {
            warehouseList = value;
            mWareHouseContainer = UIEventListener.Get(warehouseList.gameObject).parameter as ItemContainer;
        }
    }

    int mTotalBag = 5;
    int mTotalWhareHouse = 10;
    int mnBagItemOnePageCount = 25;
    int mnBagSpace = 400;
    int mnBagInitX = 0;
    int mnBagY = 0;
    Vector3 mScrollWareHousePos = Vector3.zero;
    FastArrayElementKeepHandle<BagItemInfo> mTotalBagItems = new FastArrayElementKeepHandle<BagItemInfo>(32);

    UnionBagStatus _unionBagStatus = UnionBagStatus.Common;
    UnionBagStatus unionBagStatus
    {
        get
        {
            return _unionBagStatus;
        }
        set
        {
            _unionBagStatus = value;
            OnBagStatusChagned();
        }
    }

    protected void OnBagStatusChagned()
    {
        UpdateBagSettingDescVisible();
        mDestroyBtnEffect.CustomActive(unionBagStatus == UnionBagStatus.Destory);
        mGoCancelDestory.CustomActive(unionBagStatus == UnionBagStatus.Destory);
        mGoExchangePanel.CustomActive(unionBagStatus != UnionBagStatus.Destory && CSGuildInfo.Instance.IsPresident);
        mBtnSetting.CustomActive(unionBagStatus != UnionBagStatus.Destory);
        if (unionBagStatus == UnionBagStatus.Destory)
        {
            mtg_select.Set(false);
            mtg_select_career.Set(false);
        }
    }

    HashSet<long> mItemId = new HashSet<long>();
    private enum UnionBagStatus
    {
        Common,
        Destory
    }

    protected void OnHelp(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GuildBagPanel);
    }

    ItemContainer mBagContainer;
    ItemContainer mBagLeftContainer;
    ItemContainer mBagCenterContainer;
    ItemContainer mBagRightContainer;

    ItemContainer mWareHouseContainer;
    ItemContainer mWarehouseLeftContainer;
    ItemContainer mWarehouseCenterContainer;
    ItemContainer mWarehouseRightContainer;


    public override void Init()
    {
        base.Init();

        mScrollWareHousePos = mScrollView.transform.localPosition;

        mbtn_help.onClick = OnHelp;
        mbtn_donate.onClick = OnDonateClick;

        EventDelegate.Add(mtg_select.onChange, OnSelectClick);
        EventDelegate.Add(mtg_select_career.onChange, OnSelectClick);
        UIEventListener.Get(mget_settings).onClick = OnClickStatus;

        mBagLeftContainer = new ItemContainer(mPoolHandleManager, mBagLeft.gameObject, PropItemType.GuildBag, mnBagItemOnePageCount);
        UIEventListener.Get(mBagLeft.gameObject).parameter = mBagLeftContainer;

        mBagCenterContainer = new ItemContainer(mPoolHandleManager, mBagCenter.gameObject, PropItemType.GuildBag, mnBagItemOnePageCount);
        UIEventListener.Get(mBagCenter.gameObject).parameter = mBagCenterContainer;

        mBagRightContainer = new ItemContainer(mPoolHandleManager, mBagRight.gameObject, PropItemType.GuildBag, mnBagItemOnePageCount);
        UIEventListener.Get(mBagRight.gameObject).parameter = mBagRightContainer;

        mWarehouseLeftContainer = new ItemContainer(mPoolHandleManager, mWarehouseLeft.gameObject, PropItemType.GuildWarehouse, mnBagItemOnePageCount);
        UIEventListener.Get(mWarehouseLeft.gameObject).parameter = mWarehouseLeftContainer;

        mWarehouseCenterContainer = new ItemContainer(mPoolHandleManager, mWarehouseCenter.gameObject, PropItemType.GuildWarehouse, mnBagItemOnePageCount);
        UIEventListener.Get(mWarehouseCenter.gameObject).parameter = mWarehouseCenterContainer;

        mWarehouseRightContainer = new ItemContainer(mPoolHandleManager, mWarehouseRight.gameObject, PropItemType.GuildWarehouse, mnBagItemOnePageCount);
        UIEventListener.Get(mWarehouseRight.gameObject).parameter = mWarehouseRightContainer;

        mBtnSetting.onClick = OnClickBtnSetting;
        UIEventListener.Get(mGoSettingBg.gameObject).onClick = OnClickCloseSetting;
        mbtn_combine.onClick = OnClickBtnCancelDestory;
        mbtn_destroy.onClick = OnClickDestroyItemInUnion;

        mClientEvent.AddEvent(CEvent.OnTipsGuildDonate, OnTipsGuildDonate);
        mClientEvent.AddEvent(CEvent.OnTipsGuildExchange, OnTipsGuildExchange);
        mClientEvent.AddEvent(CEvent.OnGuildBagChange, OnGuildBagChanged);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnBagItemChanged);
        mClientEvent.AddEvent(CEvent.WearEquip, OnEquipItemChanged);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildPosChanged, OnMainPlayerGuildPosChanged);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnRefreshCurrency);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnRefreshCurrency);
        mCenterOnBag.onCenter = OnBagPageTextRefresh;
        mCenterOnWarehouse.onCenter = OnWharehousePageTextRefresh;

        CSEffectPlayMgr.Instance.ShowUITexture(mTexBg1.gameObject, "guild_bg3");

        CSGuildInfo.Instance.Tab = UnionTab.UnionStoreHouse;
        unionBagStatus = UnionBagStatus.Common;
    }

    public override void Show()
    {
        base.Show();

        unionBagStatus = UnionBagStatus.Common;

        Refresh();

        Net.CSGetUnionTabMessage((int)UnionTab.UnionStoreHouse);
    }

    void Refresh()
    {
        InitOption();
        InitSettingOptions();

        RefreshUnionAttributeCurrency();

        mget_settings.CustomActive(CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President);
        RefreshDestoryState();

        mTotalBag = 5;
        mTotalWhareHouse = 10;
        UpdateBagInfo();
        UpdateWharehouseInfo();
    }

    private void RefreshDestoryState()
    {
        mbtn_destroy.CustomActive(CSMainPlayerInfo.Instance.GuildPos <= (int)GuildPos.VicePresident);
    }

    private int mIndex = 1;
    private int Index
    {
        set
        {
            mIndex = value;
        }
        get
        {
            return mIndex;
        }
    }

    private int mIndexBag = 1;
    private int IndexBag
    {
        set
        {
            mIndexBag = value;
        }
        get
        {
            return mIndexBag;
        }
    }

    bool IsMatch(BagItemInfo info)
    {
        TABLE.ITEM tbl_item;
        if (!ItemTableManager.Instance.TryGetValue(info.configId, out tbl_item))
            return false;

        if (CSGuildInfo.Instance.mLisFamilyBagDonateItem.Contains(info.configId))
            return true;

        if (tbl_item.uniondonate <= 0)
            return false;

        if (!CSBagInfo.Instance.IsWoLongEquip(tbl_item))
        {
            return false;
        }

        //if(!(tbl_item.binding == 0 || tbl_item.binding == 2))
        //{
        //    return false;
        //}

        return true;
    }

    private void OnChangeClick(GameObject gp = null)
    {
        if (gp == null) return;

        if (gp.name.Equals("btn_add"))
        {
            if (Index < 10) Index++;
        }
        else if (gp.name.Equals("btn_reduced"))
        {
            if (Index > 1) Index--;
        }
    }

    private void OnChangeBagClick(GameObject gp = null)
    {
        if (gp.name.Equals("btn_reducedbag"))
        {
            if (IndexBag > 1) IndexBag--;
        }
        else if (gp.name.Equals("btn_addbag"))
        {
            if (IndexBag < 10) IndexBag++;
        }

    }

    private void OnClickBtnSetting(GameObject go)
    {
        SetSettingPanelVisible(true);
    }

    private void OnClickCloseSetting(GameObject go)
    {
        SetSettingPanelVisible(false);
    }

    private void SetSettingPanelVisible(bool isShow)
    {
        mGoSettingPanel.SetActive(isShow);
    }

    //int donateCount = 0;
    /// <summary>
    /// 捐献银子
    /// </summary>
    /// <param name="gp"></param>
    private void OnDonateClick(GameObject gp)
    {
        UIManager.Instance.CreatePanel<UIGuildDonatePanel>();
        Net.CSGetUnionTabMessage((int)UnionTab.UnionStoreHouse);
    }

    /// <summary>
    /// 是否为单击
    /// </summary>
    //bool isSingleClick = true;
    /// <summary>
    /// 出现tip时间
    /// </summary>
    //float CDTimer = 0;
    /// <summary>
    /// 点击的上个物体
    /// </summary>
    GameObject lastObject;

    //捐款装备
    private void OnDonationEquipClick(BagItemInfo bagItemInfo)
    {
        if (bagItemInfo == null)
            return;
        if (CSGuildInfo.Instance.mLisFamilyBagDonateItem.Contains(bagItemInfo.configId))
        {
            UIManager.Instance.CreatePanel<UIGuildBagDonatePanel>(p =>
            {
                UIGuildBagDonatePanel panel = p as UIGuildBagDonatePanel;
                if (panel != null) panel.RefreshUI(UIGuildBagDonatePanel.FamilyBagType.Donate, bagItemInfo);
            });
        }
        else
        {
            Net.CSUnionDonateEquipMessage(bagItemInfo.bagIndex, 1);
        }
    }

    private void OnRefreshCurrency(uint id, object argv)
    {
        RefreshUnionAttributeCurrency();
    }

    void RefreshUnionAttributeCurrency()
    {
        long unionAttrValue = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
        mlb_gongxiandu.text = $"{unionAttrValue}".ToCurrency();
    }

    private void ResEquipCombine(uint id, params object[] data)
    {
        UpdateWharehouseInfo();
    }

    private void OnBagItemChanged(uint id, object argv)
    {
        UpdateBagInfo();
    }

    private void OnEquipItemChanged(uint id, object argv)
    {
        UpdateBagInfo();
    }

    private void OnMainPlayerGuildPosChanged(uint id, object argv)
    {
        if (CSMainPlayerInfo.Instance.GuildPos < (int)GuildPos.VicePresident)
        {
            RefreshDestoryState();
            ResfreshGuildBagState();
        }
    }

    #region 仓库移动，显示相关功能
    private void ShowWareHouseByIndex()
    {
        int surplus = CSGuildInfo.Instance.mbagItemInfos.Count % mnBagItemOnePageCount;
        int tatol = surplus > 0 ? (mTotalItemList.Count / mnBagItemOnePageCount) + 1 : mTotalItemList.Count / mnBagItemOnePageCount;
        SelectWareHouseByIndex(tatol);
    }

    private void SelectWareHouseByIndex(int nIndex)
    {
        Transform tranLeft = mWarehouseLeft.transform.parent;
        Transform tranCenter = mWarehouseCenter.transform.parent;
        Transform tranRight = mWarehouseRight.transform.parent;
        if (nIndex <= 0 || nIndex > mTotalWhareHouse) return;
        if (nIndex == 1)
        {
            tranLeft.name = "1";
            tranCenter.name = "2";
            tranRight.name = "3";
            tranLeft.localPosition = GetTransferPos(1);
            tranCenter.localPosition = GetTransferPos(2);
            tranRight.localPosition = GetTransferPos(3);
            WarehouseList = tranLeft.transform.GetChild(0).GetComponent<UIGrid>();
            Index = 1;
            UpdateWharehouseInfo();
            mCenterOnWarehouse.CenterOn(tranLeft);
        }
        else if (nIndex == 10)
        {
            tranLeft.name = "8";
            tranCenter.name = "9";
            tranRight.name = "10";
            tranLeft.localPosition = GetTransferPos(8);
            tranCenter.localPosition = GetTransferPos(9);
            tranRight.localPosition = GetTransferPos(10);
            WarehouseList = tranRight.transform.GetChild(0).GetComponent<UIGrid>();
            Index = 10;
            UpdateWharehouseInfo();
            mCenterOnWarehouse.CenterOn(tranRight);
        }
        else
        {
            tranLeft.name = (nIndex - 1).ToString();
            tranCenter.name = nIndex.ToString();
            tranRight.name = (nIndex + 1).ToString();
            tranLeft.localPosition = GetTransferPos(nIndex - 1);
            tranCenter.localPosition = GetTransferPos(nIndex);
            tranRight.localPosition = GetTransferPos(nIndex + 1);
            WarehouseList = tranCenter.transform.GetChild(0).GetComponent<UIGrid>();
            Index = nIndex;
            UpdateWharehouseInfo();
            mCenterOnWarehouse.CenterOn(tranCenter);
        }
    }

    FastArrayElementKeepHandle<BagItemInfo> mTotalItemList = new FastArrayElementKeepHandle<bag.BagItemInfo>(25);
    FastArrayElementKeepHandle<BagItemInfo> mWhareExpressedWhareHouseList = new FastArrayElementKeepHandle<BagItemInfo>(25);
    public bool OnPassWhareHouseFilter(BagItemInfo bagItemInfo, long unionContribute)
    {
        if (null == bagItemInfo)
            return false;

        TABLE.ITEM tbl_item = null;
        if (!ItemTableManager.Instance.TryGetValue(bagItemInfo.configId, out tbl_item))
            return false;

        #region 筛选
        if (mtg_select.value)
        {
            if (unionContribute < tbl_item.uniondonate)
                return false;

            if (tbl_item.wolongLv > CSWoLongInfo.Instance.GetWoLongLevel())
                return false;
        }

        if (mtg_select_career.value)
        {
            if (tbl_item.career != 0 && CSMainPlayerInfo.Instance.Career != tbl_item.career)
                return false;
        }
        #endregion

        return true;
    }

    void RebuildWhareHouseDatas()
    {
        long unionContribute = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
        mTotalItemList.Clear();
        var guildBagItems = CSGuildInfo.Instance.mbagItemInfos;
        for (int i = 0; i < guildBagItems.Count; ++i)
        {
            var bagItem = guildBagItems[i];
            if (!OnPassWhareHouseFilter(bagItem, unionContribute))
            {
                continue;
            }
            mTotalItemList.Append(bagItem);
        }
        mTotalItemList.Sort(sortguild);
    }

    public void ShowWharehouseInfo(int index)
    {
        mWhareExpressedWhareHouseList.Clear();

        int startcount = 0;
        int endcount = 0;
        int career = CSMainPlayerInfo.Instance.Career;

        int surplus = mTotalItemList.Count % mnBagItemOnePageCount;
        int tatol = surplus > 0 ? (mTotalItemList.Count / mnBagItemOnePageCount) + 1 : mTotalItemList.Count / mnBagItemOnePageCount;
        if (tatol == 0) tatol = 1;
        if (index < tatol)
        {
            startcount = (index - 1) * mnBagItemOnePageCount;
            int count = mTotalItemList.Count > mnBagItemOnePageCount ? mnBagItemOnePageCount : mTotalItemList.Count;
            mTotalItemList.GetRange(startcount, count, mWhareExpressedWhareHouseList);
        }
        else if (index == tatol)
        {
            startcount = (index - 1) * mnBagItemOnePageCount;
            endcount = mTotalItemList.Count - startcount;
            mTotalItemList.GetRange(startcount, endcount, mWhareExpressedWhareHouseList);
        }

        mWareHouseContainer.Bind(mWhareExpressedWhareHouseList, OnWharehouseItemVisible);
    }

    void RefreshWhareHouseChildIndex()
    {
        int centerIndex = int.Parse(mCenterOnWarehouse.centeredObject.name);
        if (mIndex != centerIndex)
        {
            int deltaIndex = mIndex - centerIndex;
            for (int i = 0; i < Mathf.Abs(deltaIndex); i++)
            {
                if (deltaIndex < 0 && mIndex != mTotalWhareHouse)
                {
                    Index++;
                }
                else if (deltaIndex > 0 && mIndex != 1)
                {
                    Index--;
                }
                //3~8                                              //2                                   //9  
                if ((mIndex > 2 && mIndex < mTotalWhareHouse - 1) || (mIndex == 2 && deltaIndex > 0) || (mIndex == mTotalWhareHouse - 1 && deltaIndex < 0))
                {
                    GameObject obj = GetMoveWharehouseObject(deltaIndex);
                    if (obj == null)
                    {
                        return;
                    }
                    if (deltaIndex < 0)
                    {
                        obj.name = (mIndex + 1).ToString();
                        obj.transform.localPosition = GetTransferPos(mIndex + 1);
                        WarehouseList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                        ShowWharehouseInfo(mIndex + 1);
                    }
                    else
                    {
                        obj.name = (mIndex - 1).ToString();
                        obj.transform.localPosition = GetTransferPos(mIndex - 1);
                        WarehouseList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                        ShowWharehouseInfo(mIndex - 1);
                    }
                    mWarehouseLeft.transform.parent.SetParent(mTempCenter);
                    mWarehouseCenter.transform.parent.SetParent(mTempCenter);
                    mWarehouseRight.transform.parent.SetParent(mTempCenter);
                    mWarehouseLeft.transform.parent.SetParent(mCenterOnWarehouse.transform);
                    mWarehouseCenter.transform.parent.SetParent(mCenterOnWarehouse.transform);
                    mWarehouseRight.transform.parent.SetParent(mCenterOnWarehouse.transform);
                }
            }
        }
    }

    void OnWharehousePageTextRefresh(GameObject obj)
    {
        mlb_num.text = "<  " + obj.name + "/" + mTotalWhareHouse.ToString() + "  >";
        RefreshWhareHouseChildIndex();
    }

    /// <summary>
    /// 返回移走的那页仓库
    /// </summary>
    /// <param name="temp_delate_x"></param>
    /// <returns></returns>
    private GameObject GetMoveWharehouseObject(float temp_delate_x)
    {
        //左移
        if (temp_delate_x < 0)
        {
            var temp = mWarehouseLeft;
            mWarehouseLeft = mWarehouseCenter;
            mWarehouseCenter = mWarehouseRight;
            mWarehouseRight = temp;
            return temp.transform.parent.gameObject;
        }
        else if (temp_delate_x > 0) //右移
        {
            var temp = mWarehouseRight;
            mWarehouseRight = mWarehouseCenter;
            mWarehouseCenter = mWarehouseLeft;
            mWarehouseLeft = temp;
            return temp.transform.parent.gameObject;
        }
        else
        {
            return null;
        }
    }

    private Vector3 GetTransferWharehousePos(int wharehouseindex)
    {
        return new Vector3(mnBagInitX + mnBagSpace * (wharehouseindex - 1), mnBagY, 0);
    }

    void OnWhareHouseItemClicked(UIItemBase itemBase)
    {
        if (null == itemBase && null == itemBase.infos)
        {
            return;
        }

        if (unionBagStatus == UnionBagStatus.Common)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.GuildWareHouseExchange, itemBase.infos);
        }
        else
        {
            OnClickDestoryItem(itemBase);
        }
    }

    void OnWharehouseItemVisible(UIItemBase itemBase, BagItemInfo bagItemInfo)
    {
        var eventListener = UIEventListener.Get(itemBase.obj);
        itemBase.Refresh(bagItemInfo, OnWhareHouseItemClicked);
        itemBase.SetItemDBClickedCB(OnExchangeEquip);
        itemBase.SetMaskJudgeState(true);
        UIEventListener.Get(itemBase.obj).onDrag = OnDragItemWharehouse;
        UIEventListener.Get(itemBase.obj).onDragEnd = OnDragEndWharehouse;
        itemBase.ShowSelect(null != bagItemInfo && mItemId.Contains(bagItemInfo.id));
    }

    void UpdateWharehouseInfo()
    {
        RebuildWhareHouseDatas();
        if (Index <= 2)
        {
            WarehouseList = mWarehouseLeft;
            ShowWharehouseInfo(1);
            WarehouseList = mWarehouseCenter;
            ShowWharehouseInfo(2);
            WarehouseList = mWarehouseRight;
            ShowWharehouseInfo(3);
        }
        else if (Index >= mTotalWhareHouse - 1)
        {
            WarehouseList = mWarehouseLeft;
            ShowWharehouseInfo(mTotalWhareHouse - 2);
            WarehouseList = mWarehouseCenter;
            ShowWharehouseInfo(mTotalWhareHouse - 1);
            WarehouseList = mWarehouseRight;
            ShowWharehouseInfo(mTotalWhareHouse);
        }
        else
        {
            WarehouseList = mWarehouseLeft;
            ShowWharehouseInfo(mIndex - 1);
            WarehouseList = mWarehouseCenter;
            ShowWharehouseInfo(mIndex);
            WarehouseList = mWarehouseRight;
            ShowWharehouseInfo(mIndex + 1);
        }
    }

    private void sortguild(ref long sortValue, bag.BagItemInfo itemInfo)
    {
        TABLE.ITEM itemCfg = null;
        if (!ItemTableManager.Instance.TryGetValue(itemInfo.configId, out itemCfg))
        {
            sortValue = 0;
            return;
        }
        sortValue = itemCfg.id;
    }

    private float temp_delatewharehouse_x = 0;
    private void OnDragItemWharehouse(GameObject go, Vector2 delate)
    {
        temp_delatewharehouse_x = temp_delatewharehouse_x + delate.x;
        if (Mathf.Abs(temp_delatewharehouse_x) >= 288)
        {
            if (temp_delatewharehouse_x < 0 && mIndex != mTotalWhareHouse)
            {
                Index++;
            }
            else if (temp_delatewharehouse_x > 0 && mIndex != 1)
            {
                Index--;
            }
            else
            {
                return;
            }
            if ((mIndex > 2 && mIndex < mTotalWhareHouse - 1) || (mIndex == 2 && temp_delatewharehouse_x > 0) || (mIndex == mTotalWhareHouse - 1 && temp_delatewharehouse_x < 0))
            {
                GameObject obj = GetMoveWharehouseObject(temp_delatewharehouse_x);
                if (obj == null)
                {
                    return;
                }
                if (temp_delatewharehouse_x < 0)
                {
                    obj.name = (mIndex + 1).ToString();
                    obj.transform.localPosition = GetTransferWharehousePos(mIndex + 1);
                    WarehouseList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                    ShowWharehouseInfo(mIndex + 1);
                }
                else
                {
                    obj.name = (mIndex - 1).ToString();
                    obj.transform.localPosition = GetTransferWharehousePos(mIndex - 1);
                    WarehouseList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                    ShowWharehouseInfo(mIndex - 1);
                }
                mWarehouseLeft.transform.parent.SetParent(mTempCenter.transform);
                mWarehouseCenter.transform.parent.SetParent(mTempCenter.transform);
                mWarehouseRight.transform.parent.SetParent(mTempCenter.transform);
                mWarehouseLeft.transform.parent.SetParent(mCenterOnWarehouse.transform);
                mWarehouseCenter.transform.parent.SetParent(mCenterOnWarehouse.transform);
                mWarehouseRight.transform.parent.SetParent(mCenterOnWarehouse.transform);
            }
            temp_delatewharehouse_x = 0;
        }
    }

    private void OnDragEndWharehouse(GameObject go)
    {
        temp_delatewharehouse_x = 0;
    }

    private void WarehouseResetToBegining()
    {
        mScrollView.transform.localPosition = mScrollWareHousePos;
        mScrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        mWarehouseLeft.transform.parent.localPosition = new Vector3(mnBagInitX, mnBagY, 0);
        mWarehouseCenter.transform.parent.localPosition = new Vector3(mnBagInitX + mnBagSpace, mnBagY, 0);
        mWarehouseRight.transform.parent.localPosition = new Vector3(mnBagInitX + mnBagSpace * 2, mnBagY, 0);
        mWarehouseLeft.transform.parent.gameObject.name = "1";
        mWarehouseCenter.transform.parent.gameObject.name = "2";
        mWarehouseRight.transform.parent.gameObject.name = "3";
        Index = 1;
        mlb_num.text = string.Format("{0}/{1}", Index, mTotalWhareHouse);
        mCenterOnWarehouse.CenterOn(mWarehouseLeft.transform.parent);
    }
    #endregion

    #region 人物背包移动，显示相关功能
    void UpdateBagInfo()
    {
        RebuildBuildBagInfoDatas();
        if (IndexBag <= 2)
        {
            BagList = mBagLeft;
            ShowBagInfo(1);
            BagList = mBagCenter;
            ShowBagInfo(2);
            BagList = mBagRight;
            ShowBagInfo(3);
        }
        else if (IndexBag >= mTotalBag - 1)
        {
            BagList = mBagLeft;
            ShowBagInfo(mTotalBag - 2);
            BagList = mBagCenter;
            ShowBagInfo(mTotalBag - 1);
            BagList = mBagRight;
            ShowBagInfo(mTotalBag);
        }
        else
        {
            BagList = mBagLeft;
            ShowBagInfo(mIndexBag - 1);
            BagList = mBagCenter;
            ShowBagInfo(mIndexBag);
            BagList = mBagRight;
            ShowBagInfo(mIndexBag + 1);
        }
    }

    /// <summary>
    /// 背包仓库
    /// </summary>
    FastArrayElementKeepHandle<BagItemInfo> listBag = new FastArrayElementKeepHandle<BagItemInfo>(25);
    private void OnBagItemVisible(UIItemBase itemBase, BagItemInfo bagItemInfo)
    {
        var eventListener = UIEventListener.Get(itemBase.obj);
        itemBase.Refresh(bagItemInfo, OnCallDonateTips);
        itemBase.SetItemDBClickedCB(OnDonateBagItem);
        itemBase.SetMaskJudgeState(true);
        UIEventListener.Get(itemBase.obj).onDrag = OnDragItemBag;
        UIEventListener.Get(itemBase.obj).onDragEnd = OnDragBagEnd;
    }

    void RebuildBuildBagInfoDatas()
    {
        mTotalBagItems.Clear();
        var bagItems = CSBagInfo.Instance.GetBagItemData();
        for (var it = bagItems.GetEnumerator(); it.MoveNext();)
        {
            if (!IsMatch(it.Current.Value))
            {
                continue;
            }

            mTotalBagItems.Append(it.Current.Value);
        }

        mTotalBagItems.Sort(sort);
    }

    private void ShowBagInfo(int IndexBag)
    {
        listBag.Clear();

        int surplus = mTotalBagItems.Count % mnBagItemOnePageCount;
        int total = surplus > 0 ? (mTotalBagItems.Count / mnBagItemOnePageCount) + 1 : mTotalBagItems.Count / mnBagItemOnePageCount;
        int startcount = 0;
        int endcount = 0;

        if (IndexBag < total)
        {
            startcount = (IndexBag - 1) * mnBagItemOnePageCount;
            int count = mTotalBagItems.Count > mnBagItemOnePageCount ? mnBagItemOnePageCount : mTotalBagItems.Count;
            mTotalBagItems.GetRange(startcount, count, listBag);
        }
        else if (IndexBag == total)
        {
            startcount = (IndexBag - 1) * mnBagItemOnePageCount;
            endcount = mTotalBagItems.Count - startcount;
            mTotalBagItems.GetRange(startcount, endcount, listBag);
        }

        mBagContainer.Bind(listBag, OnBagItemVisible);
    }

    void RefreshBagChildIndex()
    {
        int centerIndex = int.Parse(mCenterOnBag.centeredObject.name);
        if (mIndexBag != centerIndex)
        {
            int deltaIndex = mIndexBag - centerIndex;
            for (int i = 0; i < Mathf.Abs(deltaIndex); i++)
            {
                if (deltaIndex < 0 && mIndexBag != mTotalBag)
                {
                    IndexBag++;
                }
                else if (deltaIndex > 0 && mIndexBag != 1)
                {
                    IndexBag--;
                }
                if ((mIndexBag > 2 && mIndexBag < mTotalBag - 1) || (mIndexBag == 2 && deltaIndex > 0) || (mIndexBag == mTotalBag - 1 && deltaIndex < 0))
                {
                    GameObject obj = GetMoveBagObject(deltaIndex);
                    if (obj == null)
                    {
                        return;
                    }
                    if (deltaIndex < 0)
                    {
                        obj.name = (mIndexBag + 1).ToString();
                        obj.transform.localPosition = GetTransferPos(mIndexBag + 1);
                        BagList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                        ShowBagInfo(mIndexBag + 1);
                    }
                    else
                    {
                        obj.name = (mIndexBag - 1).ToString();
                        obj.transform.localPosition = GetTransferPos(mIndexBag - 1);
                        BagList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                        ShowBagInfo(mIndexBag - 1);
                    }
                    mBagLeft.transform.parent.SetParent(mTempCenter);
                    mBagCenter.transform.parent.SetParent(mTempCenter);
                    mBagRight.transform.parent.SetParent(mTempCenter);
                    mBagLeft.transform.parent.SetParent(mCenterOnBag.transform);
                    mBagCenter.transform.parent.SetParent(mCenterOnBag.transform);
                    mBagRight.transform.parent.SetParent(mCenterOnBag.transform);
                }
            }
        }
    }

    void OnBagPageTextRefresh(GameObject obj)
    {
        mlb_numbag.text = "<  " + obj.name + "/" + mTotalBag + "  >";
        RefreshBagChildIndex();
    }

    private GameObject GetMoveBagObject(float temp_delate_x)
    {
        if (temp_delate_x < 0)
        {
            var temp = mBagLeft;
            mBagLeft = mBagCenter;
            mBagCenter = mBagRight;
            mBagRight = temp;
            return temp.transform.parent.gameObject;
        }
        else if (temp_delate_x > 0)
        {
            var temp = mBagRight;
            mBagRight = mBagCenter;
            mBagCenter = mBagLeft;
            mBagLeft = temp;
            return temp.transform.parent.gameObject;
        }
        else
        {
            return null;
        }
    }

    private Vector3 GetTransferPos(int bagindex)
    {
        return new Vector3(mnBagInitX + mnBagSpace * (bagindex - 1), mnBagY, 0);
    }

    private void sort(ref long sortValue,BagItemInfo bagItemInfo)
    {
        sortValue = bagItemInfo.id;
    }

    private float temp_delatebag_x = 0;
    void OnDragItemBag(GameObject go, Vector2 delate)
    {
        temp_delatebag_x = temp_delatebag_x + delate.x;
        if (Mathf.Abs(temp_delatebag_x) >= mnBagSpace)
        {
            if (temp_delatebag_x < 0 && mIndexBag != mTotalBag)
            {
                IndexBag++;
            }
            else if (temp_delatebag_x > 0 && mIndexBag != 1)
            {
                IndexBag--;
            }
            else
            {
                return;
            }
            if ((mIndexBag > 2 && mIndexBag < mTotalBag - 1) || (mIndexBag == 2 && temp_delatebag_x > 0) || (mIndexBag == mTotalBag - 1 && temp_delatebag_x < 0))
            {
                GameObject obj = GetMoveBagObject(temp_delatebag_x);
                if (obj == null)
                {
                    return;
                }
                if (temp_delatebag_x < 0)
                {
                    obj.name = (mIndexBag + 1).ToString();
                    obj.transform.localPosition = GetTransferPos(mIndexBag + 1);
                    BagList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                    ShowBagInfo(mIndexBag + 1);
                }
                else
                {
                    obj.name = (mIndexBag - 1).ToString();
                    obj.transform.localPosition = GetTransferPos(mIndexBag - 1);
                    BagList = obj.transform.GetChild(0).GetComponent<UIGrid>();
                    ShowBagInfo(mIndexBag - 1);
                }
                mBagLeft.transform.parent.SetParent(mTempCenter.transform);
                mBagCenter.transform.parent.SetParent(mTempCenter.transform);
                mBagRight.transform.parent.SetParent(mTempCenter.transform);
                mBagLeft.transform.parent.SetParent(mCenterOnBag.transform);
                mBagCenter.transform.parent.SetParent(mCenterOnBag.transform);
                mBagRight.transform.parent.SetParent(mCenterOnBag.transform);
            }
            temp_delatebag_x = 0;
        }
    }

    private void OnDragBagEnd(GameObject go)
    {
        temp_delatebag_x = 0;
    }

    /// <summary>
    /// 双击捐赠
    /// </summary>
    private void OnDonateBagItem(UIItemBase itemBase)
    {
        if (null != itemBase && null != itemBase.infos)
        {
            OnDonationEquipClick(itemBase.infos);
        }
    }

    void OnTipsGuildDonate(uint id, object argv)
    {
        if (argv is BagItemInfo bagItem)
        {
            OnDonationEquipClick(bagItem);
        }
    }

    void OnTipsGuildExchange(uint id, object argv)
    {
        if (argv is BagItemInfo bagItem)
        {
            ExchangeEquip(bagItem);
        }
    }

    /// <summary>
    /// 点击背包道具
    /// </summary>
    /// <param name="gp"></param>
    private void OnCallDonateTips(UIItemBase itemBase)
    {
        if (null != itemBase && null != itemBase.infos)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.GuildWareHouseDonate, itemBase.infos);
        }
    }
    #endregion

    #region 物品装备相关功能
    /// <summary>
    /// 显示可物品
    /// </summary>
    private void OnSelectClick()
    {
        WarehouseResetToBegining();
        UpdateWharehouseInfo();
    }

    // 物品装备
    private void OnExchangeEquip(UIItemBase itemBase)
    {
        if (null == itemBase || null == itemBase.infos)
        {
            return;
        }
        ExchangeEquip(itemBase.infos);
    }

    void ExchangeEquip(BagItemInfo bagItemInfo)
    {
        if (CSGuildInfo.Instance.mLisFamilyBagDonateItem.Contains(bagItemInfo.configId))
        {
            UIManager.Instance.CreatePanel<UIGuildBagDonatePanel>(p =>
            {
                UIGuildBagDonatePanel panel = p as UIGuildBagDonatePanel;
                if (panel != null) panel.RefreshUI(UIGuildBagDonatePanel.FamilyBagType.Exchange, bagItemInfo);
            });
        }
        else
        {
            TABLE.ITEM tblItem;
            if (ItemTableManager.Instance.TryGetValue(bagItemInfo.configId, out tblItem))
            {
                long unionContribute = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
                if (tblItem.uniondonate > unionContribute)
                {
                    UtilityTips.ShowRedTips(914);
                    return;
                }
            }

            Net.CSUnionExchangeEquipMessage(bagItemInfo.id, 1);
        }
    }

    void InitOption()
    {
        int storeEquipLvLimit = CSGuildInfo.Instance.storeReincarnation;

        UpdateBagSettingDescVisible();

        var strName = CSGuildInfo.Instance.GetOptionName(storeEquipLvLimit);
        mget_settings_text.text = strName;

        if (strName == CSString.Format(902))
        {
            mLabBagSetNotPresidentDesc.text = string.Empty;
        }
        else
        {
            mLabBagSetNotPresidentDesc.text = CSString.Format(903, strName);
        }
    }

    void InitSettingOptions()
    {
        Map<int, string> mapCondition = GetSetting();
        mget_settings_grid.MaxCount = mapCondition.Count;
        mGet_settings_gridBg.height = ((mapCondition.Count / 2) + (mapCondition.Count % 2)) * (36 + 11) + 11;
        int index = 0;
        for (mapCondition.Begin(); mapCondition.Next(); index++)
        {
            InitSettingItemInfo(mget_settings_grid.controlList[index], mapCondition.Key);
        }
    }

    void InitSettingItemInfo(GameObject item, int nDiyuan)
    {
        UILabel label = item.transform.Find("Text").GetComponent<UILabel>();
        var select = item.transform.Find("Select");

        label.text = CSGuildInfo.Instance.GetOptionName(nDiyuan);

        select.CustomActive(label.text.Trim().Equals(mget_settings_text.text.Trim()));
        UIEventListener.Get(item).onClick = OnChangeStatus;
        UIEventListener.Get(item).parameter = nDiyuan;
    }

    Map<int, string> mDicCondition;
    private Map<int, string> GetSetting()
    {
        if (null != mDicCondition)
            return mDicCondition;

        mDicCondition = new Map<int, string>(6);

        TABLE.SUNDRY tblSundry;
        if (SundryTableManager.Instance.TryGetValue(458, out tblSundry))
        {
            string effectName = tblSundry.effect;
            string[] options = effectName.Split('&');

            for (int i = options.Length - 1; i >= 0; i--)
            {
                string[] subStr = options[i].Split('#');
                int serverDay = 0;
                int condition = 0;
                if (subStr.Length != 2)
                    continue;
                if (!int.TryParse(subStr[0], out serverDay))
                    continue;
                if (!int.TryParse(subStr[1], out condition))
                    continue;

                var openDays = CSMainPlayerInfo.Instance.RoleExtraValues?.openServerDays;
                if (openDays >= serverDay || true)
                {
                    Map<int, string> mapDiyuan = CSGuildInfo.Instance.GetExchangeDiYuan();
                    for (mapDiyuan.Begin(); mapDiyuan.Next();)
                    {
                        if (mapDiyuan.Key <= condition)
                        {
                            mDicCondition.Add(mapDiyuan.Key, mapDiyuan.Value);
                        }
                    }
                    return mDicCondition;
                }
            }
        }

        return mDicCondition;
    }

    bool onchoose = false;
    private void OnClickStatus(GameObject gobj)
    {
        TABLE.SUNDRY tbl_sundry = null;
        SundryTableManager.Instance.TryGetValue(457, out tbl_sundry);
        if (tbl_sundry == null)
        {
            return;
        }
        int needlv = int.Parse(tbl_sundry.effect);
        if (CSMainPlayerInfo.Instance.GuildLevel < needlv)
        {
            UtilityTips.ShowRedTips(901, needlv);
            return;
        }
        onChange(onchoose);
        mget_settings_grid.CustomActive(onchoose);
    }

    private void OnChangeStatus(GameObject btn)
    {
        onChange(true);
        mget_settings_grid.gameObject.SetActive(false);
        int par = (int)btn.GetComponent<UIEventListener>().parameter;

        Net.CSUnionExchangeEquipLimitMessage(par);
        Net.CSGetUnionTabMessage((int)UnionTab.UnionStoreHouse);
    }

    private void onChange(bool choose)
    {
        onchoose = !onchoose;
        mget_settings_arrow.transform.localEulerAngles = new Vector3(0, 0, onchoose ? 180 : 0);
        mget_settings_grid.CustomActive(onchoose);
    }

    /// <summary>
    /// 物品装备相应
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    private void OnGuildBagChanged(uint id, object argv)
    {
        Refresh();
        UpdateWharehouseInfo();
        UpdateBagInfo();
    }
    #endregion

    #region 摧毁装备
    //取消
    private void OnClickBtnCancelDestory(GameObject gp)
    {
        if (unionBagStatus != UnionBagStatus.Common)
        {
            unionBagStatus = UnionBagStatus.Common;
            ClearItem();
        }
    }

    private void ClearItem()
    {
        mItemId.Clear();
        UpdateWharehouseInfo();
    }

    //销毁装备
    private void OnUnionDestriyEquipClick()
    {
        Net.CSUnionDestroyItemMessage(mItemId.ToGoogleList());
    }

    private void OnClickDestroyItemInUnion(GameObject button)
    {
        if (unionBagStatus == UnionBagStatus.Destory)
        {
            if (IsChooseDestoryEquip())
            {
                int promptId = 48;
                bool orgValue = Constant.ShowTipsOnceList.Contains(promptId);
                UtilityTips.ShowPromptWordTips(promptId, () =>
                 {
                     Constant.ShowTipsOnceList.Remove(promptId);
                     if (orgValue)
                         Constant.ShowTipsOnceList.Add(promptId);
                 }, ConfirmToDestroy);
            }
            else
            {
                UtilityTips.ShowRedTips(900);
            }
        }
        else
        {
            ShowWareHouseByIndex();
        }
        unionBagStatus = UnionBagStatus.Destory;
    }

    protected void ConfirmToDestroy()
    {
        OnUnionDestriyEquipClick();
        ClearItem();
    }

    private void OnClickDestoryItem(UIItemBase itemBase)
    {
        if (null == itemBase || null == itemBase.infos)
            return;

        if (unionBagStatus != UnionBagStatus.Destory)
            return;

        bool choiced = mItemId.Contains(itemBase.infos.id);
        if (choiced)
        {
            mItemId.Remove(itemBase.infos.id);
            itemBase.ShowSelect(false);
        }
        else
        {
            mItemId.Add(itemBase.infos.id);
            itemBase.ShowSelect(true);
        }
    }

    private bool IsChooseDestoryEquip()
    {
        return mItemId.Count > 0;
    }
    #endregion

    private void ResfreshGuildBagState()
    {
        unionBagStatus = UnionBagStatus.Common;
        mDestroyBtnEffect.CustomActive(false);
        OnClickBtnCancelDestory(null);
    }

    /// <summary>
    /// 设置背包设置描述是否显示
    /// </summary>
    /// <param name="isShow"></param>
    private void SetBagSettingDescVisible(bool isShow)
    {
        if (!CSGuildInfo.Instance.IsPresident)
            mLabBagSetNotPresidentDesc.CustomActive(isShow);
        else
            mLabBagSetNotPresidentDesc.CustomActive(false);
    }

    private void UpdateBagSettingDescVisible()
    {
        int storeEquipLvLimit = CSGuildInfo.Instance.storeReincarnation;
        bool isPresident = CSGuildInfo.Instance.IsPresident;
        bool isDestroyMode = unionBagStatus == UnionBagStatus.Destory;
        mLabBagSetNotPresidentDesc.CustomActive(!isPresident && !isDestroyMode && storeEquipLvLimit != -1);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnTipsGuildDonate, OnTipsGuildDonate);
        mClientEvent.RemoveEvent(CEvent.OnTipsGuildExchange, OnTipsGuildExchange);
        mClientEvent.RemoveEvent(CEvent.OnGuildBagChange, OnGuildBagChanged);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnRefreshCurrency);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnRefreshCurrency);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnBagItemChanged);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnEquipItemChanged);
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildPosChanged, OnMainPlayerGuildPosChanged);

        mItemId?.Clear();
        mItemId = null;
        mBagLeftContainer?.OnDestroy();
        mBagLeftContainer = null;
        mBagCenterContainer?.OnDestroy();
        mBagCenterContainer = null;
        mBagRightContainer?.OnDestroy();
        mBagRightContainer = null;
        mBagContainer = null;
        bagList = null;

        mWarehouseLeftContainer?.OnDestroy();
        mWarehouseLeftContainer = null;
        mWarehouseCenterContainer?.OnDestroy();
        mWarehouseCenterContainer = null;
        mWarehouseRightContainer?.OnDestroy();
        mWarehouseRightContainer = null;
        mWareHouseContainer = null;
        warehouseList = null;

        Constant.ShowTipsOnceList.Remove(48);

        base.OnDestroy();
    }
}