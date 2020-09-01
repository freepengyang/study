using System;
using System.Collections.Generic;
using TABLE;
using UnityEngine;
using pet;
using bag;
using Google.Protobuf.Collections;


public partial class UIPetLevelUpPanel : UIBasePanel
{

    EndLessKeepHandleList<UIPetLevelUpItem, PetLevelUpItemData> endLessList;
    private ResPetInfo _resPetInfo;
    private List<MastItemClass> MastItemClasses;
    private List<LuckItemClass> LuckItemClasses;
    private List<UIItemBase> UIItemBaseList;
    private List<ItemDaily> mustitemList;
    private ILBetterList<ItemDaily> luckItemList;
    private List<int> listItemNum;
    private Vector3 mastitemsVec;
    private Vector3 orageVec;
    private float scrollequipx;
    private int curPage;
    private int curExpMax;
    ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();
    public System.Action CloseAction; //关闭面板时调用
    private CSPetLevelUpInfo csinfo;

    private int ShowNum = 2; //promp显示的数量
    
    //private functionopen wolongFunclevel;
    
    //UIPetLevelUpSetPanel _uiPetLevelUpSetPanel;
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
        AddCollider();
        csinfo = CSPetLevelUpInfo.Instance;
        endLessList = new EndLessKeepHandleList<UIPetLevelUpItem, PetLevelUpItemData>
            (SortType.Horizen, mwrap_page, mPoolHandleManager, 2, ScriptBinder);
        mustitemList = mPoolHandleManager.GetSystemClass<List<ItemDaily>>();
        luckItemList = mPoolHandleManager.GetSystemClass<ILBetterList<ItemDaily>>();
        UIItemBaseList = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();
        LuckItemClasses = mPoolHandleManager.GetSystemClass<List<LuckItemClass>>();
        MastItemClasses = mPoolHandleManager.GetSystemClass<List<MastItemClass>>();
        listItemNum = mPoolHandleManager.GetSystemClass<List<int>>();
        LuckItemClasses.Clear();
        MastItemClasses.Clear();
        UIItemBaseList.Clear();
        mustitemList.Clear();
        luckItemList.Clear();
        listItemNum.Clear();
        mastitemsVec = new Vector3(0, 160, 0);
        orageVec = mgrild_mastitems.transform.localPosition;
        mClientEvent.AddEvent(CEvent.CSPetInfoMessage, RefreshPetInfo);
        mClientEvent.AddEvent(CEvent.OnRecycleItemChange, OnItemChange);
        mClientEvent.AddEvent(CEvent.SelectEquip, RefreshEquipSelect);
        mClientEvent.AddEvent(CEvent.ConfigCallback, OnConfigCallback);
        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, OnClose);
        mClientEvent.AddEvent(CEvent.FastAccessTransferNpc, OnClose);

        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_config).onClick = OnConfigClick;
        UIEventListener.Get(mbtn_quesition).onClick = OnQuesitionClick;
        UIEventListener.Get(mbtn_benyuan).onClick = OnChangeMode;
        UIEventListener.Get(mbtn_wolong).onClick = OnChangeMode;
        UIEventListener.Get(mbtn_recycle).onClick = OnRecycleClick;
        UIEventListener.Get(mlb_explanation).onClick = OnExplanation;

        mscollbar.onChange.Add(new EventDelegate(OnChangePage));
        scrollequipx = mScrollView_ConsumableEquip.transform.localPosition.x;
        CurPage = 1;

        mscrollview_luckItems.SetDynamicArrowVertical(msp_scrolldowm);
        SetMoneyIds(1, 4);
        //wolongFunclevel = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel((int)FunctionType.funcP_wolongRecycle);
        //初始化设置面板
    }

    public override void Show()
    {
        base.Show();
        Net.CSPetInfoMessage();
        mlb_page.text = CSString.Format(1704, CurPage);
        //ReFreshUI();

        CSPetLevelUpInfo.Instance.ShowSelectPromp(null,true);
        RefreshLeftItemList();
        OnRedPoint();
    }

    #region 点击事件

    private void OnExplanation (GameObject obj)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.ZhangHunTianFu);
    }

    private void OnRecycleClick (GameObject obj)
    {
        var selectList = CSPetLevelUpInfo.Instance.GetCurSelectList();
        if (selectList.Count <= 0)
        {
            UtilityTips.ShowRedTips(1841);
            return;
        }
        //即将达到的列表
        string Reachname = "";
        //超过的列表
        string Outname = "";
        
        int outNameNum = 0;
        //int ReachNameNum = 0;
        if (MastItemClasses != null && mustitemList != null)
        {
            for (int i = 0; i < MastItemClasses.Count; i++)
            {
                string selectStr = MastItemClasses[i].lb_value.text;
                int selectV = 0;
                int.TryParse(selectStr, out selectV);

                int curV = mustitemList[i].itemDailyNum;
                int maxv = mustitemList[i].itemDailyMaxNum;
                int itemid = ItemCallBackBaseTableManager.Instance.GetItemCallBackBaseItem(mustitemList[i].id);
                string name = ItemTableManager.Instance.GetItemName(itemid);
                if (curV >= maxv && maxv != 0 && outNameNum < ShowNum)
                {
                    Outname = GetStr(Outname, name);
                    outNameNum++;
                }
                else if (curV + selectV >= maxv && maxv != 0)
                {
                    Reachname = GetStr(Reachname, name);
                    //ReachNameNum++;
                }
            }
        }

        if (luckItemList != null)
        {
            for (int i = 0; i < luckItemList.Count; i++)
            {
                int curv = luckItemList[i].itemDailyNum;
                int maxv = luckItemList[i].itemDailyMaxNum;

                int itemid = ItemCallBackBaseTableManager.Instance.GetItemCallBackBaseItem(luckItemList[i].id);
                string name = ItemTableManager.Instance.GetItemName(itemid);
                if (curv >= maxv && maxv != 0 && outNameNum < ShowNum)
                {
                    Outname = GetStr(Outname, name);
                    outNameNum++;
                }
            }
        }
        
        System.Action action1;
        System.Action action2;
        System.Action action3;
        if (_resPetInfo != null && _resPetInfo.isMax)
        {
            action1 = () => { UtilityTips.ShowPromptWordTips(84, OnRecyle); };
        }
        else
        {
            action1 = OnRecyle;
        }

        if (Outname != String.Empty)
        {
            int id = outNameNum >= ShowNum - 1 ? 102 : 85;
            //CSString.Format();
            action2 = () => { UtilityTips.ShowPromptWordTips(id, action1, Outname); };
        }
        else
        {
            action2 = action1;
        }

        if (Reachname != String.Empty)
        {
            action3 = () => { UtilityTips.ShowPromptWordTips(86, action2, Reachname); };
        }
        else
        {
            action3 = action2;
        }
        action3.Invoke();

    }

    private string GetStr(string outName, string name)
    {
        if (outName == string.Empty)
            outName = name;
        else
            outName = $"{outName}、{name}";
        return outName;
    }

    private void OnRecyle()
    {
        var selectList = CSPetLevelUpInfo.Instance.GetCurSelectList();
        if (selectList.Count <= 0)
        {
            UtilityTips.ShowRedTips(1841);
            return;
        }

        RepeatedField<int> bagindexs = mPoolHandleManager.GetSystemClass<RepeatedField<int>>();
        
        bagindexs.Clear();
        var iter = selectList.GetEnumerator();
        while (iter.MoveNext())
        {
            bagindexs.Add(iter.Current.Value.bagItemInfo.bagIndex);
        }
        //缓存当前 等级和经验值
        CSPetLevelUpInfo.Instance.oldCurExp = _resPetInfo.exp;
        CSPetLevelUpInfo.Instance.oldCurLv = _resPetInfo.level;
        //FNDebug.Log("Net.CSPetUpgradeMessage");
        FNDebug.Log($"回收道具数量{bagindexs.Count} 选中装备数量{selectList.Count}  装备数量{CSPetLevelUpInfo.Instance.normalEquipList.Count}");
        Net.CSPetUpgradeMessage(0,bagindexs);
        mPoolHandleManager.Recycle(bagindexs);
    }


    /// <summary>
    /// 帮助信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuesitionClick(GameObject obj)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EQUIP_RECYCLE);
    }

    /// <summary>
    /// 切换分页
    /// </summary>
    private void OnChangePage()
    {
        float x = mScrollView_ConsumableEquip.transform.localPosition.x;

        int page = UtilityMath.GetRoundingInt((scrollequipx - x) / mwrap_page.itemSize) + 1;
        if (page <= 0) page = 1;

        //UnityEngine.Debug.Log(scrollequipx+"||" + x  + "||" + mwrap_page.itemSize + "||" +page);
        if (page != CurPage)
        {
            CurPage = page;
            mlb_page.text = CSString.Format(1704, CurPage);
        }
    }

    /// <summary>
    /// 点击设置
    /// </summary>
    /// <param name="obj"></param>
    private void OnConfigClick(GameObject obj)
    {
        //_uiPetLevelUpSetPanel.Show();
        UIManager.Instance.CreatePanel<UIPetLevelUpSetPanel>();
    }

    private void OnClickMustItem(GameObject obj)
    {
        ITEM item = UIEventListener.Get(obj).parameter as ITEM;

        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item);
        }
    }

    /// <summary>
    /// 切换模式
    /// </summary>
    /// <param name="obj"></param>
    private void OnChangeMode(GameObject obj)
    {
        int type = 1;
        if (obj == mbtn_benyuan)
            type = 1;
        if (obj == mbtn_wolong)
        {
            if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_wolongRecycle))
            {
                FUNCOPEN funcopenItem;
                int id = (int)FunctionType.funcP_wolongRecycle;
                if (!FuncOpenTableManager.Instance.TryGetValue(id, out funcopenItem))
                    return;
                UtilityTips.ShowRedTips(106, funcopenItem.needLevel, funcopenItem.functionName);
                return;
            }
            type = 2; 
        }
        SelectChildPanel(type);
    }    

    #endregion

    #region 事件回调

    private void OnClose(uint uievtid, object data)
    {
        int panelId = System.Convert.ToInt32(data);
        if (UtilityPanel.CheckGameModelPanelIsThis<UIPetLevelUpPanel>(panelId))
        {
            return;
        }
        CloseAction = null;
        Close();
    }

    private void OnRedPoint(uint uievtid = 0, object data = null)
    {
        var info = CSPetLevelUpInfo.Instance;
        //info.RefreshBagData();
        var mode = info.Mode;
        mred_normal.SetActive(info.PetLevelUpRedPointByType(RecycleMode.M_NORMAL));
        mred_wolong.SetActive(info.PetLevelUpRedPointByType(RecycleMode.M_WOLONG));
        mred_recyle.SetActive(info.PetLevelUpRedPointByType(mode));
    }
    /// <summary>
    /// 设置面板刷新
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    private void OnConfigCallback(uint uievtid, object data)
    {
        ReFreshUI();
    }

    private void RefreshPetInfo(uint uievtid, object data)
    {
        //刷新ui
        RefreshRecycleAwards();
        RefreshEquipSelect();
    }


    private void OnItemChange(uint uievtid, object data)
    {
        _resPetInfo = csinfo.resPetInfo;
        //CSPetLevelUpInfo.Instance.ShowSelectPromp(null,true);
        //RefreshRecycleAwards();
        RefreshLeftItemList();
        RefreshEquipSelect();
        OnRedPoint();
    }

    private void ReFreshUI()
    {
        CSPetLevelUpInfo.Instance.ShowSelectPromp(null,true);
        //CSPetLevelUpInfo.Instance.RefreshAllSelectData();
        RefreshRecycleAwards();
        RefreshLeftItemList();
        RefreshEquipSelect();
    }

    /// <summary>
    /// 刷新收益 
    /// </summary>
    protected void RefreshRecycleAwards()
    {
        _resPetInfo = csinfo.resPetInfo;
        if (_resPetInfo == null)
            return;
        var mode = csinfo.Mode;
        mustitemList.Clear();
        luckItemList.Clear();

        var itemDaily = _resPetInfo.itemDaily;
        ITEMCALLBACKBASE callbackbase = null;
        for (int i = 0; i < itemDaily.Count; i++)
        {
            if (ItemCallBackBaseTableManager.Instance.TryGetValue(itemDaily[i].id, out callbackbase))
            {
                if (callbackbase.item != 7 && callbackbase.type == (int)mode)
                {
                    if (callbackbase.unlock == 0)
                        mustitemList.Add(itemDaily[i]);
                    else
                        luckItemList.Add(itemDaily[i]);
                }
            }
        }

        mslider_level.gameObject.SetActive(_resPetInfo.id != 0);

        if (_resPetInfo.id == 0 || CSPetLevelUpInfo.Instance.Mode == RecycleMode.M_WOLONG)
        {
            mgrild_mastitems.transform.localPosition = mastitemsVec;
            mslider_level.gameObject.SetActive(false);
        }
        else
        {
            mgrild_mastitems.transform.localPosition = orageVec;
            mslider_level.gameObject.SetActive(true);
            int level = _resPetInfo.level;
            mlb_level.text = CSString.Format(1703, level);
            ZHANHUNLEVEL zhanhunlevel;
            if (ZhanHunLevelTableManager.Instance.TryGetValue(level, out zhanhunlevel))
            {
                curExpMax = zhanhunlevel.needExp;
                mslider_level.value = (float)_resPetInfo.exp / (float)curExpMax;
                string str;
                if (!_resPetInfo.isMax)
                {
                    str = $"{_resPetInfo.exp}/{curExpMax}";
                }
                else
                {
                    str = CSString.Format(1728);
                    mslider_level.value = 1;
                }
                mlb_exp.text = str.BBCode(ColorType.MainText);
            }
        }

        //刷新必得收益
        mgrild_mastitems.MaxCount = 2;
        for (int i = 0; i < mgrild_mastitems.MaxCount; i++)
        {
            MastItemClass mastItemClass;
            if (MastItemClasses.Count - 1 < i)
            {
                mastItemClass = mPoolHandleManager.GetSystemClass<MastItemClass>();
                mastItemClass.Init(mgrild_mastitems.controlList[i].transform);
                MastItemClasses.Add(mastItemClass);
            }
            else
            {
                mastItemClass = MastItemClasses[i];
            }

            int itemid = ItemCallBackBaseTableManager.Instance.GetItemCallBackBaseItem(mustitemList[i].id);
            ITEM item;
            if (ItemTableManager.Instance.TryGetValue(itemid, out item))
            {
                //string icon = ItemTableManager.Instance.GetItemIcon(itemid);
                mastItemClass.sp_icon.spriteName = $"tubiao{item.icon}";


                int num = mustitemList[i].itemDailyNum;
                int max = mustitemList[i].itemDailyMaxNum;
                mastItemClass.lb_max.gameObject.SetActive(max != 0);
                if (max != 0)
                {
                    string str = $"{num}/{max}";
                    mastItemClass.lb_max.text = num >= max ? str.BBCode(ColorType.Red) : str.BBCode(ColorType.MainText);
                }

                UIEventListener.Get(mastItemClass.obj_click, item).onClick = OnClickMustItem;
            }
            //mastItemClass.lb_value.text = "0";
        }


        //刷新幸运收益
        mgrid_luckItems.MaxCount = luckItemList.Count;
        for (int i = 0; i < mgrid_luckItems.MaxCount; i++)
        {
            LuckItemClass luckItemClass;
            if (LuckItemClasses.Count - 1 < i)
            {
                luckItemClass = mPoolHandleManager.GetSystemClass<LuckItemClass>();
                luckItemClass.Init(mgrid_luckItems.controlList[i].transform);
                LuckItemClasses.Add(luckItemClass);
            }
            else
            {
                luckItemClass = LuckItemClasses[i];
            }

            int Num = luckItemList[i].itemDailyNum;
            int MaxNum = luckItemList[i].itemDailyMaxNum;
            int id = luckItemList[i].id;

            ITEMCALLBACKBASE itemcallbackbase;
            if (ItemCallBackBaseTableManager.Instance.TryGetValue(id, out itemcallbackbase))
            {
                int itemid = itemcallbackbase.item;
                bool isflag = luckItemList[i].lockFlag;
                bool iswuxian = MaxNum == 0;
                luckItemClass.lb_lock.gameObject.SetActive(!isflag);
                luckItemClass.lb_max.gameObject.SetActive(isflag && !iswuxian);

                luckItemClass.wuxian.gameObject.SetActive(isflag && iswuxian);
                ITEM item;
                if (ItemTableManager.Instance.TryGetValue(itemid, out item))
                {
                    var color = UtilityColor.GetColorTypeByQuality(item.quality);
                    luckItemClass.lb_name.text = item.name.BBCode(color);
                }
                if (isflag)
                {
                    if (!iswuxian)
                    {
                        string str = $"{Num}/{MaxNum}";
                        luckItemClass.lb_max.text = Num >= MaxNum ? str.BBCode(ColorType.Red) : str.BBCode(ColorType.MainText);
                    }
                }
                else
                {
                    int lv = CSPetTalentInfo.Instance.GetCoreUnlockLv(itemcallbackbase.unlock);
                    luckItemClass.lb_lock.text = CSString.Format(1725, lv);
                }

                if (UIItemBaseList.Count <= i)
                {
                    var uiItemBase = Utility.GetItemByInfo(itemid, luckItemClass.item, 1, itemSize.Size60);
                    UIItemBaseList.Add(uiItemBase);
                    uiItemBase.SetMiniLock(!luckItemList[i].lockFlag);
                }
                else
                {
                    UIItemBaseList[i].Refresh(itemid);
                    UIItemBaseList[i].SetMiniLock(!luckItemList[i].lockFlag);
                }
            }
        }
        mscrollview_luckItems.ResetPosition();
    }

    /// <summary>
    /// 刷新左边物品格子
    /// </summary>
    private void RefreshLeftItemList()
    {

        var temList = csinfo.GetBagInfo();
        bool isCount = temList.Count > 0;
        mScrollView_ConsumableEquip.gameObject.SetActive(isCount);
        memptyHint.SetActive(!isCount);
        mlb_page.gameObject.SetActive(isCount);

        if (temList.Count > 0)
            RefreshItemList();
        else
            RefreshGetWay();
    }

    /// <summary>
    /// 刷新item列表
    /// </summary>
    public void RefreshItemList()
    {
        var mode = CSPetLevelUpInfo.Instance.Mode;
        var temList = csinfo.GetBagInfo();
        endLessList.Clear();
        int pageCount = CSPetLevelUpInfo.Instance.PageNum;
        for (int i = 0; i < pageCount; i++)
        {
            var petlevelUpData = mPoolHandleManager.GetSystemClass<PetLevelUpItemData>();
            petlevelUpData.ItemsInfo = temList;
            petlevelUpData.page = i + 1;
            petlevelUpData.scriptBinder = ScriptBinder;
            petlevelUpData.mode = mode;
            //petlevelUpData.scrollView_ConsumableEquip = mScrollView_ConsumableEquip;
            endLessList.Append(petlevelUpData);
        }

        endLessList.Bind();
    }

    private void RefreshGetWay()
    {
        var mode = CSPetLevelUpInfo.Instance.Mode;
        int sundryid = mode == RecycleMode.M_NORMAL ? 1738 : 1739;
        mlb_hint.text = CSString.Format(sundryid);
        int id = mode == RecycleMode.M_NORMAL ? 601 : 602;
        GetGetWaysInfo(id);
        mgrid_getway.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
    }


    /// <summary>
    /// 选择装备信息刷新
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    private void RefreshEquipSelect(uint uievtid = 0, object data = null)
    {
        listItemNum.Clear();
        for (int i = 0; i < 2; i++)
        {
            listItemNum.Add(0);
        }

        int expAdd = 0;
        var iter = CSPetLevelUpInfo.Instance.GetCurSelectList().GetEnumerator();

        while (iter.MoveNext())
        {
            //var modeType = CSPetLevelUpInfo.Instance.Mode;
            var bagClass = iter.Current.Value;

            var item = bagClass.item;
            for (int i = 0; i < item.callback.Length; i++)
            {
                if (item.callback[i].key() == 7)
                {
                    expAdd += item.callback[i].value() * bagClass.bagItemInfo.count;
                    break;
                }

                if (listItemNum.Count > i)
                {
                    listItemNum[i] += item.callback[i].value();
                }

            }
        }

        for (int i = 0; i < MastItemClasses.Count; i++)
        {
            MastItemClasses[i].lb_value.text = listItemNum[i].ToString();
        }

        mslider_sublevel.value = (float)(_resPetInfo.exp + expAdd) / (float)curExpMax;
        if (_resPetInfo.isMax)
        {
            mlb_exp.text = CSString.Format(1728);
        }
        else if (expAdd > 0)
        {
            mlb_exp.text = $"{_resPetInfo.exp}+({expAdd})/{curExpMax}".BBCode(ColorType.MainText);
        }
        else
        {
            mlb_exp.text = $"{_resPetInfo.exp}/{curExpMax}".BBCode(ColorType.MainText);
        }

    }
    #endregion

    /// <summary>
    /// 通过GetWay的id字符串获取GetWay列表
    /// </summary>
    /// <param name="idStr">GetWay的id字符串，通过#连接</param>
    /// <returns></returns>
    void GetGetWaysInfo(int sundryid)
    {
        string idStr = SundryTableManager.Instance.GetSundryEffect(sundryid);
        getWayList.Clear();
        if (!string.IsNullOrEmpty(idStr))
        {
            CSGetWayInfo.Instance.GetGetWays(idStr, ref getWayList);
        }
    }

    public override void SelectChildPanel(int type = 1)
    {
        var info = CSPetLevelUpInfo.Instance;
        if (type == 1)
            info.Mode = RecycleMode.M_NORMAL;
        if (type == 2)
            info.Mode = RecycleMode.M_WOLONG;
        msp_benyuansel.SetActive(type == 1);
        msp_wolongsel.SetActive(type == 2);

        mred_recyle.SetActive(info.PetLevelUpRedPointByType(info.Mode));
        ReFreshUI();
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    protected override void OnDestroy()
    {
        CSPetLevelUpInfo.Instance.Mode = RecycleMode.M_NORMAL;
        mgrid_getway.UnBind<GetWayBtn>();
        // if (CloseAction != null)
        // {
        //     CloseAction();
        // }
        mClientEvent.RemoveEvent(CEvent.CSPetInfoMessage, RefreshPetInfo);
        mClientEvent.RemoveEvent(CEvent.OnRecycleItemChange, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.SelectEquip, RefreshEquipSelect);
        mClientEvent.RemoveEvent(CEvent.ConfigCallback, OnConfigCallback);
        mClientEvent.RemoveEvent(CEvent.FastAccessJumpToPanel, OnClose);
        mClientEvent.RemoveEvent(CEvent.FastAccessTransferNpc, OnClose);

        mPoolHandleManager.Recycle(mustitemList);
        mPoolHandleManager.Recycle(luckItemList);
        mPoolHandleManager.Recycle(UIItemBaseList);
        mPoolHandleManager.Recycle(LuckItemClasses);
        mPoolHandleManager.Recycle(MastItemClasses);
        endLessList?.Destroy();
        base.OnDestroy();
    }
}

