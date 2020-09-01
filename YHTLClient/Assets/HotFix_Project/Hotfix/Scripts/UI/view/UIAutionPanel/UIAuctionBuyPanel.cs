using auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIAuctionBuyPanel : UIBasePanel
{
    #region variable
    List<AuctionFirstTabs> firstitemList = new List<AuctionFirstTabs>();
    List<AuctionBuyItem> cuyItem = new List<AuctionBuyItem>();
    AuctionFirstTabs curFirstTab;
    AuctionSecondTabs curSecondTab;
    AuctionLimit sortLimit;
    AllAuctionItems buyMes;
    CSBetterLisHot<AuctionItemInfo> curData = new CSBetterLisHot<AuctionItemInfo>();
    List<AuctionItemInfo> curPageData = new List<AuctionItemInfo>();
    int curPageNum = 1;
    int totalPageNum = 1;
    SpringPanel panel;
    #endregion
    public static string[] FristTabs = { "全部", "角色装备", "卧龙装备", "怀旧装备", "其他" };
    public static string[] all = { "全部" };
    public static string[] benyuan = { "全部", "武器", "衣服", "头盔", "项链", "手镯", "戒指", "鞋子", "腰带", "勋章", "宝石" };
    public static string[] huaijiu = { "全部", "头盔", "戒指", "项链", "手镯" };
    public static string[] other = { "全部", "图鉴卡" };
    Dictionary<string, int> CareerDic = new Dictionary<string, int>()
    {
        {"全部",0},
        {"战士",1},
        {"法师",2},
        {"道士",3},
    };
    Dictionary<string, int[]> normalEquipDic = new Dictionary<string, int[]>()
    {
        {"全部",null},
        {"10级", new int[]{ 10,30} },
        {"30级",new int[]{ 31,50}},
        {"50级",new int[]{ 51,70}},
        {"70级",new int[]{ 71,90}},
        {"90级",new int[]{ 91,110}},
        {"110级",new int[]{ 111,120}},
        {"120级",new int[]{ 121,130}},
        {"130级",new int[]{ 131,140}},
        {"140级",new int[]{ 141,150}},
        {"150级",new int[]{ 151,160}},
        {"160级",new int[]{ 161,170}},
        {"170级",new int[]{ 171,180}},
        {"180级",new int[]{ 181,190}},
        {"190级",new int[]{ 190}},
    };
    Dictionary<string, int[]> wolongEquipDic = new Dictionary<string, int[]>()
    {
        {"全部",null},
        {"卧龙0级",new int[]{ 0,5}},
        {"卧龙6级", new int[]{ 6,10}},
        {"卧龙11级",new int[]{ 1,15}},
        {"卧龙16级",new int[]{ 16,20}},
        {"卧龙21级",new int[]{ 21,25}},
        {"卧龙26级",new int[]{ 26,30}},
        {"卧龙31级",new int[]{ 31,35}},
        {"卧龙36级",new int[]{ 36,40}},
    };
    public static string[] GetSecondMes(int _type)
    {
        if (_type == 0)
        {
            return all;
        }
        else if (_type == 1 || _type == 2)
        {
            return benyuan;
        }
        else if (_type == 3)
        {
            return huaijiu;
        }
        else
        {
            return other;
        }
    }
    public override void Init()
    {
        InitData();
        sortLimit = new AuctionLimit();
        sortLimit.priceUp = true;
        base.Init();
        mClientEvent.Reg((uint)CEvent.ECM_ResGetAuctionItemsMessage, GetSellMes);
        mClientEvent.Reg((uint)CEvent.ECM_ResBuyAuctionItemMessage, GetBuyMes);
        mClientEvent.Reg((uint)CEvent.AuctionBuyItemDrag, GetItemsDrag);
        mClientEvent.Reg((uint)CEvent.ResAttentionAuctionMessage, GetBuyMes);
        InitLimit();
        UIEventListener.Get(mobj_Scrdrag).onDrag = Drag;
        UIEventListener.Get(mobj_Scrdrag).onDragEnd = DragEnd;

        for (int i = 0; i < FristTabs.Length; i++)
        {
            firstitemList.Add(new AuctionFirstTabs(GameObject.Instantiate(mobj_mainTabs, mgrid_tabGrid.transform) as GameObject, i, FirstTabsClick, mobj_subTabs, SecondClick, 1));
        }
        mgrid_goodsPar.MaxCount = 8;
        for (int i = 0; i < 8; i++)
        {
            cuyItem.Add(new AuctionBuyItem(mgrid_goodsPar.controlList[i]));
        }
        UIEventListener.Get(mbtn_price).onClick = PriceBtnClick;
        UIEventListener.Get(mbtn_search).onClick = SearchBtnClick;
    }

    public override void Show()
    {
        curPageNum = 1;
        //Net.ReqGetAttentionAuctionMessage();
        Net.ReqGetAuctionItemsMessage();
        mgrid_tabGrid.Reposition();
        base.Show();
    }
    protected override void OnDestroy()
    {
        for (int i = 0; i < cuyItem.Count; i++)
        {
            cuyItem[i].UnInit();
        }
        curPageNum = 1;
        firstitemList = null;
        base.OnDestroy();
    }
    public override void SelectItem(TipsBtnData _data)
    {


    }
    public static void InitData()
    {


    }
    Vector2 vector;
    void Drag(GameObject _go, Vector2 vec)
    {
        if (vector == null) { vector = Vector2.zero; }
        vector = vector + vec;
    }
    void DragEnd(GameObject _go)
    {
        GetItemsDrag();
    }
    void GetItemsDrag(uint id, object data)
    {
        if (data == null) return;
        float x = (float)data;
        vector.x = x;
        GetItemsDrag();
    }
    void GetItemsDrag()
    {
        Vector3 vec = new Vector3(-410 * (curPageNum - 1), 167, 0);
        if (300 <= vector.x)
        {
            curPageNum--;
            curPageNum = (curPageNum < 1) ? 1 : curPageNum;
        }
        if (-300 >= vector.x)
        {
            curPageNum++;
            curPageNum = (curPageNum > totalPageNum) ? totalPageNum : curPageNum;

        }
        RefreshGoodsByLimit();
        mobj_shield.SetActive(true);
        SpringPanel.Begin(mscr_goodsScr.gameObject, new Vector3(-103, 125, 0), 40f).onFinished = ScrollFinish;
    }
    void ScrollFinish()
    {
        vector = Vector2.zero;
        mobj_shield.SetActive(false);
    }
    void PriceBtnClick(GameObject _go)
    {
        sortLimit.priceUp = !sortLimit.priceUp;
        mobj_arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (sortLimit.priceUp == true ? 180f : 0)));
        RefreshGoodsByLimit();
    }
    void SearchBtnClick(GameObject _go)
    {
        sortLimit.searchStr = minput_search.value;
        RefreshGoodsByLimit();
    }
    void GetSellMes(uint id, object data)
    {
        RefreshGoodsByLimit();
        FirstTabsClick(firstitemList[0]);
    }
    void GetBuyMes(uint id, object data)
    {
        RefreshGoodsByLimit();
    }
    void FirstTabsClick(AuctionFirstTabs _go)
    {
        if (_go == curFirstTab)
        {
            if (!curFirstTab.ReturnSelect())
            {
                curFirstTab.SetSelectState(true);
            }
            else
            {
                curFirstTab.SetSelectState(false);
            }
        }
        else
        {
            if (curFirstTab != null)
            {
                curFirstTab.SetSelect(false);
            }
            curFirstTab = _go;
            curFirstTab.SetSelect(true);
            sortLimit.Uninit();
            sortLimit.firstTabs = curFirstTab.index;
            sortLimit.secondTabs = 0;
            sortLimit.searchStr = "";
            minput_search.value = "";
            RefreshGoodsByLimit();
        }
        mgrid_tabGrid.Reposition();
        mscr_tabScroll.ResetPosition();
    }
    void SecondClick(AuctionSecondTabs _go)
    {
        if (curSecondTab != null)
        {
            curSecondTab.SetHighLight(false);
        }
        curSecondTab = _go;
        curSecondTab.SetHighLight(true);
        sortLimit.secondTabs = _go.index;
        sortLimit.searchStr = "";
        minput_search.value = "";
        RefreshGoodsByLimit();
    }
    void RefreshGoodsByLimit()
    {
        RefreshLimit();
        curData = UIAuctionInfo.Instance.GetGoodslistByLimit(sortLimit);
        //Debug.Log("筛选数量  " + curData.Count);
        if (curData.Count > 0)
        {
            int shang = curData.Count / 8;
            int yu = curData.Count % 8;
            totalPageNum = (yu > 0) ? shang + 1 : shang;
            if (totalPageNum == 0) { totalPageNum = 1; }
            if (curData.Count <= 8)
            {
                mscr_goodsScr.enabled = false;
                //mobj_shield.SetActive(true);
            }
            else
            {
                mscr_goodsScr.enabled = true;
                //mobj_shield.SetActive(false);
            }
        }
        else
        {
            curPageNum = 0;
            totalPageNum = 0;
            mscr_goodsScr.enabled = false;
            //mobj_shield.SetActive(true);
        }
        GetCurrentPageData();
    }
    int startInd = 0;
    int endInd = 0;
    void GetCurrentPageData()
    {
        if (totalPageNum == 0)
        {
            mobj_noGoods.SetActive(true);
            for (int i = 0; i < cuyItem.Count; i++)
            {
                cuyItem[i].UnRefresh();
            }
            return;
        }
        List<long> attenList = new List<long>();
        attenList = UIAuctionInfo.Instance.GetAttentionList();
        curPageData.Clear();
        curPageNum = (curPageNum <= 1) ? 1 : curPageNum;
        startInd = (curPageNum - 1) * 8;
        endInd = startInd + 7;
        //Debug.Log(curData.Count + "   " + curPageNum + "   " + totalPageNum + "   " + startInd);
        if (curData.Count < startInd)
        {
            return;
        }
        for (int i = startInd; i <= endInd; i++)
        {
            if (i < curData.Count)
            {
                curPageData.Add(curData[i]);
            }
        }
        mobj_noGoods.SetActive(curPageData.Count == 0 ? true : false);
        for (int i = 0; i < cuyItem.Count; i++)
        {
            if (i < curPageData.Count)
            {
                cuyItem[i].Refresh(curPageData[i], attenList.Contains(curPageData[i].lid));
            }
            else
            {
                cuyItem[i].UnRefresh();
            }
        }
        mlb_page.text = $"<{curPageNum}/{(totalPageNum == 0 ? 1 : totalPageNum)}>";
    }
    void InitLimit()
    {
        SetPoplist(CareerDic, mgrid_limitCareer.GetComponent<UIGridContainer>());
        SetPoplist(normalEquipDic, mgrid_limitNormalLv.GetComponent<UIGridContainer>());
        SetPoplist(wolongEquipDic, mgrid_limitWolongLv.GetComponent<UIGridContainer>());
        UIEventListener.Get(mobj_limitCareer, mgrid_limitCareer).onClick = OnPopClick;
        UIEventListener.Get(mobj_lvNormalEquip, mgrid_limitNormalLv).onClick = OnPopClick;
        UIEventListener.Get(mobj_lvWoLongEquip, mgrid_limitWolongLv).onClick = OnPopClick;
    }
    void RefreshLimit()
    {
        if (sortLimit.firstTabs == 1 || sortLimit.firstTabs == 2)
        {
            mobj_limitCareer.SetActive(true);
        }
        else
        {
            mobj_limitCareer.SetActive(false);
        }
        mobj_lvNormalEquip.SetActive((sortLimit.firstTabs == 1) ? true : false);
        mobj_lvWoLongEquip.SetActive((sortLimit.firstTabs == 2) ? true : false);
        mgrid_limitPar.Reposition();
    }
    void SetPoplist(Dictionary<string, int> dic, UIGridContainer grid)
    {
        grid.MaxCount = dic.Count;
        int index = 0;
        for (var it = dic.GetEnumerator(); it.MoveNext();)
        {
            GameObject temp = grid.controlList[index];
            UILabel name = temp.transform.Find("name").GetComponent<UILabel>();
            name.text = it.Current.Key;
            UIEventListener.Get(temp).onClick = OnPoplistClick;
            UIEventListener.Get(temp).parameter = dic;
            index++;
        }
        UISprite bg = grid.transform.Find("bg").GetComponent<UISprite>();
        bg.height = (int)grid.CellHeight * (grid.MaxCount % grid.MaxPerLine > 0 ? grid.MaxCount / grid.MaxPerLine + 1 : grid.MaxCount / grid.MaxPerLine) + 5;
        bg.width = 102 * grid.MaxPerLine + 10 + 3 * (grid.MaxPerLine - 1);
        GameObject bgContainer = grid.transform.Find("bg/shield").gameObject;
        UIEventListener.Get(bgContainer, grid.gameObject).onClick = OnPopClick;
    }
    void SetPoplist(Dictionary<string, int[]> dic, UIGridContainer grid)
    {
        grid.MaxCount = dic.Count;
        int index = 0;
        for (var it = dic.GetEnumerator(); it.MoveNext();)
        {
            GameObject temp = grid.controlList[index];
            UILabel name = temp.transform.Find("name").GetComponent<UILabel>();
            name.text = it.Current.Key;
            UIEventListener.Get(temp).onClick = OnPoplistClick;
            UIEventListener.Get(temp).parameter = dic;
            index++;
        }
        UISprite bg = grid.transform.Find("bg").GetComponent<UISprite>();
        bg.height = (int)grid.CellHeight * (grid.MaxCount % grid.MaxPerLine > 0 ? grid.MaxCount / grid.MaxPerLine + 1 : grid.MaxCount / grid.MaxPerLine) + 5;
        bg.width = 102 * grid.MaxPerLine + 10 + 3 * (grid.MaxPerLine - 1);

        GameObject bgContainer = grid.transform.Find("bg/shield").gameObject;
        UIEventListener.Get(bgContainer, grid.gameObject).onClick = OnPopClick;
    }
    private void OnPopClick(GameObject go)
    {
        GameObject obj = (GameObject)UIEventListener.Get(go).parameter;
        obj.SetActive(!obj.activeSelf);
    }
    private void OnPoplistClick(GameObject go)
    {
        string Key = go.transform.Find("name").GetComponent<UILabel>().text;
        UILabel value = go.transform.parent.parent.Find("value").GetComponent<UILabel>();

        value.text = Key;
        if (UIEventListener.Get(go).parameter is Dictionary<string, int> dic1)
        {
            Dictionary<string, int> dic = UIEventListener.Get(go).parameter as Dictionary<string, int>;

            if (dic == CareerDic)
            {
                sortLimit.career = dic[Key];
                RefreshGoodsByLimit();
            }
        }
        if (UIEventListener.Get(go).parameter is Dictionary<string, int[]> dic2)
        {
            Dictionary<string, int[]> dic = UIEventListener.Get(go).parameter as Dictionary<string, int[]>;
            if (dic == normalEquipDic)
            {
                sortLimit.level = dic[Key];
                RefreshGoodsByLimit();
            }
            else if (dic == wolongEquipDic)
            {
                sortLimit.lvClass = dic[Key];
                RefreshGoodsByLimit();
            }
        }
        go.transform.parent.gameObject.SetActive(false);
    }
    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
    }
    public override void OnHide()
    {
        base.OnHide();
    }
}

