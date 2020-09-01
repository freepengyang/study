using instance;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHonorChanllengeRankPanel : UIBasePanel
{
    enum ShowType
    {
        Rank,
        Reward,
    }
    #region   variable
    ShowType currentType;
    FastArrayElementFromPool<HonorRankTabs> tabsList;
    FastArrayElementFromPool<HonorRankRewardItem> rankRewardList;
    int tabsIndex = -1;
    int myGroup = 0;
    Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>> dataDic;
    Dictionary<int, rank.RankInfo> rankDic;
    BossChallengeInfo mes;
    string[] groupDes;
    List<List<int>> seasonGoals;
    #endregion
    public override void Init()
    {
        base.Init();
        AddCollider();
        groupDes = ClientTipsTableManager.Instance.GetClientTipsContext(1118).Split('#');
        seasonGoals = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(51));
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        tabsList = mPoolHandleManager.CreateGeneratePool<HonorRankTabs>();
        tabsList.Count = 3;
        for (int i = 0; i < 3; i++)
        {
            tabsList[i].Init(mtrans_tabsPar.GetChild(i).gameObject);
            UIEventListener.Get(tabsList[i].go, 5 + i).onClick = TabsClick;
        }
        rankRewardList = mPoolHandleManager.CreateGeneratePool<HonorRankRewardItem>();
        mscrbar_scr.onChange.Add(new EventDelegate(OnChange));
    }

    public override void Show()
    {
        base.Show();
        mscr_show.ResetPosition();
    }

    protected override void OnDestroy()
    {
        tabsList.Clear();
        for (int i = 0; i < rankRewardList.Count; i++)
        {
            rankRewardList[i].UnInit();
        }
        rankRewardList.Clear();
        base.OnDestroy();
    }
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIHonorChanllengeRankPanel>();
    }
    public void SetShowType(int _type, BossChallengeInfo _mes, Dictionary<int, rank.RankInfo> _rankDic)
    {
        currentType = (ShowType)_type;
        mes = _mes;
        myGroup = _mes.group;
        rankDic = _rankDic;
        RefreshGrid();
    }
    void RefreshDes(int _group)
    {
        mlb_title.text = (currentType == ShowType.Rank) ? ClientTipsTableManager.Instance.GetClientTipsContext(1865) : ClientTipsTableManager.Instance.GetClientTipsContext(1868);
        mobj_rewardDes.SetActive((currentType == ShowType.Rank) ? false : true);
        mlb_goalName.gameObject.SetActive((currentType == ShowType.Reward) ? false : true);
        if (_group != myGroup + 4)
        {
            mlb_noRank.gameObject.SetActive(true);
            mobj_desPar.SetActive(false);
            mlb_noRank.text = ClientTipsTableManager.Instance.GetClientTipsContext(1864);
        }
        else
        {
            mlb_noRank.gameObject.SetActive(false);
            mobj_desPar.SetActive(true);
            if (rankDic[myGroup + 4].myRank < 0)
            {
                mlb_myRank.text = ClientTipsTableManager.Instance.GetClientTipsContext(1867);
            }
            else
            {
                mlb_myRank.text = $"[00ff0c]{rankDic[myGroup + 4].myRank + 1}";

            }
            mlb_myLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1866), mes.pass);
            mlb_goalName.text = $"{string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1129), groupDes[mes.group - 1])}{string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1131), seasonGoals[mes.group - 1][1], seasonGoals[mes.group - 1][2])}";
        }
    }
    void RefreshGrid()
    {
        TabsClick(tabsList[myGroup - 1].go);
    }
    Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>> rewards = new Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>>();
    int rankMaxCount = 20;
    void TabsClick(GameObject _go)
    {
        mscr_show.ResetPosition();
        if (tabsIndex != -1)
        {
            tabsList[tabsIndex - 5].ChangeChooseState();
        }
        tabsIndex = (int)UIEventListener.Get(_go).parameter;
        tabsList[tabsIndex - 5].ChangeChooseState();
        RefreshDes(tabsIndex);

        if (currentType == ShowType.Rank)
        {
            RankAwardsTableManager.Instance.GetRankStep(tabsIndex, rewards);
            mgrid_show.MaxCount = rankMaxCount;
            rankRewardList.Count = rankMaxCount;
            rank.RankInfo rankinfo = CSRankInfo.Instance.GetRankByType(tabsIndex);
            for (int i = 0; i < rankMaxCount; i++)
            {
                rankRewardList[i].Init(mgrid_show.controlList[i]);
                //rankRewardList[i].Refresh(rankinfo.ranks[i], i + 1);
                if (i < rankinfo.ranks.Count)
                {
                    rankRewardList[i].Refresh(rankinfo.ranks[i], i + 1);
                }
                else
                {
                    rankRewardList[i].UnRefersh(i + 1);
                }
                if (i == (rankMaxCount - 1))
                {
                    rankRewardList[i].HideLine(false);
                }
            }
        }
        else
        {
            dataDic = RankAwardsTableManager.Instance.GetRewardByType(tabsIndex);
            mgrid_show.MaxCount = dataDic.Count;
            rankRewardList.Count = dataDic.Count;
            var iter = dataDic.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {
                rankRewardList[i].Init(mgrid_show.controlList[i]);
                rankRewardList[i].Refresh(iter.Current.Key, iter.Current.Value);
                i++;
            }
        }
        mscr_show.verticalScrollBar.value = 0;
    }
    void OnChange()
    {
        if (mscr_show.shouldMoveVertically)
        {
            if (mscrbar_scr.value >= 0.95)
            {
                mobj_arrow.SetActive(false);
            }
            else
            {
                mobj_arrow.SetActive(true);
            }
        }
        else
        {
            mobj_arrow.SetActive(false);
        }
    }
}
public class HonorRankTabs
{
    public GameObject go;
    public GameObject highLight;
    public HonorRankTabs()
    {

    }
    public void Init(GameObject _go)
    {
        go = _go;
        highLight = go.transform.Find("highLight").gameObject;
    }
    public void ChangeChooseState()
    {
        highLight.SetActive(!highLight.activeSelf);
    }
}
public class HonorRankRewardItem
{
    public GameObject go;
    public UISprite sp_rankNum;
    public UILabel lb_rankNum;
    public GameObject rankMes;
    public UILabel name;
    public UILabel rankLv;
    public GameObject line;
    public UIGrid Items;
    List<UIItemBase> itembaseList;
    public HonorRankRewardItem()
    {
    }
    public void Init(GameObject _go)
    {
        go = _go;
        sp_rankNum = go.transform.Find("sp_rank").GetComponent<UISprite>();
        lb_rankNum = go.transform.Find("lb_rank").GetComponent<UILabel>();
        rankMes = go.transform.Find("rankMes").gameObject;
        name = go.transform.Find("rankMes/name").GetComponent<UILabel>();
        rankLv = go.transform.Find("rankMes/rankNum").GetComponent<UILabel>();
        Items = go.transform.Find("Item").GetComponent<UIGrid>();
        line = go.transform.Find("line").gameObject;
        if (itembaseList == null)
        {
            itembaseList = UIItemManager.Instance.GetUIItems(4, PropItemType.Normal, Items.transform, itemSize.Size50);
        }
    }
    public void Refresh(int _boxId, CSBetterLisHot<TABLE.RANKAWARDS> _rewards)
    {
        Items.gameObject.SetActive(true);
        rankMes.gameObject.SetActive(false);

        if (_rewards[0].rank <= 3)
        {
            sp_rankNum.gameObject.SetActive(true);
            sp_rankNum.spriteName = $"rank{_rewards[0].rank}";
            lb_rankNum.text = "";
        }
        else
        {
            lb_rankNum.text = $"{_rewards[0].rank}~{_rewards[_rewards.Count - 1].rank}";
            lb_rankNum.color = CSColor.beige;
            sp_rankNum.gameObject.SetActive(false);
        }
        List<int> SRewardIds = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxItem(_boxId), '#');
        List<int> SRewardNums = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxNum(_boxId), '#');
        for (int i = 0; i < itembaseList.Count; i++)
        {
            if (i >= SRewardIds.Count)
            {
                itembaseList[i].obj.SetActive(false);
            }
            else
            {
                itembaseList[i].Refresh(SRewardIds[i]);
                itembaseList[i].SetCount(SRewardNums[i]);
            }
        }
    }
    public void Refresh(rank.RankData _data, int _ind)
    {
        if (_ind <= 3)
        {
            sp_rankNum.gameObject.SetActive(true);
            sp_rankNum.spriteName = $"rank{_ind}";
            lb_rankNum.text = "";
        }
        else
        {
            lb_rankNum.text = $"{_ind}";
            lb_rankNum.color = CSColor.beige;
            sp_rankNum.gameObject.SetActive(false);
        }
        Items.gameObject.SetActive(false);
        rankMes.gameObject.SetActive(true);
        name.text = _data.name;
        rankLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1123), _data.value);
    }
    public void UnRefersh(int _ind)
    {
        if (_ind <= 3)
        {
            sp_rankNum.gameObject.SetActive(true);
            sp_rankNum.spriteName = $"rank{_ind}";
            lb_rankNum.text = "";
        }
        else
        {
            lb_rankNum.text = $"{_ind}";
            lb_rankNum.color = CSColor.beige;
            sp_rankNum.gameObject.SetActive(false);
        }
        rankLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1123), 0);
        name.text = ClientTipsTableManager.Instance.GetClientTipsContext(1869);
    }
    public void HideLine(bool _state)
    {
        line.SetActive(_state);
    }
    public void UnInit()
    {
        for (int i = 0; i < itembaseList.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(itembaseList[i]);
        }
        itembaseList = null;
    }
}