public class UIPetLevelUpItem : UIBinder
{
    private List<UIItemBase> items;
    private PetLevelUpItemData _data;
    private ScriptBinder _scriptBinder;
    private UIGridContainer gridContainer;
    int itemCount = 25;
    public override void Init(UIEventListener handle)
    {
        //grid = handle.GetComponentInChildren<UIGrid>();
        items = PoolHandle.GetSystemClass<List<UIItemBase>>();
        items.Clear();
        gridContainer = handle.GetComponentInChildren<UIGridContainer>();

        HotManager.Instance.EventHandler.AddEvent(CEvent.BagItemDBClicked, OnChange);
        HotManager.Instance.EventHandler.AddEvent(CEvent.TipsBtnRecycleUnSelectd, OnChange);
    }

    private void OnChange(uint uievtid, object data)
    {
        if (data is BagItemInfo bagItemInfo)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].infos?.id == bagItemInfo.id)
                {
                    OnRecycleItemClicked(items[i]);
                    //items[i].ShowSelect(!items[i].chooseState);
                }
            }
        }
    }

    public override void Bind(object data)
    {
        _data = (PetLevelUpItemData)data;
        int page = _data.page;
        int count = _data.ItemsInfo.Count;
        var itemsinfo = _data.ItemsInfo;

        gridContainer.MaxCount = itemCount;
        for (int i = 0; i < gridContainer.MaxCount; i++)
        {
            int index = itemCount * (page - 1) + i;
            UIItemBase item = null;
            if (items.Count > i)
            {
                item = items[i];
            }
            else
            {
                var node = UtilityObj.Get<Transform>(gridContainer.controlList[i].transform, "node");
                //var itembase = PoolHandle.CreateItemPool
                //    (PropItemType.Normal, node, 8);
                //var tempitem = itembase.Append();
                item = UIItemManager.Instance.GetItem(PropItemType.Normal, node);
                items.Add(item);
            }

            if (count > index)
            {
                item.Refresh(itemsinfo[index].bagItemInfo, OnRecycleItemClicked, false);
                var itemCfg = itemsinfo[index].item;
                //判断为本职业本性别 等级符合才显示
                if (itemCfg.career == CSMainPlayerInfo.Instance.Career || itemCfg.career == 0)
                {
                    if (itemCfg.sex == 2 || itemCfg.sex == CSMainPlayerInfo.Instance.Sex)
                    {
                        if (itemCfg.level <= CSMainPlayerInfo.Instance.Level)
                            item.ShowArrow(true);    
                    }
                }
                item.SetCount(itemsinfo[index].bagItemInfo.count);
                item.SetOnPress(OnRecycleItemPress);
                item.SetItemDBClickedCB((a) =>
                {
                    UtilityTips.ShowRedTips(1777);
                });
                item.JudgeMaskWithoutType();
                //itemsinfos.Add(item);

                var dicSelects = CSPetLevelUpInfo.Instance.GetCurSelectList();
                if (item.infos != null)
                {
                    item.ShowSelect(dicSelects.ContainsKey(item.infos.id));
                }
            }
            else
            {
                item.UnInit();
            }
        }
    }



    protected void OnRecycleItemClicked(UIItemBase item)
    {
        bool isstate = !item.chooseState;
        var Info = CSPetLevelUpInfo.Instance;
        if (CSBagInfo.Instance.GetEquipLevelByPos(item.itemCfg)&&isstate && item.itemCfg.level >= Info.PrompLevel)
        {
            UtilityTips.ShowPromptWordTips(100, 
                () =>
                {
                    item.ShowSelect(false);
                }, () =>
                {
                    OnSelectItem(item,isstate);
                    
                    // item.ShowSelect(isstate);
                    // var bagclass = CSPetLevelUpInfo.Instance.GetBagClass(item.infos, item.itemCfg);
                    // CSPetLevelUpInfo.Instance.RefreshSelectList(item.infos.id, bagclass, isstate,CSPetLevelUpInfo.Instance.Mode);
                    // HotManager.Instance.EventHandler.SendEvent(CEvent.SelectEquip);
                }
            );
        }
        else
        {
            OnSelectItem(item,isstate);
        }
    }

    private void OnSelectItem(UIItemBase item,bool isstate)
    {
        item.ShowSelect(isstate);
        var bagclass = CSPetLevelUpInfo.Instance.GetBagClass(item.infos, item.itemCfg);
        CSPetLevelUpInfo.Instance.RefreshSelectList(item.infos.id, bagclass, isstate,CSPetLevelUpInfo.Instance.Mode);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SelectEquip);
    }



    protected void OnRecycleItemPress(UIItemBase item)
    {
        bool isChoose = item.chooseState;
        if (item != null && item.itemCfg != null)
        {
            if (isChoose)
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.Recycle2Bag, item.itemCfg, item.infos);
            }
            else
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.Bag2Recycle, item.itemCfg, item.infos);
            }
        }
    }

    public override void OnDestroy()
    {
        //grid = null;
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.BagItemDBClicked, OnChange);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.TipsBtnRecycleUnSelectd, OnChange);
        UIItemManager.Instance.RecycleItemsFormMediator(items);
        items.Clear();
        PoolHandle.Recycle(items);
    }
}

