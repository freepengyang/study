using auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIAuctionFocusBuyPanel : UIBasePanel
{
    #region variable
    List<AuctionFirstTabs> firstitemList = new List<AuctionFirstTabs>();
    List<AuctionSecondTabs> seconditenList = new List<AuctionSecondTabs>();
    List<AuctionFoucBuyItem> cuyItem = new List<AuctionFoucBuyItem>();
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

    public static string[] FristTabs = { "关注", "全部", "珍品本元装备", "珍品卧龙装备", "其他" };
    public static string[] all = { "全部" };
    public static string[] benyuan = { "全部", "武器", "衣服", "头盔", "项链", "手镯", "戒指", "鞋子", "腰带", "勋章", "宝石" };
    public static string[] other = { "全部", "祝福油", "技能书" };
    Dictionary<string, int> CareerDic = new Dictionary<string, int>()
    {
        {"全部",0},
        {"战士",1},
        {"法师",2},
        {"道士",3},
    };
    Dictionary<string, int> normalEquipDic = new Dictionary<string, int>()
    {
        {"全部",0},
        {"一阶",1},
        {"二阶",2},
        {"三阶",3},
        {"四阶",4},
        {"五阶",5},
        {"六阶",6},
        {"七阶",7},
        {"八阶",8},
        {"九阶",9},
        {"十阶",10},
        {"十一阶",11},
        {"十二阶",12},
        {"十三阶",13},
        {"十四阶",14},
    };
    Dictionary<string, int> wolongEquipDic = new Dictionary<string, int>()
    {
        {"全部",0},
        {"一阶",1},
        {"二阶",2},
        {"三阶",3},
        {"四阶",4},
        {"五阶",5},
        {"六阶",6},
        {"七阶",7},
        {"八阶",8},
    };
    public static string[] GetSecondMes(int _type)
    {
        if (_type == 0 || _type == 1)
        {
            return all;
        }
        else if (_type == 2 || _type == 3)
        {
            return benyuan;
        }
        else
        {
            return other;
        }
    }
    public override void Init()
    {
        Net.ReqGetAttentionAuctionMessage();
        sortLimit = new AuctionLimit();
        sortLimit.priceUp = true;
        base.Init();
        mClientEvent.Reg((uint)CEvent.ResAttentionAuctionMessage, GetSellMes);
        //mClientEvent.Reg((uint)CEvent.AuctionBuyItemDrag, GetItemsDrag);
        InitLimit();
        for (int i = 0; i < FristTabs.Length; i++)
        {
            firstitemList.Add(new AuctionFirstTabs(GameObject.Instantiate(mobj_mainTabs, mgrid_tabGrid.transform) as GameObject, i, FirstTabsClick, mobj_subTabs, SecondClick, 2));
        }
        mgrid_goodsPar.MaxCount = 8;
        for (int i = 0; i < 8; i++)
        {
            cuyItem.Add(new AuctionFoucBuyItem(mgrid_goodsPar.controlList[i]));
        }
        UIEventListener.Get(mbtn_price).onClick = PriceBtnClick;
        UIEventListener.Get(mbtn_search).onClick = SearchBtnClick;
    }

    public override void Show()
    {
        curPageNum = 1;
        mgrid_tabGrid.Reposition();
        base.Show();
    }
    protected override void OnDestroy()
    {
        curPageNum = 1;
        firstitemList = null;
        seconditenList = null;
        base.OnDestroy();
    }

    void GetItemsDrag(uint id, object data)
    {
        if (data == null) return;
        EventData eventData = (EventData)data;
        Vector2 vec = (Vector2)eventData.arg1;
        Vector3 vec3 = (Vector3)eventData.arg2;
        if (300 <= vec.x)
        {
            curPageNum--;
            curPageNum = (curPageNum < 1) ? 1 : curPageNum;
        }
        if (-300 >= vec.x)
        {
            curPageNum++;
            curPageNum = (curPageNum > totalPageNum) ? totalPageNum : curPageNum;

        }
        RefreshGoodsByLimit();

        if (panel == null)
        {
            panel = mscr_goodsScr.GetComponent<SpringPanel>();
            if (panel != null)
            {
                panel.onFinished = () =>
                {
                    mscr_goodsScr.ResetPosition();
                };
            }
        }
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
            //curFirstTab.SetSelect(!curFirstTab.ReturnSelect());
        }
        else
        {
            if (curFirstTab != null)
            {
                for (int i = 0; i < curFirstTab.list.Count; i++)
                {
                    seconditenList.Add(curFirstTab.list[i]);
                }
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
    }
    Schedule schedule;

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
        curData = UIAuctionInfo.Instance.GetAttentionGoodslistByLimit(sortLimit);
        //Debug.Log("筛选数量  " + curData.Count);
        if (curData.Count > 0)
        {
            if (totalPageNum % 8 == 0)
            {
                totalPageNum = curData.Count / 8;
            }
            else
            {
                totalPageNum = curData.Count / 8 + 1;
            }
        }
        else
        {
            //curPageNum = 0;
            totalPageNum = 0;
        }
        GetCurrentPageData();
    }
    int startInd = 0;
    int endInd = 0;
    void GetCurrentPageData()
    {
        List<long> attenList = new List<long>();
        attenList = UIAuctionInfo.Instance.GetAttentionList();
        curPageData.Clear();
        curPageNum = (curPageNum <= 1) ? 1 : curPageNum;
        startInd = (curPageNum - 1) * 8;
        endInd = startInd + 7;
        //Debug.Log(curPageNum + "   " + totalPageNum + "   " + startInd);
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
        //Debug.Log(curPageNum+"   "+ totalPageNum +"  count-- "+ curPageData.Count);
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
        if (sortLimit.firstTabs == 5 || sortLimit.firstTabs == 2 || sortLimit.firstTabs == 3 || sortLimit.firstTabs == 4
            || (sortLimit.firstTabs == 6 && sortLimit.secondTabs == 2))
        {
            mobj_limitCareer.SetActive(true);
        }
        else
        {
            mobj_limitCareer.SetActive(false);
        }
        mobj_lvNormalEquip.SetActive((sortLimit.firstTabs == 2 || sortLimit.firstTabs == 4) ? true : false);
        mobj_lvWoLongEquip.SetActive((sortLimit.firstTabs == 3 || sortLimit.firstTabs == 5) ? true : false);
        mgrid_limitPar.Reposition();
    }
    void SetPoplist(Dictionary<string, int> dic, UIGridContainer grid)
    {
        grid.MaxCount = dic.Count;
        int index = 0;
        var iter = dic.GetEnumerator();
        while (iter.MoveNext())
        {
            GameObject temp = grid.controlList[index];
            UILabel name = temp.transform.Find("name").GetComponent<UILabel>();
            name.text = iter.Current.Key;
            UIEventListener.Get(temp).onClick = OnPoplistClick;
            UIEventListener.Get(temp).parameter = dic;
            index++;
        }
        UISprite bg = grid.transform.Find("bg").GetComponent<UISprite>();
        bg.height = (int)grid.CellHeight * (grid.MaxCount % grid.MaxPerLine > 0 ? grid.MaxCount / grid.MaxPerLine + 1 : grid.MaxCount / grid.MaxPerLine) + 15;
        bg.width = (int)grid.CellWidth * grid.MaxPerLine + 15;
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
        //string Key = go.transform.Find("name").GetComponent<UILabel>().text;
        //UILabel value = go.transform.parent.parent.Find("value").GetComponent<UILabel>();
        //Dictionary<string, int> dic = UIEventListener.Get(go).parameter as Dictionary<string, int>;

        //value.text = Key;

        //if (dic == CareerDic)
        //{
        //    sortLimit.career = dic[Key];
        //    RefreshGoodsByLimit();
        //}
        //else if (dic == normalEquipDic)
        //{
        //    sortLimit.lvClass = dic[Key];
        //    RefreshGoodsByLimit();
        //}
        //else if (dic == wolongEquipDic)
        //{
        //    sortLimit.lvClass = dic[Key];
        //    RefreshGoodsByLimit();
        //}

        //go.transform.parent.gameObject.SetActive(false);
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
public class AuctionFoucBuyItem
{
    public GameObject go;
    public UILabel lb_price;
    public UISprite priceIcon;
    public UILabel lb_time;
    public UILabel lb_name;
    public UILabel lb_state;
    public GameObject itemPar;
    public UIItemBase item;
    public AuctionItemInfo info;
    //bool state = false;
    Vector2 vector;
    Vector3 startPos;
    bool isAtten = false;
    public AuctionFoucBuyItem(GameObject _go)
    {
        go = _go;
        lb_time = go.transform.Find("time").GetComponent<UILabel>();
        lb_name = go.transform.Find("name").GetComponent<UILabel>();
        lb_price = go.transform.Find("Price").GetComponent<UILabel>();
        lb_state = go.transform.Find("Attentionstate").GetComponent<UILabel>();
        itemPar = go.transform.Find("itemPar").gameObject;
        priceIcon = go.transform.Find("Price/ingotSymbol").GetComponent<UISprite>();
        UIEventListener.Get(go).onClick = Click;
        UIEventListener.Get(go).onDragStart = DragStart;
        UIEventListener.Get(go).onDrag = Drag;
        UIEventListener.Get(go).onDragEnd = DragEnd;
    }
    void Click(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
        {
            (p as UIAuctionOperationPanel).ShowState(AuctionOperation.Show, info, isAtten);
        });
    }
    void DragStart(GameObject _go)
    {
        startPos = go.transform.parent.parent.parent.parent.transform.localPosition;
        //HotManager.Instance.EventHandler.SendEvent(CEvent.AuctionBuyItemDrag);
    }
    void Drag(GameObject _go, Vector2 vec)
    {
        if (vector == null) { vector = Vector2.zero; }
        vector = vector + vec;
    }
    void DragEnd(GameObject _go)
    {
        EventData data = CSEventObjectManager.Instance.SetValue(vector, startPos);
        HotManager.Instance.EventHandler.SendEvent(CEvent.AuctionBuyItemDrag, data);
        CSEventObjectManager.Instance.Recycle(data);
        vector = Vector2.zero;
    }
    public void Refresh(AuctionItemInfo _sellinfo = null, bool _isAtten = false)
    {
        isAtten = _isAtten;
        go.SetActive(true);
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar.transform);
        }
        info = _sellinfo;
        FNDebug.Log(info.addTime + "   " + info.showTime);
        item.Refresh(info.item);
        lb_name.text = ItemTableManager.Instance.GetItemName(info.item.configId);
        lb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(info.item.configId));
        lb_price.text = info.price.ToString();
        priceIcon.spriteName = (info.priceType == 1) ? "tubiao10003" : "tubiao10001";
        lb_state.text = isAtten == true ? "已关注" : "";
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