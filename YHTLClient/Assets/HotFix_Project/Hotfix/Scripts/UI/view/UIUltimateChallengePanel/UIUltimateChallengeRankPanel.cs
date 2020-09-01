using System.Collections.Generic;
using ultimate;
using UnityEngine;

public partial class UIUltimateChallengeRankPanel
{
    private ResponseRankInfo _RankInfo;

    private List<ChallengeAward> rankTabList = new List<ChallengeAward>();

    private Dictionary<string, Dictionary<int, int>> rewardList = new Dictionary<string, Dictionary<int, int>>(6);
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.AddEvent(CEvent.UltimateRankInfo, UpdateUltimateRankInfo);

        mbtn_close.onClick = Close;
        mbtn_battleRank.onClick = OnShowRankClick;
        mbtn_battleRank.parameter = 1;
        mbtn_rankAward.onClick = OnShowRankClick;
        mbtn_rankAward.parameter = 2;

        Net.CSRankInfoMessage();
    }

    public override void Show()
    {
        base.Show();
    }

    private void RefreshRank()
    {
        mrankGrid.Bind<RankInfo, ChallengeRank>(_RankInfo.info, mPoolHandleManager);
        if (mrankGrid.MaxCount > 0)
        {
            GameObject last = mrankGrid.controlList[mrankGrid.MaxCount - 1];
            last.transform.Find("line").gameObject.SetActive(false);
        }

        bool isRank = false;
        for (var i = 0; i < _RankInfo.info.Count; i++)
        {
            if (_RankInfo.info[i].roleId == CSMainPlayerInfo.Instance.ID)
            {
                mlb_myRank.text = _RankInfo.info[i].rank.ToString();
                isRank = true;
                break;
            }
        }

        if (!isRank)
            mlb_myRank.text = CSString.Format(741);
        mlb_myLevel.text = CSUltimateInfo.Instance.UltimateData.maxInstLevel.ToString();

        mbtn_next.SetActive(mrankScrollView.shouldMoveVertically);
    }

    private void RefreshAward()
    {
        if (mawardGrid.MaxCount > 0) return;
        rewardList.Clear();
        RankAwardsTableManager.Instance.GetRankAwardTable(2, rewardList);
        if (rewardList == null || rewardList.Count == 0) return;

        mawardGrid.MaxCount = rewardList.Count;
        int index = 0;
        for (var it = rewardList.GetEnumerator(); it.MoveNext();)
        {
            ChallengeAward award = mPoolHandleManager.GetCustomClass<ChallengeAward>();
            award.Init(mawardGrid.controlList[index].transform, it.Current.Key, it.Current.Value);
            if (index == rewardList.Count - 1)
            {
                award.HideLine();
            }
            rankTabList.Add(award);
            index++;
        }

        mbtn_next.SetActive(mawardScrollView.shouldMoveVertically);

    }

    private void OnShowRankClick(GameObject go)
    {
        int index = (int) UIEventListener.Get(go).parameter;
        if (index == 1)
        {
            mrank.SetActive(true);
            maward.SetActive(false);
            RefreshRank();
        }
        else
        {
            mrank.SetActive(false);
            maward.SetActive(true);
            RefreshAward();
        }
    }


    private void UpdateUltimateRankInfo(uint id, object data)
    {
        _RankInfo = CSUltimateInfo.Instance.RankInfo;
        OnShowRankClick(mbtn_battleRank.gameObject);
    }

    protected override void OnDestroy()
    {
        mrankGrid.UnBind<ChallengeRank>();
        if (rankTabList != null && rankTabList.Count > 0)
        {
            for (var i = 0; i < rankTabList.Count; i++)
            {
                rankTabList[i].Dispose();
            }

            rankTabList.Clear();
        }

        rankTabList = null;
        rewardList.Clear();
        rewardList = null;

        base.OnDestroy();
    }
}

public class ChallengeRank : UIBinder
{
    private UISprite spRank;
    private UILabel lbRank;
    private UILabel lbName;
    private UILabel lbLevel;
    private GameObject obLine;

    private RankInfo _data;

    public override void Init(UIEventListener handle)
    {
        spRank = Get<UISprite>("sp_rank");
        lbRank = Get<UILabel>("lb_rank");
        lbName = Get<UILabel>("lb_name");
        lbLevel = Get<UILabel>("lb_level");
        obLine = Get<GameObject>("line");
    }

    public override void Bind(object data)
    {
        _data = data as RankInfo;
        if (_data == null) return;

        if (_data.rank <= 3)
        {
            spRank.spriteName = "rank" + _data.rank;
            spRank.gameObject.SetActive(true);
            lbRank.gameObject.SetActive(false);
        }
        else
        {
            lbRank.text = _data.rank.ToString();
            spRank.gameObject.SetActive(false);
            lbRank.gameObject.SetActive(true);
        }

        lbName.text = _data.name;

        lbLevel.text = CSString.Format(701, _data.data);
    }

    public override void OnDestroy()
    {
    }
}


public class ChallengeAward : IDispose
{
    private UISprite spRank;
    private UILabel lbRank;
    private GameObject obLine;
    private UIGrid itemGrid;
    private List<UIItemBase> itemList;

    public void Init(Transform go, string name, Dictionary<int, int> awardDic)
    {
        spRank = go.Find("sp_rank").GetComponent<UISprite>();
        lbRank = go.Find("lb_rank").GetComponent<UILabel>();
        obLine = go.Find("line").gameObject;
        itemGrid = go.Find("Scroll View/item").GetComponent<UIGrid>();
        obLine.CustomActive(true);
        Refresh(name, awardDic);
    }

    public void HideLine()
    {
        obLine.SetActive(false);
    }

    private void Refresh(string name, Dictionary<int, int> awardDic)
    {
        if (name == "1" || name == "2" || name == "3")
        {
            spRank.gameObject.SetActive(true);
            spRank.spriteName = "rank" + name;
            lbRank.gameObject.SetActive(false);
        }
        else
        {
            spRank.gameObject.SetActive(false);
            lbRank.gameObject.SetActive(true);
            lbRank.text = name;
        }

        itemList = UIItemManager.Instance.GetUIItems(awardDic.Count, PropItemType.Normal, itemGrid.transform, itemSize.Size50);
        int index = 0;
        for (var it = awardDic.GetEnumerator(); it.MoveNext();)
        {
            itemList[index].ListenDrag();
            itemList[index].Refresh(it.Current.Key);
            itemList[index].SetCount(it.Current.Value);
            index++;
        }

        if (itemGrid != null)
            itemGrid.Reposition();
    }

    public void Dispose()
    {
        if (itemList != null)
            UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        spRank = null;
        lbRank = null;
        obLine = null;
        itemGrid = null;
    }
}