public class PetLevelUpItemData : IDispose
{
    public ILBetterList<BagClass> ItemsInfo;
    public int page;
    public ScriptBinder scriptBinder;
    public RecycleMode mode;
    //public UIScrollView scrollView_ConsumableEquip;

    public void Dispose()
    {
        //scrollView_ConsumableEquip = null;    
        ItemsInfo = null;
        scriptBinder = null;
    }
}

public class MastItemClass : IDispose
{
    public UISprite sp_icon;
    public UILabel lb_value;
    public UILabel lb_max;
    public Transform UIItemBarPrefab;
    public GameObject obj_click;

    public void Init(Transform trans)
    {
        sp_icon = UtilityObj.Get<UISprite>(trans, "UIItemBarPrefab/sp_icon");
        lb_value = UtilityObj.Get<UILabel>(trans, "UIItemBarPrefab/lb_value");
        lb_max = UtilityObj.Get<UILabel>(trans, "lb_max");
        UIItemBarPrefab = UtilityObj.Get<Transform>(trans, "UIItemBarPrefab");
        obj_click = UtilityObj.Get<Transform>(trans, "obj_click").gameObject;
    }

    public void Dispose()
    {
        sp_icon = null;
        lb_value = null;
        lb_max = null;
    }
}

public class LuckItemClass : IDispose
{
    public Transform item;
    public UILabel lb_name;
    public UILabel lb_max;
    public UILabel lb_lock;
    public Transform wuxian;

    public void Init(Transform trans)
    {
        item = UtilityObj.Get<Transform>(trans, "item");
        lb_name = UtilityObj.Get<UILabel>(trans, "lb_name");
        lb_max = UtilityObj.Get<UILabel>(trans, "lb_max");
        lb_lock = UtilityObj.Get<UILabel>(trans, "lb_lock");
        wuxian = UtilityObj.Get<Transform>(trans, "wuxian");
    }

    public void Dispose()
    {
        item = null;
        lb_name = null;
        lb_max = null;
    }
}
