using activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIServerActivityBossKillPanel : UIBasePanel
{
    BossFirstKillDatasResponse bossData;
    BossFirstKillShowItem curBossitem;

    FastArrayElementFromPool<BossFirstKillShowItem> itemList;
    List<UIItemBase> serverRewards;
    List<UIItemBase> personalRewards;
    int sealAcId = 10104;
    SpecialActivityData rewardmes;
    Schedule schedule;
    long leftTime = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.SCBossFirstKillDatasMessage, GetExclusiveData);
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, RewardChangeBack);
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bgTex, "bosskill_bg");
        itemList = mPoolHandleManager.CreateGeneratePool<BossFirstKillShowItem>();
        serverRewards = new List<UIItemBase>();
        personalRewards = new List<UIItemBase>();
        rewardmes = CSOpenServerACInfo.Instance.GetBossFirstKillRewardData();
        bossData = CSOpenServerACInfo.Instance.GetBossData();
        if (bossData != null)
        {
            itemList.Count = bossData.bossFirstKillData.Count;
            mgrid_bossItem.MaxCount = bossData.bossFirstKillData.Count;
            for (int i = 0; i < mgrid_bossItem.MaxCount; i++)
            {
                if (i % 2 == 0)
                {
                    mgrid_bossItem.controlList[i].transform.localPosition = new Vector3(mgrid_bossItem.controlList[i].transform.localPosition.x, -51, 0);
                }
                itemList[i].Init(mgrid_bossItem.controlList[i], BossItemClick);
            }
            UIEventListener.Get(mbtn_server.gameObject).onClick = ServerBtnClick;
            UIEventListener.Get(mbtn_personal.gameObject).onClick = PersonalBtnClick;
            leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
            if (!Timer.Instance.IsInvoking(schedule))
            {
                schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
            }
            mscr_bossItem.ScrollImmidate(0);
            mscr_bossItem.ResetPosition();
        }
    }

    public override void Show()
    {
        base.Show();
        bossData = CSOpenServerACInfo.Instance.GetBossData();
        if (bossData != null)
        {
            RefreshBossItemsState();
            ChooseItem();
        }
    }
    void ChooseItem()
    {
        int canKillId = 0;
        for (int i = 0; i < bossData.bossFirstKillData.Count; i++)
        {
            if (bossData.bossFirstKillData[i].service == 1 || bossData.bossFirstKillData[i].personal == 1)
            {
                BossItemClick(itemList[i]);
                return;
            }
            if (bossData.bossFirstKillData[i].service == 0 || bossData.bossFirstKillData[i].personal == 0)
            {
                canKillId = i;
                break;
            }
        }
        BossItemClick(itemList[canKillId]);
    }
    public override void OnShow(int typeId = 0)
    {
        leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
        if (!Timer.Instance.IsInvoking(schedule))
        {
            schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
        }
    }
    public override void OnHide()
    {
        Timer.Instance.CancelInvoke(schedule);
        base.OnHide();
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bgTex);
        Timer.Instance.CancelInvoke(schedule);
        for (int i = 0; i < serverRewards.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(serverRewards[i]);
        }
        for (int i = 0; i < personalRewards.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(personalRewards[i]);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].Recycle();
        }
        base.OnDestroy();
    }
    void GetExclusiveData(uint id, object data)
    {

    }
    bool isInit = true;
    void RewardChangeBack(uint id, object data)
    {
        if (isInit)
        {
            isInit = false;
            bossData = CSOpenServerACInfo.Instance.GetBossData();
            if (bossData != null)
            {
                itemList.Count = bossData.bossFirstKillData.Count;
                mgrid_bossItem.MaxCount = bossData.bossFirstKillData.Count;
                for (int i = 0; i < mgrid_bossItem.MaxCount; i++)
                {
                    if (i % 2 == 0)
                    {
                        mgrid_bossItem.controlList[i].transform.localPosition = new Vector3(mgrid_bossItem.controlList[i].transform.localPosition.x, -51, 0);
                    }
                    itemList[i].Init(mgrid_bossItem.controlList[i], BossItemClick);
                }
                UIEventListener.Get(mbtn_server.gameObject).onClick = ServerBtnClick;
                UIEventListener.Get(mbtn_personal.gameObject).onClick = PersonalBtnClick;
                leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
                if (!Timer.Instance.IsInvoking(schedule))
                {
                    schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
                }
                mscr_bossItem.ScrollImmidate(0);
                mscr_bossItem.ResetPosition();

                RefreshBossItemsState();
                ChooseItem();
            }
        }

        SpecialActivityData mes = (SpecialActivityData)data;
        if (mes.activityId != sealAcId)
        {
            return;
        }
        rewardmes = mes;
        bossData = CSOpenServerACInfo.Instance.GetBossData();
        for (int i = 0; i < bossData.bossFirstKillData.Count; i++)
        {
            var aa = bossData.bossFirstKillData[i];
        }
        RefreshBossItemsState();
        BossItemClick(curBossitem);
    }
    void CountDown(Schedule _schedule)
    {
        mlb_countdown.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));
        leftTime--;
    }
    void BossItemClick(BossFirstKillShowItem _item)
    {
        if (_item == null)
        {
            return;
        }
        if (curBossitem != null)
        {
            curBossitem.ChangeSelectState(false);
        }
        curBossitem = _item;
        curBossitem.ChangeSelectState(true);
        ReFreshReward();
    }
    void RefreshBossItemsState()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].Refresh(bossData.bossFirstKillData[i]);
        }
        //leftTime = (bossData.time - CSServerTime.DateTimeToStampForMilli(CSServerTime.Now)) / 1000;
    }
    void ReFreshReward()
    {
        string str_server = ClientTipsTableManager.Instance.GetClientTipsContext(1894);
        //全服
        int s_rewardId = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(curBossitem.data.serviceId);
        List<int> SRewardIds = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxItem(s_rewardId), '&');
        List<int> SRewardNums = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxNum(s_rewardId), '&');
        int gap1 = SRewardIds.Count - serverRewards.Count;
        if (serverRewards.Count < SRewardIds.Count)
        {
            for (int i = 0; i < gap1; i++)
            {
                serverRewards.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_serverReward.transform, itemSize.Size64));
            }
        }
        mgrid_serverReward.Reposition();
        for (int i = 0; i < serverRewards.Count; i++)
        {
            if (i >= SRewardIds.Count)
            {
                serverRewards[i].obj.SetActive(false);
            }
            else
            {
                serverRewards[i].obj.SetActive(true);
                serverRewards[i].Refresh(SRewardIds[i], null, true, false);
                serverRewards[i].SetCount(SRewardNums[i]);
            }
        }
        if (curBossitem.data.service == 0)
        {
            mbtn_server.gameObject.SetActive(true);
            mbtn_server.spriteName = "btn_samll1";
            mlb_serverDes.gameObject.SetActive(false);
            mlb_server.text = ClientTipsTableManager.Instance.GetClientTipsContext(1892);//"前往击杀"
            mlb_server.color = UtilityColor.HexToColor("#b0bbcf");
        }
        else if (curBossitem.data.service == 1)
        {
            mbtn_server.gameObject.SetActive(true);
            mbtn_server.spriteName = "btn_samll2";
            mlb_serverDes.gameObject.SetActive(false);
            mlb_server.text = ClientTipsTableManager.Instance.GetClientTipsContext(1893);//"领取"
            mlb_server.color = UtilityColor.HexToColor("#cfbfb0");
        }
        else
        {
            mbtn_server.gameObject.SetActive(false);
            mlb_serverDes.gameObject.SetActive(true);
            mlb_serverDes.text = string.Format(str_server, curBossitem.data.killName);
        }
        //个人
        int p_rewardId = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(curBossitem.data.personalId);
        List<int> PRewardIds = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxItem(p_rewardId), '&');
        List<int> PRewardNums = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxNum(p_rewardId), '&');
        if (personalRewards.Count < PRewardIds.Count)
        {
            personalRewards.AddRange(UIItemManager.Instance.GetUIItems((PRewardIds.Count - personalRewards.Count), PropItemType.Normal, mgrid_personalReward.transform, itemSize.Size64));
        }
        mgrid_personalReward.Reposition();
        for (int i = 0; i < personalRewards.Count; i++)
        {
            if (i >= PRewardIds.Count)
            {
                personalRewards[i].obj.SetActive(false);
            }
            else
            {
                personalRewards[i].obj.SetActive(true);
                personalRewards[i].Refresh(PRewardIds[i], null, true, false);
                personalRewards[i].SetCount(PRewardNums[i]);
            }
        }
        if (curBossitem.data.personal == 0)
        {
            mbtn_personal.gameObject.SetActive(true);
            mbtn_personal.spriteName = "btn_samll1";
            mlb_personalDes.gameObject.SetActive(false);
            mlb_personal.text = ClientTipsTableManager.Instance.GetClientTipsContext(1892);//"前往击杀"
            mlb_personal.color = UtilityColor.HexToColor("#b0bbcf");
            mobj_personalLingqu.SetActive(false);
        }
        else if (curBossitem.data.personal == 1)
        {
            mbtn_personal.gameObject.SetActive(true);
            mbtn_personal.spriteName = "btn_samll2";
            mlb_personalDes.gameObject.SetActive(false);
            mlb_personal.text = ClientTipsTableManager.Instance.GetClientTipsContext(1893);//"领取"
            mlb_personal.color = UtilityColor.HexToColor("#cfbfb0");
            mobj_personalLingqu.SetActive(false);
        }
        else
        {
            mbtn_personal.gameObject.SetActive(false);
            mbtn_personal.spriteName = "btn_samll2";
            mlb_personalDes.gameObject.SetActive(false);
            mobj_personalLingqu.SetActive(true);
        }
    }
    void ServerBtnClick(GameObject _go)
    {
        if (curBossitem.data.service == 0)
        {
            UIManager.Instance.CreatePanel<UIBossCombinePanel>(p =>
            {
                (p as UIBossCombinePanel).SelectChildPanel();
            });
            UIManager.Instance.ClosePanel<UIServerActivityPanel>();
        }
        else if (curBossitem.data.service == 1)
        {
            Net.ReqSpecialActivityRewardMessage(sealAcId, curBossitem.data.serviceId);
        }
        else
        {

        }
    }
    void PersonalBtnClick(GameObject _go)
    {
        if (curBossitem.data.personal == 0)
        {
            UIManager.Instance.CreatePanel<UIBossCombinePanel>(p =>
            {
                (p as UIBossCombinePanel).SelectChildPanel();
            });
            UIManager.Instance.ClosePanel<UIServerActivityPanel>();
        }
        else if (curBossitem.data.personal == 1)
        {
            Net.ReqSpecialActivityRewardMessage(sealAcId, curBossitem.data.personalId);
        }
        else
        {

        }
    }
}
public class BossFirstKillShowItem
{
    public GameObject go;
    public UISprite icon;
    public UILabel name;
    public GameObject canKill;
    public GameObject killed;
    public GameObject select;
    public GameObject redpoint;
    public Action<BossFirstKillShowItem> action;
    public BossFirstKillData data;
    public int showType = 0;
    public BossFirstKillShowItem()
    {

    }

