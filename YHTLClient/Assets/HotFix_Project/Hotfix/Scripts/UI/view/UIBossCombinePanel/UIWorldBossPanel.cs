using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIWolrdBossPanel : UIBasePanel
{

    const int worldBossNpc = 101;
    List<WorldBossRewardItem> rewardList = new List<WorldBossRewardItem>();
    public override void Init()
    {
        base.Init();

        mClientEvent.Reg((uint)CEvent.SCWorldBossActivityInfoResponseMessage, GetDamage);
        ShowLimit();
        UIEventListener.Get(mbtn_enter).onClick = OnClick;
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        mlb_notOpen.text = ClientTipsTableManager.Instance.GetClientTipsContext(1264);
        mlb_activityEnd.text = ClientTipsTableManager.Instance.GetClientTipsContext(1265);
    }

    public override void Show()
    {
        base.Show();
        ShowReward();
        Net.CSGetWorldBossActivityInfoMessage();
    }
    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
        ShowModel();
    }
    public override void OnHide()
    {
        base.OnHide();
        //CSEffectPlayMgr.Instance.Recycle(mobj_model.gameObject);
        //CSEffectPlayMgr.Instance.Recycle(mobj_bg);
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_model.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mobj_bg);
        base.OnDestroy();
    }
    void ShowModel()
    {
        mobj_scrollBar.onChange.Add(new EventDelegate(OnChange));
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_bg, "worldboss_bg");
        TABLE.MONSTERINFO bossCfg = MonsterInfoTableManager.Instance.GetWorldBossCfg();
        if (bossCfg != null)
        {
            mlb_bossName.text = bossCfg.name;
            mlb_bossName.color = UtilityCsColor.Instance.GetColor(bossCfg.quality);
        }
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_model.gameObject, $"{MonsterInfoTableManager.Instance.GetWorldBossModel()}_Stand_4", ResourceType.MonsterAtlas, 5);
        mlb_des.SetupLink();
    }
    string npcId = "";

    string NpcId
    {
        get
        {
            npcId = $"[url=func:{(int)CSLinkFunction.CSLF_WORLDBOSS}:worldboss:{worldBossNpc}]" +
                $"{NpcTableManager.Instance.GetNpcName(worldBossNpc)}({NpcTableManager.Instance.GetNpcBornX(worldBossNpc)}," +
                $"{NpcTableManager.Instance.GetNpcBornY(worldBossNpc)})[/url]";
            return npcId;
        }
    }

    void ShowLimit()
    {
        mlb_time.text = TimerTableManager.Instance.GetTimerDesc(8);
        mlb_activityEnd.text = TimerTableManager.Instance.GetTimerDesc(8);
        mlb_activityEnd.ResizeCollider();
        mlb_lv.text = $"{InstanceTableManager.Instance.GetWorldBossOpenLevel().ToString()}级";
        mlb_lv.color = (InstanceTableManager.Instance.GetWorldBossOpenLevel() > CSMainPlayerInfo.Instance.Level) ? CSColor.red : CSColor.green;
        mlb_des.text = string.Format(SundryTableManager.Instance.GetSundryEffect(379), NpcId);
    }
    Dictionary<string, CSBetterLisHot<TABLE.RANKAWARDS>> dataDic = new Dictionary<string, CSBetterLisHot<TABLE.RANKAWARDS>>();
    bool isshowReward = false;
    void ShowReward()
    {
        if (isshowReward)
        {
            return;
        }
        mpanel_reward.alpha = 0;
        isshowReward = true;
        RankAwardsTableManager.Instance.GetWorldBossReward(1, dataDic);
        mgrid_itemsPar.MaxCount = dataDic.Count;
        var iter = dataDic.GetEnumerator();
        int i = 0;
        while (iter.MoveNext())
        {
            rewardList.Add(new WorldBossRewardItem(mgrid_itemsPar.controlList[i], iter.Current.Value));

            i++;
        }
        ScriptBinder.StartCoroutine(DelayShow());
    }
    IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(0.04f);
        for (int i = 0; i < rewardList.Count; i++)
        {
            rewardList[i].ResetPos();
        }
        yield return new WaitForSeconds(0.04f);
        mscr_reward.ResetPosition();
        mpanel_reward.alpha = 1f;
    }
    void GetDamage(uint id, object data)
    {
        if (data == null) { return; }
        worldboss.ActivityInfo msg = (worldboss.ActivityInfo)data;
        FNDebug.Log("活动状态 1 未开启 2 进行中 3 已结结束   " + msg.activityStatus);
        //活动状态 1 未开启 2 进行中 3 已结结束
        mlb_notOpen.gameObject.SetActive(msg.activityStatus == 1);
        mbtn_enter.SetActive(msg.activityStatus == 2);
        mlb_activityEnd.gameObject.SetActive(msg.activityStatus == 3);
    }
    void OnClick(GameObject _go)
    {
        Net.ReqEnterInstanceMessage(InstanceTableManager.Instance.GetWorldBossOpenId());
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WORLD_BOSS);
    }
    void OnChange()
    {
        if (mobj_scrollBar.value >= 0.95)
        {
            mobj_arrow.SetActive(false);
        }
        else
        {
            mobj_arrow.SetActive(true);
        }
    }
}
public class WorldBossRewardItem
{
    GameObject go;
    UISprite sp_rankNum;
    UILabel lb_rankNum;
    UIGridContainer rewards;
    UIGrid GridReward;
    List<UIItemBase> itemList = new List<UIItemBase>();
    CSBetterLisHot<TABLE.RANKAWARDS> data;
    public WorldBossRewardItem(GameObject _go, CSBetterLisHot<TABLE.RANKAWARDS> _data)
    {
        go = _go;
        data = _data;
        lb_rankNum = go.transform.Find("Label").GetComponent<UILabel>();
        sp_rankNum = go.transform.Find("icon").GetComponent<UISprite>();
        rewards = go.transform.Find("rewards").GetComponent<UIGridContainer>();
        GridReward = go.transform.Find("rewards").GetComponent<UIGrid>();
        if (data[0].rank == 0)
        {
            lb_rankNum.text = ClientTipsTableManager.Instance.GetClientTipsContext(1263);
            lb_rankNum.color = CSColor.beige;
            sp_rankNum.gameObject.SetActive(false);
        }
        else if (data[0].rank <= 3)
        {
            sp_rankNum.gameObject.SetActive(true);
            sp_rankNum.spriteName = $"rank{data[0].rank}";
            lb_rankNum.text = "";
        }
        else
        {
            lb_rankNum.text = $"{data[0].rank}~{data[data.Count - 1].rank}";
            lb_rankNum.color = CSColor.beige;
            sp_rankNum.gameObject.SetActive(false);
        }
        List<List<int>> datalist = UtilityMainMath.SplitStringToIntLists(data[0].awards);
        itemList = UIItemManager.Instance.GetUIItems(datalist.Count, PropItemType.Normal, GridReward.transform, itemSize.Size54);
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].Refresh(datalist[i][0]);
            itemList[i].SetCount(datalist[i][1]);
        }

    }
    public void ResetPos()
    {
        GridReward.Reposition();
    }
    public void Recycle()
    {
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                UIItemManager.Instance.RecycleSingleItem(itemList[i]);
            }
        }
    }
}

