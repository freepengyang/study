using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailyArenaCombinePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region variable
    ILBetterList<DailyArenaData> resDic = new ILBetterList<DailyArenaData>();
    List<DailyTabsItem> tabsList = new List<DailyTabsItem>();
    ILBetterList<TABLE.JINGJIHUODONGJIANGLI> jingliList = new ILBetterList<TABLE.JINGJIHUODONGJIANGLI>();
    ILBetterList<DailyTaskItem> taskList = new ILBetterList<DailyTaskItem>(4);
    DailyTabsItem curTabItem;
    int wrapitemHeight = 118;
    int[] sortIndex = { 1, 2, 0 };// 0进行中，1可领取，2已完成
    long remainTime = 0;
    #endregion
    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowEffectPlay(mobj_eff, 17524);
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        UIEventListener.Get(mbtn_gift).onClick = GiftClick;
        UIEventListener.Get(msp_bubble).onClick = GiftClick;
        UIEventListener.Get(mbtn_introduce).onClick = HelpBtnClick;
        mClientEvent.AddEvent(CEvent.SCAthleticsActivityInfoMessage, SCAthleticsActivityInfoMessage);
        mClientEvent.AddEvent(CEvent.SCReceiveAthleticsActivityRewardMessage, SCReceiveAthleticsActivityRewardMessage);
        Net.CSAthleticsActivityInfoMessage();


        for (int i = 0; i < mwrap_tasks.transform.childCount; i++)
        {
            taskList.Add(new DailyTaskItem(mwrap_tasks.transform.GetChild(i).gameObject));
        }

        mobj_bubble.SetActive(CSDailyArenaInfo.Instance.showBubble);



    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_eff);
        for (int i = 0; i < taskList.Count; i++)
        {
            taskList[i].Recycle();
        }
        base.OnDestroy();
    }
    #region NetBack
    void SCAthleticsActivityInfoMessage(uint id, object data)
    {
        tabsList.Clear();
        RefreshTabs();
        RefershTabsRed();

    }
    void SCReceiveAthleticsActivityRewardMessage(uint id, object data)
    {
        JingjiHuodongjiangliTableManager.Instance.GetJingJiTasks(curTabItem.data.id, jingliList);
        SortBuState();
        mscr_tasks.ResetPosition();
        mwrap_tasks.ResetChildPositions();
        InitWrap(curTabItem.data.dic.Count);
        RefershTabsRed();
    }
    #endregion
    #region 刷新界面
    void InitWrap(int _dataCount)
    {
        mscr_tasks.ResetPosition();
        mscr_tasks.SetDynamicArrowVerticalWithWrap(wrapitemHeight * _dataCount, mobj_arrow);
        if (_dataCount == 0)
        {
            for (int i = 0; i < mwrap_tasks.transform.childCount; i++)
            {
                mwrap_tasks.transform.GetChild(i).gameObject.SetActive(false);
            }
            mwrap_tasks.enabled = false;
        }
        else
        {
            if (_dataCount < mwrap_tasks.transform.childCount)
            {
                for (int i = 0; i < mwrap_tasks.transform.childCount; i++)
                {
                    Transform trans = mwrap_tasks.transform.GetChild(i);
                    if (i >= _dataCount)
                    {
                        trans.gameObject.SetActive(false);
                    }
                    else
                    {
                        trans.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < mwrap_tasks.transform.childCount; i++)
                {
                    Transform trans = mwrap_tasks.transform.GetChild(i);
                    trans.gameObject.SetActive(true);
                }
            }
            if (_dataCount != 1)
            {
                mwrap_tasks.onInitializeItem = OnUpdateItem;
                mwrap_tasks.maxIndex = 0;
                mwrap_tasks.minIndex = (_dataCount == 0 ? 0 : (-1 * _dataCount + 1));
                if (mwrap_tasks.enabled)
                {
                    mwrap_tasks.SortBasedOnScrollMovement();
                }
                else
                {
                    mwrap_tasks.enabled = true;
                    mwrap_tasks.SortBasedOnScrollMovement();
                }
            }
            else
            {
                mwrap_tasks.enabled = false;
                taskList[0].Refresh(0, curTabItem, jingliList[0]);
            }
            mscr_tasks.ResetPosition();
        }
    }
    void RefreshTask(GameObject go, int wrapIndex, int realIndex)
    {
        int realDataIndex = -realIndex;
        //Debug.Log($"{wrapIndex}    {realIndex}   {realDataIndex}");
        if (jingliList.Count <= realDataIndex)
        {
            return;
        }
        for (int i = 0; i < taskList.Count; i++)
        {
            if (taskList[i].go == go)
            {
                taskList[i].Refresh(realIndex, curTabItem, jingliList[realDataIndex]);
                return;
            }
        }
        //taskList[wrapIndex].Refresh(realIndex, curTabItem, jingliList[realDataIndex]);
    }
    Coroutine Cor;
    TABLE.JINGJIHUODONG cfg;
    void RefreshTitle()
    {
        if (curTabItem != null)
        {
            JingjiHuodongTableManager.Instance.TryGetValue(curTabItem.data.id, out cfg);
            if (cfg != null)
            {
                mlb_des1.text = cfg.name1;
                mlb_gift.text = cfg.tip;

                if (cfg.type != 2)
                {
                    remainTime = (curTabItem.data.remainTime - CSServerTime.Instance.TotalMillisecond) / 1000;
                    mlb_des2.text = string.Format(cfg.name2, CSServerTime.Instance.FormatLongToTimeStr(remainTime, 2));
                    Debug.Log($"{curTabItem.data.id}   {curTabItem.data.Acstate}   {curTabItem.data.remainTime}  {CSServerTime.Instance.TotalMillisecond}  {mlb_des2.text}");
                    if (Cor != null)
                    {
                        ScriptBinder.StopCoroutine(Cor);
                    }
                    Cor = ScriptBinder.StartCoroutine(BuffCountDown());
                }
                else
                {
                    mlb_des2.text = JingjiHuodongTableManager.Instance.GetJingjiHuodongName2(curTabItem.data.id);
                    if (Cor != null)
                    {
                        ScriptBinder.StopCoroutine(Cor);
                    }
                }
            }
        }
    }
    IEnumerator BuffCountDown()
    {
        yield return new WaitForSeconds(1f);
        remainTime--;
        if (remainTime > 0 && cfg != null)
        {
            mlb_des2.text = string.Format(cfg.name2, CSServerTime.Instance.FormatLongToTimeStr(remainTime, 2));
            Cor = ScriptBinder.StartCoroutine(BuffCountDown());
        }
        else
        {
            if (Cor != null)
            {
                ScriptBinder.StopCoroutine(Cor);
            }
        }
    }
    void OnUpdateItem(GameObject go, int wrapIndex, int realIndex)
    {
        //Debug.Log($"{go.name}   {wrapIndex}   {realIndex}");
        //if (go.activeSelf)
        //{
        RefreshTask(go, wrapIndex, realIndex);
        //}
    }

    void RefershTabsRed()
    {
        for (int i = 0; i < resDic.Count; i++)
        {
            tabsList[i].ChangeRed(false);
            var iter = resDic[i].dic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.num > 0 && iter.Current.Value.state == 1)
                {
                    tabsList[i].ChangeRed(true);
                    break;
                }
            }
        }
    }
    #endregion
    void RefreshTabs()
    {
        int openIndex = -1;
        resDic = CSDailyArenaInfo.Instance.GetDailyInfo();
        mgrid_tabs.MaxCount = resDic.Count;
        for (int i = 0; i < resDic.Count; i++)
        {
            if (openIndex == -1 && resDic[i].Acstate == 0)
            {
                openIndex = i;
            }
            if (tabsList.Count < resDic.Count)
            {
                tabsList.Add(new DailyTabsItem(mgrid_tabs.controlList[i], i, TabsClick, resDic[i]));
            }
        }

        if (openIndex != -1)
        {
            TabsClick(tabsList[openIndex]);
        }


        //if (resDic.Count > 0 )
        //{
        //    TabsClick(tabsList[0]);
        //}
    }
    #region click
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIDailyArenaCombinePanel>();
    }
    void GiftClick(GameObject _go)
    {
        if (CSDailyArenaInfo.Instance.showBubble)
        {
            CSDailyArenaInfo.Instance.showBubble = false;
            mobj_bubble.SetActive(CSDailyArenaInfo.Instance.showBubble);
        }
        if (curTabItem != null)
        {
            UtilityPanel.JumpToPanel(JingjiHuodongTableManager.Instance.GetJingjiHuodongUiModel(curTabItem.data.id));
        }
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.DailyAeana);
    }
    void TabsClick(DailyTabsItem _item)
    {
        if (_item.data.Acstate == 1)
        {
            UtilityTips.ShowRedTips(2038, JingjiHuodongTableManager.Instance.GetJingjiHuodongShowdate(_item.data.id));
            return;
        }
        else if (_item.data.Acstate == 2)
        {
            UtilityTips.ShowRedTips(2037);
            return;
        }
        if (curTabItem != null)
        {
            if (curTabItem == _item)
            {
                return;
            }
            else
            {
                curTabItem.ChangeCheckMark(false);
            }
        }
        curTabItem = _item;

        JingjiHuodongjiangliTableManager.Instance.GetJingJiTasks(curTabItem.data.id, jingliList);
        SortBuState();

        int type = JingjiHuodongTableManager.Instance.GetJingjiHuodongType(curTabItem.data.id);
        string textureName;
        switch (type)
        {
            case 1:
                textureName = "banner19";
                break;
            case 2:
                textureName = "banner20";
                break;
            case 3:
            case 4:
                textureName = "banner21";
                break;
            default:
                textureName = "banner19";
                break;
        }

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_titleBg, textureName);

        curTabItem.ChangeCheckMark(true);
        RefreshTitle();
        //mwrap_tasks.SortBasedOnScrollMovement();
        InitWrap(curTabItem.data.dic.Count);
    }
    void SortBuState()
    {
        jingliList.Sort((a, b) =>
        {
            DailyArenaRewardData data1;
            curTabItem.data.dic.TryGetValue(a.id, out data1);
            DailyArenaRewardData data2;
            curTabItem.data.dic.TryGetValue(b.id, out data2);
            if (data1 != null && data2 != null)
            {
                if ((data1.num == 0) != (data2.num == 0))
                {
                    return (data1.num == 0) ? 1 : -1;
                }
                if (sortIndex[data2.state] != sortIndex[data1.state])
                {
                    return sortIndex[data2.state] - sortIndex[data1.state];
                }
            }
            return a.id - b.id;
        });
    }
    #endregion
    class DailyTabsItem
    {
        GameObject go;
        UILabel name1;
        UILabel name2;
        GameObject red;
        GameObject check;
        GameObject specialDay;
        UILabel lb_specialDay;
        Action<DailyTabsItem> action;
        public DailyArenaData data;
        public DailyTabsItem(GameObject _go, int _type, Action<DailyTabsItem> _action, DailyArenaData _data)
        {
            go = _go;
            action = _action;
            data = _data;
            name1 = go.transform.Find("Label").GetComponent<UILabel>();
            name2 = go.transform.Find("Checkmark/Label").GetComponent<UILabel>();
            red = go.transform.Find("red").gameObject;
            check = go.transform.Find("Checkmark").gameObject;
            specialDay = go.transform.Find("sp_day").gameObject;
            lb_specialDay = go.transform.Find("sp_day/lb_day").GetComponent<UILabel>();

            UIEventListener.Get(go).onClick = Click;
            name1.text = JingjiHuodongTableManager.Instance.GetJingjiHuodongEventId(data.id);
            name2.text = JingjiHuodongTableManager.Instance.GetJingjiHuodongEventId(data.id);
            string str = JingjiHuodongTableManager.Instance.GetJingjiHuodongMarker(data.id);
            if (string.IsNullOrEmpty(str))
            {
                specialDay.SetActive(false);
            }
            else
            {
                specialDay.SetActive(true);
                lb_specialDay.text = str;
            }

        }

        void Click(GameObject _go)
        {
            if (action != null) { action(this); }
        }
        public void ChangeCheckMark(bool _state)
        {
            check.SetActive(_state);
        }
        public void ChangeRed(bool _state)
        {
            red.SetActive(_state);
        }
    }

    class DailyTaskItem
    {
        public GameObject go;
        public UILabel name;
        public UIGrid grid;
        public UISprite btn;
        public UILabel btn_des;
        public GameObject red;
        public UILabel count;
        public UISprite seal;
        public GameObject effect;
        Dictionary<int, int> rewardDic = new Dictionary<int, int>();
        TABLE.JINGJIHUODONGJIANGLI cfg;
        ILBetterList<UIItemBase> itemBaseList = new ILBetterList<UIItemBase>(10);
        int state = 0;// 1已领完  2已领取  3可领  4未完成
        public DailyTaskItem(GameObject _go)
        {
            go = _go;
            name = go.transform.Find("lb_name").GetComponent<UILabel>();
            grid = go.transform.Find("Grid").GetComponent<UIGrid>();
            btn = go.transform.Find("btn_receive").GetComponent<UISprite>();
            btn_des = go.transform.Find("btn_receive/Label").GetComponent<UILabel>();
            red = go.transform.Find("btn_receive/redpoint").gameObject;
            count = go.transform.Find("lb_count").GetComponent<UILabel>();
            seal = go.transform.Find("sp_complete").GetComponent<UISprite>();
            effect = go.transform.Find("btn_receive/eff").gameObject;
            CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_button_blue_add");
            effect.SetActive(false);
            UIEventListener.Get(btn.gameObject).onClick = Click;
        }
        public void Refresh(int index, DailyTabsItem curTabItem, TABLE.JINGJIHUODONGJIANGLI _cfg)
        {
            cfg = _cfg;
            var reward = cfg.reward;
            int gap = 0;
            if (reward.Length > itemBaseList.Count)
            {
                gap = reward.Length - itemBaseList.Count;
            }
            if (gap > 0)
            {
                for (int i = 0; i < gap; i++)
                {
                    itemBaseList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, grid.transform, itemSize.Size60));
                }
            }
            for (int i = 0; i < itemBaseList.Count; i++)
            {
                if (i >= reward.Length)
                {
                    itemBaseList[i].obj.SetActive(false);

                }
                else
                {
                    itemBaseList[i].obj.SetActive(true);
                    itemBaseList[i].Refresh(reward[i].key());
                    itemBaseList[i].SetCount(reward[i].value());
                }
            }
            grid.Reposition();
            RefreshBtn(curTabItem);
        }
        void RefreshBtn(DailyTabsItem curTabItem)
        {
            if (curTabItem.data.dic.ContainsKey(cfg.id))
            {
                DailyArenaRewardData data = curTabItem.data.dic[cfg.id];
                string numA = data.pro < 10000 ? data.pro.ToString() : UtilityMath.GetDecimalValue(data.pro, "F2");
                string numB = cfg.count < 10000 ? cfg.count.ToString() : UtilityMath.GetDecimalValue(cfg.count, "F2");
                string str1 = (data.pro >= cfg.count) ? $"[00ff0c]({numA}/{numB})[-]" : $"[ff0000]({numA}/{numB})[-]";
                name.text = string.Format(cfg.name, str1);

                if (data.num == 0)
                {
                    count.text = "";
                    effect.SetActive(false);
                    btn.gameObject.SetActive(false);
                    seal.spriteName = "yilingwan";
                    seal.gameObject.SetActive(true);
                }
                else
                {
                    if (data.state == 2)
                    {
                        count.text = "";
                        effect.SetActive(false);
                        btn.gameObject.SetActive(false);
                        seal.spriteName = "yilingqu2";
                        seal.gameObject.SetActive(true);
                    }
                    else if (data.state == 1)
                    {
                        count.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1834), data.num);
                        btn.spriteName = "btn_samll1";
                        btn_des.text = "领取";
                        state = 3;
                        seal.gameObject.SetActive(false);
                        effect.SetActive(true);
                        btn.gameObject.SetActive(true);
                        red.SetActive(true);
                    }
                    else if (data.state == 0)
                    {
                        count.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1834), data.num);
                        btn.spriteName = "btn_samll1";
                        btn_des.text = "去完成";
                        state = 4;
                        seal.gameObject.SetActive(false);
                        effect.SetActive(false);
                        btn.gameObject.SetActive(true);
                        red.SetActive(false);
                    }
                }
            }
        }

        void Click(GameObject _go)
        {
            if (state == 3)
            {
                Net.CSReceiveAthleticsActivityRewardMessage(cfg.id);
            }
            else if (state == 4)
            {
                if (cfg.Categories == 1)
                {
                    UtilityPanel.JumpToPanel(cfg.deliver);
                }
                else if (cfg.Categories == 2)
                {
                    string wayStr = SundryTableManager.Instance.GetSundryEffect(cfg.uiModel);
                    UtilityPanel.ShowCompleteWayWithSelfAdapt(wayStr, btn, AnchorType.TopCenter);
                }
                else if (cfg.Categories == 3)
                {
                    string wayStr = SundryTableManager.Instance.GetSundryEffect(cfg.uiModel);
                    UtilityPanel.ShowCompleteWayWithSelfAdapt(wayStr, btn, AnchorType.TopCenter, 1);
                }
            }
        }
        void RecycleItems()
        {
            for (int i = 0; i < itemBaseList.Count; i++)
            {
                UIItemManager.Instance.RecycleSingleItem(itemBaseList[i]);
            }
        }
        public void Recycle()
        {
            RecycleItems();
        }
    }
}