    public void Init(GameObject _go, Action<BossFirstKillShowItem> _action)
    {
        go = _go;
        icon = go.transform.Find("icon").GetComponent<UISprite>();
        name = go.transform.Find("lb_level").GetComponent<UILabel>();
        canKill = go.transform.Find("cankill").gameObject;
        killed = go.transform.Find("killed").gameObject;
        select = go.transform.Find("select").gameObject;
        redpoint = go.transform.Find("redpoint").gameObject;
        action = _action;
        UIEventListener.Get(go).onClick = Click;
        CSEffectPlayMgr.Instance.ShowUIEffect(select, 17506);
    }
    public void Refresh(BossFirstKillData _data)
    {
        data = _data;
        //Debug.Log($"{data.serviceId}   {data.personalId}   {data.service}    {data.personal}");
        showType = 0;
        if (data.service == 0)//显示可首杀
        {
            showType = 1;
        }
        if (data.service == 1 || data.personal == 1)//可领奖
        {
            showType = 2;
        }
        if ((data.service == 2 || data.service == 3) && data.personal == 2)//己全部领完，显示已击杀
        {
            showType = 3;
        }
        canKill.SetActive(showType == 1);
        redpoint.SetActive(showType == 2);
        killed.SetActive(showType == 3);
        int iconName = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardGoalId(data.serviceId);
        name.text = MonsterInfoTableManager.Instance.GetMonsterInfoName(iconName);
        icon.spriteName = MonsterInfoTableManager.Instance.GetMonsterInfoHead(iconName).ToString();
    }
    void Click(GameObject _go)
    {
        if (action != null) { action(this); }
    }
    public void ChangeSelectState(bool _state)
    {
        select.SetActive(_state);
    }
    public void UnInit()
    {
        showType = 0;
        icon = null;
        name = null;
        canKill = null;
        killed = null;
        select = null;
        go = null;
    }
    public void Recycle()
    {
        CSEffectPlayMgr.Instance.Recycle(select);
    }
}
