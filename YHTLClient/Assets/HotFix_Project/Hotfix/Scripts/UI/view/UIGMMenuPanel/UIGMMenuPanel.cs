using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class UIGMMenuPanel : UIBase
{
    private UIGridContainer _itemGrid;
    private UIGridContainer mItemGrid { get { return _itemGrid ?? (_itemGrid = Get<UIGridContainer>("UIQuerySystemPanel/content/Scroll View/grid/1/gridItems")); } }
    private UIGridContainer _itemGrid2;
    private UIGridContainer mItemGrid2 { get { return _itemGrid2 ?? (_itemGrid2 = Get<UIGridContainer>("UIQuerySystemPanel/content/Scroll View/grid/1/gridItems2")); } }
    private UIGridContainer _itemGrid3;
    private UIGridContainer mItemGrid3 { get { return _itemGrid3 ?? (_itemGrid3 = Get<UIGridContainer>("UIGMMessagePanel/Scroll View/grid")); } }
    private UIGridContainer _itemGrid4;
    private UIGridContainer mItemGrid4 { get { return _itemGrid4 ?? (_itemGrid4 = Get<UIGridContainer>("UIGMMessagePanel/type_scrollView/grid")); } }
    private UIGridContainer _itemGrid5;
    private UIGridContainer mItemGrid5 { get { return _itemGrid5 ?? (_itemGrid5 = Get<UIGridContainer>("UICustomPanel/type_scrollView (1)/grid")); } }
    private UIGridContainer _itemGrid6;
    private UIGridContainer mItemGrid6 { get { return _itemGrid6 ?? (_itemGrid6 = Get<UIGridContainer>("UICustomPanel/Scroll View (1)/grid")); } }
    private UIGridContainer _itemGrid7;
    private UIGridContainer mItemGrid7 { get { return _itemGrid7 ?? (_itemGrid7 = Get<UIGridContainer>("UICustomPanel/popupListBtn/bg/popupListPanel/Scroll View/grid")); } }
    
    private GameObject _typeScroll;
    private GameObject mTypeScroll { get { return _typeScroll ?? (_typeScroll = Get<GameObject>("UIGMMessagePanel/type_scrollView")); } }
    private UILabel _page;
    private UILabel mPage { get { return _page ?? (_page = Get<UILabel>("UIQuerySystemPanel/Label_page")); } }
    private GameObject _previous;
    private GameObject mPrevious { get { return _previous ?? (_previous = Get<GameObject>( "UIQuerySystemPanel/btn/page/previous")); } }
    private GameObject _next;
    private GameObject mNext { get { return _next ?? (_next = Get<GameObject>("UIQuerySystemPanel/btn/page/next")); } }

    private GameObject _search;
    private GameObject mSearch { get { return _search ?? (_search = Get<GameObject>( "UIQuerySystemPanel/btn/search")); } }

  

    private UIInput _searchInput;
    private UIInput mSearchInput { get { return _searchInput ?? (_searchInput = Get<UIInput>( "UIQuerySystemPanel/input_search")); } }

    private UILabel _warningLable;
    private UILabel mWarningLable { get { return _warningLable ?? (_warningLable = Get<UILabel>("UICustomPanel/warning")); } }
    private UIInput _gmNameInput;
    private UIInput mGmNameInput { get { return _gmNameInput ?? (_gmNameInput = Get<UIInput>( "UICustomPanel/input1")); } }
    private UIInput _parmInput1;
    private UIInput mParmInput1 { get { return _parmInput1 ?? (_parmInput1 = Get<UIInput>("UICustomPanel/input2")); } }
    private UIInput _parmInput2;
    private UIInput mParmInput2 { get { return _parmInput2 ?? (_parmInput2 = Get<UIInput>("UICustomPanel/input3")); } }
    private UIInput _parmInput3;
    private UIInput mParmInput3 { get { return _parmInput3 ?? (_parmInput3 = Get<UIInput>("UICustomPanel/input4")); } }
    private UIInput _parmInput4;
    private UIInput mParmInput4 { get { return _parmInput4 ?? (_parmInput4 = Get<UIInput>("UICustomPanel/input5")); } }
    private UIInput _btnNameInput;
    private UIInput mBtnNameInput { get { return _btnNameInput ?? (_btnNameInput = Get<UIInput>("UICustomPanel/btnName")); } }
    private UILabel _selectedLable;
    private UILabel mSelectedLable { get { return _selectedLable ?? (_selectedLable = Get<UILabel>("UICustomPanel/popupListBtn/Label")); } }
    private UILabel _selectedLable2;
    private UILabel mSelectedLable2 { get { return _selectedLable2 ?? (_selectedLable2 = Get<UILabel>("UICustomPanel/popupListBtn/Label2")); } }
    private UIInput _selected;
    private UIInput mSelected { get { return _selected ?? (_selected = Get<UIInput>("UICustomPanel/popupListBtn")); } }
    private GameObject _popupListBtn;
    private GameObject mPopupListBtn { get { return _popupListBtn ?? (_popupListBtn = Get<GameObject>("UICustomPanel/popupListBtn")); } }
    private GameObject _submit;
    private GameObject mSubmit { get { return _submit ?? (_submit = Get<GameObject>( "UICustomPanel/popupListBtn/bg/popupListPanel/submit")); } }
    private UIInput _popupInput;
    private UIInput mPopupInput { get { return _popupInput ?? (_popupInput = Get<UIInput>("UICustomPanel/popupListBtn/bg/popupListPanel/input")); } }
    private GameObject _btnCustomize2;
    private GameObject mBtnCustomize2 { get { return _btnCustomize2 ?? (_btnCustomize2 = Get<GameObject>("UICustomPanel/customize (1)")); } }

    private GameObject _UICustomPanel;
    private GameObject UICustomPanel { get { return _UICustomPanel ?? (_UICustomPanel = Get<GameObject>("UICustomPanel")); } }


    private int pageIndex;
    private int pageCount;

    private int itemCount;
    private int startIndex;
    private int gridMaxCount;

    private string popValue;
    private string inputValue;
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Hint; }
    }

    private UIGridContainer mItemGridContainer;
    private UIGridContainer ItemGridContainer { get { return mItemGridContainer ?? (mItemGridContainer = Get<UIGridContainer>("UIGMPanel/bg/Scroll View/goGrid")); } }

    private GameObject _menuClose;
    private GameObject mMenuClose { get { return _menuClose ?? (_menuClose = Get<GameObject>("menu/closePanel/close")); } }

    private UILabel _lb_time;
    private UILabel mlb_time { get { return _lb_time ?? (_lb_time = Get<UILabel>("timepanel/lb_time")); } }

    private UIToggle _to_itemid;
    private UIToggle to_itemid { get { return _to_itemid ?? (_to_itemid = Get<UIToggle>("UIQuerySystemPanel/btn/itemId")); } }
    private UIToggle _to_name;
    private UIToggle to_name { get { return _to_name ?? (_to_name = Get<UIToggle>("UIQuerySystemPanel/btn/name")); } }
    private UIToggle _to_type;
    private UIToggle to_type { get { return _to_type ?? (_to_type = Get<UIToggle>("UIQuerySystemPanel/btn/type")); } }


    private GameObject _menuSearch;
    private GameObject mMenuSearch { get { return _menuSearch ?? (_menuSearch = Get<GameObject>("menu/search")); } }

    private GameObject _Custom;
    private GameObject Custom { get { return _Custom ?? (_Custom = Get<GameObject>("menu/Custom")); } }

    private GameObject _menuMessage;
    private GameObject mmenuMessage { get { return _menuMessage ?? (_menuMessage = Get<GameObject>("menu/message")); } }

    //private TABLE.GM gm = null;
    //private Map<uint, TABLE.GM> gmMap = null;

    private GameObject _UIQuerySystemPanel;
    private GameObject mUIQuerySystemPanel { get { return _UIQuerySystemPanel ?? (_UIQuerySystemPanel = Get<GameObject>("UIQuerySystemPanel")); } }
    private GameObject _UIGMMessagePanel;
    private GameObject mUIGMMessagePanel { get { return _UIGMMessagePanel ?? (_UIGMMessagePanel = Get<GameObject>("UIGMMessagePanel")); } }
    private GameObject _btnMonster;
    private GameObject mBtnMonster { get { return _btnMonster ?? (_btnMonster = Get<GameObject>("UIQuerySystemPanel/btn/Scroll View/btn_monster")); } }
    private GameObject _btnItem;
    private GameObject mBtnItem { get { return _btnItem ?? (_btnItem = Get<GameObject>("UIQuerySystemPanel/btn/Scroll View/btn_item")); } }
    private GameObject _btnTask;
    private GameObject mBtnTask { get { return _btnTask ?? (_btnTask = Get<GameObject>("UIQuerySystemPanel/btn/Scroll View/btn_task")); } }
    private GameObject _btnTitle;
    private GameObject mBtnTitle { get { return _btnTitle ?? (_btnTitle = Get<GameObject>("UIQuerySystemPanel/btn/Scroll View/btn_title")); } }
    private GameObject _gmItem;
    private GameObject mGmItem { get { return _gmItem ?? (_gmItem = Get<GameObject>("UIGMMessagePanel/defaultPanel/gmItem")); } }
    private GameObject _defaultPanel;
    private GameObject mDefaultPanel { get { return _defaultPanel ?? (_defaultPanel = Get<GameObject>("UIGMMessagePanel/defaultPanel")); } }
    private GameObject _closeDefaultPanel;
    private GameObject mCloseDefaultPanel { get { return _closeDefaultPanel ?? (_closeDefaultPanel = Get<GameObject>("UIGMMessagePanel/defaultPanel/close")); } }
    private int curType;
    private FileStream file;
    //private string path = "C:\\customizeGM.txt";

    private  Dictionary<GameObject,GMItem> gmItemDic;

    public override void Init()
    {
        gmItemDic = new Dictionary<GameObject, GMItem>();
        base.Init();
        pageIndex = 1;
        gridMaxCount = 10;
        startIndex = 0;
        popValue = "itemId";
        inputValue = "";
        UIEventListener.Get(mPrevious).onClick = OnPreviousClick;
        UIEventListener.Get(mNext).onClick = OnNextClick;
        UIEventListener.Get(mSearch).onClick = OnSearchClick;
        UIEventListener.Get(mMenuClose).onClick = OnMenuCloseClick;

        UIEventListener.Get(mMenuSearch).onClick = SelectPanel;
        UIEventListener.Get(Custom).onClick = SelectPanel;
        UIEventListener.Get(mmenuMessage).onClick = SelectPanel;

        UIEventListener.Get(mBtnMonster).onClick = SelectSearchPanel;
        UIEventListener.Get(mBtnItem).onClick = SelectSearchPanel;
        UIEventListener.Get(mBtnTask).onClick = SelectSearchPanel;
        UIEventListener.Get(mBtnTitle).onClick = SelectSearchPanel;

        UIEventListener.Get(mPopupListBtn).onClick = OnPopupListClick;
        UIEventListener.Get(mSubmit).onClick = OnPopupListSubmitClick;
        UIEventListener.Get(mBtnCustomize2).onClick = OnCustomizeClick2;
        UIEventListener.Get(mCloseDefaultPanel).onClick = OnCloseDefaultPanel;

        UIEventListener.Get(to_itemid.gameObject,"itemId").onClick = OnChangeSearchType;
        UIEventListener.Get(to_name.gameObject,"name").onClick = OnChangeSearchType;
        UIEventListener.Get(to_type.gameObject,"type").onClick = OnChangeSearchType;

        curType = 0;
        mSelectedLable2.text = "";
    }

    public void OnCloseDefaultPanel(GameObject go)
    {
        if(mDefaultPanel.activeSelf)
        {
            mDefaultPanel.SetActive(false);
        }
    }
    public override void Show()
    {
        base.Show();
        SelectPanel(mmenuMessage);
        ScriptBinder.InvokeRepeating(0,1,TimeCount);
    }

    public void OnChangeSearchType(GameObject go)
    {
        popValue = (string)UIEventListener.Get(go).parameter;
    }

    private void SelectPanel(GameObject go)
    {
        mUIQuerySystemPanel.SetActive(go == mMenuSearch);
        mUIGMMessagePanel.SetActive(go == mmenuMessage);
        UICustomPanel.SetActive(go == Custom);
       
        if (UICustomPanel.activeSelf)
        {
            try
            {
                file = new FileStream("C:\\customizeGM.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch
            {
                UnityEngine.Debug.LogError("customizeGM.txt == 不能创建");
            }
            file.Close();
            curType = 0;
            clearPage();
            showCustomizeType();
            showCustomizeGmBtn();
            mItemGrid6.transform.parent.GetComponent<UIScrollView>().SetDragAmount(0, 0, true);
        }
        else if (mUIGMMessagePanel.activeSelf)
        {
            showMessage();
        }
        else if (mUIQuerySystemPanel.activeSelf)
        {
            curType = 0;
            clearPage();
            ShowItemsInfo(popValue, inputValue);
        }
    }
    public DateTime GetSeverTime
    {
        get
        {
            return CSServerTime.StampToDateTimeForSecond(CSServerTime.ServerNowMsChecked);
        }
    }
    private void TimeCount()
    {
        mlb_time.text = CSServerTime.ServernowTime > 0 ? "服务器时间：" + GetSeverTime.ToString("yyyy-MM-dd HH:mm:ss") : "";
    }

    public void ShowItemsInfo(string popValue, string inputValue)
    {
        TABLE.ITEM tbItem = null;
        List<TABLE.ITEM> listItem = new List<TABLE.ITEM>();
        Item item = null;
        searchBy<TABLE.ITEM>(popValue, inputValue, ItemTableManager.Instance.array.gItem.handles, tbItem, listItem);
        setPageCountAndGridMaxCount<TABLE.ITEM>(listItem, mItemGrid, 10);
        for (int i = startIndex; i < startIndex + mItemGrid.MaxCount; i++)
        {
            item = mPoolHandleManager.GetCustomClass<Item>();
            item.UIPrefab = mItemGrid.controlList[i - startIndex];
            tbItem = listItem[i];
            item.RefreshUI(tbItem);
            UIEventListener.Get(item.UIPrefabTrans.Find("icon").gameObject).onClick = OnItemClick;
        }
    }


    public void ShowMonsterItemsInfo(string popValue, string inputValue)
    {
        TABLE.MONSTERINFO tbMon = null;
        var dicMon = MonsterInfoTableManager.Instance.array.gItem.handles;
        List<TABLE.MONSTERINFO> listMon = new List<TABLE.MONSTERINFO>();
        bossItem item = null;
        searchBy<TABLE.MONSTERINFO>(popValue, inputValue, dicMon, tbMon, listMon);
        setPageCountAndGridMaxCount<TABLE.MONSTERINFO>(listMon, mItemGrid2, 7);
        for (int i = startIndex; i < startIndex + mItemGrid2.MaxCount; i++)
        {
            //item = mItemGrid2.controlList[i - startIndex].GetComponent<bossItem>();
            item = mPoolHandleManager.GetCustomClass<bossItem>();
            item.UIPrefab = mItemGrid2.controlList[i - startIndex];
            tbMon = listMon[i];
            item.RefreshUI(tbMon);
        }
    }
    public void ShowTaskItemsInfo(string popValue, string inputValue)
    {
        TABLE.TASKS tbTask = null;
        var dicTask = TasksTableManager.Instance.array.gItem.handles;
        List<TABLE.TASKS> listTask = new List<TABLE.TASKS>();
        bossItem item = null;
        searchBy<TABLE.TASKS>(popValue, inputValue, dicTask, tbTask, listTask);
        setPageCountAndGridMaxCount<TABLE.TASKS>(listTask, mItemGrid2, 7);
        for (int i = startIndex; i < startIndex + mItemGrid2.MaxCount; i++)
        {
            //item = mItemGrid2.controlList[i - startIndex].GetComponent<bossItem>();
            item = mPoolHandleManager.GetCustomClass<bossItem>();
            item.UIPrefab = mItemGrid2.controlList[i - startIndex];
            tbTask = listTask[i];
            item.RefreshUI(tbTask);
        }
    }

    public void ShowTitleItemsInfo(string popValue, string inputValue)
    {
        TABLE.TITLE tbTitle = null;
        var dicTitle = TitleTableManager.Instance.array.gItem.handles;
        List<TABLE.TITLE> listTitle = new List<TABLE.TITLE>();
        bossItem item = null;
        searchBy<TABLE.TITLE>(popValue, inputValue, dicTitle, tbTitle, listTitle);
        setPageCountAndGridMaxCount<TABLE.TITLE>(listTitle, mItemGrid2, 7);
        for (int i = startIndex; i < startIndex + mItemGrid2.MaxCount; i++)
        {
            //item = mItemGrid2.controlList[i - startIndex].GetComponent<bossItem>();
            item = mPoolHandleManager.GetCustomClass<bossItem>();
            item.UIPrefab = mItemGrid2.controlList[i - startIndex];
            tbTitle = listTitle[i];
            item.RefreshUI(tbTitle);
        }
    }

    private void setPageCountAndGridMaxCount<T>(List<T> list, UIGridContainer grid, int Count) {
        if (list.Count == 0)
        {
            pageCount = 0;
            pageIndex = 0;
            grid.MaxCount = 0;

        }
        else
        {
            pageCount = list.Count % Count == 0 ? list.Count / Count : list.Count / Count + 1;
            if (pageIndex == pageCount)
            {
                grid.MaxCount = list.Count % Count == 0 ? Count : list.Count % Count;
            }
            else
            {
                grid.MaxCount = Count;
            }
        }
        mPage.text = pageIndex + "/" + pageCount;
    }

    private void searchBy<T>(string popValue, string inputValue, TableHandle[] dic, T tb, List<T> list) where T : class,new()
    {
        switch (popValue)
        {
            case "name":
                for (int i = 0,max = dic.Length;i<max;++i)
                {
                    tb = dic[i].Value as T;
                    object name = "";
                    System.Type tType = tb.GetType();
                    PropertyInfo pname = tType.GetProperty("name");
                    if (pname != null)
                    {
                        MethodInfo mname = pname.GetGetMethod();
                        if (mname != null)
                        {
                            name = mname.Invoke(tb, null);
                        }
                    }

                    if (name == null)
                    {
                        if (tb is TABLE.ITEM item)
                        {
                            UtilityTips.ShowRedTips($"道具 id ： {item.id} name  为空，，尽快修改");
                            continue;
                        }
                    }
                    
                    if (((String)name).Contains(inputValue) || inputValue == "")
                    {
                        list.Add(tb);
                    }
                }
                break;
            case "itemId":
                for (int i = 0, max = dic.Length; i < max; ++i)
                {
                    tb = dic[i].Value as T;
                    object id = 0;
                    System.Type tType = tb.GetType();
                    PropertyInfo pid = tType.GetProperty("id");
                    if (pid != null)
                    {
                        MethodInfo mid = pid.GetGetMethod();
                        if (mid != null)
                        {
                            id = mid.Invoke(tb, null);
                        }
                    }
                    if (id.ToString() == inputValue || inputValue == "")
                    {
                        list.Add(tb);
                    }
                }
                break;
            case "type":
                for (int i = 0, max = dic.Length; i < max; ++i)
                {
                    tb = dic[i].Value as T;
                    object type = 0;
                    System.Type tType = tb.GetType();
                    PropertyInfo ptype = tType.GetProperty("type");
                    if (ptype != null)
                    {
                        MethodInfo mtype = ptype.GetGetMethod();
                        if (mtype != null)
                        {
                            type = mtype.Invoke(tb, null);
                        }
                    }
                    if (type.ToString() == inputValue || inputValue == "")
                    {
                        list.Add(tb);
                    }
                }
                break;
            default:
                break;
        }
    }
    public void OnPreviousClick(GameObject go) {
        if (pageIndex == 1) return;
        if (go == null) return;
        pageIndex--;
        if (curType == 0) { startIndex -= gridMaxCount; ShowItemsInfo(popValue, inputValue); }
        if (curType == 1) { startIndex -= 7; ShowMonsterItemsInfo(popValue, inputValue); }
        if (curType == 2) { startIndex -= 7; ShowTaskItemsInfo(popValue, inputValue); }
        if (curType == 3) { startIndex -= 7; ShowTitleItemsInfo(popValue, inputValue); }
    }
    public void OnNextClick(GameObject go) {
        if (pageIndex == pageCount) return;
        if (go == null) return;
        pageIndex++;
        if (curType == 0) { startIndex += gridMaxCount; ShowItemsInfo(popValue, inputValue); }
        if (curType == 1) { startIndex += 7; ShowMonsterItemsInfo(popValue, inputValue); }
        if (curType == 2) { startIndex += 7; ShowTaskItemsInfo(popValue, inputValue); }
        if (curType == 3) { startIndex += 7; ShowTitleItemsInfo(popValue, inputValue); }
    }

    public void OnSearchClick(GameObject go)
    {
        pageIndex = 1;
        startIndex = 0;
        inputValue = mSearchInput.value;
        if (curType == 0) { ShowItemsInfo(popValue, inputValue); }
        if (curType == 1) { ShowMonsterItemsInfo(popValue, inputValue); }
        if (curType == 2) { ShowTaskItemsInfo(popValue, inputValue); }
        if (curType == 3) { ShowTitleItemsInfo(popValue, inputValue); }
    }
    public void OnItemClick(GameObject go)
    {
        Transform i = go.transform.parent;
        string strId = i.Find("id").GetComponent<UILabel>().text;
        string strNum = i.Find("number").GetComponent<UIInput>().value;
        string strGm = "@i" + " " + strId + " " + strNum + " " + "0";
        Net.GMCommand(strGm);
    }

    private void OnMenuCloseClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIGMMenuPanel>();
    }
    private void SelectSearchPanel(GameObject go)
    {
        clearPage();
        if (go == mBtnItem)
        {
            curType = 0;
            ShowItemsInfo(popValue, inputValue);
        }
        else if (go== mBtnMonster)
        {
            curType = 1;
            ShowMonsterItemsInfo(popValue, inputValue);
        }
        else if (go == mBtnTask)
        {
            curType = 2;
            ShowTaskItemsInfo(popValue, inputValue);
        }
        else if (go == mBtnTitle)
        {
            curType = 3;
            ShowTitleItemsInfo(popValue, inputValue);
        }
        mItemGrid.gameObject.SetActive(curType == 0);
        mItemGrid2.gameObject.SetActive(curType == 1 || curType == 2 || curType == 3);
    }
    private void clearPage()
    {
        pageIndex = 1;
        startIndex = 0;
        popValue = "itemId";
        inputValue = "";
        mSearchInput.value = "";
    }
    private void showMessage()
    {
        showTypeMenu();
        showGmBtn();
    }
    private void OnMessageItemClick(GameObject go)
    {
        if (go == null) return;
        TABLE.GM tbl = (UIEventListener.Get(go).parameter as GMItem).tbl_gm;
        GMItem item =null;
        if(gmItemDic.ContainsKey(mGmItem))
            item = gmItemDic[mGmItem];
        else {
            item = mPoolHandleManager.GetCustomClass<GMItem>();
            item.UIPrefab = mGmItem;
            gmItemDic.Add(mGmItem, item);
        }
        if (item == null) return;

        if (tbl.parm.Split(' ').Length == 1)
        {
            string str = "@" + tbl.gm;
            Net.GMCommand(str);
            return;
        }
        item.Show(tbl);
        mDefaultPanel.SetActive(true);
    }
    private void OnCustomizeItemClick(GameObject go)
    {
        if (go == null) return;
        GMItem item = UIEventListener.Get(go).parameter as GMItem;
        string[] str = item.getMessage().Split(';');
        foreach (string s in str)
        {
            if (s != "")
            {
                Net.GMCommand(s);
            }
        }
    }

    private void showGmBtn()
    {
        TABLE.GM tbGM = null;
        List<TABLE.GM> listGM = new List<TABLE.GM>();
        GMItem item = null;
        var arr = GMTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            tbGM = arr[k].Value as TABLE.GM;
            listGM.Add(tbGM);
        }
        if (mItemGrid3 == null) return;
        mItemGrid3.MaxCount = listGM.Count;
        for (int i = 0; i < mItemGrid3.MaxCount; i++)
        {
            item = mPoolHandleManager.GetCustomClass<GMItem>();// mItemGrid3.controlList[i].GetComponent<GMItem>();
            item.UIPrefab = mItemGrid3.controlList[i];
            item.showGMMessage(listGM[i]);
            UIEventListener.Get(item.UIPrefabTrans.Find("btn").gameObject,item).onClick = OnMessageItemClick;
        }
    }
    private void showTypeMenu()
    {
        TABLE.GM tbGM = null;
        List<string> typeNameList = new List<string>();
        var arr = GMTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            tbGM = arr[k].Value as TABLE.GM;
            if (!typeNameList.Contains(tbGM.type) && tbGM.type != "")
            {
                typeNameList.Add(tbGM.type);
            }
        }
        mItemGrid4.MaxCount = typeNameList.Count;
        for (int i = 0; i < mItemGrid4.MaxCount; i++)
        {
            UILabel typeName = mItemGrid4.controlList[i].transform.Find("type_name").GetComponent<UILabel>();
            typeName.text = typeNameList[i];
            UIEventListener.Get(mItemGrid4.controlList[i]).onClick = showTypeContent;
        }
    }
    private void showTypeContent(GameObject go)
    {
        string typeName = go.transform.Find("type_name").GetComponent<UILabel>().text;
        TABLE.GM tbGM = null;
        List<TABLE.GM> listGM = new List<TABLE.GM>();
        GMItem item = null;
        var arr = GMTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            tbGM = arr[k].Value as TABLE.GM;
            if (tbGM.type == typeName)
            {
                listGM.Add(tbGM);
            }
        }
        mItemGrid3.MaxCount = listGM.Count;
        for (int i = 0; i < mItemGrid3.MaxCount; i++)
        {
            item = mPoolHandleManager.GetCustomClass<GMItem>();// mItemGrid3.controlList[i].GetComponent<GMItem>();
            item.UIPrefab = mItemGrid3.controlList[i];
            item.showGMMessage(listGM[i]);
            UIEventListener.Get(item.UIPrefabTrans.Find("btn").gameObject,item).onClick = OnMessageItemClick;
        }
    }

    private List<string> readFile(string fileName)
    {
        List<string> dataList = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != ""&&line.StartsWith("@"))
                    {
                        dataList.Add(line);
                    }  
                }
                sr.Close();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("The file could not be read:");
            UnityEngine.Debug.LogError(e.Message);
        }
        return dataList;
    }
    private void writeFile(string fileName, string data)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(data);
                sw.Close();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("The file could not be write:");
            UnityEngine.Debug.LogError(e.Message);
        }
    }

    public void showCustomizeType()
    {
        List<string> typesList = getUnduplicatedList(getCustomizeColumnsList(2));
        mItemGrid5.MaxCount = typesList.Count;
        for (int i = 0; i < mItemGrid5.MaxCount; i++)
        {
            UILabel typeName = mItemGrid5.controlList[i].transform.Find("type_name").GetComponent<UILabel>();
            typeName.text = typesList[i];
            UIEventListener.Get(mItemGrid5.controlList[i]).onClick = showCustomizeGmBtnByType;
        }
    }
    private void showCustomizeGmBtnByType(GameObject go)
    {
       
        UILabel typeName = go.transform.Find("type_name").GetComponent<UILabel>();
        List<string> dataList = readFile("C:\\customizeGM.txt");
        List<string> dataByTypeList = new List<string>();
        for (int i = 0; i < dataList.Count; i++)
        {
            string type = dataList[i].Split('|')[2];
            if (type == typeName.text)
            {
                dataByTypeList.Add(dataList[i]);
            }
        }
        mItemGrid6.MaxCount = dataByTypeList.Count;
        for (int i = 0; i < mItemGrid6.MaxCount; i++)
        {
            GMItem item = mPoolHandleManager.GetCustomClass<GMItem>();// mItemGrid6.controlList[i].GetComponent<GMItem>();
            item.UIPrefab = mItemGrid6.controlList[i];
            item.showGMMessage(dataByTypeList[i]);
            UIEventListener.Get(item.UIPrefabTrans.Find("btn").gameObject, item).onClick = OnCustomizeItemClick;
        }
        mItemGrid6.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        mItemGrid6.transform.parent.GetComponent<UIScrollView>().SetDragAmount(0, 0,true);
    }
    public void showCustomizeGmBtn()
    {
        List<string> dataList = readFile("C:\\customizeGM.txt");
        mItemGrid6.MaxCount = dataList.Count;
        for (int i = 0; i < mItemGrid6.MaxCount; i++)
        {
            GMItem item = mPoolHandleManager.GetCustomClass<GMItem>();// mItemGrid6.controlList[i].GetComponent<GMItem>();
            item.UIPrefab = mItemGrid6.controlList[i];
            item.showGMMessage(dataList[i]);
            UIEventListener.Get(item.UIPrefabTrans.Find("btn").gameObject, item).onClick = OnCustomizeItemClick;
        }

    }
    private List<string> getCustomizeColumnsList(int index)//gm|按钮名|分类名
    {
        List<string> dataList = readFile("C:\\customizeGM.txt");
        List<string> columnsList = new List<string>();
        foreach (string data in dataList)
        {
            string[] dataArray = data.Split('|');
            if (dataArray.Length > index)
            {
                string type = dataArray[index];
                columnsList.Add(type);
            }
        }
        return columnsList;

    }
    private List<string> getUnduplicatedList(List<string> list)//列表去重
    {
        List<string> unduplicatedList = new List<string>();
        foreach (string str in list)
        {
            if (!unduplicatedList.Contains(str))
            {
                unduplicatedList.Add(str);
            }
        }
        return unduplicatedList;
    }

    private void showCustomizeTypeInPopupList()
    {
        List<string> typesList = getUnduplicatedList(getCustomizeColumnsList(2));
        mItemGrid7.MaxCount = typesList.Count;
        for (int i = 0; i < mItemGrid7.MaxCount; i++)
        {
            UILabel typeName = mItemGrid7.controlList[i].transform.Find("type_name").GetComponent<UILabel>();
            typeName.text = typesList[i];
            UIEventListener.Get(mItemGrid7.controlList[i]).onClick = showSelectedType;
        }
    }
    private void showSelectedType(GameObject go)
    {
        UILabel typeName = go.transform.Find("type_name").GetComponent<UILabel>();
        mSelectedLable2.text = typeName.text;
        mSelectedLable.text = typeName.text;
        mPopupListBtn.transform.GetComponent<UIToggle>().value = false;//close
    }
    private void OnPopupListClick(GameObject go)
    {
        if (go.transform.Find("bg").GetComponent<UISprite>().isActiveAndEnabled)
        {
            showCustomizeTypeInPopupList();
        }
    }
    private void OnPopupListSubmitClick(GameObject go)
    {
        if (mPopupInput != null && mPopupInput.value.Trim() != "")
        {
            mSelectedLable.text = mPopupInput.value.Trim();
            mSelectedLable2.text = mPopupInput.value.Trim();
            mPopupListBtn.transform.GetComponent<UIToggle>().value = false;
            mPopupInput.value = "";
        }
    }
    private void OnCustomizeClick2(GameObject go)
    {
        if (mGmNameInput.value == "")
        {
            mWarningLable.text = "gm名字不可为空";
            mWarningLable.GetComponent<TweenAlpha>().PlayTween();
            return;
        }
        if (mBtnNameInput.value == "")
        {
            mWarningLable.text = "gm按钮名字不可为空";
            mWarningLable.GetComponent<TweenAlpha>().PlayTween();
            return;
        }
        if (getCustomizeColumnsList(1).Contains(mBtnNameInput.value))
        {
            mWarningLable.text = "gm按钮名字不可重复";
            mWarningLable.GetComponent<TweenAlpha>().PlayTween();
            return;
        }
        if (mSelectedLable2.text == "")
        {
            mWarningLable.text = "请选择分类";
            mWarningLable.GetComponent<TweenAlpha>().PlayTween();
            return;
        }
        string str = "@";
        if (mGmNameInput != null && mGmNameInput.value != "") str += mGmNameInput.value;
        if (mParmInput1 != null && mParmInput1.value != "") str += " " + mParmInput1.value;
        if (mParmInput2 != null && mParmInput2.value != "") str += " " + mParmInput2.value;
        if (mParmInput3 != null && mParmInput3.value != "") str += " " + mParmInput3.value;
        if (mParmInput4 != null && mParmInput4.value != "") str += " " + mParmInput4.value;
        if (mBtnNameInput != null && mBtnNameInput.value != "") str += ";|" + mBtnNameInput.value;
        if (mSelectedLable != null && mSelectedLable.text != "") str += "|" + mSelectedLable.text;
        writeFile("C:\\customizeGM.txt", str);
        showCustomizeType();
        showCustomizeGmBtn();
    }
}



