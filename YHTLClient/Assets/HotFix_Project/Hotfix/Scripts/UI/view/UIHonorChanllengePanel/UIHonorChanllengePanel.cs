using instance;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHonorChanllengePanel : UIBasePanel
{
    List<HonorShowBoss> bossList;
    Dictionary<int, rank.RankInfo> rankDic;
    BossChallengeInfo mes;
    List<UIItemBase> itemBaseList;
    string[] groupDes;
    int xinshouRankType = 5;
    int jinjieRankType = 6;
    int dashiRankType = 7;
    int myGroupId = 0;
    int startId = 0;
    int curInstanceId = 0;
    string seasonKey = "";
    int stockPileData = 0;
    public override void Init()
    {
        base.Init();
        AddCollider();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "boss_challenge_bg");
        //��������
        mClientEvent.AddEvent(CEvent.SCBossChallengeMessage, GetReponseBack);
        mClientEvent.AddEvent(CEvent.RankInfoChange, GetRankReponseBack);
        Net.CSBossChallengeMessage();
        Net.ReqRankInfoMessage(5);
        Net.ReqRankInfoMessage(6);
        Net.ReqRankInfoMessage(7);
        bossList = mPoolHandleManager.GetSystemClass<List<HonorShowBoss>>();
        rankDic = mPoolHandleManager.GetSystemClass<Dictionary<int, rank.RankInfo>>();
        itemBaseList = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();
        groupDes = ClientTipsTableManager.Instance.GetClientTipsContext(1118).Split('#');
        for (int i = 0; i < mtran_bossPar.childCount; i++)
        {
            bossList.Add(new HonorShowBoss());
            bossList[i].Init(mtran_bossPar.GetChild(i).gameObject);
        }
        UIEventListener.Get(mobj_moreRankMes).onClick = ShowMoreRank;
        UIEventListener.Get(mobj_rankReward).onClick = ShowRankReward;
        UIEventListener.Get(mbtn_enter).onClick = EnterBtnClick;
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        seasonKey = $"{CSMainPlayerInfo.Instance.Name}HonorChanllengeGroup";
        if (PlayerPrefs.HasKey(seasonKey))
        {
            stockPileData = PlayerPrefs.GetInt(seasonKey);
        }
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_eff, 17057);
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < itemBaseList.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(itemBaseList[i]);
        }
        CSEffectPlayMgr.Instance.Recycle(mobj_eff);
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        for (int i = 0; i < bossList.Count; i++)
        {
            bossList[i].UnInit();
        }
        base.OnDestroy();
    }
    void GetReponseBack(uint id, object data)
    {
        mes = (BossChallengeInfo)data;
        if (mes.group > 1 && stockPileData != mes.group)
        {
            UIManager.Instance.CreatePanel<UIHonorPromotionPromptPanel>(p =>
            {
                (p as UIHonorPromotionPromptPanel).SetData(mes.group - 1);
            });
        }
        PlayerPrefs.SetInt(seasonKey, mes.group);
        RefreshPanel();
    }
    void GetRankReponseBack(uint id, object data)
    {
        rank.RankInfo msg = (rank.RankInfo)data;
        if (msg.type != xinshouRankType && msg.type != jinjieRankType && msg.type != dashiRankType)
        {
            return;
        }
        if (rankDic.ContainsKey(msg.type))
        {
            rankDic.Add(msg.type, msg);
        }
        else
        {
            rankDic[msg.type] = msg;
        }
        if (rankDic.Count == 3)
        {
            RefreshRank();
        }
    }
    int passId = 0;
    int maxLv = 0;
    void RefreshPanel()
    {
        myGroupId = mes.group;
        passId = mes.pass + 1;
        maxLv = InstanceTableManager.Instance.GetHonorChanllengeMaxLevel();

        mbtn_enter.SetActive(passId <= maxLv);
        mobj_max.SetActive(passId > maxLv);

        passId = (passId > maxLv) ? maxLv : passId;
        int totalNum = InstanceTableManager.Instance.GetInstanceCountByType(20);
        startId = (totalNum - mes.pass >= 3) ? passId : (totalNum - 3);
        curInstanceId = InstanceTableManager.Instance.GetInstanceIdByType(20, passId);
        mlb_leftTime.text = CSServerTime.Instance.FormatLongToTimeStr(mes.surplusTime / 1000, 10);

        //ʱ�䳬��һ����ʾ��ɫ  ������ʾ��ɫ
        if (mes.surplusTime > 86400000)
        {
            mlb_leftTime.color = CSColor.green;
        }
        else
        {
            mlb_leftTime.color = CSColor.red;
        }
        //Debug.Log($" {mes.group}   {startId}   {mes.pass}   {mes.surplusTime}");
        for (int i = 0; i < bossList.Count; i++)
        {
            bossList[i].Refresh(startId + i);
            if ((startId + i) == passId)
            {
                bossList[i].SetFightEffect(mobj_eff);
            }
        }
        ShowKillReward();
    }
    void RefreshRank()
    {
        if (myGroupId == 0)
        {
            return;
        }
        mlb_rankTitle.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1119), groupDes[myGroupId - 1]);
        if (rankDic[myGroupId + 4].myRank < 0)
        {
            mlb_myRank.text = ClientTipsTableManager.Instance.GetClientTipsContext(1867);
        }
        else
        {
            mlb_myRank.text = $"[00ff00]{rankDic[myGroupId + 4].myRank + 1}";
        }
        for (int i = 0; i < 3; i++)
        {
            Transform temp_go = mtran_rankPar.transform.GetChild(i);
            UILabel name = temp_go.Find("lb_name").GetComponent<UILabel>();
            UILabel lv = temp_go.Find("lb_level").GetComponent<UILabel>();
            if (i >= rankDic[myGroupId + 4].ranks.Count)
            {
                name.text = ClientTipsTableManager.Instance.GetClientTipsContext(1869);
                name.color = UtilityColor.HexToColor($"#b8a586");
                lv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1123),0);
            }
            else
            {
                name.color = UtilityColor.HexToColor($"#eee5c3");
                name.text = rankDic[myGroupId + 4].ranks[i].name;
                lv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1123), rankDic[myGroupId + 4].ranks[i].value);
            }
        }
    }
    void ShowKillReward()
    {
        mlb_killDes.text = "";

        List<List<int>> reward = UtilityMainMath.SplitStringToIntLists(InstanceTableManager.Instance.GetInstanceShow(curInstanceId));
        //Debug.Log(curInstanceId + "   " + InstanceTableManager.Instance.GetInstanceShow(curInstanceId)+"   "+reward.Count);
        itemBaseList = UIItemManager.Instance.GetUIItems(reward.Count, PropItemType.Normal, mgrid_reward.transform, itemSize.Size60);
        for (int i = 0; i < itemBaseList.Count; i++)
        {
            itemBaseList[i].Refresh(reward[i][0]);
            itemBaseList[i].SetCount(reward[i][1]);
        }
        mgrid_reward.Reposition();
    }
    void ShowMoreRank(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIHonorChanllengeRankPanel>(p =>
        {
            (p as UIHonorChanllengeRankPanel).SetShowType(0, mes, rankDic);
        });
    }
    void ShowRankReward(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIHonorChanllengeRankPanel>(p =>
        {
            (p as UIHonorChanllengeRankPanel).SetShowType(1, mes, rankDic);
        });
    }
    void EnterBtnClick(GameObject _go)
    {
        //Debug.Log(curInstanceId);
        Net.ReqEnterInstanceMessage(curInstanceId);
        UIManager.Instance.ClosePanel<UIHonorChanllengeCombinePanel>();
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HonorChanllenge);
    }
}
public class HonorShowBoss
{
    public GameObject go;
    public UILabel name;
    public UILabel floorNum;
    public GameObject model;
    CSResource res;
    Vector3 effPos = new Vector3(-1, -29, 0);
    public void Init(GameObject _go)
    {
        go = _go;
        name = go.transform.Find("lb_name").GetComponent<UILabel>();
        floorNum = go.transform.Find("lb_level").GetComponent<UILabel>();
        model = go.transform.Find("sp_monster").gameObject;
    }
    public void Refresh(int _id)
    {
        floorNum.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1123), _id);
        int curInstanceId = InstanceTableManager.Instance.GetInstanceIdByType(20, _id);
        int monsterId = InstanceTableManager.Instance.GetInstanceParam(curInstanceId);
        name.text = MonsterInfoTableManager.Instance.GetMonsterInfoName(monsterId);
        name.color = UtilityCsColor.Instance.GetColor(MonsterInfoTableManager.Instance.GetMonsterInfoQuality(monsterId));
        int model = MonsterInfoTableManager.Instance.GetMonsterInfoModel(monsterId);
        res = CSResourceManager.Instance.AddQueue($"{model}_Stand_4", ResourceType.MonsterAtlas, OnLoadResources, ResourceAssistType.UI);
        res.IsCanBeDelete = false;
    }
    public void SetFightEffect(GameObject _go)
    {
        _go.transform.SetParent(go.transform);
        _go.transform.localPosition = effPos;
    }
    private void OnLoadResources(CSResource res)
    {
        if (this == null) return;
        if (res == null || res.MirrorObj == null) return;
        GameObject go = res.MirrorObj as GameObject;
        UISpriteAnimation ShowTex = model.GetComponent<UISpriteAnimation>();
        UISprite sprite = model.GetComponent<UISprite>();
        if (sprite == null) return;
        if (ShowTex == null) return;
        ShowTex.framesPerSecond = 6;
        if (go != null)
        {
            UIAtlas atlas = go.GetComponent<UIAtlas>();
            if (atlas)
            {
                sprite.atlas = atlas;
                if (ShowTex && ShowTex.AnimSprite)
                {
                    if (ShowTex != null)
                    {
                        ShowTex.RebuildSpriteList();
                        ShowTex.ResetToBeginning();
                    }
                }
            }
        }
    }
    public void UnInit()
    {
        if (res != null)
        {
            res.IsCanBeDelete = true;
        }
        go = null;
        name = null;
        floorNum = null;
        model = null;
    }

}