public class AuctionFirstTabs
{
    public GameObject go;
    UIGrid table;
    UILabel name;
    GameObject obj_highlight;
    UILabel nameHl;
    GameObject arrow;
    bool IsSelected = false;
    Action<AuctionFirstTabs> action;
    Action<AuctionSecondTabs> Childaction;
    public List<AuctionSecondTabs> list = new List<AuctionSecondTabs>();
    public int index = 0;
    string[] secondMes;
    GameObject secondGo;
    public AuctionFirstTabs(GameObject _go, int _index, Action<AuctionFirstTabs> _action, GameObject _secondGo, Action<AuctionSecondTabs> _cAction, int type)
    {
        go = _go;
        index = _index;
        secondGo = _secondGo;
        action = _action;
        Childaction = _cAction;
        table = go.transform.Find("Table").GetComponent<UIGrid>();
        arrow = go.transform.Find("arrow").gameObject;
        obj_highlight = go.transform.Find("checkmark").gameObject;
        name = go.transform.Find("name").GetComponent<UILabel>();
        nameHl = go.transform.Find("checkmark/name").GetComponent<UILabel>();
        if (type == 2)
        {
            name.text = UIAuctionFocusBuyPanel.FristTabs[index];
            nameHl.text = UIAuctionFocusBuyPanel.FristTabs[index];
            secondMes = UIAuctionFocusBuyPanel.GetSecondMes(index);

        }
        else
        {
            name.text = UIAuctionBuyPanel.FristTabs[index];
            nameHl.text = UIAuctionBuyPanel.FristTabs[index];
            secondMes = UIAuctionBuyPanel.GetSecondMes(index);

        }

        UIEventListener.Get(go).onClick = Click;

        for (int i = 0; i < secondMes.Length; i++)
        {
            list.Add(new AuctionSecondTabs(GameObject.Instantiate(secondGo, table.transform) as GameObject, i, secondMes[i], Childaction));
        }
        table.Reposition();
        SetSelect(false);
    }
    public void SetSelect(bool _state)
    {
        IsSelected = _state;
        obj_highlight.SetActive(IsSelected);
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (IsSelected == true ? 180f : 0)));
        table.gameObject.SetActive(IsSelected);
        if (IsSelected)
        {
            table.Reposition();
            Childaction(list[0]);
        }
    }
    public void SetSelectState(bool _state)
    {
        IsSelected = _state;
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (IsSelected == true ? 180f : 0)));
        table.gameObject.SetActive(IsSelected);
        if (IsSelected)
        {
            table.Reposition();
        }
    }
    public bool ReturnSelect()
    {
        return IsSelected;
    }
    public void GetSecondItems()
    {

    }
    void Click(GameObject _go)
    {
        if (action != null) { action(this); }
    }
    void SecondClick(AuctionSecondTabs _go)
    {
        FNDebug.Log(_go.name.text);
    }
}
public class AuctionSecondTabs
{
    public GameObject go;
    public UILabel name;
    public UILabel lb_Hlname;
    GameObject obj_Hl;
    Action<AuctionSecondTabs> action;
    public int index;
    bool state = false;
    public AuctionSecondTabs(GameObject _go, int _index, string _str, Action<AuctionSecondTabs> _action)
    {
        go = _go;
        name = go.transform.Find("name").GetComponent<UILabel>();
        obj_Hl = go.transform.Find("subCheckmark").gameObject;
        lb_Hlname = go.transform.Find("subCheckmark/name").GetComponent<UILabel>();
        lb_Hlname.text = _str;
        name.text = _str;
        action = _action;
        index = _index;
        UIEventListener.Get(go).onClick = Click;
    }

