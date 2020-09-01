using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIStorehousePanel : UIBasePanel
{
    #region  variable
    List<UIItemBase> itemList = new List<UIItemBase>();
    SpringPanel panel;
    int curPageNum = 1;
    Dictionary<GameObject, WarehouseGridData> itemsDic = new Dictionary<GameObject, WarehouseGridData>();
    #endregion

    public void SetGO(GameObject _go)
    {
        UIPrefab = _go;
    }


    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.GetWarehouseData, GetData);
        mClientEvent.AddEvent(CEvent.GetWarehouseItemsChange, GetWarehouseChange);
        mClientEvent.AddEvent(CEvent.GetWarehouseSort, GetWarehouseChange);
        mClientEvent.AddEvent(CEvent.BagItemsDrag, GetItemsDrag);
        mClientEvent.AddEvent(CEvent.WarehouseCountDown, GetWarehouseSortCountDown);
        UIEventListener.Get(mbtn_help).onClick = HelpClick;
        UIEventListener.Get(mbtn_sort).onClick = SortBtnClick;
        InitGrid();
        InitWrap();
    }

    public override void Show()
    {
        base.Show();
        Net.ReqGetStorehouseInfoMessage();
        if (!CSStorehouseInfo.Instance.CanSort)
        {
            msp_btnSort.spriteName = "btn_samll3";
            mbtn_sort.GetComponent<BoxCollider>().enabled = false;
            mlb_btnSort.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1309), CSStorehouseInfo.Instance.Sec);
        }
        else
        {
            msp_btnSort.spriteName = "btn_samll1";
            mbtn_sort.GetComponent<BoxCollider>().enabled = true;
            mlb_btnSort.text = ClientTipsTableManager.Instance.GetClientTipsContext(1310);
        }
    }

    protected override void OnDestroy()
    {
        curPageNum = 1;
        base.OnDestroy();
    }

    void InitGrid()
    {
        for (int i = 0; i < mwrap.transform.childCount; i++)
        {
            GameObject gridObj = mwrap.transform.GetChild(i).Find("UIGridItem").gameObject;
            itemsDic.Add(mwrap.transform.GetChild(i).gameObject, new WarehouseGridData(gridObj, OnClick, OnDoubleClick));
        }
    }
    void InitWrap()
    {
        mwrap.onInitializeItem = OnUpdateItem;
        mwrap.maxIndex = 4;
        mwrap.minIndex = 0;
    }
    GameObject updateObj;
    Dictionary<GameObject, int> realIndexDic = new Dictionary<GameObject, int>();
    void OnUpdateItem(GameObject _go, int _wrapindex, int _realindex)
    {
        //Debug.Log($"{_go}  {_wrapindex}  {_realindex}");
        updateObj = _go;
        if (realIndexDic.ContainsKey(_go))
        {
            realIndexDic[_go] = _realindex;
        }
        else
        {
            realIndexDic.Add(_go, _realindex);
        }
        itemsDic[_go].SetInfos(_realindex, _wrapindex);
    }
    void GetData(uint id, object data)
    {
        OnUpdateItem(mwrap.transform.GetChild(0).gameObject, 1, 0);
        OnUpdateItem(mwrap.transform.GetChild(1).gameObject, 2, 1);
        msc_Scroll.ResetPosition();
        RefreshPage();
        RefershCount();
        //RefreshItems();
    }
    void GetWarehouseChange(uint id, object data)
    {
        RefershCount();
        var iter = itemsDic.GetEnumerator();
        while (iter.MoveNext())
        {
            OnUpdateItem(iter.Current.Key, iter.Current.Value.wrapIndex, iter.Current.Value.realIndex);
        }
        //RefreshItems();
    }
    Vector2 vector;
    Vector3 startPos;
    void GetItemsDrag(uint id, object data)
    {
        if (data == null) return;
        ItemBaseDragPara eventData = (ItemBaseDragPara)data;
        if (eventData.mtype != PropItemType.Warehouse) { return; }
        vector = eventData.mvector;
        startPos = eventData.mstartPos;
        Vector3 vec = startPos;
        if (vector.x >= 205)
        {
            vec.x = vec.x + 420;
            curPageNum--;
            curPageNum = (curPageNum < 1) ? 1 : curPageNum;
        }
        else if (-205 >= vector.x)
        {
            vec.x = vec.x - 420;
            curPageNum++;
            curPageNum = (curPageNum > 5) ? 5 : curPageNum;
        }
        //Debug.Log($"{vec}");
        vec = new Vector3(-219 + -420 * (curPageNum - 1), -19, 0);
        mobj_mask.SetActive(true);
        SpringPanel.Begin(msc_Scroll.gameObject, vec, 40f).onFinished = ScrollFinish;
    }
    void ScrollFinish()
    {
        mobj_mask.SetActive(false);
        vector = Vector2.zero;
        RefreshPage();
    }
    void HelpClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Warehouse);
    }
    int srotCD = 0;
    void GetWarehouseSortCountDown(uint id, object data)
    {
        srotCD = CSStorehouseInfo.Instance.Sec;
        if (srotCD > 0)
        {
            msp_btnSort.spriteName = "btn_samll3";
            mbtn_sort.GetComponent<BoxCollider>().enabled = false;
            mlb_btnSort.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1309), CSStorehouseInfo.Instance.Sec);
        }
        else
        {
            msp_btnSort.spriteName = "btn_samll1";
            mbtn_sort.GetComponent<BoxCollider>().enabled = true;
            mlb_btnSort.text = ClientTipsTableManager.Instance.GetClientTipsContext(1310);
        }
    }
    void SortBtnClick(GameObject _go)
    {
        if (CSStorehouseInfo.Instance.CanSort)
        {
            Net.ReqSortStorehouseMessage();
            msp_btnSort.spriteName = "btn_samll3";
            mbtn_sort.GetComponent<BoxCollider>().enabled = false;
            mlb_btnSort.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1309), CSStorehouseInfo.Instance.Sec);
            CSStorehouseInfo.Instance.SortSchedule = Timer.Instance.InvokeRepeating(1f, 1f, CSStorehouseInfo.Instance.SortCountDown);
        }
    }

    void RefreshPage()
    {
        mlb_pageNum.text = $"<  {curPageNum}/{5}  >";
    }
    void RefershCount()
    {
        mlb_Maxcount.text = $"{CSStorehouseInfo.Instance.GetCurCount()}/{CSStorehouseInfo.Instance.GetMaxCount()}";
    }
    void OnClick(UIItemBase _item)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.WarehouseBag, _item.itemCfg, _item.infos);
    }
    void OnDoubleClick(UIItemBase _item)
    {
        Net.ReqStorehouseToBagMessage(_item.infos.bagIndex);
    }


    public class WarehouseGridData
    {
        public GameObject go;
        public List<UIItemBase> itemList = new List<UIItemBase>();
        public Dictionary<long, bag.BagItemInfo> infoList = new Dictionary<long, bag.BagItemInfo>();
        public Action<UIItemBase> onClick;
        PoolHandleManager mPoolHandleManager;
        Action<UIItemBase> onDoubleClick;
        Dictionary<int, bag.BagItemInfo> indexList = new Dictionary<int, bag.BagItemInfo>();
        public int startNum = 0;

        public int realIndex = 0;
        public int wrapIndex = 0;
        int curpageNum;
        public WarehouseGridData(GameObject obj, Action<UIItemBase> _onClick = null, Action<UIItemBase> _onDoubleClick = null)
        {
            mPoolHandleManager = new PoolHandleManager();
            go = obj;
            onClick = _onClick;
            onDoubleClick = _onDoubleClick;
            itemList = UIItemManager.Instance.GetUIItems(25, PropItemType.Warehouse, go.transform);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                itemList[i].HasCD = false;
                itemList[i].ChangeDragCom(false);
                itemList[i].ListenDrag();
                itemList[i].SetClick();
                itemList[i].SetItemDBClickedCB(onDoubleClick);
                itemList[i].SetMaskJudgeState(true);
            }
            go.GetComponent<UIGrid>().Reposition();
        }
        public void SetInfos(int _realIndex, int _wrapIndex)
        {
            realIndex = _realIndex;
            wrapIndex = _wrapIndex;

            int childIndex = 0;
            int startNum = 25 * (_realIndex) + 1;
            int endNum = startNum + 24;
            //Debug.Log(startNum + "  ------ " + endNum);
            for (int k = 0; k < 25; k++)
            {
                bag.BagItemInfo info = CSStorehouseInfo.Instance.GetIndexIsNil(startNum);
                childIndex = k;
                //Debug.Log(childIndex+"   "+ itemList.Count+"    "+startNum);
                if (info != null)
                {
                    //Debug.Log(startNum);
                    itemList[childIndex].SetMaskJudgeState(true);
                    itemList[childIndex].SetItemDBClickedCB(onDoubleClick);
                    itemList[childIndex].Refresh(info, onClick);
                }
                else
                {
                    itemList[childIndex].UnInit();
                    if (startNum > CSStorehouseInfo.Instance.GetMaxCount())
                    {
                        itemList[childIndex].ShowUnlock(startNum);
                    }
                }
                startNum++;
            }
        }
    }
}