    void Click(GameObject _go)
    {
        if (action != null) { action(this); }
    }
    public void SetHighLight(bool _state)
    {
        state = _state;
        obj_Hl.SetActive(state);
    }
    public bool ReturnHighLight()
    {
        return state;
    }
}
public class AuctionBuyItem
{
    public GameObject go;
    public UILabel lb_price;
    public UISprite priceIcon;
    public UILabel lb_time;
    public UILabel lb_name;
    public UILabel lb_state;
    public GameObject itemPar;
    public GameObject bg;
    public UIItemBase item;
    public AuctionItemInfo info;
    //bool state = false;
    Vector2 vector = Vector2.zero;
    Vector3 startPos = Vector3.zero;
    bool isAtten = false;
    public AuctionBuyItem(GameObject _go)
    {
        go = _go;
        lb_time = go.transform.Find("time").GetComponent<UILabel>();
        lb_name = go.transform.Find("name").GetComponent<UILabel>();
        lb_price = go.transform.Find("Price").GetComponent<UILabel>();
        lb_state = go.transform.Find("Attentionstate").GetComponent<UILabel>();
        itemPar = go.transform.Find("itemPar").gameObject;
        bg = go.transform.Find("bg").gameObject;
        priceIcon = go.transform.Find("Price/ingotSymbol").GetComponent<UISprite>();
        UIEventListener.Get(bg).onClick = Click;
        UIEventListener.Get(bg).onDrag = Drag;
        UIEventListener.Get(bg).onDragEnd = DragEnd;
    }
    void Click(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
        {
            (p as UIAuctionOperationPanel).ShowState(AuctionOperation.Buy, info, isAtten);
        });
    }
    void Drag(GameObject _go, Vector2 vec)
    {
        if (vector == null) { vector = Vector2.zero; }
        vector = vector + vec;
    }
    void DragEnd(GameObject _go)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.AuctionBuyItemDrag, vector.x);
        vector = Vector2.zero;
    }
    public void Refresh(AuctionItemInfo _sellinfo = null, bool _isAtten = false)
    {
        isAtten = _isAtten;
        go.SetActive(true);
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar.transform, itemSize.Size64);
            item.SetMaskJudgeState(true);
        }
        info = _sellinfo;
        int itemCfgId = 0;
        if (info.itemType == 1 || info.itemType == 3)
        {
            item.Refresh(info.item);
            item.SetExtendData(info);
            itemCfgId = info.item.configId;
            lb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemCfgId));
        }
        else
        {
            itemCfgId = HandBookTableManager.Instance.GetHandBookItemID(info.tujianItem.handBookId);
            item.Refresh(itemCfgId, ShowHandbook, false);
            item.SetExtendData(info);
            int qua = (int)((info.tujianItem.handBookId << 0) >> 24);
            lb_name.color = UtilityCsColor.Instance.GetColor(qua);
            item.SetQuality(qua);
        }
        lb_name.text = ItemTableManager.Instance.GetItemName(itemCfgId);
        lb_price.text = info.price.ToString();
        priceIcon.spriteName = (info.priceType == 1) ? $"tubiao{1}" : $"tubiao{3}";
        lb_state.text = isAtten == true ? "已关注" : "";
    }
    void ShowHandbook(UIItemBase _item)
    {
        AuctionItemInfo info = (AuctionItemInfo)_item.ExtendData;
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(info.tujianItem.handBookId, info.tujianItem.id, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
        });
    }
    public void UnRefresh()
    {
        if (item != null)
        {
            item.UnInit();
        }
        lb_name.text = "";
        lb_price.text = "";
        priceIcon.spriteName = "";
        go.SetActive(false);
    }
    public void UnInit()
    {
        if (item != null)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
        go = null;
        lb_time = null;
        lb_name = null;
        lb_price = null;
    }
}




